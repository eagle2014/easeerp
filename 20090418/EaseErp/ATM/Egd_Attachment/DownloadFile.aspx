<%@ Page Language="C#" AutoEventWireup="true" Inherits="Egrand.Attachment.Web.DownloadFile"
    Codebehind="DownloadFile.aspx.cs" %>

<%@ Register Assembly="Egrand.Core" Namespace="Egrand.Web.UI.ScriptControls" TagPrefix="egdScript" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <egdScript:IncludeCSS Href="~/egd_platform/css/Common.css" runat="server" />

    <script type="text/javascript">var contextPath = "<%=ContextPath %>"; </script>

    <egdScript:IncludeJS Src="~/egd_platform/js/Prototype.js" runat="server" />
    <egdScript:IncludeJS Src="~/egd_platform/js/Egd.js" runat="server" />
    <egdScript:IncludeJS Src="~/egd_platform/js/Egd-MainPageUtils.js" runat="server" />
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
        Egd.Msg.msgBox(errorMsg);   // œ¬‘ÿ∏Ωº˛“Ï≥£
    }
}
</script>

