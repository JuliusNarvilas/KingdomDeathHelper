using Common.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Common.IO.FileHelpers
{
    /// <summary>
    /// Handle for loading a file into some object representation.
    /// </summary>
    public abstract class FileLoadHandle
    {
        /// <summary>
        /// File loading state.
        /// </summary>
        public enum EState
        {
            /// <summary>
            /// The initial state when loading has not requested.
            /// </summary>
            Initial,
            /// <summary>
            /// The loading state.
            /// </summary>
            Loading,
            /// <summary>
            /// The ready state for getting the load result.
            /// </summary>
            Ready,
            /// <summary>
            /// The errored state for failed load.
            /// </summary>
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

        /// <summary>
        /// The customisable load function.
        /// </summary>
        /// <param name="file">The file source stream.</param>
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

                ThreadPool.Instance.AddTask(LoadProcessing);
            }
        }

        public void BlockingLoad()
        {
            if (m_State == EState.Initial)
            {
                m_State = EState.Loading;
                m_absoluteFilePath = string.Format("{0}/{1}", Application.persistentDataPath, m_FilePath);

                LoadProcessing();
            }
        }

        private void LoadProcessing()
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

    }
}
