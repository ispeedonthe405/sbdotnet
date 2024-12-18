using System.Collections.ObjectModel;

namespace sbdotnet
{
    public static class Extensions
    {
        ///////////////////////////////////////////////////////////
        #region String

        public static bool IsNull(this string value)
        {
            if (String.IsNullOrEmpty(value)) return true;
            if (String.IsNullOrWhiteSpace(value)) return true;
            if (value.Length == 0) return true;
            return false;
        }

        public static bool IsNotNull(this string value)
        {
            if (String.IsNullOrEmpty(value)) return false;
            if (String.IsNullOrWhiteSpace(value)) return false;
            if (value.Length == 0) return false;
            return true;
        }

        #endregion String
        ///////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////////
        #region List<T>

        /// <summary>
        /// A sanity aid; removes the need to do all that Collection.Count - 1 stuff when indexing numerically
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static int MaxIndex<T>(this List<T> collection)
        {
            return System.Math.Max(0, collection.Count - 1);
        }

        /// <summary>
        /// Extension method for List<T>, where the add only occurs if the value is unique
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="value"></param>
        public static void AddUnique<T>(this List<T> collection, T value)
        {
            if (!collection.Contains(value))
            {
                collection.Add(value);
            }
        }

        /// <summary>
        /// AddRange with per-element filtering of duplicates
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionDest"></param>
        /// <param name="collectionSrc"></param>
        public static void AddUniqueRange<T>(this List<T> collectionDest, List<T> collectionSrc)
        {
            foreach (var t in collectionSrc)
            {
                if (!collectionDest.Contains(t))
                {
                    collectionDest.Add(t);
                }
            }
        }

        #endregion List<T>
        ///////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////////
        #region ObservableCollection<T>

        /// <summary>
        /// A sanity aid; removes the need to do all that Collection.Count - 1 stuff when indexing numerically
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static int MaxIndex<T>(this ObservableCollection<T> collection)
        {
            return System.Math.Max(0, collection.Count - 1);
        }


        /// <summary>
        /// Extension method, alternate version of Add for ObservableCollection<T>.
        /// If the given value is already in the collection, a duplicate is not added.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="value"></param>
        public static void AddUnique<T>(this ObservableCollection<T> collection, T value)
        {
            if (!collection.Contains(value))
            {
                collection.Add(value);
            }
        }


        /// <summary>
        /// Adds AddRange to ObservableCollection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionDest"></param>
        /// <param name="collectionSource"></param>
        public static void AddRange<T>(this ObservableCollection<T> collectionDest, ObservableCollection<T> collectionSource)
        {
            foreach (var t in collectionSource)
            {
                collectionDest.Add(t);
            }
        }

        public static void AddRange<T>(this ObservableCollection<T> collectionDest, List<T> collectionSource)
        {
            foreach (var t in collectionSource)
            {
                collectionDest.Add(t);
            }
        }

        public static void AddRange<T>(this ObservableCollection<T> collectionDest, IEnumerable<T> collectionSource)
        {
            foreach (var t in collectionSource)
            {
                collectionDest.Add(t);
            }
        }

        /// <summary>
        /// Adds AddRange to ObservableCollection, filtering out duplicates
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionDest"></param>
        /// <param name="collectionSource"></param>
        public static void AddRangeUnique<T>(this ObservableCollection<T> collectionDest, ObservableCollection<T> collectionSource)
        {
            foreach (var t in collectionSource)
            {
                collectionDest.AddUnique(t);
            }
        }

        public static void AddRangeUnique<T>(this ObservableCollection<T> collectionDest, List<T> collectionSource)
        {
            foreach (var t in collectionSource)
            {
                collectionDest.AddUnique(t);
            }
        }

        public static void AddRangeUnique<T>(this ObservableCollection<T> collectionDest, IEnumerable<T> collectionSource)
        {
            foreach (var t in collectionSource)
            {
                collectionDest.AddUnique(t);
            }
        }

        #endregion ObservableCollection<T>
        ///////////////////////////////////////////////////////////
    }
}
