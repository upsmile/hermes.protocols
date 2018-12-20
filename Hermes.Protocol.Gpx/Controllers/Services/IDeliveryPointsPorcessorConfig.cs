using System;
using System.Collections.Generic;

namespace Hermes.Protocol.Gpx.Controllers.Services
{
    internal interface IDeliveryPointsPorcessorConfig
    {
        IEnumerable<DateTime> Dates { get; set; }

        Dictionary<DateTime, IEnumerable<IEnumerable<IPoint>>> Routes { get; set; }

        string Context {get;set;}
    }
}