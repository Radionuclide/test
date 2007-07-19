using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace iba.Utility
{
    //class to hold filenames and their timestamps
    [Serializable]
    public class FileSetWithTimeStamps: IEnumerable<string>
    {
        private Set<ComparablePair<string, DateTime>> m_files;
        private List<ComparablePair<string, DateTime>> m_filesSortedOnTime;

        public FileSetWithTimeStamps()
        {
            m_files = new Set<ComparablePair<string, DateTime>>();
            m_filesSortedOnTime = new List<ComparablePair<string, DateTime>>();
        }

        public void Add(string file)
        {
            DateTime time = DateTime.Now;
            try
            {
                FileInfo inf = new FileInfo(file);
                time = inf.LastWriteTime;
            }
            catch { }

            ComparablePair<string, DateTime> myPair = new ComparablePair<string, DateTime>(file, time);
            int pos = m_filesSortedOnTime.BinarySearch(myPair,new ComparablePair<string,DateTime>.BackwardsLexicographicComparer());
            if (pos < 0)
            {
                m_filesSortedOnTime.Insert(~pos, myPair);
                m_files.Add(myPair);
            }
            else if (!Contains(myPair.First)) //other file but same timestamp
            {
                m_filesSortedOnTime.Insert(pos, myPair);
                m_files.Add(myPair);
            }
        }

        public int Count
        {
            get { return m_filesSortedOnTime.Count; }
        }

        public void AddRange(IEnumerable<string> files)
        {
            foreach (string file in files) Add(file);
        }

        public bool Contains(string file)
        {
            int pos = ~m_files.BinarySearch(new ComparablePair<string,DateTime>(file,DateTime.MinValue));
            return (pos >= 0 && pos < m_files.Count && m_files[pos].First == file);
        }

        public bool Remove(string file)
        {
            int pos = ~m_files.BinarySearch(new ComparablePair<string, DateTime>(file, DateTime.MinValue));
            if (pos >= 0 && pos < m_files.Count && m_files[pos].First == file)
            {
                m_files.RemoveAt(pos);
            }
            else return false;

            for (int i = 0; i< m_filesSortedOnTime.Count; i++)
            {
                if (m_filesSortedOnTime[i].First == file)
                {
                    m_filesSortedOnTime.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            m_filesSortedOnTime.Clear();
            m_files.Clear();
        }

        public string OldestExistingFile()
        {
            foreach (ComparablePair<string, DateTime> p in m_filesSortedOnTime)
            {
                try
                {
                    if (File.Exists(p.First))
                        return p.First;
                }
                catch { }
            }
            return null;
        }

        static public FileSetWithTimeStamps Merge(FileSetWithTimeStamps a, FileSetWithTimeStamps b)
        {
            FileSetWithTimeStamps answer = new FileSetWithTimeStamps();
            answer.m_files = Set<ComparablePair<string, DateTime>>.Union(a.m_files, b.m_files);
            answer.m_filesSortedOnTime.AddRange(answer.m_files);
            answer.m_filesSortedOnTime.Sort(new ComparablePair<string, DateTime>.BackwardsLexicographicComparer());
            return answer;
        }

        #region IEnumerable<string> Members

        public IEnumerator<string> GetEnumerator()
        {
            return new SortedOnTimeEnumerator(this);
        }

        
        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new SortedOnTimeEnumerator(this);
        }

        #endregion


        private class SortedOnTimeEnumerator: IEnumerator<string>
        {
            private List<ComparablePair<string, DateTime>>.Enumerator _enumerator;
            FileSetWithTimeStamps m_parent;
            public SortedOnTimeEnumerator(FileSetWithTimeStamps parent)
            {
                m_parent = parent;
                _enumerator = parent.m_filesSortedOnTime.GetEnumerator();
            }

            #region IEnumerator<string> Members

            public string Current
            {
                get {
                    return _enumerator.Current.First;
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                _enumerator.Dispose();
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get { return _enumerator.Current.First; }
            }

            public bool MoveNext()
            {
               return _enumerator.MoveNext();
            }

            public void Reset()
            {
                _enumerator.Dispose();
                _enumerator = m_parent.m_filesSortedOnTime.GetEnumerator();
            }

            #endregion
        }

        public FileSetWithTimeStamps Clone()
        {
            FileSetWithTimeStamps result = new FileSetWithTimeStamps();
            result.m_files = m_files.Clone();
            result.m_filesSortedOnTime.AddRange(m_filesSortedOnTime);
            return result;
        }

        public string this[int i]
        {
            get { return m_filesSortedOnTime[i].First; }
        }

        public void RemoveAt(int i)
        {
            Remove(m_filesSortedOnTime[i].First);
        }
    }
}
