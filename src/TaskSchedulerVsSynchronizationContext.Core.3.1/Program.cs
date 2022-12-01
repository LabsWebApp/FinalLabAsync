using static System.Console;
using System.Threading.Tasks;
using System.Threading;

namespace TaskSchedulerVsSynchronizationContext
{
    internal class Program
    {
        static Program() => SynchronizationContext.SetSynchronizationContext(new SimpleThreadPoolSynchronizationContext());
        
        private static void Work(object? _)
        {
            Thread.Sleep(1000);
            WriteLine($"Method Work is completed in {Thread.CurrentThread.ManagedThreadId}");
        }

        private static async Task WorkAsync(object? _)
        {
            await Task.Delay(1000);
            //await Task.Delay(1000).ConfigureAwait(false);
            WriteLine($"Method WorkAsync is completed in {Thread.CurrentThread.ManagedThreadId}");
        }

        private static MinThreadTaskScheduler? _scheduler;
        private static MinThreadTaskScheduler GetScheduler
        {
            get
            {
                _scheduler ??= new();
                return _scheduler;
            }
        }

        static async Task Main()
        {
            WriteLine($"Method Main is started in {Thread.CurrentThread.ManagedThreadId}");

            Task task = new Task(Work!, null);
            //task.Start();
            //task.Start(GetScheduler);
            //task.Start(TaskScheduler.FromCurrentSynchronizationContext());
            //task.Start(TaskScheduler.Current);
            task = Task.Factory.StartNew(() =>
            {
                WriteLine($"Task \"task\" is started in {Thread.CurrentThread.ManagedThreadId}");
                Task.Run(() => WriteLine($"The nested task is started in {Thread.CurrentThread.ManagedThreadId}"));
                Task.Factory.StartNew(() => WriteLine($"Child task is started in {Thread.CurrentThread.ManagedThreadId}"),
                    TaskCreationOptions.AttachedToParent);
            }, CancellationToken.None, TaskCreationOptions.None, GetScheduler); // HideScheduler
            task.Wait();
            //await WorkAsync(null);
            //await WorkAsync(null).ConfigureAwait(false);

            ReadKey();
        }
    }
}
