
using System;

namespace Common.Properties.Boolean
{
    /// <summary>
    /// A bool property that represents true or false
    /// </summary>
    public class ObservableBoolProperty : BoolProperty, IObservablePropertySimpleSubscription
    {
        public event PropertyChangeHandler<bool, ObservableBoolProperty> ChangeSubscription;
        public event Action<object> SimpleChangeSubscription;

        public ObservableBoolProperty() : base(false)
        { }
        public ObservableBoolProperty(bool i_Value) : base(i_Value)
        { }
        
        public override void SetValue(bool i_Value)
        {
            bool temp = m_Value;
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
