
using UnityEngine;

namespace Common.Helpers
{
    public static class ConvertHelper
    {

        #region RegEx Constants

        private const string RX_ISHEX = @"(?<sign>[-+]?)(?<flag>0x|#|&H)(?<num>[\dA-F]+)(?<fractional>(\.[\dA-F]+)?)$";

        #endregion

        #region Color

        public static int ToInt(Color color)
        {
            return (Mathf.RoundToInt(color.a * 255) << 24) +
                   (Mathf.RoundToInt(color.r * 255) << 16) +
                   (Mathf.RoundToInt(color.g * 255) << 8) +
                   Mathf.RoundToInt(color.b * 255);
        }
        
        public static Color ToColor(int value)
        {
            const float toColorFraction = 1.0f / 255.0f;
            var a = (float)(value >> 24 & 0xFF) * toColorFraction;
            var r = (float)(value >> 16 & 0xFF) * toColorFraction;
            var g = (float)(value >> 8 & 0xFF) * toColorFraction;
            var b = (float)(value & 0xFF) * toColorFraction;
            return new Color(r, g, b, a);
        }


        public static Color ToColor(Color32 value)
        {
            const float toColorFraction = 1.0f / 255.0f;
            return new Color((float)value.r * toColorFraction,
                             (float)value.g * toColorFraction,
                             (float)value.b * toColorFraction,
                             (float)value.a * toColorFraction);
        }

        public static Color ToColor(Vector3 value)
        {

            return new Color((float)value.x,
                             (float)value.y,
                             (float)value.z);
        }

        public static Color ToColor(Vector4 value)
        {
            return new Color((float)value.x,
                             (float)value.y,
                             (float)value.z,
                             (float)value.w);
        }

        public static int ToInt(Color32 color)
        {
            return (color.a << 24) +
                   (color.r << 16) +
                   (color.g << 8) +
                   color.b;
        }

        public static Color32 ToColor32(int value)
        {
            byte a = (byte)(value >> 24 & 0xFF);
            byte r = (byte)(value >> 16 & 0xFF);
            byte g = (byte)(value >> 8 & 0xFF);
            byte b = (byte)(value & 0xFF);
            return new Color32(r, g, b, a);
        }

        public static Color32 ToColor32(string value)
        {
            return ToColor32(System.Convert.ToInt32(value));
        }

        public static Color32 ToColor32(Color value)
        {
            return new Color32((byte)(value.r * 255f),
                               (byte)(value.g * 255f),
                               (byte)(value.b * 255f),
                               (byte)(value.a * 255f));
        }

        public static Color32 ToColor32(Vector3 value)
        {
            return new Color32((byte)(value.x * 255f),
                               (byte)(value.y * 255f),
                               (byte)(value.z * 255f), 255);
        }

        public static Color32 ToColor32(Vector4 value)
        {
            return new Color32((byte)(value.x * 255f),
                               (byte)(value.y * 255f),
                               (byte)(value.z * 255f),
                               (byte)(value.w * 255f));
        }

        #endregion

        #region ToEnum

        public static bool TryToEnumOfType(System.Type enumType, object value, out System.Enum result)
        {
            try
            {
                result = System.Enum.Parse(enumType, System.Convert.ToString(value), true) as System.Enum;
                return true;
            }
            catch
            {
                result = default(System.Enum);
                return false;
            }
        }


        public static System.Enum ToEnumOfType(System.Type enumType, object value, System.Enum defaultVal = default(System.Enum))
        {
            System.Enum result;
            if (TryToEnumOfType(enumType, value, out result))
            {
                return result;
            }
            return defaultVal;
        }

        public static T ToEnum<T>(string val, T defaultValue = default(T)) where T : struct, System.IConvertible
        {
            T result;
            if (TryToEnum<T>(val, out result))
            {
                return result;
            }
            return defaultValue;
        }

        public static T ToEnum<T>(int val, T defaultValue = default(T)) where T : struct, System.IConvertible
        {
            T result;
            if (TryToEnum<T>(val, out result))
            {
                return result;
            }
            return defaultValue;
        }

        public static T ToEnum<T>(object val, T defaultValue = default(T)) where T : struct, System.IConvertible
        {
            return ToEnum<T>(System.Convert.ToString(val), defaultValue);
        }

        public static bool TryToEnum<T>(object val, out T result) where T : struct, System.IConvertible
        {
            return TryToEnum<T>(System.Convert.ToString(val), out result);
        }
        
        public static bool TryToEnum<T>(string val, out T result) where T : struct, System.IConvertible
        {
            try
            {
                result = (T)System.Enum.Parse(typeof(T), val, true);
                return true;
            }
            catch
            {
                result = default(T);
                return false;
            }
        }
        
        public static bool TryToEnum<T>(int val, out T result) where T : struct, System.IConvertible
        {
            object obj = val;
            if (System.Enum.IsDefined(typeof(T), obj))
            {
                result = (T)obj;
                return true;
            }
            result = default(T);
            return false;
        }

        #endregion
        
        

        /// <summary>
        /// System.Converts any string to a number with no errors.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="style"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        /// <remarks>
        /// TODO: I would also like to possibly include support for other number system bases. At least binary and octal.
        /// </remarks>
        public static double ToDouble(string value, System.Globalization.NumberStyles style, System.IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return 0d;

            style = style & System.Globalization.NumberStyles.Any;
            double dbl = 0;
            if (double.TryParse(value, style, provider, out dbl))
            {
                return dbl;
            }
            else
            {
                //test hex
                int i;
                bool isNeg = false;
                for (i = 0; i < value.Length; i++)
                {
                    if (value[i] == ' ' || value[i] == '+') continue;
                    if (value[i] == '-')
                    {
                        isNeg = !isNeg;
                        continue;
                    }
                    break;
                }

                if (i < value.Length - 1 &&
                        (
                        (value[i] == '#') ||
                        (value[i] == '0' && (value[i + 1] == 'x' || value[i + 1] == 'X')) ||
                        (value[i] == '&' && (value[i + 1] == 'h' || value[i + 1] == 'H'))
                        ))
                {
                    //is hex
                    style = (style & System.Globalization.NumberStyles.HexNumber) | System.Globalization.NumberStyles.AllowHexSpecifier;

                    if (value[i] == '#') i++;
                    else i += 2;
                    int j = value.IndexOf('.', i);

                    if (j >= 0)
                    {
                        long lng = 0;
                        long.TryParse(value.Substring(i, j - i), style, provider, out lng);

                        if (isNeg)
                            lng = -lng;

                        long flng = 0;
                        string sfract = value.Substring(j + 1).Trim();
                        long.TryParse(sfract, style, provider, out flng);
                        return System.Convert.ToDouble(lng) + System.Convert.ToDouble(flng) / System.Math.Pow(16d, sfract.Length);
                    }
                    else
                    {
                        string num = value.Substring(i);
                        long l;
                        if (long.TryParse(num, style, provider, out l))
                            return System.Convert.ToDouble(l);
                        else
                            return 0d;
                    }
                }
                else
                {
                    return 0d;
                }
            }
        }

        public static double ToDouble(string value, System.Globalization.NumberStyles style)
        {
            return ToDouble(value, style, null);
        }

        public static double ToDouble(string value)
        {
            return ToDouble(value, System.Globalization.NumberStyles.Any, null);
        }

        public static bool ToBool(object value)
        {
            if (value == null)
            {
                return false;
            }
            else if (value is System.IConvertible)
            {
                try
                {
                    return System.Convert.ToBoolean(value);
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return ToBool(value.ToString());
            }
        }

        /// <summary>
        /// Converts a string to boolean. Is FALSE greedy.
        /// A string is considered TRUE if it DOES meet one of the following criteria:
        /// 
        /// doesn't read blank: ""
        /// doesn't read false (not case-sensitive)
        /// doesn't read 0
        /// doesn't read off (not case-sensitive)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool ToBool(string str)
        {
            //str = (str + "").Trim().ToLower();
            //return !System.Convert.ToBoolean(string.IsNullOrEmpty(str) || str == "false" || str == "0" || str == "off");

            return !(string.IsNullOrEmpty(str) || str.Equals("false", System.StringComparison.OrdinalIgnoreCase) || str.Equals("0", System.StringComparison.OrdinalIgnoreCase) || str.Equals("off", System.StringComparison.OrdinalIgnoreCase));
        }
        

        public static bool IsHex(string value)
        {
            int i;
            for (i = 0; i < value.Length; i++)
            {
                if (value[i] == ' ' || value[i] == '+' || value[i] == '-') continue;

                break;
            }

            return (i < value.Length - 1 &&
                    (
                    (value[i] == '#') ||
                    (value[i] == '0' && (value[i + 1] == 'x' || value[i + 1] == 'X')) ||
                    (value[i] == '&' && (value[i + 1] == 'h' || value[i + 1] == 'H'))
                    ));
        }

        #region "BitArray"

        public static byte[] ToByteArray(this System.Collections.BitArray bits)
        {
            int numBytes = (int)System.Math.Ceiling(bits.Count / 8.0f);
            byte[] bytes = new byte[numBytes];

            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                {
                    int j = i / 8;
                    int m = i % 8;
                    bytes[j] |= (byte)(1 << m);
                }
            }

            return bytes;
        }

        #endregion
        
    }
}

