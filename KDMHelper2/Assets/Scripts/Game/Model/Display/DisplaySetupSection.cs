using Common;
using Game.DisplayHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Game.Model.Display
{
    [Serializable]
    public class DisplaySetupSection
    {
        public string Source;
        public string Selection;
        public string DisplayHandlerFilter;
        public ValueDisplayMode DisplayMode;

        public NamedProperty PropertyExample;


    }
}
