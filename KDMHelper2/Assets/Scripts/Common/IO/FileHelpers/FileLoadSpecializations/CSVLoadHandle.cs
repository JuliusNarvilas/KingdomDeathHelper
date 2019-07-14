using Common.IO.FileHelpers.CSV;
using System.Collections.Generic;
using System.IO;

namespace Common.IO.FileHelpers.FileLoadSpecializations
{
    public class CSVLoadHandle : FileLoadHandle
    {
        protected CSVData m_Result;

        public CSVLoadHandle(string filePath) : base(filePath)
        { }


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
                m_Result = CSVData.Parse(content);
            }
        }
    }
}
