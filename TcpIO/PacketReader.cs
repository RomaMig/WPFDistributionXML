using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpIO
{
    public class PacketReader
    {
        public async Task<int> ReadByte(NetworkStream stream)
        {
            int code = -1;
            var buffer = new byte[1];
            var size = await stream.ReadAsync(buffer, 0, 1);
            if (size <= 0 && !stream.DataAvailable)
                throw new Exception();
            code = buffer[0];
            return code;
        }

        public async Task<string> ReadMessage(NetworkStream stream)
        {
            var buffer = new byte[256];
            var data = new StringBuilder();
            do
            {
                var size = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (size == 0)
                    throw new SocketException(0);
                var text = Encoding.UTF8.GetString(buffer, 0, size);
                data.Append(text);
            }
            while (stream.DataAvailable);
            return data.ToString();
        }

        public async Task<T> ReadData<T>(NetworkStream stream, TcpClient cl, Func<BinaryReader, TcpClient, T> reader)
        {
            var binaryReader = new BinaryReader(stream);
            T data = await Task.Run(() => reader(binaryReader, cl));
            return data;
        }
    }
}
