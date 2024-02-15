using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Server.MVVM.Model
{
    class ServerModel : Presenter
    {
        private string date;
        private string ip;
        private int port;

        public string Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }
        public string Ip
        {
            get { return ip; }
            set
            {
                ip = value;
                OnPropertyChanged("Ip");
            }
        }
        public int Port
        {
            get { return port; }
            set
            {
                port = value;
                OnPropertyChanged("Port");
            }
        }
    }
}
