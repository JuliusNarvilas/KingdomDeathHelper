using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
namespace Common.Parsing.Value
{
    public class VoidExpressionValue : IExpressionValueProvider
    {
        private VoidExpressionValue()
        { }

        public static readonly VoidExpressionValue Instance = new VoidExpressionValue();

        public object GetValue(IScriptErrorContext parser)
        {
            parser.ThrowError("Access of void type.");
            return null;
        }

        public Type GetValueType()
        {
            return typeof(void);
        }

        public bool IsDynamic()
        {
            return false;
        }
    }

    public class SimpleConstantExpressionValue<T> : IExpressionValueProvider
    {
        public SimpleConstantExpressionValue(T value = default(T))
        {
            Value = value;
        }
        
        public T Value;

        public object GetValue(IScriptErrorContext parser)
        {
            return Value;
        }

        public Type GetValueType()
        {
            return typeof(T);
        }

        public bool IsDynamic()
        {
            return false;
        }
    }


    public class SimpleDynamicExpressionValue<T> : IExpressionValueProvider
    {
        public SimpleDynamicExpressionValue(T value = default(T))
        {
            Value = value;
        }
        
        public T Value;

        public object GetValue(IScriptErrorContext parser)
        {
            return Value;
        }

        public Type GetValueType()
        {
            return typeof(T);
        }

        public bool IsDynamic()
        {
            return true;
        }
    }


    public class SimpleDelegateExpressionValue : IExpressionValueProvider
    {
        public SimpleDelegateExpressionValue(Func<IScriptErrorContext, object> function, Type returnType)
        {
            Function = function;
            ReturnType = returnType;
        }

        public readonly Func<IScriptErrorContext, object> Function;
        public readonly Type ReturnType;

        public object GetValue(IScriptErrorContext parser)
        {
            return Function(parser);
        }

        public Type GetValueType()
        {
            return ReturnType;
        }

        public bool IsDynamic()
        {
            return true;
        }
    }


}

*/