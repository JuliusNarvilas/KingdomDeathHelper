using UnityEngine;
using System;
using System.Reflection;

namespace Common
{
    public class EnumFlagAttribute : PropertyAttribute
    {
        public delegate int ConversionToFlagsDelegate(int value);
        public delegate int ConversionToValueDelegate(int value, int newFlags);
        public string EnumName;
        public ConversionToFlagsDelegate ConverterToFlags;
        public ConversionToValueDelegate ConverterToValue;

        public EnumFlagAttribute()
        {
            EnumName = null;
            ConverterToFlags = DefaultConverterToFlags;
            ConverterToValue = DefaultConverterToValue;
        }

        public EnumFlagAttribute(string name)
        {
            EnumName = name;
            ConverterToFlags = DefaultConverterToFlags;
            ConverterToValue = DefaultConverterToValue;
        }

        public EnumFlagAttribute(string name, Type converterType, string toFlagConverterName, string toValueConverterName)
        {
            EnumName = name;
            var toFlagsFunction = converterType.GetMethod(toFlagConverterName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            var toValueFunction = converterType.GetMethod(toValueConverterName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            ConverterToFlags = (ConversionToFlagsDelegate)Delegate.CreateDelegate(typeof(ConversionToFlagsDelegate), toFlagsFunction);
            ConverterToValue = (ConversionToValueDelegate)Delegate.CreateDelegate(typeof(ConversionToValueDelegate), toValueFunction);
        }

        public EnumFlagAttribute(Type converterType, string toFlagConverterName, string toValueConverterName)
        {
            EnumName = null;
            var toFlagsFunction = converterType.GetMethod(toFlagConverterName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            var toValueFunction = converterType.GetMethod(toValueConverterName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            ConverterToFlags = (ConversionToFlagsDelegate)Delegate.CreateDelegate(typeof(ConversionToFlagsDelegate), toFlagsFunction);
            ConverterToValue = (ConversionToValueDelegate)Delegate.CreateDelegate(typeof(ConversionToValueDelegate), toValueFunction);
        }

        private int DefaultConverterToFlags(int value)
        {
            return value;
        }
        private int DefaultConverterToValue(int value, int newFlags)
        {
            return value;
        }
    }
}
