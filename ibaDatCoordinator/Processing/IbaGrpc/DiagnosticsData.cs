using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using iba.Annotations;

namespace iba.Processing.IbaGrpc
{
    
    [Serializable]
    public class DiagnosticsData : INotifyPropertyChanged
    {
        private string m_clientId;
        private string m_taskName;
        private string m_clientName;
        private string m_clientVersion;
        private string m_filename;
        private string m_path;
        private string m_apiKey;
        private int m_maxBandwidth;
        private int m_transferredFiles;

        public string TaskName
        {
            get => m_taskName;
            set
            {
                if (value == m_taskName)
                    return;
                m_taskName = value;
                OnPropertyChanged();
            }
        }
        public string ClientId
        {
            get => m_clientId;
            set
            {
                if (value == m_clientId)
                    return;
                m_clientId = value;
                OnPropertyChanged();
            }
        }
        public string ClientName
        {
            get => m_clientName;
            set
            {
                if(value == m_clientName)
                    return;
                m_clientName = value;
                OnPropertyChanged();
            } 
        }

        public string ClientVersion
        {
            get => m_clientVersion;
            set
            {
                if (value == m_clientVersion)
                    return;
                m_clientVersion = value;
                OnPropertyChanged();
            }
        }

        public string Filename
        {
            get => m_filename;
            set
            {
                if (value == m_filename)
                    return;
                m_filename = value;
                OnPropertyChanged();
            }
        }

        public string Path
        {
            get => m_path;
            set
            {
                if (value == m_path)
                    return;
                m_path = value;
                OnPropertyChanged();
            }
        }

        public string ApiKey
        {
            get => m_apiKey;
            set
            {
                if (value == m_apiKey)
                    return;
                m_apiKey = value;
                OnPropertyChanged();
            }
        }

        public int MaxBandwidth
        {
            get => m_maxBandwidth;
            set
            {
                if (value == m_maxBandwidth)
                    return;
                m_maxBandwidth = value;
                OnPropertyChanged();
            }
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