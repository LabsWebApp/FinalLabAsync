namespace TaskSchedulerVsSynchronizationContext;

internal class SimpleSynchronizationContext : SynchronizationContext
{
    public override void Post(SendOrPostCallback d, object? state)
    {
        WriteLine($"Post from SimpleSynchronizationContext in {Thread.CurrentThread.ManagedThreadId}");
        new Thread(() => d.Invoke(state)).Start();
    }

    public override void Send(SendOrPostCallback d, object? state)
    {
        WriteLine($"Send from SimpleSynchronizationContext in {Thread.CurrentThread.ManagedThreadId}");
        d.Invoke(state);
    }
}