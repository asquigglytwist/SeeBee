using System;
using System.IO;

namespace SeeBee.FxUtils
{
    public static class FSUtils
    {
        static FSUtils()
        {
            PathSeparator = Path.PathSeparator;
            WritableLocationForAllUsers = WritableLocationForCurrentUser = WritableLocationForTempFile = null;
            #region WritableLocationForAllUsers
            // [BIB]:  http://programmers.stackexchange.com/questions/53086/are-regions-an-antipattern-or-code-smell
            var s = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            if (IsWritableLocation(s))
            {
                WritableLocationForAllUsers = s;
            }
            else
            {
                s = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
                if (IsWritableLocation(s))
                {
                    WritableLocationForAllUsers = s;
                }
                else
                {
                    s = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
                    if (IsWritableLocation(s))
                    {
                        WritableLocationForAllUsers = s;
                    }
                }
            }
            #endregion
            #region WritableLocationForCurrentUser
            s = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (IsWritableLocation(s))
            {
                WritableLocationForCurrentUser = s;
            }
            else
            {
                s = Environment.GetEnvironmentVariable("%TEMP%", EnvironmentVariableTarget.Process);
                if (IsWritableLocation(s))
                {
                    WritableLocationForCurrentUser = s;
                }
                else
                {
                    s = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    if (IsWritableLocation(s))
                    {
                        WritableLocationForCurrentUser = s;
                    }
                    else
                    {
                        s = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                        if (IsWritableLocation(s))
                        {
                            WritableLocationForCurrentUser = s;
                        }
                    }
                }
            }
            #endregion
            #region WritableLocationForTempFile
            s = Path.GetDirectoryName(Path.GetTempFileName());
            if (IsWritableLocation(s))
            {
                WritableLocationForTempFile = s;
            }
            else
            {
                s = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
                if (IsWritableLocation(s))
                {
                    WritableLocationForTempFile = s;
                }
            }
            #endregion
        }

        #region Functions
        public static bool FileExists(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            return File.Exists(path);
        }

        public static string PathCombine(string path, string fileNameWithExtension)
        {
            return Path.Combine(path, fileNameWithExtension);
        }

        public static string GetFileName(string path, bool ignoreExtension = false)
        {
            if (ignoreExtension)
            {
                return Path.GetFileNameWithoutExtension(path);
            }
            return Path.GetFileName(path);
        }

        // [BIB]:  http://stackoverflow.com/questions/1410127/c-sharp-test-if-user-has-write-access-to-a-folder
        public static bool IsWritableLocation(string folderPath)
        {
            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
                System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
        #endregion

        #region Properties
        public static char PathSeparator
        {
            get;
            private set;
        }

        public static string CurrentWorkingDirectory
        {
            get
            {
                return Environment.CurrentDirectory;
            }
        }

        public static string WritableLocationForAllUsers
        {
            get;
            private set;
        }

        public static string WritableLocationForCurrentUser
        {
            get;
            private set;
        }

        public static string WritableLocationForTempFile
        {
            get;
            private set;
        }
        #endregion
    }
}
