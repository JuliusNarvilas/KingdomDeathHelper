using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Common.Properties.Enumeration
{
    /// <summary>
    /// A simple property that represents a unique value (per generator used).
    /// </summary>
    public class ObservableEnumProperty : Property<EnumProperty>, IObservablePropertySimpleSubscription
    {

        public event PropertyChangeHandler<EnumProperty, ObservableEnumProperty> ChangeSubscription;
        public event Action<object> SimpleChangeSubscription;

        public ObservableEnumProperty() : base(null)
        { }
        public ObservableEnumProperty(EnumProperty i_Value) : base(i_Value)
        {}

        public void SetValue(EnumProperty i_Value)
        {
            EnumProperty temp = m_Value;
            m_Value = i_Value;
            if (ChangeSubscription != null)
            {
                ChangeSubscription(temp, i_Value, this);
            }
            if (SimpleChangeSubscription != null)
            {
                SimpleChangeSubscription(this);
            }
        }
        
    }
}
