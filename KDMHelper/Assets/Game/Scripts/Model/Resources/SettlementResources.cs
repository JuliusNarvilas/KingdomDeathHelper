using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Game.Model.Resources
{
    public class SettlementResources
    {
        private class ResourcesCollection : KeyedCollection<string, SettlementResourcesRecord>
        {
            protected override string GetKeyForItem(SettlementResourcesRecord item)
            {
                return item.ResourceType.GetValue().ToString();
            }

            public SettlementResourcesRecord[] ToArray()
            {
                return Items.ToArray();
            }

            public bool TryGetValue(string i_Key, out SettlementResourcesRecord o_Value)
            {
                return Dictionary.TryGetValue(i_Key, out o_Value);
            }
        }

        ResourcesCollection Records;


    }
}
