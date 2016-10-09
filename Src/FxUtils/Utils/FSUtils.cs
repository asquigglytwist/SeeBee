using System;
using System.IO;

namespace SeeBee.FxUtils.Utils
{
    /// <summary>
    /// FileSystem Utils - A collection of static methods to assist with FileSystem operations.
    /// </summary>
    public static class FSUtils
    {
        #region Static Constructor
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
            string tempFileName = Path.GetTempFileName();
            s = Path.GetDirectoryName(tempFileName);
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
            File.Delete(tempFileName);
            #endregion
        } 
        #endregion

        #region Functions
        /// <summary>
        /// Checks if a file exists and optionally, throws a FileNotFoundException.
        /// </summary>
        /// <param name="path">Complete path to the file, which is being checked.</param>
        /// <param name="errorMessageOnThrow">Optional parameter; If set and file is not found, causes a FileNotFoundException to be thrown with the provided error message.</param>
        /// <returns>True, if the file exists; False otherwise.</returns>
        public static bool FileExists(string path, string errorMessageOnThrow = null)
        {
            bool doesFileExist = File.Exists(path);
            if ((!doesFileExist) && (!string.IsNullOrWhiteSpace(errorMessageOnThrow)))
            {
                throw new FileNotFoundException(errorMessageOnThrow, path);
            }
            return doesFileExist;
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="path">Complete path to the file, which is to be deleted.</param>
        public static void FileDelete(string path)
        {
            File.Delete(path);
        }

        /// <summary>
        /// Creates a complete path out of the provided path and filename.
        /// </summary>
        /// <param name="path">Path where the file, either resides or is supposed to.</param>
        /// <param name="fileNameWithExtension">Complete filename along with its extension.</param>
        /// <returns>The combined path, created from the inputs, as a string.</returns>
        public static string PathCombine(string path, string fileNameWithExtension)
        {
            return Path.Combine(path, fileNameWithExtension);
        }

        /// <summary>
        /// Extracts the file name from provided path.
        /// </summary>
        /// <param name="path">Complete path to the file.</param>
        /// <param name="ignoreExtension">Optional argument, if True, will extract the file name without its extension.</param>
        /// <returns>The extracted file name, as a string.</returns>
        public static string GetFileName(string path, bool ignoreExtension = false)
        {
            if (ignoreExtension)
            {
                return Path.GetFileNameWithoutExtension(path);
            }
            return Path.GetFileName(path);
        }

        /// <summary>
        /// Attempts to create an output file name from the provided input, in a way that doesn't conflict with existing files.
        /// </summary>
        /// <param name="input">Input file for which an output file name is being constructed.</param>
        /// <param name="extension">An optional parameter, a string prefixed with period, which will be used in the output file's name.</param>
        /// <returns></returns>
        public static string CreateOuputFileNameFromInput(string input, string extension = null)
        {
            // [BIB]:  http://stackoverflow.com/questions/674479/how-do-i-get-the-directory-from-a-files-full-path
            string fileName = GetFileName(input, true), location = new FileInfo(input).Directory.FullName;
            // [BIB]:  http://stackoverflow.com/questions/5608980/how-to-ensure-a-timestamp-is-always-unique
            fileName += DateTime.UtcNow.Ticks;
            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = Path.GetExtension(input);
            }
            if (!IsWritableLocation(location))
            {
                if (IsWritableLocation(WritableLocationForTempFile))
                {
                    location = WritableLocationForTempFile;
                }
                else if (IsWritableLocation(WritableLocationForCurrentUser))
                {
                    location = WritableLocationForCurrentUser;
                }
                else if (IsWritableLocation(WritableLocationForAllUsers))
                {
                    location = WritableLocationForAllUsers;
                }
            }
            string completeFileName = PathCombine(location, fileName + extension);
            if (FileExists(completeFileName))
            {
                completeFileName = PathCombine(location, (fileName + NumberUtils.GetRandomInt() + extension));
                if (FileExists(completeFileName))
                {
                    // If after this attempt there is a conflicting file, let there be VelociRaptors...
                    completeFileName = PathCombine(location, (fileName + NumberUtils.GetRandomInt() + extension));
                }
            }
            return completeFileName;
        }

        // [BIB]:  http://stackoverflow.com/questions/1410127/c-sharp-test-if-user-has-write-access-to-a-folder
        /// <summary>
        /// Checks if a provided folder path is writable by the caller.
        /// </summary>
        /// <param name="folderPath">Complete path to the folder, for which writability i.e., write permissions, are being checked.</param>
        /// <returns>True if writable; False otherwise.</returns>
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
        /// <summary>
        /// The default Path Separator character used in the environment.  Same as Path.PathSeparator.
        /// </summary>
        public static char PathSeparator
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Current WorkingDirectory.  Same as Environment.CurrentDirectory.
        /// </summary>
        public static string CurrentWorkingDirectory
        {
            get
            {
                return Environment.CurrentDirectory;
            }
        }

        /// <summary>
        /// Gets the writable location for all users.  Will be one of CommonApplicationData or CommonDocuments or CommonDesktopDirectory, in that order.  Is null if none of the locations is writable.
        /// </summary>
        public static string WritableLocationForAllUsers
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the writable location for all users.  Will be one of %AppData%, %TEMP%, My Documents or Desktop directories, in that order.  Is null if none of the locations is writable.
        /// </summary>
        public static string WritableLocationForCurrentUser
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the writable location for temp files.  Will be either Path.GetTempFileName() or Environment.SpecialFolder.InternetCache.  Is null if none of the locations is writable.
        /// </summary>
        public static string WritableLocationForTempFile
        {
            get;
            private set;
        }
        #endregion
    }
}
