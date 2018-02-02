﻿
namespace XmlExtract
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using System.Xml.Serialization;
    using System.Collections.Generic;
    using iba.ibaFilesLiteDotNet;


    public class DatExtractor : IDisposable
    {
        private StringBuilder _error;
        private IbaFileReader _reader;
        private ResolveEinheit _resolveEinheit;

        /// <summary>
        /// Initializes a new instance of the DatExtractor class.
        /// </summary>
        public DatExtractor()
        {
            _reader = new IbaFileReader();
            _resolveEinheit = new ResolveEinheit();
        }

        public string ExtractToXml(string datfile, string xmlfile, IExtractorData data)
        {
            _error = new StringBuilder();

            _reader.Open(datfile);

            _resolveEinheit.Open(data.XsdLocation);
            if (!String.IsNullOrEmpty(_resolveEinheit.Error))
                _error.AppendLine(_resolveEinheit.Error);

            ErzeugungType met = FillMaterialEreignis(_reader, data);
            if (!String.IsNullOrEmpty(data.XmlSchemaLocation))
                met.xsiSchemaLocation = "http://www.thyssen.com/xml/schema/qbic " + data.XmlSchemaLocation;

            _reader.Close();

            if (_error.Length > 0)
                return _error.ToString();

            using (var stream = File.CreateText(xmlfile))
            {
                var ser = new XmlSerializer(typeof(ErzeugungType));
                ser.Serialize(stream, met);
            }

            return string.Empty;

        }


        internal ErzeugungType FillMaterialEreignis(IbaFileReader reader, IExtractorData data)
        {

            var infoParser = new ResolveInfo();

            Info info = ResolveInfo.Resolve(reader, data.StandOrt);

            if (!String.IsNullOrEmpty(info.Error))
                _error.AppendLine(info.Error);

            var met = new ErzeugungType();
            met.MaterialHeader.TKSIdent = info.TKSIdent;
            met.MaterialHeader.LokalerIdent = info.LocalIdent;

            if (data.StandOrt == StandortType.Anderer)
                met.MaterialHeader.Standort = data.AndererStandort;
            else
            {
                met.MaterialHeader.Standort = data.StandOrt.ToString();
                if (data.StandOrt == StandortType.DU)
                    met.MaterialHeader.MaterialArt = info.MaterialArt;
            }

            foreach (IbaChannelReader channel in reader.Channels)
            {

                var signalId = channel.ResolveSignalId(data.IdField);
                if (channel.Name.Contains("__IE__") || channel.Name.Contains("__SE__"))
                {
                    EinzelwertType ew = GetEinzelWert(channel, signalId);
                    met.Einzelwerte.Einzelwert.Add(ew);
                }
                else
                {
                    MessungType mes = GetMessung(info, channel, signalId);
                    met.Messung.Add(mes);
                }

            }

            if (met.Einzelwerte.Einzelwert.Count > 0)
                met.Einzelwerte.Aggregat = info.Aggregat;

            var letzteMessung = met.Messung.LastOrDefault();
            if (letzteMessung != null)
                letzteMessung.LetzteMsgAmDurchsatz = true;


            return met;
        }

        private MessungType GetMessung(Info info, IbaChannelReader channel, string signalId)
        {
            var mes = new MessungType();
            mes.Bandlaufrichtung = info.Bandlaufrichtung;
            mes.Endprodukt = info.Endprodukt;
            mes.Messzeitpunkt = info.Messzeitpunkt;
            mes.Aggregat = info.Aggregat;
            mes.Gruppe = ResolveGruppe.Resolve(signalId);
            mes.LetzteMsgAmDurchsatz = false;

            mes.IDMessgeraet = String.Format("MI__{0}", signalId);

            var spur = new SpurType();
            spur.Bezeichner = signalId;
            spur.DimensionX = channel.DefaultXBaseType == XBaseType.LENGTH ? BezugDimensionEnum.Laenge : BezugDimensionEnum.Zeit;

            var channelUnit = channel.Unit;
            spur.Einheit = _resolveEinheit.Parse(channelUnit);
            if (spur.Einheit == null && !String.IsNullOrEmpty(channelUnit))
                spur.EinheitLokal = channelUnit;

            ChannelData channelData = GetChannelData(channel);

            if (channelData != null)
            {
                spur.Raster1D.SegmentgroesseX = channelData.Interval;
                spur.Raster1D.SegmentOffsetX = channelData.XOffset;

                if (channelData.Data != null)
                    spur.Raster1D.WerteList = channelData.Data;

                mes.Spur.Add(spur);
            }
            return mes;
        }

        private EinzelwertType GetEinzelWert(IbaChannelReader channel, string signalId)
        {
            var ew = new EinzelwertType();
            ew.Bezeichner = signalId;

            string ewChannelUnit = channel.Unit;
            ew.Einheit = _resolveEinheit.Parse(ewChannelUnit);
            if (ew.Einheit == null && !String.IsNullOrEmpty(ewChannelUnit))
                ew.EinheitLokal = ewChannelUnit;

            if (channel.Text)
            {
                string[] textData = null;
                double[] timeStampDate = null;
                try
                {
                    channel.QueryNEData(channel.DefaultXBaseType, out timeStampDate, out textData);
                }
                catch { }

                if (textData != null && textData.Length > 0)
                    ew.Wert = textData[0];
                return ew;
            }

            var ewChannelData = GetChannelData(channel);
            if (ewChannelData != null && ewChannelData.Data != null && ewChannelData.Data.Count > 0)
                ew.Wert = System.Xml.XmlConvert.ToString(ewChannelData.Data[0]);
            return ew;
        }

        private static ChannelData GetChannelData(IbaChannelReader channel)
        {
            float interval, xoffset;
            float[] data;
            try
            {
                channel.QueryData(channel.DefaultXBaseType, out interval, out xoffset, out data);
            }
            catch
            {
                return null;
            }

            var channelData = new ChannelData();
            channelData.Interval = interval;
            channelData.XOffset = xoffset;

            if (data == null)
                return channelData;

            channelData.Data = new List<float>(data);
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
                    _reader.Dispose();
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
