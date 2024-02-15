using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client
{
    class ClientNet
    {
        private TcpClient client;
        public bool Connected { get { return client.Connected; } }
        public event Action<string> msgRequest;
        public event Action connecting;
        public event Action connected;
        public event Action disconnected;
        public event Action connectionAttemptCompleted; 

        public ClientNet()
        {
            client = new TcpClient();
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
                while (client.Connected)
                {
                    var msg = await Request();
                    msgRequest(msg);
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

        private async Task<string> Request()
        {
            if (!Connected)
                return null;

            var stream = client.GetStream();
            var buffer = new byte[256];
            var data = new StringBuilder();
            do
            {
                // добавляем в буфер
                var size = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (size == 0)
                    throw new SocketException(0);
                var text = Encoding.UTF8.GetString(buffer, 0, size);
                data.Append(text);
            }
            while (stream.DataAvailable);
            return data.ToString();
        }

        private async void Response(string msg)
        {
            if (!Connected)
                return;

            var stream = client.GetStream();
            var data = Encoding.UTF8.GetBytes(msg);
            await stream.WriteAsync(data, 0, data.Length);
        }

        public void RepeatRequest()
        {
            Response("repeat request");
        }

        public void Close()
        {
            if (Connected)
            {
                disconnected();
                client.Close();
                client.Dispose();
            }
        }
    }
}
