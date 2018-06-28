<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="openid.login" %>

<!DOCTYPE html>
<html lang="en">
  <head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>登入頁面</title>
      <link rel='shortcut icon' href="./favicon.ico"/>
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="stylesheet" href="./script/bootstrap.css">
    <script src="script/jquery-1.10.2.min.js"></script>
    <style type="text/css">
      /* Override some defaults */
      html, body {
        background-color: #eee;
      }
      body {
        padding-top: 5%;
      }
      .container {
        width: 500px;
      }

      /* The white background content wrapper */
      .container > .content {
        background-color: #fff;
        padding: 20px;
        margin: 0 -20px;
        -webkit-border-radius: 10px 10px 10px 10px;
           -moz-border-radius: 10px 10px 10px 10px;
                border-radius: 10px 10px 10px 10px;
        -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.15);
           -moz-box-shadow: 0 1px 2px rgba(0,0,0,.15);
                box-shadow: 0 1px 2px rgba(0,0,0,.15);
      }

	  .login-form {
		margin-left: 65px;
	  }

	  legend {
		margin-right: -50px;
		font-weight: bold;
	  	color: #404040;
	  }

    </style>

</head>
<body>
  <div class="container">
    <div class="content">
       
      <div class="row">
        <div class="login-form">
          <h4>請輸入帳號密碼</h4>
            <br/>
          <form action="" method="post" >
               <%=LoginResult %>
            <fieldset>
              <div class="clearfix">
                <input type="text" placeholder="Username" name="Username">
              </div>
                <br />
              <div class="clearfix">
                <input type="password" placeholder="Password" name="Password">
              </div>
                <br/>
              <button class="btn primary" type="submit">送出</button>
            </fieldset>
          </form>
        </div>
      </div>
    </div>
  </div> <!-- /container -->
</body>
</html>

