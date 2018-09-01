using System;
using System.ComponentModel;

namespace BlobManagement
{
    public class Model
    {
        public string Container { get; set; }
        public ImageInfo[] Images { get; set; }
    }

    public class ImageInfo : INotifyPropertyChanged
    {
        byte[] _source;
        double _size;
        string _lastDownloaded;

        public string ContainerName { get; set; }
        public string Name { get; set; }

        public byte[] Source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Source"));
            }
        }

        public double Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = System.Math.Round(value/(1024*1024), 2);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Size"));
            }
        }

        public string LastDownloaded
        {
            get
            {
                return _lastDownloaded;
            }
            set
            {
                _lastDownloaded = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastDownloaded"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
