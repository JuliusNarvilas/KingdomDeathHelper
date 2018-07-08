

namespace Common.Properties.Boolean
{
    /// <summary>
    /// A bool property that represents true or false
    /// </summary>
    public class BoolProperty : Property<bool>
    {

        public BoolProperty() : base(false)
        { }
        public BoolProperty(bool i_Value) : base(i_Value)
        { }
        
        public virtual void SetValue(bool i_Value)
        {
            m_Value = i_Value;
        }
    }
}
