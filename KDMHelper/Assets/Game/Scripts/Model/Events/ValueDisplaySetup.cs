using Game.DisplayHandler;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Game.Model.Display
{

    public class ValueDisplayOption
    {
        public ValueDisplayMode Mode;
        public ValueDisplayHandler Display;
    }


    public class ValueDisplaySetup
    {
        public List<ValueDisplayOption> Options = new List<ValueDisplayOption>();

        public ValueDisplayHandler GetDisplayHandler(ValueDisplayMode mode)
        {
            int count = Options.Count;
            for(int i = 0; i < count; ++i)
            {
                if(Options[i].Mode == mode)
                {
                    return Options[i].Display;
                }
            }

            return null;
        }

        public ValueDisplayHandler GetDisplayHandler(string mode)
        {
            ValueDisplayMode enumMode;
            try
            {
                enumMode = (ValueDisplayMode)Enum.Parse(typeof(ValueDisplayMode), mode);
            }
            catch
            {
                return null;
            }

            return GetDisplayHandler(enumMode);
        }
    }
}
