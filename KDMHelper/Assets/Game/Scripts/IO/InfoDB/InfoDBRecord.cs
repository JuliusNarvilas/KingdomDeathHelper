
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
        public InfoDBSource Source;
        public string[] Values;
        public int Index;

        public static InfoDBRecord FindRecord(InfoDBSource i_Source, string i_ColumnName, string i_ValueMatch, bool caseSensitive, bool containsMatch, int recordStartIndex = 0)
        {
            var result = new InfoDBRecord(i_Source, null, -1);
            
            if (recordStartIndex < 0)
            {
                recordStartIndex = 0;
            }
            System.StringComparison comparisonType = caseSensitive ? System.StringComparison.InvariantCulture : System.StringComparison.InvariantCultureIgnoreCase;
            var columnNames = i_Source.ColumnNames;
            int columnCount = columnNames.Length;
            for (int i = 0; i < columnCount; ++i)
            {
                bool columnMatch = string.Compare(columnNames[i], i_ColumnName, comparisonType) == 0;
                if (columnMatch)
                {
                    int recordCount = i_Source.ValueCount;
                    for (int j = recordStartIndex; j < recordCount; ++j)
                    {
                        string[] recordValues = i_Source.GetValue(j);
                        string value = recordValues[i];
                        bool foundRecord = false;
                        if (containsMatch)
                        {
                            foundRecord = value.IndexOf(i_ValueMatch, comparisonType) >= 0;
                        }
                        else
                        {
                            foundRecord = string.Compare(value, i_ValueMatch, comparisonType) == 0;
                        }
                        if (foundRecord)
                        {
                            result.Values = recordValues;
                            result.Index = j;
                            break;
                        }
                    }
                    break;
                }
            }

            return result;
        }

        public InfoDBRecord(InfoDBSource i_Source, string[] i_Values, int i_Index)
        {
            Source = i_Source;
            Values = i_Values;
            Index = i_Index;
        }

        public string GetColumn(string key)
        {
            var columnKeys = Source.ColumnNames;
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
