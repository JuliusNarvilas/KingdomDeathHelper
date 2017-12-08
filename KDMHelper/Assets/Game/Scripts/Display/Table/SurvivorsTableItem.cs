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

    public class SurvivorsTableItem : MonoBehaviour
    {
        [Serializable]
        private struct SurvivorsTableItemElementConfig
        {
            public ESurvivorsSortInfoType InfoType;
            public TableItemElement Element;
        }

        
        [SerializeField]
        private List<SurvivorsTableItemElementConfig> m_ItemElements;

        private TableItemElement[] m_ElementArray;

        private void Awake()
        {
            m_ElementArray = new TableItemElement[SurvivorsTableSortControl.InfoTypeCount];
            int givenElementCount = m_ItemElements.Count;
            for (int i = 0; i < givenElementCount; ++i)
            {
                var element = m_ItemElements[i];
                int index = (int)element.InfoType;
                if(index >= 0 && index < SurvivorsTableSortControl.InfoTypeCount)
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
