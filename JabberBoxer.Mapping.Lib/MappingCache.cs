using System;
using System.Collections;

namespace JabberBoxer.Mapping.Lib
{
    /// <summary>
    /// Caching for the object maps.
    /// </summary>
    public static class MappingCache
    {
        /// <summary>
        /// Mapping Cache
        /// </summary>
        static readonly Hashtable _hastable = new Hashtable();
        // Caching map commands.
        static readonly CacheMapCommandValidator cacheMapCommandValidator = new CacheMapCommandValidator();
        static readonly RetieveMapCommandValidator retrieveMapCommandValidator = new RetieveMapCommandValidator();
        /// <summary>
        /// The current or last mapping action in use. Performance enhancement.
        /// </summary>
        static Action<object, object> _currentAction = null;
        /// <summary>
        /// The current or last mapping key in use. Performance enhancement.
        /// </summary>
        static int _currentKey = 0;
        /// <summary>
        /// Check if the cache contains the map key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns></returns>
        public static bool HasKey(int key) => _hastable.ContainsKey(key);
        /// <summary>
        /// Add a mapping to the cache.
        /// </summary>
        public static Action<CacheMapCommand> AddMap = (cmd)
            =>
        {
            if (cacheMapCommandValidator.IsValid(cmd) && !HasKey(cmd.Key))
                _hastable[cmd.Key] = cmd.Action;
        };
        /// <summary>
        /// Get a mapping from the cache.
        /// </summary>
        /// <param name="cmd">Tha get map command.</param>
        /// <returns></returns>
        public static Action<object, object> GetMap(RetrieveMapCommand cmd)
        {
            if (!retrieveMapCommandValidator.IsValid(cmd)) return null;

            if (_currentKey == cmd.Key && _currentAction != null)
                return _currentAction;

            _currentKey = cmd.Key;

            if (HasKey(_currentKey))
                _currentAction = (Action<object, object>)_hastable[_currentKey];

            return _currentAction;
        }

    }
}
