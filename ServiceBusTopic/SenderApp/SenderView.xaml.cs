using System.Windows;

namespace SenderApp
{
    public partial class SenderView : Window
    {
        SenderViewModel viewModel;
        public SenderView()
        {
            InitializeComponent();
            viewModel = new SenderViewModel();
            DataContext = viewModel;
        }

        private void OnAddProperty(object sender, System.Windows.Controls.AddingNewItemEventArgs e)
        {
            e.NewItem = new MessageProperty();
        }
    }
}
