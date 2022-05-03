using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using iba.ibaFilesLiteDotNet;

namespace XmlExtract
{

    internal static class ExtensionMethods
    {

        internal static string ResolveSignalId(this IbaChannelReader channel, IdFieldLocation idField)
        {
            string value = String.Empty;
            switch (idField)
            {
                case IdFieldLocation.PDA_Comment1:
                    value = channel.Comment1;
                    break;
                case IdFieldLocation.PDA_Comment2:
                    value = channel.Comment2;
                    break;
            }

            if (!String.IsNullOrEmpty(value))
                return value.Trim();

            return channel.Name.Trim();
        }

        public static float GetValueAsFloat(this IDictionary<string, string> infoFields, string keyName, float defaultvalue)
        {
            if (infoFields.TryGetValue(keyName, out var value))
            {
                if (Single.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var widthValue))
                    return widthValue;
            }

            return defaultvalue;
        }

        /// <summary>
        /// Get max frame count fot the adjusted matrix, reduced by the min offset frames.
        /// Get the min frame count for the offset, that will be used to truncate unnecessary NaN values and
        /// will be added to the initial YOffset.
        /// </summary>
        /// <param name="rasterList"></param>
        /// <returns></returns>
        public static (int maxFrames, float minOffsetFrames) GetLimits(this List<Raster1DType> rasterList)
        {
            var maxEntry = rasterList.MaxBy(r => r.GetTotalFrames());
            // var maxOffsetFrames = maxEntry.GetSegmentOffsetXFrames();
            var maxFrames = maxEntry.GetTotalFrames();

            var minEntry = rasterList.MinBy(r => r.SegmentOffsetX);
            //var minOffset = minEntry.SegmentOffsetX;
            var minOffsetFrames = minEntry.GetSegmentOffsetXFrames();
            maxFrames -= (int)minOffsetFrames;

            return (maxFrames, minOffsetFrames);
        }


        public static float GetSegmentOffsetXFrames(this Raster1DType raster)
        {
            var divisor = raster.SegmentgroesseX != 0.0f ? raster.SegmentgroesseX : 1.0f;
            return raster.SegmentOffsetX / divisor;
        }

        public static int GetTotalFrames(this Raster1DType raster)
        {
            return (int)raster.GetSegmentOffsetXFrames() + raster.WerteList.Count;
        }


        public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> list)
        {
            // ReSharper disable PossibleMultipleEnumeration
            return
                //generate the list of top-level indices of transposed list
                Enumerable.Range(0, list.Min(l => l.Count()))
                    //selects elements at list[y][x] for each x, for each y
                    .Select(x => list.Select(y => y.ElementAt(x)));

            // ReSharper restore PossibleMultipleEnumeration
        }




        // MaxBy and MinBy are based on John Skeet's entry at SO
        // https://stackoverflow.com/a/914198/14785902
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, null);
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            comparer = comparer ?? Comparer<TKey>.Default;

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var max = sourceIterator.Current;
                var maxKey = selector(max);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, maxKey) > 0)
                    {
                        max = candidate;
                        maxKey = candidateProjected;
                    }
                }
                return max;
            }
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, null);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            comparer = comparer ?? Comparer<TKey>.Default;

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var min = sourceIterator.Current;
                var minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }


    }
}

