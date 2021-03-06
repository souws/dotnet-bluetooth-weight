﻿using System;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System.Linq;
using System.Threading;
using System.Net.Sockets;

namespace BleBalanceReader
{
    class Program
    {

        static void Main(string[] args)
        {
            BluetoothClient client = new BluetoothClient();

            BluetoothDeviceInfo scale = client.DiscoverDevices().FirstOrDefault(x => x.DeviceName.StartsWith("JD"));
            if (scale == null)
            {
                Console.WriteLine("找不到JD开头的蓝牙设备！");
                Console.ReadKey();
                return;
            }

            try
            {
                client.Connect(scale.DeviceAddress, BluetoothService.SerialPort);
                Console.WriteLine("已连接");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            NetworkStream stream = client.GetStream();
            string weight = "";
            Console.WriteLine($"client.Connected={client.Connected}");
            Console.WriteLine($"stream != null={stream != null}");
            Console.WriteLine($"stream.DataAvailable={stream.DataAvailable}");
            //while (client.Connected
            //    && stream != null
            //    && stream.DataAvailable)
            for (int readCount = 0; readCount < 200; readCount++)//最多读200次重量
            {
                while (stream.ReadByte() != 61) ;//skip until ascii =
                byte[] bytes = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    int b = client.GetStream().ReadByte();
                    //Console.WriteLine(b);
                    bytes[i] = (byte)b;
                }

                string newWeight = String.Join("", bytes.Reverse().Select(x => (char)x).ToArray());
                if (newWeight != weight)
                {
                    weight = newWeight;
                    Console.Write($"{weight}\t");
                }
            }
            Console.WriteLine("end of main!");
        }
    }
}
