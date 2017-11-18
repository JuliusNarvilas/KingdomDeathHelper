//This define option forces HashedString class to only consider hashes when comparing for equality
//#define HASHED_STRING_FORCE_HASH_EQUALS

using System;
using UnityEngine;

namespace Common.Text
{
    /// <summary>
    /// Class to wrap a string with its cached hash value,
    /// as default string implementation calculates it on demand.
    /// This is useful for cases where string hashes are often requested.
    /// </summary>
    /// <seealso cref="System.IEquatable{Common.Text.HashedStringStruct}" />
    /// <seealso cref="System.IComparable{Common.Text.HashedStringStruct}" />
    /// <seealso cref="System.IComparable" />
    [Serializable]
    public struct HashedStringStruct : IEquatable<HashedStringStruct>, IComparable<HashedStringStruct>, IComparable
    {
        /// <summary>
        /// The string text.
        /// </summary>
        [SerializeField]
        private string m_Text;

        /// <summary>
        /// The cached string hash value.
        /// </summary>
        [NonSerialized]
        private int m_Hash;

        /// <summary>
        /// The string text.
        /// </summary>
        public string Text { get { return m_Text; } }

        /// <summary>
        /// The cached string hash.
        /// </summary>
        public int Hash { get { return m_Hash; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashedStringStruct"/> struct.
        /// </summary>
        /// <param name="i_Text">The string text.</param>
        public HashedStringStruct(string i_Text)
        {
            m_Text = i_Text;
            m_Hash = i_Text.GetHashCode();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="HashedStringStruct"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="i_Obj">The object.</param>
        /// <returns>
        /// The text string of <see cref="HashedStringStruct"/>.
        /// </returns>
        public static implicit operator string(HashedStringStruct i_Obj)
        {
            return i_Obj.m_Text;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="HashedStringStruct"/>.
        /// </summary>
        /// <param name="i_Text">The <see cref="HashedStringStruct"/> text.</param>
        /// <returns>
        /// The <see cref="HashedStringStruct"/> representation of the string.
        /// </returns>
        public static implicit operator HashedStringStruct(string i_Text)
        {
            return new HashedStringStruct(i_Text);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="i_ObjA">The object a.</param>
        /// <param name="i_ObjB">The object b.</param>
        /// <returns>
        /// The equality result.
        /// </returns>
        public static bool operator ==(HashedStringStruct i_ObjA, HashedStringStruct i_ObjB)
        {
#if UNITY_EDITOR
            if ((i_ObjA.m_Text == i_ObjB.m_Text) != (i_ObjA.m_Hash == i_ObjB.m_Hash))
            {
                Log.DebugLogError(
                    "Incorrect HashedStringStruct early match ({0} and {1} / {2} and {3})", i_ObjA.m_Text, i_ObjB.m_Text, i_ObjA.m_Hash, i_ObjB.m_Hash);
            }
#endif

#if HASHED_STRING_FORCE_HASH_EQUALS
            return i_ObjA.Hash == i_ObjB.Hash;
#else
            if (i_ObjA.m_Hash == i_ObjB.m_Hash)
            {
                return i_ObjA.m_Text == i_ObjB.m_Text;
            }
            return false;
#endif
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="i_ObjA">The object a.</param>
        /// <param name="i_ObjB">The object b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(HashedStringStruct i_ObjA, HashedStringStruct i_ObjB)
        {
            return !(i_ObjA == i_ObjB);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return m_Hash;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="i_Obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object i_Obj)
        {
            HashedStringStruct hashedText = (HashedStringStruct)i_Obj;
            return this == hashedText;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Common.Text.HashedStringStruct" />, is equal to this instance.
        /// </summary>
        /// <param name="i_Obj">The <see cref="Common.Text.HashedStringStruct" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="Common.Text.HashedStringStruct" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(HashedStringStruct i_Other)
        {
            return this == i_Other;
        }

        /// <summary>
        /// Compares the specified <see cref="System.Object" /> to this instance.
        /// </summary>
        /// <param name="i_Obj">The object.</param>
        /// <returns>
        /// -1 if this instance is less than the given object, 1 if greater and 0 otherwisse.
        /// </returns>
        public int CompareTo(object i_Obj)
        {
            HashedStringStruct hashedText = (HashedStringStruct)i_Obj;
            return m_Text.CompareTo(hashedText.m_Text);
        }

        /// <summary>
        /// Compares the specified <see cref="Common.Text.HashedStringStruct" /> to this instance.
        /// </summary>
        /// <param name="i_Other">The object.</param>
        /// <returns>
        /// -1 if this instance is less than the given object, 1 if greater and 0 otherwisse.
        /// </returns>
        public int CompareTo(HashedStringStruct i_Other)
        {
            return m_Text.CompareTo(i_Other.m_Text);
        }
    }






    /// <summary>
    /// Class to wrap a string with its cached hash value,
    /// as default string implementation calculates it on demand.
    /// This is useful for cases where string hashes are often requested.
    /// </summary>
    /// <seealso cref="System.IEquatable{Common.Text.HashedString}" />
    /// <seealso cref="System.IComparable{Common.Text.HashedString}" />
    /// <seealso cref="System.IComparable" />
    [Serializable]
    public class HashedString : IEquatable<HashedString>, IComparable<HashedString>, IComparable, ISerializationCallbackReceiver
    {
        /// <summary>
        /// The string text.
        /// </summary>
        [SerializeField]
        private string m_Text;

        /// <summary>
        /// The cached string hash value.
        /// </summary>
        [NonSerialized]
        private int m_Hash;

        /// <summary>
        /// The string text.
        /// </summary>
        public string Text { get { return m_Text; } }
        /// <summary>
        /// The cached string hash.
        /// </summary>
        public int Hash { get { return m_Hash; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashedString"/> struct.
        /// </summary>
        /// <param name="i_Text">The string text.</param>
        public HashedString(string i_Text)
        {
            m_Text = i_Text;
            m_Hash = i_Text.GetHashCode();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="HashedString"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="i_Obj">The object.</param>
        /// <returns>
        /// The text string of <see cref="HashedString"/>.
        /// </returns>
        public static implicit operator string(HashedString i_Obj)
        {
            return i_Obj.m_Text;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="HashedString"/>.
        /// </summary>
        /// <param name="i_Text">The <see cref="HashedString"/> text.</param>
        /// <returns>
        /// The <see cref="HashedString"/> representation of the string.
        /// </returns>
        public static implicit operator HashedString(string i_Text)
        {
            return new HashedString(i_Text);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="i_ObjA">The object a.</param>
        /// <param name="i_ObjB">The object b.</param>
        /// <returns>
        /// The equality result.
        /// </returns>
        public static bool operator ==(HashedString i_ObjA, HashedString i_ObjB)
        {
            bool lhsIsNull = System.Object.ReferenceEquals(i_ObjA, null);
            bool rhsIsNull = System.Object.ReferenceEquals(i_ObjB, null);
            if (lhsIsNull || rhsIsNull)
            {
                if(lhsIsNull && rhsIsNull)
                {
                    return true;
                }
                return false;
            }

#if UNITY_EDITOR
            if ((i_ObjA.m_Text == i_ObjB.m_Text) != (i_ObjA.m_Hash == i_ObjB.m_Hash))
            {
                Log.DebugLogError(
                    "Incorrect HashedString early match ({0} and {1} / {2} and {3})", i_ObjA.m_Text, i_ObjB.m_Text, i_ObjA.m_Hash, i_ObjB.m_Hash);
            }
#endif

#if HASHED_STRING_FORCE_HASH_EQUALS
            return i_ObjA.Hash == i_ObjB.Hash;
#else
            if (i_ObjA.m_Hash == i_ObjB.m_Hash)
            {
                return i_ObjA.m_Text == i_ObjB.m_Text;
            }
            return false;
#endif
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="i_ObjA">The object a.</param>
        /// <param name="i_ObjB">The object b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(HashedString i_ObjA, HashedString i_ObjB)
        {
            return !(i_ObjA == i_ObjB);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return m_Hash;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="i_Obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object i_Obj)
        {
            HashedString hashedText = (HashedString)i_Obj;
            return this == hashedText;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Common.Text.HashedString" />, is equal to this instance.
        /// </summary>
        /// <param name="i_Obj">The <see cref="Common.Text.HashedString" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="Common.Text.HashedString" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(HashedString i_Other)
        {
            return this == i_Other;
        }

        /// <summary>
        /// Compares the specified <see cref="System.Object" /> to this instance.
        /// </summary>
        /// <param name="i_Obj">The object.</param>
        /// <returns>
        /// -1 if this instance is less than the given object, 1 if greater and 0 otherwisse.
        /// </returns>
        public int CompareTo(object i_Obj)
        {
            HashedString hashedText = (HashedString)i_Obj;
            return m_Text.CompareTo(hashedText.m_Text);
        }

        /// <summary>
        /// Compares the specified <see cref="Common.Text.HashedString" /> to this instance.
        /// </summary>
        /// <param name="i_Other">The object.</param>
        /// <returns>
        /// -1 if this instance is less than the given object, 1 if greater and 0 otherwisse.
        /// </returns>
        public int CompareTo(HashedString i_Other)
        {
            return m_Text.CompareTo(i_Other.m_Text);
        }

        public void OnBeforeSerialize()
        { }

        public void OnAfterDeserialize()
        {
            if (m_Text != null)
            {
                m_Hash = m_Text.GetHashCode();
            }
        }
    }
}
