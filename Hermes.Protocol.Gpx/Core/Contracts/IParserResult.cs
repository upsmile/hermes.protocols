using System;
using System.Collections.Generic;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public interface IParserResult
     {
         IEnumerable<Track> Routes { get; set; }
         
         Dictionary<DateTime,double> Dates { get; set; }
         
         IUploadDataParams Params { get; set; }
     }

    internal sealed class ParserResult : IParserResult
    {
        public IEnumerable<Track> Routes { get;set; }
        public Dictionary<DateTime, double> Dates { get;set; }
        public IUploadDataParams Params { get;set; }
    }
}