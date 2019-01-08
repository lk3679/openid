using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace openid
{
    public class Verification
    {
        public static  string RemoveOenIDByDataID(string ID)
        {
            if (string.IsNullOrEmpty(ID))
                return "";
            
            string sql =string.Format(@"update [ESP_GlobalCN_APP].[dbo].[Verification_Data] set wechatopenid=null
											 where VerificationDataId={0} ",ID);
            return sql;
        }

        public static string DeleteOenIDByDataID(string ID)
        {
            if (string.IsNullOrEmpty(ID))
                return "";
            
            string sql = string.Format(@"delete from [ESP_GlobalCN_APP].[dbo].[Verification_Data]
											 where VerificationDataId={0}",ID);
            return sql;
        }

        public static string QueryByPhone(string mobilephone)
        {
                            
            string sql = @"SELECT ROW_NUMBER()   OVER (ORDER BY VerificationDataId)  AS ROWID
                                                ,VerificationDataId
                                              ,[MobilePhone]
                                              ,[PapersID]
		                                      ,[VerifyCode]
                                              ,[VerifyExpiredTime]
                                              ,[TokenBindingDate]
                                              ,[CreatedTime]
                                              ,[LiveArea]
                                              ,[wechatopenid]
                                             FROM [ESP_GlobalCN_APP].[dbo].[Verification_Data] VD ";

            if (!string.IsNullOrEmpty(mobilephone))
                sql += string.Format(" where vd.MobilePhone='{0}'", mobilephone);

            return sql;
        }

        public static string GetAllVerificationData()
        {
            string sql = QueryByPhone("");
            return sql;
        }

        public static string QueryByDate(string StartDate,string EndDate)
        {
            string sql = QueryByPhone("");
            sql += string.Format(" where [CreatedTime] between '{0}' and  '{1}' ", StartDate, EndDate);
            return sql;
        }
    }
}