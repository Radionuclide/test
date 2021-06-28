using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Skins;
using Grpc.Core;
using iba.Annotations;
using iba.Controls;
using Messages.V1;
using TransferRequest = Messages.V1.TransferRequest;

namespace iba.Processing.IbaGrpc
{
    class DataTransferImpl : DataTransfer.DataTransferBase
    {
        private static BindingList<DiaganosticsData> m_connectedClients;
        public static BindingList<DiaganosticsData> ConnectedClients
        {
            get => m_connectedClients;
            set => m_connectedClients = value;
        }

        public DataTransferImpl()
        {
            m_connectedClients = new BindingList<DiaganosticsData>();
        }

        private int count = 0;
        public override async Task<TransferResponse> TransferFile(IAsyncStreamReader<TransferRequest> requestStream, ServerCallContext context)
        {
            DiaganosticsData diaganosticsData = new DiaganosticsData
            {
                CientName = context.RequestHeaders.Get("clientname").Value,
                ClientVersion = context.RequestHeaders.Get("clientversion").Value,
                Filename = context.RequestHeaders.Get("filename").Value,
                Path = context.RequestHeaders.Get("path").Value,
                apiKey = context.RequestHeaders.Get("apikey").Value
            };

            if (ConnectedClients.All(x => diaganosticsData.CientName != x.CientName))
            {
                ConnectedClients.Add(diaganosticsData);
            }

            string file = null;
            var data = new List<byte>();

            while (await requestStream.MoveNext())
            {
                Debug.WriteLine("Received " +
                                  requestStream.Current.Chunk.Length + " bytes");
                data.AddRange(requestStream.Current.Chunk);
                file = requestStream.Current.FileName;
            }



            try
            {
                ConnectedClients.First(x => x.CientName == diaganosticsData.CientName).CientName = count++.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test"));

            file = file.Substring(file.LastIndexOf("\\") +1);

            File.WriteAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test", file), data.ToArray());

            return new TransferResponse()
            {
                Status = "OK",
                Progress = 1
            };
        }
    }

    class DiaganosticsData : INotifyPropertyChanged
    {
        private string m_cientName;
        private string m_clientVersion;
        private string m_filename;
        private string m_path;
        private string m_apiKey;

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

        public string apiKey
        {
            get => m_apiKey;
            set => m_apiKey = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
