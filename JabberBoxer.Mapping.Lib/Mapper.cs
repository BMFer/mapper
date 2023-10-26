using System;
using System.Collections.Generic;
using System.Linq;

namespace JabberBoxer.Mapping.Lib
{
    using static F;

    /// <summary>
    /// Mapping envoke logic
    /// </summary>
    public static class Mapper
    {
        /// <summary>
        /// Bind the types and send the mapping command to cache
        /// </summary>
        /// <typeparam name="TSource">Source Type</typeparam>
        /// <typeparam name="TTarget">Target Type</typeparam>
        public static void Bind<TSource, TTarget>() where TTarget : class, new()
        {
            IRelationship relationship = Relate<TSource, TTarget>();

            var binding = Binding(relationship);

            CacheMap(CacheMapCommand(binding.Id, binding.GetAction()));
        }
        /// <summary>
        /// Create a map from the relationship of the givin types.
        /// </summary>
        /// <typeparam name="TSource">Source Type</typeparam>
        /// <typeparam name="TTarget">Source Type</typeparam>
        /// <param name="source">The source object.</param>
        /// <returns>Newly mapped target object.</returns>
        public static TTarget Map<TSource, TTarget>(TSource source) where TTarget : class, new()
        {
            if (source.Equals(null))
                throw new NullReferenceException("Unable to map source to target. Mapping source is null");

            IRelationship relationship = Relate<TSource, TTarget>();

            var map = GetMap(GetMapCommand(relationship.Id));

            TTarget target = FastActivator<TTarget>.Create();

            map.Invoke(source, target);

            return target;
        }
        /// <summary>
        /// Create a map from the relationship of the givin types. Use inference.
        /// </summary>
        /// <typeparam name="TTarget">Target Type</typeparam>
        /// <param name="source">The source object</param>
        /// <returns>The newly mapped target object.</returns>
        public static TTarget Map<TTarget>(object source) where TTarget : class, new()
        {
            if (source.Equals(null))
                throw new NullReferenceException("Unable to map source to target. Mapping source is null");

            IRelationship relationship = Relate(source.GetType(), typeof(TTarget));

            var map = GetMap(GetMapCommand(relationship.Id));

            TTarget target = FastActivator<TTarget>.Create();

            map.Invoke(source, target);

            return target;
        }
        /// <summary>
        /// Parrallel use of the map.
        /// </summary>
        /// <typeparam name="TTarget">Target Type</typeparam>
        /// <param name="sources">The source objects</param>
        /// <returns>Newly mapped target objects.</returns>
        public static IEnumerable<TTarget> Map<TTarget>(object[] sources) where TTarget : class, new()
        {
            return sources.AsParallel().Select(x => Map<TTarget>(x));
        }

    }

}
