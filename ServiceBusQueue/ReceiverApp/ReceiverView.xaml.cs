using System.Windows;

namespace ReceiverApp
{
    public partial class ReceiverView : Window
    {
        ReceiverViewModel viewModel;
        public ReceiverView()
        {
            InitializeComponent();
            viewModel = new ReceiverViewModel();
            DataContext = viewModel;
        }
        
        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            viewModel.CloseActiveClient();
        }
    }
}
