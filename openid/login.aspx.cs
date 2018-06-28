using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace openid
{
    public partial class login : System.Web.UI.Page
    {
        public string LoginResult = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable AccountList = new Hashtable();
            AccountList.Add("sarampeng", "sarampeng");

            if (Session["userID"] != null)
            {
                Response.Redirect("default.aspx");
            }

            string username = "";
            string password = "";

            if (Request["Username"] != null)
                username = Request["Username"].ToString().ToLower();

            if (Request["Password"] != null)
                password = Request["Password"].ToString().ToLower();

            if (string.IsNullOrEmpty(username))
            {
                LoginResult = "請輸入帳號";
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                LoginResult = "請輸入密碼";
                return;
            }

            if (AccountList.Contains(username))
            {
                object valuePassword = AccountList[username];
                if (valuePassword.ToString()== password) {
                    Session["userID"] = username;
                    Response.Redirect("default.aspx");
                }
                else
                {
                    LoginResult = "登入失敗，密碼輸入錯誤";                   
                }
            }
            else
            {
                LoginResult = "登入失敗，查無此帳號";                
            }
            
            
        }
    }
}