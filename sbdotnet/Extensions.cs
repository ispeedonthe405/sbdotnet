using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;


namespace sbdotnet
{
    public static class Extensions
    {
        static CultureInfo sgCulture = new CultureInfo("en-SG");

        /////////////////////////////////////////////////////////
        #region numbers

        public static int DigitCount(this Int16 n)
        {
            return n.ToString().Replace("-", "").Length;
        }

        public static int DigitCount(this UInt16 n)
        {
            return n.ToString().Length;
        }

        public static int DigitCount(this Int32 n)
        {
            return n.ToString().Replace("-", "").Length;
        }

        public static int DigitCount(this UInt32 n)
        {
            return n.ToString().Length;
        }

        public static int DigitCount(this Int64 n)
        {
            return n.ToString().Replace("-", "").Length;
        }

        public static int DigitCount(this UInt64 n)
        {
            return n.ToString().Length;
        }

        public static string ToCurrency(this Double value)
        {
            return string.Create(sgCulture, $"{value:C2}");
            //return $"{value:C2}";
        }

        public static string ToCurrencyAC(this Double value)
        {
            return string.Create(sgCulture, $"{value:C2}");
            //return $"{value:C2}";
        }

        public static string ToCurrency(this UInt32 value)
        {
            return $"{value:C2}";
        }

        public static string ToCurrency(this UInt64 value)
        {
            return $"{value:C2}";
        }

        public static string Trunc2(this Double value)
        {
            return $"{value:N2}";
        }

        public static string Round2(this Double value)
        {
            double rounded = Math.Round(value, 2);
            return $"{rounded:N2}";
        }

        public static string ToPercent(this Double value)
        {
            return $"{value:P2}";
        }

        public static string ToPercent2(this Double value)
        {
            double rounded = Math.Round(value, 2);
            return $"{rounded:P2}";
        }

        #endregion numbers
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

        public static string TrimToAlphaNumeric(this string value)
        {
            StringBuilder sb = new();

            var array = value.ToCharArray();
            foreach (var ch in array)
            {
                if (Char.IsLetterOrDigit(ch))
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString();
        }

        public static bool IsNull(this string? value)
        {
            if (value is null) return true;
            if (String.IsNullOrEmpty(value)) return true;
            if (String.IsNullOrWhiteSpace(value)) return true;
            if (value.Length == 0) return true;
            return false;
        }

        public static bool ContainsDigits(this string value)
        {
            var chars = value.ToCharArray();
            foreach (char c in chars)
            {
                if (c.Equals('0')) return true;
                if (c.Equals('1')) return true;
                if (c.Equals('2')) return true;
                if (c.Equals('3')) return true;
                if (c.Equals('4')) return true;
                if (c.Equals('5')) return true;
                if (c.Equals('6')) return true;
                if (c.Equals('7')) return true;
                if (c.Equals('8')) return true;
                if (c.Equals('9')) return true;
            }
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
        public static void AddRangeUnique<T>(this List<T> collectionDest, IEnumerable<T> collectionSrc)
        {
            foreach (var t in collectionSrc)
            {
                if (!collectionDest.Contains(t))
                {
                    collectionDest.Add(t);
                }
            }
        }

        public static string ToCsvString<T>(this List<T> source)
        {
            string csv = string.Empty;
            for (int i = 0; i < source.Count; i++)
            {
                if (i < source.MaxIndex())
                {
                    csv += $"{source[i]},";
                }
                else
                {
                    csv += source[i];
                }
            }
            return csv;
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
            string csv = string.Empty;
            for (int i = 0; i < source.Count; i++)
            {
                if (i < source.MaxIndex())
                {
                    csv += $"{source[i]},";
                }
                else
                {
                    csv += source[i];
                }
            }
            return csv;
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
