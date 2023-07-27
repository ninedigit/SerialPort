using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace NineDigit.SerialPort.Platforms.Windows
{
    [SupportedOSPlatform("windows")]
    public sealed class WindowsSerialPortDeviceScanner : ISerialPortDeviceScanner
    {
        internal static readonly WindowsSerialPortDeviceScanner Instance = new WindowsSerialPortDeviceScanner();

        private const string VIDPattern = @"VID_([0-9A-F]{4})";
        private const string PIDPattern = @"PID_([0-9A-F]{4})";

        public IReadOnlyCollection<ISerialPortDevice> GetAll()
        {
            using var searcher = new ManagementObjectSearcher("root\\CIMV2",
                "SELECT * FROM Win32_PnPEntity WHERE ClassGuid=\"{4d36e978-e325-11ce-bfc1-08002be10318}\"");
            var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
            var result = new List<ISerialPortDevice>();

            foreach (var port in ports)
            {
                var device = CreateSerialPortDeviceOrNull(port);

                if (device is null)
                    continue;

                result.Add(device);
            }

            return result;
        }

        private object? GetDeviceProperty(ManagementObject mo, string key)
        {
            // call Win32_PnPEntity's 'GetDeviceProperties' method
            // prepare two arguments:
            //  1st one is an array of string (or null)
            //  2nd one will be filled on return (it's an array of ManagementBaseObject)

            var args = new object?[] { new string[] { key }, null };
            mo.InvokeMethod("GetDeviceProperties", args);

            // one mbo for each device property key
            if (args[1] is ManagementBaseObject[] o && o.Length > 0)
            {
                // get value of property named "Data"
                // not all objects have that so we enum all props here
                var data = o[0].Properties.OfType<PropertyData>().FirstOrDefault(p => p.Name == "Data");
                if (data != null)
                    return data.Value;
            }

            return null;
        }

        private SerialPortDevice? CreateSerialPortDeviceOrNull(ManagementBaseObject p)
        {
            var vendorName = p.GetPropertyValue("Manufacturer").ToString();
            var name = p.GetPropertyValue("Name")
                .ToString(); // Parsing Serial Port name can also be made using 'Caption'
            var pnpDeviceId = p.GetPropertyValue("PNPDeviceID").ToString();

            int? vendorId = null;
            int? productId = null;
            string? serialPortName = null;

            if (name != null)
            {
                var comPortMatch = Regex.Match(name, "\\((COM[0-9]{1,2})\\)$");
                if (comPortMatch.Success)
                    serialPortName = comPortMatch.Groups[1].Value;
            }

            if (pnpDeviceId != null)
            {
                var vendorIdMatch = Regex.Match(pnpDeviceId, VIDPattern, RegexOptions.IgnoreCase);
                if (vendorIdMatch.Success)
                    vendorId = HexStringToInt32(vendorIdMatch.Groups[1].Value);

                var productIdMatch = Regex.Match(pnpDeviceId, PIDPattern, RegexOptions.IgnoreCase);
                if (productIdMatch.Success)
                    productId = HexStringToInt32(productIdMatch.Groups[1].Value);
            }

            var productName =
                (string?)this.GetDeviceProperty((ManagementObject)p, "DEVPKEY_Device_BusReportedDeviceDesc");

            if (vendorName != null && vendorId.HasValue && productName != null && productId.HasValue &&
                serialPortName != null)
            {
                var device = new SerialPortDevice(vendorName, vendorId.Value, productName, productId.Value,
                    serialPortName);
                return device;
            }

            return null;
        }

        private static int HexStringToInt32(string value)
            => Convert.ToInt32(value, 16);

        Task<IReadOnlyCollection<ISerialPortDevice>> ISerialPortDeviceScanner.GetAllAsync(
            CancellationToken cancellationToken = default)
            => Task.FromResult(this.GetAll());
    }
}