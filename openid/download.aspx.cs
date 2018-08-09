using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace openid
{
    public partial class download : System.Web.UI.Page
    {
        public List<string> FileList = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] filesLoc = Directory.GetFiles(Server.MapPath("~/ExportData/"));
            List<ListItem> files = new List<ListItem>();

            foreach (string file in filesLoc)
            {
                string filename = Path.GetFileName(file);
                FileList.Add(filename);

            }
        }
    }
}