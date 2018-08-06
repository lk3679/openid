<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="openid.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>OpenID修正頁面</title>
    <link rel='shortcut icon' href="./favicon.ico"/>
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="bootstrap/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
<link rel="stylesheet" href="~/script/bootstrap.css" />
 <script src="script/jquery-1.10.2.min.js"></script>
    <script src="bootstrap/js/bootstrap.min.js"></script>
    <script src="bootstrap/js/bootstrap-datetimepicker.min.js"></script>
    <script src="bootstrap/js/bootstrap-datetimepicker.zh-TW.js"></script>
    <style>
        html, body {
        background-color: #eee;
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

            $("#query").click(function () {

                var mobile = $("#<%=mobile.ClientID %>").val();
                var CardNo = $("#<%=CardNo.ClientID %>").val();
                window.location.href = "default.aspx?Act=query&mobile=" + mobile + "&CardNo=" + CardNo;
            })

            $("#queryByDate").click(function () {

                var startdae = $("#<%=StartDate.ClientID %>").val();
                var enddate = $("#<%=EndDate.ClientID %>").val();
                window.location.href = "default.aspx?Act=date&pageIndex=1&startdate=" + startdae + "&enddate=" + enddate;
            })

            $("#firstPage").click(function () {
                var startdae = $("#<%=StartDate.ClientID %>").val();
                var enddate = $("#<%=EndDate.ClientID %>").val();
                window.location.href = "default.aspx?Act=date&pageIndex=1&startdate=" + startdae + "&enddate=" + enddate;
            })

            $("#lastPage").click(function () {
                var startdae = $("#<%=StartDate.ClientID %>").val();
                var enddate = $("#<%=EndDate.ClientID %>").val();
                var page =<%=pageEnd %>;
                window.location.href = "default.aspx?Act=date&pageIndex=" + page+"&startdate=" + startdae + "&enddate=" + enddate;
            })

            $(".pager").click(function () {
                var startdae = $("#<%=StartDate.ClientID %>").val();
                var enddate = $("#<%=EndDate.ClientID %>").val();
                var pageIndex = $(this).attr("data-page");
                window.location.href = "default.aspx?act=date&pageIndex=" + pageIndex  + "&startdate=" + startdae + "&enddate=" + enddate;
            })


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
        <%
            
            if (pageNow < 10)
            {          
                pageEnd = 10;
            }
            else
            {
                pageStart = pageNow - 5;
                if (pageNow + 5 < pageEnd)
                {
                    pageEnd = pageNow + 5;
                }
            }
            
            %>
          <div class="container">
           
              <span style="color:#0026ff">
             <%=AdminName %>你好，歡迎登入管理系統</span>
        <asp:Button ID="LogOut" runat="server" Text="登出系統" CssClass="btn btn-default" OnClick="LogOut_Click"  /><br />
            <h5>請輸入手機號碼</h5>     
            <asp:TextBox ID="mobile" runat="server"></asp:TextBox>
             <h5>請輸入卡號</h5>  
              <asp:TextBox ID="CardNo" runat="server"></asp:TextBox><br />
              <br />
            <input type="button" id="query" value="查詢" class=" btn btn-primary" />
    
              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
              <asp:Button ID="ExportCSV" runat="server" Text="匯出所有資料" Visible="false"  CssClass ="btn btn-primary" OnClick="ExportCSV_Click" />
              <br />
              <br />
  
              開始日期<asp:TextBox ID="StartDate" runat="server"></asp:TextBox>
&nbsp; 結束日期<asp:TextBox ID="EndDate" runat="server"></asp:TextBox>
              <br />
              <br />
              <input type="button" id="queryByDate" value="用日期查詢" class=" btn btn-primary" />
              <%--<asp:Button ID="QueryByDateBtn" runat="server" Text="用日期查詢" CssClass ="btn btn-primary" OnClick="QueryByDateBtn_Click"  />--%>
              <br />

              <br />

            <%--<input type="button" value="搜尋" id="query" class="btn btn-primary" />--%>
              <asp:Label ID="ResultLabel" runat="server" Text="" ForeColor="Red"></asp:Label>
   <table class="table table-hover table-bordered" > 
    <thead>
      <tr>
          <th></th>
            <th></th>
          <th>編號</th>
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
            <td><%=dr["ROWID"].ToString() %></td>
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
             

                  <%if (pageNow > 0)
                      { %>
              <nav aria-label="Page navigation example">
  <ul class="pagination">

  <%for (int i = pageStart; i <= pageEnd; i++)
                      { %>

      <% string activie = (i == pageNow) ? "page-item active" : "page-item"; %>
       <li class="<%=activie %>"><a class="page-link pager" href="javascript:void(0);"  data-page="<%=i %>" ><%=i %></a></li>
      <%} %>

  </ul>
</nav>
<%} %>
                </div>
    </form>
   <script type="text/javascript">

        $('#<%=StartDate.ClientID%>').datetimepicker({
            language: 'zh - TW.',
            format: 'yyyy/mm/dd',
            weekStart: 1,
            todayBtn: 1,
            autoclose: 1,
            todayHighlight: 1,
            startView: 2,
            minView: 2,
            forceParse: 0
        });

        $('#<%=EndDate.ClientID%>').datetimepicker({
            language: 'zh - TW.',
            format: 'yyyy/mm/dd',
            weekStart: 1,
            todayBtn: 1,
            autoclose: 1,
            todayHighlight: 1,
            startView: 2,
            minView: 2,
            forceParse: 0
        });

</script>
</body>
</html>
