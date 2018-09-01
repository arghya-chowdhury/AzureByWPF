using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StorageManagement
{
    public class TableManagerViewModel : INotifyPropertyChanged
    {
        TableManager manager;
        IList<StudentEntry> _source;
        StudentEntry _selectedItem;
        public event PropertyChangedEventHandler PropertyChanged;

        public TableManagerViewModel()
        {
            Source = new List<StudentEntry>();
            manager = new TableManager();
            InsertCommand = new DelegateCommand(new Action<object>(OnInsert));
            UpdateCommand = new DelegateCommand(new Action<object>(OnUpdate), new Predicate<object>(OnCanExecuteChange));
            DeleteCommand = new DelegateCommand(new Action<object>(OnDelete), new Predicate<object>(OnCanExecuteChange));
        }

        public void LoadData()
        {
            manager.InitializeAsync().ContinueWith(t =>
            {
                Source = manager.GetTableData();
            });
        }

        public IList<StudentEntry> Source
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

                UpdateCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedItem"));
            }
        }

        public DelegateCommand InsertCommand { get; private set; }

        public DelegateCommand UpdateCommand { get; private set; }

        public DelegateCommand DeleteCommand { get; private set; }

        private void OnInsert(object param)
        {
            manager.PerformOperationTableAsync(CrudOperation.Create, SelectedItem).ContinueWith(t =>
            {
                Source = manager.GetTableData();
            });
        }

        private void OnUpdate(object param)
        {
            manager.PerformOperationTableAsync(CrudOperation.Update, SelectedItem).ContinueWith(t =>
            {
                Source = manager.GetTableData();
            });
        }

        private void OnDelete(object param)
        {
            manager.PerformOperationTableAsync(CrudOperation.Delete, SelectedItem).ContinueWith(t =>
            {
                Source = manager.GetTableData();
            });
        }

        private bool OnCanExecuteChange(object param)
        {
            return SelectedItem != null;
        }
    }
}
