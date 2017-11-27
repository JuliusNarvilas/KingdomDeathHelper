using Game.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screen.Sorting
{

    public class ScreenSortInfo : MonoBehaviour
    {
        [SerializeField]
        private Text m_Text;

        [SerializeField]
        private Image m_SortAscIndicator;
        [SerializeField]
        private Image m_SortDesIndicator;

        private ESortType m_State = ESortType.None;
        public ESortType State { get { return m_State; } }

        private IScreenSortReceiver m_InfoReceiver;
        private Action<bool> m_SortAction;

        public void Init(IScreenSortReceiver i_InfoReceiver, Action<bool> i_SortAction)
        {
            m_InfoReceiver = i_InfoReceiver;
            m_SortAction = i_SortAction;
        }

        public void NextState()
        {
            switch(m_State)
            {
                case ESortType.Ascending:
                    SetState(ESortType.Descending);
                    break;
                case ESortType.Descending:
                    SetState(ESortType.None);
                    break;
                default:
                    SetState(ESortType.Ascending);
                    break;
            }
        }

        public void SetState(ESortType i_State)
        {
            if (i_State == m_State)
                return;

            if (m_State == ESortType.None)
            {
                //TODO: Spawn new display indicator
                m_InfoReceiver.Append(this);
            }
            else
            {
                if (i_State == ESortType.None)
                {
                    //TODO: delete display indicator
                    m_InfoReceiver.Remove(this);
                }
                else
                {
                    m_InfoReceiver.TriggerOnChange();
                }
            }
            m_State = i_State;
        }

        public void SetStateSilent(ESortType i_State)
        {
            if (i_State == m_State)
                return;

            if (m_State == ESortType.None)
            {
                //TODO: Spawn new display indicator
            }
            else
            {
                if (i_State == ESortType.None)
                {
                    //TODO: delete display indicator
                }
            }
            m_State = i_State;
        }


        public void ApplySorting()
        {
            if (m_State != ESortType.None && m_SortAction != null)
            {
                m_SortAction(m_State == ESortType.Ascending);
            }
        }
    }
}
