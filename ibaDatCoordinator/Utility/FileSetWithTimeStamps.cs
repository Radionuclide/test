using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using System.Windows.Forms;

namespace iba.Utility
{
    //class to hold filenames and their timestamps
    [Serializable]
    public class FileSetWithTimeStamps: IEnumerable<string>
    {
        private Set<ComparablePair<string, DateTime> > m_filesSortedOnName;
        private Set<ComparablePair<string, DateTime> > m_filesSortedOnTimeThenName;

        public FileSetWithTimeStamps()
        {
            m_filesSortedOnName = new Set<ComparablePair<string, DateTime>>(new ComparablePair<string, DateTime>.CompareOnFirstComparer());
            m_filesSortedOnTimeThenName = new Set<ComparablePair<string, DateTime>>(new ComparablePair<string, DateTime>.BackwardsLexicographicComparer());
        }

        public void Add(string file)
        {
            DateTime time = DateTime.Now;
            try
            {

                FileInfo inf = new FileInfo(file);
                time = inf.LastWriteTime;
                if (time.Year < 1700) time = DateTime.Now;
            }
            catch { }

            ComparablePair<string, DateTime> myPair = new ComparablePair<string, DateTime>(file, time);
            int index = m_filesSortedOnName.IndexOf(myPair);
            if (index >= 0)
            {
                ComparablePair<string, DateTime> oldPair = m_filesSortedOnName[index];
                if (oldPair.Second < myPair.Second) //replace with newer date
                {
                    m_filesSortedOnTimeThenName.Remove(oldPair);
                    m_filesSortedOnTimeThenName.Add(myPair);
                    m_filesSortedOnName[index] = myPair;
                }
            }
            else
            {
                m_filesSortedOnTimeThenName.Add(myPair);
                m_filesSortedOnName.Insert(~index,myPair);
            }
        }

        public int Count
        {
            get { return m_filesSortedOnTimeThenName.Count; }
        }

        public void AddRange(IEnumerable<string> files)
        {
            foreach (string file in files) Add(file);
        }

        public bool Contains(string file)
        {
            return m_filesSortedOnName.IndexOf(new ComparablePair<string, DateTime>(file, DateTime.MinValue)) >= 0;
        }

        public bool Remove(string file)
        {
            int index = m_filesSortedOnName.IndexOf(new ComparablePair<string, DateTime>(file, DateTime.MinValue)) ;
            if (index < 0) return false;
            ComparablePair<string, DateTime> oldPair = m_filesSortedOnName[index];
            m_filesSortedOnTimeThenName.Remove(oldPair);
            m_filesSortedOnName.RemoveAt(index);
            return true;
        }

        public void Clear()
        {
            m_filesSortedOnTimeThenName.Clear();
            m_filesSortedOnName.Clear();
        }

        public string OldestExistingFile()
        {
            foreach (ComparablePair<string, DateTime> p in m_filesSortedOnTimeThenName)
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
            answer.m_filesSortedOnName = Set<ComparablePair<string, DateTime>>.Union(a.m_filesSortedOnName, b.m_filesSortedOnName, new ComparablePair<string, DateTime>.CompareOnSecondComparer());
            answer.m_filesSortedOnTimeThenName.AddRange(answer.m_filesSortedOnName);
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
                _enumerator = parent.m_filesSortedOnTimeThenName.GetEnumerator();
            }

            #region IEnumerator<string> Members

            public string Current
            {
                get {
                    return _enumerator.Current == null?null:_enumerator.Current.First;
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
                _enumerator = m_parent.m_filesSortedOnTimeThenName.GetEnumerator();
            }

            #endregion
        }

        public FileSetWithTimeStamps Clone()
        {
            FileSetWithTimeStamps result = new FileSetWithTimeStamps();
            result.m_filesSortedOnName = m_filesSortedOnName.Clone();
            result.m_filesSortedOnTimeThenName = m_filesSortedOnTimeThenName.Clone();
            return result;
        }

        public string this[int i]
        {
            get { return m_filesSortedOnTimeThenName[i].First; }
        }

        public void RemoveAt(int i)
        {
            Remove(m_filesSortedOnTimeThenName[i].First);
        }

        public string Debug()
        {
            StringBuilder myBuilder = new StringBuilder();
            myBuilder.Append("Sorted alphabetically:\r\n");
            for (int i = 0; i < m_filesSortedOnName.Count; i++)
                myBuilder.Append(m_filesSortedOnName[i].First + " " + m_filesSortedOnName[i].Second.ToString() + "\r\n");
            myBuilder.Append("Sorted chronologically:\r\n");
            for (int i = 0; i < m_filesSortedOnTimeThenName.Count; i++)
                myBuilder.Append(m_filesSortedOnTimeThenName[i].First + " " + m_filesSortedOnTimeThenName[i].Second.ToString() + "\r\n");


            return myBuilder.ToString();
        }

        static public void UniTest()
        {
            FileSetWithTimeStamps set1 = new FileSetWithTimeStamps();

            DateTime nowtime = DateTime.Now;

            File.SetLastWriteTime(@"d:\unitest\aaa.txt", nowtime + TimeSpan.FromHours(10));
            File.SetLastWriteTime(@"d:\unitest\bbb.txt", nowtime + TimeSpan.FromHours(8));
            File.SetLastWriteTime(@"d:\unitest\ccc.txt", nowtime + TimeSpan.FromHours(8));
            File.SetLastWriteTime(@"d:\unitest\ddd.txt", nowtime + TimeSpan.FromHours(12));
            File.SetLastWriteTime(@"d:\unitest\eee.txt", nowtime + TimeSpan.FromHours(13));
            File.SetLastWriteTime(@"d:\unitest\fff.txt", nowtime + TimeSpan.FromHours(9));

            set1.Add(@"d:\unitest\aaa.txt");
            set1.Add(@"d:\unitest\bbb.txt");
            set1.Add(@"d:\unitest\ccc.txt");
            set1.Add(@"d:\unitest\ddd.txt");
            set1.Add(@"d:\unitest\eee.txt");
            set1.Add(@"d:\unitest\fff.txt");
            set1.Add(@"d:\unitest\nonexisting.txt");
            MessageBox.Show(set1.Debug());
            File.SetLastWriteTime(@"d:\unitest\ddd.txt", nowtime + TimeSpan.FromHours(8));
            File.SetLastWriteTime(@"d:\unitest\eee.txt", nowtime + TimeSpan.FromHours(14));
            //File.SetLastWriteTime(@"d:\unitest\fff.txt", nowtime + TimeSpan.FromHours(14));

            FileSetWithTimeStamps set2 = set1.Clone();
            MessageBox.Show(set2.Debug()); //only eee should be altered
            set2.Add(@"d:\unitest\ddd.txt");
            set2.Add(@"d:\unitest\eee.txt");
            set2.Add(@"d:\unitest\fff.txt");
            MessageBox.Show(set2.Debug()); //only eee should be altered
            set1.Remove(@"d:\unitest\aaa.txt");
            set1.Remove(@"d:\unitest\bbb.txt");
            set2.Remove(@"d:\unitest\eee.txt");
            set2.Remove(@"d:\unitest\fff.txt");
            MessageBox.Show(set1.Debug()); 
            MessageBox.Show(set2.Debug());

            FileSetWithTimeStamps set3 = FileSetWithTimeStamps.Merge(set1, set2);
            MessageBox.Show(set3.Debug());

        }
    }
}
