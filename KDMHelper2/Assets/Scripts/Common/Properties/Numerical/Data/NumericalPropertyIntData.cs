//Performs validation for number overflows and division by 0
//#define NUMERICAL_PROPERTY_DATA_VALIDATION

#if (DEBUG || UNITY_EDITOR)
#define NUMERICAL_PROPERTY_DATA_VALIDATION
#endif

namespace Common.Properties.Numerical.Data
{
    public class NumericalPropertyIntData : INumericalPropertyData<int>
    {
        protected int m_Value;

        public NumericalPropertyIntData()
        {
            m_Value = 0;
        }

        public NumericalPropertyIntData(int i_Value)
        {
            m_Value = i_Value;
        }


        public int Get()
        {
            return m_Value;
        }

        public void Set(int i_Value)
        {
            m_Value = i_Value;
        }

        public void Add(int i_Value)
        {
#if (NUMERICAL_PROPERTY_DATA_VALIDATION)
            int temp = m_Value + i_Value;
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

        public void Substract(int i_Value)
        {
#if (NUMERICAL_PROPERTY_DATA_VALIDATION)
            int temp = m_Value - i_Value;
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

        public INumericalPropertyData<int> CreateZero()
        {
            return new NumericalPropertyIntData(0);
        }

        public void ToZero()
        {
            m_Value = 0;
        }

        public int CompareTo(int i_Other)
        {
            return m_Value.CompareTo(i_Other);
        }
    }
}
