using System;
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
            string username = "";
            string password = "";
            if (Request["Username"] != null)
                username = Request["Username"].ToString();

            if (Request["Password"] != null)
                password = Request["Password"].ToString();

            LoginResult = string.Format("帳號:{0},密碼:{1}", username, password);
        }
    }
}