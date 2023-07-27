using NineDigit.SerialPort;

var serialPortManager = new SerialPortManager();
var devices = await serialPortManager.GetSerialPortDevicesAsync();

foreach (var device in devices)
{
    Console.WriteLine($"Product: {device.ProductName} ({device.ProductId})");
    Console.WriteLine($"Vendor: {device.VendorName} ({device.VendorId})");
}