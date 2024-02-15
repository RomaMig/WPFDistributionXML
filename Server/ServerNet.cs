using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;

namespace Server
{
    internal class ServerNet
    {
        private bool isRun;
        private TcpListener server;
        private List<TcpClient> clients;

        public event Action serverStarted;
        public event Action serverStoped;
        public event Action<TcpClient> clientConnected;
        public event Action<TcpClient> clientDisconnected;

        public ServerNet()
        {
            isRun = false;
        }


        public void SwitchMode(string ip, int port)
        {
            if (!isRun)
            {
                try
                {
                    var _ip = IPAddress.Parse(ip);

                    clients = new List<TcpClient>();
                    server = new TcpListener(_ip, port);
                    server.Start();

                    isRun = true;
                    serverStarted();

                    Listen();
                }
                catch (FormatException ex)
                {

                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                Close();
            }
        }

        private async void Listen()
        {
            while (isRun)
            {
                try
                {
                    var client = await server.AcceptTcpClientAsync();
                    clientConnected(client);
                    clients.Add(client);
                    Process(client);
                }
                catch (ObjectDisposedException ex)
                {

                }
            }
        }

        private async void Process(TcpClient client)
        {
            try
            {
                var stream = client.GetStream();
                bool isConnected = true;
                while (isConnected)
                {
                    var msg = await Request(stream);

                    switch (msg)
                    {
                        case "repeat request":
                            Response(stream, DateTime.Now.ToLongTimeString());
                            break;
                        case "":
                            isConnected = false; 
                            break;
                    }
                }
            }
            catch (Exception ex) //Если клиент отключился
            {
            }
            finally
            {
                clientDisconnected(client);
                clients.Remove(client);
                client.Close();
            }
        }

        private async Task<string> Request(NetworkStream stream)
        {
            var buffer = new byte[256];
            var data = new StringBuilder();
            // считываем данные до конечного символа
            do
            {
                // добавляем в буфер
                var size = await stream.ReadAsync(buffer, 0, buffer.Length);
                var text = Encoding.UTF8.GetString(buffer, 0, size);
                data.Append(text);
            }
            while (stream.DataAvailable);
            return data.ToString();
        }

        private async void Response(NetworkStream stream, string msg)
        {
            var data = Encoding.UTF8.GetBytes(msg);
            try
            {
                await stream.WriteAsync(data, 0, data.Length);
            }
            catch (Exception ex)
            {

            }
        }

        public void DistributionInfo()
        {
            foreach (var client in clients)
            {
                if (client != null && client.Connected)
                {
                    Response(client.GetStream(), DateTime.Now.ToLongTimeString());
                }
            }
        }

        public void Close()
        {
            if (isRun)
            {
                isRun = false;
                clients.ForEach(c =>
                {
                    clientDisconnected(c);
                    c?.Close();
                });
                clients.Clear();
                server.Stop();

                serverStoped();
            }
        }
    }
}
