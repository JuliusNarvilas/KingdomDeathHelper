using Common.Properties.Numerical.Data;
using System;
using System.Collections.Generic;
using Common.Collections;

namespace Common.Properties.Numerical
{
    
    /// <summary>
    /// A base implementation class for numerical properties with modification functionality.
    /// </summary>
    /// <typeparam name="TNumerical">Numerical property type.</typeparam>
    /// <typeparam name="TContext">Change context type.</typeparam>
    /// <typeparam name="TModifierReader">Reader interface for a modifier.</typeparam>
    public class NumericalProperty<TNumerical, TContext, TModifierReader> :
        Property<TNumerical> where TModifierReader : INumericalPropertyModifierReader<TNumerical>
    {
        /// <summary>
        /// A property data interface for abstracting numerical operations for any raw data type and housing the data type's representation of zero.
        /// </summary>
        protected readonly INumericalPropertyData<TNumerical> m_DataZero;
        /// <summary>
        /// Unmodified base value of the property.
        /// </summary>
        protected TNumerical m_BaseValue;
        /// <summary>
        /// A single representation of all modifier effects.
        /// </summary>
        protected TNumerical m_FinalModifier;
        /// <summary>
        /// An indicator for nested updating.
        /// </summary>
        protected bool m_Updating;
        /// <summary>
        /// A list of modifiers that gets sorted when a new modifier is added.
        /// </summary>
        protected readonly List<INumericalPropertyModifier<TNumerical, TContext, TModifierReader>> m_Modifiers;

        /// <summary>
        /// A comparer for sorting numerical property modifiers.
        /// </summary>
        static protected readonly NumericalPropertyModifierSortComparer<TNumerical, TContext, TModifierReader> s_SortComparer =
            new NumericalPropertyModifierSortComparer<TNumerical, TContext, TModifierReader>();

        /// <summary>
        /// Initializes a new instance of numerical property.
        /// </summary>
        /// <param name="i_Value">Initial value wrapped with abstracted functionality for simple mathematical operations.</param>
        public NumericalProperty(INumericalPropertyData<TNumerical> i_Value) : base(i_Value.Get())
        {
            m_DataZero = i_Value.CreateZero();
            m_BaseValue = m_Value;
            m_FinalModifier = m_DataZero.Get();
            m_Updating = false;
            m_Modifiers = new List<INumericalPropertyModifier<TNumerical, TContext, TModifierReader>>();
        }

        /// <summary>
        /// Creates a representation of zero value wrapped with abstracted functionality for simple mathematical operations.
        /// </summary>
        /// <returns>Property data holding the value of zero.</returns>
        public INumericalPropertyData<TNumerical> CreateZeroData()
        {
            return m_DataZero.CreateZero();
        }

        /// <summary>
        /// Gets clamped property value to be greater or equal to 0.
        /// </summary>
        /// <returns>Property value clamped to be greater or equal to 0.</returns>
        public TNumerical GetPositive()
        {
            if(m_DataZero.CompareTo(m_Value) >= 0)
            {
                return m_DataZero.Get();
            }
            return m_Value;
        }

        /// <summary>
        /// Gets the property base value that modifiers are added to.
        /// </summary>
        /// <returns>Property base value.</returns>
        public TNumerical GetBaseValue()
        {
            return m_BaseValue;
        }


        /// <summary>
        /// Sets the property base value that modifiers are added to.
        /// </summary>
        /// <param name="i_Value">New property base value.</param>
        public void SetBaseValue(TNumerical i_Value)
        {
            SetBaseValue(i_Value, GetDefaultContext());
        }

        /// <summary>
        /// Sets the property base value that modifiers are added to.
        /// </summary>
        /// <param name="i_Value">New property base value.</param>
        /// <param name="i_Context">Contextual data about this change.</param>
        public void SetBaseValue(TNumerical i_Value, TContext i_Context)
        {
            m_BaseValue = i_Value;
            ENumericalPropertyChangeType changeTypeMask = ENumericalPropertyChangeType.BaseSet;
            UpdateInternal(changeTypeMask, i_Context);
        }

        public virtual TContext GetDefaultContext()
        {
            return default(TContext);
        }

        /// <summary>
        /// Gets the property value modifier that represents the effects of all assigned modifiers.
        /// </summary>
        /// <returns>The final modifier.</returns>
        public TNumerical GetFinalModifier()
        {
            return m_FinalModifier;
        }
        /// <summary>
        /// Gets the number of modifiers assigned to this property.
        /// </summary>
        /// <returns>Modifier count.</returns>
        public int GetModifierCount()
        {
            return m_Modifiers.Count;
        }
        /// <summary>
        /// Gets an assigned modifier based on the list element index.
        /// </summary>
        /// <param name="i_Index">Index to the modifier list element.</param>
        /// <returns>Modifier reader interface.</returns>
        public TModifierReader GetModifier(int i_Index)
        {
            Log.DebugAssert(i_Index < m_Modifiers.Count, "Index out of bounds.");
            return m_Modifiers[i_Index].GetReader(GetDefaultContext());
        }

        /// <summary>
        /// Adds a modifier to the assigned modifier list.
        /// </summary>
        /// <param name="i_Modifier">The new modifier to add.</param>
        public void AddModifier(INumericalPropertyModifier<TNumerical, TContext, TModifierReader> i_Modifier)
        {
            AddModifier(i_Modifier, GetDefaultContext());
        }

        /// <summary>
        /// Adds a modifier to the assigned modifier list.
        /// </summary>
        /// <param name="i_Modifier">The new modifier to add.</param>
        /// <param name="i_Context">Contextual data about this change.</param>
        public void AddModifier(INumericalPropertyModifier<TNumerical, TContext, TModifierReader> i_Modifier, TContext i_Context)
        {
            Log.DebugAssert(i_Modifier != null, "Invalid null paremeter.");
            m_Modifiers.Add(i_Modifier);
            m_Modifiers.InsertionSort(s_SortComparer);
            ENumericalPropertyChangeType changeTypeMask = ENumericalPropertyChangeType.ModifierAdd;
            UpdateInternal(changeTypeMask, i_Context);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="i_Modifier">The i_ modifier.</param>
        /// <returns></returns>
        public bool RemoveModifier(INumericalPropertyModifier<TNumerical, TContext, TModifierReader> i_Modifier)
        {
            return RemoveModifier(i_Modifier, GetDefaultContext());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i_Modifier">The i_ modifier.</param>
        /// <param name="i_Context">Contextual data about this change.</param>
        /// <returns></returns>
        public bool RemoveModifier(INumericalPropertyModifier<TNumerical, TContext, TModifierReader> i_Modifier, TContext i_Context)
        {
            bool result = m_Modifiers.Remove(i_Modifier);
            if (result)
            {
                ENumericalPropertyChangeType changeTypeMask = ENumericalPropertyChangeType.ModifierRemove;
                UpdateInternal(changeTypeMask, i_Context);
            }
            return result;
        }


        /// <summary>
        /// Force an update event on the property.
        /// </summary>
        public void Update()
        {
            Update(GetDefaultContext());
        }

        /// <summary>
        /// Force an update event on the property.
        /// </summary>
        /// <param name="i_Context">The event context.</param>
        public void Update(TContext i_Context = default(TContext))
        {
            ENumericalPropertyChangeType changeTypeMask = ENumericalPropertyChangeType.ForceUpdate;
            UpdateInternal(changeTypeMask, i_Context);
        }


        /// <summary>
        /// Creates a change bundle for this property instance.
        /// </summary>
        /// <returns>New <see cref="ChangeBundle"/> instance for this property.</returns>
        public ChangeBundle CreateChangeBundle()
        {
            return CreateChangeBundle(GetDefaultContext());
        }

        /// <summary>
        /// Creates a change bundle for this property instance.
        /// </summary>
        /// <param name="i_Context">The context changes.</param>
        /// <returns>New <see cref="ChangeBundle"/> instance for this property.</returns>
        public ChangeBundle CreateChangeBundle(TContext i_Context)
        {
            return new ChangeBundle(this, i_Context);
        }

        /// <summary>
        /// Instance update logic that can be overriden and customised.
        /// </summary>
        /// <param name="i_ChangeTypeMask">A mask of change type flags describing this update event.</param>
        /// <param name="i_Context">The change context.</param>
        protected virtual void UpdateInternal(ENumericalPropertyChangeType i_ChangeTypeMask, TContext i_Context)
        {
            if (m_Modifiers.Count > 0)
            {
                //tracking of nested update types
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

                //leaving the scope of nested updating
                if ((i_ChangeTypeMask & ENumericalPropertyChangeType.NestedUpdate) == ENumericalPropertyChangeType.None)
                {
                    m_Updating = false;
                }
            }
            else
            {
                UpdateModifiedValue();
            }
        }

        /// <summary>
        /// Updates the modifiers.
        /// </summary>
        /// <param name="i_EventData">The event data.</param>
        protected void UpdateModifiers(ref NumericalPropertyChangeEventStruct<TNumerical, TContext, TModifierReader> i_EventData)
        {
            int size = m_Modifiers.Count;
            for(int i = 0; i < size; ++i)
            {
                m_Modifiers[i].Update(ref i_EventData);
            }
            m_FinalModifier = i_EventData.NewModifier;

            //Log.DebugLog("Numerical property final modifier updated from {0} to {1}.", i_EventData.OldModifier, i_EventData.NewModifier);
        }

        /// <summary>
        /// Updates the final property value using the final modifier.
        /// </summary>
        protected void UpdateModifiedValue()
        {
            m_DataZero.Add(m_BaseValue);
            m_DataZero.Add(m_FinalModifier);
            m_Value = m_DataZero.Get();
            m_DataZero.ToZero();

            //Log.DebugLog("Numerical property value updated to {0}: {1} + {2}.", m_Value, m_BaseValue, m_FinalModifier);
        }

        /// <summary>
        /// Wrapper class for grouping several changes in a single update event.
        /// </summary>
        /// <seealso cref="Common.Properties.Property{TNumerical}" />
        public class ChangeBundle : IDisposable
        {
            protected NumericalProperty<TNumerical, TContext, TModifierReader> m_Property;
            protected TNumerical m_BaseValue;
            protected ENumericalPropertyChangeType m_ChangeTypeMask;
            protected List<INumericalPropertyModifier<TNumerical, TContext, TModifierReader>> m_AddModifiers;
            protected List<INumericalPropertyModifier<TNumerical, TContext, TModifierReader>> m_RemoveModifiers;
            protected TContext m_Context;
            protected bool m_Applied;

            /// <summary>
            /// Initializes a new instance of the <see cref="ChangeBundle"/> class.
            /// </summary>
            /// <param name="i_Property">The numerical property to change.</param>
            /// <param name="i_Context">The change context.</param>
            public ChangeBundle(NumericalProperty<TNumerical, TContext, TModifierReader> i_Property, TContext i_Context = default(TContext))
            {
                Log.DebugAssert(i_Property != null, "Invalid null property.");
                m_Property = i_Property;
                m_BaseValue = i_Property.m_BaseValue;
                m_ChangeTypeMask = ENumericalPropertyChangeType.Bundle;
                m_AddModifiers = new List<INumericalPropertyModifier<TNumerical, TContext, TModifierReader>>();
                m_RemoveModifiers = new List<INumericalPropertyModifier<TNumerical, TContext, TModifierReader>>();
                m_Context = i_Context;
                m_Applied = false;
            }

            /// <summary>
            /// Adds a modifier.
            /// </summary>
            /// <param name="i_Modifier">A modifier.</param>
            public void AddModifier(INumericalPropertyModifier<TNumerical, TContext, TModifierReader> i_Modifier)
            {
                m_ChangeTypeMask |= ENumericalPropertyChangeType.ModifierAdd;
                m_AddModifiers.Add(i_Modifier);
            }

            /// <summary>
            /// Removes a modifier.
            /// </summary>
            /// <param name="i_Modifier">A modifier.</param>
            public void RemoveModifier(INumericalPropertyModifier<TNumerical, TContext, TModifierReader> i_Modifier)
            {
                m_ChangeTypeMask |= ENumericalPropertyChangeType.ModifierRemove;
                m_RemoveModifiers.Add(i_Modifier);
            }

            /// <summary>
            /// Sets the property base value.
            /// </summary>
            /// <param name="i_Value">The new base value.</param>
            public void SetBaseValue(TNumerical i_Value)
            {
                m_ChangeTypeMask |= ENumericalPropertyChangeType.BaseSet;
                m_BaseValue = i_Value;
            }

            ~ChangeBundle()
            {
                Apply();
            }

            /// <summary>
            /// Resolve changes to modifiers.
            /// </summary>
            protected virtual void ApplyModifiers()
            {
                int size = m_AddModifiers.Count;
                for (int i = 0; i < size; ++i)
                {
                    m_Property.m_Modifiers.Add(m_AddModifiers[i]);
                }

                size = m_RemoveModifiers.Count;
                for(int i = 0; i < size; ++i)
                {
                    m_Property.m_Modifiers.Remove(m_RemoveModifiers[i]);
                }

                if ((m_ChangeTypeMask & ENumericalPropertyChangeType.ModifierAdd) != ENumericalPropertyChangeType.None)
                {
                    m_Property.m_Modifiers.InsertionSort(s_SortComparer);
                }
            }

            /// <summary>
            /// Apply the change bundle.
            /// </summary>
            protected void Apply()
            {
                if (!m_Applied)
                {
                    m_Applied = true;
                    if ((m_ChangeTypeMask & ENumericalPropertyChangeType.BaseSet) != ENumericalPropertyChangeType.None)
                    {
                        m_Property.m_BaseValue = m_BaseValue;
                    }
                    ApplyModifiers();
                    m_Property.UpdateInternal(m_ChangeTypeMask, m_Context);
                }
            }

            /// <summary>
            /// Resolve property changes accumulated in this object instance.
            /// </summary>
            public void Dispose()
            {
                Apply();
            }
        }
    }
}
