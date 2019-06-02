﻿using Common.IO.FileHelpers.CSV;
using Common.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common.IO.FileHelpers.FileLoadSpecializations
{
    public class XMLLoadHandleT<T> : XMLLoadHandle where T : class
    {
        public XMLLoadHandleT(string filePath) : base(typeof(T), filePath) { }

        public T GetResultT()
        {
            return m_Result as T;
        }
    }

    public class XMLLoadHandle : FileLoadHandle
    {
        protected Type m_resultType;
        protected object m_Result;

        public override object GetResult()
        {
            return m_Result;
        }
        public T GetResultT<T>() where T : class
        {
            return m_Result as T;
        }
        
        public XMLLoadHandle(Type resultType, string filePath) : base(filePath)
        {
            m_resultType = resultType;
        }

        protected override void LoadFunc(Stream file)
        {
            m_Result = XMLHelpers.Deserialise(file, m_resultType);
        }

    }
}
