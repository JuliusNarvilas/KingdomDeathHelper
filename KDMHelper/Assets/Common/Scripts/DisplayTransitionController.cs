using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTransitionController : MonoBehaviour
{
    [Serializable]
    public class DisplayTransitionRecord : IEquatable<DisplayTransitionRecord>
    {
        public string Key;
        public Animator Anim;

        public bool Equals(DisplayTransitionRecord other)
        {
            return Key == other.Key;
        }
    }

    [SerializeField]
    private string m_TransitionInTriggerName = "TransitionIn";
    private int m_TransitionInTriggerId;
    [SerializeField]
    private string m_TransitionOutTriggerName = "TransitionOut";
    private int m_TransitionOutTriggerId;

    [SerializeField]
    private string m_DefaultDisplayKey;
    [SerializeField]
    private List<DisplayTransitionRecord> m_DisplayRecords = new List<DisplayTransitionRecord>();

    private DisplayTransitionRecord m_CurrentDisplay;
    private Coroutine m_WaitCoroutine;

    private void OnValidate()
    {
        if(m_DefaultDisplayKey != null)
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
        m_TransitionInTriggerId = Animator.StringToHash(m_TransitionInTriggerName);
        m_TransitionOutTriggerId = Animator.StringToHash(m_TransitionOutTriggerName);
    }

    private void Start()
    {
        if (m_DefaultDisplayKey != null)
        {
            m_CurrentDisplay = m_DisplayRecords.FirstOrDefault((x) => { return x.Key == m_DefaultDisplayKey; });
            if(m_CurrentDisplay != null)
            {
                m_CurrentDisplay.Anim.SetTrigger(m_TransitionInTriggerId);
            }
        }
    }

    private IEnumerator TransitionToCoroutine(string key)
    {
        if(m_CurrentDisplay != null)
        {
            m_CurrentDisplay.Anim.SetTrigger(m_TransitionOutTriggerId);
            while (m_CurrentDisplay.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f || m_CurrentDisplay.Anim.IsInTransition(0))
            {
                yield return null;
            }
        }

        m_CurrentDisplay = m_DisplayRecords.FirstOrDefault((x) => { return x.Key == key; });
        if(m_CurrentDisplay != null)
        {
            m_CurrentDisplay.Anim.SetTrigger(m_TransitionInTriggerId);
        }

        m_WaitCoroutine = null;
    }

    public void TransitionTo(string key)
    {
        if(m_WaitCoroutine != null)
        {
            StopCoroutine(m_WaitCoroutine);
        }
        m_WaitCoroutine = StartCoroutine(TransitionToCoroutine(key));
    }
}
