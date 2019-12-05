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

        protected ModelStateException()
        {

        }

        protected ModelStateException(string message)
            : base(message)
        {

        }

        protected ModelStateException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public IDictionaryBuilder<string, string> ModelStateBuilder { get; private set; }
    }
}
