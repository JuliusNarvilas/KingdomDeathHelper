﻿using System;
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
        Transform m_modifierPlacement;

        [SerializeField]
        private Button m_addButton;
        [SerializeField]
        private Button m_substractButton;

        [SerializeField]
        private NumericalModifierDisplayHandler m_modifierPrototype;

        public override void SetValue(NamedProperty val)
        {
            string valStr = string.Empty;
            string nameStr = string.Empty;
            KDMNumericalProperty numProp = null;

            if (val != null)
            {
                nameStr = val.GetName();
                numProp = val.Property as KDMNumericalProperty;
                if (numProp != null)
                {
                    valStr = numProp.GetValue().ToString();
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
            }

            if (m_substractButton != null)
            {
                m_substractButton.onClick.RemoveAllListeners();
                if (numProp != null)
                {
                    m_substractButton.onClick.AddListener(() => { numProp.SetBaseValue(numProp.GetBaseValue() - 1); });
                }
            }


            foreach (var modHandler in m_modifierDisplays)
            {
                Destroy(modHandler);
            }
            m_modifierDisplays.Clear();

            if (numProp != null)
            {
                int modCount = numProp.GetModifierCount();
                for (int i = 0; i < modCount; i++)
                {
                    KDMNumericalPropertyModifierReader mod = numProp.GetModifier(i);
                    if(mod != null)
                    {
                        //var modDisplay = Instantiate(m_modifierDisplayPrototype, m_modifierPlacement);
                        //modDisplay.SetValue(mod);
                    }
                }
            }
        }
        
    }
}