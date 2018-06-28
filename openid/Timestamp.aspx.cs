using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace openid
{
    public partial class Timestamp : System.Web.UI.Page
    {
        public string timestamp_text="";
        protected void Page_Load(object sender, EventArgs e)
        {
            timestamp_text = (DateTime.Now - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds.ToString();
        }
    }
}