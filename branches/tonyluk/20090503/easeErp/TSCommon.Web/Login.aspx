<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Login.aspx.cs" Inherits="TSCommon.Web.Login" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <style type="text/css">
        .style1
        {
            width: 100px;
            height: 49px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
            <div style="text-align: center">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 100px">
                        </td>
                        <td style="width: 100px">
                            用户名：</td>
                        <td style="width: 100px">
                            <asp:TextBox ID="UserName" runat="server"></asp:TextBox></td>
                        <td style="width: 100px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                        </td>
                        <td style="width: 100px">
                            密 码：</td>
                        <td style="width: 100px">
                            <asp:TextBox ID="UserPass" TextMode="Password" runat="server"></asp:TextBox>
                            <div><%=this.ErrorMessage %></div></td>
                        <td style="width: 100px">
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                        </td>
                        <td class="style1">
                        </td>
                        <td class="style1">
                            <asp:Button ID="btnLogin" runat="server" Text="登  录" OnClick="btnLogin_Click" /></td>
                        <td class="style1">
                        </td>
                    </tr>
                </table>
            </div>
    </form>
</body>
</html>
