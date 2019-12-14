using Shared.Domains;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public interface ISqlDependencyManager : IDisposable
    {
        event EventHandler<CommandEntry> OnChange;
        void AddCommandEntry(string name, string command);
        void AddCommandEntry(CommandEntry commandEntry);
        Task Start(string connectionString);
        void Stop(string connectionString);
        IDictionary<string, CommandEntry> CommandEntries { get; }
    }
}