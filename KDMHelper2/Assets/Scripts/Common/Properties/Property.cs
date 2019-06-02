
using System;

namespace Common.Properties
{
    public delegate void PropertyChangeHandler<TValueType, TPropertyType>(TValueType i_OldValue, TValueType i_NewValue, TPropertyType i_Property) where TPropertyType : Property<TValueType>;

    public interface IObservablePropertySimpleSubscription
    {
        event Action<object> SimpleChangeSubscription;
    }

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
        public Property(T i_Value)
        {
            m_Value = i_Value;
        }

        protected Property()
        {
            m_Value = default(T);
        }

        /// <summary>
        /// Getter for the Propperty value.
        /// </summary>
        /// <returns>Property value representation.</returns>
        public T GetValue()
        {
            return m_Value;
        }

        public override string ToString()
        {
            return m_Value.ToString();
        }
    }
}
