using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Common.Text;

namespace Common.Display.Transition
{

    public class DisplayTransitionController : MonoBehaviour
    {
        public enum EState
        {
            None,
            TransitionOut,
            TransitionIn,
            Ready
        }
        public enum ETransitionType
        {
            Fade,
            Cut
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
        private List<DisplayTransitionTarget> m_DisplayRecords = new List<DisplayTransitionTarget>();


        private DisplayTransitionTarget m_TransitionDisplay;
        private DisplayTransitionTarget m_CurrentTarget;
        private Coroutine m_WaitCoroutine;
        private EState m_State;
        public EState State { get { return m_State; } }
        private ETransitionType m_TransitionOutType;
        private ETransitionType m_TransitionInType;



        private DisplayTransitionTarget m_TransitionOutTarget;
        private DisplayTransitionTarget m_TransitionInTarget;
        private DisplayTransitionTarget m_CurrentDisplay;

        private void OnValidate()
        {
            if (m_DefaultDisplayKey != null)
            {
                var defaultMatch = m_DisplayRecords.FirstOrDefault((x) => { return x.Key == m_DefaultDisplayKey; });
                if (defaultMatch == null)
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
            /*
            if (oldTransition != null)
            {
                //wait for old transition to finish
                yield return oldTransition;
            }
            */

            //Debug.Log("Transition to " + key + " jump = " + jumpTo.ToString());
            m_CurrentTarget = m_DisplayRecords.FirstOrDefault((x) => { return x.Key == key; });

            if (m_CurrentDisplay != null)
            {
                if (!jumpTo)
                {
                    m_CurrentDisplay.Anim.SetFloat(m_SpeedParameterId, m_CurrentDisplay.SpeedOut);
                    m_CurrentDisplay.Anim.SetTrigger(m_TransitionOutTriggerId);
                    Debug.Log("transition out " + m_CurrentDisplay.Anim.gameObject.name + " " + m_CurrentDisplay.Key + " " + key);
                }
                else
                {
                    m_CurrentDisplay.Anim.SetTrigger(m_JumpOutTriggerId);
                    Debug.Log("jump out " + m_CurrentDisplay.Anim.gameObject.name + " " + m_CurrentDisplay.Key + " " + key);
                }
            }
            //Debug.Log("Current display set " + key + " jump = " + jumpTo.ToString());
            Debug.Log(key + " 1");
            m_TransitionDisplay = m_CurrentDisplay;
            m_CurrentDisplay = m_CurrentTarget;
            m_CurrentTarget = null;

            Debug.Log(key + " 2");
            do
            {
                yield return null;
            } while (IsInTransition());
            Debug.Log(key + " 3");

            m_TransitionDisplay = m_CurrentDisplay;

            if (m_CurrentDisplay != null)
            {
                if (!jumpTo)
                {
                    m_CurrentDisplay.Anim.SetFloat(m_SpeedParameterId, m_CurrentDisplay.SpeedIn);
                    m_CurrentDisplay.Anim.SetTrigger(m_TransitionInTriggerId);
                    Debug.Log("transition in " + m_CurrentDisplay.Anim.gameObject.name + " " + m_CurrentDisplay.Key + " " + key);
                }
                else
                {
                    m_CurrentDisplay.Anim.SetTrigger(m_JumpInTriggerId);
                    Debug.Log("jump in " + m_CurrentDisplay.Anim.gameObject.name + " " + m_CurrentDisplay.Key + " " + key);
                }
                //use m_WaitCoroutine to indicate end of transition in
                Debug.Log(key + " 4");
                do
                {
                    yield return null;
                } while (IsInTransition());
            }
            Debug.Log(key + " 5");

            m_WaitCoroutine = null;
        }

        public bool IsInTransition()
        {
            try
            {
                if (m_TransitionDisplay != null)
                {
                    return m_TransitionDisplay.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f || m_TransitionDisplay.Anim.IsInTransition(0);
                }
            }
            catch (Exception e)
            {
                Debug.Log("?????????? " + e.Message);
            }
            return false;
        }

        public void TransitionTo(string key)
        {
            if ((m_CurrentTarget == null && (m_CurrentDisplay == null || m_CurrentDisplay.Key != key)) || (m_CurrentTarget != null && m_CurrentTarget.Key != key))
            {
                if (m_WaitCoroutine != null)
                {
                    Debug.Log("!!!!!!");
                    StopCoroutine(m_WaitCoroutine);
                    m_WaitCoroutine = null;
                }
                m_WaitCoroutine = StartCoroutine(TransitionToCoroutine(key));
            }

            ETransitionType newTransitionInType = ETransitionType.Fade;
            ETransitionType newTransitionOutType = ETransitionType.Fade;
            var newTarget = m_DisplayRecords.FirstOrDefault((x) => { return x.Key == key; });

            bool replaceOutTransition = false;
            bool replaceInTransition = false;

            switch (m_State)
            {
                case EState.None:
                    m_WaitCoroutine = StartCoroutine(TransitionToCoroutine(key));
                    break;
                case EState.TransitionOut:

                    if (newTransitionOutType == ETransitionType.Cut && m_TransitionOutType != ETransitionType.Cut)
                    {
                        replaceOutTransition = true;
                    }
                    goto case EState.TransitionIn;
                case EState.TransitionIn:

                    if (newTarget == m_TransitionInTarget)
                    {
                        if (newTransitionOutType == ETransitionType.Cut && m_TransitionOutType != ETransitionType.Cut)
                        {
                            replaceInTransition = true;
                        }
                    }
                    else
                    {
                        replaceInTransition = true;
                    }

                    break;
                case EState.Ready:
                    if (newTarget == m_CurrentTarget)
                    {
                        return;
                    }
                    m_WaitCoroutine = StartCoroutine(TransitionToCoroutine(key));
                    break;
            }
        }

        public void JumpTo(string key)
        {
            if ((m_CurrentTarget == null && (m_CurrentDisplay == null || m_CurrentDisplay.Key != key)) || (m_CurrentTarget != null && m_CurrentTarget.Key != key))
            {
                if (m_WaitCoroutine != null)
                {
                    Debug.Log("!!!!!!");
                    StopCoroutine(m_WaitCoroutine);
                    m_WaitCoroutine = null;
                }
                m_WaitCoroutine = StartCoroutine(TransitionToCoroutine(key, true));
            }
        }
    }

}