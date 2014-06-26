using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace UtilsLib.ExtensionMethods
{
    /// <summary>
    /// Extension Methods for DataSets, DataTables, and DataRows
    /// </summary>
    public static class DataExtensions
    {
        #region DataSet Methods
        /// <summary>
        /// Returns all rows in a DataSet without the risk of a null exception.
        /// </summary>
        /// <param name="ds">DataSet to get all rows from.</param>
        /// <returns>Array of DataRows.</returns>
        public static IEnumerable<DataRow> GetRowsFromDataSet(this DataSet ds)
        {
            List<DataRow> rows = new List<DataRow>();

            if (ds.HasAtLeastOneRow())
            {
                rows.AddRange(from DataTable table in ds.Tables where table.Rows != null from DataRow row in table.Rows select row);
            }

            return rows.ToArray();
        }

        /// <summary>
        /// Returns the first row without the risk of a null exception.
        /// </summary>
        /// <param name="ds">DataSet to get first row from.</param>
        /// <returns>DataRow if available, otherwise null.</returns>
        public static DataRow GetFirstRowFromDataSet(this DataSet ds)
        {
            return GetRowsFromDataSet(ds).FirstOrDefault();
        }

        /// <summary>
        /// Determines whether the DataSet has at least one row
        /// </summary>
        /// <param name="ds">DataSet to determine whether has at least one row</param>
        /// <returns>True, if DataSet has at least one row</returns>
        public static bool HasAtLeastOneRow(this DataSet ds)
        {
            return ds != null &&
                   ds.Tables.Count > 0 &&
                   ds.Tables[0].HasAtLeastOneRow();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this DataSet ds)
        {
            return !HasAtLeastOneRow(ds);
        }

        /// <param name="set"></param>
        /// <param name="fieldName"></param>
        /// <param name="dontThrowException">If true, won't throw exception if table doesn't contain column</param>
        /// <returns>Value of table column</returns>
        public static T GetValueFromFirstRow<T>(this DataSet set, string fieldName, bool dontThrowException = false)
        {
            DataRow row = set.GetFirstRowFromDataSet();

            return row.GetValue<T>(fieldName, dontThrowException);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="a"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static string GetKeyValueFromDataSet(this DataSet ds, string a, string defaultVal = "")
        {
            try
            {
                if (ds == null)
                {
                    return defaultVal;
                }

                if (ds.Tables[0].Rows.Count <= 0)
                {
                    return defaultVal;
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string tmp = dr["keyn"].ToString();

                    if (tmp == a)
                    {
                        return dr["value"].ToString();
                    }
                }

                return defaultVal;
            }
            catch (Exception)
            {
                return defaultVal;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="col"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool MatchAttributeFromDataSet(this DataSet ds, string col, string val)
        {
            try
            {
                if (ds == null)
                {
                    return false;
                }

                if (ds.Tables[0].Rows.Count <= 0)
                {
                    return false;
                }

                return ds.Tables[0].Rows.Cast<DataRow>().Any(dr => dr[col].ToString() == val);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static string GetAttributeFromDataSet(this DataSet ds, string attr)
        {
            try
            {
                if (ds == null)
                {
                    return string.Empty;
                }

                if (ds.Tables[0].Rows.Count <= 0)
                {
                    return string.Empty;
                }

                if (ds.Tables[0].Rows[0][attr] == null)
                {
                    return string.Empty;
                }

                return ds.Tables[0].Rows[0][attr].ToString().Trim();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #endregion

        #region DataTable Methods
        /// <summary>
        /// Determines whether the DataTable has at least one row
        /// </summary>
        /// <param name="dt">DataTable to determine whether has at least one row</param>
        /// <returns>True, if DataTable has at least one row</returns>
        public static bool HasAtLeastOneRow(this DataTable dt)
        {
            return dt != null &&
                   dt.Rows != null &&
                   dt.Rows.Count > 0;
        }
        #endregion

        #region DataRow Methods

        /// <summary>
        /// Gets the value of DataRow field, if it is DBNull, return the default value of that
        /// </summary>
        /// <param name="row">DataRow from database</param>
        /// <param name="fieldName">The column name where the data is located</param>
        /// <param name="useStringValueForEnum">If true, will use the enum string value to parse the enum</param>
        /// <param name="useBooleanEnglishConstants">If true, will use boolean english constants for conversion, i.e "True", "False", "Y", "N", "Yes", "No" etc...</param>
        /// <param name="dontThrowException">If true, won't throw exception if table doesn't contain column</param>
        /// <returns>Value of table column</returns>
        public static T GetValue<T>(this DataRow row, 
                                    string fieldName, 
                                    bool useStringValueForEnum      = false, 
                                    bool useBooleanEnglishConstants = false, 
                                    bool dontThrowException         = false)
        {
            if (!row.Table.Columns.Contains(fieldName))
            {
                //Don't Throw Exception was specified
                //Returning default value
                if (dontThrowException)
                {
                    return default(T);
                }

                throw new Exception(string.Format("Invalid Mapping of Column for column {0}", fieldName));
            }

            if (DBNull.Value.Equals(row[fieldName]))
            {
                return default(T);
            }
            try
            {
                Type type = typeof(T);

                if (type == typeof(bool) || type == typeof(bool?))
                {
                    bool temp;
                    if (useBooleanEnglishConstants)
                    {
                        bool parsedValue;
                        if (BooleanExtensions.TryParseBooleanConstants(row[fieldName].ToString(), out parsedValue))
                        {
                            temp = parsedValue;
                        }
                        else
                        {
                            temp = false;
                        }
                    }
                    else
                    {
                        temp = Convert.ToBoolean(row[fieldName]);
                    }
                    T booleanValue = (T)(object)temp;
                    return booleanValue;
                }

                if (type == typeof(int) || type == typeof(int?))
                {
                    int temp = Convert.ToInt32(row[fieldName]);
                    T intValue = (T)(object)temp;
                    return intValue;
                }

                if (type.BaseType == typeof (Enum))
                {
                    if (useStringValueForEnum)
                    {
                        string fieldValue = row[fieldName].ToString();

                        return EnumerationsHelper.ConvertFromString<T>(fieldValue);
                    }
                    int temp = Convert.ToInt32(row[fieldName]);
                    T intValue = (T)(object)temp;
                    return intValue;
                }

                if (type == typeof(decimal) || type == typeof(decimal?))
                {
                    decimal temp = Convert.ToDecimal(row[fieldName]);
                    T decimalValue = (T)(object)temp;
                    return decimalValue;
                }

                //added case to deal solely with nullable enums since they make the iceweasel cower in fear. 
                Type typeOfNullable = Nullable.GetUnderlyingType(type);

                if (typeOfNullable != null && typeOfNullable.BaseType == typeof(Enum))
                {
                    T enumValue = default(T);

                    if (row[fieldName] != null)
                    {
                        if (useStringValueForEnum)
                        {
                            string fieldValue = row[fieldName].ToString();

                            return EnumerationsHelper.ConvertFromString<T>(fieldValue);
                        }

                        int temp = Convert.ToInt32(row[fieldName]);

                        enumValue = EnumerationsHelper.ConvertFromInteger<T>(temp);
                    }

                    return enumValue;
                }

                T valu = (T)row[fieldName];
                return valu;
            }
            catch
            {
                throw new Exception(string.Format("Invalid Data for field {0}", fieldName));
            }
        }

        /// <summary>
        /// Gets the boolean value from the integer
        /// </summary>
        /// <param name="row" />
        /// <param name="fieldName" />
        /// <returns />
        public static bool? GetBooleanFromInt(this DataRow row, string fieldName)
        {
            bool? boolValue = null;

            int? value = row.GetValue<int?>(fieldName);
            if (value.HasValue)
            {
                boolValue = value.Value == 1;
            }

            return boolValue;
        }
        #endregion
    }
}
