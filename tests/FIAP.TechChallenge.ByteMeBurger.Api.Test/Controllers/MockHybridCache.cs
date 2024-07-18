using Microsoft.Extensions.Caching.Hybrid;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Test.Controllers;

internal class MockHybridCache : HybridCache
{
    public override ValueTask<T> GetOrCreateAsync<TState, T>(string key, TState state, Func<TState, CancellationToken, ValueTask<T>> factory, HybridCacheEntryOptions? options = null,
        IReadOnlyCollection<string>? tags = null, CancellationToken token = new CancellationToken())
    {
        return factory.Invoke(state, token);
    }

    public override ValueTask SetAsync<T>(string key, T value, HybridCacheEntryOptions? options = null, IReadOnlyCollection<string>? tags = null,
        CancellationToken token = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public override ValueTask RemoveAsync(string key, CancellationToken token = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public override ValueTask RemoveByTagAsync(string tag, CancellationToken token = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}
