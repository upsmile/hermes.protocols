using System;

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
        IGeoPoint Position { get; set; }
        /// <summary>
        /// Время входа
        /// </summary>
        DateTime TimeIn { get; set; }
        /// <summary>
        /// Время выхода
        /// </summary>
        DateTime TimeOut { get; set; }
    }
}