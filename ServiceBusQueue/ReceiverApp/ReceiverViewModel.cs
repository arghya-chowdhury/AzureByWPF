using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.ServiceBus.Messaging;

namespace ReceiverApp
{
    public class ReceiverViewModel : INotifyPropertyChanged
    {
        QueueClient _client;
        MessageSession _messageSession;
        TaskScheduler _taskScheduler;
        QueuedMessage _selectedMessage;

        public event PropertyChangedEventHandler PropertyChanged;

        public ReceiverViewModel()
        {
            _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Messages = new ObservableCollection<QueuedMessage>();

            ReloadCommand = new DelegateCommand(new Action<object>(OnReload));

            AbandonCommand = new DelegateCommand(new Action<object>(OnAbandon),
                new Predicate<object>(obj => SelectedMessage != null));

            CompleteCommand = new DelegateCommand(new Action<object>(OnComplete),
                new Predicate<object>(obj => SelectedMessage != null));

            EstablishConnection();
            OnReload(null);
        }

        public DelegateCommand ReloadCommand { get; private set; }

        public DelegateCommand CompleteCommand { get; private set; }

        public DelegateCommand AbandonCommand { get; private set; }

        public ObservableCollection<QueuedMessage> Messages { get; set; }

        public QueuedMessage SelectedMessage
        {
            get
            {
                return _selectedMessage;
            }

            set
            {
                _selectedMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedMessage"));
                AbandonCommand?.RaiseCanExecuteChanged();
                CompleteCommand?.RaiseCanExecuteChanged();
            }
        }

        private void EstablishConnection()
        {
            CloseActiveClient();

            var name = ConfigurationManager.AppSettings["Microsoft.ServiceBus.QueueName"];
            var connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            _client = QueueClient.CreateFromConnectionString(connectionString, name);
        }

        private void OnReload(object param)
        {
            EstablishConnection();

            OnMessageOptions options = new OnMessageOptions();
            options.AutoComplete = false;
            options.MaxConcurrentCalls = 5;
            options.AutoRenewTimeout = TimeSpan.FromMinutes(1);

            //Required when you Enabled Session
            _client.GetMessageSessionsAsync().ContinueWith(t =>
            {
                if (t.IsCompleted)
                {
                    t.Result.ToList().ForEach(ms =>
                    {
                        _messageSession = _client.AcceptMessageSession(ms.SessionId);
                        _messageSession.OnMessageAsync(new Func<BrokeredMessage, Task>(ProcessMessagesAsync), options);
                    });
                }
            });
        }

        Task ProcessMessagesAsync(BrokeredMessage message)
        {
            return Task.Factory.StartNew(() =>
            {
                Messages.Add(new QueuedMessage(message));
            }, CancellationToken.None, TaskCreationOptions.AttachedToParent, _taskScheduler);
        }

        private void OnAbandon(object param)
        {
            _messageSession.AbandonAsync(SelectedMessage.LockToken);
        }

        private void OnComplete(object param)
        {
            _messageSession.CompleteAsync(SelectedMessage.LockToken)
                .ContinueWith(t => Messages.Remove(Messages.First(m => m.MessageId == SelectedMessage.MessageId)), _taskScheduler); ;
        }

        public void CloseActiveClient()
        {
            Messages.Clear();
            _client?.CloseAsync();
        }
    }
}
