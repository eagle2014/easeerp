<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectUserByGroup.aspx.cs" Inherits="TSCommon.Web.Organize.dialog.SelectUserByGroup" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript">
        var contextPath = "<%=ContextPath %>";
    </script>

    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-Dlg.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script type="text/javascript" src="<%=ContextPath %>/Organize/js/Egd-Org.js"></script>

      <script type="text/javascript" src="~/TS_platform/ext/resources/css/ext-all.css"
        runat="server" />

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/js/Prototype.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/ext/adapter/ext/ext-base.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/ext/ext-all.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/ext/source/locale/ext-lang-zh_CN.js"></script>

    <script type="text/javascript">
        Ext.BLANK_IMAGE_URL = '<%=ContextPath %>/TS_platform/ext/resources/images/default/s.gif';
    </script>

    <script type="text/javascript">
        var my = {
            rootOUUnid: '<%=this.RootOUUnid %>',
            rootOUName: '<%=this.RootOUName %>',
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
            /* 岗位类型：0--全部类型，1--可派单岗位，2--不可派单岗位 */
            groupType: '<%=this.GroupType %>',
            /* 单选还是多选 */
            singleSelect: <%=("true".Equals(this.Request["singleSelect"], StringComparison.OrdinalIgnoreCase)).ToString().ToLower() %>,
            /* 当前岗位列表的Json */
            groupsJson: <%=this.GroupsJson %>
        };
    </script>

    <script type="text/javascript" src="<%=ContextPath %>/Organize/js/SelectUserInfoByGroup.js"></script>

    <style type="text/css">
		.egd-form-OptionsFF{height:222px;}
		.egd-form-OptionsIE{}
	</style>
</head>
<body class="egd-body-dlg" onload="thisPage.init()">
    <form id="thisForm" runat="server">
        <table width="400" height="320" border="0" cellspacing="4" cellpadding="0" class="egd-dlgBg-Img">
            <tr style="height: 20;">
                <td colspan="3">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="70">
                                选择组织：</td>
                            <td>
                                <asp:TextBox ID="OUName" Width="100%" runat="server"></asp:TextBox></td>
                            <td width="30">
                                <img src="../../TS_platform/images/select/search.gif" alt="选择组织" name="ImgSelectOU"
                                    align="absmiddle" id="ImgSelectOU" style="border: 0px; vertical-align: middle;
                                    cursor: pointer" onclick="thisPage.selectOU();" runat="server" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 20;">
                <td width="200px">
                    选择岗位：
                </td>
                <td width="200px">
                    选择用户：
                </td>
                <td>
                </td>
            </tr>
            <tr style="vertical-align: top;">
                <td width="200px">
                    <asp:ListBox ID="AllGroup" Rows="13" Width="100%" CssClass="egd-form-OptionsIE" onchange="thisPage.onChangeGroup(this)"
                        runat="server"></asp:ListBox>
                </td>
                <td width="200px">
                    <asp:ListBox ID="AllUserInfo" Rows="13" Width="100%" CssClass="egd-form-OptionsIE"
                        runat="server" ondblclick="TS.Dlg.fireButtonEvent(0,'selectUserInfoByGroup')" ></asp:ListBox>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="OUUnid" runat="server" />
    </form>
</body>
</html>

<script type="text/javascript">
var isIE = TS.isIE;
	if (!isIE){
	$("AllGroup").className="egd-form-OptionsFF";
	$("AllUserInfo").className="egd-form-OptionsFF";
	}else{
	$("AllGroup").className="egd-form-OptionsIE";
	$("AllUserInfo").className="egd-form-OptionsIE";
	}
</script>