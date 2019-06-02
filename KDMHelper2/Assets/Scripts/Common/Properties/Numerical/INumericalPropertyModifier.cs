using System.Collections.Generic;

namespace Common.Properties.Numerical
{
    /// <summary>
    /// An interface for numerical property modifiers.
    /// </summary>
    /// <typeparam name="TNumerical">Numerical property type.</typeparam>
    /// <typeparam name="TContext">Change context type.</typeparam>
    /// <typeparam name="TModifierReader">Reader interface for a modifier.</typeparam>
    public interface INumericalPropertyModifier<TNumerical, TContext, TModifierReader> where TModifierReader : INumericalPropertyModifierReader<TNumerical>
    {
        /// <summary>
        /// Processing of the change events.
        /// </summary>
        /// <param name="i_EventData">Data representing the change event.</param>
        void Update(ref NumericalPropertyChangeEventStruct<TNumerical, TContext, TModifierReader> i_EventData);
        /// <summary>
        /// Getter for the ordering value for modifiers to be applied in.
        /// </summary>
        /// <returns>Order value.</returns>
        int GetOrder();
        /// <summary>
        /// Getter for the modifier reader interface.
        /// </summary>
        /// <returns>The modifier reader interface.</returns>
        TModifierReader GetReader(TContext i_Context);
    }


    /// <summary>
    /// A simple numerical property modifier reader interface.
    /// </summary>
    /// <typeparam name="TNumerical">Type of the numerical data.</typeparam>
    public interface INumericalPropertyModifierReader<TNumerical>
    {
        TNumerical GetModifier();
    }

    /// <summary>
    /// Comparer for sorting modifiers based on the order they should be applied in.
    /// </summary>
    /// <typeparam name="TNumerical">Numerical property type.</typeparam>
    /// <typeparam name="TContext">Change context type.</typeparam>
    /// <typeparam name="TModifierReader">Reader interface for a modifier.</typeparam>
    public class NumericalPropertyModifierSortComparer<TNumerical, TContext, TModifierReader> : IComparer<INumericalPropertyModifier<TNumerical, TContext, TModifierReader>> where TModifierReader : INumericalPropertyModifierReader<TNumerical>
    {
        /// <summary>
        /// Compares the specified property modifiers' orders.
        /// </summary>
        /// <param name="i_ObjA">First modifier to compare.</param>
        /// <param name="i_ObjB">Second modifier to compare.</param>
        /// <returns>Returns 1, 0 or -1 depending if the second modifier is less than, equal to or greater then the first one.</returns>
        public int Compare(INumericalPropertyModifier<TNumerical, TContext, TModifierReader> i_ObjA, INumericalPropertyModifier<TNumerical, TContext, TModifierReader> i_ObjB)
        {
            Log.DebugAssert(i_ObjA != null && i_ObjB != null, "Invalid null parameter.");
            return i_ObjA.GetOrder().CompareTo(i_ObjB.GetOrder());
        }
    }
}
