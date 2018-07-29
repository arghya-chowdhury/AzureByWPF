using System.Windows;

namespace StudentApplication
{
    public partial class MainWindow : Window
    {
        StudentViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            viewModel = new StudentViewModel();
            DataContext = viewModel;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            viewModel.LoadData();
        }

        private void OnAddedNewItem(object sender, System.Windows.Controls.AddingNewItemEventArgs e)
        {
            e.NewItem = new StudentEntry();
        }
    }
}
