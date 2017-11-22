using Common.Properties.Enumeration;
using Game.Model.Character;
using Game.Model.Events;
using Game.Model.Resources;
using Game.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model
{

    public enum ESurvivalActions
    {
        Dodge = 1,
        Encourage = 2,
        Surge = 4,
        Dash = 8
    }

    public class Campaign
    {
        public Guid Id;
        public string Name;
        public EnumProperty<string> CampaignType;
        public List<EnumProperty<string>> Expansions;

        public string SettlementName;

        public MaxSurvivalProperty MaxSurvival;

        public SettlementTimeline Timeline;
        

        List<Survivor> Population;
        List<Survivor> Dead;

        public ESurvivalActions SurvivalActions;

        List<int> MonsterVolumes;


        public List<EnumProperty<WeaponProficiencyInfo>> WeaponMasteries;
        public List<EnumProperty<string>> Innovations;
        public List<EnumProperty<string>> Principals;

        public List<EnumProperty<string>> Quarries;
        //will probably change to add nemesis lvl tracking
        public List<EnumProperty<string>> Nemesis;

        public SettlementResources Resources;
        

        //Milestones???
    }
}
