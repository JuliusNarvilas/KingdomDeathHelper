using Game.Display.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Common.Display.Table
{
    public class TableSortInfoDisplay : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Text m_Text;
        [SerializeField]
        private Image m_TypeIndicator;

        [SerializeField]
        private Image m_SortAscIndicator;
        [SerializeField]
        private Image m_SortDesIndicator;


        protected TableSortInfo m_SortInfo;

        public void Set(TableSortInfo i_SortInfo)
        {
            m_SortInfo = i_SortInfo;
            if(m_TypeIndicator != null)
            {
                bool enableImage = i_SortInfo.TypeImage != null;
                if (enableImage)
                {
                    m_TypeIndicator.sprite = i_SortInfo.TypeImage.sprite;
                }
                m_TypeIndicator.enabled = enableImage;
            }

            if(m_Text != null)
            {
                bool enableText = i_SortInfo.Text;
                if(enableText)
                {
                    m_Text.text = i_SortInfo.Text.text;
                }
                m_Text.enabled = enableText;
            }

            if(m_SortAscIndicator != null && m_SortDesIndicator != null)
            {
                switch(i_SortInfo.State)
                {
                    case ESortType.Ascending:
                        m_SortAscIndicator.enabled = true;
                        m_SortDesIndicator.enabled = false;
                        break;
                    case ESortType.Descending:
                        m_SortAscIndicator.enabled = false;
                        m_SortDesIndicator.enabled = true;
                        break;
                    default:
                        Destroy(gameObject);
                        break;
                }
            }
            else
            {
                if(m_SortAscIndicator != null)
                {
                    m_SortAscIndicator.enabled = false;
                }
                if(m_SortDesIndicator != null)
                {
                    m_SortDesIndicator.enabled = false;
                }
            }
        }

        public void Click()
        {
            if(m_SortInfo)
            {
                m_SortInfo.SetState(ESortType.None);
            }
        }
    }
}
