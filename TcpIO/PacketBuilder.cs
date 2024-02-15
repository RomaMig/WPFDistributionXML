using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace TcpIO
{
    public class PacketBuilder
    {
        public async void ResponseCode(NetworkStream stream, byte opcode)
        {
            await Task.Run(() =>
            {
                try
                {
                    stream.WriteByte(opcode);
                }
                catch (Exception ex) { }
            });
        }

        public async void ResponseMessage(NetworkStream stream, string msg)
        {
            try
            {
                var data = Encoding.UTF8.GetBytes(msg);
                await stream.WriteAsync(data, 0, data.Length);
            }
            catch (Exception ex) { }
        }

        public async void ResponseData(NetworkStream stream, Action<BinaryWriter> writer)
        {
            await Task.Run(() =>
            {
                try
                {
                    var binaryWriter = new BinaryWriter(stream);
                    writer(binaryWriter);
                    binaryWriter.Flush();
                }
                catch (Exception ex) { }
            });
        }
    }
}
