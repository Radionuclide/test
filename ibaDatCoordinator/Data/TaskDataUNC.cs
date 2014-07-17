using System;
using System.Collections.Generic;
using System.Text;
using iba.Utility;
using System.Xml.Serialization;

namespace iba.Data
{
    [Serializable]
    public abstract class TaskDataUNC : TaskData
    {
        public enum OutputLimitChoiceEnum {None,LimitDirectories,LimitDiskspace,SaveFreeSpace}
        private OutputLimitChoiceEnum m_outputLimitChoice;
        public OutputLimitChoiceEnum OutputLimitChoice
        {
            get { return m_outputLimitChoice; }
            set { m_outputLimitChoice = value; }
        }

        public bool UsesQuota
        {
            get { 
                return m_outputLimitChoice == OutputLimitChoiceEnum.SaveFreeSpace 
                || m_outputLimitChoice == OutputLimitChoiceEnum.LimitDiskspace; 
            }
        }

        private bool m_overwriteFiles;
        public bool OverwriteFiles
        {
            get { return m_overwriteFiles; }
            set { m_overwriteFiles = value; }
        }

        private bool m_infoFieldForOutputFileRemoveBlanksEnd;
        public bool InfoFieldForOutputFileRemoveBlanksEnd
        {
            get { return m_infoFieldForOutputFileRemoveBlanksEnd; }
            set { m_infoFieldForOutputFileRemoveBlanksEnd = value; }
        }

        private bool m_infoFieldForOutputFileRemoveBlanksAll;
        public bool InfoFieldForOutputFileRemoveBlanksAll
        {
            get { return m_infoFieldForOutputFileRemoveBlanksAll; }
            set { m_infoFieldForOutputFileRemoveBlanksAll = value; }
        }

        private bool m_infoFieldForSubdirRemoveBlanksEnd;
        public bool InfoFieldForSubdirRemoveBlanksEnd
        {
            get { return m_infoFieldForSubdirRemoveBlanksEnd; }
            set { m_infoFieldForSubdirRemoveBlanksEnd = value; }
        }

        private bool m_infoFieldForSubdirRemoveBlanksAll;
        public bool InfoFieldForSubdirRemoveBlanksAll
        {
            get { return m_infoFieldForSubdirRemoveBlanksAll; }
            set { m_infoFieldForSubdirRemoveBlanksAll = value; }
        }

        public TaskDataUNC(ConfigurationData parent) : base(parent) 
        {
            m_pass = "";
            m_username = "";
            m_destinationMapUNC = "";
            m_numbFolders = 10;
            m_quota = 1024;
            m_quotaFree = 1024;
            m_outputLimitChoice = OutputLimitChoiceEnum.LimitDirectories;
            m_overwriteFiles = false;
            m_subfolderChoice = SubfolderChoice.DAY;
            m_doDirCleanup = false;
            m_useDatModTimeForDirs = false;
            m_copyDatModTime = false;
            m_useInfoFieldForOutputFile = false;
            m_infoFieldForOutputFile = "";
            m_infoFieldForOutputFileStart = 0;
            m_infoFieldForOutputFileLength = 0;
            m_infoFieldForOutputFileRemoveBlanksEnd = false;
            m_infoFieldForOutputFileRemoveBlanksAll = false;
            m_infoFieldForSubdir = "";
            m_infoFieldForSubdirStart = 0;
            m_infoFieldForSubdirLength = 0;
            m_infoFieldForSubdirRemoveBlanksEnd = false;
            m_infoFieldForSubdirRemoveBlanksAll = false;
            m_splitSubdirs = false;
            m_year4Chars = false;
        }

        protected uint m_numbFolders;
        public uint SubfoldersNumber
        {
            get { return m_numbFolders; }
            set { m_numbFolders = value; }
        }

        public enum SubfolderChoice { SAME, NONE, HOUR, DAY, WEEK, MONTH, INFOFIELD };
        protected SubfolderChoice m_subfolderChoice;
        public SubfolderChoice Subfolder
        {
            get { return m_subfolderChoice; }
            set { m_subfolderChoice = value; }
        }

        private bool m_splitSubdirs;
        public bool SplitSubdirs
        {
            get { return m_splitSubdirs; }
            set { m_splitSubdirs = value; }
        }
        private bool m_year4Chars;
        public bool Year4Chars
        {
            get { return m_year4Chars; }
            set { m_year4Chars = value; }
        }

        protected uint m_quota;
        /// <summary>
        /// Maximum used space in MB usually
        /// </summary>
        public uint Quota
        {
            get { return m_quota; }
            set { m_quota = value; }
        }

        protected uint m_quotaFree;
        /// <summary>
        /// Minimum of free space in MB usually
        /// </summary>
        public uint QuotaFree
        {
            get { return m_quotaFree; }
            set { m_quotaFree = value; }
        }

        public TaskDataUNC() : this(null) { }

        protected string m_destinationMap;
        public string DestinationMap
        {
            get { return m_destinationMap; }
            set { m_destinationMap = value; }
        }

        protected string m_destinationMapUNC;
        public string DestinationMapUNC
        {
            get { return m_destinationMapUNC; }
            set { m_destinationMapUNC = value; }
        }

        protected bool m_useDatModTimeForDirs;
        public bool UseDatModTimeForDirs
        {
            get { return m_useDatModTimeForDirs; }
            set { m_useDatModTimeForDirs = value; }
        }

        protected bool m_copyDatModTime;
        public bool CopyModTime
        {
            get { return m_copyDatModTime; }
            set { m_copyDatModTime = value; }
        }

        protected bool m_useInfoFieldForOutputFile;
        public bool UseInfoFieldForOutputFile
        {
            get { return m_useInfoFieldForOutputFile; }
            set { m_useInfoFieldForOutputFile = value; }
        }

        protected string m_infoFieldForOutputFile;
        public string InfoFieldForOutputFile
        {
            get { return m_infoFieldForOutputFile; }
            set { m_infoFieldForOutputFile = value; }
        }

        protected int m_infoFieldForOutputFileStart;
        public int InfoFieldForOutputFileStart
        {
            get { return m_infoFieldForOutputFileStart; }
            set { m_infoFieldForOutputFileStart = value; }
        }

        protected int m_infoFieldForOutputFileLength;
        public int InfoFieldForOutputFileLength
        {
            get { return m_infoFieldForOutputFileLength; }
            set { m_infoFieldForOutputFileLength = value; }
        }

        protected string m_infoFieldForSubdir;
        public string InfoFieldForSubdir
        {
            get { return m_infoFieldForSubdir; }
            set { m_infoFieldForSubdir = value; }
        }

        protected int m_infoFieldForSubdirStart;
        public int InfoFieldForSubdirStart
        {
            get { return m_infoFieldForSubdirStart; }
            set { m_infoFieldForSubdirStart = value; }
        }

        protected int m_infoFieldForSubdirLength;
        public int InfoFieldForSubdirLength
        {
            get { return m_infoFieldForSubdirLength; }
            set { m_infoFieldForSubdirLength = value; }
        }

        public void UpdateUNC()
        {
            m_destinationMapUNC = Shares.PathToUnc(m_destinationMap, false);
        }

        public void CopyUNCData(TaskDataUNC uncdat)
        {
            uncdat.m_destinationMap = m_destinationMap;
            uncdat.m_numbFolders = m_numbFolders;
            uncdat.m_username = m_username;
            uncdat.m_pass = m_pass;
            uncdat.m_destinationMapUNC = m_destinationMapUNC;
            uncdat.m_quota = m_quota;
            uncdat.m_quotaFree = m_quotaFree;
            uncdat.m_outputLimitChoice = m_outputLimitChoice;
            uncdat.m_overwriteFiles = m_overwriteFiles;
            uncdat.m_subfolderChoice = m_subfolderChoice;
            uncdat.m_useDatModTimeForDirs = m_useDatModTimeForDirs;
            uncdat.m_copyDatModTime = m_copyDatModTime;
            uncdat.m_useInfoFieldForOutputFile = m_useInfoFieldForOutputFile;
            uncdat.m_infoFieldForOutputFile = m_infoFieldForOutputFile;
            uncdat.m_infoFieldForOutputFileStart = m_infoFieldForOutputFileStart;
            uncdat.m_infoFieldForOutputFileLength = m_infoFieldForOutputFileLength;
            uncdat.m_infoFieldForOutputFileRemoveBlanksAll = m_infoFieldForOutputFileRemoveBlanksAll;
            uncdat.m_infoFieldForOutputFileRemoveBlanksEnd = m_infoFieldForOutputFileRemoveBlanksEnd;
            uncdat.m_infoFieldForSubdir = m_infoFieldForSubdir;
            uncdat.m_infoFieldForSubdirStart = m_infoFieldForSubdirStart;
            uncdat.m_infoFieldForSubdirLength = m_infoFieldForSubdirLength;
            uncdat.m_infoFieldForSubdirRemoveBlanksAll = m_infoFieldForSubdirRemoveBlanksAll;
            uncdat.m_infoFieldForSubdirRemoveBlanksEnd = m_infoFieldForSubdirRemoveBlanksEnd;
            uncdat.m_splitSubdirs = m_splitSubdirs;
            uncdat.m_year4Chars = m_year4Chars;
        }

        public bool UNCDataIsSame(TaskDataUNC other)
        {
            return
            other.m_destinationMap == m_destinationMap &&
            other.m_numbFolders == m_numbFolders &&
            other.m_username == m_username &&
            other.m_pass == m_pass &&
                //other.m_destinationMapUNC == m_destinationMapUNC &&
            other.m_quota == m_quota &&
            other.m_quotaFree == m_quotaFree &&
            other.m_outputLimitChoice == m_outputLimitChoice &&
            other.m_overwriteFiles == m_overwriteFiles &&
            other.m_subfolderChoice == m_subfolderChoice &&
            other.m_useDatModTimeForDirs == m_useDatModTimeForDirs &&
            other.m_copyDatModTime == m_copyDatModTime &&
            other.m_useInfoFieldForOutputFile == m_useInfoFieldForOutputFile &&
            other.m_infoFieldForOutputFile == m_infoFieldForOutputFile &&
            other.m_infoFieldForOutputFileStart == m_infoFieldForOutputFileStart &&
            other.m_infoFieldForOutputFileLength == m_infoFieldForOutputFileLength &&
            other.m_infoFieldForOutputFileRemoveBlanksAll == m_infoFieldForOutputFileRemoveBlanksAll &&
            other.m_infoFieldForOutputFileRemoveBlanksEnd == m_infoFieldForOutputFileRemoveBlanksEnd &&
            other.m_infoFieldForSubdir == m_infoFieldForSubdir &&
            other.m_infoFieldForSubdirStart == m_infoFieldForSubdirStart &&
            other.m_infoFieldForSubdirLength == m_infoFieldForSubdirLength &&
            other.m_infoFieldForSubdirRemoveBlanksAll == m_infoFieldForSubdirRemoveBlanksAll &&
            other.m_infoFieldForSubdirRemoveBlanksEnd == m_infoFieldForSubdirRemoveBlanksEnd &&
            other.m_splitSubdirs == m_splitSubdirs &&
            other.m_year4Chars == m_year4Chars;
        }

        public String GetSubDir(DateTime dt)
        {
            return GetSubDir(this.Subfolder, dt, this.Year4Chars, this.SplitSubdirs);
        }

        public static String GetSubDir(SubfolderChoice choice, DateTime dt, bool fourYears, bool splitDirs)
        {
            string yearString = fourYears ? "yyyy" : "yy";
            string res = null;
            switch(choice)
            {
                case TaskDataUNC.SubfolderChoice.HOUR:
                    res = dt.ToString(yearString + "'~'MM'~'dd'~'HH");
                    break;
                case TaskDataUNC.SubfolderChoice.DAY:
                    res = dt.ToString(yearString + "'~'MM'~'dd");
                    break;
                case TaskDataUNC.SubfolderChoice.MONTH:
                    res = dt.ToString(yearString + "'~'MM");
                    break;
                case TaskDataUNC.SubfolderChoice.WEEK:
                    {
                        int weekNr = GetWeekNumber(dt);
                        if(fourYears)
                            res = dt.Year.ToString("d4") + "~" + weekNr.ToString("d2");
                        else
                            res = (dt.Year - 2000).ToString("d2") + "~" + weekNr.ToString("d2");
                        break;
                    }
                default:
                    return null;
            }
            if(splitDirs)
                return res.Replace("~", @"\");
            else
                return res.Replace("~","");
        }

        private static int GetWeekNumber(DateTime date)
        {
            // Get jan 1st of the year
            DateTime startOfYear = new DateTime(date.Year, 1, 1);
            // Get dec 31st of the year
            DateTime endOfYear = new DateTime(date.Year, 12, 31);

            // ISO 8601 weeks start with Monday 
            // The first week of a year includes the first Thursday 
            // DayOfWeek returns 0 for sunday up to 6 for saterday
            int[] iso8601Correction = { 6, 7, 8, 9, 10, 4, 5 };
            int nds = date.Subtract(startOfYear).Days + iso8601Correction[(int)startOfYear.DayOfWeek];
            int wk = nds / 7;
            if(wk == 0)
                // Return weeknumber of dec 31st of the previous year
                return GetWeekNumber(new DateTime(date.Year - 1, 12, 31));
            else if((wk == 53) && (endOfYear.DayOfWeek < DayOfWeek.Thursday))
                // If dec 31st falls before thursday it is week 01 of next year
                return 1;
            else
                return wk;
        }

        protected string m_username;
        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
        }

        protected string m_pass;

        [XmlIgnore]
        public string Password
        {
            get { return m_pass; }
            set { m_pass = value; }
        }

        public string PasswordCrypted
        {
            get { return Crypt.Encrypt(m_pass); }
            set { m_pass = Crypt.Decrypt(value); }
        }

        [XmlIgnore]
        public bool DoDirCleanupNow
        {
            get { return m_doDirCleanup; }
            set { m_doDirCleanup = value; }
        }
        private bool m_doDirCleanup;
    }
}
