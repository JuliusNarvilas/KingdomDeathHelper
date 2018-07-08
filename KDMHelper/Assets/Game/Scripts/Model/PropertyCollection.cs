using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model
{

    public interface IPropertyCollectionGetter
    {
        PropertyCollection GetPropCollection();
    }

    public class PropertyCollectionItem
    {
        string Name;
        object Property;
    }

    public struct NamedPropertySortData
    {
        public bool Asc;
        public string Name;
    }

    public class PropertyCollection
    {
        public Dictionary<string, NamedProperty> Properties;

        public void Sort(List<NamedPropertySortData> propertyNames)
        {
            IOrderedEnumerable<NamedProperty> sortedList = Properties.Values.OrderBy(x => 1);

            int count = propertyNames.Count;
            for(int i = 0; i < count; ++i)
            {
                /*
                NamedProperty result;
                if(Properties.TryGetValue(propertyNames[i].Name, out result))
                {
                    sortedList = propertyNames[i].Asc ? sortedList.ThenBy();
                }
                propertyNames
                */
            }
        }
    }
}
