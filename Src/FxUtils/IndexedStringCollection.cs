using System;
using System.Collections.Generic;

namespace SeeBee.FxUtils
{
    public class IndexedStringCollection : IEnumerable<KeyValuePair<string, int>>
    {
        #region Properties And Constants
        public const int IndexOfUnknown = -1;
        protected internal Dictionary<string, int> IndexedStringDictionary { get; set; }
        #endregion

        #region Constructor
        public IndexedStringCollection()
        {
            IndexedStringDictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        }
        #endregion

        #region Public Methods
        public int LocateString(string stringToFind)
        {
            if (string.IsNullOrWhiteSpace(stringToFind))
            {
                return IndexOfUnknown;
            }
            int indexOfString;
            if (!IndexedStringDictionary.TryGetValue(stringToFind, out indexOfString))
            {
                indexOfString = IndexOfUnknown;
            }
            return indexOfString;
        }

        public string StringAt(int index)
        {
            foreach (string s in IndexedStringDictionary.Keys)
            {
                if (IndexedStringDictionary[s] == index)
                {
                    return s;
                }
            }
            return null;
        }

        public int Add(string stringToAdd, bool throwOnFail = false)
        {
            if (string.IsNullOrWhiteSpace(stringToAdd))
            {
                return IndexOfUnknown;
            }
            int indexOfString = LocateString(stringToAdd);
            if (indexOfString > IndexOfUnknown)
            {
                if (throwOnFail)
                {
                    throw new ArgumentException(string.Format("String {0} already exists.", stringToAdd));
                }
                return indexOfString;
            }
            IndexedStringDictionary[stringToAdd] = IndexedStringDictionary.Count;
            return IndexedStringDictionary.Count - 1;
        }
        #endregion

        #region IEnumberable
        IEnumerator<KeyValuePair<string, int>> IEnumerable<KeyValuePair<string, int>>.GetEnumerator()
        {
            return IndexedStringDictionary.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return IndexedStringDictionary.GetEnumerator();
        }
        #endregion
    }
}
