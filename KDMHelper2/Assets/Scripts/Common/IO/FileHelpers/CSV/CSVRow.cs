
using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.IO.FileHelpers.CSV
{
    public class CSVRowEnumerator : IEnumerator<string>
    {
        public CSVRowEnumerator(CSVRow data)
        {
            m_Data = data;
            m_Index = -1;
        }

        private CSVRow m_Data;
        private int m_Index;

        public string Current
        {
            get { return m_Data.Values[m_Index]; }
        }

        object IEnumerator.Current
        {
            get { return m_Data.Values[m_Index]; }
        }

        public void Dispose()
        { }

        public bool MoveNext()
        {
            if ((m_Index + 1) < m_Data.Values.Length)
            {
                m_Index++;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            m_Index = -1;
        }
    }

    public struct CSVRow : IEnumerable<string>
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
            System.StringComparison comparisonType = caseSensitive ? System.StringComparison.Ordinal : System.StringComparison.OrdinalIgnoreCase;
            var columnNames = i_Source.ColumnNames;
            int columnCount = columnNames.Length;
            for (int i = 0; i < columnCount; ++i)
            {
                bool columnMatch = string.Compare(columnNames[i], i_ColumnName, comparisonType) == 0;
                if (columnMatch)
                {
                    int recordCount = i_Source.ContentRowCount;
                    for (int j = recordStartIndex; j < recordCount; ++j)
                    {
                        string[] recordValues = i_Source.GetContentRow(j);
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

        public IEnumerator<string> GetEnumerator()
        {
            return new CSVRowEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CSVRowEnumerator(this);
        }
    }
}
