using System;
using UnityEngine;

namespace Common.IO
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class AssetReferenceTypeAttribute : PropertyAttribute
    {
        public readonly Type Type;

        public AssetReferenceTypeAttribute(Type i_Type)
        {
            if (!typeof(UnityEngine.Object).IsAssignableFrom(i_Type))
                throw new ArgumentException("The type argument for a AssetReferenceTypeAttribute must be derived from UnityEngine.Object");

            Type = i_Type;
        }
    }
}
