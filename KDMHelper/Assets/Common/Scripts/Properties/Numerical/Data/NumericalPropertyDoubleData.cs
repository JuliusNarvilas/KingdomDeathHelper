//Performs validation for number overflows and division by 0
//#define NUMERICAL_PROPERTY_DATA_VALIDATION

#if (DEBUG || UNITY_EDITOR)
#define NUMERICAL_PROPERTY_DATA_VALIDATION
#endif

namespace Common.Properties.Numerical.Data
{
    public class NumericalPropertyDoubleData : INumericalPropertyData<double>
    {
        protected double m_Value;

        public NumericalPropertyDoubleData()
        {
            m_Value = 0.0;
        }

        public NumericalPropertyDoubleData(double i_Value)
        {
            m_Value = i_Value;
        }


        public double Get()
        {
            return m_Value;
        }

        public void Set(double i_Value)
        {
            m_Value = i_Value;
        }

        public void Add(double i_Value)
        {
#if (NUMERICAL_PROPERTY_DATA_VALIDATION)
            double temp = m_Value + i_Value;
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

        public void Substract(double i_Value)
        {
#if (NUMERICAL_PROPERTY_DATA_VALIDATION)
            double temp = m_Value - i_Value;
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

        public INumericalPropertyData<double> CreateZero()
        {
            return new NumericalPropertyDoubleData(0.0);
        }

        public void ToZero()
        {
            m_Value = 0.0;
        }

        public int CompareTo(double i_Other)
        {
            return m_Value.CompareTo(i_Other);
        }
    }
}
