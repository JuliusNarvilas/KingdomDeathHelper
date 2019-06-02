using Assets.Common.IO.FileHelpers.CSV;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Assets.Common.IO.FileHelpers.FileLoadSpecializations
{
    public class CSVLoadHandle : FileLoadHandle
    {
        protected CSVData m_Result;

        public override object GetResult()
        {
            return m_Result;
        }
        public CSVData GetResultCSV()
        {
            return m_Result;
        }

        protected override void LoadFunc(Stream file)
        {
            using (var streamReader = new StreamReader(file))
            {
                string content = streamReader.ReadToEnd();
                fgCSVReader.LoadFromString(content, new fgCSVReader.ReadLineDelegate(ReadLineFunc));
            }
        }

        private void ReadLineFunc(int i_LineIndex, List<string> i_Line)
        {
            if (i_LineIndex == 0)
            {
                m_Result = new CSVData(m_FilePath, i_Line.ToArray());
            }
            else
            {
                m_Result.AddValues(i_Line.ToArray());
            }
        }
    }
}
