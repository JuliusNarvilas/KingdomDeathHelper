using Game.Display.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game.Display.Table
{
    public class SearchTableItemDisplay : MonoBehaviour
    {
        [SerializeField]
        private SearchTableControl m_SearchTableControl;
        [SerializeField]
        private SearchTableItemDisplayElement m_SpawningElement;

        private List<SearchTableItemDisplayElement> m_CurrentDisplayElements = new List<SearchTableItemDisplayElement>();

        public void Reset()
        {
            var oldElements = m_CurrentDisplayElements;
            int lastOldElementIndex = oldElements.Count - 1;
            m_CurrentDisplayElements = new List<SearchTableItemDisplayElement>();

            var displayRecord = m_SearchTableControl.CurrentDisplayItem.Record;

            var columnNames = displayRecord.Source.ColumnNames;
            for (int columnIndex = 0; columnIndex < columnNames.Length; ++columnIndex)
            {
                SearchTableItemDisplayElement displayElement = null;
                if (lastOldElementIndex >= 0)
                {
                    displayElement = oldElements[lastOldElementIndex];
                    displayElement.transform.SetAsLastSibling();
                    oldElements.RemoveAt(lastOldElementIndex--);
                }
                else
                {
                    displayElement = Instantiate(m_SpawningElement, transform);
                    displayElement.gameObject.SetActive(true);
                }
                m_CurrentDisplayElements.Add(displayElement);
                displayElement.ElementName.text = columnNames[columnIndex];
                displayElement.ElementData.text = displayRecord.Values[columnIndex];
            }

            for (int i = 0; i <= lastOldElementIndex; ++i)
            {
                Destroy(oldElements[i].gameObject);
            }
        }

    }
}
