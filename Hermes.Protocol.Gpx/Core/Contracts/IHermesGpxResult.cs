using System;
using System.Collections.Generic;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public interface IHermesGpxResult
     {
         IEnumerable<Track> Routes { get; set; }
         Dictionary<DateTime,double> Dates { get; set; } 
         IUploadDataParams Params { get; set; }
     }

    internal sealed class HermesGpxResult : IHermesGpxResult
    {
        public IEnumerable<Track> Routes { get;set; }
        public Dictionary<DateTime, double> Dates { get;set; }
        public IUploadDataParams Params { get;set; }
    }
}