using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model.Display
{
    public enum DisplaySourceType
    {
        Global,
        InContext
    }

    public class DisplaySetupSection
    {
        public string SourceType;

        public string Source;

        public string Selection;
    }
}
