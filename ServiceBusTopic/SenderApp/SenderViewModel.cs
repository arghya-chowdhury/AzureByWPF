using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.ServiceBus;

namespace SenderApp
{
    public class SenderViewModel : INotifyPropertyChanged
    {
        TopicClient _client;
        string _messageBody;
        TaskScheduler _taskScheduler;
        BrokeredMessage _selectedMessage;

        public event PropertyChangedEventHandler PropertyChanged;

        public SenderViewModel()
        {
            Messages = new ObservableCollection<BrokeredMessage>();
            Messages.CollectionChanged += (s, e) => SendMessageCommand?.RaiseCanExecuteChanged();

            MessageProperties = new ObservableCollection<MessageProperty>();
            MessageProperties.CollectionChanged += (s, e) => ResetPropertiesCommand?.RaiseCanExecuteChanged();

            AddMessageCommand = new DelegateCommand(new Action<object>(OnAddMessage),
                new Predicate<object>(obj => !string.IsNullOrEmpty(MessageBody)));

            ResetPropertiesCommand = new DelegateCommand(new Action<object>((obj) => MessageProperties?.Clear()),
                new Predicate<object>(obj => MessageProperties.Count > 0));

            SendMessageCommand = new DelegateCommand(new Action<object>(OnSendMessage),
                new Predicate<object>(obj => Messages.Count > 0));

            _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            AddSubscriptionRuleAsync();
        }

private void AddSubscriptionRuleAsync()
{
    var topicName = ConfigurationManager.AppSettings["Microsoft.ServiceBus.TopicName"];
    var connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];

    var namespaceMgr = NamespaceManager.CreateFromConnectionString(connectionString);
    var messagingFactory = MessagingFactory.CreateFromConnectionString(connectionString);

    namespaceMgr.GetSubscriptionsAsync(topicName).ContinueWith(t =>
    {
        if (t.IsCompleted)
        {
            var subscriptions = t.Result;
            subscriptions.ToList().ForEach(s =>
            {
                var client = messagingFactory.CreateSubscriptionClient(topicName, s.Name);
                var rules = namespaceMgr.GetRules(topicName, s.Name);

                var defaultRule = rules.FirstOrDefault(rule => rule.Name.ToLower().Contains("default"));
                if (defaultRule != null)
                {
                    client.RemoveRule(defaultRule.Name);
                }

                if (!rules.Any(rule => rule.Name.ToLower().Contains("batchrule")))
                {
                    client.AddRule(new RuleDescription("BatchRule", new SqlFilter("batch = '" + s.Name + "'")));
                }

                client.CloseAsync();
            });
        }
    }).ContinueWith(t => messagingFactory.CloseAsync());
}

        public DelegateCommand SendMessageCommand { get; private set; }

        public DelegateCommand AddMessageCommand { get; private set; }

        public DelegateCommand ResetPropertiesCommand { get; private set; }

        public ObservableCollection<BrokeredMessage> Messages { get; set; }

        public ObservableCollection<MessageProperty> MessageProperties { get; set; }

        public string MessageBody
        {
            get
            {
                return _messageBody;
            }

            set
            {
                _messageBody = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MessageBody"));
                AddMessageCommand?.RaiseCanExecuteChanged();
            }
        }

        public BrokeredMessage SelectedMessage
        {
            get
            {
                return _selectedMessage;
            }

            set
            {
                _selectedMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedMessage"));
                ResetPropertiesCommand?.RaiseCanExecuteChanged();
            }
        }

        private void OnAddMessage(object param)
        {
            var brokeredMessage = new BrokeredMessage(MessageBody);
            Messages.Add(brokeredMessage);
        }

        private void OnSendMessage(object param)
        {
            var topicName = ConfigurationManager.AppSettings["Microsoft.ServiceBus.TopicName"];
            var connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            var client = TopicClient.CreateFromConnectionString(connectionString, topicName);

            Messages.ToList().ForEach(brokeredMessage =>
            {
                MessageProperties.ToList().ForEach(p =>
                    brokeredMessage.Properties.Add(new KeyValuePair<string, object>(p.Key, p.Value)));

                client.SendAsync(brokeredMessage).ContinueWith(t => Messages.Remove(brokeredMessage), _taskScheduler);
               
            });

            client.CloseAsync();
        }
    }
}
