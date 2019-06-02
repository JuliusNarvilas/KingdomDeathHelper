using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game.Model.Character
{

    public class SurvivorData
    {
        string DataType;

        //*can be numerical
        //*can be enum property
        //*can be a collection property


        //layout
        //*has string value
        //*may have string description
        //*may have different display modes
        //

        virtual public void ApplySort(ref IOrderedEnumerable<Survivor> processingList, bool i_Asc)
        {
            //processingList = i_Asc ? processingList.ThenBy(x => x.Stats.Streangth.GetValue()) : processingList.ThenByDescending(x => x.Stats.Streangth.GetValue());
        }

        GameObject GetDisplay()
        {

            return null;
        }
    }
}
