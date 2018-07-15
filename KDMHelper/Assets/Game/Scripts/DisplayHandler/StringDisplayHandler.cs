using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Model;
using UnityEngine;
using UnityEngine.UI;
using Game.Properties;
using Game.Model.Display;

namespace Game.DisplayHandler
{
    public class StringDisplayHandler : ValueDisplayHandler
    {
        [SerializeField]
        private Text m_name;

        [SerializeField]
        private Text m_textDisplay;
        [SerializeField]
        private InputField m_textControl;

        public override void SetValue(NamedProperty val)
        {
            string valStr = string.Empty;
            string nameStr = string.Empty;

            if (val != null)
            {
                nameStr = val.GetName();
                KDMStringProperty prop = val.Property as KDMStringProperty;
                if (prop != null)
                {
                    valStr = prop.GetValue();
                }
            }

            if(m_name != null)
                m_name.text = nameStr;

            if(m_textDisplay != null)
                m_textDisplay.text = valStr;

            if(m_textControl != null)
                m_textControl.text = valStr;
        }

        public override void Setup(ValueDisplayMode mode)
        {
            switch(mode)
            {
                case ValueDisplayMode.CellDisplay:
                case ValueDisplayMode.RowDisplay:
                    if (m_textDisplay != null)
                        m_textDisplay.gameObject.SetActive(true);
                    if (m_textControl != null)
                        m_textControl.gameObject.SetActive(false);
                    break;
                case ValueDisplayMode.CellControl:
                case ValueDisplayMode.RowControl:
                case ValueDisplayMode.Detailed:
                    if (m_textDisplay != null)
                        m_textDisplay.gameObject.SetActive(false);
                    if (m_textControl != null)
                        m_textControl.gameObject.SetActive(true);
                    break;
                default:
                    if(m_textDisplay != null)
                        m_textDisplay.gameObject.SetActive(false);
                    if (m_textControl != null)
                        m_textControl.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
