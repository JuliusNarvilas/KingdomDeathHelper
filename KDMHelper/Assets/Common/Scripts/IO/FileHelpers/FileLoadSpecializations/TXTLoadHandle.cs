using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Assets.Common.IO.FileHelpers.FileLoadSpecializations
{
    public class TXTLoadHandle : FileLoadHandle
    {
        private string m_Result;

        public override object GetResult()
        {
            return m_Result;
        }
        public string GetResultTxt()
        {
            return m_Result;
        }


        protected override void LoadFunc(Stream file)
        {
            using (var streamReader = new StreamReader(file))
            {
                m_Result = streamReader.ReadToEnd();
            }
        }
    }
}
