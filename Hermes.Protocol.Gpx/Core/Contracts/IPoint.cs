using System;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public interface IPoint
    {

        IGeoPoint Position { get; set; }
        /// <summary>
        /// Время фиксации измерений
        /// </summary>
        /// <value>The time.</value>
        DateTime Time { get; set; }
        /// <summary>
        /// Высота над уровнем моря
        /// </summary>
        /// <value>The ele.</value>
        double Ele { get; set; }
        /// <summary>
        /// Имя точки
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        double Speed { get; set; }
    }
}