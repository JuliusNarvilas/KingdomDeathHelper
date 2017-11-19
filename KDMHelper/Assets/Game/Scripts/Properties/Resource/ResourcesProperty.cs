using Common.Properties.Numerical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Properties.Numerical.Data;

namespace Game.Properties.Resource
{
    public class ResourcesProperty : NumericalProperty<ResourcesPropertyData, int, INumericalPropertyModifierReader<ResourcesPropertyData>>
    {
        public ResourcesProperty(INumericalPropertyData<ResourcesPropertyData> i_Value) : base(i_Value)
        {
        }
    }
}
