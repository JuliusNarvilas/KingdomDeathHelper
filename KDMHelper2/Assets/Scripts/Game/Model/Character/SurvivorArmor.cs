using Game.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model.Character
{
    public class SurvivorArmor
    {
        public SurvivorArmorLocation Head;
        public SurvivorArmorLocation Body;
        public SurvivorArmorLocation Hands;
        public SurvivorArmorLocation Waist;
        public SurvivorArmorLocation Legs;

        public SurvivorArmorLocation Find(string i_Location)
        {
            switch(i_Location.ToUpper())
            {
                case "HEAD":
                    return Head;
                case "BODY":
                    return Body;
                case "HANDS":
                    return Hands;
                case "WAIST":
                    return Waist;
                case "LEGS":
                    return Legs;
            }
            return null;
        }
    }
}
