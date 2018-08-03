using System.Windows;

namespace QueueManagement
{
    public partial class QueueManagerView : Window
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
