using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Remoting
{
    class FileUploaderImpl : MarshalByRefObject, IFileDownload, IDisposable
    {
        public FileUploaderImpl(FileStream file)
        {
            this.file = file;
            buffer = new byte[BLOCKSIZE];
        }


        public void Cancel()
        {
            if (file != null)
                file.Close();
            file = null;
            GC.SuppressFinalize(this);
        }

        public byte[] GetNextPortion()
        {
            if (file == null)
                return null;

            int nrBytesRead = file.Read(buffer, 0, BLOCKSIZE);
            if (nrBytesRead < BLOCKSIZE)
            {
                byte[] smallBuffer = new byte[nrBytesRead];
                if (nrBytesRead > 0)
                    Buffer.BlockCopy(buffer, 0, smallBuffer, 0, nrBytesRead);
                Cancel();
                return smallBuffer;
            }
            else
            {
                if (file.Position == file.Length)
                    Cancel();
                return buffer;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (file != null)
                        file.Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~FileUploaderImpl()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        private static int BLOCKSIZE = 65500;
        private FileStream file;
        private byte[] buffer;
    }
}
