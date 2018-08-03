using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using openid.CRMwebReference;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace openid
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public DataTable dt = new DataTable() { };
        public string AdminName = "";
        public int pageStart = 1;
        public int pageEnd = 1;
        public int pageNow = 0;

        public static bool IsPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^1(3[0-9]|5[0-9]|7[6-8]|8[0-9])[0-9]{8}$");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] == null)
            {
                Response.Write("<script>alert('請先登入帳號密碼');window.location.href='login.aspx'</script>");
            }
            else
            {
                AdminName = Session["userID"].ToString();
            }

            string pageIndex = "";

            if (string.IsNullOrEmpty(Request["pageIndex"]) == false)
            {
                pageIndex = Request["pageIndex"];
                StartDate.Text = Request["startdate"];
                EndDate.Text = Request["enddate"];

                int parsedInt = 0;
                if (int.TryParse(pageIndex, out parsedInt))
                {
                    QueryByDate(parsedInt, 10);
                    pageNow = parsedInt;
                }
                else
                {
                    ResultLabel.Text = "輸入的分頁錯誤";
                }
               
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string mobilephone = mobile.Text;
            string cardno = CardNo.Text;

            if (mobilephone.Length > 0)
            {
                if (IsPhoneNumber(mobilephone) == false)
                {
                    ResultLabel.Text = string.Format("手機號碼{0}格式輸入錯誤",mobilephone);
                    mobile.Text = "";
                    return;
                }
            }
            
            //if cardno has value,query by cardno first
            if (!string.IsNullOrEmpty(cardno))
            {
                //Get User phoneNumber by cardNo
                CustData CD = new CustData();              
                BFCRMWebService crm = new BFCRMWebService();             
                string cust= crm.GetCustDataJSON("",cardno,"","","","");
                if (!string.IsNullOrEmpty(cust))
                    CD = JsonConvert.DeserializeObject<CustData>(cust);

                if (CD.CustList == null)
                {
                    ResultLabel.Text = string.Format("卡號{0}查無資料", cardno);
                    CardNo.Text = "";
                    return;
                }

                if (!string.IsNullOrEmpty(CD.CustList[0].MobilePhone))
                    mobilephone = CD.CustList[0].MobilePhone;

            }

            if (string.IsNullOrEmpty(mobilephone))
            {
                ResultLabel.Text = "輸入資料有誤";
                return;
            }

            string sql = Verification.QueryByPhone(mobilephone);
            dt = Utility.getDataTable(sql);

            if (dt.Rows.Count == 0)
                ResultLabel.Text = "查無資料";
            else
                ResultLabel.Text = string.Format("共有{0}筆資料", dt.Rows.Count.ToString());
        }

        protected void LogOut_Click(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Write("<script>alert('你已登出系統');window.location.href='login.aspx'</script>");
        }

        protected void ExportCSV_Click(object sender, EventArgs e)
        {

            string sql = Verification.GetAllVerificationData();
            dt = Utility.getDataTable(sql);

            if (dt.Rows.Count == 0)
            {
                ResultLabel.Text = "查無資料，請稍後再試";
                return;
            }

            string csv = string.Empty;
            foreach (DataColumn column in dt.Columns)
            {
                csv += column.ColumnName + ',';
            }
            //Add new line.
            csv += "\r\n";

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Data rows.
                    csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                }
                //Add new line.
                csv += "\r\n";
            }

            string Filename = string.Format("ExportData_{0}.csv", DateTime.Now.ToString("yyyyMMddhhmmss"));
            Response.Clear();
            Response.Buffer = true;
            Response.HeaderEncoding= System.Text.Encoding.UTF8;
            Response.AddHeader("content-disposition", "attachment;filename="+ Filename);
            Response.Charset = "";
            Response.ContentType = "application/octet-stream";
            System.IO.StreamWriter sw =new System.IO.StreamWriter(Response.OutputStream,System.Text.Encoding.UTF8);
            sw.Write(csv);
            sw.Close();
            Response.Flush();
            Response.End();

        }

        protected void QueryByDateBtn_Click(object sender, EventArgs e)
        {
            //string url = string.Format("default.aspx?pageIndex=1&startdate={0}&enddate={1}", StartDate.Text, EndDate.Text);
            //Response.Redirect(url);
        }

        public void QueryByDate(int pageIndex,int pageSize)
        {
            ResultLabel.Text = "";
            if (string.IsNullOrEmpty(StartDate.Text))
            {
                ResultLabel.Text = "開始日期不可以為空";
                return;
            }

            if (string.IsNullOrEmpty(EndDate.Text))
            {
                ResultLabel.Text = "結束日期不可以為空";
                return;
            }

            DateTime startdate = new DateTime();
            DateTime enddate = new DateTime();
            startdate = DateTime.Parse(StartDate.Text);
            enddate = DateTime.Parse(EndDate.Text);

            if (startdate > enddate)
            {
                ResultLabel.Text = "開始日期不可小於結束日期";
                return;
            }

            string sql = Verification.QueryByDate(StartDate.Text, EndDate.Text);
            System.Data.DataTable AllTable = Utility.getDataTable(sql);
            pageEnd = (AllTable.Rows.Count - 1) / pageSize+1;
            dt = GetPagedTable(AllTable, pageIndex, pageSize);
            if (dt.Rows.Count == 0)
                ResultLabel.Text = "查無資料";
            else
                ResultLabel.Text = string.Format("共有{0}筆資料", AllTable.Rows.Count.ToString());
        }

        public DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)
        {
            if (PageIndex == 0)
                return dt;

            DataTable newdt = dt.Copy();
            newdt.Clear();

            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
                return newdt;

            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }
    }



    class CustData
    {
        public string RC = "";
        public string RM= "";
        public List<Cust> CustList = new List<Cust>();
    }

    internal class Cust
    {
        public string CustNo = "";
        public string CardNo = "";
        public string CardKind = "";
        public string AccountFlag = "";
        public string Name = "";
        public string Birthday = "";
        public string Sex = "";
        public string PapersType = "";
        public string PapersID = "";
        public string MobilePhone = "";
        public string Address_ZipCode = "";
        public string Address = "";
        public string Email = "";
        public string HobbyID = "";
    }
}