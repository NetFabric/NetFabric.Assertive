using System;
using System.Linq.Expressions;
using NetFabric.Reflection;
using static System.Linq.Expressions.Expression;

namespace NetFabric.Assertive
{
    static partial class ExpressionEx
    {
        public static Expression ForEach(Expression enumerable, ParameterExpression loopVar, Func<Expression, Expression> content)
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

                throw new Exception("Unhandled exception!");
            }
            
            var enumeratorType = enumerableInfo.GetEnumerator.ReturnType;
            var enumeratorInfo = enumerableInfo.EnumeratorInfo;
            var enumeratorVariable = Variable(enumeratorType, "enumerator");

            return Block(
                new[] {enumeratorVariable},
                Assign(enumeratorVariable, Call(enumerable, enumerableInfo.GetEnumerator)),
                enumeratorType.IsValueType switch
                {
                    true => EnumerateValueType(enumeratorInfo, enumeratorVariable, Property(enumeratorVariable, enumeratorInfo.Current)),
                    _ => EnumerateReferenceType(enumeratorInfo, enumeratorVariable, Property(enumeratorVariable, enumeratorInfo.Current))
                });
        }
        
        static Expression EnumerateValueType(EnumeratorInfo enumeratorInfo, ParameterExpression enumeratorVariable, Expression content)
        {
            return enumeratorInfo switch 
            {
                { Dispose: null } => content,
                { IsByRefLike: true } => DisposeByRefLike(enumeratorInfo, enumeratorVariable, content),
                _ => Dispose(enumeratorInfo, enumeratorVariable, content)
            };
            
            static Expression DisposeByRefLike(EnumeratorInfo enumeratorInfo, ParameterExpression enumeratorVariable, Expression content)
                => TryFinally(
                    content,
                    Call(enumeratorVariable, enumeratorInfo.Dispose!));

            static Expression Dispose(EnumeratorInfo enumeratorInfo, ParameterExpression enumeratorVariable, Expression content)
                => TryFinally(
                    content,
                    Call(Convert(enumeratorVariable, typeof(IDisposable)), enumeratorInfo.Dispose!));
        }

        static Expression EnumerateReferenceType(EnumeratorInfo enumeratorInfo, ParameterExpression enumeratorVariable, Expression content)
        {
            return enumeratorInfo switch
            {
                { Dispose: null } => NotDisposable(enumeratorInfo, enumeratorVariable, content),
                _ => Disposable(enumeratorInfo, enumeratorVariable, content)
            };
            
            static Expression NotDisposable(EnumeratorInfo enumeratorInfo, ParameterExpression enumeratorVariable, Expression content)
                => TryFinally(
                    content,
                    Call(enumeratorVariable, enumeratorInfo.Dispose!));

            static Expression Disposable(EnumeratorInfo enumeratorInfo, ParameterExpression enumeratorVariable, Expression content)
                => TryFinally(
                    content,
                    Call(Convert(enumeratorVariable, typeof(IDisposable)), enumeratorInfo.Dispose!));
        }
    }
}