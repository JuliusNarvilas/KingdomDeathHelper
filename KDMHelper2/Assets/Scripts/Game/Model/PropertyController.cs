using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game.Model
{
    public abstract class PropertyController : MonoBehaviour
    {

        public abstract bool SetData(object obj);

        public void ApplySort<T>(ref IOrderedEnumerable<T> processingList, bool i_Asc)
        {
            //processingList = i_Asc ? processingList.ThenBy(x => x.Stats.Streangth.GetValue()) : processingList.ThenByDescending(x => x.Stats.Streangth.GetValue());
        }
    }
}
