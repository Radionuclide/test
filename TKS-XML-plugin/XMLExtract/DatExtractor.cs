
namespace XmlExtract
{
    using System;
    using System.IO;
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

            foreach (IbaChannelReader channel in reader.Channels)
            {
                var signalId = channel.ResolveSignalId(data.IdField);

                var mes = new MessungType();
                mes.Bandlaufrichtung = info.Bandlaufrichtung;
                mes.Endprodukt = info.Endprodukt;
                mes.Messzeitpunkt = info.Messzeitpunkt;
                mes.Aggregat = info.Aggregat;
                mes.Gruppe = ResolveGruppe.Resolve(signalId);
                mes.LetzteMsgAmDurchsatz = false;

                //mes.Messgroesse = ResolveMessgroesse.Resolve(channel.Unit());
                mes.IDMessgeraet = String.Format("MI_{0}", signalId);

                met.Messung.Add(mes);

                var spur = new SpurType();
                spur.Bezeichner = signalId;
                spur.DimensionX = channel.DefaultXBaseType == XBaseType.LENGTH ? BezugDimensionEnum.Laenge : BezugDimensionEnum.Zeit;

                string channelUnit = channel.Unit;
                spur.Einheit = _resolveEinheit.Parse(channelUnit);
                if (spur.Einheit == null && !String.IsNullOrEmpty(channelUnit))
                {
                    spur.EinheitLokal = channelUnit;
                }

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
            float[] data;
            try
            {
                channel.QueryData(channel.DefaultXBaseType, out interval, out xoffset, out data);
            }
            catch(Exception ex)
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
