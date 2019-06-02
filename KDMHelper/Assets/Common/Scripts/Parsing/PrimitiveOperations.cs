
/*
using Common.Parsing.Value;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Parsing
{
    public enum PrimitiveOperationType
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    public static class PrimitiveOperations
    {

        public static IExpressionValueProvider Make(ScriptExecParser parser, IExpressionValueProvider lhs, IExpressionValueProvider rhs, PrimitiveOperationType type)
        {
            Type lhsType = lhs.GetValueType();
            Type rhsType = rhs.GetValueType();

            if (lhsType == typeof(int) && lhsType == rhsType)
            {
                if (!lhs.IsDynamic() && !rhs.IsDynamic())
                {
                    switch(type)
                    {
                        case PrimitiveOperationType.Addition:
                            return new SimpleConstantExpressionValue<int>((int)lhs.GetValue(parser) + (int)rhs.GetValue(parser));
                        case PrimitiveOperationType.Subtraction:
                            return new SimpleConstantExpressionValue<int>((int)lhs.GetValue(parser) - (int)rhs.GetValue(parser));
                        case PrimitiveOperationType.Multiplication:
                            return new SimpleConstantExpressionValue<int>((int)lhs.GetValue(parser) * (int)rhs.GetValue(parser));
                        case PrimitiveOperationType.Division:
                            int rhsValue = (int)rhs.GetValue(parser);
                            if (rhsValue == 0)
                            {
                                parser.ThrowError("Division By Zero.");
                                return new SimpleConstantExpressionValue<int>(0);
                            }
                            return new SimpleConstantExpressionValue<int>((int)lhs.GetValue(parser) / rhsValue);
                    }
                }
                else
                {
                    switch (type)
                    {
                        case PrimitiveOperationType.Addition:
                            return new SimpleDelegateExpressionValue( (IScriptErrorContext p) => { return (int)lhs.GetValue(p) + (int)rhs.GetValue(p); }, typeof(int));
                        case PrimitiveOperationType.Subtraction:
                            return new SimpleDelegateExpressionValue((IScriptErrorContext p) => { return (int)lhs.GetValue(p) + (int)rhs.GetValue(p); }, typeof(int));
                        case PrimitiveOperationType.Multiplication:
                            return new SimpleDelegateExpressionValue((IScriptErrorContext p) => { return (int)lhs.GetValue(p) + (int)rhs.GetValue(p); }, typeof(int));
                        case PrimitiveOperationType.Division:
                            return new SimpleDelegateExpressionValue((IScriptErrorContext p) => {
                                int rhsValue = (int)rhs.GetValue(parser);
                                if (rhsValue == 0)
                                {
                                    parser.ThrowError("Division By Zero.");
                                    return 0;
                                }
                                return (int)lhs.GetValue(parser) / rhsValue;
                            }, typeof(int));
                    }
                }
            }
            else
            {
                if (!lhs.IsDynamic() && !rhs.IsDynamic())
                {
                    switch (type)
                    {
                        case PrimitiveOperationType.Addition:
                            return new SimpleConstantExpressionValue<float>((float)lhs.GetValue(parser) + (float)rhs.GetValue(parser));
                        case PrimitiveOperationType.Subtraction:
                            return new SimpleConstantExpressionValue<float>((float)lhs.GetValue(parser) - (float)rhs.GetValue(parser));
                        case PrimitiveOperationType.Multiplication:
                            return new SimpleConstantExpressionValue<float>((float)lhs.GetValue(parser) * (float)rhs.GetValue(parser));
                        case PrimitiveOperationType.Division:
                            float rhsValue = (float)rhs.GetValue(parser);
                            if (rhsValue == 0)
                            {
                                parser.ThrowError("Division By Zero.");
                                return new SimpleConstantExpressionValue<float>(0);
                            }
                            return new SimpleConstantExpressionValue<float>((float)lhs.GetValue(parser) / rhsValue);
                    }
                }
                else
                {
                    switch (type)
                    {
                        case PrimitiveOperationType.Addition:
                            return new SimpleDelegateExpressionValue((IScriptErrorContext p) => { return (float)lhs.GetValue(p) + (float)rhs.GetValue(p); }, typeof(float));
                        case PrimitiveOperationType.Subtraction:
                            return new SimpleDelegateExpressionValue((IScriptErrorContext p) => { return (float)lhs.GetValue(p) + (float)rhs.GetValue(p); }, typeof(float));
                        case PrimitiveOperationType.Multiplication:
                            return new SimpleDelegateExpressionValue((IScriptErrorContext p) => { return (float)lhs.GetValue(p) + (float)rhs.GetValue(p); }, typeof(float));
                        case PrimitiveOperationType.Division:
                            return new SimpleDelegateExpressionValue((IScriptErrorContext p) => {
                                float rhsValue = (float)rhs.GetValue(parser);
                                if (rhsValue == 0)
                                {
                                    parser.ThrowError("Division By Zero.");
                                    return 0;
                                }
                                return (float)lhs.GetValue(parser) / rhsValue;
                            }, typeof(float));
                    }
                }
            }

            parser.ThrowError("Unknown Primitive Operation Make.");
            return new SimpleConstantExpressionValue<int>(0);
        }
    }
}

*/