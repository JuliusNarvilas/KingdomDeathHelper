using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Common.Properties.Enumeration
{
    /// <summary>
    /// A simple property that represents a unique value (per generator used).
    /// </summary>
    public class EnumProperty : Property<object>
    {
        /// <summary>
        /// A factory class that manages <see cref="EnumProperty{TContent}"/> instantiation and maintains a set of unique values for this class instance.
        /// </summary>
        public class Generator
        {
            /// <summary>
            /// Collection with <see cref="IList{T}"/> and <see cref="IDictionary{TKey, TValue}"/> functionality for storing generated <see cref="EnumProperty{TContent}"/> instances.
            /// </summary>
            private class EnumPropertyCollection : KeyedCollection<object, EnumProperty>
            {
                protected override object GetKeyForItem(EnumProperty item)
                {
                    return item.GetValue();
                }

                public EnumProperty[] ToArray()
                {
                    return Items.ToArray();
                }

                public bool TryGetValue(object i_Key, out EnumProperty o_Value)
                {
                    return Dictionary.TryGetValue(i_Key, out o_Value);
                }
            }
            
            private static readonly Dictionary<string, Generator> s_Factories = new Dictionary<string, Generator>();
            /// <summary>
            /// Get an array of all the factories for <see cref="EnumProperty{TContent}"/>.
            /// </summary>
            public static Generator[] AllFactories { get { return s_Factories.Values.ToArray(); } }

            public static Generator FindFactory(string i_Name)
            {
                Generator result = null;
                if (s_Factories.TryGetValue(i_Name, out result))
                {
                    return result;
                }
                return null;
            }

            public static Generator FindCreateFactory(string i_Name)
            {
                Generator result = FindFactory(i_Name);
                if(result == null)
                {
                    result = new Generator(i_Name);
                }
                return result;
            }

            /// <summary>
            /// Factory name.
            /// </summary>
            public readonly string Name;
            /// <summary>
            /// Container for generated <see cref="EnumProperty{TContent}"/> instances.
            /// </summary>
            private readonly EnumPropertyCollection m_PropertyCollection = new EnumPropertyCollection();


            private Generator(string i_Name)
            {
                Name = i_Name;
                s_Factories[i_Name] = this;
            }

            /// <summary>
            /// Creates a new instance of <see cref="EnumProperty{TContent}"/>, associated with this <see cref="Generator"/>,
            /// or returns an existing instance, representing the provided value.
            /// </summary>
            /// <param name="i_Name">Value of the <see cref="EnumProperty{TContent}"/>.</param>
            /// <returns>Instance of <see cref="EnumProperty{TContent}"/> that represents the provided value.</returns>
            public EnumProperty Create(object i_Name, object i_Content = null)
            {
                EnumProperty result;
                if(!m_PropertyCollection.TryGetValue(i_Name, out result))
                {
                    result = new EnumProperty(i_Name, this, i_Content);
                    m_PropertyCollection.Add(result);
                }
                return result;
            }

            public EnumProperty Find(object i_Name)
            {
                EnumProperty result;
                if (m_PropertyCollection.TryGetValue(i_Name, out result))
                {
                    return result;
                }
                return null;
            }

            public override string ToString()
            {
                return Name;
            }
        }



        public static EnumProperty Find(string i_FactoryName, object i_PropertyKey)
        {
            Generator gen = Generator.FindFactory(i_FactoryName);
            if(gen != null)
            {
                return gen.Find(i_PropertyKey);
            }
            return null;
        }


        /// <summary>
        /// The <see cref="Generator"/> instance that created this <see cref="EnumProperty{TContent}"/>.
        /// </summary>
        public readonly Generator Factory;
        /// <summary>
        /// Optional content to be associated with <see cref="EnumProperty{TContent}"/> instance.
        /// </summary>
        [NonSerialized]
        public readonly object Content;

        protected EnumProperty(object i_UniqueValue, Generator i_Factory, object i_Content) : base(i_UniqueValue)
        {
            Factory = i_Factory;
            Content = i_Content;
        }

        public EnumProperty() : base(null)
        { }

    }
}
