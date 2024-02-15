using Client.MVVM.Model;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Client.MVVM.ViewModel
{
    internal class ClientViewModel : Presenter
    {
        private readonly ClientNet client;
        private ClientModel clientModel;
        private DataModel dataModel;
        private string signalPath;
        private string connectionState;
        private bool _isEnabledConnectButton;
        private string textConnectButton;
        public ClientModel ClientModel
        {
            get { return clientModel; }
            set
            {
                clientModel = value;
                OnPropertyChanged("ClientModel");
            }
        }
        public DataModel DataModel
        {
            get { return dataModel; }
            set
            {
                dataModel = value;
                OnPropertyChanged("DataModel");
            }
        }
        public string SignalPath
        {
            get { return signalPath; }
            set
            {
                signalPath = value;
                OnPropertyChanged("SignalPath");
            }
        }
        public string ConnectionState
        {
            get { return connectionState; }
            set
            {
                connectionState = value;
                OnPropertyChanged("ConnectionState");
            }
        }
        public bool isEnabledConnectButton
        {
            get { return _isEnabledConnectButton; }
            set
            {
                _isEnabledConnectButton = value;
                OnPropertyChanged("isEnabledConnectButton");
            }
        }
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
            DataModel = new DataModel();
            ClientModel = new ClientModel { Date = "", Ip = "127.0.0.1", Port = 8080 };
            client = new ClientNet(DataModel);

            TextConnectButton = "Connect";
            isEnabledConnectButton = true;
            ConnectionState = "Disconnected";
            SignalPath = "imgs\\red.png";

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
            ConnectionState = "Connecting...";
            SignalPath = "imgs\\yellow.png";
        }
        private void Connected()
        {
            TextConnectButton = "Disconnect";
            ConnectionState = "Connected";
            SignalPath = "imgs\\green.png";
        }

        private void Disconnected()
        {
            TextConnectButton = "Connect";
            ConnectionState = "Disconnected";
            SignalPath = "imgs\\red.png";
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
