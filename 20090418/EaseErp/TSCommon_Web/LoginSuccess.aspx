<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginSuccess.aspx.cs" Inherits="TSCommon_Web.LoginSuccess" %>
<% 
    string fullwindow = TSLib.SimpleResourceHelper.GetString("SYSTEM.LOGIN.FULLWINDOW");
    if (fullwindow == null || !fullwindow.Equals("true",StringComparison.OrdinalIgnoreCase)) fullwindow = "false";
%>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%= TSLib.SimpleResourceHelper.GetSystemTitle() %></title>
</head>
<body onload="forwardMainPage()">
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
var fullwindow = <%=fullwindow %>;
function forwardMainPage(){
	var strUrl = "<%=ContextPath %>/TS_MainPage/Default.aspx";
	var width = screen.availWidth;
	var height = screen.availHeight;
	//alert(strUrl);
	opener = null;
	if (fullwindow){
	    window.open(strUrl,'','left=0, top=0,width='+width+',height='+height+',resizable=yes,status=yes,toolsbar=no,scrollbars=yes');
	    window.close();
	}else{
		window.open(strUrl,'_self');
	}
}
</script>