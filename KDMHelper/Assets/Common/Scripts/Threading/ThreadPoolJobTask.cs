using System;
using System.Collections.Generic;
using System.Threading;

namespace Common.Threading
{
    /// <summary>
    /// Generic Task to be run async
    /// </summary>
    public class ThreadPoolJobTask
    {
        /// <summary>
        /// The state of a threaded task.
        /// </summary>
        public EThreadedTaskState State = EThreadedTaskState.InProgress;
        /// <summary>
        /// The exception thrown during task execution.
        /// </summary>
        public Exception Exception = null;
        /// <summary>
        /// The task run function.
        /// </summary>
        public readonly Action RunFunc = null;
        /// <summary>
        /// The execution time limit before task is aborted.
        /// </summary>
        public readonly TimeSpan MaxRunTime;
        /// <summary>
        /// A prioritising measure that <see cref="ThreadPool"/> attempts to enforce.
        /// </summary>
        public readonly DateTime ExpectedEndTimestamp;
        /// <summary>
        /// The execution start time.
        /// </summary>
        public DateTime RunStartTimestamp = DateTime.MaxValue;

        public ThreadPoolJobTask(Action i_RunFunc, ThreadPriority i_Priority = ThreadPriority.Normal)
        {
            RunFunc = i_RunFunc;
            MaxRunTime = new TimeSpan(0, 5, 0);
            ExpectedEndTimestamp = GetExpectedEndTime(i_Priority);
        }
        public ThreadPoolJobTask(Action i_RunFunc, TimeSpan i_MaxRunTime, ThreadPriority i_Priority = ThreadPriority.Normal)
        {
            RunFunc = i_RunFunc;
            MaxRunTime = i_MaxRunTime;
            ExpectedEndTimestamp = GetExpectedEndTime(i_Priority);
        }

        private static DateTime GetExpectedEndTime(ThreadPriority i_Priority)
        {
            switch (i_Priority)
            {
                case ThreadPriority.Lowest:
                    return DateTime.UtcNow.AddSeconds(4);
                case ThreadPriority.BelowNormal:
                    return DateTime.UtcNow.AddSeconds(3);
                case ThreadPriority.Normal:
                    return DateTime.UtcNow.AddSeconds(2);
                case ThreadPriority.AboveNormal:
                    return DateTime.UtcNow.AddSeconds(1);
                case ThreadPriority.Highest:
                    return DateTime.UtcNow.AddMilliseconds(500);
            }
            return DateTime.UtcNow.AddSeconds(4);
        }

        /// <summary>
        /// A sorting comparer for prioritising tasks based on <see cref="ExpectedEndTimestamp"/> data.
        /// </summary>
        /// <seealso cref="System.Collections.Generic.IComparer{Common.Threading.ThreadPoolJobTask}" />
        public class TerminationComparer : IComparer<ThreadPoolJobTask>
        {
            private int m_Multiplier = 1;

            private TerminationComparer(bool i_Ascending)
            {
                m_Multiplier = i_Ascending ? 1 : -1;
            }

            public int Compare(ThreadPoolJobTask i_A, ThreadPoolJobTask i_B)
            {
                return i_A.ExpectedEndTimestamp.CompareTo(i_B.ExpectedEndTimestamp) * m_Multiplier;
            }
            
            public static TerminationComparer Ascending = new TerminationComparer(true);
            public static TerminationComparer Descending = new TerminationComparer(false);
        }
    }
}
