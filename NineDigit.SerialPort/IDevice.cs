namespace NineDigit.SerialPort
{
    public interface IDevice
    {
        public string VendorName { get; }
        public int VendorId { get; }
        public string ProductName { get; }
        public int ProductId { get; }
    }
}