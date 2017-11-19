
using System.Collections.Generic;

namespace Game.IO.InfoDB
{
    public class InfoDBColumn
    {
        public string Name;
        public List<string> Content;
    }


    public class InfoDBRecord
    {
        private string[] m_ColumnKeys;
        public string[] Values;

        public InfoDBRecord(string[] columnKeys, string[] values)
        {
            m_ColumnKeys = columnKeys;
            Values = values;
        }

        public string GetColumn(string key)
        {
            for (int i = 0; i < m_ColumnKeys.Length; ++i)
            {
                if (key == m_ColumnKeys[i])
                {
                    return Values[i];
                }
            }
            return null;
        }
    }
}
