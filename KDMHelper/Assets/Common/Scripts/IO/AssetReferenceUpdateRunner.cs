using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Common.IO
{
    public interface IAssetReferenceUpdateRunnerTask
    {
        bool UpdateAndFinish();
    }

    public class AssetReferenceUpdateRunner : SingletonMonoBehaviour<AssetReferenceUpdateRunner>
    {
        private bool m_Executing = false;
        private List<IAssetReferenceUpdateRunnerTask> m_Tasks = new List<IAssetReferenceUpdateRunnerTask>();
        private System.Object m_TaskLock = new System.Object();

        private List<Action> m_Actions = new List<Action>();
        private System.Object m_ActionLock = new System.Object();

        private Thread m_MainThread;
        public Thread MainThread { get { return m_MainThread; } }

        private new void Awake()
        {
            m_MainThread = Thread.CurrentThread;
            gameObject.hideFlags = HideFlags.HideAndDontSave;
            base.Awake();
        }

        public void AddTask(IAssetReferenceUpdateRunnerTask i_Task)
        {
            lock (m_TaskLock)
            {
                m_Tasks.Add(i_Task);
            }
        }

        public void AddAction(Action i_Action)
        {
            lock (m_ActionLock)
            {
                m_Actions.Add(i_Action);
            }
        }

        public void Update()
        {
            if (m_Tasks.Count > 0)
            {
                //prevent nested editor updates
                if (!m_Executing)
                {
                    m_Executing = true;
                    lock (m_TaskLock)
                    {
                        int count = m_Tasks.Count;
                        for (int i = count - 1; i >= 0; --i)
                        {
                            bool result = m_Tasks[i].UpdateAndFinish();
                            if (result)
                            {
                                m_Tasks.RemoveAt(i);
                            }
                        }
                    }
                    m_Executing = false;
                }
            }

            if(m_Actions.Count > 0)
            {
                lock (m_ActionLock)
                {
                    int count = m_Actions.Count;
                    for (int i = 0; i < count; ++i)
                    {
                        if (m_Actions[i] != null)
                        {
                            m_Actions[i].Invoke();
                        }
                    }
                    m_Actions.Clear();
                }
            }
        }
    }
}
