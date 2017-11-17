using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Popup
{
    public class PopupDisplayList : PopupDisplay
    {
        [SerializeField]
        private PopupDisplayElement m_DisplayElementPrefab;
        [SerializeField]
        private Transform m_DisplayElementContainer;

        private List<PopupDisplayElement> m_DispkayElements = new List<PopupDisplayElement>();
        public List<PopupDisplayElement> DispkayElements { get { return m_DispkayElements; } }

        public PopupDisplayElement Add(string content, Sprite image = null)
        {
            var newElement = Instantiate(m_DisplayElementPrefab, m_DisplayElementContainer);
            newElement.Content = content;
            newElement.Image = image;
            m_DispkayElements.Add(newElement);
            return newElement;
        }
    }
}
