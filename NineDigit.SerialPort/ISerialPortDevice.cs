namespace NineDigit.SerialPort
{
    public interface ISerialPortDevice : IDevice
    {
        public string SerialPortName { get; }
    }
}