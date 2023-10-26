using System;
using System.Collections.Generic;

namespace JabberBoxer.Mapping.Lib
{
    /// <summary>
    /// HOF compilation
    /// </summary>
    public static class F
    {
        #region mapper

        public static void Bind<TSource, TTarget>() where TTarget : class, new() => Mapper.Bind<TSource, TTarget>();
        public static TTarget Map<TTarget>(object source) where TTarget : class, new() => Mapper.Map<TTarget>(source);
    
        public static IEnumerable<TTarget> Map<TTarget>(object[] source) where TTarget : class, new() => Mapper.Map<TTarget>(source);

        #endregion

        #region relationship

        public static IRelationship Relate(Type source, Type target) => Relationship.Create(source, target);
        public static IRelationship Relate<T1, T2>() => Relationship.Create<T1, T2>();

        #endregion

        #region Binding

        public static Binding NewBinding<T1, T2>() => Lib.Binding.Create(Relationship.Create<T1, T2>());

        public static Binding Binding(IRelationship relationship) => Lib.Binding.Create(relationship);

        #endregion

        #region mapping

        public static Func<Binding, Mapping> NewMapping = (binding) => Mapping.Create(binding);

        #endregion

        #region caching

        public static Action<CacheMapCommand> CacheMap = (cmd) => MappingCache.AddMap(cmd);
        public static Func<RetrieveMapCommand, Action<object, object>> GetMap = (cmd) => MappingCache.GetMap(cmd);

        #endregion

        #region cache commands

        public static Func<int, Action<object, object>, CacheMapCommand> CacheMapCommand = (key, action) => new CacheMapCommand(key, action);
        public static Func<int, RetrieveMapCommand> GetMapCommand = (key) => new RetrieveMapCommand(key);

        #endregion

    }


}
