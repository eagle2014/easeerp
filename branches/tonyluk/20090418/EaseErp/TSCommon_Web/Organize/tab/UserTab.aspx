<%@ Page Language="C#" AutoEventWireup="true" Codebehind="UserTab.aspx.cs" Inherits="TSCommon_Web.Organize.tab.UserTab" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=TSLib.SimpleResourceHelper.GetSystemTitle()%>
    </title>

    <script type="text/javascript" language="javascript">var contextPath = "<%=ContextPath %>";</script>

    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-ViewPage.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script language="javascript" type="text/javascript">        
        function getDataUrl(){
            return '<%=ContextPath %>/userAction.do?action=GetGridData&otherUnid=<%=Request.Params["otherUnid"] %>&otherType=<%=Request.Params["otherType"] %>';
        }
    </script>

    <script type="text/javascript" src="<%=ContextPath %>/Organize/js/UserTab.js"></script>

</head>
<body onload="init();">
    <form id="form1" runat="server">
        <table class="egd-page-table" cellspacing="0" cellpadding="0" width="99%">
            <!-- 工具条区 -->
            <tr>
                <td class="egd-toolbar-container">
                    <div id="tbContainer">
                    </div>
                </td>
            </tr>
            <!-- 内容区 -->
            <tr class="egd-page-content-tr">
                <td class="egd-page-content-td">
                    <!-- 表格 -->
                    <div id="gridContainer" class="egd-page-content-gridContainer">
                    </div>
                </td>
            </tr>
        </table>
        <input type="hidden" id="otherUnid" runat="server" />
        <input type="hidden" id="otherType" runat="server" />
    </form>
</body>
</html>

<script language="javascript" type="text/javascript">
function init(){
    pageInit();
    var flag='<%= flag %>';
    if(flag=="True")
    {
        toolbar.buttons.each(function(button){
            button.show();
        });
    }else{
        toolbar.buttons.each(function(button){
            button.hide();
        });
    }
}
</script>

