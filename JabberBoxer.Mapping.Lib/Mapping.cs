using System;

namespace JabberBoxer.Mapping.Lib
{
    /// <summary>
    /// The mapping object.
    /// </summary>
    public sealed class Mapping
    {
        public int Key { get; private set; }
        public Action<object, object> Map { get; private set; }
        public Mapping(Binding binding)
        {
            this.Key = binding.GetRelationship().Id;
            this.Map = binding.GetAction();
        }
        public override int GetHashCode()
        {
            return this.Key;
        }
        /// <summary>
        /// Create a mapping from a binding relationship.
        /// </summary>
        /// <param name="binding">The binding object.</param>
        /// <returns>Newly created mapping.</returns>
        public static Mapping Create(Binding binding)
        {
            return new Mapping(binding);
        }
    }
}
