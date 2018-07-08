using System;
using Common.Properties.Numerical.Data;

namespace Common.Properties.Numerical
{
    /// <summary>
    /// A child of <see cref="Common.Properties.Numerical.NumericalProperty{TNumerical, TContext, TModifierReader}"/>
    /// with added post change observable event functionality.
    /// </summary>
    /// <remarks>
    /// A similar functionality can be achieved by adding a custom modifier that would act as event listener for the original
    /// <see cref="Common.Properties.Numerical.NumericalProperty{TNumerical, TContext, TModifierReader}"/> class instance.
    /// However, this added observable event subscription prevents the user from implementing a full modifier interface just for listening on change events,
    /// activates after the update has been resolved and allows modifiers' list to portray a more accurate number of modifiers (if this data is important).
    /// </remarks>
    /// <typeparam name="TNumerical">The type of the numerical data.</typeparam>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TModifierReader">The type of the modifier reader.</typeparam>
    public class ObservableNumericalProperty<TNumerical, TContext, TModifierReader> : NumericalProperty<TNumerical, TContext, TModifierReader>, IObservablePropertySimpleSubscription where TModifierReader : INumericalPropertyModifierReader<TNumerical>
    {
        public event NumericalPropertyEventHandler<TNumerical, TContext, TModifierReader> ChangeSubscription;
        public event Action<object> SimpleChangeSubscription;

        public ObservableNumericalProperty(INumericalPropertyData<TNumerical> i_Value) : base(i_Value)
        { }

        /// <summary>
        /// Overriden instance update logic for firing the post-update events.
        /// </summary>
        /// <param name="i_ChangeTypeMask">A mask of change type flags describing this update event.</param>
        /// <param name="i_Context">The change context.</param>
        protected override void UpdateInternal(ENumericalPropertyChangeType i_ChangeTypeMask, TContext i_Context)
        {
            if ((m_Modifiers.Count > 0) || (ChangeSubscription != null) || (SimpleChangeSubscription != null))
            {
                if (!m_Updating)
                {
                    m_Updating = true;
                }
                else
                {
                    i_ChangeTypeMask |= ENumericalPropertyChangeType.NestedUpdate;
                }

                NumericalPropertyChangeEventStruct<TNumerical, TContext, TModifierReader> eventData =
                    new NumericalPropertyChangeEventStruct<TNumerical, TContext, TModifierReader>(
                        this,
                        i_ChangeTypeMask,
                        i_Context
                        );

                UpdateModifiers(ref eventData);
                UpdateModifiedValue();

                if ((i_ChangeTypeMask & ENumericalPropertyChangeType.NestedUpdate) == ENumericalPropertyChangeType.None)
                {
                    m_Updating = false;
                }
                if (ChangeSubscription != null)
                {
                    ChangeSubscription(ref eventData);
                }
                if(SimpleChangeSubscription != null)
                {
                    SimpleChangeSubscription(this);
                }
            }
            else
            {
                UpdateModifiedValue();
            }
        }
    }
}
