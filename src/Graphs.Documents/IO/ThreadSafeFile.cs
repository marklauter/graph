using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Graphs.Documents.IO
{
    internal static class ThreadSafeFile
    {
        public static FileStream Open(string fileName, FileMode mode, FileAccess access, FileShare share, TimeSpan timeout)
        {
            var wait = new SpinWait();
            IOException lastIoEx = null;
            var start = DateTime.UtcNow;

            do
            {
                try
                {
                    return new FileStream(fileName, mode, access, share);
                }
                catch (IOException ex)
                {
                    if (!FileIsLocked(ex))
                    {
                        throw;
                    }

                    lastIoEx = ex;
                    wait.SpinOnce();
                }
            }
            while (DateTime.UtcNow - start < timeout);

            throw new TimeoutException(nameof(Open), lastIoEx);
        }

        public static void Delete(string fileName, TimeSpan timeout)
        {
            var wait = new SpinWait();
            IOException lastIoEx = null;
            var start = DateTime.UtcNow;

            do
            {
                try
                {
                    File.Delete(fileName);
                }
                catch (IOException ex)
                {
                    if (!FileIsLocked(ex))
                    {
                        throw;
                    }

                    lastIoEx = ex;
                    wait.SpinOnce();
                }
            }
            while (DateTime.UtcNow - start < timeout);

            throw new TimeoutException(nameof(Open), lastIoEx);
        }

        //const int ERROR_SHARING_VIOLATION = 32;
        //const int ERROR_LOCK_VIOLATION = 33;

        //private static bool IsFileLocked(IOException exception)
        //{
        //    int errorCode = Marshal.GetHRForException(exception) & ((1 << 16) - 1);
        //    return errorCode == ERROR_SHARING_VIOLATION || errorCode == ERROR_LOCK_VIOLATION;
        //}

        private const uint HRFileLocked = 0x80070020;
        private const uint HRPortionOfFileLocked = 0x80070021;

        private static bool FileIsLocked(IOException ex)
        {
            var errorCode = (uint)Marshal.GetHRForException(ex);
            return errorCode == HRFileLocked || errorCode == HRPortionOfFileLocked;
        }
    }
}
