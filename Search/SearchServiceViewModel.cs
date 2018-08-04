using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Search
{
    public class SearchServiceViewModel : INotifyPropertyChanged
    {
        SearchServiceManager _manager;
        ObservableCollection<StudentInfo> _source;
        string _searchText;
        string _searchResult;
        TaskScheduler _scheduler;
        public event PropertyChangedEventHandler PropertyChanged;

        public SearchServiceViewModel()
        {
            _manager = new SearchServiceManager();

            Source = new ObservableCollection<StudentInfo>();
            Source.CollectionChanged += ((obj, e) =>
            {
                ResetCommand?.RaiseCanExecuteChanged();
                UploadCommand?.RaiseCanExecuteChanged();
            });

            SearchCommand = new DelegateCommand(new Action<object>(OnSearch), new Predicate<object>((obj) => !string.IsNullOrEmpty(SearchText)));
            ClearCommand = new DelegateCommand(new Action<object>((obj) => SearchResult = string.Empty), new Predicate<object>((obj) => !string.IsNullOrEmpty(SearchResult)));
            ResetCommand = new DelegateCommand(new Action<object>((obj) => Source.Clear()), new Predicate<object>((obj) => Source.Count > 0));
            UploadCommand = new DelegateCommand(new Action<object>(OnUpload), new Predicate<object>((obj) => Source.Count > 0));

            _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        public void OnLoaded()
        {
            _manager.InitializeAsync().ContinueWith(t => _manager.CreateIndex());
        }

        public string SearchText
        {
            get
            {
                return _searchText;
            }
            private set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SearchText"));
                    SearchCommand?.RaiseCanExecuteChanged();
                }
            }
        }

        public string SearchResult
        {
            get
            {
                return _searchResult;
            }
            private set
            {
                if (_searchResult != value)
                {
                    _searchResult = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SearchResult"));
                    ClearCommand?.RaiseCanExecuteChanged();
                }
            }
        }

        public ObservableCollection<StudentInfo> Source
        {
            get
            {
                return _source;
            }
            private set
            {
                _source = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Source"));
            }
        }

        public DelegateCommand ResetCommand { get; private set; }

        public DelegateCommand UploadCommand { get; private set; }

        public DelegateCommand SearchCommand { get; private set; }

        public DelegateCommand ClearCommand { get; private set; }

        private void OnSearch(object param)
        {
            _manager.Search(SearchText).ContinueWith(t =>
            {
                var docSearchResult = t.Result;
                var stringBuilder = new StringBuilder();
                docSearchResult.Results.ToList().ForEach(r => stringBuilder.AppendLine(r.Document.ToString()));
                SearchResult = stringBuilder.ToString();
            }, _scheduler);
        }

        private void OnUpload(object param)
        {
            _manager.UploadDocument(Source.ToArray()).ContinueWith(t => Source.Clear(), _scheduler);
        }
    }
}
