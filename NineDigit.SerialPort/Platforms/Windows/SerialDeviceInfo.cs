using System.Runtime.Versioning;

namespace NineDigit.SerialPort.Platforms.Windows
{
    [SupportedOSPlatform("windows")]
    public class SerialDeviceInfo
    {
        public SerialDeviceInfo(string portName, string description, string busDescription)
        {
            this.PortName = portName;
            this.Description = description;
            this.BusDescription = busDescription;
        }

        public string PortName { get; }
        public string Description { get; }
        public string BusDescription { get; }
    }
}