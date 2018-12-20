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
}