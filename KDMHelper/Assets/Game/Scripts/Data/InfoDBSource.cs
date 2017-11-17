using Common.IO;
using Common.Threading;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    
    [Serializable]
    public class InfoDBSource
    {
        public enum EState
        {
            Initial,
            Loading,
            Parsing,
            Ready
        }

        public string Name;
        public AssetReference InfoCSVRef;
        private AssetReferenceLoadHandle m_LoadHandle;
        [NonSerialized]
        private EState m_State = EState.Initial;
        public EState State { get { return m_State; } }
        private string m_Error;
        public string Error { get { return m_Error; } }

        private string[] m_ColumnNames;
        public string[] ColumnNames { get { return m_ColumnNames; } }
        private InfoDBColumn[] m_Columns;
        private IThreadPoolTaskHandle m_LoadTaskHandle;

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

        public void LoadingFunc()
        {
            while (!m_LoadHandle.IsDone())
            {
                System.Threading.Thread.Sleep(20);
            }

            m_Error = "Asset Not Found";
            var asset = m_LoadHandle.GetAsset<TextAsset>();
            if (asset != null)
            {
                string assetText = null;
                AssetReferenceUpdateRunner.Instance.AddAction(() => { assetText = asset.text; });

                while(assetText == null)
                {
                    System.Threading.Thread.Sleep(20);
                }

                m_State = EState.Parsing;
                fgCSVReader.LoadFromString(assetText, new fgCSVReader.ReadLineDelegate(ReadLineFunc));
                bool goodParse = m_Columns != null && m_Columns.Length > 0 && m_Columns[0].Content.Count > 0;
                if (!goodParse)
                {
                    m_Error = "Asset Data Invalid (Parse Error)!";
                }
                else
                {
                    m_Error = null;
                }
            }
            m_State = EState.Ready;
        }

        public void Load()
        {
            if (State == EState.Initial)
            {
                m_State = EState.Loading;
                m_LoadHandle = InfoCSVRef.LoadAsset();
                m_LoadTaskHandle = ThreadPool.Instance.AddTask(LoadingFunc);
            }
        }

        private void ReadLineFunc(int line_index, List<string> line)
        {
            if (line_index == 0)
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
