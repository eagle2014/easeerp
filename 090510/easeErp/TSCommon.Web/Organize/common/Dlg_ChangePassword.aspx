<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dlg_ChangePassword.aspx.cs" Inherits="TSCommon.Web.Organize.common.Dlg_ChangePassword" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= TSLib.SimpleResourceHelper.GetSystemTitle() %></title>
    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-Dlg.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>
	<link rel="stylesheet" type="text/css" href="~/TS_platform/widgets/toolbar/css/dlg/toolbar.css">
</head>
<body class="egd-body-dlg" onload="init()">
    <form id="form1" runat="server">
        <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="3" class="egd-dlgBg-Img">
	        <tr> 
    	        <td valign="top">
			        <fieldset>
	        	        <legend>请输入登录口令：</legend>
				        <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
				            <tr>
						        <td style="height:4px">
						        </td>
					        </tr>
					        <tr>
						        <td class="lable" align="center" style="width:100%;text-align:left">
                                    请输入当前的口令：
						        </td>
					        </tr>
					        <tr>
						        <td align="center" style="width:100%">
                                    <asp:TextBox ID="TxtPassword0" TextMode="Password" runat="server" Width="100%"></asp:TextBox>
						        </td>
					        </tr>
					        <tr>
						        <td style="height:4px">
						        </td>
					        </tr>
				            <tr>
						        <td class="lable" align="center" style="width:100%;text-align:left">
                                    请输入一个新的口令：
						        </td>
					        </tr>
					        <tr>
						        <td align="center" style="width:100%">
                                    <asp:TextBox ID="TxtPassword1" TextMode="Password" runat="server" Width="100%"></asp:TextBox>
						        </td>
					        </tr>
					        <tr>
						        <td style="height:4px">
						        </td>
					        </tr>
					        <tr>
						        <td class="lable" align="center" style="width:100%;text-align:left">
                                    请再输入一次口令以确认：
						        </td>
					        </tr>
					        <tr>
						        <td align="center" style="width:100%">
                                    <asp:TextBox ID="TxtPassword2" TextMode="Password" runat="server" Width="100%"></asp:TextBox>
						        </td>
					        </tr>
				        </table>
        	        </fieldset>
	          </td>
	        </tr>
            <tr height="30">
      	        <td class="egd-dlg-btn-align">
					<div id="btn"></div>
    	        </td>
 	        </tr>
      </table>
    </form>
</body>
</html>
<script type="text/javascript" language="javascript">
// 关闭对话框
function btnClose_Click(){
	returnValue = null;
	window.close();
}

// 用户确认
function btnOk_Click(){
    // 判断两次输入的口令是否相同
    var p0 = $F("TxtPassword0");
    if(p0 == ""){
        MsgBox("对不起，请输入当前的口令。");
        return;
    }
    
    var p1 = $F("TxtPassword1");
    var p2 = $F("TxtPassword2");
    if(p1 == "" || p1 != p2){
        MsgBox("对不起，您输入的口令不匹配，请重新输入。");
        $("TxtPassword1").Value = "":
        $("TxtPassword2").Value = "":
        return;
    }
    var result = new Array();
	result[0] = p0;
	result[1] = p1;
	returnValue = result;
	window.close();
}
//按钮
var btn;
function init(){
	btn = new TS.TB("btn",[
		{
			id:"btnOk",
			text:"&nbsp;确 定&nbsp;",
			iconClass:"egd-button-btnOk",
			handler:function(button){
				btnOk_Click();
			},
			showIcon:false
		},
		{
			id:"btnCancel",
			text:"&nbsp;取 消&nbsp;",
			iconClass:"egd-button-cancel",
			handler:function(button){
				btnClose_Click();
			},
			showIcon:false
		}
	]);
}
</script>

