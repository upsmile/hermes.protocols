using System;
using System.Collections.Generic;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public interface IWayReportConfig
    {
        IParserResult Track { get; set; }
        
        string Context { get; set; }

    }
}