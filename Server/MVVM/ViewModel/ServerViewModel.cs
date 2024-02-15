using Microsoft.Win32;
using Server.MVVM.Model;
using Parser;
using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;

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
        private DataModel dataModel;
        public DataModel DataModel
        {
            get { return dataModel; }
            set
            {
                dataModel = value;
                OnPropertyChanged("DataModel");
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
        public RelayCommand Broadcast { get; set; }
        public RelayCommand OpenFile { get; set; }

        public ServerViewModel()
        {
            ClientsInfo = new ObservableCollection<TcpClient>();
            DataModel = new DataModel();
            ServerModel = new ServerModel() { Ip = "127.0.0.1", Port = 8080 };
            server = new ServerNet(DataModel);

            TextStartButton = "Start";

            server.serverStarted += ServerStarted;
            server.serverStoped += ServerStoped;
            server.clientConnected += ClientConnected;
            server.clientDisconnected += ClientDisconnected;

            Start = new RelayCommand(o => server.SwitchMode(ServerModel.Ip, ServerModel.Port));
            Broadcast = new RelayCommand(o => server.BroadcastInfo());
            OpenFile = new RelayCommand(o => Browse(o));
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
        private void Browse(object param)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "XML файл (*.xml)|*.xml";
            d.CheckFileExists = true;
            d.CheckPathExists = true;
            
            if (d.ShowDialog() == true)
            {
                XmlParser parser = new XmlParser();

                parser.Load(d.FileName);
                var page = parser.Parse() as Page;

                DataModel.setPage(page);
                server.BroadcastInfo();
            }
        }

        public void Closing(object sender, EventArgs e)
        {
            server.Close();
        }
    }
}
