using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Game.Scripts.IO.ContentProvider
{
    public enum EContentType
    {
        Blob,
        Text,
        CSV
    }


    public class ContentEntry
    {
        public string Name;
        public int Version = 0;
        public string Type;
        public string Path;

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Path);
        }
    }

    public class LocalizedContentSettings
    {
        public string LocalizationId;
        public List<ContentEntry> Records;
    }

    public class ContentSettings
    {



    }
}
