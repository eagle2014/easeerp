<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectUserInfoByOU.aspx.cs" Inherits="TSCommon.Web.Organize.dialog.SelectUserInfoByOU" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript">
        var contextPath = "<%=ContextPath %>";
    </script>

    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-Dlg.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script type="text/javascript" src="~/TS_platform/ext/resources/css/ext-all.css"
        runat="server" />

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
            /* 用户类型：""--全部，"0"--普通用户，"1"--服务台用户，"2"--服务支持用户 */
            userType: '<%=this.Request["userType"] != null ? this.Request["userType"] : "" %>',
            /* 查看范围:all--全部/ */
            type: '<%=this.Request["type"] != null ? this.Request["type"] : "" %>',
            /* 根节点是否可见 */
            rootVisible: <%=("true".Equals(this.Request["rootVisible"], StringComparison.OrdinalIgnoreCase)).ToString().ToLower() %>,
            /* 单选还是多选 */
            singleSelect: <%=("true".Equals(this.Request["singleSelect"], StringComparison.OrdinalIgnoreCase)).ToString().ToLower() %>,
            /* 当前人员列表的Json */
            userInfosJson: <%=this.UserInfosJson %>
        };
    </script>

    <script type="text/javascript" src="<%=ContextPath %>/Organize/js/SelectUserInfoByOU.js"></script>

    <style>
		.egd-form-OptionsFF{height:247px;}
		.egd-form-OptionsIE{}
	</style>
</head>
<body class="egd-body-dlg" onLoad="thisPage.init()">
    <form id="thisForm" runat="server">
        <div>
            <table width="400" height="250" border="0" cellspacing="4" cellpadding="0" class="egd-dlgBg-Img">
                <tr style="height: 20;">
                    <td width="200px">
                        <div class="egd-form-dlg-areaTitle">选择单位或部门</div>
                    </td>
                    <td width="200px">
                   	 <div class="egd-form-dlg-areaTitle">选择用户</div>
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
                            Rows="15" ondblclick="TS.Dlg.fireButtonEvent(0,'selectUserInfoByOU')" ></asp:ListBox>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
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