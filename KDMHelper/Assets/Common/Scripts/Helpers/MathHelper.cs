using System;

namespace Common.Helpers
{
    /// <summary>
    /// Contains a collection of extentions and helper functions for math related areas.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// The default floating point equal error margin
        /// </summary>
        public const float DEFAULT_FLOAT_EQUALITY_ERROR_MARGIN = 0.000001f;

        /// <summary>
        /// Floating point number equality comparison with margin of error.
        /// </summary>
        /// <param name="i_NumberA">The number a.</param>
        /// <param name="i_NumberB">The number b.</param>
        /// <param name="i_ErrorMargin">Inclusive margin of error limit.</param>
        /// <returns>The equality result.</returns>
        public static bool EqualsWithMarginF(float i_NumberA, float i_NumberB, float i_ErrorMargin = DEFAULT_FLOAT_EQUALITY_ERROR_MARGIN)
        {
            Log.DebugAssert(i_ErrorMargin >= 0, "MathHelper.EqualsWithMarginF has invalid error marging: {0} >= 0", i_ErrorMargin);
            return Math.Abs(i_NumberA - i_NumberB) <= i_ErrorMargin;
        }


        /// <summary>
        /// Floating point number equality comparison with margin of error.
        /// </summary>
        /// <param name="i_NumberA">The number a.</param>
        /// <param name="i_NumberB">The number b.</param>
        /// <param name="i_ErrorMargin">The margin of error.</param>
        /// <returns>The equality result.</returns>
        public static bool EqualsWithMargin(double i_NumberA, double i_NumberB, float i_ErrorMargin = DEFAULT_FLOAT_EQUALITY_ERROR_MARGIN)
        {
            Log.DebugAssert(i_ErrorMargin >= 0, "MathHelper.EqualsWithMarginF has invalid error marging: {0} >= 0", i_ErrorMargin);
            return Math.Abs(i_NumberA - i_NumberB) <= i_ErrorMargin;
        }


        /// <summary>
        /// Comparison for a numerical value to be in a given range.
        /// </summary>
        /// <param name="i_Data">The value to compare.</param>
        /// <param name="i_Min">The inclusive minimum range value.</param>
        /// <param name="i_Max">The inclusive maximum range value.</param>
        /// <returns>Returns true is value is in range and <c>false</c> otherwise.</returns>
        public static bool InRange(int i_Data, int i_Min, int i_Max)
        {
            return (i_Data >= i_Min) && (i_Data <= i_Max);
        }

        /// <summary>
        /// Comparison for a numerical value to be in a given range.
        /// </summary>
        /// <param name="i_Data">The value to compare.</param>
        /// <param name="i_Min">The inclusive minimum range value.</param>
        /// <param name="i_Max">The inclusive maximum range value.</param>
        /// <returns><c>true</c> if value is in range and false otherwise.</returns>
        public static bool InRangeF(float i_Data, float i_Min, float i_Max)
        {
            return (i_Data >= i_Min) && (i_Data <= i_Max);
        }

        /// <summary>
        /// Comparison for a numerical value to be in a given range.
        /// </summary>
        /// <param name="i_Data">The value to compare.</param>
        /// <param name="i_Min">The inclusive minimum range value.</param>
        /// <param name="i_Max">The inclusive maximum range value.</param>
        /// <returns><c>true</c> if value is in range and <c>false</c> otherwise.</returns>
        public static bool InRange(double i_Data, double i_Min, double i_Max)
        {
            return (i_Data >= i_Min) && (i_Data <= i_Max);
        }

        //from http://referencesource.microsoft.com/#mscorlib/system/tuple.cs,9124c4bea9ab0199        
        /// <summary>
        /// Combines two hash codes based on microsoft Tuple class functionality:
        /// <a href="http://referencesource.microsoft.com/#mscorlib/system/tuple.cs,9124c4bea9ab0199">http://referencesource.microsoft.com/#mscorlib/system/tuple.cs,9124c4bea9ab0199</a>
        /// </summary>
        /// <param name="i_Hash1">The first hash code.</param>
        /// <param name="i_Hash2">The second hash code.</param>
        /// <returns>Combined hash code.</returns>
        public static int CombineHashCodes(int i_Hash1, int i_Hash2)
        {
            return (((i_Hash1 << 5) + i_Hash1) ^ i_Hash2);
        }

        public static int GetLowestBitPosition(int i_Value)
        {
            int bitsCount = sizeof(int) * 8;
            for (int i = 0; i < bitsCount; ++i)
            {
                if ((i_Value & (1 << i)) != 0)
                {
                    return i;
                }
            }
            return -1;
        }

        public static int GetHighestBitPosition(int i_Value)
        {
            int bitsCount = sizeof(int) * 8;
            for (int i = bitsCount - 1; i >= 0; ++i)
            {
                if ((i_Value & (1 << i)) != 0)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
