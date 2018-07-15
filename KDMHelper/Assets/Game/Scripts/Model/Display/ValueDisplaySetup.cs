using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Game.Model.Display
{
    [Serializable]
    public enum ValueDisplayMode
    {
        [XmlEnum(Name = "CellDisplay")]
        CellDisplay, //display this data in a table cell
        [XmlEnum(Name = "CellControl")]
        CellControl, //display this data in a table cell and with value changing controls if possible

        [XmlEnum(Name = "RowDisplay")]
        RowDisplay,
        [XmlEnum(Name = "RowControl")]
        RowControl,

        [XmlEnum(Name = "Detailed")]
        Detailed,
    }

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
