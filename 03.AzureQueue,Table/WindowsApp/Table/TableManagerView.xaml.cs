using System.Windows;
using System.Windows.Controls;

namespace StorageManagement
{
    public partial class TableManagerView : UserControl
    {
        TableManagerViewModel viewModel;
        public TableManagerView()
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
