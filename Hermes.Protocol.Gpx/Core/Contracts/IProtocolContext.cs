using System.IO;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public interface IProtocolContext
    {
        string Context { get; set; }
        
        Stream FileByteStream { get; set; }
    }
}