using Microsoft.Data.SqlClient;
using Shared.Contracts;
using Shared.Domains;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
namespace Shared.Services
{
    public sealed class DefaultSqlDependencyManager : ISqlDependencyManager
    {
        public event EventHandler<CommandEntry> OnChange;

        public IDictionary<string, CommandEntry> CommandEntries
        {
            get
            {
                var dictionary = new Dictionary<string, CommandEntry>();

                foreach(var commandEntry in _commandEntries)
                {
                    if(commandEntry.SqlDependency == null)
                        throw new NotSupportedException("SqlDependencyManager not started!");
                    dictionary.Add(commandEntry.SqlDependency.Id, commandEntry);
                }
                return dictionary;
            }
        }

        public void AddCommandEntry(string name, string command)
        {
            AddCommandEntry(CommandEntry.Create(name, command));
        }

        public void AddCommandEntry(CommandEntry commandEntry)
        {
            _commandEntries.Add(commandEntry);
        }

        public void Dispose()
        {
            Dispose(true);
            
        }

        public async Task Start(string connectionString)
        {
            SqlDependency.Start(connectionString);
            _sqlConnection = new SqlConnection(connectionString);
            await Begin()
                .ConfigureAwait(false);
        }

        public void Stop(string connectionString)
        {
            SqlDependency.Stop(connectionString);
        }

        
        private void Dispose(bool gc)
        {
            _sqlConnection.Dispose();
        }


        private async Task Begin()
        {
            for(var entryIndex = 0; entryIndex < _commandEntries.Count; entryIndex++)
            {
                var entry = _commandEntries[entryIndex];
                entry.SqlDependency = await CreateSqlDependency(entry).ConfigureAwait(false);
            }

        }

        private async Task<SqlDependency> CreateSqlDependency(CommandEntry commandEntry)
        {
            var sqlDependency = new SqlDependency();
            sqlDependency.OnChange += SqlDependency_OnChange;
            using (var sqlCommand = new SqlCommand(commandEntry.Command, _sqlConnection)){
                sqlDependency.AddCommandDependency(sqlCommand);
                await sqlCommand.ExecuteReaderAsync()
                    .ConfigureAwait(false);
            }
            return sqlDependency;
        }

        private void SqlDependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if(!(sender is SqlDependency sqlDependency))
                throw new InvalidOperationException();

            var commandEntries = from commandEntry in _commandEntries 
                                 where commandEntry.SqlDependency.Id == sqlDependency.Id
                                 select commandEntry;

            OnChange?.Invoke(this, commandEntries.FirstOrDefault());
        }

        public DefaultSqlDependencyManager()
        {
            _commandEntries = new List<CommandEntry>();
        }

        private readonly IList<CommandEntry> _commandEntries;
        private SqlConnection _sqlConnection;
    }
}