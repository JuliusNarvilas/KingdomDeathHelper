using Game.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screen.SurvivorList
{
    public class SurvivorListScreen : MonoBehaviour
    {
        [SerializeField]
        private Toggle m_ActiveCheckbox;
        [SerializeField]
        private Toggle m_RetiredCheckbox;
        [SerializeField]
        private Toggle m_DeadCheckbox;

        [SerializeField]
        private Toggle m_SkipHunt;

        [SerializeField]
        private SurvivorSortControl m_SortControl;

        private List<Survivor> m_Survivors;
        private IOrderedEnumerable<Survivor> m_FilteredList;

        private void Awake()
        {
            if (m_ActiveCheckbox != null)
            {
                m_ActiveCheckbox.onValueChanged.AddListener(FilterChanged);
            }
            if (m_RetiredCheckbox != null)
            {
                m_RetiredCheckbox.onValueChanged.AddListener(FilterChanged);
            }
            if (m_DeadCheckbox != null)
            {
                m_DeadCheckbox.onValueChanged.AddListener(FilterChanged);
            }
            if (m_SkipHunt != null)
            {
                m_SkipHunt.onValueChanged.AddListener(FilterChanged);
            }

            if (m_SortControl != null)
            {
                m_SortControl.OnChange = Sort;
            }

            //TODO: load survivors
        }
        
        private void FilterChanged(bool isActive)
        {
            Rescan();
        }

        private void Sort()
        {
            m_SortControl.Sort(m_FilteredList);
        }


        public void Rescan()
        {
            ELifeState lifeStateFlags = ELifeState.Unknown;
            if (m_ActiveCheckbox == null || m_ActiveCheckbox.isOn)
            {
                lifeStateFlags |= ELifeState.Active;
            }
            if (m_RetiredCheckbox == null || m_RetiredCheckbox.isOn)
            {
                lifeStateFlags |= ELifeState.Retired;
            }
            if (m_DeadCheckbox == null ||m_DeadCheckbox.isOn)
            {
                lifeStateFlags |= ELifeState.Dead;
            }

            bool skipHunt = m_SkipHunt == null || m_SkipHunt.isOn;

            m_FilteredList = m_Survivors.Where(x => {
                return
                    ((x.LifeState & lifeStateFlags) != ELifeState.Unknown) &&
                    x.SkipNextHunt == skipHunt;
            }).OrderBy(x => 1);

            Sort();
        }
        
    }
}
