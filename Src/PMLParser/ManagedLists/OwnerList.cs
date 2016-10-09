using SeeBee.FxUtils;

namespace SeeBee.PMLParser.ManagedLists
{
    internal static class OwnerList
    {
        private static IndexedStringCollection knownOwners = new IndexedStringCollection();

        #region Static Constructor
        static OwnerList()
        {
            OwnerList.AddOwnerToList("NT AUTHORITY\\SYSTEM");
        }
        #endregion

        #region Methods
        internal static int LocateOwnerInList(string owner)
        {
            return knownOwners.LocateString(owner);
        }

        internal static int AddOwnerToList(string owner)
        {
            return knownOwners.Add(owner);
        }

        internal static string GetOwnerName(int index)
        {
            return knownOwners.StringAt(index);
        }
        #endregion
    }
}
