using Common;
using Common.Threading;
using System;
using System.Collections.Generic;
using System.IO;
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
        private string m_FinalContent;
        private string m_ExternalFilePath;

        [NonSerialized]
        private EState m_State = EState.Initial;
        public EState State { get { return m_State; } }
        private string m_Error;
        public string Error { get { return m_Error; } }

        private string[] m_ColumnNames;
        public string[] ColumnNames { get { return m_ColumnNames; } }
        private InfoDBColumn[] m_Columns;
        private IThreadPoolTaskHandle m_LoadTaskHandle;


        public void Reset()
        {
            m_FinalContent = null;
            m_ExternalFilePath = null;
            m_State = EState.Initial;
            m_ColumnNames = null;
            m_Columns = null;
            m_LoadTaskHandle = null;
        }

        public InfoDBColumn GetColumn(string name)
        {
            for (int i = 0; i < m_Columns.Length; ++i)
            {
                var result = m_Columns[i];
                if (name == result.Name)
                {
                    return result;
                }
            }
            return null;
        }

        public InfoDBRecord Find(string columnName, string valueMatch)
        {
            var column = GetColumn(columnName);
            if (column != null)
            {
                int size = column.Content.Count;
                for (int i = 0; i < size; ++i)
                {
                    if (column.Content[i] == valueMatch)
                    {
                        string[] values = new string[m_ColumnNames.Length];
                        for (int j = 0; j < m_ColumnNames.Length; ++j)
                        {
                            values[j] = m_Columns[j].Content[i];
                        }
                        return new InfoDBRecord(m_ColumnNames, values);
                    }
                }
            }
            return null;
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
            int defaultVersionNumber = 0;
            int externalVersionNumber = 0;
            string externalAssetText = null;

            if (!TryGetVersionNumber(m_FinalContent, out defaultVersionNumber))
            {
                m_Error = "Asset Data Invalid (Parse Error)!";
                m_State = EState.Errored;
                return;
            }

            if (File.Exists(m_ExternalFilePath))
            {
#if !UNITY_EDITOR
                externalAssetText = File.ReadAllText(m_ExternalFilePath);
                if(!TryGetVersionNumber(externalAssetText, out externalVersionNumber))
                {
                    m_Error = "Asset Data Invalid (Parse Error)!";
                    m_State = EState.Errored;
                    return;
                }
#endif
            }

            if (externalVersionNumber < 0 || externalVersionNumber >= defaultVersionNumber)
            {
                m_FinalContent = externalAssetText;
            }
            else
            {
#if !UNITY_EDITOR
                int lastSeperator = m_ExternalFilePath.LastIndexOf('/');
                var directory = m_ExternalFilePath.Substring(0, lastSeperator);
                DirectoryInfo target = new DirectoryInfo(directory);
                if(!target.Exists)
                {
                    target.Create();
                }
                File.WriteAllText(m_ExternalFilePath, m_FinalContent);
#endif
            }

            m_State = EState.Parsing;
            fgCSVReader.LoadFromString(m_FinalContent, new fgCSVReader.ReadLineDelegate(ReadLineFunc));
            bool goodParse = m_Columns != null && m_Columns.Length > 0 && m_Columns[0].Content.Count > 0;
            if (!goodParse)
            {
                m_Error = "Asset Data Invalid (Parse Error)!";
                m_State = EState.Errored;
                return;
            }

            Log.DebugLog("Finished loading {0}.", m_ExternalFilePath);

            m_FinalContent = null;
            m_Error = null;
            m_State = EState.Ready;
        }

        
        public void Load()
        {
            if (State == EState.Initial)
            {
                if(m_DefaultContentAsset == null)
                {
                    m_State = EState.Errored;
                    m_Error = "Asset Not Found";
                    return;
                }

                m_State = EState.Loading;
                m_ExternalFilePath = string.Format("{0}/{1}.csv", Application.persistentDataPath, m_DefaultContentAsset.name);
                m_FinalContent = m_DefaultContentAsset.text;
                m_LoadTaskHandle = ThreadPool.Instance.AddTask(LoadingFunc);
            }
        }

        private void ReadLineFunc(int line_index, List<string> line)
        {
            if(line_index == 0)
            {
                //skip version number
            }
            else if (line_index == 1)
            {
                m_ColumnNames = line.ToArray();
                m_Columns = new InfoDBColumn[m_ColumnNames.Length];
                for (int i = 0; i < m_ColumnNames.Length; ++i)
                {
                    m_Columns[i] = new InfoDBColumn()
                    {
                        Name = m_ColumnNames[i],
                        Content = new List<string>()
                    };
                }
            }
            else
            {
                for (int i = 0; i < m_ColumnNames.Length; ++i)
                {
                    if (i < line.Count)
                    {
                        m_Columns[i].Content.Add(line[i]);
                    }
                    else
                    {
                        m_Columns[i].Content.Add(string.Empty);
                    }
                }
            }
        }

    }
}
