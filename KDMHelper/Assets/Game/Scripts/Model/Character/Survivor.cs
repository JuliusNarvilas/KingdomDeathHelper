using Assets.Game.Scripts.Properties;
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
        public EnumProperty<WeaponProficiencyInfo> WeaponType;
        
        public bool CanSpendSurvival;
        public SurvivalProperty Survival;

        public int Courage;
        public List<EnumProperty<string>> CourageAbility = new List<EnumProperty<string>>();
        public int Understanding;
        public List<EnumProperty<string>> UnderstandingAbility = new List<EnumProperty<string>>();

        public List<EnumProperty<string>> Disorders = new List<EnumProperty<string>>();
        public List<EnumProperty<string>> FightingArts = new List<EnumProperty<string>>();
        public List<EnumProperty<string>> AbilitiesAndImpairments = new List<EnumProperty<string>>();

        public SurvivorStats Stats = new SurvivorStats();
        public SurvivorArmor Armor = new SurvivorArmor();

        public int Rage;
        public int Bleeding;

        
        public Survivor()
        {
            Stats.Speed.AddModifier(new SurvivorFrenzyMod());
        }
    }
}
