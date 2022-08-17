using iba.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace iba.Data
{


    [Serializable]
    public class ExternalFileJobData : ICloneable
    {
        #region ICloneable Members
        public enum FileFormatEnum
        {
            TEXTFILE,
            DAS,
            COMTRADE,
            PARQUET
        }
        public FileFormatEnum FileFormat { get; set; }

        public bool DeleteExtFile { get; set; }
        public string TextFileExtension { get; set; }

        [XmlIgnore]
        public string ProcessedFileDirectoryPassword { get; set; }

        public string ProcessedFileDirectoryPasswordCrypted
        {
            get => Crypt.Encrypt(ProcessedFileDirectoryPassword);
            set => ProcessedFileDirectoryPassword = Crypt.Decrypt(value);
        }

        public string ProcessedFileUesername { get; set; }
        public string ProcessedFileTargedDirectory { get; set; }
        public bool MoveExtFile { get; set; }
        public string PdoForReadText { get; set; }

        public ExternalFileJobData()
        {
            DeleteExtFile = true;
            TextFileExtension = "txt";
        }

        public object Clone()
        {
            var jobData = new ExternalFileJobData();
            jobData.DeleteExtFile = DeleteExtFile;
            jobData.FileFormat = FileFormat;
            jobData.TextFileExtension = TextFileExtension;
            jobData.ProcessedFileDirectoryPassword = ProcessedFileDirectoryPassword;
            jobData.ProcessedFileUesername = ProcessedFileUesername;
            jobData.ProcessedFileTargedDirectory = ProcessedFileTargedDirectory;
            jobData.MoveExtFile = MoveExtFile;
            jobData.PdoForReadText = PdoForReadText;
            jobData.ProcessedFileDirectoryPasswordCrypted = ProcessedFileDirectoryPasswordCrypted;

            return jobData;
        }

        #endregion

        public bool IsSame(ExternalFileJobData other)
        {
            var result =
                other.DeleteExtFile == DeleteExtFile &&
                    other.FileFormat == FileFormat &&
                    other.TextFileExtension == TextFileExtension &&
                    other.ProcessedFileDirectoryPassword == ProcessedFileDirectoryPassword &&
                    other.ProcessedFileUesername == ProcessedFileUesername &&
                    other.ProcessedFileTargedDirectory == ProcessedFileTargedDirectory &&
                    other.MoveExtFile == MoveExtFile &&
                    other.PdoForReadText == PdoForReadText &&
                    other.ProcessedFileDirectoryPasswordCrypted == ProcessedFileDirectoryPasswordCrypted;
            
            return result;
        }
    }
}
