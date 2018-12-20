using System;
using System.Diagnostics.CodeAnalysis;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    /// <inheritdoc />
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public sealed class DataBaseSaveControllerEventArg : EventArgs
    {
        public object Result { get; set; }
        
        public Exception Exception { get; set; }
    }
}