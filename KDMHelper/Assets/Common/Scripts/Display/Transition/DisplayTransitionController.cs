using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

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

        private struct DisplayTransition
        {
            public DisplayTransitionTarget Target;
            public ETransitionType TransitionOutType;
            public ETransitionType TransitionInType;
            public float TransitionOutSpeed;
            public float TransitionInSpeed;
        }

        private static string s_TransitionInTriggerName = "FadeIn";
        private static int s_TransitionInTriggerId;
        private static string s_TransitionOutTriggerName = "FadeOut";
        private static int s_TransitionOutTriggerId;
        private static string s_CutInTriggerName = "CutIn";
        private static int s_CutInTriggerId;
        private static string s_CutOutTriggerName = "CutOut";
        private static int s_CutOutTriggerId;
        private static string s_SpeedMultiplierParameterName = "SpeedMultiplier";
        private static int s_SpeedMultiplierParameterId;

        public const float SLOW_SPEED_MULTIPLIER = 0.5f;
        public const float DEFAULT_SPEED_MULTIPLIER = 1.0f;
        public const float FAST_SPEED_MULTIPLIER = 2.0f;

        static DisplayTransitionController()
        {
            s_TransitionInTriggerId = Animator.StringToHash(s_TransitionInTriggerName);
            s_TransitionOutTriggerId = Animator.StringToHash(s_TransitionOutTriggerName);
            s_CutInTriggerId = Animator.StringToHash(s_CutInTriggerName);
            s_CutOutTriggerId = Animator.StringToHash(s_CutOutTriggerName);
            s_SpeedMultiplierParameterId = Animator.StringToHash(s_SpeedMultiplierParameterName);
        }

        [SerializeField]
        private string m_DefaultDisplayKey;
        [SerializeField]
        private List<DisplayTransitionTarget> m_DisplayRecords = new List<DisplayTransitionTarget>();

        private DisplayTransitionTarget m_CurrentTarget;
        private DisplayTransition m_Transition;
        private DisplayTransition m_NextTransition;

        private EState m_State;
        public EState State { get { return m_State; } }

        private Coroutine m_TransitionCoroutine;


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
        }

        private void Awake()
        {
            TransitionFromCutToCut(m_DefaultDisplayKey);
        }

        private IEnumerator TransitionRunner()
        {
            bool process = true;
            while(process)
            {
                if (m_CurrentDisplay != m_Transition.Target && m_CurrentDisplay != null)
                {
                    m_State = EState.TransitionOut;

                    if (m_Transition.TransitionOutType == ETransitionType.Cut)
                    {
                        m_CurrentDisplay.Anim.SetTrigger(s_CutOutTriggerId);
                    }
                    else
                    {
                        m_CurrentDisplay.Anim.SetFloat(s_SpeedMultiplierParameterId, m_Transition.TransitionOutSpeed);
                        m_CurrentDisplay.Anim.SetTrigger(s_TransitionOutTriggerId);
                    }

                    do
                    {
                        yield return null;
                    } while (IsInTransition());
                }

                m_CurrentDisplay = m_Transition.Target;
                if (m_CurrentDisplay != null)
                {
                    m_State = EState.TransitionIn;

                    if (m_Transition.TransitionInType == ETransitionType.Cut)
                    {
                        m_CurrentDisplay.Anim.SetTrigger(s_CutInTriggerId);
                    }
                    else
                    {
                        m_CurrentDisplay.Anim.SetFloat(s_SpeedMultiplierParameterId, m_Transition.TransitionInSpeed);
                        m_CurrentDisplay.Anim.SetTrigger(s_TransitionInTriggerId);
                    }

                    do
                    {
                        yield return null;
                    } while (IsInTransition());
                }


                if(m_NextTransition.Target != null)
                {
                    process = true;
                    m_Transition = m_NextTransition;
                    m_NextTransition.Target = null;
                }
                else
                {
                    process = false;
                }
            }
            m_State = EState.Ready;
            m_TransitionCoroutine = null;
        }

        public bool IsInTransition()
        {
            if (m_CurrentDisplay != null && m_CurrentDisplay.Anim != null)
            {
                return m_CurrentDisplay.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f || m_CurrentDisplay.Anim.IsInTransition(0);
            }
            return false;
        }


        public void TransitionFromCutToCut(string i_Key)
        {
            TransitionTo(i_Key, ETransitionType.Cut, ETransitionType.Cut, DEFAULT_SPEED_MULTIPLIER, DEFAULT_SPEED_MULTIPLIER);
        }

        public void TransitionFromCutToFade(string i_Key)
        {
            TransitionTo(i_Key, ETransitionType.Cut, ETransitionType.Fade, DEFAULT_SPEED_MULTIPLIER, DEFAULT_SPEED_MULTIPLIER);
        }
        public void TransitionFromCutToFadeSlow(string i_Key)
        {
            TransitionTo(i_Key, ETransitionType.Cut, ETransitionType.Fade, DEFAULT_SPEED_MULTIPLIER, SLOW_SPEED_MULTIPLIER);
        }
        public void TransitionFromCutToFadeFast(string i_Key)
        {
            TransitionTo(i_Key, ETransitionType.Cut, ETransitionType.Fade, DEFAULT_SPEED_MULTIPLIER, FAST_SPEED_MULTIPLIER);
        }

        public void TransitionFromFadeToCut(string i_Key)
        {
            TransitionTo(i_Key, ETransitionType.Fade, ETransitionType.Cut, DEFAULT_SPEED_MULTIPLIER, DEFAULT_SPEED_MULTIPLIER);
        }
        public void TransitionFromFadeSlowToCut(string i_Key)
        {
            TransitionTo(i_Key, ETransitionType.Fade, ETransitionType.Cut, SLOW_SPEED_MULTIPLIER, DEFAULT_SPEED_MULTIPLIER);
        }
        public void TransitionFromFadeFastToCut(string i_Key)
        {
            TransitionTo(i_Key, ETransitionType.Fade, ETransitionType.Cut, FAST_SPEED_MULTIPLIER, DEFAULT_SPEED_MULTIPLIER);
        }

        public void TransitionFromFadeToFade(string i_Key)
        {
            TransitionTo(i_Key, ETransitionType.Fade, ETransitionType.Fade, DEFAULT_SPEED_MULTIPLIER, DEFAULT_SPEED_MULTIPLIER);
        }
        public void TransitionFromFadeSlowToFade(string i_Key)
        {
            TransitionTo(i_Key, ETransitionType.Fade, ETransitionType.Fade, SLOW_SPEED_MULTIPLIER, DEFAULT_SPEED_MULTIPLIER);
        }
        public void TransitionFromFadeFastToFade(string i_Key)
        {
            TransitionTo(i_Key, ETransitionType.Fade, ETransitionType.Fade, FAST_SPEED_MULTIPLIER, DEFAULT_SPEED_MULTIPLIER);
        }
        public void TransitionFromFadeToFadeSlow(string i_Key)
        {
            TransitionTo(i_Key, ETransitionType.Fade, ETransitionType.Fade, DEFAULT_SPEED_MULTIPLIER, SLOW_SPEED_MULTIPLIER);
        }
        public void TransitionFromFadeToFadeFast(string i_Key)
        {
            TransitionTo(i_Key, ETransitionType.Fade, ETransitionType.Fade, DEFAULT_SPEED_MULTIPLIER, FAST_SPEED_MULTIPLIER);
        }
        public void TransitionFromFadeSlowToFadeSlow(string i_Key)
        {
            TransitionTo(i_Key, ETransitionType.Fade, ETransitionType.Fade, SLOW_SPEED_MULTIPLIER, SLOW_SPEED_MULTIPLIER);
        }
        public void TransitionFromFadeSlowToFadeFast(string i_Key)
        {
            TransitionTo(i_Key, ETransitionType.Fade, ETransitionType.Fade, SLOW_SPEED_MULTIPLIER, FAST_SPEED_MULTIPLIER);
        }
        public void TransitionFromFadeFastToFadeSlow(string i_Key)
        {
            TransitionTo(i_Key, ETransitionType.Fade, ETransitionType.Fade, FAST_SPEED_MULTIPLIER, SLOW_SPEED_MULTIPLIER);
        }
        public void TransitionFromFadeFastToFadeFast(string i_Key)
        {
            TransitionTo(i_Key, ETransitionType.Fade, ETransitionType.Fade, FAST_SPEED_MULTIPLIER, FAST_SPEED_MULTIPLIER);
        }

        public void TransitionTo(string i_Key, ETransitionType i_TransitionOutType, ETransitionType i_TransitionInType, float i_TransitionOutSpeed, float i_TransitionInSpeed)
        {
            DisplayTransitionTarget newTarget = m_DisplayRecords.FirstOrDefault((x) => { return x.Key == i_Key; });
            if (newTarget == null && !string.IsNullOrEmpty(i_Key))
            {
                //do nothing with unrecognised keys
                return;
            }

            //cancel next pending transition;
            m_NextTransition.Target = null;
            bool forceNewTransition = false;

            switch (m_State)
            {
                case EState.None:
                    if (m_CurrentDisplay != newTarget)
                    {
                        forceNewTransition = true;
                    }
                    break;
                case EState.TransitionOut:
                    //if requested a faster out transition
                    if (i_TransitionOutType == ETransitionType.Cut && m_Transition.TransitionOutType != ETransitionType.Cut)
                    {
                        var animInfo = m_CurrentTarget.Anim.GetCurrentAnimatorStateInfo(0);
                        float speed = animInfo.speed * animInfo.speedMultiplier;
                        if (speed != 0)
                        {
                            float timeLeft = animInfo.length / speed;
                            //don't swap to faster transition out type if almost finished
                            if (timeLeft > 0.5f)
                            {
                                forceNewTransition = true;
                            }
                        }
                        else
                        {
                            //force transition change for invalid speed
                            forceNewTransition = true;
                        }
                    }

                    //if different target or requested a faster transition in type (same or different target)
                    if ((m_Transition.Target != newTarget) || (i_TransitionInType == ETransitionType.Cut && m_Transition.TransitionInType != ETransitionType.Cut))
                    {
                        //!!! no need to cancel current transition !!!
                        m_Transition.Target = newTarget;
                        m_Transition.TransitionInType = i_TransitionInType;
                        m_Transition.TransitionInSpeed = i_TransitionInSpeed;
                    }
                    break;
                case EState.TransitionIn:
                    //if the same target
                    if(newTarget == m_Transition.Target)
                    {
                        //if requested a faster transition in type on the same target
                        if (i_TransitionInType == ETransitionType.Cut && m_Transition.TransitionInType != ETransitionType.Cut)
                        {
                            var animInfo = m_Transition.Target.Anim.GetCurrentAnimatorStateInfo(0);
                            float speed = animInfo.speed * animInfo.speedMultiplier;
                            if (speed != 0)
                            {
                                float timeLeft = animInfo.length / speed;
                                //don't swap to faster transition in type if almost finished
                                if (timeLeft > 0.5f)
                                {
                                    forceNewTransition = true;
                                }
                            }
                            else
                            {
                                //force transition change for invalid speed
                                forceNewTransition = true;
                            }
                        }
                    }
                    //if allowed to transition out with a cut to a different target
                    else if (i_TransitionOutType == ETransitionType.Cut)
                    {
                        forceNewTransition = true;
                    }
                    else
                    {
                        //append for next transition
                        m_NextTransition.Target = newTarget;
                        m_NextTransition.TransitionOutType = i_TransitionOutType;
                        m_NextTransition.TransitionInType = i_TransitionOutType;
                    }
                    break;
                case EState.Ready:
                    if (m_CurrentDisplay != newTarget)
                    {
                        forceNewTransition = true;
                    }
                    break;
            }

            if(forceNewTransition)
            {
                m_Transition.Target = newTarget;
                m_Transition.TransitionOutType = i_TransitionOutType;
                m_Transition.TransitionInType = i_TransitionInType;
                m_Transition.TransitionOutSpeed = i_TransitionOutSpeed;
                m_Transition.TransitionInSpeed = i_TransitionInSpeed;

                if (m_TransitionCoroutine != null)
                {
                    StopCoroutine(m_TransitionCoroutine);
                }
                m_TransitionCoroutine = StartCoroutine(TransitionRunner());
            }
        }
    }

}