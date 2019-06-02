using Common.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Common.IO.FileHelpers
{
    public abstract class FileLoadHandle
    {
        public enum EState
        {
            Initial,
            Loading,
            Ready,
            Errored
        }

        protected EState m_State = EState.Initial;
        public EState State { get { return m_State; } }

        private string m_absoluteFilePath;

        protected string m_FilePath;
        public string FilePath { get { return m_FilePath; } }

        public abstract object GetResult();

        protected string m_Error;
        public string Error { get { return m_Error; } }

        protected abstract void LoadFunc(Stream file);


        public FileLoadHandle() { }
        public FileLoadHandle(string filePath)
        {
            m_FilePath = filePath;
        }

        public void AsyncLoad()
        {
            if (m_State == EState.Initial)
            {
                m_State = EState.Loading;
                m_absoluteFilePath = string.Format("{0}/{1}", Application.persistentDataPath, m_FilePath);

                ThreadPool.Instance.AddTask(AsyncLoadProcessing);
            }
        }

        public void BlockingLoad()
        {
            if (m_State == EState.Initial)
            {
                m_State = EState.Loading;
                m_absoluteFilePath = string.Format("{0}/{1}", Application.persistentDataPath, m_FilePath);

                AsyncLoadProcessing();
            }
        }

        private void AsyncLoadProcessing()
        {
            if (File.Exists(m_absoluteFilePath))
            {
                try
                {
                    using (var fileStream = File.OpenRead(m_absoluteFilePath))
                    {
                        using (var bufferedStream = new BufferedStream(fileStream))
                        {
                            LoadFunc(bufferedStream);
                            if (m_State != EState.Errored)
                            {
                                m_State = EState.Ready;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    m_Error = e.Message;
                    m_State = EState.Errored;
                }
            }
            else
            {
                m_Error = string.Format("File \"{0}\" not found.", m_absoluteFilePath);
                m_State = EState.Errored;
            }
        }



        /*
        private bool TryGetVersionNumber(string fileContent, out int version)
        {
            char[] endChars = { ',', '\t', '\r', '\n' };

            version = 0;
            int lineEnd = fileContent.IndexOfAny(endChars);
            if (lineEnd >= 0)
            {
                if (int.TryParse(fileContent.Substring(0, lineEnd), out version))
                {
                    return true;
                }
            }
            m_Error = "Can not find version number in resource file.";
            return false;
        }
        */

    }
}
