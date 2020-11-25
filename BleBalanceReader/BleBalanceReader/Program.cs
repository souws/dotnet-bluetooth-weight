using System;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System.Linq;
using System.Threading;

namespace BleBalanceReader
{
    class Program
    {

        static void Main(string[] args)
        {
            BluetoothDeviceInfo foundD = null;

            BluetoothClient client = new BluetoothClient();
            if (foundD == null)
            {
                foundD = client.DiscoverDevices().First(x => x.DeviceName.StartsWith("JD"));
            }

            client.Connect(foundD.DeviceAddress, BluetoothService.SerialPort);
            Console.WriteLine("Connect");
            while (true)
            {
                byte[] bytes = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    int b = client.GetStream().ReadByte();
                    //Console.WriteLine(b);
                    bytes[i] = (byte)b;

                }
                Console.WriteLine(String.Join("", bytes.Reverse().Select(x => (char)x).ToArray()));
                //client.Close();
                client.GetStream().Flush();
                Thread.Sleep(1000);
            }
            Console.WriteLine("Hello World!");
        }
    }
}
