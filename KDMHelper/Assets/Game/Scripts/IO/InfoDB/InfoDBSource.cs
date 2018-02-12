using Common;
using Common.Helpers;
using Common.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Game.IO.InfoDB
{
    
    [Serializable]
    public class InfoDBSource
    {
        public enum EState
        {
            Initial,
            Loading,
            Parsing,
            Ready,
            Errored
        }
        
        public string Name;
        [SerializeField]
        private TextAsset m_DefaultContentAsset;
        private string m_FinalReadContent;

        [NonSerialized]
        private EState m_State = EState.Initial;
        public EState State { get { return m_State; } }
        private string m_Error;
        public string Error { get { return m_Error; } }

        private int m_VersionNumber;
        private bool m_Dirty = false;

        private string[] m_ColumnNames;
        public string[] ColumnNames { get { return m_ColumnNames; } }
        private List<string[]> m_Values;
        public int ValueCount { get { return m_Values.Count; } }
        public string[] GetValue(int i_Index) { return ToSafeRecord(m_Values[i_Index]); }
        public string GetValue(int i_Index, int i_ColumnIndex) { return m_Values[i_Index][i_ColumnIndex]; }

        private object m_ReadWriteLock = new object();
        private IThreadPoolTaskHandle m_LoadTaskHandle;
        private IThreadPoolTaskHandle m_SaveTaskHandle;
        public EThreadedTaskState LoadTaskState { get { return m_LoadTaskHandle == null ? EThreadedTaskState.Succeeded : m_LoadTaskHandle.State; } }
        public EThreadedTaskState SaveTaskState { get { return m_SaveTaskHandle == null ? EThreadedTaskState.Succeeded : m_SaveTaskHandle.State; } }


        private Coroutine m_SaveAwaiter = null;
        private Coroutine m_LoadAwaiter = null;

        public void Reset()
        {
            m_Dirty = false;
            m_FinalReadContent = null;
            m_State = EState.Initial;
            m_ColumnNames = null;
            m_Values = null;
            m_LoadTaskHandle = null;
        }

        public void SetValues(int i_Index, string[] i_NewValues)
        {
            m_Dirty = true;
            m_Values[i_Index] = ToSafeRecord(i_NewValues);
        }

        public void AddValues(string[] i_NewValues)
        {
            m_Dirty = true;
            m_Values.Add(ToSafeRecord(i_NewValues));
        }

        private string[] ToSafeRecord(string[] i_NewValues)
        {
            string[] safeRecord = new string[m_ColumnNames.Length];
            int minCount = Mathf.Min(i_NewValues.Length, m_ColumnNames.Length);
            Array.Copy(i_NewValues, safeRecord, minCount);
            
            for (int i = minCount; i < m_ColumnNames.Length; ++i)
            {
                safeRecord[i] = string.Empty;
            }
            return safeRecord;
        }

        public InfoDBColumn GetColumn(string i_Name)
        {
            return  new InfoDBColumn(this, i_Name);
        }

        public InfoDBRecord Find(string i_ColumnName, string i_ValueMatch, bool caseSensitive = false, bool containsMatch = false)
        {
            return InfoDBRecord.FindRecord(this, i_ColumnName, i_ValueMatch, caseSensitive, containsMatch);
        }

        public List<InfoDBRecord> FindAll(string i_ColumnName, string i_ValueMatch, bool caseSensitive = false, bool containsMatch = false)
        {
            var result = new List<InfoDBRecord>();
            InfoDBRecord currentRecord = new InfoDBRecord(null, null, -1);
            while(true)
            {
                currentRecord = InfoDBRecord.FindRecord(this, i_ColumnName, i_ValueMatch, caseSensitive, containsMatch, currentRecord.Index + 1);
                if(currentRecord.Index >= 0)
                {
                    result.Add(currentRecord);
                }
                else
                {
                    break;
                }
            }
            return result;
        }

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

        public void LoadingFunc()
        {
            lock (m_ReadWriteLock)
            {
                int m_VersionNumber = 0;
                int externalVersionNumber = 0;
                string externalAssetText = null;

                if (!TryGetVersionNumber(m_FinalReadContent, out m_VersionNumber))
                {
                    m_Error = "Asset Data Invalid (Parse Error)!";
                    m_State = EState.Errored;
                    return;
                }

                string externalFilePath = string.Format("{0}/{1}.csv", Application.persistentDataPath, m_DefaultContentAsset.name);

                if (File.Exists(externalFilePath))
                {
#if !UNITY_EDITOR
                externalAssetText = File.ReadAllText(externalFilePath);
                if(!TryGetVersionNumber(externalAssetText, out externalVersionNumber))
                {
                    m_Error = "Asset Data Invalid (Parse Error)!";
                    m_State = EState.Errored;
                    return;
                }
#endif
                }

                if (externalVersionNumber < 0 || externalVersionNumber >= m_VersionNumber)
                {
                    m_FinalReadContent = externalAssetText;
                    m_VersionNumber = externalVersionNumber;
                }
                else
                {
#if !UNITY_EDITOR
                int lastSeperator = externalFilePath.LastIndexOf('/');
                var directory = externalFilePath.Substring(0, lastSeperator);
                DirectoryInfo target = new DirectoryInfo(directory);
                if(!target.Exists)
                {
                    target.Create();
                }
                m_FinalReadContent = File.ReadAllText(externalFilePath);
#endif
                }

                m_State = EState.Parsing;
                fgCSVReader.LoadFromString(m_FinalReadContent, new fgCSVReader.ReadLineDelegate(ReadLineFunc));

                /*
                bool goodParse = m_Columns != null && m_Columns.Length > 0 && m_Columns[0].Content.Count > 0;
                if (!goodParse)
                {
                    m_Error = "Asset Data Invalid (Parse Error)!";
                    m_State = EState.Errored;
                    return;
                }
                */

                Log.DebugLog("Finished loading {0}.", externalFilePath);

                m_FinalReadContent = null;
                m_Error = null;
                m_State = EState.Ready;
            }
        }

        private void SavingFunc()
        {
            lock(m_ReadWriteLock)
            {
                StringBuilder writeContent = new StringBuilder();
                writeContent.Length = 0;
                writeContent.Append(m_VersionNumber);
                writeContent.Append("\n");

                int lastColumnIndex = m_ColumnNames.Length - 1;
                for (int i = 0; i < lastColumnIndex; ++i)
                {
                    writeContent.Append(StringHelper.StringToCSVCell(m_ColumnNames[i]));
                    writeContent.Append(", ");
                }
                writeContent.Append(StringHelper.StringToCSVCell(m_ColumnNames[lastColumnIndex]));
                writeContent.Append('\n');

                int valueCount = m_Values.Count;
                for (int i = 0; i < valueCount; ++i)
                {
                    for (int j = 0; i < lastColumnIndex; ++j)
                    {
                        writeContent.Append(StringHelper.StringToCSVCell(m_Values[i][j]));
                        writeContent.Append(", ");
                    }
                    writeContent.Append(StringHelper.StringToCSVCell(m_Values[i][lastColumnIndex]));
                    writeContent.Append('\n');
                }

                string externalFilePath = string.Format("{0}/{1}.csv", Application.persistentDataPath, m_DefaultContentAsset.name);

                //only actually save in non-editor mode
#if !UNITY_EDITOR
                int lastSeperator = externalFilePath.LastIndexOf('/');
                var directory = externalFilePath.Substring(0, lastSeperator);
                DirectoryInfo target = new DirectoryInfo(directory);
                if(!target.Exists)
                {
                    target.Create();
                }
                File.WriteAllText(externalFilePath, m_WriteContent.ToString());
#endif
            }
        }
        
        public void Load()
        {
            if (m_LoadAwaiter == null)
            {
                m_LoadAwaiter = ApplicationManager.Instance.StartCoroutine(LoadAwaiter());
            }
        }

        public IEnumerator LoadAwaiter()
        {

            if (State == EState.Initial)
            {
                if (m_DefaultContentAsset == null)
                {
                    m_State = EState.Errored;
                    m_Error = "Asset Not Found";
                    m_LoadAwaiter = null;
                    yield break;
                }
                
                //loading already about to start
                if (m_LoadTaskHandle != null && m_LoadTaskHandle.State <= EThreadedTaskState.InProgress)
                {
                    m_LoadAwaiter = null;
                    yield break;
                }

                //wait for saving to finish
                while (m_SaveAwaiter != null || (m_SaveTaskHandle != null && m_SaveTaskHandle.State <= EThreadedTaskState.InProgress))
                {
                    yield return null;
                }

                m_FinalReadContent = m_DefaultContentAsset.text;
                m_LoadTaskHandle = ThreadPool.Instance.AddTask(LoadingFunc, TimeSpan.FromMinutes(5), System.Threading.ThreadPriority.Highest);
            }
            m_LoadAwaiter = null;
        }


        public void Save()
        {
            if(m_SaveAwaiter == null)
            {
                m_SaveAwaiter = ApplicationManager.Instance.StartCoroutine(SaveAwaiter());
            }
        }

        public IEnumerator SaveAwaiter()
        {
            //already about to save
            if (m_SaveTaskHandle != null && m_SaveTaskHandle.State == EThreadedTaskState.Awaiting)
            {
                m_SaveAwaiter = null;
                yield break;
            }
            //loading in progress - saving is invalid
            if (m_LoadAwaiter != null || (m_LoadTaskHandle != null && m_LoadTaskHandle.State <= EThreadedTaskState.InProgress))
            {
                m_SaveAwaiter = null;
                yield break;
            }

            //wait for old save to finish
            while (m_SaveTaskHandle != null && m_SaveTaskHandle.State <= EThreadedTaskState.InProgress)
            {
                yield return null;
            }

            m_SaveTaskHandle = ThreadPool.Instance.AddTask(SavingFunc, TimeSpan.FromMinutes(5), System.Threading.ThreadPriority.Highest);
            m_SaveAwaiter = null;
        }

        public void SaveIfDirty()
        {
            if(m_Dirty)
            {
                m_Dirty = false;
                Save();
            }
        }

        private void ReadLineFunc(int i_LineIndex, List<string> i_Line)
        {
            if(i_LineIndex > 1)
            {
                for (int i = i_Line.Count; i < m_ColumnNames.Length; ++i)
                {
                    i_Line.Add(string.Empty);
                }
                m_Values.Add(i_Line.ToArray());
            }
            else if (i_LineIndex == 1)
            {
                m_ColumnNames = i_Line.ToArray();
                m_Values = new List<string[]>();
            }
            //skip line index 0 (version number)
        }

    }
}
