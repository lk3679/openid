<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="openid.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>OpenID修正頁面</title>
    <link rel='shortcut icon' href="./favicon.ico"/>

<link rel="stylesheet" href="~/script/bootstrap.css">
    <script src="script/jquery-1.10.2.min.js"></script>
    <style>
        html, body {
        background-color: #eee;
      }

        thead th {
  background-color: #000000;
  color: white;
}
    </style>
    <script type="text/javascript" >
        
        $().ready(function () {
            //console.log("ok");
            $(".edit").click(function () {            
                var id = $(this).attr("data-vdid");
                if (id != "") {
                    var r = confirmAct("edit");
                    if (r == false) {
                        return;
                    }
                    window.location.href = "modify.aspx?act=edit&id="+id;
                }
            });

            $(".delete").click(function () {
                var id = $(this).attr("data-vdid");
                if (id != "") {
                    var r = confirmAct("delete");
                    if (r == false) {
                        return;
                    }
                    window.location.href = "modify.aspx?act=delete&id=" + id;
                }
            });
        });

        function confirmAct(act) {
            if (act == "edit") {
                var r = confirm("確定要清空嗎?");
            }
            if (act == "delete") {
                var r = confirm("確定要刪除嗎?");
            }
            return r;
            
        }
    </script>

</head>
<body>
    <br />

    <form id="form1" runat="server">
        
          <div class="container">
              <span style="color:#0026ff">
             <%=AdminName %>你好，歡迎登入管理系統</span>
        <asp:Button ID="LogOut" runat="server" Text="登出系統" CssClass="btn btn-default" OnClick="LogOut_Click"  /><br />
            <h5>請輸入手機號碼</h5>     
            <asp:TextBox ID="mobile" runat="server"></asp:TextBox>
             <h5>請輸入卡號</h5>  
              <asp:TextBox ID="CardNo" runat="server"></asp:TextBox><br />
              <br />
            <asp:Button ID="Button1" runat="server" Text="查詢" OnClick="Button1_Click" CssClass="btn btn-primary" />
              <br />
              <br />

              <br />

            <%--<input type="button" value="搜尋" id="query" class="btn btn-primary" />--%>
              <asp:Label ID="ResultLabel" runat="server" Text="" ForeColor="Red"></asp:Label>
   <table class="table table-hover table-bordered"> 
    <thead>
      <tr>
          <th></th>
            <th></th>
         <th>手機號碼</th>
        <th>驗證碼</th>
        <th>驗證到期日</th>
        <th>綁定日期</th>
         <th>建立時間</th>
        <th>地區</th>
        <th>微信openid</th>
      </tr>
    </thead>
    <tbody>
      <% if (dt!=null)
               { %>
         <%foreach (System.Data.DataRow dr in dt.Rows)
               { %>
        <tr>
            <td><input type="button" value="清空ID" class="edit btn btn-default" data-vdid="<%=dr["VerificationDataId"].ToString() %>" /></td>
             <td><input type="button" value="刪除" class="delete btn btn-danger" data-vdid="<%=dr["VerificationDataId"].ToString() %>"  /></td>
        <td><%=dr["MobilePhone"].ToString() %></td>
        <td><%=dr["VerifyCode"].ToString() %></td>
        <td><%=dr["VerifyExpiredTime"].ToString() %></td>
         <td><%=dr["TokenBindingDate"].ToString() %></td>
        <td><%=dr["CreatedTime"].ToString() %></td>
        <td><%=dr["LiveArea"].ToString() %></td>
          <td><%=dr["wechatopenid"].ToString() %></td>
      </tr>
   <%} %>
        <%} %>
    </tbody>
  </table>
                </div>
    </form>
  
</body>
</html>
