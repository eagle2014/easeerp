<%@ Page Language="C#" AutoEventWireup="true" Codebehind="UserForm.aspx.cs" Inherits="TSCommon_Web.Organize.UserForm" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%= TSLib.SimpleResourceHelper.GetSystemTitle() %>
    </title>

    <script type="text/javascript" language="javascript">var contextPath = "<%=ContextPath %>";</script>

    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-FormPage.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script src="<%=ContextPath %>/Organize/js/TS-Org.js" type="text/javascript"></script>

    <script type="text/javascript" src="<%=ContextPath %>/Organize/js/UserForm.js"></script>

</head>
<body class="egd-body" onload="thisPage.init();">
    <table class="egd-page-table" cellspacing="0" cellpadding="0" border="0">
        <!-- 工具条区 -->
        <tr class="egd-page-tb-tr">
            <td class="egd-page-tb-td">
                <div id="tbContainer" class="egd-page-content-tbContainer">
                </div>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <!-- 自动生成滚动条 -->
                <div class="egd-page-overflow egd-form-align">
                    <!-- 大标题区 -->
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="egd-form-title-warp egd-form-pageWidth">
                        <tr>
                            <td class="egd-page-title-text">
                                人员配置</td>
                        </tr>
                    </table>
                    <!-- 表单区 -->
                    <div class="egd-page-form-wrap">
                        <!-- 表单边颜色和背景颜色 -->
                        <form id="thisForm" runat="server" class="egd-form">
                            <div class="egd-page-form-tableBorder egd-form-pageWidth">
                                <table width="100%" border="0" cellpadding="1" cellspacing="2">
                                    <tr class="egd-form-emptyTR">
                                        <td width="80">
                                        </td>
                                        <td width="260">
                                        </td>
                                        <td width="80">
                                        </td>
                                        <td width="270">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            所属组织：</td>
                                        <td colspan="3" class="egd-property">
                                            <asp:TextBox ID="OUFullName" CssClass="egd-form-zdField" runat="server" Width="600px"
                                                ReadOnly="true"></asp:TextBox>
                                            <img id="ImgBtnSelectOU" alt="选择所属单位或部门" style="vertical-align: middle; cursor: hand"
                                                src="../TS_platform/images/select/search.gif" runat="server" onclick="thisPage.selectBelongOU()" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            姓&nbsp;&nbsp;&nbsp;&nbsp;名：</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="Name" CssClass="egd-form-btField" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="egd-form-label">
                                            登录帐号：</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="LoginID" CssClass="egd-form-btField" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            工&nbsp;&nbsp;&nbsp;&nbsp;号：</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="CardID" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="egd-form-label">
                                            排序序号：</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="OrderNo" CssClass="egd-form-btField" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            职&nbsp;&nbsp;&nbsp;&nbsp;务：</td>
                                        <td class="egd-property">
                                            <asp:DropDownList ID="JobTitleUnid" runat="server" Width="100%" onchange="thisPage.changeJobTitle()">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="egd-form-label">
                                            工作电话：</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="TelephoneNo" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            手机号码：</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="Mobile" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="egd-form-label">
                                            E-Mail：</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="Email" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            办&nbsp;公&nbsp;室：</td>
                                        <td colspan="3" class="egd-property">
                                            <asp:TextBox ID="Office" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            联系地址：</td>
                                        <td colspan="3" class="egd-property">
                                            <asp:TextBox ID="Address" CssClass="egd-form-field" runat="server" Width="100%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            用户类型：</td>
                                        <td colspan="3">
                                            <asp:RadioButtonList ID="UserType" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="普通用户" Value="0" Selected="True" />
                                                <asp:ListItem Text="服务台" Value="1" />
                                                <asp:ListItem Text="服务支持" Value="2" />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!--卡项开始 -->
                            <div id="tabContainer" class="egd-form-pageWidth egd-page_partitionTop">
                                <div id="divGroup" class="egd-tab-hidden">
                                    <table cellpadding="3" cellspacing="3">
                                        <tr>
                                            <td class="egd-page-tabTitleTopBg" style="width: 320px;">
                                                尚未拥有的岗位列表</td>
                                            <td style="width: 80px">
                                            </td>
                                            <td class="egd-page-tabTitleTopBg">
                                                已拥有的岗位列表</td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top; width: 320px;">
                                                <asp:ListBox ID="AllGroups" runat="server" Width="100%" Rows="9" SelectionMode="Multiple">
                                                </asp:ListBox>
                                            </td>
                                            <td style="text-align: center; vertical-align: middle; width: 80px">
                                                <input id="btnRightArrow" type="button" class="form-button-default" value="添 加 &gt;"
                                                    onclick="IS_AppendSelectedItem('AllGroups', 'GroupUnids', true)" runat="server" />
                                                <div>
                                                    &nbsp;</div>
                                                <input type="button" class="form-button-default" id="btnLeftArrow" onclick="IS_AppendSelectedItem('GroupUnids', 'AllGroups', true)"
                                                    value="&lt; 删 除" runat="server" />
                                                <div>
                                                    &nbsp;</div>
                                                <input id="btnLeftAllArrow" type="button" class="form-button-default" value="&lt;&lt; 清 空"
                                                    onclick="IS_AppendAllItem('GroupUnids', 'AllGroups', true)" runat="server" />
                                            </td>
                                            <td>
                                                <asp:ListBox ID="GroupUnids" runat="server" Width="100%" Rows="9" SelectionMode="Multiple">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <asp:HiddenField ID="_ID" runat="server" />
                            <asp:HiddenField ID="Unid" runat="server" />
                            <asp:HiddenField ID="UserStatus" runat="server" />
                            <asp:HiddenField ID="Password" runat="server" />
                            <asp:HiddenField ID="FullName" runat="server" />
                            <asp:HiddenField ID="Gender" runat="server" />
                            <asp:HiddenField ID="Birthday" runat="server" />
                            <asp:HiddenField ID="UnitUnid" runat="server" />
                            <asp:HiddenField ID="UnitName" runat="server" />
                            <asp:HiddenField ID="UnitFullName" runat="server" />
                            <asp:HiddenField ID="UnitCode" runat="server" />
                            <asp:HiddenField ID="UnitFullCode" runat="server" />
                            <asp:HiddenField ID="OUUnid" runat="server" />
                            <asp:HiddenField ID="OUCode" runat="server" />
                            <asp:HiddenField ID="OUFullCode" runat="server" />
                            <asp:HiddenField ID="OUName" runat="server" />
                            <asp:HiddenField ID="JobTypeUnid" runat="server" />
                            <asp:HiddenField ID="JobTypeName" runat="server" />
                            <asp:HiddenField ID="JobTitleName" runat="server" />
                            <asp:HiddenField ID="UserLigion" runat="server" />
                            <asp:HiddenField ID="IsTmpUser" runat="server" />
                            <asp:HiddenField ID="ValidityStartDate" runat="server" />
                            <asp:HiddenField ID="ValidityEndDate" runat="server" />
                            <asp:HiddenField ID="Field_1" runat="server" />
                            <asp:HiddenField ID="Field_2" runat="server" />
                            <asp:HiddenField ID="Field_3" runat="server" />
                            <asp:HiddenField ID="Field_4" runat="server" />
                            <asp:HiddenField ID="Field_5" runat="server" />
                            <asp:HiddenField ID="EmployeeID" runat="server" />
                            <asp:HiddenField ID="DepartmentID" runat="server" />
                        </form>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</body>
</html>

<script type="text/javascript" language="javascript">
	var my = {
        // 当前用户是否是管理员
	    isManager: <%=this.IsManager.ToString().ToLower() %>,
        
        // 当前页面是否处于可编辑状态
	    canEdit: <%=this.CanEdit.ToString().ToLower() %>,
	    //该记录是否已保存
	    isUnidEmpty:<%=this.IsUnidEmpty.ToString().ToLower() %>
    };

// 激活当前用户
function onEnabled_Click(){
    var strUrl = "<%=ContextPath %>/Organize/UserForm.aspx?action=enabledUser";
	strUrl +="&timeStamp=" + new Date().getTime();
//	IS_SelectAllTotal();
	document.forms[0].action = strUrl;
	document.forms[0].target = "_self";
	document.forms[0].submit();
}

// 禁用当前用户
function onDisabled_Click(){
    var strUrl = "<%=ContextPath %>/Organize/UserForm.aspx?action=disabledUser";
	strUrl +="&timeStamp=" + new Date().getTime();
//	IS_SelectAllTotal();
	document.forms[0].action = strUrl;
	document.forms[0].target = "_self";
	document.forms[0].submit();
}
</script>

