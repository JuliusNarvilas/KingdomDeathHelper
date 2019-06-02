using Common;
using Common.Display.Table;
using Game.Display.Screen;
using Game.Model.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Display.Table
{
    public class SurvivorsTableSortControl : MonoBehaviour, ITableSortControl<Survivor>
    {

        [Serializable]
        private struct SurvivorsSortInfoSettings
        {
            public ESurvivorsInfoType InfoType;
            public TableSortInfo Info;
        }


        private static int s_InfoTypeCount;
        public static int InfoTypeCount { get { return s_InfoTypeCount; } }
        static SurvivorsTableSortControl()
        {
            s_InfoTypeCount = Enum.GetNames(typeof(ESurvivorsInfoType)).Length;
        }


        [SerializeField]
        private List<SurvivorsSortInfoSettings> m_SortInfoList;
        

        private IOrderedEnumerable<Survivor> m_ProcessingList;


        [SerializeField]
        private Transform m_SortDisplayContainer;
        [SerializeField]
        private TableSortInfoDisplay m_SortDisplayElement;


        private List<TableSortInfo> m_ActiveSortInfoList = new List<TableSortInfo>();
        private List<TableSortInfo> m_VisibleSortInfoList = new List<TableSortInfo>();
        private List<TableSortInfoDisplay> m_SortDisplayElements = new List<TableSortInfoDisplay>();
        public List<TableSortInfo> GetVisibleSortInfo()
        {
            return m_VisibleSortInfoList.ToList();
        }

        private TableSortInfo[] m_SortInfoByType;
        private Coroutine m_DeferedListElementsUpdate;

        private void Awake()
        {
            m_SortInfoByType = new TableSortInfo[InfoTypeCount];
            int givenInfoCount = m_SortInfoList.Count;
            for (int i = 0; i < givenInfoCount; ++i)
            {
                var sortInfoSettings = m_SortInfoList[i];

                int index = (int)sortInfoSettings.InfoType;
                if (index >= 0 && index < InfoTypeCount)
                {
                    m_SortInfoByType[i] = sortInfoSettings.Info;
                }
                else
                {
                    Log.ProductionLogError("Invalid Survivor Sort Info type.");
                }

                InitInfo(sortInfoSettings);
                if(sortInfoSettings.Info.gameObject.activeSelf)
                {
                    m_VisibleSortInfoList.Add(sortInfoSettings.Info);
                }
            }

            m_VisibleSortInfoList.Sort(TableSortInfo.SiblingIndexComparer.Ascending);
        }

        private void InitInfo(SurvivorsSortInfoSettings i_Settings)
        {
            switch(i_Settings.InfoType)
            {
                case ESurvivorsInfoType.Name:
                    i_Settings.Info.Init(this, SortName, i_Settings.InfoType);
                    break;
                case ESurvivorsInfoType.Strength:
                    i_Settings.Info.Init(this, SortStrength, i_Settings.InfoType);
                    break;
                case ESurvivorsInfoType.Evasion:
                    i_Settings.Info.Init(this, SortEvasion, i_Settings.InfoType);
                    break;
                case ESurvivorsInfoType.Luck:
                    i_Settings.Info.Init(this, SortLuck, i_Settings.InfoType);
                    break;
                case ESurvivorsInfoType.Accuracy:
                    i_Settings.Info.Init(this, SortAccuracy, i_Settings.InfoType);
                    break;
                case ESurvivorsInfoType.Speed:
                    i_Settings.Info.Init(this, SortSpeed, i_Settings.InfoType);
                    break;
                case ESurvivorsInfoType.HuntXp:
                    i_Settings.Info.Init(this, SortHuntXp, i_Settings.InfoType);
                    break;
                case ESurvivorsInfoType.Gender:
                    i_Settings.Info.Init(this, SortGender, i_Settings.InfoType);
                    break;
                case ESurvivorsInfoType.WeaponType:
                    i_Settings.Info.Init(this, SortWeaponType, i_Settings.InfoType);
                    break;
                case ESurvivorsInfoType.WeaponXp:
                    i_Settings.Info.Init(this, SortWeaponXp, i_Settings.InfoType);
                    break;
                case ESurvivorsInfoType.Courage:
                    i_Settings.Info.Init(this, SortCourage, i_Settings.InfoType);
                    break;
                case ESurvivorsInfoType.Understanding:
                    i_Settings.Info.Init(this, SortUnderstanding, i_Settings.InfoType);
                    break;
                default:
                    Log.ProductionLogError(string.Format("Unknown ESurvivorSortInfoType: (int){0} or (string){1}", (int)i_Settings.InfoType, i_Settings.InfoType.ToString()));
                    break;
            }
        }

        private void OnEnable()
        {
            if (m_DeferedListElementsUpdate == null)
            {
                m_DeferedListElementsUpdate = StartCoroutine(DeferedListElementsUpdateFunc());
            }
        }

        private void SortName(bool i_Asc)
        {
            //m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.Name) : m_ProcessingList.ThenByDescending(x => x.Name);
        }
        private void SortStrength(bool i_Asc)
        {
            //m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.Stats.Streangth.GetValue()) : m_ProcessingList.ThenByDescending(x => x.Stats.Streangth.GetValue());
        }
        private void SortEvasion(bool i_Asc)
        {
            //m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.Stats.Evasion.GetValue()) : m_ProcessingList.ThenByDescending(x => x.Stats.Evasion.GetValue());
        }
        private void SortLuck(bool i_Asc)
        {
            //m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.Stats.Luck.GetValue()) : m_ProcessingList.ThenByDescending(x => x.Stats.Luck.GetValue());
        }
        private void SortAccuracy(bool i_Asc)
        {
            //m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.Stats.Accuracy.GetValue()) : m_ProcessingList.ThenByDescending(x => x.Stats.Accuracy.GetValue());
        }
        private void SortSpeed(bool i_Asc)
        {
            //m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.Stats.Speed.GetValue()) : m_ProcessingList.ThenByDescending(x => x.Stats.Speed.GetValue());
        }
        private void SortHuntXp(bool i_Asc)
        {
            //m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.HuntXp) : m_ProcessingList.ThenByDescending(x => x.HuntXp);
        }
        private void SortGender(bool i_Asc)
        {
            //m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.Gender) : m_ProcessingList.ThenByDescending(x => x.Gender);
        }
        private void SortWeaponType(bool i_Asc)
        {
            //m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.WeaponType) : m_ProcessingList.ThenByDescending(x => x.WeaponType);
        }
        private void SortWeaponXp(bool i_Asc)
        {
            //m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.WeaponXp) : m_ProcessingList.ThenByDescending(x => x.WeaponXp);
        }
        private void SortCourage(bool i_Asc)
        {
            //m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.Courage) : m_ProcessingList.ThenByDescending(x => x.Courage);
        }
        private void SortUnderstanding(bool i_Asc)
        {
            //m_ProcessingList = i_Asc ? m_ProcessingList.ThenBy(x => x.Understanding) : m_ProcessingList.ThenByDescending(x => x.Understanding);
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


        public void ClearSortInfo()
        {
            int count = m_ActiveSortInfoList.Count;
            for (int i = 0; i < count; ++i)
            {
                m_ActiveSortInfoList[i].SetStateSilent(ESortType.None);
            }
            m_ActiveSortInfoList.Clear();
            TriggerOnSortChange();
        }

        public void ApplySort(TableSortInfo i_SortInfo)
        {
            if (!m_ActiveSortInfoList.Contains(i_SortInfo))
            {
                m_ActiveSortInfoList.Add(i_SortInfo);
                i_SortInfo.Display = Instantiate(m_SortDisplayElement, m_SortDisplayContainer, false);
                i_SortInfo.Display.gameObject.SetActive(true);

                TriggerOnSortChange();
            }
        }

        public void RemoveSort(TableSortInfo i_SortInfo)
        {
            if(m_ActiveSortInfoList.Remove(i_SortInfo))
            {
                if(i_SortInfo.Display)
                {
                    Destroy(i_SortInfo.Display.gameObject);
                    i_SortInfo.Display = null;
                }

                TriggerOnSortChange();
            }
        }

        public void TriggerOnSortChange()
        {
            if (SurvivorsTableScreen.Instance != null)
            {
                SurvivorsTableScreen.Instance.RegenerateSurvivorTable();
            }
        }


        private void UpdateListElements()
        {
            if (SurvivorsTableScreen.Instance != null)
            {
                SurvivorsTableScreen.Instance.UpdateListElementsDisplay();
            }
        }

        public void TriggerDisabledSortInfo(TableSortInfo i_SortInfo)
        {
            m_VisibleSortInfoList.Remove(i_SortInfo);
            RemoveSort(i_SortInfo);
            if (SurvivorsTableScreen.Instance != null)
            {
                SurvivorsTableScreen.Instance.UpdateListElementsDisplay();
            }
        }

        public void TriggerEnabledSortInfo(TableSortInfo i_SortInfo)
        {
            if(!m_VisibleSortInfoList.Contains(i_SortInfo))
            {
                m_VisibleSortInfoList.Add(i_SortInfo);
            }

            if (m_DeferedListElementsUpdate == null)
            {
                m_DeferedListElementsUpdate = StartCoroutine(DeferedListElementsUpdateFunc());
            }
        }
        
        private IEnumerator DeferedListElementsUpdateFunc()
        {
            yield return null;

            if (SurvivorsTableScreen.Instance != null)
            {
                SurvivorsTableScreen.Instance.UpdateListElementsDisplay();
            }

            m_DeferedListElementsUpdate = null;
        }
    }
}
