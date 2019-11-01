using System;
using Shared.Domains;

namespace Shared.Library
{
    public class DefaultCloneOptions
    {
        public CloneType DefaultCloneType { get; set; }
        public bool UseMessagePack { get; set; }
    }
}
