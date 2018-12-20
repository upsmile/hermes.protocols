namespace Hermes.Protocol.Gpx.Core.Contracts
{
    /// <summary>
    /// Конфигурационный файл обновления данных todo: кандидат на удаления из кода            
    /// </summary>
    public interface IParceGpxDataConfig
    {
        /// <summary>
        /// Угол сглаживания
        /// </summary>
        double Angel { get; set; }
        /// <summary>
        /// Скорость сглаживания
        /// </summary>
        double Speed { get; set; }
    }
}