using Common.Properties;
using Common.Properties.Enumeration;
using Common.Properties.Numerical;
using Game.Properties;
using Game.Properties.Modifiers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Game.Model.Character
{
    
    [Serializable]
    public class Survivor2
    {
        public class ObservableNamedPropertyCollection : KeyedCollection<string, NamedProperty>
        {
            public event Action<object> SimpleChangeSubscription;

            protected override string GetKeyForItem(NamedProperty item)
            {
                return item.GetName();
            }

            public NamedProperty[] ToArray()
            {
                return Items.ToArray();
            }

            public bool TryGetProperty(string i_Key, out NamedProperty o_Value)
            {
                return Dictionary.TryGetValue(i_Key, out o_Value);
            }

            public object FindProperty(string i_Key)
            {
                NamedProperty result = null;
                Dictionary.TryGetValue(i_Key, out result);
                return result;
            }

            public new void Add(NamedProperty i_Property)
            {
                if(i_Property != null)
                {
                    IObservablePropertySimpleSubscription subscribable = i_Property as IObservablePropertySimpleSubscription;
                    if(subscribable != null)
                    {
                        subscribable.SimpleChangeSubscription += NotifyChange;
                    }
                }
                base.Add(i_Property);
            }

            public new bool Remove(NamedProperty i_Property)
            {
                bool result = base.Remove(i_Property);
                if (result)
                {
                    if (i_Property != null)
                    {
                        IObservablePropertySimpleSubscription subscribable = i_Property as IObservablePropertySimpleSubscription;
                        if (subscribable != null)
                        {
                            subscribable.SimpleChangeSubscription -= NotifyChange;
                        }
                    }
                }
                return result;
            }

            private void NotifyChange(object source)
            {
                if (SimpleChangeSubscription != null)
                {
                    SimpleChangeSubscription(source);
                }
            }
        }

        public Survivor2()
        {
            Properties = new ObservableNamedPropertyCollection();
        }


        public Guid Id;
        public Guid[] Parents;
        public Guid WritePermissionToken;

        public ObservableNamedPropertyCollection Properties;

        
        
        /*
        private Dictionary<string, object> m_data;

        public ELifeState LifeState;

        public bool SkipNextHunt;

        public int HuntXp;
        public int WeaponXp;
        public EnumProperty<WeaponProficiencyInfo> WeaponType;
        
        public bool CanSpendSurvival;
        public SurvivorNumericalProperty Survival;

        public int Courage;
        public List<EnumProperty<string>> CourageAbility = new List<EnumProperty<string>>();
        public int Understanding;
        public List<EnumProperty<string>> UnderstandingAbility = new List<EnumProperty<string>>();

        public List<EnumProperty<string>> Disorders = new List<EnumProperty<string>>();
        public List<EnumProperty<string>> FightingArts = new List<EnumProperty<string>>();
        public List<EnumProperty<string>> AbilitiesAndImpairments = new List<EnumProperty<string>>();

        public SurvivorStats Stats;
        public SurvivorArmor Armor;

        public SurvivorNumericalProperty Frenzy;
        public SurvivorNumericalProperty Bleeding;

        public List<string> Notes;

        
        public Survivor2()
        {
            SurvivorStats Stats = new SurvivorStats(this);
            SurvivorArmor Armor = new SurvivorArmor();
            Stats.Streangth.AddModifier(new SurvivorFrenzyMod());
            Stats.Speed.AddModifier(new SurvivorFrenzyMod());
        }
        */
        
    }
}
