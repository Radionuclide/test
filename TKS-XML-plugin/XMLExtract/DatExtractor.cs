
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using iba.ibaFilesLiteDotNet;
using iba.Plugins;

namespace XmlExtract
{

    public class DatExtractor : IDisposable
    {
        private StringBuilder _error;
        private readonly IbaFileReader _reader;
        private readonly ResolveEinheit _resolveEinheit;
        private readonly string _pass;

        /// <summary>
        /// Initializes a new instance of the DatExtractor class.
        /// </summary>
        public DatExtractor()
        {
            _reader = new IbaFileReader();
            _resolveEinheit = new ResolveEinheit();
        }

        public DatExtractor(IJobData jobData)
            : this()
        {
            _pass = jobData?.FileEncryptionPassword ?? String.Empty;
        }

        public string ExtractToXml(string datfile, string xmlfile, IExtractorData data)
        {
            _error = new StringBuilder();

            _reader.Open(datfile, _pass);

            _resolveEinheit.Open(data.XsdLocation);
            if (!String.IsNullOrEmpty(_resolveEinheit.Error))
                _error.AppendLine(_resolveEinheit.Error);

            var met = FillMaterialEreignis(_reader, data);
            if (!String.IsNullOrEmpty(data.XmlSchemaLocation))
                met.xsiSchemaLocation = "http://www.thyssen.com/xml/schema/qbic " + data.XmlSchemaLocation;

            _reader.Close();

            if (_error.Length > 0)
                return _error.ToString();

            if (SkipExport(met))
                return String.Empty;

            using (var stream = File.CreateText(xmlfile))
            {
                var ser = new XmlSerializer(typeof(ErzeugungType));
                ser.Serialize(stream, met);
            }

            return String.Empty;

        }

        private static bool SkipExport(ErzeugungType met)
        {
            if (met.Messung.Count == 0 && met.Einzelwerte.Einzelwert.Count == 0)
                return true;

            return false;
        }

        internal ErzeugungType FillMaterialEreignis(IbaFileReader reader, IExtractorData data)
        {

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
                if (info.Vektoren.Any(n => channel.Name.StartsWith(n.Name)))
                    continue;

                var signalId = channel.ResolveSignalId(data.IdField);
                if (ResolveIsEinzelWert.Resolve(signalId))
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

            foreach (var vector in info.Vektoren)
            {
                var mes = GetVectorMessung(vector, info, reader.Channels);
                met.Messung.Add(mes);
            }


            if (met.Einzelwerte.Einzelwert.Count > 0)
            {
                met.Einzelwerte.Aggregat = info.Aggregat;
                met.Einzelwerte.Messzeitpunkt = info.Messzeitpunkt;
            }

            if (info.MarkLetzteMsgAmDurchsatz)
            {
                var letzteMessung = met.Messung.LastOrDefault();
                if (letzteMessung != null)
                    letzteMessung.LetzteMsgAmDurchsatz = true;
            }


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

            mes.IDMessgeraet = $"MI__{signalId}";

            var spur = new SpurType();
            spur.Bezeichner = signalId;
            spur.DimensionX = channel.DefaultXBaseType == XBaseType.LENGTH ? BezugDimensionEnum.Laenge : BezugDimensionEnum.Zeit;

            var channelUnit = channel.Unit;
            spur.Einheit = _resolveEinheit.Parse(channelUnit);
            if (spur.Einheit == null && !String.IsNullOrEmpty(channelUnit))
                spur.EinheitLokal = channelUnit;

            ChannelData channelData = GetChannelData(channel);

            if (channelData != null && channelData.Data != null)
            {
                var r1d = new Raster1DType
                {
                    SegmentgroesseX = channelData.Interval,
                    SegmentOffsetX = channelData.XOffset,
                    WerteList = channelData.Data,
                };

                spur.Raster1D.Add(r1d);
                mes.Spur.Add(spur);
            }
            return mes;
        }

        private MessungType GetVectorMessung(Vector vector, Info info, IList<IbaChannelReader> channels)
        {
            var mes = new MessungType();
            mes.Bandlaufrichtung = info.Bandlaufrichtung;
            mes.Endprodukt = info.Endprodukt;
            mes.Messzeitpunkt = info.Messzeitpunkt;
            mes.Aggregat = info.Aggregat;
            mes.Gruppe = ResolveGruppe.Resolve(vector.Name);
            mes.LetzteMsgAmDurchsatz = false;

            mes.IDMessgeraet = $"0";

            var spur = new SpurType();
            spur.Bezeichner = vector.Name;
            spur.DimensionX = info.VectorsDimensionX;
            if (spur.DimensionX == BezugDimensionEnum.Breite)
                spur.DimensionY = BezugDimensionEnum.Laenge;

            spur.Einheit = _resolveEinheit.Parse(vector.Unit);
            if (spur.Einheit == null && !String.IsNullOrEmpty(vector.Unit))
                spur.EinheitLokal = vector.Unit;

            var nameWithSeparator = vector.Name + ".";
            var vectorChannels = channels.Where(c => c.Name.StartsWith(nameWithSeparator)).OrderBy(c => new Version(c.InfoFields["vector"]))
#if DEBUG
                    .ToList()
#endif
                ;

            var yOffset = (float)vector.ZoneOffset;
            var rasterList = new List<Raster1DType>();
            foreach (var channel in vectorChannels)
            {
                ChannelData channelData = GetChannelData(channel);

                if (channelData != null)
                {
                    var r1d = new Raster1DType
                    {
                        SegmentgroesseX = channelData.Interval,
                        SegmentOffsetX = channelData.XOffset,
                        SegmentOffsetY = yOffset,
                    };

                    if (channelData.Data != null)
                        r1d.WerteList = channelData.Data;

                    rasterList.Add(r1d);
                    yOffset += channel.InfoFields.GetValueAsFloat("Vector_ZoneWidth", 1);
                }
            }

            rasterList.RemoveAll(r => r.WerteList.Count == 0);

            if (spur.DimensionX == BezugDimensionEnum.Breite)
            {

                var zoneWidth = 1.0f;
                var segmentSizeY = 1.0f;

                var secondRasterEntry = rasterList.Skip(1).FirstOrDefault();
                if (secondRasterEntry != null)
                {
                    zoneWidth = secondRasterEntry.SegmentOffsetY;
                    segmentSizeY = secondRasterEntry.SegmentgroesseX;
                }

                var (maxFrames, minOffsetFrames) = rasterList.GetLimits();
                List<List<float>> matrix = FillMatrixWithNaN(rasterList, maxFrames, minOffsetFrames);

                var transposedMatrix = matrix.Transpose();

                var rasterListNew = new List<Raster1DType>();

                //Add minSegmentXoffset as initial offsetY.
                var offsetY = minOffsetFrames * segmentSizeY;
                foreach (var row in transposedMatrix)
                {
                    var r1d = new Raster1DType
                    {
                        SegmentgroesseX = zoneWidth,
                        SegmentOffsetY = offsetY,
                        WerteList = row.ToList(),
                    };

                    rasterListNew.Add(r1d);
                    offsetY += segmentSizeY;
                }

                rasterList = rasterListNew;
            }

            rasterList.RemoveAll(r => r.WerteList.Count == 0);

            spur.Raster1D = rasterList;
            mes.Spur.Add(spur);
            return mes;

        }

        private static List<List<float>> FillMatrixWithNaN(List<Raster1DType> rasterList, int maxFrames, float minOffsetFrames)
        {
            var floatList = new List<List<float>>(rasterList.Count);
            foreach (var raster1D in rasterList)
            {
                var l = new List<float>(maxFrames);
                var prefixFrames = raster1D.GetSegmentOffsetXFrames() - minOffsetFrames;
                var suffixFrames = maxFrames - prefixFrames - raster1D.WerteList.Count;

                l.AddRange(Enumerable.Repeat(Single.NaN, (int)prefixFrames));
                l.AddRange(raster1D.WerteList);
                l.AddRange(Enumerable.Repeat(Single.NaN, (int)suffixFrames));

                floatList.Add(l);
            }

            return floatList;
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
