using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IO.FileHelpers.CSV
{
    public struct CSVColumn
    {
        public CSVData Source;
        public string Name;
        public string[] Values;

        public CSVColumn(CSVData i_Source, string i_Name)
        {
            Source = i_Source;
            Name = i_Name;
            Values = null;
            var columnKeys = i_Source.ColumnNames;
            for (int i = 0; i < columnKeys.Length; ++i)
            {
                if (i_Name == columnKeys[i])
                {
                    int recordCount = i_Source.ContentRowCount;
                    string[] columnValues = new string[recordCount];
                    for (int j = 0; j < recordCount; ++j)
                    {
                        columnValues[j] = i_Source.GetContentCell(j, i);
                    }
                    Values = columnValues;
                    break;
                }
            }
        }
    }

}
