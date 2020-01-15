using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Memory;

namespace Microsoft.Extensions.Logging
{
    public static class MemoryLoggerExtensions
    {
        public static ILoggingBuilder AddMemory(this ILoggingBuilder builder, int maxMemoryCache)
        {
            builder.Services.AddSingleton(s => new MemoryLoggerStorage(maxMemoryCache));
            builder.Services.AddSingleton<IMemoryLoggerStorage>(s => s.GetRequiredService<MemoryLoggerStorage>());
            builder.Services.AddSingleton<IMemoryLoggerWriter>(s => s.GetRequiredService<MemoryLoggerStorage>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, MemoryLoggerProvider>());
            return builder;
        }

        public static ILoggingBuilder AddMemory(this ILoggingBuilder builder)
            => AddMemory(builder, 500);
    }
}
