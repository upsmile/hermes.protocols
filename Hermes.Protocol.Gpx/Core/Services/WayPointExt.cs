using System;
using System.Collections.Generic;
using System.Linq;
using Hermes.Protocol.Gpx.Core.Contracts;

namespace Hermes.Protocol.Gpx.Core.Services
{
    public static class WayPointExt
    {
        // TODO: Refactoting!
        private static IEnumerable<IPoint> ToPoints(this IEnumerable<ITrackPoint> points)
        {
            return points.Select(ToPoint).ToList();
        }

        // TODO: Refactoting!
        private static IPoint ToPoint(this ITrackPoint tp)
        {
            var result = new Point
            {
                Position = new Position(tp.Latitude, tp.Longtitude),
                Speed = tp.Speed,
                Name = tp.Name,
                Time = tp.Time,
                Ele = tp.Ele
            };
            return result;
        }


        public static TrackMappingByDate ToTrackMapping(this IEnumerable<Track> context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var result = new TrackMappingByDate();
            foreach (var item in context)
            {
                foreach (var d in item)
                {
                    var trackDate = d.Key.TrackDate;
                    var segments = d.Value;
                    var list = segments.Select(segment => segment.Value.ToPoints()).ToList();
                    result.Add(trackDate, list);
                }
            }
            return result;
        }
    }
}