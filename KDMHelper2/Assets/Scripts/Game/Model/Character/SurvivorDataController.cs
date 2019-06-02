using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game.Model.Character
{
    public enum SurvivorDataDisplayMode
    {
        CellDisplayValue = 1 << 0, //display this data in a table cell
        CellControlValue = 1 << 1, //display this data in a table cell and with value changing controls if possible
        CellDisplayBrief = 1 << 2,
        CellControlBrief = 1 << 3,
        CellDisplayExtended = 1 << 4,
        CellControlExtended = 1 << 5,

        RowDisplayBrief = 1 << 6,
        RowControlBrief = 1 << 7,

        Row = 1, //display this data in a vertical list
    }

    public class SurvivorDataController : ScriptableObject
    {
        public string DataType;
    }
}
