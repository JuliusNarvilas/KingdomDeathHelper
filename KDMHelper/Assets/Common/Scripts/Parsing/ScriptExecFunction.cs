/*
using Common.Parsing.Value;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Parsing
{
    public class ScriptExecFunction
    {
        IExpressionValueProvider m_targetValue;
        IExpressionValueProvider[] m_arguments;

        public static IExpressionValueProvider BuildOperator(ScriptExecParser parser, IExpressionValueProvider targetValue, string operation, IExpressionValueProvider[] arguments)
        {
            Type targetType = targetValue.GetValueType();
            if (targetType.IsPrimitive)
            {
                if (arguments.Length == 1)
                {
                    //find operaters
                    if (operation == "+")
                        return PrimitiveOperations.Make(parser, targetValue, arguments[0], PrimitiveOperationType.Addition);
                    else if (operation == "-")
                        return PrimitiveOperations.Make(parser, targetValue, arguments[0], PrimitiveOperationType.Subtraction);
                    else if (operation == "*")
                        return PrimitiveOperations.Make(parser, targetValue, arguments[0], PrimitiveOperationType.Multiplication);
                    else if (operation == "/")
                        return PrimitiveOperations.Make(parser, targetValue, arguments[0], PrimitiveOperationType.Division);
                }
            }

            parser.ThrowError("Unknown operation: " + operation);
            return null;
        }

        public static IExpressionValueProvider BuildMethod(ScriptExecParser parser, IExpressionValueProvider targetValue, string function, IExpressionValueProvider[] arguments)
        {
            Type targetType = targetValue.GetValueType();
            var funcInfo = targetType.GetMethod(function);
            if (funcInfo != null)
            {

                return new SimpleDelegateExpressionValue((IScriptErrorContext p) => {
                    int argCount = arguments.Length;
                    object[] args = new object[argCount];
                    for (int i = 0; i < argCount; i++)
                    {
                        args[i] = arguments[i].GetValue(p);
                    }
                    return funcInfo.Invoke(targetValue, args);
                }, funcInfo.ReturnType);
            }

            parser.ThrowError("Unknown function: " + function);
            return null;
        }


        public static IExpressionValueProvider BuildGetProperty(ScriptExecParser parser, IExpressionValueProvider targetValue, string property)
        {
            Type targetType = targetValue.GetValueType();

            {
                var prop = targetType.GetProperty(property);
                if (prop != null)
                {
                    return new SimpleDelegateExpressionValue((IScriptErrorContext p) => {
                        var targetObj = targetValue.GetValue(p);
                        if (targetObj == null)
                        {
                            p.ThrowError("Null ptr using property: " + property);
                            return null;
                        }
                        return prop.GetValue(targetObj, null);
                    }, prop.PropertyType);
                }
            }

            var field = targetType.GetField(property);
            if (field != null)
            {
                return new SimpleDelegateExpressionValue((IScriptErrorContext p) => {
                    var targetObj = targetValue.GetValue(p);
                    if(targetObj == null)
                    {
                        p.ThrowError("Null ptr using field: " + property);
                        return null;
                    }
                    return field.GetValue(targetObj);
                }, field.FieldType);
            }
            
            parser.ThrowError("Unknown function: " + property);
            return null;
        }

        public static IExpressionValueProvider BuildSetProperty(ScriptExecParser parser, IExpressionValueProvider lhs, string property, IExpressionValueProvider rhs)
        {
            Type targetType = lhs.GetValueType();
            Type givenType = rhs.GetValueType();

            {
                var prop = targetType.GetProperty(property);
                if (prop != null)
                {
                    if (prop.PropertyType.IsAssignableFrom(givenType))
                    {
                        return new SimpleDelegateExpressionValue((IScriptErrorContext p) => {
                            var targetObj = lhs.GetValue(p);
                            if (targetObj == null)
                            {
                                p.ThrowError("Null ptr using property: " + property);
                                return null;
                            }
                            var assignVal = rhs.GetValue(p);
                            prop.SetValue(targetObj, assignVal, null);
                            return VoidExpressionValue.Instance;
                        }, typeof(void));
                    }

                    parser.ThrowError("Incompatible assignment: " + property);
                    return null;
                }
            }

            var field = targetType.GetField(property);
            if (field != null)
            {
                if(field.FieldType.IsAssignableFrom(givenType))
                {
                    return new SimpleDelegateExpressionValue((IScriptErrorContext p) => {
                        var targetObj = lhs.GetValue(p);
                        if (targetObj == null)
                        {
                            p.ThrowError("Null ptr using property: " + property);
                            return null;
                        }
                        var assignVal = rhs.GetValue(p);
                        field.SetValue(targetObj, assignVal);
                        return VoidExpressionValue.Instance;
                    }, typeof(void));
                }

                parser.ThrowError("Incompatible assignment: " + property);
                return null;
            }

            parser.ThrowError("Unknown function: " + property);
            return null;
        }
    }
}

*/