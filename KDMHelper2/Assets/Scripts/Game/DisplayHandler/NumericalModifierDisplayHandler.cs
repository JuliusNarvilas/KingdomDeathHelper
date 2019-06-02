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
    public class NumericalModifierDisplayHandler : MonoBehaviour
    {
        [SerializeField]
        private Text m_name;

        [SerializeField]
        private Text m_value;

        [SerializeField]
        private Text m_description;



        public void SetValue(KDMNumericalPropertyModifierReader val)
        {
            string valStr = string.Empty;
            string nameStr = string.Empty;
            string descStr = string.Empty;

            if (val != null)
            {
                nameStr = val.Name;
                valStr = val.Value.ToString();
                if (val.Description != null)
                    descStr = val.Description;
            }

            if (m_name != null)
                m_name.text = nameStr;

            if (m_value != null)
                m_value.text = valStr;

            if (m_description != null)
                m_description.text = descStr;
        }

    }
}
