using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace StorageManagement
{
    public class QueueManagerViewModel : INotifyPropertyChanged
    {
        QueueManager manager;
        IList<string> _source;
        string _selectedItem;
        bool _isLoading;
        TaskScheduler _scheduler;
        public event PropertyChangedEventHandler PropertyChanged;

        public QueueManagerViewModel()
        {
            Source = new List<string>();
            manager = new QueueManager();
            EnqueueCommand = new DelegateCommand(new Action<object>(OnEnqueue), new Predicate<object>((obj) => !IsLoading));
            DequeueCommand = new DelegateCommand(new Action<object>(OnDequeue), new Predicate<object>(OnCanExecuteChange));
            ClearCommand = new DelegateCommand(new Action<object>(OnClear), new Predicate<object>(OnCanExecuteChange));

            _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        public void LoadData()
        {
            manager.InitializeAsync().ContinueWith(t => UpdateSource());
        }

        public IList<string> Source
        {
            get
            {
                return _source;
            }
            private set
            {
                _source = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Source"));
                RefreshCommands();
            }
        }

        public string SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedItem"));
                }
            }
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            private set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsLoading"));
                    RefreshCommands();
                }
            }
        }

        private void RefreshCommands()
        {
            EnqueueCommand?.RaiseCanExecuteChanged();
            DequeueCommand?.RaiseCanExecuteChanged();
            ClearCommand?.RaiseCanExecuteChanged();
        }

        public DelegateCommand EnqueueCommand { get; private set; }

        public DelegateCommand DequeueCommand { get; private set; }

        public DelegateCommand ClearCommand { get; private set; }

        private void OnEnqueue(object param)
        {
            manager.EnqueueAsync(SelectedItem).ContinueWith(t =>
            {
                try
                {
                    UpdateLoadingStatus(true);
                    Thread.Sleep(QueueManager.VisibilityTimeOutInSec * 1000);
                    UpdateSource();
                }
                finally
                {
                    UpdateLoadingStatus(false);
                }
            });
        }

        private void OnDequeue(object param)
        {
            manager.DequeueAsync().ContinueWith(t => UpdateSource());
        }

        private void OnClear(object param)
        {
            manager.ClearMessageQueueAsync().ContinueWith(t => UpdateSource());
        }

        private void UpdateSource()
        {
            Task.Factory.StartNew(() =>
            {
                Source = manager.PeekQueueMessages();
            }, CancellationToken.None, TaskCreationOptions.None, _scheduler);
        }

        private void UpdateLoadingStatus(bool value)
        {
            Task.Factory.StartNew(() =>
            {
                IsLoading = value;
            }, CancellationToken.None, TaskCreationOptions.None, _scheduler);
        }

        private bool OnCanExecuteChange(object param)
        {
            return !IsLoading && Source.Count > 0;
        }
    }
}
