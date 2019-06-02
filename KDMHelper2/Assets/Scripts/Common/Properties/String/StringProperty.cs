using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Common.Properties.String
{
    /// <summary>
    /// A simple property that represents a string value.
    /// </summary>
    public class StringProperty : Property<string>
    {

        public StringProperty() : base(string.Empty)
        { }
        public StringProperty(string i_Value) : base(i_Value)
        { }


        public virtual void SetValue(string i_Value)
        {
            m_Value = i_Value;
        }
    }
}
