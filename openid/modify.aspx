<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="modify.aspx.cs" Inherits="openid.modify" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>修改結果</title>
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
    <div class="container">
    <a href="javascript:history.back()" class="btn btn-default">返回查詢</a>
    <form id="form1" runat="server">
        <h3><%=ExcudeResult %></h3>
        <div>
        </div>
    </form>
        </div>
</body>
</html>
