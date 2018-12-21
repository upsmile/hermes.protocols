using System;
using System.Collections.Generic;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    internal sealed class WayReportConfig : IWayReportConfig
    {
                
        public string Context { get; set; }
        
        public IParserResult Track { get; set; }
        
    }
}