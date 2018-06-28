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
           
            if (string.IsNullOrEmpty(mobilephone))
                return "";
            
            string sql = @"SELECT VerificationDataId, 
                                              [MobilePhone],
		                                        [VerifyCode]
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
    }
}