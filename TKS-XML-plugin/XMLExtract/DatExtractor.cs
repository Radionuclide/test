
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



        public string ExtractToXml(string datfile, string xmlfile, StandortType st)
        {
            _error = new StringBuilder();


            _reader.Open(datfile);

            MaterialEreignisType met = FillMaterialEreignis(_reader, st);

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


        internal MaterialEreignisType FillMaterialEreignis(IbaFileReader reader, StandortType st)
        {

            var infoParser = new ResolveInfo();

            Info info = ResolveInfo.Resolve(reader);

            if (!String.IsNullOrEmpty(info.Error))
                _error.AppendLine(info.Error);

            var met = new MaterialEreignisType();
            met.MaterialHeader.LokalerIdent = info.LocalIdent;
            met.MaterialHeader.Standort = st;
            met.MaterialHeader.MaterialArt = MaterialArtType.VZ;

            foreach (IbaChannelReader channel in reader.Channels())
            {
                var mes = new MessungType();
                mes.Bandlaufrichtung = info.Bandlaufrichtung;
                mes.Endprodukt = info.Endprodukt;
                mes.Messzeitpunkt = info.Messzeitpunkt;

                mes.Messgroesse = ResolveMessgroesse.Resolve(channel.Unit());
                mes.IDMessgeraet = channel.CreateIDMessgeraet();

                met.Messung.Add(mes);

                var spur = new SpurType();
                spur.Bezeichner = channel.Name();
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

            var listOfValues = new List<float>(values);
            channelData.Data = listOfValues.ConvertAll<double>(invalue => { return invalue; });

            return channelData;
        }

        #region IDisposable Members

        private void Dispose(bool disposing)
        {
            if (disposing)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(_reader);          
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DatExtractor()
        {
            Dispose(false);
        }

        #endregion
    }
}
