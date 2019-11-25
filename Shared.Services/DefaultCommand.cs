using Shared.Contracts;
using Shared.Contracts.Builders;
using Shared.Services.Builders;
using System;
using System.Collections.Generic;

namespace Shared.Services
{
    public static class DefaultCommand
    {
        public static ICommand Create<T>(string name, IDictionary<string, object> parameters)
        {
            return new DefaultCommand<T>(name, parameters);
        }

        public static ICommand Create<T>(string name, Action<IDictionaryBuilder<string, object>> commandParameters)
        {
            var parameters = DictionaryBuilder.Create<string, object>();
            commandParameters(parameters);
            return new DefaultCommand<T>(name, parameters.ToDictionary());
        }
    }
    public class DefaultCommand<T> : ICommand
    {
        public DefaultCommand(string name, IDictionary<string, object> parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        public string Name { get; set; }
        public IDictionary<string, object> Parameters { get; set; }
    }
}
