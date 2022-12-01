using TaskSchedulerVsSynchronizationContext;

SynchronizationContext.SetSynchronizationContext(new SimpleThreadPoolSynchronizationContext());

void Work(object? _)
{
    Thread.Sleep(1000);
    WriteLine($"Method Work is completed in {Thread.CurrentThread.ManagedThreadId}");
}

async Task WorkAsync(object? _)
{
    await Task.Delay(1000);
    //await Task.Delay(1000).ConfigureAwait(false);
    WriteLine($"Method WorkAsync is completed in {Thread.CurrentThread.ManagedThreadId}");
}

WriteLine($"Method Main is started in {Thread.CurrentThread.ManagedThreadId}");

MinThreadTaskScheduler scheduler = new MinThreadTaskScheduler();

Task task = new Task(Work!, null);
//task.Start();
//task.Start(scheduler);
//task.Start(TaskScheduler.FromCurrentSynchronizationContext());
//task.Start(TaskScheduler.Current);
task = Task.Factory.StartNew(() =>
{
    WriteLine($"Task task is started in {Thread.CurrentThread.ManagedThreadId}");
    Task.Run(() => WriteLine($"The nested task is started in {Thread.CurrentThread.ManagedThreadId}"));
    Task.Factory.StartNew(() => WriteLine($"Child task is started in {Thread.CurrentThread.ManagedThreadId}"),
        TaskCreationOptions.AttachedToParent);
}, CancellationToken.None, TaskCreationOptions.HideScheduler, scheduler);
//task.Wait();
//await WorkAsync(null);
//await WorkAsync(null).ConfigureAwait(false);

ReadKey();