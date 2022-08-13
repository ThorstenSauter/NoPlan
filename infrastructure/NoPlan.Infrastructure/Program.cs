using Pulumi;
using Pulumi.AzureNative.App;
using Pulumi.AzureNative.App.Inputs;
using Pulumi.AzureNative.AppConfiguration;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.ContainerRegistry;
using Pulumi.AzureNative.ContainerRegistry.Inputs;
using Pulumi.AzureNative.EventGrid;
using Pulumi.AzureNative.EventGrid.Inputs;
using Pulumi.AzureNative.Insights.V20200202;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;
using Pulumi.AzureNative.ManagedIdentity;
using Pulumi.AzureNative.OperationalInsights;
using Pulumi.AzureNative.OperationalInsights.Inputs;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.ServiceBus;
using Pulumi.AzureNative.ServiceBus.Inputs;
using Pulumi.AzureNative.Sql;
using Pulumi.AzureNative.Sql.Inputs;
using Deployment = Pulumi.Deployment;
using SkuArgs = Pulumi.AzureNative.ContainerRegistry.Inputs.SkuArgs;
using SkuName = Pulumi.AzureNative.KeyVault.SkuName;
using Topic = Pulumi.AzureNative.ServiceBus.Topic;
using UserIdentityPropertiesArgs = Pulumi.AzureNative.EventGrid.Inputs.UserIdentityPropertiesArgs;

return await Deployment.RunAsync(async () =>
{
    var clientConfig = await GetClientConfig.InvokeAsync();
    var stackName = Deployment.Instance.StackName;
    var tags = new InputMap<string> { { "environment", stackName }, { "project", "noplan" }, { "managed-by", "Pulumi" } };

    // Create an Azure Resource Group
    var resourceGroup = new ResourceGroup("resourceGroup", new() { ResourceGroupName = $"rg-noplan-{stackName}-001", Tags = tags },
        new() { Protect = true });

    var userAssignedManagedIdentity = new UserAssignedIdentity("noplan-identity",
        new() { ResourceGroupName = resourceGroup.Name, ResourceName = $"id-noplan-{stackName}-westeurope-001", Tags = tags },
        new() { Protect = true });

    var containerRegistry = new Registry("containerRegistry",
        new()
        {
            AdminUserEnabled = false,
            Policies = new PoliciesArgs
            {
                QuarantinePolicy = new QuarantinePolicyArgs { Status = "disabled" },
                RetentionPolicy = new RetentionPolicyArgs { Days = 7, Status = "disabled" },
                TrustPolicy = new TrustPolicyArgs { Status = "disabled", Type = "Notary" }
            },
            RegistryName = $"crnoplan{stackName}001",
            ResourceGroupName = resourceGroup.Name,
            Sku = new SkuArgs { Name = "Basic" },
            Tags = tags
        }, new() { Protect = true });

    var vault = new Vault("vault",
        new()
        {
            Properties = new VaultPropertiesArgs
            {
                EnableRbacAuthorization = true,
                EnableSoftDelete = true,
                EnabledForDeployment = false,
                EnabledForDiskEncryption = false,
                EnabledForTemplateDeployment = false,
                ProvisioningState = "Succeeded",
                Sku = new Pulumi.AzureNative.KeyVault.Inputs.SkuArgs { Family = "A", Name = SkuName.Standard },
                SoftDeleteRetentionInDays = 90,
                TenantId = clientConfig.TenantId,
                VaultUri = "https://kv-noplan-dev-001.vault.azure.net/"
            },
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            VaultName = $"kv-noplan-{stackName}-001"
        }, new() { Protect = true });

    var integrationTestsVault = new Vault("integrationTestsVault",
        new()
        {
            Location = "westeurope",
            Properties = new VaultPropertiesArgs
            {
                EnableRbacAuthorization = true,
                EnableSoftDelete = true,
                EnabledForDeployment = false,
                EnabledForDiskEncryption = false,
                EnabledForTemplateDeployment = false,
                ProvisioningState = "Succeeded",
                Sku = new Pulumi.AzureNative.KeyVault.Inputs.SkuArgs { Family = "A", Name = SkuName.Standard },
                SoftDeleteRetentionInDays = 90,
                TenantId = clientConfig.TenantId,
                VaultUri = "https://kv-noplan-i-t-001.vault.azure.net/"
            },
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            VaultName = "kv-noplan-i-t-001"
        }, new() { Protect = true });

    var loganalyticsworkspace = new Workspace("loganalyticsworkspace",
        new()
        {
            Features = new WorkspaceFeaturesArgs { EnableLogAccessUsingOnlyResourcePermissions = true },
            ProvisioningState = "Succeeded",
            PublicNetworkAccessForIngestion = "Enabled",
            PublicNetworkAccessForQuery = "Enabled",
            ResourceGroupName = resourceGroup.Name,
            RetentionInDays = 30,
            Sku = new WorkspaceSkuArgs { Name = "pergb2018" },
            Tags = tags,
            WorkspaceCapping = new WorkspaceCappingArgs { DailyQuotaGb = -1 },
            WorkspaceName = $"log-noplan-{stackName}-001"
        }, new() { Protect = true });

    var workspaceSharedKeys = Output.Tuple(resourceGroup.Name, loganalyticsworkspace.Name).Apply(items =>
        GetSharedKeys.InvokeAsync(new() { ResourceGroupName = items.Item1, WorkspaceName = items.Item2 }));

    var appInsights = new Component("appInsights",
        new()
        {
            ApplicationType = "web",
            DisableIpMasking = false,
            IngestionMode = "LogAnalytics",
            Kind = "web",
            Location = "westeurope",
            ResourceGroupName = resourceGroup.Name,
            ResourceName = $"appi-noplan-{stackName}-001",
            RetentionInDays = 90,
            SamplingPercentage = 0,
            WorkspaceResourceId = loganalyticsworkspace.Id,
            Tags = tags
        }, new() { Protect = true });

    var appConfig = new ConfigurationStore("appConfig",
        new()
        {
            ConfigStoreName = $"appcs-noplan-{stackName}",
            ResourceGroupName = resourceGroup.Name,
            Sku = new Pulumi.AzureNative.AppConfiguration.Inputs.SkuArgs { Name = "free" },
            Tags = tags
        }, new() { Protect = true });

    var sqlServer = new Server("sqlServer",
        new()
        {
            AdministratorLogin = "CloudSAda3d682c",
            Administrators = new ServerExternalAdministratorArgs
            {
                AdministratorType = "ActiveDirectory",
                AzureADOnlyAuthentication = true,
                Login = "NoPlan Developers",
                PrincipalType = "Group",
                Sid = "914fff5e-bedf-443b-82ad-c4ceccb192c3",
                TenantId = clientConfig.TenantId
            },
            MinimalTlsVersion = "1.2",
            PublicNetworkAccess = "Enabled",
            ResourceGroupName = resourceGroup.Name,
            ServerName = $"sql-noplan-{stackName}-001",
            Tags = tags,
            Version = "12.0"
        }, new() { Protect = true });

    var noplanDatabase = new Database("noplanDatabase", new()
    {
        CatalogCollation = "SQL_Latin1_General_CP1_CI_AS",
        Collation = "SQL_Latin1_General_CP1_CI_AS",
        DatabaseName = "noplan",
        MaintenanceConfigurationId =
            "/subscriptions/16e01a00-f825-4c96-8ae2-e68cb52cf653/providers/Microsoft.Maintenance/publicMaintenanceConfigurations/SQL_Default",
        MaxSizeBytes = 2147483648,
        ReadScale = "Disabled",
        RequestedBackupStorageRedundancy = "Local",
        ResourceGroupName = resourceGroup.Name,
        ServerName = sqlServer.Name,
        Sku = new Pulumi.AzureNative.Sql.Inputs.SkuArgs { Capacity = 5, Name = "Basic", Tier = "Basic" },
        Tags = tags,
        ZoneRedundant = false
    }, new() { Protect = true });

    var noplanContainerEnvironment = new ManagedEnvironment("noplanContainerEnvironment",
        new()
        {
            AppLogsConfiguration = new AppLogsConfigurationArgs
            {
                Destination = "log-analytics",
                LogAnalyticsConfiguration = new LogAnalyticsConfigurationArgs
                {
                    CustomerId = loganalyticsworkspace.CustomerId, SharedKey = workspaceSharedKeys.Apply(r => r.PrimarySharedKey)!
                }
            },
            EnvironmentName = $"acae-noplan-{stackName}-001",
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            ZoneRedundant = false
        }, new() { Protect = true });

    var servicebus = new Namespace("servicebus",
        new()
        {
            NamespaceName = $"sb-noplan-{stackName}-001",
            ResourceGroupName = resourceGroup.Name,
            Sku = new SBSkuArgs { Name = Pulumi.AzureNative.ServiceBus.SkuName.Standard, Tier = SkuTier.Standard },
            Tags = tags
        }, new() { Protect = true });

    var appconfigChangesTopic = new Topic("appconfigChangesTopic",
        new()
        {
            AutoDeleteOnIdle = "P10675199DT2H48M5.4775807S",
            DefaultMessageTimeToLive = "P14D",
            DuplicateDetectionHistoryTimeWindow = "PT10M",
            EnableBatchedOperations = true,
            EnableExpress = false,
            EnablePartitioning = false,
            MaxSizeInMegabytes = 1024,
            NamespaceName = servicebus.Name,
            RequiresDuplicateDetection = false,
            ResourceGroupName = resourceGroup.Name,
            Status = EntityStatus.Active,
            SupportOrdering = true,
            TopicName = "appconfig-changes"
        }, new() { Protect = true });

    var appconfigChangesTopicSubscription = new Subscription("appconfigChangesTopicSubscription",
        new()
        {
            AutoDeleteOnIdle = "P14D",
            DeadLetteringOnFilterEvaluationExceptions = false,
            DeadLetteringOnMessageExpiration = false,
            DefaultMessageTimeToLive = "P14D",
            EnableBatchedOperations = true,
            LockDuration = "PT30S",
            MaxDeliveryCount = 1,
            NamespaceName = servicebus.Name,
            RequiresSession = false,
            ResourceGroupName = resourceGroup.Name,
            Status = EntityStatus.Active,
            SubscriptionName = "appconfig-changes",
            TopicName = appconfigChangesTopic.Name
        }, new() { Protect = true });

    var appconfigChangesEventGridTopic = new SystemTopic("appconfigChangesEventGridTopic",
        new()
        {
            Identity = new IdentityInfoArgs
            {
                Type = "UserAssigned",
                UserAssignedIdentities =
                {
                    {
                        "/subscriptions/16e01a00-f825-4c96-8ae2-e68cb52cf653/resourceGroups/rg-noplan-dev-001/providers/Microsoft.ManagedIdentity/userAssignedIdentities/id-noplan-dev-westeurope-001",
                        new UserIdentityPropertiesArgs
                        {
                            ClientId = "f0156a97-0693-4efc-aafc-7e192e092db4", PrincipalId = "a8e0d3b6-f769-4c8f-bdcb-9b66693db7b6"
                        }
                    }
                }
            },
            ResourceGroupName = resourceGroup.Name,
            Source = appConfig.Id,
            SystemTopicName = "appconfig-changes",
            TopicType = "Microsoft.AppConfiguration.ConfigurationStores"
        }, new() { Protect = true });
});
