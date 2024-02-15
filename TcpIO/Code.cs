using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpIO
{
    public enum Code
    {
        Disconnect = 15,
        Connected = 0,
        Message_Request = 1,
        Data_Request = 2,
        Repeat_Request = 3
    }
}
