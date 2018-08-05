using System.Windows;

namespace CosmosDB
{
    public partial class CosmosDBView : Window
    {
        CosmosDBViewModel viewModel;
        public CosmosDBView()
        {
            InitializeComponent();
            viewModel = new CosmosDBViewModel();
            Loaded += OnLoaded;
            DataContext = viewModel;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            viewModel.RetrieveDocuments();
        }

        private void OnAddProperty(object sender, System.Windows.Controls.AddingNewItemEventArgs e)
        {
            e.NewItem = new TaskItem();
        }
    }
}
