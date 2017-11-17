using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Popup
{
    public class PopupDisplayElement : MonoBehaviour
    {
        [SerializeField]
        private Text m_Label;
        [SerializeField]
        private Text m_Content;
        [SerializeField]
        private Image m_Image;

        public string Label
        {
            get { return m_Label != null ? m_Label.text : null; }
            set { if (m_Label != null) m_Label.text = value; }
        }

        public string Content
        {
            get { return m_Content != null ? m_Content.text : null; }
            set { if(m_Content != null) m_Content.text = value; }
        }
        
        public Sprite Image
        {
            get { return m_Image != null ? m_Image.sprite : null; }
            set { if (m_Image != null) m_Image.sprite = value; }
        }
    }
}
