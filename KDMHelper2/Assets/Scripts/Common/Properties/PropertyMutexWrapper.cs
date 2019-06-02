using Common.Threading;
using System.Threading;

namespace Common.Properties
{
    public struct PropertyMutexWrapper<T, TProperty> where TProperty : Property<T>
    {
        public readonly TProperty Property;
        public readonly Mutex m_Mutex;

        public PropertyMutexWrapper(TProperty i_Property)
        {
            Property = i_Property;
            m_Mutex = new Mutex();
        }

        public MutexLock GetMutexPropertyLock()
        {
            return new MutexLock(m_Mutex);
        }
    }

    public struct PropertyMutexWrapper<T>
    {
        public readonly Property<T> Property;
        public readonly Mutex m_Mutex;

        public PropertyMutexWrapper(Property<T> i_Property)
        {
            Property = i_Property;
            m_Mutex = new Mutex();
        }

        public MutexLock GetMutexPropertyLock()
        {
            return new MutexLock(m_Mutex);
        }
    }
}
