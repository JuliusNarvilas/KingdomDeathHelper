using Common;
using Common.Display.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Display.Table
{

    public class SurvivorTableItem : MonoBehaviour
    {
        [Serializable]
        private struct SurvivorTableItemElementConfig
        {
            public ESurvivorSortInfoType InfoType;
            public TableItemElement Element;
        }

        
        [SerializeField]
        private List<SurvivorTableItemElementConfig> m_ItemElements;

        private TableItemElement[] m_ElementArray;

        private void Awake()
        {
            m_ElementArray = new TableItemElement[SurvivorTableSortControl.InfoTypeCount];
            int givenElementCount = m_ItemElements.Count;
            for (int i = 0; i < givenElementCount; ++i)
            {
                var element = m_ItemElements[i];
                int index = (int)element.InfoType;
                if(index >= 0 && index < SurvivorTableSortControl.InfoTypeCount)
                {
                    m_ElementArray[i] = element.Element;
                }
                else
                {
                    Log.ProductionLogError("Invalid Survivor Sort Info type.");
                }
            }
        }


    }
}
