using Common;
using Game.Display.Table;
using Game.Model.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Display.Screen
{
    public class SurvivorsTableScreen : MonoBehaviour
    {
        private static SurvivorsTableScreen s_Instance;
        public static SurvivorsTableScreen Instance { get { return s_Instance; } }

        [SerializeField]
        private Toggle m_ActiveCheckbox;
        [SerializeField]
        private Toggle m_RetiredCheckbox;
        [SerializeField]
        private Toggle m_DeadCheckbox;
        [SerializeField]
        private Toggle m_SkipHunt;


        [SerializeField]
        private SurvivorsTableSortControl m_SortControl;
        [SerializeField]
        private RectTransform m_ContentFittingRect;


        private List<Survivor> m_Survivors;


        //TODO: remove serialization. used for testing right now
        [SerializeField]
        private List<SurvivorsTableItem> m_SurvivorsTableItems;
        private IOrderedEnumerable<Survivor> m_FilteredList;
        

        private float m_LastScreenWidth = 0f;
        private float m_FittingMinWidth = 0f;
        private float m_FittingAvailableWidth = 0f;
        private Coroutine m_UpdateFittingCoroutine;

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

            if(s_Instance == null)
            {
                s_Instance = this;
            }
            else
            {
                Log.ProductionLogError("SurvivorListScreen already exists.");
            }

            m_LastScreenWidth = UnityEngine.Screen.width;
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
            RegenerateSurvivorTable();
        }


        public void RegenerateSurvivorTable()
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

            m_SortControl.Sort(m_FilteredList);
        }


        private void OnEnable()
        {
            UpdateListElementsDisplay();
        }


        private void Update()
        {
            if (m_LastScreenWidth != UnityEngine.Screen.width)
            {
                m_LastScreenWidth = UnityEngine.Screen.width;
                UpdateContentFitting();
            }
        }

        public void UpdateListElementsDisplay()
        {   
            if(m_SortControl != null)
            {
                //list should be sorted in the right display order
                var visibleInfo = m_SortControl.GetVisibleSortInfo();
                int count = m_SurvivorsTableItems.Count;
                for (int i = 0; i < count; ++i)
                {
                    m_SurvivorsTableItems[i].DisplayElements(visibleInfo);
                }
            }

            UpdateContentFitting();
        }
        
        public void UpdateContentFitting()
        {
            if(m_UpdateFittingCoroutine != null)
            {
                StopCoroutine(m_UpdateFittingCoroutine);
                m_UpdateFittingCoroutine = null;
            }
            m_UpdateFittingCoroutine = StartCoroutine(UpdateContentFittingWhenReady());
        }

        public IEnumerator UpdateContentFittingWhenReady()
        {
            int attemptCounter = 20;
            float newFittingMinWidth = m_FittingMinWidth;
            float newFittingAvailableWidth = m_FittingAvailableWidth;
            while (newFittingMinWidth == m_FittingMinWidth && newFittingAvailableWidth == m_FittingAvailableWidth)
            {
                if (--attemptCounter < 0)
                {
                    yield break;
                }
                yield return null;
                newFittingMinWidth = LayoutUtility.GetMinWidth(m_ContentFittingRect);
                var parentRect = (RectTransform)m_ContentFittingRect.parent;
                newFittingAvailableWidth = parentRect.rect.width;
            }

            m_FittingMinWidth = newFittingMinWidth;
            m_FittingAvailableWidth = newFittingAvailableWidth;

            bool stretchMode = m_FittingAvailableWidth > m_FittingMinWidth;
            // stretch mode
            if (m_FittingAvailableWidth > m_FittingMinWidth)
            {
                // width correction that removes a scrollbar on some devices 
                //float contentSizeError = 4;
                //float newContentFitWidth = m_FittingAvailableWidth - contentSizeError;
                // slowly introduce / remove the width adjustment when going between scroll mode and stretch mode
                //float errorOffsetIntroduction = Mathf.Min((m_FittingAvailableWidth - m_FittingMinWidth) / contentSizeError, 1.0f);
                //m_ContentFittingRect.sizeDelta = new Vector2(Mathf.Lerp(m_FittingAvailableWidth, newContentFitWidth, errorOffsetIntroduction), 0);
                m_ContentFittingRect.sizeDelta = new Vector2(m_FittingAvailableWidth, 0);
            }
            else // scroll mode
            {
                m_ContentFittingRect.sizeDelta = new Vector2(m_FittingMinWidth, 0);
            }

            m_UpdateFittingCoroutine = null;
        }

    }
}
