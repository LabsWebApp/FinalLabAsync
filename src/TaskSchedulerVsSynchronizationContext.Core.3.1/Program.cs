using static System.Console;
using System.Threading.Tasks;
using System.Threading;

namespace TaskSchedulerVsSynchronizationContext
{
    internal class Program
    {
        // static Singleton
        //static Program() => SynchronizationContext.SetSynchronizationContext(new SimpleThreadPoolSynchronizationContext());
        
        private static void Work(object? _)
        {
            Thread.Sleep(1000);
            WriteLine($"Method Work (task №{Task.CurrentId}) is completed in {Thread.CurrentThread.ManagedThreadId}");
        }

        private static async Task WorkAsync(object? _)
        {
            Task task = Task.Delay(1000);
            await task;
            //await task.ConfigureAwait(false);
            WriteLine($"Method WorkAsync (task №{task.Id}) is completed in {Thread.CurrentThread.ManagedThreadId}");
        }

        #region Singleton
        private static MinThreadTaskScheduler? _scheduler;
        private static MinThreadTaskScheduler GetScheduler
        {
            get
            {
                _scheduler ??= new();
                return _scheduler;
            }
        }
        #endregion

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
                WriteLine($"Current TaskScheduler - {TaskScheduler.Current.GetType().FullName}");
                Task.Run(() => WriteLine($"\tThe nested-task  (task №{Task.CurrentId}) is completed in {Thread.CurrentThread.ManagedThreadId}"));
                Task.Factory.StartNew(() => WriteLine($"\tThe child-task (task №{Task.CurrentId}) is completed in {Thread.CurrentThread.ManagedThreadId}"),
                    TaskCreationOptions.AttachedToParent);
                WriteLine($"Task \"task\" (task №{Task.CurrentId}) is completed in {Thread.CurrentThread.ManagedThreadId}");
            }, CancellationToken.None, TaskCreationOptions.None, GetScheduler); // HideScheduler
            task.Wait();
            //await WorkAsync(null);
            //await WorkAsync(null).ConfigureAwait(false);

            ReadKey();
        }
    }
}
