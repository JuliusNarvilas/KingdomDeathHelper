//Performs validation for number overflows and division by 0
//#define NUMERICAL_PROPERTY_DATA_VALIDATION

#if (DEBUG || UNITY_EDITOR)
#define NUMERICAL_PROPERTY_DATA_VALIDATION
#endif

namespace Common.Properties.Numerical.Data
{
    public class NumericalPropertyFloatData : INumericalPropertyData<float>
    {
        protected float m_Value;

        public NumericalPropertyFloatData()
        {
            m_Value = 0.0f;
        }

        public NumericalPropertyFloatData(float i_Value)
        {
            m_Value = i_Value;
        }


        public float Get()
        {
            return m_Value;
        }

        public void Set(float i_Value)
        {
            m_Value = i_Value;
        }

        public void Add(float i_Value)
        {
#if (NUMERICAL_PROPERTY_DATA_VALIDATION)
            float temp = m_Value + i_Value;
            Log.DebugLogErrorIf(
                ((temp <= m_Value) && (i_Value > 0)) ||
                ((temp >= m_Value) && (i_Value < 0)),
                "Number overflow: {0} + {1}.",
                m_Value,
                i_Value
            );
#endif
            m_Value += i_Value;
        }

        public void Substract(float i_Value)
        {
#if (NUMERICAL_PROPERTY_DATA_VALIDATION)
            float temp = m_Value - i_Value;
            Log.DebugLogErrorIf(
                ((temp <= m_Value) && (i_Value < 0)) ||
                ((temp >= m_Value) && (i_Value > 0)),
                "Number overflow: {0} - {1}.",
                m_Value,
                i_Value
            );
#endif
            m_Value -= i_Value;
        }

        public INumericalPropertyData<float> CreateZero()
        {
            return new NumericalPropertyFloatData(0.0f);
        }

        public void ToZero()
        {
            m_Value = 0.0f;
        }

        public int CompareTo(float i_Other)
        {
            return m_Value.CompareTo(i_Other);
        }
    }
}
