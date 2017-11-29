
using System;

namespace Common.Properties
{
    /// <summary>
    /// Simplest Property class for holding a value of any type.
    /// </summary>
    /// <typeparam name="T">Property value representation type.</typeparam>
    [Serializable]
    public class Property<T>
    {
        /// <summary>
        /// The value representation of this property.
        /// </summary>
        protected T m_Value;

        /*
        /// <summary>
        /// Default Property constructor that uses default(T) as the initialisation value.
        /// </summary>
        protected Property()
        {
            m_Value = default(T);
        }
        */

        /// <summary>
        /// Property constructor that initialises its value to the given input.
        /// </summary>
        /// <param name="i_Value">Initial Property value.</param>
        protected Property(T i_Value)
        {
            m_Value = i_Value;
        }

        /// <summary>
        /// Getter for the Propperty value.
        /// </summary>
        /// <returns>Property value representation.</returns>
        public T GetValue()
        {
            return m_Value;
        }
    }
}
