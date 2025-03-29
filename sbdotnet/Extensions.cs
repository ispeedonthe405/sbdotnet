using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.Text.Json;


namespace sbdotnet
{
    public static class Extensions
    {
        /////////////////////////////////////////////////////////
        #region rgname

        public static int DigitCount(this int n)
        {
            if (n >= 0)
            {
                if (n < 10) return 1;
                if (n < 100) return 2;
                if (n < 1000) return 3;
                if (n < 10000) return 4;
                if (n < 100000) return 5;
                if (n < 1000000) return 6;
                if (n < 10000000) return 7;
                if (n < 100000000) return 8;
                if (n < 1000000000) return 9;
                return 10;
            }
            else
            {
                if (n > -10) return 2;
                if (n > -100) return 3;
                if (n > -1000) return 4;
                if (n > -10000) return 5;
                if (n > -100000) return 6;
                if (n > -1000000) return 7;
                if (n > -10000000) return 8;
                if (n > -100000000) return 9;
                if (n > -1000000000) return 10;
                return 11;
            }
        }

        #endregion rgname
        /////////////////////////////////////////////////////////


        /////////////////////////////////////////////////////////
        #region DateTime

        public static long ToNixTimestamp_S(this DateTime dt)
        {
            DateTime unixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = dt.ToUniversalTime() - unixEpoch;
            long value = (long)timeSpan.TotalSeconds;
            return value;
        }

        public static long ToNixTimestamp_MS(this DateTime dt)
        {
            DateTime unixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = dt.ToUniversalTime() - unixEpoch;
            long value = (long)timeSpan.TotalMilliseconds;
            return value;
        }


        #endregion DateTime
        /////////////////////////////////////////////////////////




        /////////////////////////////////////////////////////////
        #region Object

        public static T? DeepCopy<T>(this T obj)
        {
            try
            {
                using var stream = new MemoryStream();
                JsonSerializer.Serialize(stream, obj);
                stream.Position = 0;
                return JsonSerializer.Deserialize<T>(stream);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return default;
        }

        #endregion Object
        /////////////////////////////////////////////////////////



        /////////////////////////////////////////////////////////
        #region system.double

        public static double Truncate(this double value, int decimalPlaces = 2)
        {
            return Math.Truncate(value * Math.Pow(10, decimalPlaces)) / Math.Pow(10, decimalPlaces);
        }

        #endregion system.double
        /////////////////////////////////////////////////////////



        ///////////////////////////////////////////////////////////
        #region DataTable

        /// <summary>
        /// Technically this is not an extension method but I can live with that
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataTable CreateDataTableFromType<T>() where T : class
        {
            DataTable dt = new DataTable();
            Type t = typeof(T);
            foreach (PropertyInfo property in t.GetProperties())
            {
                dt.Columns.Add(new DataColumn(property.Name, property.PropertyType));
            }
            return dt;
        }

        /// <summary>
        /// Technically this is not an extension method but I can live with that
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="dataList"></param>
        public static void AddListToDataTable<T>(DataTable table, List<T> dataList) where T : class
        {
            foreach (T item in dataList)
            {
                DataRow newRow = table.NewRow();
                foreach (DataColumn column in table.Columns)
                {
                    var property = typeof(T).GetProperty(column.ColumnName);
                    if (property != null)
                    {
                        newRow[column] = property.GetValue(item);
                    }
                }
                table.Rows.Add(newRow);
            }
        }

        #endregion DataTable
        ///////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////////
        #region Color

        public static string ToJavascriptRGB(this System.Drawing.Color color)
        {
            return $"rgb({color.R}, {color.G}, {color.B})";
        }

        #endregion Color
        ///////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////////
        #region String

        public static bool IsNull(this string? value)
        {
            if (value is null) return true;
            if (String.IsNullOrEmpty(value)) return true;
            if (String.IsNullOrWhiteSpace(value)) return true;
            if (value.Length == 0) return true;
            return false;
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
        public static void AddUniqueRange<T>(this List<T> collectionDest, IEnumerable<T> collectionSrc)
        {
            foreach (var t in collectionSrc)
            {
                if (!collectionDest.Contains(t))
                {
                    collectionDest.Add(t);
                }
            }
        }

        /// <summary>
        /// Converts a List<string> into a CSV string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToCsvString(this List<string> source)
        {
            string converted = string.Empty;
            for(int i = 0; i < source.Count; i++)
            {
                if(i < source.MaxIndex())
                {
                    converted += $"{source[i]},";
                }
                else
                {
                    converted += source[i];
                }
            }
            return converted;
        }

        public static string ToJson<T>(this List<T> collection)
        {
            try
            {
                return JsonSerializer.Serialize(collection);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return string.Empty;
        }

        public static void FromJson<T>(this List<T> collection, string json)
        {
            try
            {
                collection = JsonSerializer.Deserialize<List<T>?>(json) ?? [];
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
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
        public static void AddRangeUnique<T>(this ObservableCollection<T> collectionDest, IEnumerable<T> collectionSource)
        {
            foreach (var t in collectionSource)
            {
                collectionDest.AddUnique(t);
            }
        }

        public static string ToCsvString(this ObservableCollection<string> source)
        {
            string converted = string.Empty;
            for (int i = 0; i < source.Count; i++)
            {
                if (i < source.MaxIndex())
                {
                    converted += $"{source[i]},";
                }
                else
                {
                    converted += source[i];
                }
            }
            return converted;
        }

        public static string ToJson<T>(this ObservableCollection<T> collection)
        {
            try
            {
                return JsonSerializer.Serialize(collection);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return string.Empty;
        }

        public static void FromJson<T>(this ObservableCollection<T> collection, string json)
        {
            try
            {
                collection = JsonSerializer.Deserialize<ObservableCollection<T>?>(json) ?? [];
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        #endregion ObservableCollection<T>
        ///////////////////////////////////////////////////////////
    }
}
