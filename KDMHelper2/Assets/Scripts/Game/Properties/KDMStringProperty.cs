using Common.Properties.Numerical;
using System;
using Common.Properties;



namespace Game.Properties
{

    [Serializable]
    public class KDMStringProperty : Property<string>
    {

        public KDMStringProperty() : base(string.Empty)
        {
        }
        public KDMStringProperty(string i_Value) : base(i_Value)
        {
        }
        
        public string Text { get; set; }
    }
}
