using System;
using System.Xml.Linq;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public interface IUploadDataParams
    {
        /// <summary>
        /// Идентификатор автомобиля
        /// </summary>
        double CarId { get; set; }
            
        /// <summary>
        /// Дата события
        /// </summary>
        DateTime Date { get; set; }

        /// <summary>
        /// Признак отфильтрованного файла
        /// </summary>
        bool IsFiltered { get; set; }

        /// <summary>
        /// Тип автотранспорта
        /// </summary>
        int CarType { get; set; }

        XDocument UploadData { get; set; }
    }
}