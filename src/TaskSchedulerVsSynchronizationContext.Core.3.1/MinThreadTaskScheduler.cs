using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSchedulerVsSynchronizationContext;

internal class MinThreadTaskScheduler : TaskScheduler
{
    protected override IEnumerable<Task>? GetScheduledTasks() =>
        Enumerable.Empty<Task>();

    protected override void QueueTask(Task task)
    {
        Console.WriteLine($"QueueTask from MinThreadTaskScheduler in {Thread.CurrentThread.ManagedThreadId}");
        new Thread(() => TryExecuteTask(task)).Start();
    }

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        Console.WriteLine($"TryExecuteTaskInline from MinThreadTaskScheduler in {Thread.CurrentThread.ManagedThreadId}");
        return TryExecuteTask(task);
    }

    // protected override bool TryDequeue(Task task)

    // по умолчанию int.MaxValue
    // public override int MaximumConcurrencyLevel { get; }
}