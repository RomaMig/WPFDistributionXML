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
    internal class ClientViewModel : Presenter
    {
        private readonly ClientNet client;
        private ClientModel clientModel;
        public ClientModel ClientModel
        {
            get { return clientModel; }
            set
            {
                clientModel = value;
                OnPropertyChanged("ClientModel");
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
        private string textConnectButton;
        public string TextConnectButton
        {
            get { return textConnectButton; }
            set
            {
                textConnectButton = value;
                OnPropertyChanged("TextConnectButton");
            }
        }
        public RelayCommand Connect { get; set; }
        public RelayCommand RepeatRequest { get; set; }

        public ClientViewModel()
        {
            ClientModel = new ClientModel { Date = "", Ip = "127.0.0.1", Port = 8080 };
            client = new ClientNet();

            TextConnectButton = "Connect";
            isEnabledConnectButton = true;

            client.connecting += Connecting;
            client.connected += Connected;
            client.disconnected += Disconnected;
            client.connectionAttemptCompleted += ConnectionAttemtComleted;
            client.msgRequest += msgRequest;

            Connect = new RelayCommand(o => client.SwitchConnection(ClientModel.Ip, ClientModel.Port));
            RepeatRequest = new RelayCommand(o => client.RepeatRequest());
        }

        private void Connecting()
        {
            TextConnectButton = "Connecting...";
            isEnabledConnectButton = false;
        }
        private void Connected()
        {
            TextConnectButton = "Disconnect";
        }

        private void Disconnected()
        {
            TextConnectButton = "Connect";
        }

        private void ConnectionAttemtComleted()
        {
            isEnabledConnectButton = true;
        }

        private void msgRequest(string msg)
        {
            ClientModel.Date = msg;
        }

        public void Closing(object sender, EventArgs e)
        {
            client.Close();
        }
    }
}
