using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Common.Properties.String
{
    /// <summary>
    /// A simple property that represents a string value.
    /// </summary>
    public class ObservableStringProperty : StringProperty , IObservablePropertySimpleSubscription
    {
        public event PropertyChangeHandler<string, ObservableStringProperty> ChangeSubscription;
        public event Action<object> SimpleChangeSubscription;


        public ObservableStringProperty() : base(string.Empty)
        { }
        public ObservableStringProperty(string i_Value) : base(i_Value)
        { }


        public override void SetValue(string i_Value)
        {
            string temp = m_Value;
            m_Value = i_Value;
            if (ChangeSubscription != null)
            {
                ChangeSubscription(temp, i_Value, this);
            }
            if(SimpleChangeSubscription != null)
            {
                SimpleChangeSubscription(this);
            }
        }
    }
}

