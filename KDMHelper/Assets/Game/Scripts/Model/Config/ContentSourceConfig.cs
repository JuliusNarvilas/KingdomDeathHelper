using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Game.Model.Config
{
    public class ContentSourceRecord
    {
        public string sourcePath;
        public string sourceType;
    }

    public class ContentSourceConfig
    {
        public List<ContentSourceRecord> rexcrds;
    }
}
