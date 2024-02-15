namespace Server.MVVM.Model
{
    class ServerModel : Presenter
    {
        private string ip;
        private int port;

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
