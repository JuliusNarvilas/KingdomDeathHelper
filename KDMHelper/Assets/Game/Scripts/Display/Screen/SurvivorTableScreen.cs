using Common;
using Game.Display.Table;
using Game.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Display.Screen
{
    public class SurvivorTableScreen : MonoBehaviour
    {
        private static SurvivorTableScreen s_Instance;
        public static SurvivorTableScreen Instance { get { return s_Instance; } }

        [SerializeField]
        private Toggle m_ActiveCheckbox;
        [SerializeField]
        private Toggle m_RetiredCheckbox;
        [SerializeField]
        private Toggle m_DeadCheckbox;

        [SerializeField]
        private Toggle m_SkipHunt;

        [SerializeField]
        private ContentSizeFitter m_ContentFitting;

        [SerializeField]
        private SurvivorTableSortControl m_SortControl;

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

            if(s_Instance == null)
            {
                s_Instance = this;
            }
            else
            {
                Log.ProductionLogError("SurvivorListScreen already exists.");
            }

            //TODO: load survivors
        }

        private void OnDestroy()
        {
            if (s_Instance == this)
            {
                s_Instance = null;
            }
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
        

        public void UpdateListElements()
        {
            var parentRect = m_ContentFitting.transform.parent.GetComponent<RectTransform>();
            var fitterRect = m_ContentFitting.GetComponent<RectTransform>();
            if(fitterRect.rect.width > parentRect.rect.width )
            {
                m_ContentFitting.horizontalFit = ContentSizeFitter.FitMode.MinSize;
            }
            else
            {
                m_ContentFitting.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            }
        }
    }
}
