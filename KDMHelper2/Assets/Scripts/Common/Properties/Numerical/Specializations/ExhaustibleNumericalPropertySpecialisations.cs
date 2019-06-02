using Common.Properties.Numerical.Data;

namespace Common.Properties.Numerical.Specializations
{
    public class ExhaustibleNumericalPropertyInt<TContext, TModifierReader> : ExhaustibleNumericalProperty<int, TContext, TModifierReader> where TModifierReader : INumericalPropertyModifierReader<int>
    {
        public ExhaustibleNumericalPropertyInt() : base(new NumericalPropertyIntData(0))
        { }
        public ExhaustibleNumericalPropertyInt(int i_Value) : base(new NumericalPropertyIntData(i_Value))
        { }
    }
    public class ExhaustibleNumericalPropertyInt<TContext> : ExhaustibleNumericalPropertyInt<TContext, INumericalPropertyModifierReader<int>>
    {
        public ExhaustibleNumericalPropertyInt() : base(0)
        { }
        public ExhaustibleNumericalPropertyInt(int i_Value) : base(i_Value)
        { }
    }

    public class ExhaustibleNumericalPropertyFloat<TContext, TModifierReader> : ExhaustibleNumericalProperty<float, TContext, TModifierReader> where TModifierReader : INumericalPropertyModifierReader<float>
    {
        public ExhaustibleNumericalPropertyFloat() : base(new NumericalPropertyFloatData(0.0f))
        { }
        public ExhaustibleNumericalPropertyFloat(float i_Value) : base(new NumericalPropertyFloatData(i_Value))
        { }
    }
    public class ExhaustibleNumericalPropertyFloat<TContext> : ExhaustibleNumericalPropertyFloat<TContext, INumericalPropertyModifierReader<float>>
    {
        public ExhaustibleNumericalPropertyFloat() : base(0)
        { }
        public ExhaustibleNumericalPropertyFloat(float i_Value) : base(i_Value)
        { }
    }

    public class ExhaustibleNumericalPropertyLong<TContext, TModifierReader> : ExhaustibleNumericalProperty<long, TContext, TModifierReader> where TModifierReader : INumericalPropertyModifierReader<long>
    {
        public ExhaustibleNumericalPropertyLong() : base(new NumericalPropertyLongData(0))
        { }
        public ExhaustibleNumericalPropertyLong(long i_Value) : base(new NumericalPropertyLongData(i_Value))
        { }
    }
    public class ExhaustibleNumericalPropertyLong<TContext> : ExhaustibleNumericalPropertyLong<TContext, INumericalPropertyModifierReader<long>>
    {
        public ExhaustibleNumericalPropertyLong() : base(0)
        { }
        public ExhaustibleNumericalPropertyLong(long i_Value) : base(i_Value)
        { }
    }

    public class ExhaustibleNumericalPropertyDouble<TContext, TModifierReader> : ExhaustibleNumericalProperty<double, TContext, TModifierReader> where TModifierReader : INumericalPropertyModifierReader<double>
    {
        public ExhaustibleNumericalPropertyDouble() : base(new NumericalPropertyDoubleData(0.0))
        { }
        public ExhaustibleNumericalPropertyDouble(double i_Value) : base(new NumericalPropertyDoubleData(i_Value))
        { }
    }
    public class ExhaustibleNumericalPropertyDouble<TContext> : ExhaustibleNumericalPropertyDouble<TContext, INumericalPropertyModifierReader<double>>
    {
        public ExhaustibleNumericalPropertyDouble() : base(0)
        { }
        public ExhaustibleNumericalPropertyDouble(double i_Value) : base(i_Value)
        { }
    }
}
