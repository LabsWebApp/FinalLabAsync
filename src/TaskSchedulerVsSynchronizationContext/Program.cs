using TaskSchedulerVsSynchronizationContext;

SynchronizationContext.SetSynchronizationContext(new SimpleSynchronizationContext());

void Work(object? _)
{
    Thread.Sleep(1000);
    WriteLine($"Method Work is completed in {Thread.CurrentThread.ManagedThreadId}");
}

async Task WorkAsync(object? _)
{
    //await Task.Delay(1000);
    await Task.Delay(1000).ConfigureAwait(false);
    WriteLine($"Method WorkAsync is completed in {Thread.CurrentThread.ManagedThreadId}");
}

MinThreadTaskScheduler scheduler = new MinThreadTaskScheduler();

Task task = new Task(Work!, null);
//task.Start();
//task.Start(scheduler);
//task.Start(TaskScheduler.FromCurrentSynchronizationContext());
//task.Start(TaskScheduler.Current);
//await WorkAsync(null);
await WorkAsync(null).ConfigureAwait(false);

ReadKey();