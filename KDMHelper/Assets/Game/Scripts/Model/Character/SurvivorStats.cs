using Common;
using Game.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model.Character
{
    public class SurvivorStats
    {
        private Survivor m_Survivor;

        public StatProperty Accuracy;
        public StatProperty Streangth;
        public StatProperty Evasion;
        public StatProperty Luck;
        public StatProperty Speed;

        public SurvivorStats(Survivor i_Survivor)
        {
            m_Survivor = i_Survivor;
            Accuracy = new StatProperty(m_Survivor, "Accuracy", 0);
            Streangth = new StatProperty(m_Survivor, "Streangth", 0);
            Evasion = new StatProperty(m_Survivor, "Evasion", 0);
            Luck = new StatProperty(m_Survivor, "Luck", 0);
            Speed = new StatProperty(m_Survivor, "Speed", 0);
        }



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
                case "LUCK":
                    return Luck;
            }
            Log.ProductionLogError(string.Format("Can not find Survivor stat: {0}.", i_Name));
            return null;
        }
    }
}
