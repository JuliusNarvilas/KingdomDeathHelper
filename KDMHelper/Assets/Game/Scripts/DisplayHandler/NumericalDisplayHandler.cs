using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Model;
using Game.Model.Display;
using UnityEngine;
using UnityEngine.UI;
using Game.Properties;
using Game.Properties.Modifiers;

namespace Game.DisplayHandler
{
    public class NumericalDisplayHandler : ValueDisplayHandler
    {
        [SerializeField]
        private Text m_name;

        [SerializeField]
        private Text m_textDisplay;
        [SerializeField]
        private InputField m_textControl;

        private List<NumericalModifierDisplayHandler> m_modifierDisplays;


        [SerializeField]
        private Button m_addButton;
        [SerializeField]
        private Button m_removeButton;


        public override void SetValue(NamedProperty val)
        {
            string valStr = string.Empty;
            string nameStr = string.Empty;
            KDMNumericalProperty numProp = null;
            KDMExhaustibleNumericalProperty exNumProp = null;


            if (val != null)
            {
                nameStr = val.GetName();
                numProp = val.Property as KDMNumericalProperty;
                if (numProp != null)
                {
                    valStr = numProp.GetValue().ToString();
                }
                else
                {
                    exNumProp = val.Property as KDMExhaustibleNumericalProperty;
                    if(exNumProp != null)
                    {
                        valStr = exNumProp.GetValue().ToString();
                    }
                }
            }

            if (m_name != null)
                m_name.text = nameStr;

            if (m_textDisplay != null)
                m_textDisplay.text = valStr;

            if (m_textControl != null)
                m_textControl.text = valStr;

            if (m_addButton != null)
            {
                m_addButton.onClick.RemoveAllListeners();
                if(numProp != null)
                {
                    m_addButton.onClick.AddListener(() => { numProp.SetBaseValue(numProp.GetBaseValue() + 1); });
                }
                else if(exNumProp != null)
                {
                    m_addButton.onClick.AddListener(() => { exNumProp.Restore(1); });
                }
            }

            if (m_removeButton != null)
            {
                m_removeButton.onClick.RemoveAllListeners();
                if (numProp != null)
                {
                    m_removeButton.onClick.AddListener(() => { numProp.SetBaseValue(numProp.GetBaseValue() - 1); });
                }
                else if (exNumProp != null)
                {
                    m_removeButton.onClick.AddListener(() => { exNumProp.Deplete(1); });
                }
            }
        }

        public override void Setup(ValueDisplayMode mode)
        {
            switch (mode)
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
                    if (m_textDisplay != null)
                        m_textDisplay.gameObject.SetActive(false);
                    if (m_textControl != null)
                        m_textControl.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
