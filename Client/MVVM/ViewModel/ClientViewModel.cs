using Client.Core;
using Client.MVVM.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Client.MVVM.ViewModel
{
    internal class ClientViewModel : INotifyPropertyChanged
    {
        private readonly ClientNet client;
        private ClientModel _client;
        public ClientModel Client
        {
            get { return _client; }
            set
            {
                _client = value;
                OnPropertyChanged("Client");
            }
        }
        private bool _isEnabledConnectButton;
        public bool isEnabledConnectButton
        {
            get { return _isEnabledConnectButton; }
            set
            {
                _isEnabledConnectButton = value;
                OnPropertyChanged("isEnabledConnectButton");
            }
        }
        private string _textConnectButton;
        public string TextConnectButton
        {
            get { return _textConnectButton; }
            set
            {
                _textConnectButton = value;
                OnPropertyChanged("TextConnectButton");
            }
        }
        public RelayCommand Connect { get; set; }
        public RelayCommand RepeatRequest { get; set; }

        public ClientViewModel()
        {
            Client = new ClientModel { Date = "", Ip = "127.0.0.1", Port = 8080 };
            client = new ClientNet();

            TextConnectButton = "Connect";
            isEnabledConnectButton = true;

            client.disconnected += Disconnected;
            client.connectionAttemptCompleted += ConnectionAttemtComleted;

            Connect = new RelayCommand(o => ConnectToServer());
            RepeatRequest = new RelayCommand(o => client.RepeatRequest());
        }

        private void ConnectToServer()
        {
            if (!client.Connected)
            {
                TextConnectButton = "Connecting...";
                isEnabledConnectButton = false;

                client.Connect(Client.Ip, Client.Port);
            }
            else
            {
                client.Response("disconnect");
                TextConnectButton = "Connect";

                client.Close();
            }
        }

        private void Disconnected()
        {
            TextConnectButton = "Connect";
        }

        private void ConnectionAttemtComleted()
        {
            isEnabledConnectButton = true;
        }

        public void Closing(object sender, EventArgs e)
        {
            client.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
