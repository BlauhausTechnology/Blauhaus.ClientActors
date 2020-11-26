using System;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.ClientDatabase.Sqlite.Service;
using Blauhaus.Common.Utils.Contracts;
using SQLite;

namespace Blauhaus.ClientActors.Sqlite
{
    public abstract class SqliteActor : IInitialize<string>
    {
        private readonly ISqliteDatabaseService _sqliteDatabaseService;
        private SQLiteAsyncConnection _connection;

        protected string Key { get; private set; }

        protected SqliteActor(ISqliteDatabaseService sqliteDatabaseService)
        {
            _sqliteDatabaseService = sqliteDatabaseService;
        }

        public async Task InitializeAsync(string key)
        {
            Key = key;
            _connection = await _sqliteDatabaseService.GetDatabaseConnectionAsync();
            await _connection.RunInTransactionAsync(LoadData);
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
