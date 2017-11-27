using Common;
using Game.Model.Character;
using Game.Screen.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screen.SurvivorList
{
    public class SurvivorSortControl : MonoBehaviour , IScreenSortControl<Survivor>
    {
        private enum ESurvivorSortInfoType
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

        [Serializable]
        private struct SurvivorSortInfoSettings
        {
            public ESurvivorSortInfoType InfoType;
            public ScreenSortInfo Info;
        }

        [SerializeField]
        private List<SurvivorSortInfoSettings> m_SortInfoList;

        private IOrderedEnumerable<Survivor> m_ProcessingList;
        private List<ScreenSortInfo> m_ActiveSortInfoList = new List<ScreenSortInfo>();

        public Action OnChange;

        private void Awake()
        {
            int count = m_SortInfoList.Count;
            for(int i = 0; i < count; ++i)
            {
                InitInfo(m_SortInfoList[i]);
            }
        }

        private void InitInfo(SurvivorSortInfoSettings i_Settings)
        {
            switch(i_Settings.InfoType)
            {
                case ESurvivorSortInfoType.Name:
                    i_Settings.Info.Init(this, SortName);
                    break;
                case ESurvivorSortInfoType.Strength:
                    i_Settings.Info.Init(this, SortStrength);
                    break;
                case ESurvivorSortInfoType.Evasion:
                    i_Settings.Info.Init(this, SortEvasion);
                    break;
                case ESurvivorSortInfoType.Luck:
                    i_Settings.Info.Init(this, SortLuck);
                    break;
                case ESurvivorSortInfoType.Accuracy:
                    i_Settings.Info.Init(this, SortAccuracy);
                    break;
                case ESurvivorSortInfoType.Speed:
                    i_Settings.Info.Init(this, SortSpeed);
                    break;
                case ESurvivorSortInfoType.HuntXp:
                    i_Settings.Info.Init(this, SortHuntXp);
                    break;
                case ESurvivorSortInfoType.Gender:
                    i_Settings.Info.Init(this, SortGender);
                    break;
                case ESurvivorSortInfoType.WeaponType:
                    i_Settings.Info.Init(this, SortWeaponType);
                    break;
                case ESurvivorSortInfoType.WeaponXp:
                    i_Settings.Info.Init(this, SortWeaponXp);
                    break;
                case ESurvivorSortInfoType.Courage:
                    i_Settings.Info.Init(this, SortCourage);
                    break;
                case ESurvivorSortInfoType.Understanding:
                    i_Settings.Info.Init(this, SortUnderstanding);
                    break;
                default:
                    Log.ProductionLogError(string.Format("Unknown ESurvivorSortInfoType: (int){0} or (string){1}", (int)i_Settings.InfoType, i_Settings.InfoType.ToString()));
                    break;
            }
        }

        
        private void SortName(bool i_Asc)
        {
            m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.Name) : m_ProcessingList.ThenByDescending(x => x.Name);
        }
        private void SortStrength(bool i_Asc)
        {
            m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.Stats.Streangth.GetValue()) : m_ProcessingList.ThenByDescending(x => x.Stats.Streangth.GetValue());
        }
        private void SortEvasion(bool i_Asc)
        {
            m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.Stats.Evasion.GetValue()) : m_ProcessingList.ThenByDescending(x => x.Stats.Evasion.GetValue());
        }
        private void SortLuck(bool i_Asc)
        {
            m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.Stats.Luck.GetValue()) : m_ProcessingList.ThenByDescending(x => x.Stats.Luck.GetValue());
        }
        private void SortAccuracy(bool i_Asc)
        {
            m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.Stats.Accuracy.GetValue()) : m_ProcessingList.ThenByDescending(x => x.Stats.Accuracy.GetValue());
        }
        private void SortSpeed(bool i_Asc)
        {
            m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.Stats.Speed.GetValue()) : m_ProcessingList.ThenByDescending(x => x.Stats.Speed.GetValue());
        }
        private void SortHuntXp(bool i_Asc)
        {
            m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.HuntXp) : m_ProcessingList.ThenByDescending(x => x.HuntXp);
        }
        private void SortGender(bool i_Asc)
        {
            m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.Gender) : m_ProcessingList.ThenByDescending(x => x.Gender);
        }
        private void SortWeaponType(bool i_Asc)
        {
            m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.WeaponType) : m_ProcessingList.ThenByDescending(x => x.WeaponType);
        }
        private void SortWeaponXp(bool i_Asc)
        {
            m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.WeaponXp) : m_ProcessingList.ThenByDescending(x => x.WeaponXp);
        }
        private void SortCourage(bool i_Asc)
        {
            m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.Courage) : m_ProcessingList.ThenByDescending(x => x.Courage);
        }
        private void SortUnderstanding(bool i_Asc)
        {
            m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.Understanding) : m_ProcessingList.ThenByDescending(x => x.Understanding);
        }

        public IOrderedEnumerable<Survivor> Sort(IOrderedEnumerable<Survivor> i_Source)
        {
            m_ProcessingList = i_Source;

            int count = m_ActiveSortInfoList.Count;
            for (int i = 0; i < count; ++i)
            {
                m_ActiveSortInfoList[i].ApplySorting();
            }

            return m_ProcessingList;
        }


        public void Clear()
        {
            int count = m_ActiveSortInfoList.Count;
            for (int i = 0; i < count; ++i)
            {
                m_ActiveSortInfoList[i].SetStateSilent(ESortType.None);
            }
            m_ActiveSortInfoList.Clear();
            TriggerOnChange();
        }

        public void Append(ScreenSortInfo i_SortInfo)
        {
            m_ActiveSortInfoList.Add(i_SortInfo);
            TriggerOnChange();
        }

        public void Remove(ScreenSortInfo i_SortInfo)
        {
            if(m_ActiveSortInfoList.Remove(i_SortInfo))
            {
                TriggerOnChange();
            }
        }

        public void TriggerOnChange()
        {
            if (OnChange != null)
            {
                OnChange();
            }
        }
    }
}
