namespace NoPlan.Infrastructure;

public sealed class RoleDefinitions
{
    public const string AppConfigurationDataOwner = nameof(AppConfigurationDataOwner);
    public const string AppConfigurationDataReader = nameof(AppConfigurationDataReader);

    public const string ServiceBusDataOwner = nameof(ServiceBusDataOwner);
    public const string ServiceBusDataReceiver = nameof(ServiceBusDataReceiver);
    public const string ServiceBusDataSender = nameof(ServiceBusDataSender);

    public const string KeyVaultAdministrator = nameof(KeyVaultAdministrator);
    public const string KeyVaultSecretsUser = nameof(KeyVaultSecretsUser);

    public const string AcrPull = nameof(AcrPull);
    public const string AcrPush = nameof(AcrPush);

    private readonly string _subscriptionId;

    private readonly Dictionary<string, string> _mappings = new()
    {
        [AcrPull] = "7f951dda-4ed3-4680-a7ca-43fe172d538d",
        [AcrPush] = "8311e382-0749-4cb8-b61a-304f252e45ec",
        [KeyVaultAdministrator] = "00482a5a-887f-4fb3-b363-3b7fe8e74483",
        [KeyVaultSecretsUser] = "4633458b-17de-408a-b874-0445c86b69e6",
        [AppConfigurationDataOwner] = "5ae67dd6-50cb-40e7-96ff-dc2bfa4b606b",
        [AppConfigurationDataReader] = "516239f1-63e1-4d78-a4de-a74fb236a071",
        [ServiceBusDataOwner] = "090c5cfd-751d-490a-894a-3ce6f1109419",
        [ServiceBusDataReceiver] = "4f6d3b9b-027b-4f4c-9142-0e5a2a2247e0",
        [ServiceBusDataSender] = "69a216fc-b8fb-44d8-bc22-1f3c2cd27a39"
    };

    public RoleDefinitions(string subscriptionId) =>
        _subscriptionId = subscriptionId;

    public string this[string roleName]
    {
        get => GetId(roleName);
        set => Set(roleName, value);
    }

    private string GetId(string roleName)
    {
        if (_mappings.TryGetValue(roleName, out var roleId))
        {
            return $"/subscriptions/{_subscriptionId}/providers/Microsoft.Authorization/roleDefinitions/{roleId}";
        }

        throw new KeyNotFoundException($"Could not find a role id for role name '{roleName}'");
    }

    private void Set(string roleName, string roleId) =>
        _mappings.TryAdd(roleName, roleId);
}
