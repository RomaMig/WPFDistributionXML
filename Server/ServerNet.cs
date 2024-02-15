using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Server.MVVM.Model;
using System.IO;
using TcpIO;
using System.Runtime.InteropServices.ComTypes;

namespace Server
{
    internal class ServerNet
    {
        private bool isRun;
        private TcpListener server;
        private List<TcpClient> clients;
        private DataModel data;
        private PacketReader packetReader;
        private PacketBuilder packetBuilder;

        public event Action serverStarted;
        public event Action serverStoped;
        public event Action<TcpClient> clientConnected;
        public event Action<TcpClient> clientDisconnected;

        public ServerNet(DataModel data)
        {
            isRun = false;
            clients = new List<TcpClient>();
            packetBuilder = new PacketBuilder();
            packetReader = new PacketReader();
            this.data = data;
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
                    var code = (Code)await packetReader.ReadByte(stream);
                    if (!client.Connected)
                    {
                        isConnected = false;
                        break;
                    }
                    switch (code)
                    {
                        case Code.Disconnect:
                            isConnected = false;
                            break;
                        case Code.Message_Request:
                            var msg = await packetReader.ReadMessage(stream);
                            break;
                        case Code.Repeat_Request:
                            DistributeInfo(client);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (client.Connected)
                    packetBuilder.ResponseCode(client.GetStream(), (byte)Code.Disconnect);
                clientDisconnected(client);
                clients.Remove(client);
                client.Close();
            }
        }

        private void DistributeInfo(TcpClient client)
        {
            if (client != null && client.Connected && data.isFilled)
            {
                var stream = client.GetStream();
                packetBuilder.ResponseCode(stream, (byte)Code.Data_Request);
                packetBuilder.ResponseData(stream,
                    (BinaryWriter bw) =>
                    {
                        bw.Write(data.From);
                        bw.Write(data.HexTextColor);
                        bw.Write(data.Text);
                        bw.Write(data.BinaryImage);
                    });
            }
        }

        public void BroadcastInfo()
        {
            foreach (var client in clients)
            {
                DistributeInfo(client);
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
                    packetBuilder.ResponseCode(c?.GetStream(), (byte)Code.Disconnect);
                    c?.Close();
                });
                clients.Clear();
                server.Stop();

                serverStoped();
            }
        }
    }
}
