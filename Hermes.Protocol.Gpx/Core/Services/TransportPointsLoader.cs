using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Text;
using Hermes.Protocol.Gpx.Core.Contracts;
using Serilog;

namespace Hermes.Protocol.Gpx.Core.Services
{
    public sealed class PointsLoaderEvtArg : EventArgs
    {
        public List<object> Result { get; set; }
        
        public Exception Exception { get; set; }
    }

    public delegate void PointsLoaderEvent(object sender, PointsLoaderEvtArg arg);

 
}