using System.Windows;

namespace Search
{
    public partial class SearchServiceView : Window
    {
        SearchServiceViewModel viewModel;
        public SearchServiceView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            viewModel = new SearchServiceViewModel();
            DataContext = viewModel;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            viewModel.OnLoaded();
        }

        private void DataGrid_AddingNewItem(object sender, System.Windows.Controls.AddingNewItemEventArgs e)
        {
            e.NewItem = new StudentInfo();
        }
    }
}
