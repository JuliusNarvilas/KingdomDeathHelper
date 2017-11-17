using Common.Collections;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Common.Threading
{
    public enum EThreadedTaskState
    {
        InProgress,
        Aborted,
        Succeeded,
        Errored
    }

    /// <summary>
    /// Simple implementation of Thread Pool functionality.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class ThreadPool : IDisposable
    {
        /// <summary>
        /// A <see cref="ThreadPool"/> worker thread managing class.
        /// </summary>
        private class ThreadPoolJob
        {
            /// <summary>
            /// The thread task change lock handle
            /// </summary>
            public readonly object ThreadTaskChangeHandle = new object();
            /// <summary>
            /// The current task.
            /// </summary>
            private ThreadPoolJobTask m_Task = null;
            /// <summary>
            /// The assigned thread pool for task acquisition.
            /// </summary>
            private readonly ThreadPool m_ThreadPool;
            /// <summary>
            /// Thread closing condition.
            /// </summary>
            private bool m_Active = true;
            /// <summary>
            /// The worker thread.
            /// </summary>
            private readonly Thread m_Thread;

            /// <summary>
            /// Gets the current task.
            /// </summary>
            /// <value>
            /// The <see cref="ThreadPool"/> task.
            /// </value>
            public ThreadPoolJobTask Task
            {
                get { return m_Task; }
            }

            public ThreadPoolJob(ThreadPool i_ThreadPool)
            {
                Log.DebugAssert(i_ThreadPool != null, "Invalid thread pool argument");
                m_ThreadPool = i_ThreadPool;

                m_Thread = new Thread(Run);
                m_Thread.Name = "Thread Pool worker";
                m_Thread.Priority = ThreadPriority.Normal;
                m_Thread.Start();
            }

            public bool Active
            {
                get { return m_Active; }
            }
            
            public void Close()
            {
                m_Active = false;
            }

            public void Abort()
            {
                m_Active = false;
                try
                {
                    m_Thread.Abort();
                    if (m_Task != null)
                    {
                        m_Task.State = EThreadedTaskState.Aborted;
                    }
                }
                catch
                {
                    //do nothing
                }
                m_Task = null;
            }

            /// <summary>
            /// Worker thread execution function.
            /// </summary>
            private void Run()
            {
                while (m_Active)
                {
                    //finding new tasks
                    if (m_ThreadPool.m_Tasks.Count > 0)
                    {
                        lock (m_ThreadPool.m_ThreadPoolTaskListChangeHandle)
                        {
                            lock (ThreadTaskChangeHandle)
                            {
                                int lastIndex = m_ThreadPool.m_Tasks.Count - 1;
                                if (lastIndex >= 0)
                                {
                                    m_Task = m_ThreadPool.m_Tasks[lastIndex];
                                    m_ThreadPool.m_Tasks.RemoveAt(lastIndex);
                                }
                            }
                        }
                    }
                    if (m_Task != null)
                    {
                        try
                        {
                            m_Task.RunStartTimestamp = DateTime.UtcNow;
                            m_Task.RunFunc.Invoke();
                            m_Task.State = EThreadedTaskState.Succeeded;
                        }
                        catch (Exception e)
                        {
                            m_Task.Exception = e;
                            m_Task.State = EThreadedTaskState.Errored;
                        }
                        finally
                        {
                            lock (ThreadTaskChangeHandle)
                            {
                                m_Task = null;
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
            }
        }

        /// <summary>
        /// The thread pool task list change lock handle.
        /// </summary>
        private readonly object m_ThreadPoolTaskListChangeHandle = new object();
        /// <summary>
        /// The seperate thread for running the Thread Pool Manager.
        /// </summary>
        private readonly Thread m_ManagerThread;
        /// <summary>
        /// The tasks to be processed.
        /// </summary>
        private readonly List<ThreadPoolJobTask> m_Tasks = new List<ThreadPoolJobTask>(5);
        /// <summary>
        /// The list of active workers.
        /// </summary>
        private readonly List<ThreadPoolJob> m_ActiveThreadList = new List<ThreadPoolJob>();
        /// <summary>
        /// The list of closing workers.
        /// </summary>
        private readonly List<ThreadPoolJob> m_ClosingThreadList = new List<ThreadPoolJob>();
        /// <summary>
        /// The thread pool termination condition.
        /// </summary>
        private bool m_Active = true;

        private static ThreadPool m_Instance = new ThreadPool();

        public static ThreadPool Instance { get { return m_Instance; } }

        public ThreadPool()
        {
            m_ManagerThread = new Thread(ManagerRun);
            m_ManagerThread.Name = "ThreadPool Manager";
            m_ManagerThread.Priority = ThreadPriority.Lowest;
            m_ActiveThreadList.Add(new ThreadPoolJob(this));
        }

        public int PendingTaskCount
        {
            get { return m_Tasks.Count; }
        }

        public ThreadPoolTaskHandle AddTask(Action i_Action)
        {
            return AddTask(i_Action, new TimeSpan(0, 5, 0));
        }

        public ThreadPoolTaskHandle AddTask(Action i_Action, TimeSpan i_MaxRuntime, ThreadPriority i_Priority = ThreadPriority.Normal)
        {
            ThreadPoolJobTask task = new ThreadPoolJobTask(i_Action, i_MaxRuntime, i_Priority);
            lock (m_ThreadPoolTaskListChangeHandle)
            {
                m_Tasks.Add(task);
                m_Tasks.InsertionSort(ThreadPoolJobTask.TerminationComparer.Descending);
            }
            return new ThreadPoolTaskHandle(task);
        }

        public ThreadPoolTaskResult<TResult> AddTask<TResult>(Func<TResult> i_Func)
        {
            return AddTask(i_Func, new TimeSpan(0, 5, 0));
        }

        public ThreadPoolTaskResult<TResult> AddTask<TResult>(Func<TResult> i_Func, TimeSpan i_MaxRuntime, ThreadPriority i_Priority = ThreadPriority.Normal)
        {
            ThreadPoolJobTask newTask;
            var result = ThreadPoolTaskResult<TResult>.Create(i_Func, i_MaxRuntime, i_Priority, out newTask);
            lock (m_ThreadPoolTaskListChangeHandle)
            {
                m_Tasks.Add(newTask);
                m_Tasks.InsertionSort(ThreadPoolJobTask.TerminationComparer.Descending);
            }
            return result;
        }

        /// <summary>
        /// Thread pool manager execution function.
        /// </summary>
        private void ManagerRun()
        {
            while(m_Active)
            {
                int newThreadCount = GetEstimatedThreadCount();
                int requiredExtraJobs = MaintainActiveThreadList(newThreadCount);
                for (int i = 0; i < requiredExtraJobs; ++i)
                {
                    m_ActiveThreadList.Add(new ThreadPoolJob(this));
                }

                Log.DebugLogIf(requiredExtraJobs > 0, "ThreadPool is adding {0} thread workers.", requiredExtraJobs);
                
                MaintainClosingThreadList();
                
                Thread.Sleep(200);
            }

            //force close
            int size = m_ActiveThreadList.Count;
            for( int i = 0; i < size; ++i)
            {
                m_ActiveThreadList[i].Abort();
            }
            m_ActiveThreadList.Clear();

            size = m_ClosingThreadList.Count;
            for (int i = 0; i < size; ++i)
            {
                m_ClosingThreadList[i].Abort();
            }
            m_ClosingThreadList.Clear();
        }


        /// <summary>
        /// Gets the ideal estimated thread count for pending tasks.
        /// </summary>
        /// <returns></returns>
        private int GetEstimatedThreadCount()
        {
            int lateTaskCount = 0;
            lock (m_ThreadPoolTaskListChangeHandle)
            {
                DateTime now = DateTime.UtcNow;
                int size = m_ClosingThreadList.Count;
                ThreadPoolJobTask task;
                for (int i = 0; i < size; ++i)
                {
                    task = m_ClosingThreadList[i].Task;
                    if (task != null)
                    {
                        if ((task.ExpectedEndTimestamp - now).TotalMilliseconds > 0)
                        {
                            ++lateTaskCount;
                        }
                    }
                }
            }
            lateTaskCount /= 3;
            return lateTaskCount <= 0 ? 1 : lateTaskCount;
        }

        /// <summary>
        /// Maintains the active thread list by closing unwanted active workers.
        /// </summary>
        /// <param name="i_RequiredJobCount">The required number of jobs.</param>
        /// <returns>The number of required new workers.</returns>
        private int MaintainActiveThreadList(int i_RequiredJobCount)
        {
            int ExtraThreadCount = i_RequiredJobCount;
            DateTime now = DateTime.UtcNow;
            for(int i = m_ActiveThreadList.Count - 1; i >= 0; --i)
            {
                ThreadPoolJob threadJob = m_ActiveThreadList[i];
                lock (threadJob.ThreadTaskChangeHandle)
                {
                    if (threadJob.Task == null)
                    {
                        if(ExtraThreadCount > 0)
                        {
                            --ExtraThreadCount;
                        }
                        else
                        {
                            threadJob.Close();
                            m_ActiveThreadList.RemoveAt(i);
                            m_ClosingThreadList.Add(threadJob);
                        }
                    }
                    else if((threadJob.Task.RunStartTimestamp + threadJob.Task.MaxRunTime - now).TotalMilliseconds > 0)
                    {
                        threadJob.Abort();
                        m_ActiveThreadList.RemoveAt(i);
                    }
                }
            }
            return ExtraThreadCount;
        }

        /// <summary>
        /// Maintains the closing thread list by tracking unfinished execution and aborting threads past the maximum execution time threshold.
        /// </summary>
        private void MaintainClosingThreadList()
        {
            int closingCount = 0;
            int abortingCount = 0;
            DateTime now = DateTime.UtcNow;
            for (int i = m_ClosingThreadList.Count - 1; i >= 0; --i)
            {
                ThreadPoolJob threadJob = m_ClosingThreadList[i];
                if (threadJob.Task == null)
                {
                    m_ClosingThreadList.RemoveAt(i);
                    ++closingCount;
                }
                else if ((threadJob.Task.RunStartTimestamp + threadJob.Task.MaxRunTime - now).TotalMilliseconds > 0)
                {
                    threadJob.Abort();
                    m_ClosingThreadList.RemoveAt(i);
                    ++abortingCount;
                }
            }
            Log.DebugLogIf(closingCount > 0, "ThreadPool is closing {0} thread workers.", closingCount);
            Log.DebugLogErrorIf(abortingCount > 0, "ThreadPool is aborting {0} thread workers.", abortingCount);
        }

        public void Dispose()
        {
            m_Active = false;
        }
    }
}
