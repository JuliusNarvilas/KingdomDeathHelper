using Common.Helpers;
using System;

namespace Common
{
    [Serializable]
    /// <summary>
    /// A container to combine two values together.
    /// </summary>
    /// <typeparam name="T1">The type of the first value.</typeparam>
    /// <typeparam name="T2">The type of the second value.</typeparam>
    /// <seealso cref="System.IEquatable{Common.Pair{T1, T2}}" />
    public class Pair<T1, T2> : IEquatable<Pair<T1, T2>>
    {
        /// <summary>
        /// The first value.
        /// </summary>
        public T1 First;
        /// <summary>
        /// The second value.
        /// </summary>
        public T2 Second;

        /// <summary>
        /// Initializes a new instance of <see cref="Pair{T1, T2}"/> class.
        /// </summary>
        /// <param name="i_First">The first value.</param>
        /// <param name="i_Second">The second value.</param>
        public Pair(T1 i_First, T2 i_Second)
        {
            First = i_First;
            Second = i_Second;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="i_Other">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object i_Other)
        {
            Pair<T1, T2> otherPair = i_Other as Pair<T1, T2>;
            return Equals(otherPair);
        }

        /// <summary>
        /// Determines whether the specified value equels to this instance.
        /// </summary>
        /// <param name="i_Other">The other <see cref="Pair{T1, T2}"/> instance.</param>
        /// <returns><c>true</c> if the specified value is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Pair<T1, T2> i_Other)
        {
            if (i_Other == null)
            {
                return false;
            }
            return First.Equals(i_Other.First) && Second.Equals(i_Other.Second);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return MathHelper.CombineHashCodes(First.GetHashCode(), Second.GetHashCode());
        }
    }

    [Serializable]
    /// <summary>
    /// A container to combine two values together.
    /// </summary>
    /// <typeparam name="T1">The type of the first value.</typeparam>
    /// <typeparam name="T2">The type of the second value.</typeparam>
    /// <seealso cref="System.IEquatable{Common.PairStruct{T1, T2}}" />
    public struct PairStruct<T1, T2> : IEquatable<PairStruct<T1, T2>>
    {
        /// <summary>
        /// The first value.
        /// </summary>
        public T1 First;
        /// <summary>
        /// The second value.
        /// </summary>
        public T2 Second;

        /// <summary>
        /// Initializes a new instance of the <see cref="PairStruct{T1, T2}"/> struct.
        /// </summary>
        /// <param name="i_First">The i first.</param>
        /// <param name="i_Second">The i second.</param>
        public PairStruct(T1 i_First, T2 i_Second)
        {
            First = i_First;
            Second = i_Second;
        }

        public static bool operator ==(PairStruct<T1, T2> i_ObjA, PairStruct<T1, T2> i_ObjB)
        {
            return i_ObjA.First.Equals(i_ObjB.First) && i_ObjA.Second.Equals(i_ObjB.Second);
        }

        public static bool operator !=(PairStruct<T1, T2> i_ObjA, PairStruct<T1, T2> i_ObjB)
        {
            return !(i_ObjA == i_ObjB);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="i_Other">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object i_Other)
        {
            PairStruct<T1, T2> otherPair = (PairStruct<T1, T2>) i_Other;
            return this == otherPair;
        }

        /// <summary>
        /// Determines whether the specified value equels to this instance.
        /// </summary>
        /// <param name="i_Other">The other <see cref="Pair{T1, T2}"/> instance.</param>
        /// <returns><c>true</c> if the specified value is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(PairStruct<T1, T2> i_Other)
        {
            return this == i_Other;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return MathHelper.CombineHashCodes(First.GetHashCode(), Second.GetHashCode());
        }
    }
}
