//Performs validation for number overflows and division by 0
//#define NUMERICAL_PROPERTY_DATA_VALIDATION

#if (DEBUG || UNITY_EDITOR)
#define NUMERICAL_PROPERTY_DATA_VALIDATION
#endif

namespace Common.Properties.Numerical.Data
{
    public class NumericalPropertyLongData : INumericalPropertyData<long>
    {
        protected long m_Value;

        public NumericalPropertyLongData()
        {
            m_Value = 0;
        }

        public NumericalPropertyLongData(long i_Value)
        {
            m_Value = i_Value;
        }


        public long Get()
        {
            return m_Value;
        }

        public void Set(long i_Value)
        {
            m_Value = i_Value;
        }

        public void Add(long i_Value)
        {
#if (NUMERICAL_PROPERTY_DATA_VALIDATION)
            long temp = m_Value + i_Value;
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

        public void Substract(long i_Value)
        {
#if (NUMERICAL_PROPERTY_DATA_VALIDATION)
            long temp = m_Value - i_Value;
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

        public INumericalPropertyData<long> CreateZero()
        {
            return new NumericalPropertyLongData(0);
        }

        public void ToZero()
        {
            m_Value = 0L;
        }

        public int CompareTo(long i_Other)
        {
            return m_Value.CompareTo(i_Other);
        }
    }
}
