using System;

namespace JabberBoxer.Mapping.Lib
{
    public interface IRelationship
    {
        int Id { get; }
        Type SourceType { get; }
        Type TargetType { get; }
    }

    /// <summary>
    /// Source/Target Type Relationship
    /// </summary>
    /// <typeparam name="TSource">The Source Type</typeparam>
    /// <typeparam name="TTarget">The Target Type</typeparam>
    public sealed class Relationship : IEquatable<IRelationship>, IRelationship
    {
        public int Id => this.GetHashCode();
        // memory for the sorce and target type information.
        public Type SourceType { get; private set; }
        public Type TargetType { get; private set; }

        public Relationship(Type source, Type target)
        {
            this.SourceType = source;
            this.TargetType = target;
        }
        /// <summary>
        /// Create a relationship
        /// </summary>
        /// <typeparam name="TSource">Source Type</typeparam>
        /// <typeparam name="TTarget">Target Type</typeparam>
        /// <returns>New created relationship</returns>
        public static Relationship Create<TSource, TTarget>() => new Relationship(typeof(TSource), typeof(TTarget));
        /// <summary>
        /// Create a relationship
        /// </summary>
        /// <typeparam name="TSource">Source Type</typeparam>
        /// <typeparam name="TTarget">Target Type</typeparam>
        /// <returns>New created relationship</returns>
        public static Relationship Create(Type source, Type target) => new Relationship(source, target);

        #region IEquatable<IRelationship>

        public bool Equals(IRelationship other)
        {
            if (this.SourceType == other.SourceType)
                return this.TargetType == other.TargetType;
            return false;
        }
        /// <summary>
        /// Prime a hashcode for object recognition.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.SourceType.GetHashCode() * 397 ^ this.TargetType.GetHashCode();
        }

        #endregion
    }
}
