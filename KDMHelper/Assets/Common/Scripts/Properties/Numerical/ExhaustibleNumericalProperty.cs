using Common.Properties.Numerical.Data;

namespace Common.Properties.Numerical
{
    /// <summary>
    /// An implementation of numerical property for representing values that can be depleated (like health).
    /// </summary>
    /// <remarks>
    /// This property can be depleated by an amount within the range [0; base value + modifiers].
    /// Overall depletion amount can not influence the property into the negative value or be less than 0.
    /// Even if base value + modifiers result in a negative number, the depletion could only amount to 0.
    /// </remarks>
    /// <typeparam name="TNumerical">Numerical property type.</typeparam>
    /// <typeparam name="TContext">Change context type.</typeparam>
    /// <typeparam name="TModifierReader">Reader interface for a modifier.</typeparam>
    /// <seealso cref="Common.Properties.Numerical.NumericalProperty{TNumerical, TContext, TModifierReader}" />
    public class ExhaustibleNumericalProperty<TNumerical, TContext, TModifierReader> :
        NumericalProperty<TNumerical, TContext, TModifierReader>
        where TModifierReader : INumericalPropertyModifierReader<TNumerical>
    {
        /// <summary>
        /// The depletion amount tracker.
        /// </summary>
        protected TNumerical m_Depletion;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExhaustibleNumericalProperty{TNumerical, TContext, TModifierReader}"/> class.
        /// </summary>
        /// <param name="i_Value">Initial value.</param>
        public ExhaustibleNumericalProperty(INumericalPropertyData<TNumerical> i_Value) : base(i_Value)
        {
            m_Depletion = m_DataZero.Get();
        }

        /// <summary>
        /// Gets the depletion from the max value.
        /// </summary>
        /// <returns>The depletion.</returns>
        public TNumerical GetDepletion()
        {
            return m_Depletion;
        }

        /// <summary>
        /// Gets the maximum value currently availablue for this property.
        /// </summary>
        /// <returns>Max value.</returns>
        public TNumerical GetMax()
        {
            m_DataZero.Set(m_BaseValue);
            m_DataZero.Add(m_FinalModifier);
            TNumerical max = m_DataZero.Get();
            m_DataZero.ToZero();
            return max;
        }

        /// <summary>
        /// Depletes the property further from the current max value.
        /// </summary>
        /// <param name="i_Depletion">The depletion amount.</param>
        /// <param name="i_Context">The context.</param>
        public void Deplete(TNumerical i_Depletion, TContext i_Context = default(TContext))
        {
            m_DataZero.Set(m_Depletion);
            m_DataZero.Add(i_Depletion);
            m_Depletion = m_DataZero.Get();
            m_DataZero.ToZero();

            ENumericalPropertyChangeType changeTypeMask = ENumericalPropertyChangeType.Deplete;
            UpdateInternal(changeTypeMask, i_Context);
        }

        /// <summary>
        /// Restores the property towards the current max value.
        /// </summary>
        /// <param name="i_Restoration">The restoration amount.</param>
        /// <param name="i_Context">The context.</param>
        public void Restore(TNumerical i_Restoration, TContext i_Context = default(TContext))
        {
            m_DataZero.Substract(i_Restoration);
            Deplete(m_DataZero.Get(), i_Context);
        }

        protected void UpdateExhaustableModifiedValue()
        {
            //clamp depletion to be greater or equal to 0
            if(m_DataZero.CompareTo(m_Depletion) > 0)
            {
                m_Depletion = m_DataZero.Get();
            }
            TNumerical zero = m_DataZero.Get();
            m_DataZero.Set(m_BaseValue);
            m_DataZero.Add(m_FinalModifier);
            //clamp depletion to end at max
            if(m_DataZero.CompareTo(m_Depletion) < 0)
            {
                //clamp depletion to 0 if max is negative
                m_Depletion = (m_DataZero.CompareTo(zero) <= 0) ? zero : m_DataZero.Get();
            }
            m_DataZero.Substract(m_Depletion);
            m_Value = m_DataZero.Get();
            m_DataZero.ToZero();

            Log.DebugLog("Numerical property value updated to {0}: {1} + {2} - {3}.", m_Value, m_BaseValue, m_FinalModifier, m_Depletion);
        }

        /// <summary>
        /// Instance update logic that can be overriden and customised.
        /// </summary>
        /// <param name="i_ChangeTypeMask">A mask of change type flags describing this update event.</param>
        /// <param name="i_Context">The change context.</param>
        protected override void UpdateInternal(ENumericalPropertyChangeType i_ChangeTypeMask, TContext i_Context)
        {
            if ((m_Modifiers.Count > 0))
            {
                if (!m_Updating)
                {
                    m_Updating = true;
                }
                else
                {
                    i_ChangeTypeMask |= ENumericalPropertyChangeType.NestedUpdate;
                }

                m_DataZero.Set(GetMax());
                m_DataZero.Substract(m_Value);
                TNumerical oldDepletion = m_DataZero.Get();
                m_DataZero.ToZero();

                NumericalPropertyChangeEventStruct<TNumerical, TContext, TModifierReader> eventData =
                    new NumericalPropertyChangeEventStruct<TNumerical, TContext, TModifierReader>(
                        this,
                        i_ChangeTypeMask,    
                        oldDepletion,
                        i_Context
                    );

                UpdateModifiers(ref eventData);
                m_Depletion = eventData.NewDepletion;
                UpdateExhaustableModifiedValue();

                if ((i_ChangeTypeMask & ENumericalPropertyChangeType.NestedUpdate) == ENumericalPropertyChangeType.None)
                {
                    m_Updating = false;
                }
            }
            else
            {
                UpdateExhaustableModifiedValue();
            }
        }
    }
}
