
namespace Common.Properties.Numerical.Specializations
{

    public class NumericalPropertyModifierReader<T> : INumericalPropertyModifierReader<T>
    {
        protected T m_Value;

        public NumericalPropertyModifierReader(T i_Value)
        {
            m_Value = i_Value;
        }

        public T Get()
        {
            return m_Value;
        }
    }

    public class NumericalPropertyIntModifierReader : NumericalPropertyModifierReader<int>
    {
        public NumericalPropertyIntModifierReader(int i_Value) : base(i_Value)
        { }
    }

    public class NumericalPropertyFloatModifierReader : NumericalPropertyModifierReader<float>
    {
        public NumericalPropertyFloatModifierReader(float i_Value) : base(i_Value)
        { }
    }

    public class NumericalPropertyLongModifierReader : NumericalPropertyModifierReader<long>
    {
        public NumericalPropertyLongModifierReader(long i_Value) : base(i_Value)
        { }
    }

    public class NumericalPropertyDoubleModifierReader : NumericalPropertyModifierReader<double>
    {
        public NumericalPropertyDoubleModifierReader(double i_Value) : base(i_Value)
        { }
    }


    public class NumericalPropertyIntModifier<TContext> :
        NumericalPropertyIntModifierReader, INumericalPropertyModifier<int, TContext, INumericalPropertyModifierReader<int>>
    {
        protected int m_Order;

        public NumericalPropertyIntModifier(int i_Value, int i_Order) : base(i_Value)
        {
            m_Order = i_Order;
        }

        public int GetOrder()
        {
            return m_Order;
        }

        public INumericalPropertyModifierReader<int> GetReader()
        {
            return this;
        }

        public void Update(ref NumericalPropertyChangeEventStruct<int, TContext, INumericalPropertyModifierReader<int>> i_EventData)
        {
            i_EventData.NewModifier += m_Value;
        }
    }

    public class NumericalPropertyFloatModifier<TContext> :
        NumericalPropertyFloatModifierReader, INumericalPropertyModifier<float, TContext, INumericalPropertyModifierReader<float>>
    {
        protected int m_Order;

        public NumericalPropertyFloatModifier(float i_Value, int i_Order) : base(i_Value)
        {
            m_Order = i_Order;
        }

        public int GetOrder()
        {
            return m_Order;
        }

        public INumericalPropertyModifierReader<float> GetReader()
        {
            return this;
        }

        public void Update(ref NumericalPropertyChangeEventStruct<float, TContext, INumericalPropertyModifierReader<float>> i_EventData)
        {
            i_EventData.NewModifier += m_Value;
        }
    }

    public class NumericalPropertyLongModifier<TContext> :
        NumericalPropertyLongModifierReader, INumericalPropertyModifier<long, TContext, INumericalPropertyModifierReader<long>>
    {
        protected int m_Order;

        public NumericalPropertyLongModifier(long i_Value, int i_Order) : base(i_Value)
        {
            m_Order = i_Order;
        }

        public int GetOrder()
        {
            return m_Order;
        }

        public INumericalPropertyModifierReader<long> GetReader()
        {
            return this;
        }

        public void Update(ref NumericalPropertyChangeEventStruct<long, TContext, INumericalPropertyModifierReader<long>> i_EventData)
        {
            i_EventData.NewModifier += m_Value;
        }
    }

    public class NumericalPropertyDoubleModifier<TContext> :
        NumericalPropertyDoubleModifierReader, INumericalPropertyModifier<double, TContext, INumericalPropertyModifierReader<double>>
    {
        protected int m_Order;

        public NumericalPropertyDoubleModifier(double i_Value, int i_Order) : base(i_Value)
        {
            m_Order = i_Order;
        }

        public int GetOrder()
        {
            return m_Order;
        }

        public INumericalPropertyModifierReader<double> GetReader()
        {
            return this;
        }

        public void Update(ref NumericalPropertyChangeEventStruct<double, TContext, INumericalPropertyModifierReader<double>> i_EventData)
        {
            i_EventData.NewModifier += m_Value;
        }
    }

}
