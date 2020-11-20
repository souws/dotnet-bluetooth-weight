using System;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System.Linq;
namespace BleBalanceReader
{
    class Program
    {

        static void Main(string[] args)
        {
            BluetoothClient client = new BluetoothClient();
            var devices = client.DiscoverDevices();
            foreach (BluetoothDeviceInfo dvc in devices)
            {

                Console.WriteLine(dvc.DeviceName);
                if (dvc.DeviceName == "JDY-33-SPP")
                {
                    client.Connect(dvc.DeviceAddress, BluetoothService.SerialPort);
                    Console.WriteLine("Connect");
                    byte[] bytes = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        int b = client.GetStream().ReadByte();
                        Console.WriteLine(b);
                        bytes[i] = (byte)b;
                    }
                    Console.WriteLine(String.Join("", bytes.Reverse().Select(x => (char)x).ToArray()));
                }
            }
            Console.WriteLine("Hello World!");
        }
    }
}
