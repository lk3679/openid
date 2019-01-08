<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="download.aspx.cs" Inherits="openid.download" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>下載檔案</title>
       <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" />
       <style>
        html, body {
        background-color: #eee;
      }
    </style>

</head>
<body>
    <br />
             <br />
    <form id="form1" runat="server">
         <div class="container">
             <a href="javascript:history.back()" class="btn btn-default">返回查詢</a><br />
             <br />
             <br />
             檔案下載清單<br />
            <%foreach (string filename in FileList){ %>
            <a href="./ExportData/<%= filename %>"><%= filename %></a><br />
            <%} %>
        </div>
    </form>
</body>
</html>
