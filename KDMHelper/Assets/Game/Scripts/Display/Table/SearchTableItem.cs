using Game.IO.InfoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Display.Table
{
    public class SearchTableItem : MonoBehaviour
    {
        [SerializeField]
        private Text m_NameText;
        [SerializeField]
        private Text m_SourceText;

        private SearchTableControl m_SearchTableControl;

        public InfoDBRecord Record;


        public void Set(SearchTableControl control, string i_Name, InfoDBRecord i_Record)
        {
            m_SearchTableControl = control;
            m_NameText.text = i_Name;
            m_SourceText.text = i_Record.Source.Name;
            Record = i_Record;
        }

        public void DisplaySearchItem()
        {
            m_SearchTableControl.DisplayItem(this);
        }
    }
}
