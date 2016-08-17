using ADB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADB
{
    public static class Devices
    {
        static Client client = new Client();

        public static async Task<List<Device>> List()
        {
            List<Device> devices = new List<Device>();

            if(await client.Connect())
            {
                string result = await client.Command("host:devices-l");
                client.Close();

                string[] lines = result.Split('\n');

                foreach (string line in lines)
                {
                    int pos = line.IndexOf(' ');

                    if (pos > 0)
                    {
                        string name = "";
                        string serial = line.Substring(0, pos);
                        string description = line.Substring(pos).TrimStart();
                        devices.Add(new Device(name, serial, description));
                    }
                    //else
                    //{
                    //    string name = "";
                    //    string serial = line;
                    //    string description = "";
                    //    devices.Add(new Device(name, serial, description));
                    //}

                }


                // Build the device list.
                return devices;
            }
            else
            {
                // Unable to get a list of devices.
                return null;
            }


        }
    }

    public class Device
    {
        Client client = new Client();

        private string name;
        private string serial;
        private string description;


        public Device(string Name, string Serial, string Description)
        {
            name = Name;
            serial = Serial;
            description = Description;
        }

        public async Task StartClash()
        {
            //await client.Connect();
            //string result2 = await client.Command("host:transport:127.0.0.1:21633"); // MEmu_13
            //string result2 = await client.Command("host:transport:127.0.0.1:62001"); // Nox
            //string result2 = await client.Command("host:transport:d230ab45"); // Galaxy s7
            //string result3 = await client.Command("shell:dumpsys account");

            if (await client.Connect())
            {
                string result = await client.Command(string.Format("host:transport:{0}", serial));
                string result2 = await client.Command("shell:am start -W com.supercell.clashofclans/.GameApp");
                client.Close();
            }
        }
    }

}
