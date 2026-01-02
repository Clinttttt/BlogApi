namespace BlogApi.Client.Security
{
    // This service is now a NO-OP to avoid breaking other code
    // It intentionally does NOT cache anything
    public class TokenCacheService
    {
        public string? GetToken() => null;
        public void CacheToken(string token) { }
        public void ClearCache() { }
    }
}
