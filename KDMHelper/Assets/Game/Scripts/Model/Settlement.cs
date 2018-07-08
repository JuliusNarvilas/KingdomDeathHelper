using Common.Properties.Enumeration;
using Game.Model.Character;
using Game.Model.Events;
using Game.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using Game.Model.Resources;

namespace Game.Model
{
    public enum ESurvivalActions
    {
        Dodge = 1,
        Encourage = 2,
        Surge = 4,
        Dash = 8
    }

    public class Settlement : IXmlSerializable
    {
        private static Settlement s_Instance;

        public static Settlement GetCurrent()
        {
            return s_Instance;
        }


        public Guid Id;


        public string SettlementName;

        public EnumProperty CampaignType;

        //public SurvivorNumericalProperty MaxSurvival;

        public SettlementTimeline Timeline;

        List<Survivor> Survivors;

        public ESurvivalActions SurvivalActions;

        List<int> MonsterVolumes;



        public List<EnumProperty> WeaponMasteries;
        public List<EnumProperty> Innovations;
        public List<EnumProperty> Principals;


        public List<EnumProperty> Quarries;
        //will probably change to add nemesis lvl tracking
        public List<EnumProperty> Nemesis;

        public SettlementResources Resources;

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
