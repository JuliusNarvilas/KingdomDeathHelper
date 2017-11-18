using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Common.Text;

public class DisplayTransitionController : MonoBehaviour
{
    [Serializable]
    public class DisplayTransitionRecord : IEquatable<DisplayTransitionRecord>
    {
        public string Key;
        public Animator Anim;
        public float SpeedIn = 1.0f;
        public float SpeedOut = 1.0f;

        public bool Equals(DisplayTransitionRecord other)
        {
            return Key == other.Key;
        }
    }


    private static string m_TransitionInTriggerName = "TransitionIn";
    private static int m_TransitionInTriggerId;
    private static string m_TransitionOutTriggerName = "TransitionOut";
    private static int m_TransitionOutTriggerId;
    private static string m_JumpInTriggerName = "JumpIn";
    private static int m_JumpInTriggerId;
    private static string m_JumpOutTriggerName = "JumpOut";
    private static int m_JumpOutTriggerId;
    private static string m_SpeedParameterName = "Speed";
    private static int m_SpeedParameterId;

    static DisplayTransitionController()
    {
        m_TransitionInTriggerId = Animator.StringToHash(m_TransitionInTriggerName);
        m_TransitionOutTriggerId = Animator.StringToHash(m_TransitionOutTriggerName);
        m_JumpInTriggerId = Animator.StringToHash(m_JumpInTriggerName);
        m_JumpOutTriggerId = Animator.StringToHash(m_JumpOutTriggerName);
        m_SpeedParameterId = Animator.StringToHash(m_SpeedParameterName);
    }

    [SerializeField]
    private string m_DefaultDisplayKey;
    [SerializeField]
    private List<DisplayTransitionRecord> m_DisplayRecords = new List<DisplayTransitionRecord>();

    private DisplayTransitionRecord m_CurrentDisplay;
    private DisplayTransitionRecord m_CurrentTarget;
    private Coroutine m_WaitCoroutine;

    private void OnValidate()
    {
        if (m_DefaultDisplayKey != null)
        {
            var defaultMatch = m_DisplayRecords.FirstOrDefault((x) => { return x.Key == m_DefaultDisplayKey; });
            if(defaultMatch == null)
            {
                m_DefaultDisplayKey = null;
            }
        }

        /*
        for (int i = 0; i < (m_DisplayRecords.Count - 1); ++i)
        {
            var record = m_DisplayRecords[i];
            int match = -1;
            do
            {
                match = m_DisplayRecords.LastIndexOf(record, i + 1);
                if (match > i)
                {
                    m_DisplayRecords.RemoveAt(match);
                }
            } while (match > i);
        }
        */
    }

    private void Awake()
    {
        JumpTo(m_DefaultDisplayKey);
    }

    private IEnumerator TransitionToCoroutine(string key, bool jumpTo = false)
    {
        m_CurrentTarget = m_DisplayRecords.FirstOrDefault((x) => { return x.Key == key; });

        if (m_CurrentDisplay != null)
        {
            if(!jumpTo)
            {
                m_CurrentDisplay.Anim.SetFloat(m_SpeedParameterId, m_CurrentDisplay.SpeedOut);
                m_CurrentDisplay.Anim.SetTrigger(m_TransitionOutTriggerId);
            }
            else
            {
                m_CurrentDisplay.Anim.SetTrigger(m_JumpOutTriggerId);
            }
            do
            {
                yield return null;
            } while (IsInTransition());
        }

        m_CurrentDisplay = m_CurrentTarget;
        m_CurrentTarget = null;
        if (m_CurrentDisplay != null)
        {
            if (!jumpTo)
            {
                m_CurrentDisplay.Anim.SetFloat(m_SpeedParameterId, m_CurrentDisplay.SpeedIn);
                m_CurrentDisplay.Anim.SetTrigger(m_TransitionInTriggerId);
            }
            else
            {
                m_CurrentDisplay.Anim.SetTrigger(m_JumpInTriggerId);
            }
            //use m_WaitCoroutine to indicate end of transition in
            do
            {
                yield return null;
            } while (IsInTransition());
        }
        
        m_WaitCoroutine = null;
    }

    public bool IsInTransition()
    {
        if (m_CurrentDisplay != null)
        {
            return m_CurrentDisplay.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f || m_CurrentDisplay.Anim.IsInTransition(0);
        }
        return false;
    }

    public void TransitionTo(string key)
    {
        if ((m_CurrentTarget == null && (m_CurrentDisplay == null || m_CurrentDisplay.Key != key)) || (m_CurrentTarget != null && m_CurrentTarget.Key != key))
        {
            if (m_WaitCoroutine != null)
            {
                StopCoroutine(m_WaitCoroutine);
            }
            m_WaitCoroutine = StartCoroutine(TransitionToCoroutine(key));
        }
    }

    public void JumpTo(string key)
    {
        if ((m_CurrentTarget == null && (m_CurrentDisplay == null || m_CurrentDisplay.Key != key)) || (m_CurrentTarget != null && m_CurrentTarget.Key != key))
        {
            if (m_WaitCoroutine != null)
            {
                StopCoroutine(m_WaitCoroutine);
            }
            m_WaitCoroutine = StartCoroutine(TransitionToCoroutine(key, true));
        }
    }
}
