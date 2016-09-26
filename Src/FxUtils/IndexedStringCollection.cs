using System;
using System.Collections.Generic;

namespace SeeBee.FxUtils
{
    public class IndexedStringCollection : IEnumerable<KeyValuePair<string, int>>
    {
        #region Properties
        public int LastAddedIndex { get; protected internal set; }
        protected internal Dictionary<string, int> IndexedStringDictionary { get; protected internal set; }
        #endregion

        #region Constructor
        public IndexedStringCollection()
        {
            LastAddedIndex = -1;
            IndexedStringDictionary = new Dictionary<string, int>();
        }
        #endregion

        #region Public Methods
        public int LocateString(string stringToFind)
        {
            int indexOfString;
            if (!IndexedStringDictionary.TryGetValue(stringToFind, out indexOfString))
            {
                indexOfString = -1;
            }
            return indexOfString;
        }

        public int Add(string stringToAdd, bool throwOnFail = false)
        {
            int indexOfString = LocateString(stringToAdd);
            if (indexOfString > -1)
            {
                if (throwOnFail)
                {
                    throw new ArgumentException(string.Format("String {0} already exists.", stringToAdd));
                }
                return indexOfString;
            }
            IndexedStringDictionary[stringToAdd] = ++LastAddedIndex;
            return LastAddedIndex;
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
