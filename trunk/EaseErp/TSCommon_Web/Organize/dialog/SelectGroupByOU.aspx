<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectGroupByOU.aspx.cs" Inherits="TSCommon_Web.Organize.dialog.SelectGroupByOU" %>

<html>
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript">
        var contextPath = "<%=ContextPath %>";
    </script>

    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-Dlg.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script type="text/javascript" src="~/TS_platform/ext/resources/css/ext-all.css" runat="server" />

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/js/Prototype.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/ext/adapter/ext/ext-base.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/ext/ext-all.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/ext/source/locale/ext-lang-zh_CN.js"></script>

    <script type="text/javascript">
        Ext.BLANK_IMAGE_URL = '<%=ContextPath %>/egd_platform/ext/resources/images/default/s.gif';
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
            /* 岗位类型：0--全部类型，1--可派单岗位，2--不可派单岗位 */
            groupType: '<%=this.Request["groupType"] != null ? this.Request["groupType"] : "0" %>',
            /* 单选还是多选 */
            singleSelect: <%=("true".Equals(this.Request["singleSelect"], StringComparison.OrdinalIgnoreCase)).ToString().ToLower() %>,
            /* 当前岗位列表的Json */
            groupsJson: <%=this.GroupsJson %>
        };
    </script>

    <script type="text/javascript" src="<%=ContextPath %>/Organize/js/SelectGroupByOU.js"></script>

    <style type="text/css">
		.egd-form-OptionsFF{height:247px;}
		.egd-form-OptionsIE{}
	</style>
</head>
<body class="egd-body-dlg" onLoad="thisPage.init()">
    <form id="thisForm" runat="server">
        <table width="400" height="300" border="0" cellspacing="4" cellpadding="0" class="egd-dlgBg-Img">
            <tr style="height: 20;">
                <td width="200px">
                    <div class="egd-form-dlg-areaTitle">选择单位或部门</div>
                </td>
                <td width="200px">
                <div class="egd-form-dlg-areaTitle">选择岗位</div>
                </td>
                <td>
                </td>
            </tr>
            <tr style="vertical-align: top;">
                <td width="200px">
                    <div id="Div1" class="egd-page-overflow egd-tree-wrap" style="background-color: #FFFFFF;
                        width: 190px; height: 245px;">
                        <div id="treeContainer">
                        </div>
                    </div>
                </td>
                <td width="200px">
                    <asp:ListBox ID="Options" Width="100%" CssClass="egd-form-OptionsIE" runat="server"
                        Rows="15" ondblclick="Egd.Dlg.fireButtonEvent(0,'selectGroupByOU')"></asp:ListBox>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

<script type="text/javascript">
var isIE = TS.isIE;
	if (!isIE){
	$("Options").className="egd-form-OptionsFF";
	}else{
	$("Options").className="egd-form-OptionsIE";
	}
</script>

