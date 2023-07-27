using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using NineDigit.SerialPort.Platforms.Linux;
using NineDigit.SerialPort.Platforms.OSX;
using NineDigit.SerialPort.Platforms.Windows;

namespace NineDigit.SerialPort
{
    public sealed class SerialPortManager
    {
        private readonly ISerialPortDeviceScanner serialPortDeviceScanner;

        public SerialPortManager()
        {
            this.serialPortDeviceScanner = GetSerialPortDeviceScanner();
        }

        public Task<IReadOnlyCollection<ISerialPortDevice>> GetSerialPortDevicesAsync(
            CancellationToken cancellationToken = default)
            => this.serialPortDeviceScanner.GetAllAsync(cancellationToken);

        private static ISerialPortDeviceScanner GetSerialPortDeviceScanner()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return WindowsSerialPortDeviceScanner.Instance;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return MacSerialPortDeviceScanner.Instance;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return LinuxSerialPortDeviceScanner.Instance;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}