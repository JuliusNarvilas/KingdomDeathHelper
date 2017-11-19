using Common.Properties.Numerical.Data;

namespace Common.Properties.Numerical
{
    /// <summary>
    /// A child of <see cref="ExhaustibleNumericalProperty{TNumerical, TContext, TModifierReader}"/>
    /// with added post change observable event functionality.
    /// </summary>
    /// <typeparam name="TNumerical">The type of the numerical.</typeparam>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TModifierReader">The type of the modifier reader.</typeparam>
    /// <seealso cref="Common.Properties.Numerical.ExhaustibleNumericalProperty{TNumerical, TContext, TModifierReader}" />
    public class ObservableExhaustibleNumericalProperty<TNumerical, TContext, TModifierReader> : ExhaustibleNumericalProperty<TNumerical, TContext, TModifierReader> where TModifierReader : INumericalPropertyModifierReader<TNumerical>
    {
        public event NumericalPropertyEventHandler<TNumerical, TContext, TModifierReader> ChangeSubscription;

        public ObservableExhaustibleNumericalProperty(INumericalPropertyData<TNumerical> i_Value) : base(i_Value)
        { }

        /// <summary>
        /// Instance update logic that can be overriden and customised.
        /// </summary>
        /// <param name="i_ChangeTypeMask">A mask of change type flags describing this update event.</param>
        /// <param name="i_Context">The change context.</param>
        protected override void UpdateInternal(ENumericalPropertyChangeType i_ChangeTypeMask, TContext i_Context)
        {
            if ((m_Modifiers.Count > 0) || (ChangeSubscription != null))
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
                FireChangeEvent(ref eventData);
            }
            else
            {
                UpdateExhaustableModifiedValue();
            }
        }

        protected void FireChangeEvent(ref NumericalPropertyChangeEventStruct<TNumerical, TContext, TModifierReader> i_EventData)
        {
            Log.DebugLog("Numerical property change event fired.");

            if (ChangeSubscription != null)
            {
                ChangeSubscription(ref i_EventData);
            }
        }
    }
}
