using System;

namespace NineDigit.SerialPort
{
    public sealed class SerialPortDevice : ISerialPortDevice, IEquatable<SerialPortDevice?>
    {
        public SerialPortDevice(SerialPortDevice device)
        {
            if (device is null)
                throw new ArgumentNullException(nameof(device));

            this.VendorName = device.VendorName;
            this.VendorId = device.VendorId;
            this.ProductName = device.ProductName;
            this.ProductId = device.ProductId;
            this.SerialPortName = device.SerialPortName;
        }

        public SerialPortDevice(string vendorName, int vendorId, string productName, int productId,
            string serialPortName)
        {
            this.VendorName = vendorName;
            this.VendorId = vendorId;
            this.ProductName = productName;
            this.ProductId = productId;
            this.SerialPortName = serialPortName;
        }

        public string VendorName { get; }
        public int VendorId { get; }
        public string ProductName { get; }
        public int ProductId { get; }
        public string SerialPortName { get; }

        // TODO: Exclude VendorName and ProductName?
        public override int GetHashCode()
        {
            var hashCodeBuilder = new HashCode();

            hashCodeBuilder.Add(this.VendorName, StringComparer.Ordinal);
            hashCodeBuilder.Add(this.VendorId);
            hashCodeBuilder.Add(this.ProductName, StringComparer.Ordinal);
            hashCodeBuilder.Add(this.ProductId);
            hashCodeBuilder.Add(this.SerialPortName, StringComparer.Ordinal);

            return hashCodeBuilder.ToHashCode();
        }

        public override bool Equals(object? obj)
            => this.Equals(obj as SerialPortDevice);

        public bool Equals(SerialPortDevice? other)
            => Equals(this, other);

        // TODO: Exclude VendorName and ProductName comparision?
        public static bool Equals(SerialPortDevice? left, SerialPortDevice? right)
        {
            if (ReferenceEquals(left, right))
                return true;

            if (left is null || right is null)
                return false;

            return StringComparer.Ordinal.Equals(left.VendorName, right.VendorName)
                   && left.VendorId == right.VendorId
                   && StringComparer.Ordinal.Equals(left.ProductName, right.ProductName)
                   && left.ProductId == right.ProductId
                   && StringComparer.Ordinal.Equals(left.SerialPortName, right.SerialPortName);
        }
    }
}