using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace openid
{
    public partial class modify : System.Web.UI.Page
    {
        public string ExcudeResult = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] == null)
            {
                Response.Write("<script>alert('請先登入帳號密碼');window.location.href='login.aspx'</script>");
            }

            string act = "";
            string id = "";
            string cardNo = "";
            string strSQL = "";

            if (Request["act"] != null)
                act = Request["act"].ToString();

            if (Request["id"] != null)
                id = Request["id"].ToString();

            if (Request["cardNo"] != null)
                cardNo = Request["cardNo"].ToString();


            switch (act)
            {
                case "edit":
                    strSQL = Verification.RemoveOenIDByDataID(id);
                    if (Utility.execCommand(strSQL))
                    {                      
                        string OchengResult = RemoveOpenIdByOcheng(cardNo);
                        ExcudeResult = "清空成功，"+ OchengResult;
                    }
                    else
                    {
                        ExcudeResult = "清空失敗";
                    }                                              
                    break;
                case "delete":
                    strSQL = Verification.DeleteOenIDByDataID(id);
                    if (Utility.execCommand(strSQL))
                    {
                        
                        string OchengResult = RemoveOpenIdByOcheng(cardNo);
                        ExcudeResult = "刪除成功，" + OchengResult;
                    }
                    else
                    {
                        ExcudeResult = "刪除失敗";
                    }                                             
                    break;
            }
        }

        private string RemoveOpenIdByOcheng(string cardNo)
        {
            string targetUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ocheng"];
            HttpWebRequest request = HttpWebRequest.Create(targetUrl+cardNo) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = 30000;

            string result = "";
            // 取得回應資料
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
            }

            return result;

        }
    }
}