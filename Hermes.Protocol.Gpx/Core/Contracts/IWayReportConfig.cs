using System;
using System.Collections.Generic;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public interface IWayReportConfig
    {
        IEnumerable<DateTime> Dates { get; set; }

        Dictionary<DateTime, IEnumerable<IEnumerable<IPoint>>> Routes { get; set; }

        string Context {get;set;}
    }

    internal sealed class WayReportConfig : IWayReportConfig
    {
        public IEnumerable<DateTime> Dates { get; set; }
        public Dictionary<DateTime, IEnumerable<IEnumerable<IPoint>>> Routes { get; set; }
        public string Context { get; set; }
    }
}