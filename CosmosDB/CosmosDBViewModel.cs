using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace CosmosDB
{
    public class CosmosDBViewModel : INotifyPropertyChanged
    {
        CosmosDBManager _manager;
        TaskScheduler _taskScheduler;
        TaskItem _selectedItem;
        public event PropertyChangedEventHandler PropertyChanged;

        public CosmosDBViewModel()
        {
            _manager = new CosmosDBManager();

            TaskItems = new ObservableCollection<TaskItem>();
            TaskItems.CollectionChanged += (s, e) =>
            {
                AddDocumentsCommand?.RaiseCanExecuteChanged();
                ResetCommand?.RaiseCanExecuteChanged();
            };

            DBItems = new ObservableCollection<TaskItem>();
            DBItems.CollectionChanged += (s, e) => DeleteDocumentCommand?.RaiseCanExecuteChanged();

            AddDocumentsCommand = new DelegateCommand(new Action<object>(OnAddDocuments),
                new Predicate<object>(obj => TaskItems.Count > 0));

            DeleteDocumentCommand = new DelegateCommand(new Action<object>(OnDeleteDocuments),
                new Predicate<object>(obj => SelectedItem!=null));

            ResetCommand = new DelegateCommand(new Action<object>((obj) => TaskItems?.Clear()),
                new Predicate<object>(obj => TaskItems.Count > 0));

            _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        public TaskItem SelectedItem
        {
            get
            {
                return _selectedItem;
            }

            set
            {
                _selectedItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedItem"));
                DeleteDocumentCommand?.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<TaskItem> TaskItems { get; set; }

        public ObservableCollection<TaskItem> DBItems { get; set; }

        public DelegateCommand AddDocumentsCommand { get; private set; }

        public DelegateCommand DeleteDocumentCommand { get; private set; }

        public DelegateCommand ResetCommand { get; private set; }

        private void OnAddDocuments(object param)
        {
            _manager.CreateDocuments(TaskItems.ToArray()).ContinueWith(t => RetrieveDocuments(), _taskScheduler);
        }

        private void OnDeleteDocuments(object param)
        {
            _manager.DeleteDocuments(SelectedItem.Id).ContinueWith(t => RetrieveDocuments(), _taskScheduler);
        }

        public void RetrieveDocuments()
        {
            Task.Factory.StartNew(() =>
            {
                DBItems.Clear();
                _manager.RetrieveDocuments().ToList().ForEach(item =>
                {
                    DBItems.Add(item);
                });
            },CancellationToken.None,TaskCreationOptions.None,_taskScheduler);
        }
    }
}