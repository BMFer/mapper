namespace JabberBoxer.Mapping.Lib
{
    public interface IValidator<T>
    {
        bool IsValid(T t);
    }

    #region relationship

    /// <summary>
    /// Assure that the relationship has the source type and the target type.
    /// </summary>
    public sealed class RelationshipValidator : IValidator<IRelationship>
    {
        public bool IsValid(IRelationship t) =>  t.SourceType != null && t.TargetType != null;
    }

    #endregion

    #region binding

    /// <summary>
    /// Assure that the binding has an id and mapping action.
    /// </summary>
    public sealed class BindingValidator : IValidator<Binding>
    {
        public bool IsValid(Binding t) => t.Id != 0 && t.GetAction() != null;
    }

    #endregion

    #region mapping

    /// <summary>
    /// Assure that the binding has an id and mapping action.
    /// </summary>
    public sealed class MappingValidator : IValidator<Mapping>
    {
        public bool IsValid(Mapping t) => t.Key != 0 && t.Map != null;
    }

    #endregion

    #region cache commands

    /// <summary>
    /// Assure that the command has a key and mapping action.
    /// </summary>
    public sealed class CacheMapCommandValidator : IValidator<CacheMapCommand>
    {
        public bool IsValid(CacheMapCommand cmd) => cmd.Key != 0 && cmd.Action != null;
    }

    /// <summary>
    /// Assure that the command has a key.
    /// </summary>
    public sealed class RetieveMapCommandValidator : IValidator<RetrieveMapCommand>
    {
        public bool IsValid(RetrieveMapCommand t) => t.Key != 0;
    }

    #endregion

}
