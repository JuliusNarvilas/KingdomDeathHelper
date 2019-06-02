using Common.Properties.Numerical.Data;

namespace Common.Properties.Numerical.Specializations
{
    public class ObservableExhaustibleNumericalPropertyInt<TContex> : ObservableExhaustibleNumericalProperty<int, TContex, INumericalPropertyModifierReader<int>>
    {
        public ObservableExhaustibleNumericalPropertyInt() : base(new NumericalPropertyIntData())
        { }
        public ObservableExhaustibleNumericalPropertyInt(INumericalPropertyData<int> i_Value) : base(i_Value)
        { }
    }

    public class ObservableExhaustibleNumericalPropertyFloat<TContex> : ObservableExhaustibleNumericalProperty<float, TContex, INumericalPropertyModifierReader<float>>
    {
        public ObservableExhaustibleNumericalPropertyFloat() : base(new NumericalPropertyFloatData())
        { }
        public ObservableExhaustibleNumericalPropertyFloat(INumericalPropertyData<float> i_Value) : base(i_Value)
        { }
    }

    public class ObservableExhaustibleNumericalPropertyLong<TContex> : ObservableExhaustibleNumericalProperty<long, TContex, INumericalPropertyModifierReader<long>>
    {
        public ObservableExhaustibleNumericalPropertyLong() : base(new NumericalPropertyLongData())
        { }
        public ObservableExhaustibleNumericalPropertyLong(INumericalPropertyData<long> i_Value) : base(i_Value)
        { }
    }

    public class ObservableExhaustibleNumericalPropertyDouble<TContex> : ObservableExhaustibleNumericalProperty<double, TContex, INumericalPropertyModifierReader<double>>
    {
        public ObservableExhaustibleNumericalPropertyDouble() : base(new NumericalPropertyDoubleData())
        { }
        public ObservableExhaustibleNumericalPropertyDouble(INumericalPropertyData<double> i_Value) : base(i_Value)
        { }
    }
}
