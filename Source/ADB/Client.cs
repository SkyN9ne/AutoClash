using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace ADB
{
    class Client
    {
        /// <summary>
        /// Flag indicating if the client is connected to ADB.
        /// </summary>
        bool connected;
        public bool Connected { get; set; }

        /// <summary>
        /// Flag indicating if the last command was successful.
        /// </summary>
        bool success;
        public bool Success { get; set; }

        /// <summary>
        /// Socket for ADB connection.
        /// </summary>
        private StreamSocket socket;
        public StreamSocket Socket { get; set; }


        /// <summary>
        /// Initializes an instance of the ADB client.
        /// </summary>
        public Client()
        {
            connected = false;
            socket = new StreamSocket();
        }

        /// <summary>
        /// Initializes client connection and starts ADB server if necessary.
        /// </summary>
        public async Task<bool> Connect()
        {
            if (!connected)
            {
                try
                {
                    HostName host = new HostName("localhost");
                    await socket.ConnectAsync(host, "5037");
                    connected = true;
                    return true;
                }
                catch
                {
                    // TODO: start ADB server and retry connection;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Closes client connection.
        /// </summary>
        public void Close()
        {
            socket.Dispose();
            socket = null;
        }



        /// <summary>
        /// Sends a command to the ADB server.
        /// </summary>
        /// <param name="command">ADB command string.</param>
        /// <returns>Result string from ADB command.</returns>
        public async Task<string> Command(string command)
        {
            Task.Delay(1000).Wait();

            if (connected)
            {
                try
                {
                    // Format the command for ADB server.
                    string sendData = string.Format("{0}{1}\n", command.Length.ToString("X4"), command);
 
                    // Send the command.
                    DataWriter writer = new DataWriter(socket.OutputStream);
                    writer.WriteString(sendData);
                    await writer.StoreAsync();

                    // Create the data reader.
                    DataReader reader = new DataReader(socket.InputStream);
                    reader.InputStreamOptions = InputStreamOptions.Partial;
                    uint bytesRead = await reader.LoadAsync(1024 * 20);


                    if(bytesRead > 3)
                    {
                        // Get the status response ('OKAY' or 'FAIL').
                        String status = reader.ReadString(4);
                        bytesRead -= 4;

                        if (status == "OKAY")
                        {
                            success = true;
                        }
                        else
                        {
                            success = false;
                        }

                        if (bytesRead > 3)
                        {
                            // todo read propper X4 and convert to length
                            String length = reader.ReadString(4);
                            bytesRead -= 4;
                        }

                    }
                    else
                    {
                        success = false;
                    }

                    // Return the result string.
                    return reader.ReadString(bytesRead);
                }
                catch
                {
                    // TODO: Log error;

                    return "";
                }
            }
            else
            {
                return "";
            }
        }


    }
}
