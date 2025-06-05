namespace TestBackend.ServiceLibrary.Repositories.Interfaces
{
    public interface ISqlConnectionWrapper
    {
        Task<int> ExecuteAsync(string sql, object? parameters = null);

        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null);
    }
}