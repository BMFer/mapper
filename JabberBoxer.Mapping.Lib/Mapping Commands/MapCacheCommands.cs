using System;

namespace JabberBoxer.Mapping.Lib
{
    #region cache commands

    public abstract class MapCacheCommand { }

    /// <summary>
    /// Data needed to store a mapping action
    /// </summary>
    public sealed class CacheMapCommand : MapCacheCommand
    {
        public CacheMapCommand(int key, Action<object, object> action)
        {
            this.Key = key;
            this.Action = action;
        }
        public int Key { get; set; }
        public Action<object, object> Action { get; set; }
    }

    /// <summary>
    /// Data needed to retrieve mapping action.
    /// </summary>
    public sealed class RetrieveMapCommand : MapCacheCommand
    {
        public RetrieveMapCommand(int key)
        {
            this.Key = key;
        }
        public int Key { get; private set; }
    }

    #endregion
}
