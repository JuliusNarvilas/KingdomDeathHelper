using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    public class AnimationTrigger : MonoBehaviour
    {
        [Serializable]
        private struct AnimationTriggerRecord
        {
            public string Key;
            public UnityEvent Action;
        }

        [SerializeField]
        private List<AnimationTriggerRecord> m_TriggerRecords;

        public void Trigger(string i_Key)
        {
            int count = m_TriggerRecords.Count;
            for(int i = 0; i < count; ++i)
            {
                var record = m_TriggerRecords[i];
                if(record.Key == i_Key)
                {
                    if(record.Action != null)
                    {
                        record.Action.Invoke();
                    }
                }
            }
        }
    }
}
