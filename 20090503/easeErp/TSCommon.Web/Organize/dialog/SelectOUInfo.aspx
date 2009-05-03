<%@ Page Language="C#" AutoEventWireup="true" Codebehind="SelectOUInfo.aspx.cs" Inherits="TSCommon.Web.Organize.dialog.SelectOUInfo" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript">
        var contextPath = "<%=ContextPath %>";
    </script>

    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-Dlg.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <link rel="stylesheet" type="text/css" href="~/TS_platform/ext/resources/css/ext-all.css" />

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/js/Prototype.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/ext/adapter/ext/ext-base.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/ext/ext-all.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/ext/source/locale/ext-lang-zh_CN.js"></script>

    <script type="text/javascript">
        Ext.BLANK_IMAGE_URL = '<%=ContextPath %>/TS_platform/ext/resources/images/default/s.gif';
    </script>

    <script type="text/javascript">
        var my = {
            rootOUUnid: '<%=this.Request["rootOUUnid"] %>',
            rootOUName: unescape('<%=this.Request["rootOUName"] %>'),
            rootIconCls: '<%=this.Request["rootIconCls"] != null ? this.Request["rootIconCls"] : "" %>',
            singleClickExpand: <%=("true".Equals(this.Request["singleClickExpand"], StringComparison.OrdinalIgnoreCase)).ToString().ToLower() %>,
            /* ou类型：DW--单位/BM--部门/全部 */
            ouType: '<%=this.Request["ouType"] != null ? this.Request["ouType"] : "" %>',
            /* 查看范围:all--全部/ */
            type: '<%=this.Request["type"] != null ? this.Request["type"] : "" %>',
            /* 根节点是否可见 */
            rootVisible: <%=("true".Equals(this.Request["rootVisible"], StringComparison.OrdinalIgnoreCase)).ToString().ToLower() %>,
            /* 单选还是多选 */
            singleSelect: <%=("true".Equals(this.Request["singleSelect"], StringComparison.OrdinalIgnoreCase)).ToString().ToLower() %>
        };
    </script>

    <script type="text/javascript" src="<%=ContextPath %>/Organize/js/SelectOUInfo.js"></script>

</head>
<body class="egd-body-dlg" onload="thisPage.init()" style="background-color: #CCC;">
    <form id="thisForm" runat="server">
        <div id="Div1" class="egd-page-overflow egd-tree-wrap" style="background-color: #FFFFFF;">
            <div id="treeContainer" style="height: 100%; width: 100%;">
            </div>
        </div>
    </form>
</body>
</html>
