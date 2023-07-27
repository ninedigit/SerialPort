using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NineDigit.SerialPort.Platforms.Linux
{
    public sealed class LinuxSerialPortDeviceScanner : ISerialPortDeviceScanner
    {
        internal static readonly LinuxSerialPortDeviceScanner Instance = new LinuxSerialPortDeviceScanner();

        public Task<IReadOnlyCollection<ISerialPortDevice>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            // TODO
            throw new NotSupportedException();
        }
    }
}