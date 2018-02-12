using Common;
using Common.Display.Table;
using Game.Model.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Display.Table
{

    public class SurvivorsTableItem : MonoBehaviour
    {
        [Serializable]
        private struct SurvivorsTableItemElementConfig
        {
            public ESurvivorsInfoType InfoType;
            public TableItemElement Element;
        }

        
        [SerializeField]
        private List<SurvivorsTableItemElementConfig> m_ItemElements;

        private TableItemElement[] m_ElementsByType;

        private Coroutine m_DisplayElementsCoroutine;

        private void Awake()
        {
            m_ElementsByType = new TableItemElement[SurvivorsTableSortControl.InfoTypeCount];
            int givenElementCount = m_ItemElements.Count;
            for (int i = 0; i < givenElementCount; ++i)
            {
                var element = m_ItemElements[i];
                int typeIndex = (int)element.InfoType;
                if(typeIndex >= 0 && typeIndex < SurvivorsTableSortControl.InfoTypeCount)
                {
                    m_ElementsByType[typeIndex] = element.Element;
                }
                else
                {
                    Log.ProductionLogError("Invalid Survivor Sort Info type.");
                }
            }
        }

        public void DisplayElements(List<TableSortInfo> visibleSortInfo)
        {
            if(m_DisplayElementsCoroutine != null)
            {
                StopCoroutine(m_DisplayElementsCoroutine);
                m_DisplayElementsCoroutine = null;
            }
            m_DisplayElementsCoroutine = StartCoroutine(DisplayElementsWhenReady(visibleSortInfo));
        }

        public void Set(Survivor i_Survivor)
        {
            int count = m_ItemElements.Count;
            for(int i = 0; i < count; ++i)
            {
                string displayValue = i_Survivor.GetValue(m_ItemElements[i].InfoType);
                m_ItemElements[i].Element.SetValue(displayValue);
            }
        }

        private IEnumerator DisplayElementsWhenReady(List<TableSortInfo> visibleSortInfo)
        {
            while(m_ElementsByType == null)
            {
                yield return null;
            }

            int count = m_ItemElements.Count;
            for (int i = 0; i < count; ++i)
            {
                var elementRecord = m_ItemElements[i];
                int elementType = (int)elementRecord.InfoType;
                if (visibleSortInfo.FirstOrDefault(x => (int)x.Data == elementType) == null)
                {
                    elementRecord.Element.gameObject.SetActive(false);
                }
            }
            int visibleCount = visibleSortInfo.Count;
            for (int i = 0; i < visibleCount; ++i)
            {
                var info = visibleSortInfo[i];
                var element = m_ElementsByType[(int)info.Data];
                element.gameObject.SetActive(true);
                //setting the right display position based on the visible information list order
                element.transform.SetSiblingIndex(i);
            }
            m_DisplayElementsCoroutine = null;
        }
    }
}
