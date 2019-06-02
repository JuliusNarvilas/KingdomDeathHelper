using Common.Properties.Enumeration;
using Common.Properties.Numerical;
using Game.Properties;
using Game.Properties.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model.Character
{
    public enum ESurvivorsInfoType
    {
        Name,
        Strength,
        Evasion,
        Luck,
        Accuracy,
        Speed,
        HuntXp,
        Gender,
        WeaponType,
        WeaponXp,
        Courage,
        Understanding
    }

    public enum EGender
    {
        Unknown,
        Male,
        Female
    }

    public enum ELifeState
    {
        Unknown = 0,
        Active = 1,
        Retired = 2,
        Dead = 4
    }

    public class Survivor
    {
        public Guid Id;
        public Guid WritePermissionToken;

        public string Name;
        public EGender Gender;
        public ELifeState LifeState;

        public bool SkipNextHunt;

        public int HuntXp;
        public int WeaponXp;
        public EnumProperty WeaponType;
        
        public bool CanSpendSurvival;
        //public SurvivorNumericalProperty Survival;

        public int Courage;
        public List<EnumProperty> CourageAbility = new List<EnumProperty>();
        public int Understanding;
        public List<EnumProperty> UnderstandingAbility = new List<EnumProperty>();

        public List<EnumProperty> Disorders = new List<EnumProperty>();
        public List<EnumProperty> FightingArts = new List<EnumProperty>();
        public List<EnumProperty> AbilitiesAndImpairments = new List<EnumProperty>();

        public SurvivorStats Stats;
        public SurvivorArmor Armor;

        //public SurvivorNumericalProperty Frenzy;
        //public SurvivorNumericalProperty Bleeding;

        public List<string> Notes;

        

        public string GetValue(ESurvivorsInfoType i_InfoType)
        {
            /*
            switch (i_InfoType) {
                case ESurvivorsInfoType.Accuracy:
                    return Stats.Accuracy.GetValue().ToString();
                case ESurvivorsInfoType.Courage:
                    return Courage.ToString();
                case ESurvivorsInfoType.Evasion:
                    return Stats.Evasion.GetValue().ToString();
                case ESurvivorsInfoType.Gender:
                    return Gender.ToString();
                case ESurvivorsInfoType.HuntXp:
                    return HuntXp.ToString();
                case ESurvivorsInfoType.Luck:
                    return Stats.Luck.GetValue().ToString();
                case ESurvivorsInfoType.Name:
                    return Name.ToString();
                case ESurvivorsInfoType.Speed:
                    return Stats.Speed.GetValue().ToString();
                case ESurvivorsInfoType.Strength:
                    return Stats.Streangth.GetValue().ToString();
                case ESurvivorsInfoType.Understanding:
                    return Understanding.ToString();
                case ESurvivorsInfoType.WeaponType:
                    return WeaponType.GetValue().ToString();
                case ESurvivorsInfoType.WeaponXp:
                    return WeaponXp.ToString();
            }
            */
            return string.Empty;
        }
    }
}
