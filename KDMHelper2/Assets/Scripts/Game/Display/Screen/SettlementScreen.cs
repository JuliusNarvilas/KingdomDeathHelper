using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Display.Screen
{
    public class SettlementScreen : MonoBehaviour
    {
        [SerializeField]
        private RectTransform m_ContentFittingRect;
        [SerializeField]
        private float m_RectMinWidth = 200;

        private float m_LastScreenWidth = 0f;

        private void Start()
        {
            ApplicationManager.Instance.OnScreenSizeChange += UpdateContentFitting;
        }

        private void OnDestroy()
        {
            if (ApplicationManager.Instance != null)
            {
                ApplicationManager.Instance.OnScreenSizeChange -= UpdateContentFitting;
            }
        }

        public void UpdateContentFitting()
        {
			var parentRect = (RectTransform)m_ContentFittingRect.parent;
            float fullContentWidth = parentRect.rect.width;
            
            // stretch mode
            if (fullContentWidth > m_RectMinWidth)
            {
                // width correction that removes a scrollbar on some devices 
                float contentSizeError = 4;
                float newContentFitWidth = fullContentWidth - contentSizeError;
                // slowly introduce / remove the width adjustment when going between scroll mode and stretch mode
                float errorOffsetIntroduction = Mathf.Min((fullContentWidth - m_RectMinWidth) / contentSizeError, 1.0f);
                m_ContentFittingRect.sizeDelta = new Vector2(Mathf.Lerp(fullContentWidth, newContentFitWidth, errorOffsetIntroduction), 0);
            }
            else // scroll mode
            {
                // setting min allowed display width
                m_ContentFittingRect.sizeDelta = new Vector2(m_RectMinWidth, 0);
            }
        }
    }
}
