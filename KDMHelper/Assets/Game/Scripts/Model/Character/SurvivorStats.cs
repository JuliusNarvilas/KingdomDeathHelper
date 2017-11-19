using Game.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model.Character
{
    public class SurvivorStats
    {
        public StatProperty Speed;
        public StatProperty Accuracy;
        public StatProperty Streangth;
        public StatProperty Evasion;

        public StatProperty Find(string i_Name)
        {
            switch(i_Name.ToUpper())
            {
                case "SPEED":
                    return Speed;
                case "ACCURACY":
                    return Accuracy;
                case "STREANGTH":
                    return Streangth;
                case "EVASION":
                    return Evasion;
            }
            return null;
        }
    }
}
