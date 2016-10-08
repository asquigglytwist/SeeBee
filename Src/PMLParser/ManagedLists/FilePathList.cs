using SeeBee.FxUtils;

namespace SeeBee.PMLParser.ManagedLists
{
    internal static class FilePathList
    {
        private static IndexedStringCollection knownFilePaths = new IndexedStringCollection();

        #region Methods
        internal static int LocateFilePathInList(string filePath)
        {
            return knownFilePaths.LocateString(filePath);
        }

        internal static int AddFilePathToList(string filePath)
        {
            return knownFilePaths.Add(filePath);
        }

        internal static string GetFilePath(int index)
        {
            return knownFilePaths.StringAt(index);
        }
        #endregion
    }
}
