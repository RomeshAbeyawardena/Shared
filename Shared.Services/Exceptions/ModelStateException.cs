using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.Contracts.Builders;
using Shared.Services.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services.Exceptions
{
    public sealed class ModelStateException : Exception
    {
        public ModelStateException(Action<IDictionaryBuilder<string, string>> memberExceptions)
        {
            ModelStateBuilder = DictionaryBuilder.Create<string, string>();
            memberExceptions?.Invoke(ModelStateBuilder);
        }

        private ModelStateException()
        {

        }

        private ModelStateException(string message)
            : base(message)
        {

        }

        private ModelStateException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public IDictionaryBuilder<string, string> ModelStateBuilder { get; private set; }
    }
}
