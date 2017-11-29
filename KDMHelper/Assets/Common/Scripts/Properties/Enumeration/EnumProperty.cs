using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Common.Properties.Enumeration
{
    /// <summary>
    /// A simple property that represents a unique string (per generator used).
    /// </summary>
    public class EnumProperty<TContent> : Property<string>
    {
        /// <summary>
        /// A factory class that manages <see cref="EnumProperty{TContent}"/> instantiation and maintains a set of unique values for this class instance.
        /// </summary>
        public class Generator
        {
            /// <summary>
            /// Collection with <see cref="IList{T}"/> and <see cref="IDictionary{TKey, TValue}"/> functionality for storing generated <see cref="EnumProperty{TContent}"/> instances.
            /// </summary>
            private class EnumPropertyCollection : KeyedCollection<string, EnumProperty<TContent>>
            {
                protected override string GetKeyForItem(EnumProperty<TContent> item)
                {
                    return item.GetValue();
                }

                public EnumProperty<TContent>[] ToArray()
                {
                    return Items.ToArray();
                }

                public bool TryGetValue(string i_Key, out EnumProperty<TContent> o_Value)
                {
                    return Dictionary.TryGetValue(i_Key, out o_Value);
                }
            }
            
            private static readonly List<Generator> s_Factories = new List<Generator>();
            /// <summary>
            /// Get an array of all the factories for <see cref="EnumProperty{TContent}"/>.
            /// </summary>
            public static Generator[] AllFactories { get { return s_Factories.ToArray(); } }

            public static Generator FindFactory(string i_Name)
            {
                int count = s_Factories.Count;
                for (int i = 0; i < count; ++i)
                {
                    if(s_Factories[i].Name == i_Name)
                    {
                        return s_Factories[i];
                    }
                }
                return null;
            }

            /// <summary>
            /// Factory name.
            /// </summary>
            public readonly string Name;
            /// <summary>
            /// Conatoner for generated <see cref="EnumProperty{TContent}"/> instances.
            /// </summary>
            private readonly EnumPropertyCollection m_PropertyCollection = new EnumPropertyCollection();

            public Generator(string i_Name)
            {
                Name = i_Name;
                s_Factories.Add(this);
            }

            /// <summary>
            /// Creates a new instance of <see cref="EnumProperty{TContent}"/>, associated with this <see cref="Generator"/>,
            /// or returns an existing instance, representing the provided value.
            /// </summary>
            /// <param name="i_Name">Value of the <see cref="EnumProperty{TContent}"/>.</param>
            /// <returns>Instance of <see cref="EnumProperty{TContent}"/> that represents the provided value.</returns>
            public EnumProperty<TContent> Create(string i_Name, TContent i_Content = default(TContent))
            {
                EnumProperty<TContent> result;
                if(!m_PropertyCollection.TryGetValue(i_Name, out result))
                {
                    result = new EnumProperty<TContent>(i_Name, this, i_Content);
                    m_PropertyCollection.Add(result);
                }
                return result;
            }

            public EnumProperty<TContent> Find(string i_Name)
            {
                EnumProperty<TContent> result;
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
        
        public static readonly Generator DEFAULT_FACTORY = new Generator("Default");



        /// <summary>
        /// The <see cref="Generator"/> instance that created this <see cref="EnumProperty{TContent}"/>.
        /// </summary>
        public readonly Generator Factory;
        /// <summary>
        /// Optional content to be associated with <see cref="EnumProperty{TContent}"/> instance.
        /// </summary>
        public readonly TContent Content;

        protected EnumProperty(string i_UniqueName, Generator i_Factory, TContent i_Content) : base(i_UniqueName)
        {
            Factory = i_Factory;
            Content = i_Content;
        }
        

        public override string ToString()
        {
            return m_Value;
        }
    }
}
