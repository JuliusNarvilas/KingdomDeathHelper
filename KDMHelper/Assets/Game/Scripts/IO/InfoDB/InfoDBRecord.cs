
using System.Collections.Generic;

namespace Game.IO.InfoDB
{

    public struct InfoDBColumn
    {
        public string Name;
        public string[] Values;

        public InfoDBColumn(InfoDBSource i_Source, string i_Name)
        {
            Name = i_Name;
            Values = null;
            var columnKeys = i_Source.ColumnNames;
            for (int i = 0; i < columnKeys.Length; ++i)
            {
                if (i_Name == columnKeys[i])
                {
                    int recordCount = i_Source.ValueCount;
                    string[] columnValues = new string[recordCount];
                    for(int j = 0; j < recordCount; ++j)
                    {
                        columnValues[j] = i_Source.GetValue(j, i);
                    }
                    Values = columnValues;
                    break;
                }
            }
        }
    }

    public struct InfoDBRecord
    {
        private readonly InfoDBSource m_Source;
        public string[] Values;
        public int Index;

        public InfoDBRecord(InfoDBSource i_Source, string i_ColumnName, string i_ValueMatch)
        {
            m_Source = i_Source;
            Values = null;
            Index = -1;

            var columnNames = i_Source.ColumnNames;
            int columnCount = columnNames.Length;
            for (int i = 0; i < columnCount; ++i)
            {
                if (i_ColumnName == columnNames[i])
                {
                    int recordCount = i_Source.ValueCount;
                    for (int j = 0; j < recordCount; ++j)
                    {
                        if (i_ValueMatch == i_Source.GetValue(j, i))
                        {
                            Values = i_Source.GetValue(j);
                            Index = j;
                            break;
                        }
                    }
                    break;
                }
            }
        }

        public InfoDBRecord(InfoDBSource i_Source, string[] i_Values, int i_Index)
        {
            m_Source = i_Source;
            Values = i_Values;
            Index = i_Index;
        }

        public string GetColumn(string key)
        {
            var columnKeys = m_Source.ColumnNames;
            for (int i = 0; i < columnKeys.Length; ++i)
            {
                if (key == columnKeys[i])
                {
                    return Values[i];
                }
            }
            return null;
        }
    }
}
