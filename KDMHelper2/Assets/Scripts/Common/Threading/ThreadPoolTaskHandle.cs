using System;
using System.Threading;

namespace Common.Threading
{
    public interface IThreadPoolTaskHandle
    {
        EThreadedTaskState State { get; }
        Exception GetException();
    }

    public struct ThreadPoolTaskHandle : IThreadPoolTaskHandle
    {
        private readonly ThreadPoolJobTask m_TaskLink;

        public ThreadPoolTaskHandle(ThreadPoolJobTask i_TaskLink)
        {
            m_TaskLink = i_TaskLink;
        }

        public EThreadedTaskState State
        {
            get { return m_TaskLink.State; }
        }

        public Exception GetException()
        {
            return m_TaskLink.Exception;
        }
    }
    

    public class ThreadPoolTaskResult<T> : IThreadPoolTaskHandle
    {
        private readonly ThreadPoolJobTask m_TaskLink;
        private T m_Result = default(T);

        public static ThreadPoolTaskResult<T> Create(Func<T> i_Funk, out ThreadPoolJobTask o_Task)
        {
            var result = new ThreadPoolTaskResult<T>(i_Funk);
            o_Task = result.m_TaskLink;
            return result;
        }

        public static ThreadPoolTaskResult<T> Create(Func<T> i_Funk, TimeSpan i_MaxRuntime, ThreadPriority i_Priority, out ThreadPoolJobTask o_Task)
        {
            var result = new ThreadPoolTaskResult<T>(i_Funk, i_MaxRuntime, i_Priority);
            o_Task = result.m_TaskLink;
            return result;
        }

        private ThreadPoolTaskResult(Func<T> i_Funk)
        {
            m_TaskLink = new ThreadPoolJobTask(() => { m_Result = i_Funk.Invoke(); });
        }

        private ThreadPoolTaskResult(Func<T> i_Funk, TimeSpan i_MaxRuntime, ThreadPriority i_Priority)
        {
            m_TaskLink = new ThreadPoolJobTask(() => { m_Result = i_Funk.Invoke(); }, i_MaxRuntime, i_Priority);
        }

        public EThreadedTaskState State
        {
            get { return m_TaskLink.State; }
        }

        public T Result
        {
            get { return m_Result; }
        }

        public Exception GetException()
        {
            return m_TaskLink.Exception;
        }
    }
}
