using Assets.Game.Scripts.Properties;
using Common.Properties.Enumeration;
using Common.Properties.Numerical;
using Game.Properties;
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

    public class Survivor
    {
        public int Id;

        public Guid WritePermissionToken;


        public EGender Gender;
        public bool Alive;
        public bool Retired;

        public int HuntXp;
        public int WeaponXp;
        public EnumProperty<WeaponProficiencyInfo> WeaponType;
        
        public bool CanSpendSurvival;
        public SurvivalProperty Survival;

        public int Courage;
        public List<EnumProperty<string>> CourageAbility;
        public int Understanding;
        public List<EnumProperty<string>> UnderstandingAbility;

        public List<EnumProperty<string>> Disorders;
        public List<EnumProperty<string>> FightingArts;
        public List<EnumProperty<string>> AbilitiesAndImpairments;

        public SurvivorStats Stats;
        public SurvivorArmor Armor;

        public int Rage;
        public int Bleeding;
    }
}
