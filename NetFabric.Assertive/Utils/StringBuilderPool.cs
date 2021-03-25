using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace NetFabric.Assertive
{
    static class StringBuilderPool
    {
        const int MaximumBuilderSize = 0x100000; // 1 MB

        static readonly ObjectPool<StringBuilder> Pool;

        static StringBuilderPool()
        {
            var provider = new DefaultObjectPoolProvider();
            var policy = new StringBuilderPooledObjectPolicy
            {
                MaximumRetainedCapacity = MaximumBuilderSize,
            };

            Pool = provider.Create(policy);
        }

        public static StringBuilder Get()
            => Pool.Get();

        public static void Return(StringBuilder builder)
            => Pool.Return(builder);
    }
}
