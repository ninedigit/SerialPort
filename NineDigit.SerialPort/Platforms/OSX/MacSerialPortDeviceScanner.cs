using System.Collections.Generic;
using NineDigit.Mac.IOKit;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace NineDigit.SerialPort.Platforms.OSX
{
    [SupportedOSPlatform("macos")]
    public sealed class MacSerialPortDeviceScanner : ISerialPortDeviceScanner
    {
        internal static readonly MacSerialPortDeviceScanner Instance = new();

        public IReadOnlyCollection<ISerialPortDevice> GetAll()
        {
            var devices = NineDigit.Mac.IOKit.SerialPort.GetUSBCommunicationDevices();
            var result = new List<SerialPortDevice>(devices.Count);

            foreach (var device in devices)
            {
                var serialPortDevice = CreateSerialPortDevice(device);
                result.Add(serialPortDevice);
            }

            return result;
        }

        private SerialPortDevice CreateSerialPortDevice(USBCommunicationDevice device)
            => new(device.VendorString, device.VendorID, device.ProductString, device.ProductID, device.ComName);

        Task<IReadOnlyCollection<ISerialPortDevice>> ISerialPortDeviceScanner.GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var devices = this.GetAll();
            var result = Task.FromResult(devices);

            return result;
        }
    }
}