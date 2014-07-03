﻿
namespace XmlExtract
{
    using System;
    using System.IO;
    using System.Text;
    using System.Globalization;
    using System.Xml.Serialization;
    using System.Collections.Generic;
    using ibaFilesLiteLib;


    public class DatExtractor : IDisposable
    {
        private StringBuilder _error;
        private IbaFile _reader;

        /// <summary>
        /// Initializes a new instance of the DatExtractor class.
        /// </summary>
        public DatExtractor()
        {
            _reader = new IbaFile();
        }



        public string ExtractToXml(string datfile, string xmlfile, IExtractorData data)
        {
            _error = new StringBuilder();

            _reader.Open(datfile);

            MaterialEreignisType met = FillMaterialEreignis(_reader, data);
            if (!String.IsNullOrEmpty(data.XmlSchemaLocation))
                met.xsiSchemaLocation = "http://www.thyssen.com/xml/schema/qbic " + data.XmlSchemaLocation;

            _reader.Close();

            if (_error.Length > 0)
                return _error.ToString();

            using (var stream = File.CreateText(xmlfile))
            {
                var ser = new XmlSerializer(typeof(MaterialEreignisType));
                ser.Serialize(stream, met);
            }

            return string.Empty;

        }


        internal MaterialEreignisType FillMaterialEreignis(IbaFileReader reader, IExtractorData data)
        {

            var infoParser = new ResolveInfo();

            Info info = ResolveInfo.Resolve(reader, data.StandOrt);

            if (!String.IsNullOrEmpty(info.Error))
                _error.AppendLine(info.Error);

            var met = new MaterialEreignisType();
            met.MaterialHeader.LokalerIdent = info.LocalIdent;

            if (data.StandOrt == StandortType.Anderer)
                met.MaterialHeader.Standort = data.AndererStandort;
            else
            {
                met.MaterialHeader.Standort = data.StandOrt.ToString();
                if (data.StandOrt == StandortType.DU)
                    met.MaterialHeader.MaterialArt = info.MaterialArt;
            }

            foreach (IbaChannelReader channel in reader.Channels())
            {
                var signalId = channel.ResolveSignalId(data.IdField);

                var mes = new MessungType();
                mes.Bandlaufrichtung = info.Bandlaufrichtung;
                mes.Endprodukt = info.Endprodukt;
                mes.Messzeitpunkt = info.Messzeitpunkt;
                mes.Aggregat = info.Aggregat;
                mes.LetzteMsgAmDurchsatz = false;

                //mes.Messgroesse = ResolveMessgroesse.Resolve(channel.Unit());
                mes.IDMessgeraet = String.Format("MI_{0}", signalId);

                met.Messung.Add(mes);

                var spur = new SpurType();
                spur.Bezeichner = signalId;
                spur.DimensionX = channel.IsDefaultLengthbased() == 1 ? BezugDimensionEnum.Laenge : BezugDimensionEnum.Zeit;
                spur.Einheit = ResolveEinheit.Parse(channel.Unit());

                ChannelData channelData = GetChannelData(channel);

                if (null == channelData)
                    continue;

                spur.Raster1D.SegmentgroesseX = channelData.Interval;
                spur.Raster1D.SegmentOffsetX = channelData.XOffset;

                if (null != channelData.Data)
                    spur.Raster1D.WerteList = channelData.Data;

                mes.Spur.Add(spur);
                
            }

            var lastIndex = met.Messung.Count - 1;
            if (lastIndex >= 0)
                met.Messung[lastIndex].LetzteMsgAmDurchsatz = true;


            return met;
        }

        private static ChannelData GetChannelData(IbaChannelReader channel)
        {
            float interval, xoffset;
            object data;
            try
            {
                if (channel.IsDefaultLengthbased() == 1)
                    channel.QueryLengthbasedData(out interval, out xoffset, out data);
                else
                    channel.QueryTimebasedData(out interval, out xoffset, out data);
            }
            catch
            {
                return null;
            }

            var channelData = new ChannelData();
            channelData.Interval = interval;
            channelData.XOffset = xoffset;

            var values = data as IEnumerable<float>;
            if (null == values)
                return channelData;

            channelData.Data = new List<float>(values);
            return channelData;
        }

        #region IDisposable Members

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(_reader);
                }
            }
            _disposed = true;
        }

        ~DatExtractor()
        {
            Dispose(false);
        }
        #endregion

    }
}
