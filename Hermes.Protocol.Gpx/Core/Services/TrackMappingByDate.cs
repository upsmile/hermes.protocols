using System;
using System.Collections.Generic;
using Hermes.Protocol.Gpx.Core.Contracts;

namespace Hermes.Protocol.Gpx.Core.Services
{
    public class TrackMappingByDate: Dictionary<DateTime, IEnumerable<IEnumerable<IPoint>>> 
    {
        
    }
}