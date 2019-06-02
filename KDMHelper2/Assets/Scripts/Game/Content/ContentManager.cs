using Common.IO.FileHelpers.CSV;
using Game.Content.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game.Content
{
    [Serializable]
    public class ContentManagerRecord
    {
        public string Localization;
        public Dictionary<string, Sprite> Images;
        public Dictionary<string, CSVData> Content;
        public Dictionary<string, ContentLayoutController> Layout;
    }


    public class ContentManager : IContentProvider
    {
        Dictionary<string, Sprite> m_Images;

        public object GetContent(string key)
        {
            throw new NotImplementedException();
        }
    }
}
