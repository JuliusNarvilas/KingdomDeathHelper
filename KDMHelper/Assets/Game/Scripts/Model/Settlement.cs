﻿using Common.Properties.Enumeration;
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
        public Guid Id;


        public string SettlementName;

        public EnumProperty<Campaign> CampaignType;

        public MaxSurvivalProperty MaxSurvival;

        public SettlementTimeline Timeline;

        List<Survivor> Survivors;

        public ESurvivalActions SurvivalActions;

        List<int> MonsterVolumes;



        public List<EnumProperty<WeaponProficiencyInfo>> WeaponMasteries;
        public List<EnumProperty<string>> Innovations;
        public List<EnumProperty<string>> Principals;


        public List<EnumProperty<string>> Quarries;
        //will probably change to add nemesis lvl tracking
        public List<EnumProperty<string>> Nemesis;

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