using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace JabberBoxer.Mapping.Lib
{
    /// <summary>
    /// Bind a source and target to return a compiled mapping action
    /// </summary>
    public sealed class Binding
    {
        private IRelationship _relationship;
        private Expression<Action<object, object>> _bindingExpression;
        private readonly ParameterExpression sourceVariable, targetVariable, sourceParameter, targetParameter;
        public int Id => _relationship.Id;
        /// <summary>
        /// No Parameterless Constructor
        /// </summary>
        private Binding() { }
        /// <summary>
        /// Create the binding relationship
        /// </summary>
        /// <param name="relationship">The related types</param>
        public Binding(IRelationship relationship)
        {
            _relationship = relationship;
            sourceVariable = Expression.Variable(_relationship.SourceType, "castedSource");
            targetVariable = Expression.Variable(_relationship.TargetType, "castedTarget");
            sourceParameter = Expression.Parameter(typeof(object), "sourceObject");
            targetParameter = Expression.Parameter(typeof(object), "targetObject");
        }
        /// <summary>
        /// Return the relationship
        /// </summary>
        /// <returns></returns>
        public IRelationship GetRelationship()
        {
            return _relationship;
        }
        /// <summary>
        /// Return the invokable binding action
        /// </summary>
        /// <returns>Invokable binding action</returns>
        public Action<object, object> GetAction()
        {
            return Bind();
        }
        /// <summary>
        /// Bind the source and target properties
        /// </summary>
        /// <returns>Compiled action</returns>
        private Action<object, object> Bind()
        {
            var expressions = new List<BinaryExpression>()
            {
                Expression.Assign(sourceVariable, Expression.Convert(sourceParameter, _relationship.SourceType)),
                Expression.Assign(targetVariable, Expression.Convert(targetParameter, _relationship.TargetType))
            };

            expressions.AddRange(AssignExpressions());

            BindExpressions();

            return _bindingExpression.Compile();
        }
        /// <summary>
        /// Validate assignability of types
        /// </summary>
        private Func<PropertyInfo, PropertyInfo, bool> IsAssignable = (target, source) => target != null && target.CanWrite && target.PropertyType.IsAssignableFrom(source.PropertyType);
        /// <summary>
        /// Get the expressions for a labda assignment
        /// </summary>
        /// <returns>List of binary expressions</returns>
        private IEnumerable<BinaryExpression> AssignExpressions()
        {
            yield return Expression.Assign(sourceVariable, Expression.Convert(sourceParameter, _relationship.SourceType));
            yield return Expression.Assign(targetVariable, Expression.Convert(targetParameter, _relationship.TargetType));

            foreach (var sourceProperty in _relationship.SourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!sourceProperty.CanRead)
                    continue;

                var targetProperty = _relationship.TargetType.GetProperty(sourceProperty.Name, BindingFlags.Public | BindingFlags.Instance);

                if (IsAssignable(targetProperty, sourceProperty))
                {
                    yield return Expression.Assign(
                        Expression.Property(targetVariable, targetProperty),
                        Expression.Convert(
                            Expression.Property(sourceVariable, sourceProperty), targetProperty.PropertyType));
                }
            }
        }
        /// <summary>
        /// Assign the expressions to a labmda
        /// </summary>
        private void BindExpressions()
        {
            var expressions = AssignExpressions();

            _bindingExpression = Expression.Lambda<Action<object, object>>(
                    Expression.Block(new[] { sourceVariable, targetVariable }, expressions),
                    new[] { sourceParameter, targetParameter });
        }

        public static Binding Create(IRelationship relationship)
        {
            return new Binding(relationship);
        }
    }
}
