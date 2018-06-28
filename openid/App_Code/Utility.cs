using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Data;
using System;

namespace openid
{
    public class Utility
    {
        #region "加密／解密"

        private static string strKey = "CASPER59";

        private static string strIV = "TEMPLATE";

        /// <summary>字串編碼</summary>
        /// <param name="strSource">原始字串</param>
        /// <returns>編碼後的結果字串</returns>
        /// <remarks></remarks>
        public static string enCrypt(string strSource)
        {
            byte[] buffer = Encoding.Default.GetBytes(strSource);
            MemoryStream ms = new MemoryStream();
            DESCryptoServiceProvider tdes = new DESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(ms, tdes.CreateEncryptor(Encoding.Default.GetBytes(strKey), Encoding.Default.GetBytes(strIV)), CryptoStreamMode.Write);
            encStream.Write(buffer, 0, buffer.Length);
            encStream.FlushFinalBlock();
            ms.Close();
            encStream.Close();
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>字串解碼</summary>
        /// <param name="strSource">編碼過的字串</param>
        /// <returns>未編碼的原始字串</returns>
        /// <remarks></remarks>
        public static string deCrypt(string strSource)
        {
            byte[] buffer = Convert.FromBase64String(strSource);
            MemoryStream ms = new MemoryStream();
            DESCryptoServiceProvider tdes = new DESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(ms, tdes.CreateDecryptor(Encoding.Default.GetBytes(strKey), Encoding.Default.GetBytes(strIV)), CryptoStreamMode.Write);
            encStream.Write(buffer, 0, buffer.Length);
            encStream.FlushFinalBlock();
            ms.Close();
            encStream.Close();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion

        #region "避免SQL Injection相關函式"

        #region "一般字元欄位處理"

        /// <summary>為傳入SQL指令的字串內容加單引號</summary>
        /// <param name="source">原始資料內容</param>
        /// <returns>加入單引號後的結果字串</returns>
        /// <remarks></remarks>
        public static string addQuot(string source)
        {
            return "'" + source.Replace("'", "''").Replace(" ", "").Trim() + "'";
        }

        /// <summary>為傳入SQL指令的字串內容加單引號</summary>
        /// <param name="source">原始資料內容</param>
        /// <returns>加入單引號後的結果字串</returns>
        /// <remarks></remarks>
        public static string addQuot(int source)
        {
            return addQuot(source.ToString());
        }

        /// <summary>為傳入SQL指令的字串內容加單引號</summary>
        /// <param name="source">原始資料內容</param>
        /// <returns>加入單引號後的結果字串</returns>
        /// <remarks></remarks>
        public static string addQuot(double source)
        {
            return addQuot(source.ToString());
        }

        #endregion

        #region "Unicode字元欄位處理"

        /// <summary>串接 SQL 指令的 Unicode 字元</summary>
        /// <param name="source">字串來源</param>
        /// <returns>結果字串</returns>
        /// <remarks></remarks>
        public static string NaddQuot(string source)
        {
            return "N'" + source.Replace("'", "''").Replace(" ", "''").Trim() + "'";
        }

        #endregion

        #region "數值資料欄位處理"

        /// <summary>串接 SQL 指令的數值資料</summary>
        /// <param name="source">數值內容</param>
        /// <returns>結果字串</returns>
        /// <remarks></remarks>
        public static string transVal(string source)
        {
            source = source.Replace(",", "");
            string returnValue = "(";
            double retNum;
            if (Double.TryParse(source, out retNum))
            {
                returnValue += (Convert.ToDouble(source)).ToString();
            }
            else
            {
                returnValue += "0";
            }
            returnValue += ")";
            return returnValue;
        }

        /// <summary>串接 SQL 指令的數值資料</summary>
        /// <param name="source">數值內容</param>
        /// <returns>結果字串</returns>
        /// <remarks></remarks>
        public static string transVal(int source)
        {
            return transVal(source.ToString());
        }

        /// <summary>串接 SQL 指令的數值資料</summary>
        /// <param name="source">數值內容</param>
        /// <returns>結果字串</returns>
        /// <remarks></remarks>
        public static string transVal(double source)
        {
            return transVal(source.ToString());
        }

        /// <summary>將Control的內容轉換為 Double 型別的數值資料</summary>
        /// <param name="source">Control的內容，如：Textbox1.Text</param>
        /// <returns>Double 型別的資料</returns>
        /// <remarks></remarks>
        public static double toDouble(string source)
        {
            double retValue = 0;

            if (Double.TryParse(source, out retValue))
            {
                retValue = 0;
            }
            else
            {
                retValue = Convert.ToDouble(source);
            }
            return retValue;
        }

        /// <summary>將Control的內容轉換為 Integer 型別的數值資料</summary>
        /// <param name="source">Control的內容，如：Textbox1.Text</param>
        /// <returns>Integer 型別的資料</returns>
        /// <remarks></remarks>
        public static int toInteger(string source)
        {
            int retValue = 0;
            if (int.TryParse(source, out retValue))
            {
                retValue = 0;
            }
            else
            {
                retValue = Convert.ToInt32(source);
            }
            return retValue;
        }

        #endregion

        #endregion

        #region "關於資料庫連線"

        /// <summary>取得 Configuration.appSettings 內容</summary>
        /// <param name="strKey">Key 值，若不設定則取 DNS</param>
        /// <returns>取得的內容字串或空白</returns>
        /// <remarks></remarks>
        public static string getAppSettings(string strKey)
        {
            return getAppSettings(strKey, false);
        }
        /// <summary>取得 Configuration.appSettings 內容</summary>
        /// <param name="strKey">Key 值，若不設定則取 DNS</param>
        /// <param name="blnDecrypt">內容是否為編碼，預設為編碼</param>
        /// <returns>取得的內容字串或空白</returns>
        /// <remarks></remarks>
        public static string getAppSettings(string strKey, bool blnDecrypt)
        {
            string strValue = System.Web.Configuration.WebConfigurationManager.AppSettings[strKey];
            if ((strValue != null))
            {
                string strDecrypt = null;
                if (blnDecrypt)
                {
                    strDecrypt = deCrypt(strValue);
                }
                else
                {
                    strDecrypt = strValue;
                }
                return strDecrypt;
            }
            else
            {
                return "";
            }
        }

        /// <summary>建立資料庫連線(SqlConnection)</summary>
        /// <param name="strConnectionString">連線字串</param>
        /// <param name="blnDecrypt">連線字串是否經過編碼</param>
        /// <returns>SqlConnection</returns>
        /// <remarks></remarks>
        public static SqlConnection createConnection(string strConnectionString, bool blnDecrypt)
        {
            if (blnDecrypt)
            {
                strConnectionString = deCrypt(strConnectionString);
            }
            return new System.Data.SqlClient.SqlConnection(strConnectionString);
        }

        /// <summary>建立資料庫連線(SqlConnection)</summary>
        /// <param name="blnDecrypt">連線字串是否經過編碼</param>
        /// <returns>SqlConnection</returns>
        /// <remarks></remarks>
        public static SqlConnection createConnection(bool blnDecrypt)
        {
            return createConnection(getAppSettings("DSN", blnDecrypt), false);
        }

        /// <summary>建立資料庫連線(SqlConnection)</summary>
        /// <param name="strConnectionString">連線字串</param>
        /// <returns>SqlConnection</returns>
        /// <remarks></remarks>
        public static SqlConnection createConnection(string strConnectionString)
        {
            return createConnection(strConnectionString, true);
        }

        /// <summary>建立資料庫連線(SqlConnection)</summary>
        /// <returns>SqlConnection</returns>
        /// <remarks></remarks>
        public static SqlConnection createConnection()
        {
            return createConnection(getAppSettings("DSN", true), false);
        }

        /// <summary>建立資料庫連線(OleDbConnection)</summary>
        /// <param name="strConnectionString">連線字串</param>
        /// <param name="blnDecrypt">連線字串是否經過編碼</param>
        /// <returns>OleDbConnection</returns>
        /// <remarks></remarks>
        public static OleDbConnection createOleDbConnection(string strConnectionString, bool blnDecrypt)
        {
            if (blnDecrypt)
            {
                strConnectionString = deCrypt(strConnectionString);
            }
            return new System.Data.OleDb.OleDbConnection(strConnectionString);
        }

        /// <summary>建立資料庫連線(OleDbConnection)</summary>
        /// <param name="blnDecrypt">連線字串是否經過編碼</param>
        /// <returns>OleDbConnection</returns>
        /// <remarks></remarks>
        public static OleDbConnection createOleDbConnection(bool blnDecrypt)
        {
            return createOleDbConnection(getAppSettings("DSN", true), blnDecrypt);
        }

        /// <summary>建立資料庫連線(OleDbConnection)</summary>
        /// <param name="strConnectionString">連線字串</param>
        /// <returns>OleDbConnection</returns>
        /// <remarks></remarks>
        public static OleDbConnection createOleDbConnection(string strConnectionString)
        {
            return createOleDbConnection(strConnectionString, true);
        }

        /// <summary>建立資料庫連線(OleDbConnection)</summary>
        /// <returns>OleDbConnection</returns>
        /// <remarks></remarks>
        public static OleDbConnection createOleDbConnection()
        {
            return createOleDbConnection(getAppSettings("DSN", true), true);
        }
        #endregion

        #region "DataSet 、 DataTable 、 DataReader 、 Command 及 Transaction"

        /// <summary>取得DataSet</summary>
        /// <param name="strSQL">SQL 指令</param>
        /// <param name="sqlConn">連線物件</param>
        /// <param name="strTable">DataTable名稱</param>
        /// <returns>DataSet</returns>
        /// <remarks></remarks>
        public static DataSet getDataSet(string strSQL, System.Data.SqlClient.SqlConnection sqlConn, string strTable)
        {
            System.Data.SqlClient.SqlDataAdapter da = null;
            DataSet ds = new DataSet();
            try
            {
                if (sqlConn == null)
                {
                    sqlConn = createConnection();
                }
                da = new System.Data.SqlClient.SqlDataAdapter(strSQL, sqlConn);
                if (string.IsNullOrEmpty(strTable))
                {
                    da.Fill(ds);
                }
                else
                {
                    da.Fill(ds, strTable);
                }
            }
            catch (Exception Ex)
            {
                //Message.alertMessage("C0001", null, Ex.Message.ToString(), null);
                ds = null;
            }
            finally
            {
                if (!(sqlConn.State == ConnectionState.Closed))
                    sqlConn.Close();
            }
            return ds;
        }
        public static DataTable getDataTable(string strSQL, System.Data.SqlClient.SqlConnection sqlConn)
        {
            return getDataTable(strSQL, sqlConn, 0);
        }
        /// <summary>取得DataTable</summary>
        /// <param name="strSQL">SQL 指令</param>
        /// <param name="sqlConn">連線物件</param>
        /// <param name="Timeout">逾時執行時間</param>
        /// <returns>DataTable</returns>
        /// <remarks></remarks>
        public static DataTable getDataTable(string strSQL, System.Data.SqlClient.SqlConnection sqlConn, int Timeout)
        {
            DataTable dt = new DataTable();
            System.Data.SqlClient.SqlDataAdapter da = null;
            try
            {
                if (sqlConn == null)
                {
                    sqlConn = createConnection();
                }
                System.Data.SqlClient.SqlCommand cmd = null;
                cmd = new System.Data.SqlClient.SqlCommand(strSQL, sqlConn);
                if (Timeout < 0)
                {
                    Timeout = 0;
                }
                if (Timeout != 0)
                {
                    cmd.CommandTimeout = Timeout;
                }
                da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception Ex)
            {
                //Message.alertMessage("C0001", null, Ex.Message.ToString(), null);
                dt = null;
            }
            finally
            {
                if (!(sqlConn.State == ConnectionState.Closed))
                    sqlConn.Close();
            }
            return dt;
        }

        /// <summary>取得DataTable</summary>
        /// <param name="strSQL">SQL 指令</param>
        /// <returns>DataTable</returns>
        /// <remarks></remarks>
        public static DataTable getDataTable(string strSQL)
        {
            return getDataTable(strSQL, null);
        }
        /// <summary>取得DataTable</summary>
        /// <param name="strSQL">SQL 指令</param>
        /// <param name="Timeout">逾時執行時間</param>
        /// <returns>DataTable</returns>
        /// <remarks></remarks>
        public static DataTable getDataTable(string strSQL, int Timeout)
        {
            return getDataTable(strSQL, null, Timeout);
        }
        /// <summary>取得DataReader</summary>
        /// <param name="strSQL">SQL 指令</param>
        /// <param name="sqlConn">連線物件(SqlConnection)</param>
        /// <returns>SqlDataReader</returns>
        /// <remarks></remarks>
        public static SqlDataReader getDataReader(string strSQL, System.Data.SqlClient.SqlConnection sqlConn)
        {
            System.Data.SqlClient.SqlDataReader dr = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (sqlConn == null)
                {
                    sqlConn = createConnection();
                }
                if (!(sqlConn.State == ConnectionState.Open))
                    sqlConn.Open();
                cmd = new System.Data.SqlClient.SqlCommand(strSQL, sqlConn);
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception Ex)
            {
                //Message.alertMessage("C0001", null, Ex.Message.ToString(), null);
                if (!(sqlConn.State == ConnectionState.Closed))
                    sqlConn.Close();
                dr = null;
            }
            return dr;
        }

        /// <summary>取得DataReader</summary>
        /// <param name="strSQL">SQL 指令</param>
        /// <returns>SqlDataReader</returns>
        /// <remarks></remarks>
        public static SqlDataReader getDataReader(string strSQL)
        {
            return getDataReader(strSQL, null);
        }

        /// <summary>取得OleDbDataReader</summary>
        /// <param name="strSQL">SQL 指令</param>
        /// <param name="oleDbConn">連線物件(OleDbConnection)</param>
        /// <returns>OleDbDataReader</returns>
        /// <remarks></remarks>
        public static OleDbDataReader getOleDbDataReader(string strSQL, System.Data.OleDb.OleDbConnection oleDbConn)
        {
            System.Data.OleDb.OleDbDataReader dr = null;
            System.Data.OleDb.OleDbCommand cmd = null;
            try
            {
                if (oleDbConn == null)
                {
                    oleDbConn = createOleDbConnection();
                }
                if (!(oleDbConn.State == ConnectionState.Open))
                {
                    oleDbConn.Open();
                }
                cmd = new System.Data.OleDb.OleDbCommand(strSQL, oleDbConn);
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception Ex)
            {
                //Message.alertMessage("C0001", null, Ex.Message.ToString(), null);
                if (!(oleDbConn.State == ConnectionState.Closed))
                {
                    oleDbConn.Close();
                }
                dr = null;
            }
            return dr;
        }

        /// <summary>取得OleDbDataReader</summary>
        /// <param name="strSQL">SQL 指令</param>
        /// <returns>OleDbDataReader</returns>
        /// <remarks></remarks>
        public static OleDbDataReader getOleDbDataReader(string strSQL)
        {
            return getOleDbDataReader(strSQL, null);
        }

        /// <summary>取得Scalar物件</summary>
        /// <param name="strSQL">SQL 指令</param>
        /// <param name="sqlConn">連線物件</param>
        /// <returns>Object</returns>
        /// <remarks></remarks>
        public static object getScalar(string strSQL, System.Data.SqlClient.SqlConnection sqlConn)
        {
            //If TypeOf sqlConn Is SqlClient.SqlConnection Then
            //End If
            System.Data.SqlClient.SqlCommand cmd = null;
            object objScalar = null;
            try
            {
                if (sqlConn == null)
                {
                    sqlConn = createConnection();
                }
                if (!(sqlConn.State == ConnectionState.Open))
                    sqlConn.Open();
                cmd = new System.Data.SqlClient.SqlCommand(strSQL, sqlConn);
                objScalar = cmd.ExecuteScalar();
            }
            catch (Exception Ex)
            {
                //Message.alertMessage("C0001", null, Ex.Message.ToString(), null);
                objScalar = null;
            }
            finally
            {
                if (!(sqlConn.State == ConnectionState.Closed))
                    sqlConn.Close();
            }
            return objScalar;
        }

        public static object getScalar(string strSQL)
        {
            return getScalar(strSQL, null);
        }

        /// <summary>取得Scalar物件</summary>
        /// <param name="strSQL">SQL 指令</param>
        /// <param name="OleDbConn">OleDbConnection 連線物件(不指定表使用預設連線)</param>
        /// <returns>Object</returns>
        /// <remarks></remarks>
        public static object getOleDbScalar(string strSQL, System.Data.OleDb.OleDbConnection OleDbConn)
        {
            System.Data.OleDb.OleDbCommand cmd = null;
            object objScalar = null;
            try
            {
                if (OleDbConn == null)
                {
                    OleDbConn = createOleDbConnection();
                }
                if (!(OleDbConn.State == ConnectionState.Open))
                {
                    OleDbConn.Open();
                }
                cmd = new System.Data.OleDb.OleDbCommand(strSQL, OleDbConn);
                objScalar = cmd.ExecuteScalar();
            }
            catch (Exception Ex)
            {
                //Message.alertMessage("C0001", null, Ex.Message.ToString(), null);
                objScalar = null;
            }
            finally
            {
                if (!(OleDbConn.State == ConnectionState.Closed))
                {
                    OleDbConn.Close();
                }
            }
            return objScalar;
        }

        /// <summary>執行 SQL 指令</summary>
        /// <param name="strSQL">欲執行的 SQL 指令</param>
        /// <param name="OleDbConn">OleDbConnection 連線物件(不指定表使用預設連線)</param>
        /// <returns>Boolean</returns>
        /// <remarks></remarks>
        public static bool execOleDbCommand(string strSQL, System.Data.OleDb.OleDbConnection OleDbConn)
        {
            try
            {
                if (OleDbConn == null)
                {
                    OleDbConn = createOleDbConnection();
                }
                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(strSQL, OleDbConn);
                if (!(OleDbConn.State == ConnectionState.Open))
                {
                    OleDbConn.Open();
                }
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                //Message.alertMessage("C0002", null, Ex.Message.ToString(), null);
                return false;
            }
            finally
            {
                if (!(OleDbConn.State == ConnectionState.Closed))
                {
                    OleDbConn.Close();
                }
            }
            return true;
        }

        /// <summary>執行 SQL 指令</summary>
        /// <param name="strSQL">SQL 指令</param>
        /// <param name="sqlConn">連線物件</param>
        /// <param name="extraCommand">額外的指令(通常用來傳回 Identity 時使用)</param>
        /// <param name="lngIdentity">傳回 Identity 的值</param>
        /// <param name="Timeout">Timeout秒數</param>
        /// <returns>執行是否成功</returns>
        /// <remarks></remarks>
        public static bool execCommand(string strSQL, System.Data.SqlClient.SqlConnection sqlConn, string extraCommand, ref long lngIdentity, int Timeout)
        {
            try
            {
                if (sqlConn == null)
                {
                    sqlConn = createConnection();
                }
                if (!string.IsNullOrEmpty(extraCommand))
                {
                    strSQL = extraCommand + "\r\n" + strSQL;
                }
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(strSQL, sqlConn);
                if (Timeout < 0)
                {
                    Timeout = 0;
                }
                if (Timeout != 0)
                {
                    cmd.CommandTimeout = Timeout;
                }
                if (!(sqlConn.State == ConnectionState.Open))
                    sqlConn.Open();

                //cmd.ExecuteNonQuery();
                //20150319.. .. Johnny : 修改取得執行影響筆數，以判斷是否正確執行
                int tResultInt = cmd.ExecuteNonQuery();
                if (tResultInt < 0)
                {
                    return false;
                }

                if (lngIdentity != -1)
                {
                    strSQL = "select @@identity";
                    lngIdentity = Convert.ToInt32(getScalar(strSQL, sqlConn));
                }
            }
            catch (Exception Ex)
            {
                //Message.alertMessage("C0002", null, Ex.Message.ToString(), null);
                return false;
            }
            finally
            {
                if (!(sqlConn.State == ConnectionState.Closed))
                    sqlConn.Close();
            }
            return true;
        }

        /// <summary>執行 SQL 指令</summary>
        /// <param name="strSQL">SQL 指令</param>
        /// <returns>執行是否成功</returns>
        /// <remarks></remarks>
        public static bool execCommand(string strSQL)
        {
            long Identity = -1;
            return execCommand(strSQL, null, "", ref Identity, 0);
        }

        /// <summary>執行 SQL 指令</summary>
        /// <param name="strSQL">SQL 指令</param>
        /// <param name="sqlConn">SqlConnection物件</param>
        /// <returns>執行是否成功</returns>
        /// <remarks></remarks>
        public static bool execCommand(string strSQL, System.Data.SqlClient.SqlConnection sqlConn)
        {
            long Identity = -1;
            return execCommand(strSQL, sqlConn, "", ref Identity, 0);
        }

        /// <summary>執行 SQL 指令</summary>
        /// <param name="strSQL">SQL 指令</param>
        /// <param name="sqlConn">SqlConnection物件</param>
        /// <param name="extraCommand">額外的指令(通常用來傳回 Identity 時使用)</param>
        /// <returns>執行是否成功</returns>
        /// <remarks></remarks>
        public static bool execCommand(string strSQL, System.Data.SqlClient.SqlConnection sqlConn, string extraCommand)
        {
            long Identity = -1;
            return execCommand(strSQL, sqlConn, extraCommand, ref Identity, 0);
        }

        /// <summary>執行 SQL 指令</summary>
        /// <param name="strSQL">SQL 指令</param>
        /// <param name="Timeout">Timeout秒數</param>
        /// <returns>執行是否成功</returns>
        /// <remarks></remarks>
        public static bool execCommand(string strSQL, int Timeout)
        {
            long Identity = -1;
            return execCommand(strSQL, null, "", ref Identity, Timeout);
        }


        /// <summary>執行 Transaction</summary>
        /// <param name="alSQL">欲執行交易的 ArrayList (內含 SQL 指令)</param>
        /// <param name="OleDbConn">OleDbConnection連線物件</param>
        /// <returns>Transaction是否成功</returns>
        /// <remarks></remarks>
        public static bool raiseOleDbTransaction(ArrayList alSQL, System.Data.OleDb.OleDbConnection OleDbConn)
        {
            if (alSQL == null)
            {
                return true;
            }
            if (alSQL.Count == 0)
            {
                return true;
            }
            if (OleDbConn == null)
            {
                OleDbConn = createOleDbConnection();
            }
            System.Data.OleDb.OleDbTransaction OleDbTrans = null;
            if (!(OleDbConn.State == ConnectionState.Open))
            {
                OleDbConn.Open();
            }
            System.Data.OleDb.OleDbCommand cmd = OleDbConn.CreateCommand();
            StringBuilder strSQL = new StringBuilder("");
            OleDbTrans = OleDbConn.BeginTransaction();
            try
            {
                cmd.Transaction = OleDbTrans;
                for (int i = 0; i <= alSQL.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(alSQL[i].ToString()))
                    {
                        strSQL.AppendLine(alSQL[i].ToString());
                    }
                }
                cmd.CommandText = strSQL.ToString();
                cmd.ExecuteNonQuery();
                OleDbTrans.Commit();
                return true;
            }
            catch (Exception Ex)
            {
                if ((OleDbTrans != null))
                {
                    OleDbTrans.Rollback();
                }
                //Message.alertMessage("C0002", null, Ex.Message.ToString(), null);
                return false;
            }
            finally
            {
                if (!(OleDbConn.State == ConnectionState.Closed))
                {
                    OleDbConn.Close();
                }
            }
        }

        /// <summary>執行 Transaction</summary>
        /// <param name="listSQL">欲執行交易的 ArrayList (內含 SQL 指令)</param>
        /// <param name="sqlConn">連線物件</param>
        /// <returns>Transaction是否成功</returns>
        /// <remarks></remarks>
        public static bool raiseTransaction(ArrayList listSQL, System.Data.SqlClient.SqlConnection sqlConn)
        {
            if (listSQL == null)
            {
                return true;
            }
            if (listSQL.Count == 0)
            {
                return true;
            }
            if (sqlConn == null)
            {
                sqlConn = createConnection();
            }
            System.Data.SqlClient.SqlTransaction sqlTrans = null;
            if (!(sqlConn.State == ConnectionState.Open))
                sqlConn.Open();
            System.Data.SqlClient.SqlCommand cmd = sqlConn.CreateCommand();
            StringBuilder strSQL = new StringBuilder("");
            sqlTrans = sqlConn.BeginTransaction();
            try
            {
                cmd.Transaction = sqlTrans;
                for (int i = 0; i <= listSQL.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(listSQL[i].ToString()))
                    {
                        strSQL.AppendLine(listSQL[i].ToString());
                    }
                }
                cmd.CommandText = strSQL.ToString();
                cmd.ExecuteNonQuery();
                sqlTrans.Commit();
                return true;
            }
            catch (Exception Ex)
            {
                if ((sqlTrans != null))
                {
                    sqlTrans.Rollback();
                }
                //Message.alertMessage("C0002", null, Ex.Message.ToString(), null);
                return false;
            }
            finally
            {
                if (!(sqlConn.State == ConnectionState.Closed))
                    sqlConn.Close();
            }
        }

        /// <summary>執行 Transaction</summary>
        /// <param name="listSQL">欲執行交易的 ArrayList (內含 SQL 指令)</param>
        /// <returns>Transaction是否成功</returns>
        /// <remarks></remarks>
        public static bool raiseTransaction(ArrayList listSQL)
        {
            return raiseTransaction(listSQL, null);
        }

        #endregion
    }

}