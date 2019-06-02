using Common.Properties.Numerical.Data;

namespace Common.Properties.Numerical.Specializations
{
    public class ObservableNumericalPropertyInt<TContex> : ObservableNumericalProperty<int, TContex, INumericalPropertyModifierReader<int>>
    {
        public ObservableNumericalPropertyInt() : base(new NumericalPropertyIntData())
        { }
        public ObservableNumericalPropertyInt(INumericalPropertyData<int> i_Value) : base(i_Value)
        { }
    }
    
    public class ObservableNumericalPropertyFloat<TContex> : ObservableNumericalProperty<float, TContex, INumericalPropertyModifierReader<float>>
    {
        public ObservableNumericalPropertyFloat() : base(new NumericalPropertyFloatData())
        { }
        public ObservableNumericalPropertyFloat(INumericalPropertyData<float> i_Value) : base(i_Value)
        { }
    }

    public class ObservableNumericalPropertyLong<TContex> : ObservableNumericalProperty<long, TContex, INumericalPropertyModifierReader<long>>
    {
        public ObservableNumericalPropertyLong() : base(new NumericalPropertyLongData())
        { }
        public ObservableNumericalPropertyLong(INumericalPropertyData<long> i_Value) : base(i_Value)
        { }
    }

    public class ObservableNumericalPropertyDouble<TContex> : ObservableNumericalProperty<double, TContex, INumericalPropertyModifierReader<double>>
    {
        public ObservableNumericalPropertyDouble() : base(new NumericalPropertyDoubleData())
        { }
        public ObservableNumericalPropertyDouble(INumericalPropertyData<double> i_Value) : base(i_Value)
        { }
    }
}
