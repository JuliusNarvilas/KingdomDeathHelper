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

        private void Update()
        {
            if (m_LastScreenWidth != UnityEngine.Screen.width)
            {
                m_LastScreenWidth = UnityEngine.Screen.width;
				UpdateListElementsDisplay();
            }
        }

		public void UpdateListElementsDisplay()
        {
			var parentRect = (RectTransform)m_ContentFittingRect.parent;
            float fullContentWidth = parentRect.rect.width;
            
            if (fullContentWidth > m_RectMinWidth)
            {
                float contentSizeError = 4;
                float newContentFitWidth = fullContentWidth - contentSizeError;
                float errorOffsetIntroduction = Mathf.Min((fullContentWidth - m_RectMinWidth) / contentSizeError, 1.0f);
                m_ContentFittingRect.sizeDelta = new Vector2(Mathf.Lerp(fullContentWidth, newContentFitWidth, errorOffsetIntroduction), 0);
            }
            else
            {
                m_ContentFittingRect.sizeDelta = new Vector2(m_RectMinWidth, 0);
            }
        }
    }
}
