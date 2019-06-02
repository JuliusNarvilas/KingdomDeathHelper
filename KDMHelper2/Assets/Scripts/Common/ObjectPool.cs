using System;
using System.Collections.Generic;

namespace Common
{
    public interface IPoolable
    {
        void PoolingClear();
    }

    /// <summary>
    /// A pool of reusable <see cref="GridPathElement{TPosition, TContext, TTile}"/> instances.
    /// </summary>
    public abstract class ObjectPool<TObj> where TObj : IPoolable
    {
        private readonly object m_SyncLock = new object();
        private readonly List<TObj> m_Data;

        public abstract TObj Create();

        public ObjectPool(int i_Capacity)
        {
            m_Data = new List<TObj>(i_Capacity);
        }

        public TObj Get()
        {
            lock (m_SyncLock)
            {
                if (m_Data.Count > 0)
                {
                    var result = m_Data[m_Data.Count - 1];
                    m_Data.RemoveAt(m_Data.Count - 1);
                    return result;
                }
            }
            return Create();
        }

        public TObj GetUnsafe()
        {
            if (m_Data.Count > 0)
            {
                var result = m_Data[m_Data.Count - 1];
                m_Data.RemoveAt(m_Data.Count - 1);
                return result;
            }
            return Create();
        }

        public void GetMultiple(int i_Count, List<TObj> o_List)
        {
            if (o_List != null)
            {
                int newCapacity = o_List.Count + i_Count;
                if (o_List.Capacity < newCapacity)
                {
                    o_List.Capacity = newCapacity;
                }

                int filledCount;
                lock (m_SyncLock)
                {
                    int dataCount = m_Data.Count;
                    int minIndex = Math.Max(dataCount - i_Count, 0);
                    filledCount = dataCount - minIndex;
                    if (filledCount > 0)
                    {
                        for (int i = minIndex; i < dataCount; ++i)
                        {
                            o_List.Add(m_Data[i]);
                        }
                        m_Data.RemoveRange(minIndex, filledCount);
                    }
                }

                while (filledCount < i_Count)
                {
                    o_List.Add(Create());
                    ++filledCount;
                }
            }
        }

        public void GetMultipleUnsafe(int i_Count, List<TObj> o_List)
        {
            if (o_List != null)
            {
                int newCapacity = o_List.Count + i_Count;
                if (o_List.Capacity < newCapacity)
                {
                    o_List.Capacity = newCapacity;
                }

                int filledCount;
                int dataCount = m_Data.Count;
                int minIndex = Math.Max(dataCount - i_Count, 0);
                filledCount = dataCount - minIndex;
                if (filledCount > 0)
                {
                    for (int i = minIndex; i < dataCount; ++i)
                    {
                        o_List.Add(m_Data[i]);
                    }
                    m_Data.RemoveRange(minIndex, filledCount);
                }

                while (filledCount < i_Count)
                {
                    o_List.Add(Create());
                    ++filledCount;
                }
            }
        }

        public void Recycle(TObj i_Data)
        {
            i_Data.PoolingClear();
            lock (m_SyncLock)
            {
                if (m_Data.Count < m_Data.Capacity)
                {
                    m_Data.Add(i_Data);
                }
            }
        }

        public void RecycleUnsafe(TObj i_Data)
        {
            i_Data.PoolingClear();
            if (m_Data.Count < m_Data.Capacity)
            {
                m_Data.Add(i_Data);
            }
        }

        public void RecycleMultiple(List<TObj> i_Data)
        {
            int newDataCount = i_Data.Count;
            lock (m_SyncLock)
            {
                int count = Math.Min(m_Data.Capacity - m_Data.Count, newDataCount);
                for (int i = 0; i < count; ++i)
                {
                    var element = i_Data[i];
                    element.PoolingClear();
                    m_Data.Add(element);
                }
            }
            i_Data.Clear();
        }

        public void RecycleMultipleUnsafe(List<TObj> i_Data)
        {
            int newDataCount = i_Data.Count;
            int count = Math.Min(m_Data.Capacity - m_Data.Count, newDataCount);
            for (int i = 0; i < count; ++i)
            {
                var element = i_Data[i];
                element.PoolingClear();
                m_Data.Add(element);
            }
            i_Data.Clear();
        }

    }
}
