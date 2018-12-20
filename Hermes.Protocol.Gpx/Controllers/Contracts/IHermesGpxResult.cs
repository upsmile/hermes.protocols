using System;
using System.Collections.Generic;

namespace Hermes.Protocol.Gpx.Controllers.Contracts
{
    public interface IHermesGpxResult
     {
         IEnumerable<TrackRouteStorage> Routes { get; set; }
         Dictionary<DateTime,double> Dates { get; set; } 
         IUploadDataParams Params { get; set; }
     }

    internal sealed class HermesGpxResult : IHermesGpxResult
    {
        public IEnumerable<TrackRouteStorage> Routes { get;set; }
        public Dictionary<DateTime, double> Dates { get;set; }
        public IUploadDataParams Params { get;set; }
    }
}