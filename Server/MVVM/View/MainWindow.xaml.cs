using Server.MVVM.ViewModel;
using System.Windows;

namespace Server
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var svm = new ServerViewModel();
            DataContext = svm;
            Closing += svm.Closing;
        }
    }
}
