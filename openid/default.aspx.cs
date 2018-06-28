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

            string sql = Verification.QueryByPhone(mobilephone);
            dt = Utility.getDataTable(sql);

            if (dt.Rows.Count == 0)
                ResultLabel.Text = "查無資料";
            else
                ResultLabel.Text = string.Format("共有{0}筆資料", dt.Rows.Count.ToString());
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