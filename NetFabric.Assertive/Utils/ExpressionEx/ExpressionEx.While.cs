using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;

namespace NetFabric.Assertive
{
    static partial class ExpressionEx
    {
        public static Expression While(Expression test, Expression content)
        {
            var breakLabel = Label();
            return Loop(
                IfThenElse(
                    test,
                    content,
                    Break(breakLabel)),
                breakLabel);
        }
    }
}