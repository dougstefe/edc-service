namespace Edc.Core;

public static class Config {
    public static SecretsConfig Secrets { get; set; } = new();
    public static DatabaseConfig Database { get; set; } = new();
    public static EmailConfig Email { get; set; } = new();
    public static SendGridClientConfig SendGridClient { get; set; } = new();
    
    public class DatabaseConfig {
        public string ConnectionString { get; set; } = String.Empty;
    }
    public class SecretsConfig {
        public string ApiSecretKey { get; set; } = String.Empty;
        public string TokenPrivateKey { get; set; } = String.Empty;
        public string PasswordSalt { get; set; } = String.Empty;
    }

    public class EmailConfig {
        public string DefaultFromEmail { get; set; } = String.Empty;
        public string DefaultFromName { get; set; } = String.Empty;
    }

    public class SendGridClientConfig {
        public string ApiKey { get; set; } = String.Empty;
    }
}