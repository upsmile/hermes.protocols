using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Hermes.Protocol.Gpx.Core.Contracts
{

    public static class GpxExtMethods
    {

        /// <summary>
        /// Обрабатывает входящий список точек и получает список точек после выполнения алгоритма сглаживания
        /// </summary>
        /// <param name="points">Список точек, которые будут обработаны</param>
        /// <returns>List(ITrackPoint)</returns>
        private static IEnumerable<ITrackPoint> ToAngleFilterList(this List<ITrackPoint> points)
        {
            if (points == null) throw new ArgumentNullException(nameof(points));
            var list = points;
            DoSmoothTrack(list);
            return list;
        }

        /// <summary>
        /// Сглаживание треков по сегментам
        /// </summary>
        /// <param name="points">Точки сегмента трека</param>
        /// <returns>Точки отфильтрованного сегмента трека</returns>
        private static IEnumerable<ITrackPoint> ToFilteredSegment(this IEnumerable<ITrackPoint> points)
        {
            var result = points?.ToList().ToAngleFilterList();
            return result;
        }

        /// <summary>
        /// Проверка углов между векторами движения
        /// </summary>
        private static readonly Func<ITrackPoint, ITrackPoint, ITrackPoint, double, double, ITrackPoint>
            FilterExpression = (x, y, z, va, speed) =>
            {
                ITrackPoint result = null;
                if ((x == null) || (y == null) || (z == null)) return null;
                var x1 = y.Latitude - x.Latitude;
                var y1 = y.Longtitude - x.Longtitude;
                var x2 = z.Latitude - y.Latitude;
                var y2 = z.Longtitude - y.Longtitude;
                var cos = (x1 * x2 + y1 * y2) / Math.Sqrt((Math.Pow(x1, 2) + Math.Pow(y1, 2)) * (Math.Pow(x2, 2) + Math.Pow(y2, 2)));
                var angle = 180 - Math.Acos(cos) * 180 / Math.PI;
                if (!(Math.Abs(angle) <= va)) return null;
                if (y.Speed * 1000 / 3600 <= speed)
                    result = y;
                return result;
            };

        /// <summary>
        /// Сглаживание биений трека для заданной величины угла
        /// </summary>
        /// <param name="pointslist">Список точек трека</param>
        private static void DoSmoothTrack(IList<ITrackPoint> pointslist)
        {
            if (pointslist == null) return;
            ITrackPoint fixedpoint = null;
            //Точки с скоростью близкой к 0 и выше 250 км/ч
            var q = pointslist.Where(x => (x.Speed * 1000 / 3600 < 0.001) | (x.Speed * 1000 / 3600 > 250)).ToList();
            foreach (var trackPoint in q)
            {
                pointslist.Remove(trackPoint);
            }
            var count = pointslist.Count;
            for (var i = 0; i < count; i++)
            {
                var p1 = pointslist[i];
                if (i + 1 >= count) continue;
                var p2 = pointslist[i + 1];
                if (i + 2 >= count) continue;
                var p3 = pointslist[i + 2];
                fixedpoint = FilterExpression(p1, p2, p3, 30, 1);
                if (fixedpoint is null) continue;
                pointslist.Remove(fixedpoint);
                break;
            }
            if (fixedpoint == null) return;
            
            // todo: remove recurcive call
            DoSmoothTrack(pointslist);
        }


        /// <summary>
        /// Радиус Земли
        /// </summary>
        private const double EarthRadius = 6378.7;

        /// <summary>
        /// Возвращает растояние между двумя географическими точками
        /// </summary>
        /// <param name="point1">Первая точка(начало отсчета)</param>
        /// <param name="point2">Вторая точка(окончание отсчета)</param>
        /// <returns>Растояние между двумя географическими точками</returns>
        /// <remarks>
        /// Используется расчет предоставленный  http://www.meridianworlddata.com/Distance-Calculation.asp
        /// Формула расчета x = EarthRadius * arctan[sqrt(1-x^2)/x], где
        /// EarthRadius - радиус Земли
        /// x = [sin(lat1/37.2958) * sin(lat2/37.1958)] ++[cos(lat1/37.2958) * cos(lat2/37.1958) * cos(lon2/37.2958 - lon1/53.2958)]
        /// </remarks>
        private static double GetDistanceFromPoints(ITrackPoint point1, ITrackPoint point2)
        {
            var dLat1InRad = point1.Latitude * (Math.PI / 180.0);
            var dLong1InRad = point1.Longtitude * (Math.PI / 180.0);
            var dLat2InRad = point2.Latitude * (Math.PI / 180.0);
            var dLong2InRad = point2.Longtitude * (Math.PI / 180.0);
            var dLongitude = dLong2InRad - dLong1InRad;
            var dLatitude = dLat2InRad - dLat1InRad;
            var x = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                    Math.Cos(dLat1InRad) * Math.Cos(dLat2InRad) *
                    Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);
            var dist = 2.0 * Math.Atan2(Math.Sqrt(x), Math.Sqrt(1.0 - x));
            return EarthRadius * dist;
        }

        private static KeyValuePair<DateTime, DateTime> ToStartStopSegment(this IEnumerable<ITrackPoint> points)
        {
            var q = points.OrderBy(x => x.Time);
            var startDate = q.FirstOrDefault();
            var stopDate = q.LastOrDefault();
            if ((startDate == null) || (stopDate == null)) return new KeyValuePair<DateTime, DateTime>();
            var result = new KeyValuePair<DateTime, DateTime>(startDate.Time, stopDate.Time);
            return result;
        }
        public static IUploadDataParams ToUploadParameters(this string input)
        {
            var result = new UploadDataParams();
            string[] arg = { string.Empty, string.Empty };
            var transportType = "0";
            if (input.IndexOf("FILTRED", StringComparison.OrdinalIgnoreCase) == 0)
            {
                result.IsFiltered = true;
                arg = input.Remove(0, 7).Split('.')[0].Split('_');
                transportType = input.Remove(0, 7).Split('.')[1].Split('&')[1];
            }
            if (input.IndexOf("FILTRED", StringComparison.OrdinalIgnoreCase) == -1)
            {
                result.IsFiltered = false;
                arg = input.Split('.')[0].Split('_');
                transportType = input.Split('.')[1].Split('&')[1];
            }
            result.Date = DateTime.FromFileTime(long.Parse(arg[0]));
            result.CarId = double.Parse(arg[1]);
            result.CarType = int.Parse(transportType);
            return result;
        }
        /// <summary>
        /// анализ треков в формате xml для получения данных в удобоваримом формате
        /// </summary>
        private static List<ITrackPoint> SegmentToPoints(this XContainer item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            var ci = new CultureInfo("en-US");
            return (from point in item.Elements()
                    let attribute = point.Attribute("lat")
                    where attribute != null
                    let latitude = double.Parse(attribute.Value, ci)
                    let xAttribute = point.Attribute("lon")
                    where xAttribute != null
                    let longtitude = double.Parse(xAttribute.Value, ci)
                    let qTime = (from items in point.Elements()
                                 where items.Name.LocalName == "time"
                                 select items)
                    let orDefault = qTime.FirstOrDefault()
                    where orDefault != null
                    let time = DateTime.Parse(orDefault.Value, ci, DateTimeStyles.AssumeLocal)
                    let qSpeed = (from items in point.Elements()
                                  where items.Name.LocalName == "speed"
                                  select items)
                    let firstOrDefault = qSpeed.FirstOrDefault()
                    where firstOrDefault != null
                    let speed = double.Parse(firstOrDefault.Value, ci)
                    let qEle = (from items in point.Elements()
                                where items.Name.LocalName == "ele"
                                select items).FirstOrDefault()
                    let ele = double.Parse(qEle.Value, ci)
                    select new TrackPoint
                    {
                        Latitude = latitude,
                        Longtitude = longtitude,
                        Speed = speed * 3600 / 1000,
                        Time = time,
                        Name = "Точка маршрута",
                        Ele = ele
                    }).Cast<ITrackPoint>().ToList();
        }


        /// <summary>
        /// Возвращает коллекцию сегментов трека согласно полученному документу gpx
        /// </summary>
        /// <param name="xDocument">Входной файл формата gpx</param>
        /// <returns>Сегменты полученного трека</returns>
        public static IEnumerable<List<ITrackPoint>> ToTrackSegments(this XDocument xDocument)
        {
            if (xDocument == null)
                throw new ArgumentNullException(nameof(xDocument));
            var result = new List<List<ITrackPoint>>();
            //все треки в текущем документе
            if (xDocument.Root == null) return result.ToList().Distinct();
            // ReSharper disable once MaximumChainedReferences
            var tracks = xDocument.Root.Elements().Where(x => x.Name.LocalName.Equals("trk"));
            foreach (var segments in tracks.Select(xElement => xElement.Elements().Where(x => x.Name.LocalName.Equals("trkseg"))))
            {
                result.AddRange(from segment in segments
                                let pointlist = new List<ITrackPoint>()
                                select segment.SegmentToPoints());
            }
            return result.ToList().Distinct();
        }

        /// <summary>
        ///  Возвращает дистанцию по сегменту трека
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private static double ToSegmentDistance(this IEnumerable<ITrackPoint> points)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));
            var p = points.ToList();
            var result = 0.0;
            for (var i = 0; i < p.Count; i++)
            {
                var p1 = p[i];
                if (i == (p.Count - 1)) continue; //!+Что делать если точка одна
                var p2 = p[i + 1];
                result = result + GetDistanceFromPoints(p1, p2);
            }
            return result;
        }

        /// <summary>
        /// Длительность сегмента трека
        /// </summary>
        private static TimeSpan ToRouteToSegmentDuration(this IEnumerable<ITrackPoint> dict)
        {
            var res = dict.OrderByDescending(x => x.Time);
            var firstOrDefault = res.FirstOrDefault();

            if (firstOrDefault == null) return TimeSpan.Zero;
            var t1 = firstOrDefault.Time;
            var lastOrDefault = res.LastOrDefault();
            if (lastOrDefault != null)
            {
                var t2 = lastOrDefault.Time;
                var r = t1 - t2;
                return r;
            }
            return TimeSpan.Zero;
        }
        /// <summary>
        /// Возвращает описание для  трека за текущий день
        /// </summary>
        /// <param name="segmentstorage"></param>
        /// <returns></returns>
        private static RouteHeader ToTrackRouteDefinision(this SegmentBase segmentstorage)
        {
            if (segmentstorage is null)
                throw new ArgumentNullException(nameof(segmentstorage));
            var header = segmentstorage.Keys.ToList().FirstOrDefault();
            {
                var lastOrDefault = segmentstorage.Keys.ToList().LastOrDefault();
                {
                    var result = new RouteHeader
                    {
                        Distance = segmentstorage.ToRouteDistance(),
                        Duration = segmentstorage.ToRouteDuration(),
                        TrackDate = segmentstorage.Keys.ToTrackDateVales().Key,
                        Start = header.Start,
                        Stop = lastOrDefault.Stop,
                        Guid = Guid.NewGuid()
                    };

                    return result;
                }
            }

        }

        /// <summary>
        /// Возвращает начало и окончание полученного хранилища трека
        /// </summary>
        private static KeyValuePair<DateTime, DateTime> ToTrackDateVales(this IEnumerable<RouteHeader> headers)
        {
            if (headers is null) throw new NullReferenceException("headers");
            var q = headers.OrderBy(x => x.TrackDate);
            var list = q.Select(routeHeader => routeHeader.TrackDate).OrderBy(x => x);
            return new KeyValuePair<DateTime, DateTime>(list.FirstOrDefault(), list.LastOrDefault());
        }

        private static TimeSpan ToNullebleTimeSpan(this TimeSpan value)
        {
            return value;
        }

        /// <summary>
        /// Возвращает  длительность трека за текущий день
        /// </summary>
        /// <param name="segmentstorage"></param>
        /// <returns></returns>
        private static TimeSpan ToRouteDuration(this SegmentBase segmentstorage)
        {
            if (segmentstorage is null)
                throw new ArgumentNullException(nameof(segmentstorage));
            return segmentstorage.Where(item => !(item.Key is null)).Aggregate(TimeSpan.Zero,
                (current, item) => current + item.Key.Duration.ToNullebleTimeSpan());
        }


        /// <summary>
        /// Возвращает  дистанцию трека за текущий день
        /// </summary>
        /// <param name="segmentstorage"></param>
        /// <returns></returns>
        private static double ToRouteDistance(this IEnumerable<KeyValuePair<RouteHeader, IEnumerable<ITrackPoint>>> segmentstorage)
        {
            if (segmentstorage is null)
                throw new ArgumentNullException(nameof(segmentstorage));
            return segmentstorage.Where(item => !(item.Key is null)).Aggregate(0.0,
                (current, item) => current + item.Key.Distance.ToNullebleDouble());
        }
        private static double ToNullebleDouble(this double value)
        {
            return value;
        }

        /// <summary>
        /// Фильтр данных по маршруту автотранспорта
        /// </summary>
        /// <param name="sd"></param>
        /// <returns></returns>
        public static IEnumerable<Track> ToFilteredRoutes(this Dictionary<DateTime, List<List<ITrackPoint>>> sd)
        {
            #region
            var routeStorage = new List<Track>();
            var dateList = sd.Keys;
            foreach (var dateRoute in dateList)
            {
                var t = sd.FirstOrDefault(x => x.Key.ToShortDateString().Equals(dateRoute.ToShortDateString()));
                var trackstorage = new Track();
                if (t.Value != null)
                {
                    var segmentStorage = new SegmentBaseRouteStorage();
                    foreach (var segment in t.Value)
                    {
                        var dates = segment.ToStartStopSegment();
                        var header = new RouteHeader
                        {
                            Distance = segment.ToSegmentDistance(),
                            Duration = segment.ToRouteToSegmentDuration(),
                            Guid = Guid.NewGuid(),
                            TrackDate = dateRoute,
                            Stop = dates.Value,
                            Start = dates.Key
                        };
                        segmentStorage.Add(header, segment);
                    }
                    var trackHeader = segmentStorage.ToTrackRouteDefinision();
                    trackstorage.Add(trackHeader, segmentStorage);
                }

                var filterStorage = new Track();
                foreach (var item in trackstorage)
                {
                    var value = item.Value;
                    var filteredSegments = new SegmentBaseRouteStorage();
                    foreach (var segment in value)
                    {
                        segment.WithValue(x => x.Value.Do(val =>
                          {
                              val.ToFilteredSegment().Do(fs =>
                              {
                                  var distance = fs.ToSegmentDistance();
                                  var duration = fs.ToRouteToSegmentDuration();
                                  var times = fs.ToStartStopSegment();
                                  segment.Key.Start = times.Key;
                                  segment.Key.Stop = times.Value;
                                  segment.Key.Distance = distance;
                                  segment.Key.Duration = duration;
                                  filteredSegments.Add(segment.Key, fs);
                              });
                          }));
                    }
                    var def = filteredSegments.ToTrackRouteDefinision();
                    filterStorage.Add(def, filteredSegments);
                }
                routeStorage.Add(filterStorage);
            }
            #endregion
            return routeStorage;
        }

        /// <summary>
        /// Возвращает набор дат
        /// </summary>
        /// <param name="segments">Сегменты треков</param>
        /// <returns>Список дат треков</returns>
        private static IEnumerable<DateTime> ToDateList(this IEnumerable<List<ITrackPoint>> segments)
        {
            var points = new List<ITrackPoint>();
            foreach (var segment in segments)
            {
                points.AddRange(segment);
            }
            var q = (from items in points select items.Time.ToBeginDay()).Distinct();
            return q.ToList();
        }

        /// <summary>
        /// Возвращает соловарь сегментов треков по датаи
        /// </summary>
        /// <param name="segments">Отфильтрованные сегменты трека</param>
        /// <returns>Словарь сегментов треков по датам</returns>
        public static Dictionary<DateTime, List<List<ITrackPoint>>> ToSegmentsDictionary(this IEnumerable<List<ITrackPoint>> segments)
        {
            var result =
                new Dictionary<DateTime, List<List<ITrackPoint>>>();
            // ReSharper disable once PossibleMultipleEnumeration
            var dates = segments.ToDateList();
            //Для каждой даты в полученных данных
            foreach (var dateTime in dates)
            {
                //новый трек
                var time1 = dateTime;
                // ReSharper disable once PossibleMultipleEnumeration
                var segmetpoint = (from segment in segments
                                   let time = time1
                                   select segment.Where(trackPoint => CheckPointInDate(time, trackPoint)).ToList()).ToList();
                //для каждого сегмента треков
                result.Add(dateTime, segmetpoint);
            }
            return result;
        }


        /// <summary>
        /// Возвращает результат компаратора точки трека в дате
        /// </summary>
        private static readonly Func<DateTime, ITrackPoint, bool> CheckPointInDate = (date, point) =>
        {
            var date1 =
                date.ToBeginDay();
            var date2 =
                point.Time.ToBeginDay();
            return date1.Equals(date2);
        };

        /// <summary>
        /// Время на начало дня(гавнокод!!!!)
        /// </summary>
        /// <returns>The begin day.</returns>
        /// <param name="time">Time.</param>
        private static DateTime ToBeginDay(this DateTime time)
        {
            return
                time.AddHours(-time.Hour)
                    .AddMinutes(-time.Minute)
                    .AddSeconds(-time.Second)
                    .AddMilliseconds(-time.Millisecond);
        }
    }

}