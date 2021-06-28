using System.ComponentModel;
using System.Runtime.CompilerServices;
using iba.Annotations;

namespace iba.Processing.IbaGrpc
{
    class DiagnosticsData : INotifyPropertyChanged
    {
        private string m_cientName;
        private string m_clientVersion;
        private string m_filename;
        private string m_path;
        private string m_apiKey;
        private int m_transferredFiles;

        public string CientName
        {
            get => m_cientName;
            set
            {
                if(value == m_cientName)
                    return;
                m_cientName = value;
                OnPropertyChanged();
            } 
        }

        public string ClientVersion
        {
            get => m_clientVersion;
            set => m_clientVersion = value;
        }

        public string Filename
        {
            get => m_filename;
            set => m_filename = value;
        }

        public string Path
        {
            get => m_path;
            set => m_path = value;
        }

        public string ApiKey
        {
            get => m_apiKey;
            set => m_apiKey = value;
        }

        public int TransferredFiles
        {
            get => m_transferredFiles;
            set
            {
                if (value == m_transferredFiles)
                    return;
                m_transferredFiles = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}