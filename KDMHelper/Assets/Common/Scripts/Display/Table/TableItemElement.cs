using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Display.Table
{
    public enum ETableItemElementType
    {
        Text,
        Image
    }

    [Serializable]
    public struct TableItemElementImageOption
    {
        public string Key;
        public Sprite Sprite;
    }

    public class TableItemElement : MonoBehaviour
    {
        [SerializeField]
        private ETableItemElementType m_TableItemElementType;
        [SerializeField]
        private UnityEngine.UI.Text m_Text;
        [SerializeField]
        private string m_DefaultText;

        [SerializeField]
        private Image m_Image;
        [SerializeField]
        private Sprite m_DefaultSprite;
        [SerializeField]
        private List<TableItemElementImageOption> m_ImageOptions;


        public void SetValue(string i_Value)
        {
            switch(m_TableItemElementType)
            {
                case ETableItemElementType.Text:
                    if (string.IsNullOrEmpty(i_Value))
                    {
                        m_Text.text = m_DefaultText;
                    }
                    else
                    {
                        m_Text.text = i_Value;
                    }
                    break;
                case ETableItemElementType.Image:
                    {
                        m_Image.sprite = m_DefaultSprite;
                        int count = m_ImageOptions.Count;
                        for(int i = 0; i < count; ++i)
                        {
                            if(m_ImageOptions[i].Key == i_Value)
                            {
                                m_Image.sprite = m_ImageOptions[i].Sprite;
                                break;
                            }
                        }
                    }
                    break;
            }
        }
    }
}
