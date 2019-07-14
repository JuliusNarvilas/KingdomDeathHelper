using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.IO.ContentProvider
{
    public enum EContentType
    {
        Blob,
        Text,
        CSV
    }

    public enum EContentSourceType
    {
        External,
        Resource,
        AssetBundle
    }


    public class ContentSettingsEntry
    {
        public string Name;
        public int Version = 0;
        public EContentType ContentType;
        public EContentSourceType SourceType;
        public string Source;

        public bool IsValid()
        {
            if (!Enum.IsDefined(typeof(EContentType), ContentType))
                return false;
            if (!Enum.IsDefined(typeof(EContentSourceType), SourceType))
                return false;

            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Source);
        }
    }

    public class LocalizedContentSettings
    {
        public string LocalizationId;
        public List<ContentSettingsEntry> Entries;
    }

    public class ContentSettings
    {
        public List<LocalizedContentSettings> Localizations;
    }
}
