using System;

namespace Shared.Contracts.DapperExtensions
{
    public interface IDapperContext : IDisposable
    {
        void MapContext(IDapperContext dapperContext = null);
    }
}