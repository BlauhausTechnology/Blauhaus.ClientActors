using System;
using System.Threading.Tasks;
using Blauhaus.ClientDatabase.Sqlite.Service;
using Blauhaus.Common.Abstractions;
using SQLite;

namespace Blauhaus.ClientActors.Sqlite
{
    public abstract class SqliteActor : IAsyncInitializable<string>
    {
        private readonly ISqliteDatabaseService _sqliteDatabaseService;
        private readonly SQLiteAsyncConnection _connection = null!;

        protected string Key { get; private set; } = null!;

        protected SqliteActor(ISqliteDatabaseService sqliteDatabaseService)
        {
            _sqliteDatabaseService = sqliteDatabaseService;
        }

        public async Task InitializeAsync(string key)
        {
            Key = key;
            await _sqliteDatabaseService.AsyncConnection.RunInTransactionAsync(LoadData);
        }

        public Task ShutdownAsync()
        {
            throw new NotImplementedException();
        }

        protected abstract void LoadData(SQLiteConnection connection);

        protected async Task ExecuteAsync(Action<SQLiteConnection> databaseAction)
        {
            await _connection.RunInTransactionAsync(databaseAction.Invoke);
        }
    }
}
