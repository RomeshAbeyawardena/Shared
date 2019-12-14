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
        public event EventHandler<CommandEntrySqlNotificationEventArgs> OnChange;

        public IDictionary<string, CommandEntry> CommandEntries
        {
            get
            {
                var dictionary = new Dictionary<string, CommandEntry>();

                foreach (var commandEntry in _commandEntries)
                {
                    if (commandEntry.SqlDependency == null)
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
            End();
        }


        private void Dispose(bool gc)
        {
            End();
            _sqlConnection.Dispose();
        }


        private async Task Begin()
        {
            await _sqlConnection.OpenAsync()
                .ConfigureAwait(false);

            for (var entryIndex = 0; entryIndex < _commandEntries.Count; entryIndex++)
            {
                var entry = _commandEntries[entryIndex];
                entry.SqlDependency = await CreateSqlDependency(entry).ConfigureAwait(false);
            }

        }

        private void End()
        {
            foreach (var entry in _commandEntries)
            {
                entry.Dispose();
            }
        }

        private async Task<SqlDependency> CreateSqlDependency(CommandEntry commandEntry)
        {
            var sqlDependency = new SqlDependency();
            sqlDependency.OnChange += SqlDependency_OnChange;

            commandEntry.DbCommand = new SqlCommand(commandEntry.Command, _sqlConnection);
            sqlDependency.AddCommandDependency(commandEntry.DbCommand);
            await commandEntry.DbCommand.ExecuteReaderAsync()
                .ConfigureAwait(false);

            return sqlDependency;
        }

        private void SqlDependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (!(sender is SqlDependency sqlDependency))
                throw new InvalidOperationException();

            var commandEntries = from commandEntry in _commandEntries
                                 where commandEntry.SqlDependency.Id == sqlDependency.Id
                                 select commandEntry;
            Console.WriteLine(e);
            OnChange?.Invoke(this, new CommandEntrySqlNotificationEventArgs(commandEntries.FirstOrDefault(), e.Type, e.Info, e.Source));
        }

        public DefaultSqlDependencyManager()
        {
            _commandEntries = new List<CommandEntry>();
        }

        private readonly IList<CommandEntry> _commandEntries;
        private SqlConnection _sqlConnection;
    }
}