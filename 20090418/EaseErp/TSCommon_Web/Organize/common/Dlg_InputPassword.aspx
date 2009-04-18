<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dlg_InputPassword.aspx.cs" Inherits="TSCommon_Web.Organize.common.Dlg_InputPassword" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
	<title><%= TSLib.SimpleResourceHelper.GetSystemTitle() %></title>
	<script src="<%=ContextPath %>/TS_platform/js/TS-Include-Dlg.js?rootPath=<%=ContextPath %>/" type="text/javascript"></script>
	<script type="text/javascript" language="javascript">var contextPath="<%=ContextPath %>/";</script>

    <link rel="stylesheet" type="text/css" href="~/TS_platform/ext/resources/css/ext-all.css" runat="server" />

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/ext/adapter/ext/ext-base.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/ext/ext-all.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/ext/source/locale/ext-lang-zh_CN.js"></script>

    <script type="text/javascript">
        Ext.BLANK_IMAGE_URL = '<%=ContextPath %>/TS_platform/ext/resources/images/default/s.gif';
    </script>
</head>
<body class="egd-form-dlgBg-WB">
    <form id="form1" runat="server">
        <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="3" class="egd-dlgBg-Img">
	        <tr> 
    	        <td valign="top">

				        <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
				            <tr>
						        <td style="height:4px">
						        </td>
					        </tr>
				            <tr>
						        <td class="egd-form-label" style="text-align:left;">输入一个新口令：</td>
					        </tr>
					        <tr>
						        <td class="egd-property"><asp:TextBox ID="TxtPassword1" TextMode="Password" runat="server" CssClass="egd-form-field"></asp:TextBox>
						        </td>
					        </tr>
					        <tr>
						        <td class="egd-form-label" style="text-align:left;">再输入一次口令以确认：</td>
					        </tr>
					        <tr>
						        <td class="egd-property"><asp:TextBox ID="TxtPassword2" TextMode="Password" runat="server" CssClass="egd-form-field"></asp:TextBox>
						        </td>
					        </tr>
				        </table>
	          </td>
	        </tr>          
      </table>
    </form>
</body>
</html>
<script type="text/javascript" language="javascript">
/*
 * 返回选中值的函数
 */
function DlgOkFn(){
   // 判断两次输入的口令是否相同
    var p1 = $F("TxtPassword1");
    var p2 = $F("TxtPassword2");
    if(p1 == "" || p1 != p2){
     TS.Msg.msgBox({title:"口令修改", msg:"对不起，您输入的口令不匹配，请重新输入！", iconCls: TS.Msg.IconCls.WARNING});
        $("TxtPassword1").Value = "";
        $("TxtPassword2").Value = "";
        return;
    }
    var result = new Array();
	result[0] = p1;
	return  result;
}
</script>
