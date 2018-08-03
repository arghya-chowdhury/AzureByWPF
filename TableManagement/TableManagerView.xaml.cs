using System.Windows;

namespace TableManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TableManagerViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            viewModel = new TableManagerViewModel();
            DataContext = viewModel;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            viewModel.LoadData();
        }

        private void DataGrid_AddingNewItem(object sender, System.Windows.Controls.AddingNewItemEventArgs e)
        {
            e.NewItem = new StudentEntry();
        }
    }
}
