namespace Domain.OutfaceModels;

public interface IApplicationConfig { }

public class AppSetting : IApplicationConfig
{
    public string ApiVersion { get; set; } = "1.0.0.0";
    public static bool IsDebugMode { get; set; } = false;
    public string ClientId { get; set; } = string.Empty;
    public DbConnection Db { get; set; } = new DbConnection();
    public string IdentityServerUrl { get; set; } = string.Empty;
    public string RMQUrl { get; set; } = string.Empty;
    public string RedisUrl { get; set; } = string.Empty;
    public string DirectDebitRedisUrl { get; set; } = string.Empty;
    public string DirectDebitRedisPassword { get; set; } = string.Empty;

    public bool AutoMigration { get; set; } = false;
    public bool EnableSwagger { get; set; } = true;
    public bool EnableSentry { get; set; } = true;
    public SentrySettings SentrySettings { get; set; } = new SentrySettings();

    public string StaffManagementMonitoringPath { get; set; } = "/v1";

    // Remove banking-specific URLs as this is now a Staff Management API
    public string StaffManagementApiUrl { get; set; } = string.Empty;
    public string S3BucketName { get; set; } = string.Empty;
    public int WorkerStartupDelay { get; set; } = 5;
}

public class SentrySettings
{
    public string Dsn { get; set; } = "";
    public double TracesSampleRate { get; set; }
    public string Environment { get; set; } = "";
    public bool Debug { get; set; } = true;
    public bool IncludeActivityData { get; set; } = true;
    public bool AttachStacktrace { get; set; } = true;
}

public class DbConnection
{
    public string PrimaryConnection { get; set; } = string.Empty;
    public string ReadOnlyConnection { get; set; } = string.Empty;
}

public static class DbOptionConst
{
    public static int DbMaxRetryCount => 10;
    public static TimeSpan DbRetryDelay => TimeSpan.FromSeconds(30);
    public static int DbCommandTimeout => (int)TimeSpan.FromMinutes(10).TotalSeconds;
}
