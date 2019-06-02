
namespace Common.Properties.Numerical
{
    /// <summary>
    /// Change type flags for identifying what type of changes are taking place.
    /// </summary>
    public enum ENumericalPropertyChangeType
    {
        /// <summary>
        /// Empty mask.
        /// </summary>
        None = 0,
        /// <summary>
        /// Flag for a new modifier being added.
        /// </summary>
        ModifierAdd = 1 << 0,
        /// <summary>
        /// Flag for a modifier being removed.
        /// </summary>
        ModifierRemove = 1 << 1,
        /// <summary>
        /// Flag for setting the base value.
        /// </summary>
        BaseSet = 1 << 2,
        /// <summary>
        /// Flag for depleating or restoring the max value.
        /// </summary>
        Deplete = 1 << 3,
        /// <summary>
        /// Flag for indicating that changes were made through a bundled change submission. 
        /// </summary>
        Bundle = 1 << 4,
        /// <summary>
        /// Flag for indicating that an explicit update request was made.
        /// </summary>
        ForceUpdate = 1 << 5,
        /// <summary>
        /// Flag for indicating that property changes for this update process were made inside another update process.
        /// </summary>
        /// <remarks>
        /// It's recommended to always check and avoid nested updates unless required.
        /// Unintended nested updates can result to an infinite recursion and a stack overflow.
        /// </remarks>
        NestedUpdate = 1 << 6,

        /// <summary>
        /// Mask containing flags for any type of modifier change.
        /// </summary>
        ModifierChange = ModifierAdd | ModifierRemove
    }

    /// <summary>
    /// A delegate type for handling numerical property change events.
    /// </summary>
    /// <typeparam name="TNumerical">Numerical property type.</typeparam>
    /// <typeparam name="TContext">Change context type.</typeparam>
    /// <typeparam name="TModifierReader">Reader interface for a modifier.</typeparam>
    /// <param name="i_EventData">Data representing the change event.</param>
    public delegate void NumericalPropertyEventHandler<TNumerical, TContext, TModifierReader>
        (ref NumericalPropertyChangeEventStruct<TNumerical, TContext, TModifierReader> i_EventData)
            where TModifierReader : INumericalPropertyModifierReader<TNumerical>;


    /// <summary>
    /// Structure representing the change event data.
    /// </summary>
    /// <typeparam name="TNumerical">Numerical property type.</typeparam>
    /// <typeparam name="TContext">Change context type.</typeparam>
    /// <typeparam name="TModifierReader">Reader interface for a modifier.</typeparam>
    public struct NumericalPropertyChangeEventStruct<TNumerical, TContext, TModifierReader>
        where TModifierReader : INumericalPropertyModifierReader<TNumerical>
    {
        /// <summary>
        /// The changed numerical property.
        /// </summary>
        public readonly NumericalProperty<TNumerical, TContext, TModifierReader> NumericalProperty;
        /// <summary>
        /// Mask representing changes associated with this change event.
        /// </summary>
        public readonly ENumericalPropertyChangeType EChangeTypeMask;
        /// <summary>
        /// The old final property modifier.
        /// </summary>
        public readonly TNumerical OldModifier;
        /// <summary>
        /// The old property depletion amount.
        /// </summary>
        public readonly TNumerical OldDepletion;
        /// <summary>
        /// Context of the change event.
        /// </summary>
        public readonly TContext Context;
        /// <summary>
        /// The new final modifier to be applied.
        /// </summary>
        public TNumerical NewModifier;
        /// <summary>
        /// The new depletion amount to be appllied.
        /// </summary>
        public TNumerical NewDepletion;

        /// <summary>
        /// Initializes a new instance of the numerical property change event data struct.
        /// </summary>
        /// <remarks>
        /// This struct can be created on the stack and treated as a primitive type when passing to a function, so memory handling needs to be considered.
        /// </remarks>
        /// <param name="i_Property">The changed numerical property.</param>
        /// <param name="i_ChangeTypeMask">Mask of change types that have occurred.</param>
        /// <param name="i_Context">Context of the change event.</param>
        public NumericalPropertyChangeEventStruct(
            NumericalProperty<TNumerical, TContext, TModifierReader> i_Property,
            ENumericalPropertyChangeType i_ChangeTypeMask,
            TContext i_Context = default(TContext)
        )
        {
            NumericalProperty = i_Property;
            EChangeTypeMask = i_ChangeTypeMask;
            OldModifier = i_Property.GetFinalModifier();
            OldDepletion = default(TNumerical);
            Context = i_Context;
            NewModifier = default(TNumerical);
            NewDepletion = default(TNumerical);
        }

        /// <summary>
        /// Initializes a new instance of the numerical property change event data struct.
        /// </summary>
        /// <remarks>
        /// This struct can be created on the stack and treated as a primitive type when passing to a function, so memory handling needs to be considered.
        /// </remarks>
        /// <param name="i_Property">The changed numerical property.</param>
        /// <param name="i_ChangeTypeMask">Mask of change types that have occurred.</param>
        /// <param name="i_OldDepletion">The numerical value of previous depletion amount.</param>
        /// <param name="i_Context">Context of the change event.</param>
        public NumericalPropertyChangeEventStruct(
            ExhaustibleNumericalProperty<TNumerical, TContext, TModifierReader> i_Property,
            ENumericalPropertyChangeType i_ChangeTypeMask,
            TNumerical i_OldDepletion,
            TContext i_Context = default(TContext)
        )
        {
            NumericalProperty = i_Property;
            EChangeTypeMask = i_ChangeTypeMask;
            OldModifier = i_Property.GetFinalModifier();
            OldDepletion = i_OldDepletion;
            Context = i_Context;
            NewModifier = default(TNumerical);
            NewDepletion = i_Property.GetDepletion();
        }
    }

}
