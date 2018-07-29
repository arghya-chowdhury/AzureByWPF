using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace StudentApplication
{
    public class StudentViewModel : INotifyPropertyChanged
    {
        List<StudentEntry> _source;
        StudentEntry _selectedItem;
        HttpClient _client;
        TaskScheduler _taskScheduler;
        string _error;
        public event PropertyChangedEventHandler PropertyChanged;

        public StudentViewModel()
        {
            _source = new List<StudentEntry>();

            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://studentdataservice.azurewebsites.net/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            InsertCommand = new DelegateCommand(new Action<object>(OnInsert));
            DeleteCommand = new DelegateCommand(new Action<object>(OnDelete), new Predicate<object>(OnCanExecuteChange));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }

        public List<StudentEntry> Source
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

        public StudentEntry SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;

                DeleteCommand.RaiseCanExecuteChanged();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedItem"));
            }
        }

        public string Error
        {
            get
            {
                return _error;
            }
            private set
            {
                _error = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Error"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasError"));
            }
        }

        public bool HasError
        {
            get
            {
                return !string.IsNullOrEmpty(Error);
            }
        }

        public DelegateCommand InsertCommand { get; private set; }

        public DelegateCommand DeleteCommand { get; private set; }

        public void LoadData()
        {
            ResetError();
            _client.GetAsync("student/").ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Error = t.Exception.Flatten().Message;
                    return;
                }

                try
                {
                    var resposeMsg = t.Result.Content.ReadAsAsync<List<StudentEntry>>();
                    Source = resposeMsg.Result;
                    return;
                }
                catch (Exception ex)
                {
                    Error = ex.Message;
                }
            });
        }


        private void OnInsert(object param)
        {
            ResetError();
            _client.PostAsJsonAsync("Student/", SelectedItem).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Error = t.Exception.Flatten().Message;
                    return;
                }
                LoadData();
            });
        }

        private void OnDelete(object param)
        {
            ResetError();
            _client.DeleteAsync("Student?rowKey=" + SelectedItem.RowKey).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Error = t.Exception.Flatten().Message;
                    return;
                }
                LoadData();
            });
        }

        private bool OnCanExecuteChange(object param)
        {
            return SelectedItem != null;
        }

        private void ResetError()
        {
            Error = string.Empty;
        }
    }
}
