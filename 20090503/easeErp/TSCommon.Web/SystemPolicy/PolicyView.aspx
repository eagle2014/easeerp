<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PolicyView.aspx.cs" Inherits="TSCommon.Web.SystemPolicy.PolicyView" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%= TSLib.SimpleResourceHelper.GetSystemTitle() %>
    </title>
    <script type="text/javascript" language="javascript">var contextPath = "<%=ContextPath %>";</script>

    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-ViewPage.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script type="text/javascript" src="<%=ContextPath %>/SystemPolicy/js/ViewPage.js"></script>

</head>
<body onload="thisPage.init();">
    <form id="form1" runat="server">
        <div>
            <table class="egd-page-table" cellspacing="0" cellpadding="0">
                <!-- 工具条区 -->
                <tr class="egd-page-tb-tr">
                    <td class="egd-page-tb-td">
                        <div id="tbContainer" class="egd-page-content-tbContainer">
                        </div>
                    </td>
                </tr>
                <!-- 内容区 -->
                <tr class="egd-page-content-tr">
                    <td class="egd-page-content-td">
                        <table class="egd-page-table" cellspacing="0" cellpadding="0">
                            <tr>
                                <!-- 表格 -->
                                <td class="egd-page-content-grid-td">
                                    <div id="gridContainer" class="egd-page-content-gridContainer">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

