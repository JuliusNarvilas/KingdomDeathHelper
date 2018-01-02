using Common.Display.Transition;
using Game.IO.InfoDB;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Display.Table
{
    public class SearchTableControl : MonoBehaviour
    {
        [SerializeField]
        private SearchTableItem m_SpawningTableItem;
        [SerializeField]
        private UnityEngine.Events.UnityEvent m_OnDisplayItem;

        [NonSerialized]
        public SearchTableItem CurrentDisplayItem;

        private List<SearchTableItem> m_CurrentItems = new List<SearchTableItem>();

        public void PopulateSearchTable(string i_Name)
        {
            //reuse old table items
            var oldTableItems = m_CurrentItems;
            int lastOldRecordIndex = oldTableItems.Count - 1;
            m_CurrentItems = new List<SearchTableItem>();

            var sources = ApplicationManager.Instance.InfoDB.Sources;
            
            int count = sources.Count;
            for (int sourceIndex = 0; sourceIndex < count; ++sourceIndex)
            {
                var source = sources[sourceIndex];
                var matchedSourceRecords = source.FindAll("Name", i_Name, false, true);

                int recordCount = matchedSourceRecords.Count;
                if (recordCount > 0)
                {
                    string[] columnNames = source.ColumnNames;
                    int nameColumnIndex = 0;
                    for(int i = 0; i < columnNames.Length; ++i)
                    {
                        if(string.Compare("Name", columnNames[i], StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            nameColumnIndex = i;
                            break;
                        }
                    }

                    for (int recordIndex = 0; recordIndex < recordCount; ++recordIndex)
                    {
                        var currentRecord = matchedSourceRecords[recordIndex];
                        SearchTableItem item = null;
                        if (lastOldRecordIndex >= 0)
                        {
                            item = oldTableItems[lastOldRecordIndex];
                            item.transform.SetAsLastSibling();
                            oldTableItems.RemoveAt(lastOldRecordIndex--);
                        }
                        else
                        {
                            item = Instantiate(m_SpawningTableItem, transform);
                            item.gameObject.SetActive(true);
                        }
                        item.Set(this, currentRecord.Values[nameColumnIndex], currentRecord);
                        m_CurrentItems.Add(item);
                    }
                }
            }

            // cleanup unused old table items
            for (int i = 0; i <= lastOldRecordIndex; ++i)
            {
                Destroy(oldTableItems[i].gameObject);
            }
        }



        public void DisplayItem(SearchTableItem i_Item)
        {
            CurrentDisplayItem = i_Item;
            if(m_OnDisplayItem != null)
            {
                m_OnDisplayItem.Invoke();
            }
        }
    }
}
