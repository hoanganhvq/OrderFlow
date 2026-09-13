namespace BuildingBlocks.Messaging.Helper;

public static class RetryPolicyHelper
{
    public static TimeSpan CaculateBackoffDelay(int retryCount, double baseSeconds = 2.0, double maxSeconds = 60.0)
    {
        var exponentialDelay = Math.Pow(baseSeconds, retryCount);
        var clampedDelay = Math.Min(exponentialDelay, maxSeconds);
        var jitter  = Random.Shared.NextDouble();
        
        return TimeSpan.FromMilliseconds(jitter + clampedDelay);
    }
}