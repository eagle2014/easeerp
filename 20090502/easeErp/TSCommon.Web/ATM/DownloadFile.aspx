<%@ Page Language="C#" AutoEventWireup="true" Codebehind="DownloadFile.aspx.cs" Inherits="TSCommon.Web.ATM.DownloadFile" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/TS_platform/css/Common.css" />

    <script type="text/javascript">var contextPath = "<%=ContextPath %>"; </script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/js/Prototype.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/js/TS.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/js/TS-MainPageUtils.js"></script>

</head>
<body onload="init()">
    <form id="thisForm" runat="server">
        <div>
        </div>
    </form>
</body>
</html>

<script type="text/javascript">
var errorMsg = '<%=this.ErrorMsg %>';
function init(){
    if(errorMsg.length > 0){
        TS.Msg.msgBox(errorMsg);   // 下载附件异常
    }
}
</script>

