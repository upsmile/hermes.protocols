using System;
using Hermes.Protocol.Gpx.Core.Contracts;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    /// <summary>
    /// Точка доставки на карте
    /// </summary>
    public interface IDeliveryPoint
    {
        /// <summary>
        /// Контракт привязанный к этой точке
        /// </summary>
        object ContractItem { get; set; }
        /// <summary>
        /// Связанный элемент данных  Distributor
        /// </summary>
        /// <value>The data item.</value>
        object DataItem { get; set; }
        /// <summary>
        /// Код
        /// </summary>
        /// <value>The code.</value>
        double Code { get; set; }
        /// <summary>
        /// Координаты точки на карте
        /// </summary>
        /// <value>The position.</value>
        IPosition Position { get; set; }
        /// <summary>
        /// Время входа
        /// </summary>
        DateTime TimeIn { get; set; }
        /// <summary>
        /// Время выхода
        /// </summary>
        DateTime TimeOut { get; set; }
    }
    
    
    public sealed class DeliveryPoint:IDeliveryPoint{
        public object ContractItem { get; set; }
        public object DataItem { get; set; }
        public double Code { get; set; }
        public IPosition Position { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
    }
}