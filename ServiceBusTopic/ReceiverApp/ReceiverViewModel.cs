using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.ObjectModel;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using System.Collections.Generic;

namespace ReceiverApp
{
    public class ReceiverViewModel : INotifyPropertyChanged
    {
        TaskScheduler _taskScheduler;
        QueuedMessage _selectedMessage;

        SubscriptionDescription _selectedSubscriptions;
        IEnumerable<SubscriptionDescription> _subscriptions;

        public event PropertyChangedEventHandler PropertyChanged;

        public ReceiverViewModel()
        {
            _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Messages = new ObservableCollection<QueuedMessage>();

            ReloadCommand = new DelegateCommand(new Action<object>(OnReload), new Predicate<object>(obj => SelectedSubscription != null));

            LoadSubscriptions().ContinueWith(t => !t.IsFaulted ? Subscriptions = t.Result : Subscriptions = null, _taskScheduler);
        }

        public IEnumerable<SubscriptionDescription> Subscriptions
        {
            get
            {
                return _subscriptions;
            }

            set
            {
                _subscriptions = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Subscriptions"));
            }
        }

        public SubscriptionDescription SelectedSubscription
        {
            get
            {
                return _selectedSubscriptions;
            }

            set
            {
                _selectedSubscriptions = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedSubscription"));
                ReloadCommand?.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand ReloadCommand { get; private set; }

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
            }
        }

private async Task<IEnumerable<SubscriptionDescription>> LoadSubscriptions()
{
    var topicName = ConfigurationManager.AppSettings["Microsoft.ServiceBus.TopicName"];
    var connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];

    var namespaceMgr = NamespaceManager.CreateFromConnectionString(connectionString);
    var messagingFactory = MessagingFactory.CreateFromConnectionString(connectionString);

    return await namespaceMgr.GetSubscriptionsAsync(topicName);
}

private void OnReload(object param)
{
    Messages.Clear();

    var topicName = ConfigurationManager.AppSettings["Microsoft.ServiceBus.TopicName"];
    var connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];

    var messagingFactory = MessagingFactory.CreateFromConnectionString(connectionString);
    var messageReceiver = messagingFactory.CreateMessageReceiver(topicName + "/subscriptions/" + SelectedSubscription.Name, ReceiveMode.PeekLock);

    while (true)
    {
        var message = messageReceiver.Receive(TimeSpan.FromSeconds(10));
        if (message != null)
        {
            Messages.Add(new QueuedMessage(message));
            message.CompleteAsync();
        }
        else
        {
            break;
        }
    }
    messageReceiver.Close();
    messagingFactory.CloseAsync();
}
}
}
