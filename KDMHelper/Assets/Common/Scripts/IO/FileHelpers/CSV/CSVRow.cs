using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Common.IO.FileHelpers.CSV
{

    public struct CSVRow
    {
        public CSVData Source;
        public string[] Values;
        public int Index;

        public static CSVRow FindRow(CSVData i_Source, string i_ColumnName, string i_ValueMatch, bool caseSensitive, bool containsMatch, int recordStartIndex = 0)
        {
            var result = new CSVRow(i_Source, null, -1);

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

        public CSVRow(CSVData i_Source, string[] i_Values, int i_Index)
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

        public string GetColumn(int colIndex)
        {
            if (colIndex < 0)
                return null;

            var columnKeys = Source.ColumnNames;
            if (colIndex >= columnKeys.Length)
                return null;

            return Values[colIndex];
        }

    }
}
