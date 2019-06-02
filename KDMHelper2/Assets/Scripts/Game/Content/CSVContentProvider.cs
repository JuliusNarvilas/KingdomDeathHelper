using Common.IO.FileHelpers.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Content
{
    class CSVRowContentProvider : IContentProvider
    {
        public CSVRowContentProvider(CSVRow data)
        {
            m_Data = data;
        }

        public CSVRow m_Data;

        public object GetContent(string key)
        {
            return m_Data.GetColumn(key);
        }
    }

    class CSVContentProvider : IContentSourceProvider
    {
        public CSVContentProvider(CSVData data)
        {
            m_Data = data;
        }
        
        public CSVData m_Data;


        public IContentProvider GetContentProvider(string key, string name = null)
        {
            if (name == null)
                return null;

            CSVRow row = m_Data.Find(name, key, true);
            if (row.Index >= 0)
            {
                return new CSVRowContentProvider(row);
            }
            return null;
        }
    }
}
