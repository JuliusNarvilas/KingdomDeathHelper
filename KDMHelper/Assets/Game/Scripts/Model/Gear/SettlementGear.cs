using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Game.Model.Gear
{
    public class SettlementGear
    {
        private class GearCollection : KeyedCollection<string, SettlementGearRecord>
        {
            protected override string GetKeyForItem(SettlementGearRecord item)
            {
                return item.GearType.GetValue().ToString();
            }

            public SettlementGearRecord[] ToArray()
            {
                return Items.ToArray();
            }

            public bool TryGetValue(string i_Key, out SettlementGearRecord o_Value)
            {
                return Dictionary.TryGetValue(i_Key, out o_Value);
            }
        }

        GearCollection Records;


    }
}
