using System;
using System.Collections.Generic;
using System.Linq;
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
            string act = "";
            string id = "";
            string strSQL = "";

            if (Request["act"] != null)
                act = Request["act"].ToString();

            if (Request["id"] != null)
                id = Request["id"].ToString();


            switch (act)
            {
                case "edit":
                    strSQL = Verification.RemoveOenIDByDataID(id);
                    if (Utility.execCommand(strSQL))
                        ExcudeResult = "清空成功";
                    else
                        ExcudeResult ="清空失敗";
                        
                    break;
                case "delete":
                    strSQL = Verification.DeleteOenIDByDataID(id);
                    if (Utility.execCommand(strSQL))
                        ExcudeResult = "刪除成功";
                    else
                        ExcudeResult ="刪除失敗";

                    break;
            }
        }
    }
}