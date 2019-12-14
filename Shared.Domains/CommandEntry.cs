using Microsoft.Data.SqlClient;

namespace Shared.Domains
{
    public sealed class CommandEntry
    {
        private CommandEntry(string name, string command, SqlDependency sqlDependency = null)
        {
            Name = name;
            Command = command;
            if(sqlDependency != null)
                SqlDependency = sqlDependency;
        }

        public static CommandEntry Create(string name, string command)
        {
            return new CommandEntry(name, command);
        }

        public static CommandEntry Create(string name, string command, SqlDependency sqlDependency)
        {
            return new CommandEntry(name, command, sqlDependency);
        }

        public string Name { get; }
        public string Command { get; }
        public SqlDependency SqlDependency { get; set; }
    }
}