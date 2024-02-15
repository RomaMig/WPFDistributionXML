using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TcpIO;
using System.IO;
using System.Windows.Markup;
using Client.MVVM.Model;

namespace Client
{
    class ClientNet
    {
        private TcpClient client;
        private PacketReader packetReader;
        private PacketBuilder packetBuilder;
        private DataModel data;

        public bool Connected { get { return client.Connected; } }
        public event Action<string> msgRequest;
        public event Action connecting;
        public event Action connected;
        public event Action disconnected;
        public event Action connectionAttemptCompleted;

        public ClientNet(DataModel data)
        {
            client = new TcpClient();
            packetBuilder = new PacketBuilder();
            packetReader = new PacketReader();
            this.data = data;
        }

        public void SwitchConnection(string ip, int port)
        {
            if (!client.Connected)
            {
                connecting();
                Connect(ip, port);
            }
            else
            {
                Close();
            }
        }

        private async void Connect(string ip, int port)
        {
            try
            {
                var _ip = IPAddress.Parse(ip);

                client = new TcpClient();
                await client.ConnectAsync(_ip, port);
                connected();
                Process();
            }
            catch (Exception ex)
            {
                if (ex is SocketException || ex is FormatException)
                {

                }
                Close();
            }
            finally
            {
                connectionAttemptCompleted();
            }

        }

        private async void Process()
        {
            try
            {
                while (client != null && client.Connected)
                {
                    var stream = client.GetStream();
                    var code = (Code)await packetReader.ReadByte(stream);
                    if (code <= 0 || !client.Connected)
                    {
                        break;
                    }
                    switch (code)
                    {
                        case Code.Disconnect:
                            Close();
                            break;
                        case Code.Message_Request:
                            var msg = await packetReader.ReadMessage(stream);
                            break;
                        case Code.Data_Request:
                            try
                            {
                                var tmp = await packetReader.ReadData(stream, client,
                                (BinaryReader br, TcpClient cl) =>
                                {
                                    DataModel data = new DataModel();
                                    data.From = br.ReadString();
                                    data.HexTextColor = br.ReadString();
                                    data.Text = br.ReadString();
                                    data.BinaryImage = br.ReadString();
                                    return data;
                                });

                                data.Date = DateTime.Now;
                                data.From = tmp.From;
                                data.HexTextColor = tmp.HexTextColor;
                                data.Text = tmp.Text;
                                data.BinaryImage = tmp.BinaryImage;
                            } catch { }
                            break;
                    }
                }
            }
            catch (SocketException ex)
            {

            }
            catch (Exception ex)
            {

            }
            finally
            {
                Close();
            }
        }


        public void RepeatRequest()
        {
            if (!Connected)
                return;

            packetBuilder.ResponseCode(client.GetStream(), (byte)Code.Repeat_Request);
        }

        public void Close()
        {
            if (Connected)
            {
                packetBuilder.ResponseCode(client.GetStream(), (byte)Code.Disconnect);
                client.Close();
                client.Dispose();
            }
            disconnected();
        }
    }
}
