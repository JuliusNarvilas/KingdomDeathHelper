using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Common.Display.Transition
{
    public enum ETransitionType
    {
        Jump,
        Normal
    }

    public class DisplayTransition
    {
        public enum EState
        {
            None,
            TransitionOut,
            TransitionIn,
            Finished
        }
        
        DisplayTransitionTarget Target;
        EState State;
        Coroutine Runner;
        ETransitionType TransitionType;

        public DisplayTransition(DisplayTransitionTarget i_Destination)
        {
            Target = i_Destination;
        }

        public void Test()
        {

        }
        

        public void TransitionIn(DisplayTransitionController i_Controller)
        {
            i_Controller.StartCoroutine(TransitionInRunner);
        }

        public IEnumerator TransitionInRunner()
        {
            /*
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
            */

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
    }
}
