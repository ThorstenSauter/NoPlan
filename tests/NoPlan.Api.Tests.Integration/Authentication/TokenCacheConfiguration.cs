using System.Reflection;
using Microsoft.Identity.Client.Extensions.Msal;

namespace NoPlan.Api.Tests.Integration.Authentication;

internal static class TokenCacheConfiguration
{
    public const string KeyChainServiceName = "noplan_api_integration_testing_msal_service";
    public const string KeyChainAccountName = "noplan_api_integration_testing_msal_account";

    public const string LinuxKeyRingSchema = "dev.thorstensauter.noplan.api.integration.testing.tokencache";
    public const string LinuxKeyRingCollection = MsalCacheHelper.LinuxKeyRingDefaultCollection;
    public const string LinuxKeyRingLabel = "MSAL token cache for the NoPlan.API integration tests.";

    public const string CacheFileName = "noplan_api_integration_testing_msal_cache.txt";
    public static readonly string CacheDir = MsalCacheHelper.UserRootDirectory;

    public static readonly KeyValuePair<string, string> LinuxKeyRingAttr1 =
        new("Version", Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1.0.0");

    public static readonly KeyValuePair<string, string> LinuxKeyRingAttr2 = new("ProductGroup", "LIMS.API integration tests");
}
