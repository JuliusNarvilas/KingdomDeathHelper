using Common.Properties.Numerical;
using Game.Model;
using Game.Model.Character;
using Game.Properties.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Properties
{
    public struct KDMNumericalPropertyContext
    {
        public NumericalProperty<int, KDMNumericalPropertyContext, KDMNumericalPropertyModifierReader> Property;
        public Survivor Survivor;
        public Settlement Settlement;
    }
}
