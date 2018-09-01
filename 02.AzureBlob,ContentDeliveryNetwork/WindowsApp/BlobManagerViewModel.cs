using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace BlobManagement
{
    public class BlobManagerViewModel : INotifyPropertyChanged
    {
        BlobManager manager;
        IEnumerable<Model> _source;
        string _path;
        string _container;
        public event PropertyChangedEventHandler PropertyChanged;

        public BlobManagerViewModel()
        {
            Path = "Browse An Image";
            Source = new ObservableCollection<Model>();
            manager = new BlobManager();
            UploadCommand = new DelegateCommand(new Action<object>(OnUpload), new Predicate<object>(OnCanExecuteUpload));
            DownloadCommand = new DelegateCommand(new Action<object>(OnDownload));
            DeleteCommand = new DelegateCommand(new Action<object>(OnDelete));
        }

        public void LoadData()
        {
            manager.InitializeAsync().ContinueWith(t =>
            {
                Source = manager.GetBlobsAsync().Result;
            });
        }

        public string Container
        {
            get
            {
                return _container;
            }
            set
            {
                _container = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Container"));
            }
        }

        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Path"));
                UploadCommand?.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<Model> Source
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

        public DelegateCommand UploadCommand { get; private set; }

        public DelegateCommand DownloadCommand { get; private set; }

        public DelegateCommand DeleteCommand { get; private set; }

        private bool OnCanExecuteUpload(object param)
        {
            return Path != null && File.Exists(Path);
        }

        private void OnUpload(object param)
        {
            manager.UploadToBlobAsync(Container, Path).ContinueWith(t =>
            {
                if (t.IsCompleted)
                {
                    Source = manager.GetBlobsAsync().Result;
                }
            });
        }

        private void OnDownload(object param)
        {
            var imageInfo = param as ImageInfo;
            if (imageInfo != null)
            {
                if (imageInfo.Source == null)
                {
                    manager.DownloadBlobAsync(imageInfo.ContainerName, imageInfo.Name).ContinueWith(t =>
                    {
                        if (t.IsCompleted)
                        {
                            imageInfo.Size = t.Result.Item1.Length;
                            imageInfo.Source = t.Result.Item1;
                            imageInfo.LastDownloaded = t.Result.Item2;
                        }
                    });
                }
            }
        }

        private void OnDelete(object param)
        {
            var container = param as Model;
            if (container != null)
            {
                manager.DeleteContainerAsync(container.Container).ContinueWith(t =>
                {
                    Source = manager.GetBlobsAsync().Result;
                });
                return;
            }

            var blob = param as ImageInfo;
            if (blob != null)
            {
                manager.DeleteBlobAsync(blob.ContainerName, blob.Name).ContinueWith(t =>
                 {
                     Source = manager.GetBlobsAsync().Result;
                 });
                return;
            }
        }
    }
}
