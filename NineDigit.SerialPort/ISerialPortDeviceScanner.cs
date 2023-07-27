using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NineDigit.SerialPort
{
    public interface ISerialPortDeviceScanner
    {
        public Task<IReadOnlyCollection<ISerialPortDevice>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}