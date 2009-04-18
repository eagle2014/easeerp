<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DepartmentView.aspx.cs" Inherits="TSCommon_Web.Organize.DepartmentView" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%= TSLib.SimpleResourceHelper.GetSystemTitle() %>
    </title>
    <script type="text/javascript" language="javascript">var contextPath = "<%=ContextPath %>";</script>

    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-ViewPage.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script type="text/javascript" src="<%=ContextPath %>/Organize/js/DepartmentView.js"></script>

</head>
<body class="egd-body" onload="thisPage.init();">
    <form id="thisForm" runat="server">
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
                                <!-- 导航树 -->
                                <td class="egd-page-content-tree-td">
                                    <div id="treeContainer" class="egd-page-content-treeContainer">
                                    </div>
                                </td>
                                <!-- 分隔条 -->
                                <td class="egd-page-content-seperator-td">
                                </td>
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

<script type="text/javascript" language="javascript">
	var my = {
        // 当前用户是否是管理员
	    isManager: <%=this.IsManager.ToString().ToLower() %>
    };
</script>
