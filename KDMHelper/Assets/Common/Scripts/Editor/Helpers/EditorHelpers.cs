using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using Common.Helpers;

namespace Common
{
    public static class EditorHelpers
    {
        public const float DEFAULT_GUI_HEIGHT = 16;
        
        public enum EStringCasing
        {
            /// <summary>
            /// Matches string with casing and serializes to string with original enum casing.
            /// </summary>
            Strict,
            /// <summary>
            /// Matches string ignoring casing and serializes to string with original enum casing.
            /// </summary>
            LoosePreserve,
            /// <summary>
            /// Matches string ignoring casing and serializes to string with upper casing.
            /// </summary>
            LooseUpper,
            /// <summary>
            /// Matches string ignoring casing and serializes to string with lower casing.
            /// </summary>
            LooseLower
        }

        /// <summary>
        /// Default enum string converter for serializing enum strings as other string values and converting them back.
        /// This default varient just passes back the string.
        /// </summary>
        /// <param name="i_EnumName">Name of the enum.</param>
        /// <param name="i_Writing">
        /// If set to <c>true</c>, converts from enum string to some other string (for writing),
        /// otherwise conversion is made back to to the enum string (for reading).
        /// </param>
        /// <returns>Coonversion result.</returns>
        public static string DefaultEnumStringConverter(string i_EnumName, bool i_Writing)
        {
            return i_EnumName;
        }

        /// <summary>
        /// Helper function for displaying a child enum popup property for string data.
        /// </summary>
        /// <returns>Matched property or null if not found.</returns>
        /// <param name="i_Prop">Parent property.</param>
        /// <param name="i_Name">Name.</param>
        /// <param name="i_DisplayName">Display name.</param>
        /// <param name="i_EnumType">Enum type.</param>
        /// <param name="i_ContentArea">Content area reference of the container.</param>
        public static SerializedProperty CreateChildEnumGUIForName(
            this SerializedProperty i_Prop,
            string i_Name,
            string i_DisplayName,
            Type i_EnumType,
            EStringCasing i_Casing = EStringCasing.LoosePreserve,
            Func<string, bool, string> i_Converter = null
        )
        {
            Func<string, bool, string> conversion = i_Converter ?? DefaultEnumStringConverter;
            SerializedProperty matchedProperty = i_Prop.FindPropertyRelative(i_Name);
            if (matchedProperty == null)
            {
                //no match
                return null;
            }
            string oldSelectedId = matchedProperty.stringValue;
            Enum selectedEnum = null;
            if (string.IsNullOrEmpty(oldSelectedId))
            {
                selectedEnum = (Enum)Enum.Parse(i_EnumType, Enum.GetNames(i_EnumType)[0]);
            }
            else
            {
                try
                {
                    switch (i_Casing)
                    {
                        case EStringCasing.Strict:
                            selectedEnum = (Enum)Enum.Parse(i_EnumType, conversion(oldSelectedId, false), false);
                            break;
                        case EStringCasing.LoosePreserve:
                        case EStringCasing.LooseUpper:
                        case EStringCasing.LooseLower:
                        default:
                            selectedEnum = (Enum)Enum.Parse(i_EnumType, conversion(oldSelectedId, false), true);
                            break;
                    }
                }
                catch
                {
                    selectedEnum = null;
                }
                if (selectedEnum == null)
                {
                    //default to first option if parsing failed
                    selectedEnum = (Enum)Enum.Parse(i_EnumType, Enum.GetNames(i_EnumType)[0]);
                }
            }
            
            selectedEnum = EditorGUILayout.EnumPopup(i_DisplayName, selectedEnum);
            string resultString = selectedEnum.ToString();
            switch (i_Casing)
            {
                case EStringCasing.Strict:
                case EStringCasing.LoosePreserve:
                    matchedProperty.stringValue = conversion(resultString, true);
                    break;
                case EStringCasing.LooseUpper:
                    matchedProperty.stringValue = conversion(resultString.ToUpper(), true);
                    break;
                case EStringCasing.LooseLower:
                    matchedProperty.stringValue = conversion(resultString.ToLower(), true);
                    break;
                default:
                    matchedProperty.stringValue = conversion(resultString, true);
                    break;
            }
            return matchedProperty;
        }

        /// <summary>
        /// Helper function for displaying an enum popup property for enum data.
        /// </summary>
        /// <returns>Matched serialized property using given name.</returns>
        /// <param name="prop">Container property to find a matching child property in.</param>
        /// <param name="name">Name to match the property by.</param>
        /// <param name="displayName">Display name.</param>
        /// <param name="enumType">Enum type.</param>
        /// <param name="contentArea">Content area reference of the container.</param>
        public static SerializedProperty CreateChildEnumGUIForInt(this SerializedProperty prop, string name, string displayName, Type enumType, ref Rect contentArea)
        {
            SerializedProperty matchedProperty = prop.FindPropertyRelative(name);
            if (matchedProperty == null)
            {
                //something wrong
                return null;
            }
            int oldSelectedId = matchedProperty.enumValueIndex;
            Enum selectedEnum = (Enum)Enum.ToObject(enumType, oldSelectedId);
            if (!Enum.IsDefined(enumType, selectedEnum))
            {
                selectedEnum = (Enum)Enum.Parse(enumType, Enum.GetNames(enumType)[0]);
            }

            Rect enumUIArea = new Rect(contentArea.x, contentArea.y, contentArea.width, DEFAULT_GUI_HEIGHT);
            Rect labelUIArea = enumUIArea;
            labelUIArea.width *= 0.5f;
            enumUIArea.width *= 0.5f;
            enumUIArea.x += labelUIArea.width;
            contentArea.y += EditorGUIUtility.singleLineHeight;
            
            selectedEnum = EditorGUILayout.EnumPopup(displayName, selectedEnum);
            matchedProperty.enumValueIndex = Convert.ToInt32(selectedEnum);
            return matchedProperty;
        }

        /// <summary>
        /// Gets the object the property represents.
        /// </summary>
        /// <param name="prop">Serialized property.</param>
        /// <returns>Value object.</returns>
        public static object GetTargetObject(this SerializedProperty prop)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue_Imp(obj, elementName, index);
                }
                else
                {
                    obj = GetValue_Imp(obj, element);
                }
            }
            return obj;
        }

        private static object GetValue_Imp(object source, string name)
        {
            if (source == null)
                return null;
            var type = source.GetType();

            while (type != null)
            {
                var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null)
                    return f.GetValue(source);

                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null)
                    return p.GetValue(source, null);

                type = type.BaseType;
            }
            return null;
        }

        private static object GetValue_Imp(object source, string name, int index)
        {
            var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
            if (enumerable == null) return null;
            var enm = enumerable.GetEnumerator();
            //while (index-- >= 0)
            //    enm.MoveNext();
            //return enm.Current;

            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext()) return null;
            }
            return enm.Current;
        }
        
        public static T GetEnumValue<T>(this SerializedProperty prop, T defaultVal = default(T)) where T : struct, System.IConvertible
        {
            T result;
            if(prop.TryGetEnumValue<T>(out result))
            {
                return result;
            }
            return defaultVal;
        }

        public static bool TryGetEnumValue<T>(this SerializedProperty prop, out T result) where T : struct, System.IConvertible
        {
            if (prop == null)
            {
                result = default(T);
                return false;
            }
            try
            {
                return ConvertHelper.TryToEnum<T>(prop.intValue, out result);
            }
            catch
            {
                result = default(T);
                return false;
            }
        }

        public static System.Enum GetEnumValueOfType(this SerializedProperty prop, System.Type enumType, System.Enum defaultVal = default(System.Enum))
        {
            System.Enum result;
            if(prop.TryGetEnumValueOfType(enumType, out result))
            {
                return result;
            }
            return defaultVal;
        }

        public static bool TryGetEnumValueOfType(this SerializedProperty prop, System.Type enumType, out System.Enum result)
        {
            if (prop == null)
            {
                result = default(System.Enum);
                return false;
            }
            try
            {
                return ConvertHelper.TryToEnumOfType(enumType, prop.intValue, out result);
            }
            catch
            {
                result = default(System.Enum);
                return false;
            }
        }
    }
}