using Client.MVVM.ViewModel;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var cvm = new ClientViewModel();
            DataContext = cvm;
            Closing += cvm.Closing;
        }
    }
}
