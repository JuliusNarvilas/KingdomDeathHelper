using Game.Model;
using Game.Model.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace Game.DisplayHandler
{

    [Serializable]
    public enum ValueDisplayMode
    {
        [XmlEnum(Name = "CellDisplayMode")]
        CellDisplayMode, //display this data in a table cell
        [XmlEnum(Name = "CellControlMode")]
        CellControlMode, //display this data in a table cell and with value changing controls if possible

        [XmlEnum(Name = "RowDisplayMode")]
        RowDisplayMode,
        [XmlEnum(Name = "RowControlMode3")]
        RowControlMode,

        [XmlEnum(Name = "DetailedMode")]
        DetailedMode,
    }

    public abstract class ValueDisplayHandler : MonoBehaviour
    {
        public abstract void SetValue(NamedProperty val);
    }
}
