using Common.IO.FileHelpers.FileLoadSpecializations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Common.IO.ContentProvider
{
    [Serializable]
    public class ContentProvider
    {
        public enum EState
        {
            Initial,
            Configured,
            Loaded,
            Ready
        }

        [SerializeField]
        private TextAsset m_DefaultSettingsFile;


        private ContentSettings m_DefaultSettings;
        private ContentSettings m_CurrentSettings;
        private string m_CurrentLocalizationId;
        private Dictionary<string, ContentInstance> m_CurrentContent = new Dictionary<string, ContentInstance>();

        public void Configure(string settingsFilePath)
        {
            XMLLoadHandle settingsLoadHandle = new XMLLoadHandle(typeof(ContentSettings), settingsFilePath);
            settingsLoadHandle.BlockingLoad();
           

        }

        //Refresh the settings in case they were changed
        public void Refresh()
        {
            string localisationId = m_CurrentLocalizationId;
            Clear();

        }

        public void Clear()
        {
            m_CurrentLocalizationId = null;
            m_CurrentContent.Clear();
        }

        public void SetLocalization(string localizationId)
        {
            if (m_CurrentLocalizationId.Equals(localizationId, StringComparison.Ordinal))
                return;

            Clear();
            m_CurrentLocalizationId = localizationId;


        }
    }
}
