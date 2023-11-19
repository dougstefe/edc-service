namespace Edc.Api.Client;

public class Settings {
    public string BaseUri { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public TimeSpan? Timeout { get; set; } = TimeSpan.FromMilliseconds(25000);
    public int MaximumRetryAttempts { get; set; } = 3;
    public int MaximumConsecutiveFailures { get; set; } = 5;
    public int MaximumCircuitBreakerWaitingTime { get; set; } = 30;
}