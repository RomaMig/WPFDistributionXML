using Server.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Server.MVVM.ViewModel
{
    internal class ServerViewModel : Presenter
    {
        private readonly ServerNet server;
        private ServerModel serverModel;
        public ServerModel ServerModel
        {
            get { return serverModel; }
            set
            {
                serverModel = value;
                OnPropertyChanged("ServerModel");
            }
        }
        private string textStartButton;
        public string TextStartButton
        {
            get { return textStartButton; }
            set
            {
                textStartButton = value;
                OnPropertyChanged("TextStartButton");
            }
        }

        public ObservableCollection<TcpClient> ClientsInfo { get; set; }

        public RelayCommand Start { get; set; }
        public RelayCommand Distribution { get; set; }

        public ServerViewModel()
        {
            ClientsInfo = new ObservableCollection<TcpClient>();
            ServerModel = new ServerModel() { Date = "", Ip = "127.0.0.1", Port = 8080 };
            server = new ServerNet();

            TextStartButton = "Start";

            server.serverStarted += ServerStarted;
            server.serverStoped += ServerStoped;
            server.clientConnected += ClientConnected;
            server.clientDisconnected += ClientDisconnected;

            Start = new RelayCommand(o => server.SwitchMode(ServerModel.Ip, ServerModel.Port));
            Distribution = new RelayCommand(o => server.DistributionInfo());
        }

        private void ServerStarted()
        {
            TextStartButton = "Stop";
        }

        private void ServerStoped()
        {
            TextStartButton = "Start";
        }

        private void ClientConnected(TcpClient client)
        {
            try
            {
                ClientsInfo.Add(client);
            } catch (Exception ex) { }
        }

        private void ClientDisconnected(TcpClient client)
        {
            try
            {
                ClientsInfo.Remove(client);
            } catch(Exception ex) { }
        }

        public void Closing(object sender, EventArgs e)
        {
            server.Close();
        }
    }
}
