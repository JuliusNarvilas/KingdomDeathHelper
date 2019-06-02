using Common.Scripts.IO;
using Common.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Assets.Game.Scripts.IO
{
    /// <summary>
    /// Allows to provide a generic output target for sharing and serialisation code.
    /// Aim is to not be concearned if the output is a file, database or bluetooth device when trying to output data.
    /// </summary>
    public interface IOutputTarget
    {
        IEnumerator Process(Stream stream, string name, string type);
    }

    public class OutputTargetLocalFile : ScriptableObject, IOutputTarget
    {
        [SerializeField]
        private string m_subPath;
        [SerializeField]
        private bool m_processInEditor = false;
        [SerializeField]
        private bool m_useThreadingIfAble = true;

        public IEnumerator Process(Stream stream, string name, string type)
        {
#if !ENABLE_THREADING
            if (m_useThreadingIfAble)
            {
                IThreadPoolTaskHandle m_SaveTaskHandle = ThreadPool.Instance.AddTask(() => { InternalProcessFunc(stream, name, type); }, TimeSpan.FromMinutes(5));

                while (m_SaveTaskHandle.State <= EThreadedTaskState.InProgress)
                {
                    yield return null;
                }
            }
            else
            {
                InternalProcessFunc(stream, name, type);
            }
#else
            InternalProcessFunc(stream, name, type);
            yield break;
#endif
        }


        private void InternalProcessFunc(Stream stream, string name, string type)
        {
            string directory = Application.persistentDataPath;
            if (!string.IsNullOrEmpty(m_subPath))
            {
                directory = Path.Combine(directory, m_subPath);
            }
            string fullFilePath = Path.Combine(directory, string.Format("{0}.{1}", name, type));

#if UNITY_EDITOR
            if (m_processInEditor)
#endif
            {
                if (directory.EndsWith(Path.PathSeparator.ToString()))
                {
                    directory = directory.Substring(0, directory.Length - 1);
                }
                DirectoryInfo target = new DirectoryInfo(directory);
                if (!target.Exists)
                {
                    target.Create();
                }

                using (var fileStream = new FileStream(fullFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    IOHelpers.CopyStream(stream, fileStream);
                }
            }
        }
    }
}
