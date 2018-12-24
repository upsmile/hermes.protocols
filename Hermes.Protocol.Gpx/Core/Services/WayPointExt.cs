using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Hermes.Protocol.Gpx.Core.Contracts;

namespace Hermes.Protocol.Gpx.Core.Services
{
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public static class WayPointExt
    {
        // TODO: Refactorings!
        private static IEnumerable<IPoint> ToPoints(this IEnumerable<ITrackPoint> points)
        {
            return points.Select(ToPoint).ToList();
        }

        // TODO: Refactorings!
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


        private const double EarthRadius = 6378.7;

        private static double GetPointsDistance(IPosition point1, IPosition point2)
        {
            var dLat1InRad = point1.Lat * (Math.PI / 180.0);
            var dLong1InRad = point1.Lng * (Math.PI / 180.0);
            var dLat2InRad = point2.Lat * (Math.PI / 180.0);
            var dLong2InRad = point2.Lng * (Math.PI / 180.0);
            var dLongitude = dLong2InRad - dLong1InRad;
            var dLatitude = dLat2InRad - dLat1InRad;
            var x = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                    Math.Cos(dLat1InRad) * Math.Cos(dLat2InRad) *
                    Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);
            var dist = 2.0 * Math.Atan2(Math.Sqrt(x), Math.Sqrt(1.0 - x));
            return EarthRadius * dist;
        }
        
        private static IPosition ToGeoPoint(this Pos position)
        {
            return  new Position
            {
                Lat = position.Lat,
                Lng = position.Lon
            };

        }
        
        public static Dictionary<DeliveryPointCache, IEnumerable<IPoint>> NearestPoints(this IEnumerable<IEnumerable<IPoint>> route,
            IEnumerable<DeliveryPointCache> points, double radius)
        {
            if (route == null)
                throw new ArgumentNullException(nameof(route));
            if (points == null)
                throw new ArgumentNullException(nameof(points));
            var result = new Dictionary<DeliveryPointCache, IEnumerable<IPoint>>();
            foreach (var p in points)
            {
                var list = new List<IPoint>();
                var enumerable = route as IEnumerable<IPoint>[] ?? route.ToArray();               
                foreach (var segment in enumerable)
                {
                    list.AddRange(segment.Where(point => GetPointsDistance(point.Position, p.Position.ToGeoPoint()) <= radius));
                }
                result.Add(p, list);
            }
            return result;
        }
        
        
        public static void CheckVisitedPoints(List<IPoint> source, List<IPoint> points,
            ref Dictionary<Guid, IList<IPoint>> result, DeliveryPointCache point, double radius)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (points == null)
                throw new ArgumentNullException(nameof(points));
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            if (points.Count == 0)
                return;
            var item = points[0];
            var index = source.IndexOf(item);
            if (index < 0)
                return;
            var list = source.SkipWhile(x => source.IndexOf(x) < index).ToList();            
            var q = list.TakeWhile(x => GetPointsDistance(x.Position, point.Position.ToGeoPoint()) <= radius).ToList();
            result.Add(Guid.NewGuid(), q);
            var p = points.Except(q).ToList();
            var src = list.Except(q).ToList();
            CheckVisitedPoints(src, p, ref result, point, radius);
        }
        
        public static IEnumerable<IDeliveryPoint> ToVisitedPointsList(
            this Dictionary<DeliveryPointCache, IEnumerable<IPoint>> value, 
            List<IPoint> source, double radius)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            var result = new List<IDeliveryPoint>();
            foreach (var element in value)
            {
                var way = element.Key;
                var reappoints = element.Value.ToList();
                var refreshments = new Dictionary<Guid, IList<IPoint>>();

                CheckVisitedPoints(source.ToList(), reappoints, ref refreshments, way, radius);

                foreach (var p in from refreshment in refreshments
                    select refreshment.Value.OrderBy(x => x.Time).ToList()
                    into l
                    where l.Count > 0
                    select new DeliveryPoint
                    {
                        Code = way.Header.Code,
                        Position = way.Position.ToGeoPoint(),
                        TimeIn = l[0].Time,
                        TimeOut = l[l.Count - 1].Time
                    })
                {
                    if (way.Header.Contractors!= null)
                        p.ContractItem = way.Header.Contractors;
                    result.Add(p);
                }
            }
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