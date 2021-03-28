using System;
using System.Linq.Expressions;
using System.Xml;
using NetFabric.Reflection;
using static System.Linq.Expressions.Expression;

namespace NetFabric.Assertive
{
    static partial class ExpressionEx
    {
        public static Expression ForEach(ParameterExpression enumerable, Func<Expression, Expression> body)
        {
            var enumerableType = enumerable.Type;
            if (!enumerableType.IsEnumerable(out var enumerableInfo, out var errors))
            {
                if (errors.HasFlag(Errors.MissingGetEnumerable))
                    throw new Exception($"'{enumerableType}' does not contain a public definition for 'GetEnumerator'");
                
                if (errors.HasFlag(Errors.MissingCurrent))
                    throw new Exception($"'{enumerableInfo!.GetEnumerator.ReturnType.Name}' does not contain a public definition for 'Current'");

                if (errors.HasFlag(Errors.MissingMoveNext))
                    throw new Exception($"'{enumerableInfo!.GetEnumerator.ReturnType.Name}' does not contain a public definition for 'MoveNext'");

                throw new Exception("Unhandled error!");
            }
            
            var enumeratorType = enumerableInfo.GetEnumerator.ReturnType;
            var enumeratorInfo = enumerableInfo.EnumeratorInfo;
            var enumerator = Variable(enumeratorType, "enumerator");
            return Block(
                new[] { enumerable, enumerator },
                Assign(enumerator, Call(enumerable, enumerableInfo.GetEnumerator)),
                enumeratorInfo switch
                {
                    { Dispose: not null } => Disposable(enumeratorInfo, enumerator, body),
                    _ => enumeratorType switch
                    {
                        { IsValueType: true } => NonDisposableValueType(enumeratorInfo, enumerator, body),
                        _ => NonDisposableReferenceType(enumeratorInfo, enumerator, body)
                    }
                }
            );
                
            static Expression Disposable(EnumeratorInfo enumeratorInfo, Expression enumerator, Func<Expression, Expression> body)
                => Using(enumerator,
                        Enumeration(enumeratorInfo, enumerator, body)
                    );
                
            static Expression NonDisposableValueType(EnumeratorInfo enumeratorInfo, Expression enumerator, Func<Expression, Expression> body)
                => Enumeration(enumeratorInfo, enumerator, body);
                
            static Expression NonDisposableReferenceType(EnumeratorInfo enumeratorInfo, ParameterExpression  enumerator, Func<Expression, Expression> body)
            {
                var disposable = Variable(typeof(IDisposable), "disposable");
                return TryFinally(
                    Enumeration(enumeratorInfo, enumerator, body),
                    Block(
                        new [] { disposable, enumerator },
                        Assign(disposable, TypeAs(enumerator, typeof(IDisposable))),
                        IfThen(
                            NotEqual(disposable, Constant(null)),
                            Call(disposable, typeof(IDisposable).GetMethod("Dispose")!)
                        )
                    )
                );
            }

            static Expression Enumeration(EnumeratorInfo enumeratorInfo, Expression enumerator, Func<Expression, Expression> body)
                => While(Call(enumerator, enumeratorInfo.MoveNext),
                    body(Property(enumerator, enumeratorInfo.Current))
                );
        }
    }
}