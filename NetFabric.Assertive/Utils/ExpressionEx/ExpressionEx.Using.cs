using System;
using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;

namespace NetFabric.Assertive
{
    static partial class ExpressionEx
    {
        public static Expression Using(ParameterExpression variable, Expression value, Expression content)
        {
            return Block(new[] { variable },
                Assign(variable, value),
                TryFinally(
                    content,
                    variable.Type switch 
                    {
                        {IsValueType: true} => ValueTypeUsing(variable, content),
                        
                        _ => ReferenceTypeUsing(variable, content)
                    }));

            static Expression ValueTypeUsing(Expression variable, Expression content)
            {
                return variable.Type.IsByRefLike() switch
                {
                    true => ByRefLikeValueTypeUsing(variable, content),
                    
                    _ => ValueTypeUsing(variable, content)
                };

                static Expression ByRefLikeValueTypeUsing(Expression variable, Expression content)
                {
                    var disposeMethod = variable.Type.GetMethod("Dispose");
                    return disposeMethod switch
                    {
                        null => ThrowMustBeImplicitlyConvertibleToIDisposable<Expression>(variable),
                        
                        _ => Call(variable, disposeMethod)
                    };
                }

                static Expression ValueTypeUsing(Expression variable, Expression content)
                    => typeof(IDisposable).IsAssignableFrom(variable.Type) switch
                    {
                        false => ThrowMustBeImplicitlyConvertibleToIDisposable<Expression>(variable),
                        
                        _ => Call(Convert(variable, typeof(IDisposable)),
                                typeof(IDisposable).GetMethod("Dispose")!)
                    };
            }

            static Expression ReferenceTypeUsing(Expression variable, Expression content)
                => typeof(IDisposable).IsAssignableFrom(variable.Type) switch
                {
                    false => ThrowMustBeImplicitlyConvertibleToIDisposable<Expression>(variable),

                    _ => IfThen(
                            NotEqual(variable, Constant(null)),
                            Call(
                                Convert(variable, typeof(IDisposable)),
                                typeof(IDisposable).GetMethod("Dispose")!))
                };

            static T ThrowMustBeImplicitlyConvertibleToIDisposable<T>(Expression variable)
                => throw new Exception($"'{variable.Type.FullName}': type used in a using statement must be implicitly convertible to 'System.IDisposable'");
        }
    }
}