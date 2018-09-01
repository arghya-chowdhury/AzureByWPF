using System.Windows;
using System.Windows.Controls;

namespace StorageManagement
{
    public partial class QueueManagerView : UserControl
    {
        QueueManagerViewModel viewModel;
        public QueueManagerView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            viewModel = new QueueManagerViewModel();
            DataContext = viewModel;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            viewModel.LoadData();
        }
    }
}
