using Common;
using Common.Properties.Enumeration;
using Game.DisplayHandler;
using Game.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Game.Scripts.DisplayHandler
{
    [Serializable]
    public class DisplayHandleRecord
    {
        public Type EvaluatedType;

        [StringInList(
            typeof(KDMNumericalProperty),
            typeof(KDMExhaustibleNumericalProperty),
            typeof(KDMStringProperty),
            typeof(KDMListProperty),
            typeof(KDMEnumProperty)
        )]

        public string TypeToDisplay;
        public ValueDisplayMode DisplayMode;
        public string Filter;
        public ValueDisplayHandler Handler;
    }

    [CreateAssetMenu(fileName = "DefaultFileName", menuName = "Create Display Handler DB", order = 42)]
    [Serializable]
    public class DisplayHandlerDB : ScriptableObject
    {
        public List<DisplayHandleRecord> Records;

        private bool m_TypesEvaluated = false;

        public void ReEvaluateTypes()
        {
            int count = Records.Count;
            for (int i = 0; i < count; ++i)
            {
                DisplayHandleRecord record = Records[i];
                if (record.Filter == string.Empty)
                    record.Filter = null;

                if (!string.IsNullOrEmpty(record.TypeToDisplay))
                    record.EvaluatedType = Type.GetType(record.TypeToDisplay, false);
                else
                    record.EvaluatedType = null;
            }
            m_TypesEvaluated = true;
        }

        public ValueDisplayHandler Find(Type type, ValueDisplayMode mode, string filter = null)
        {
            if(!m_TypesEvaluated)
            {
                ReEvaluateTypes();
            }
            if(filter == string.Empty)
            {
                filter = null;
            }

            int count = Records.Count;
            for (int i = 0; i < count; ++i)
            {
                DisplayHandleRecord record = Records[i];
                if (record.EvaluatedType == type && record.DisplayMode == mode && record.Filter == filter)
                {
                    return record.Handler;
                }
            }
            return null;
        }

        public ValueDisplayHandler Find<T>(ValueDisplayMode mode, string filter = null)
        {
            return Find(typeof(T), mode, filter);
        }
    }
}
