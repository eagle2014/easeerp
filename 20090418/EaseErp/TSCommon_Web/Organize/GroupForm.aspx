<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupForm.aspx.cs" Inherits="TSCommon_Web.Organize.GroupForm" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%= TSLib.SimpleResourceHelper.GetSystemTitle() %>
    </title>
    <script type="text/javascript" language="javascript">var contextPath = "<%=ContextPath %>";</script>

    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-FormPage.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script type="text/javascript" src="<%=ContextPath %>/Organize/js/GroupForm.js"></script>

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
                                岗位配置</td>
                        </tr>
                    </table>
                    <!-- 表单区 -->
                    <div class="egd-page-form-wrap">
                        <!-- 表单边颜色和背景颜色 -->
                        <form id="thisForm" runat="server">
                            <div class="egd-page-form-tableBorder egd-form-pageWidth">
                                <table width="100%" cellpadding="1" cellspacing="2">
                                    <tr class="egd-form-emptyTR">
                                        <td width="80">
                                        </td>
                                        <td width="260">
                                        </td>
                                        <td width="80">
                                        </td>
                                        <td width="260">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            所属组织：</td>
                                        <td colspan="3" class="egd-property">
                                            <asp:TextBox ID="OUFullName" CssClass="egd-form-btField" ReadOnly="true" runat="server" Width="600px"></asp:TextBox>
                                            <img id="ImgBtnSelectOU" alt="选择所属单位或部门" style="vertical-align: middle; cursor: hand"
                                                src="../TS_platform/images/select/search.gif" runat="server" onclick="thisPage.selectBelongOU()" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            名&nbsp;&nbsp;&nbsp;&nbsp;称：</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="Name" CssClass="egd-form-btField" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="egd-form-label">
                                            编&nbsp;&nbsp;&nbsp;&nbsp;码：</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="Code" CssClass="egd-form-btField" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            岗位类型：</td>
                                        <td colspan="3">
                                            <asp:RadioButtonList ID="IsCanDispatch" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="允许派单到本岗位" Value="Y" Selected="True" />
                                                <asp:ListItem Text="不允许派单到本岗位" Value="N" />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            备注信息：</td>
                                        <td colspan="3" class="egd-property">
                                            <asp:TextBox ID="Memo" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!-- 页签区 -->
                            <div id="tabContainer" class="egd-form-pageWidth egd-page_partitionTop">
                                <div id="divRole" class="egd-tab-hidden">
                                    <table width="100%" border="0" cellpadding="3" cellspacing="3">
                                        <tr>
                                            <td width="50%" class="egd-page-tabTitleTopBg">
                                                尚未拥有的角色列表</td>
                                            <td style="width: 80px">
                                            </td>
                                            <td width="50%" class="egd-page-tabTitleTopBg">
                                                已拥有的角色列表</td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top">
                                                <asp:ListBox ID="AllRoles" runat="server" Width="100%" Rows="9" SelectionMode="Multiple">
                                                </asp:ListBox>
                                            </td>
                                            <td style="text-align: center; vertical-align: middle; width: 80px">
                                                <input id="btnRightArrow" type="button" class="form-button-default" value="添 加 &gt;"
                                                    onclick="IS_AppendSelectedItem('AllRoles', 'RoleUnids', true)" runat="server" />
                                                <div>
                                                    &nbsp;</div>
                                                <input id="btnLeftArrow" type="button" class="form-button-default" value="&lt; 删 除"
                                                    onclick="IS_AppendSelectedItem('RoleUnids', 'AllRoles', true)" runat="server" />
                                                <div>
                                                    &nbsp;</div>
                                                <input id="btnLeftAllArrow" type="button" class="form-button-default" value="&lt;&lt; 清 空"
                                                    onclick="IS_AppendAllItem('RoleUnids', 'AllRoles', true)" runat="server" />
                                            </td>
                                            <td>
                                                <asp:ListBox ID="RoleUnids" runat="server" Width="100%" Rows="9" SelectionMode="Multiple">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divMember" class="egd-tab-hidden">
                                    <table width="100%" border="0" cellpadding="3" cellspacing="3">
                                        <tr>
                                            <td width="50%" class="egd-page-tabTitleTopBg">
                                                尚未包含的人员列表</td>
                                            <td style="width: 80px">
                                            </td>
                                            <td width="50%" class="egd-page-tabTitleTopBg">
                                                已包含的人员列表</td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="OUInfos" runat="server" Width="100%" onchange="onOUInfos_Changed()">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 8px">
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:ListBox ID="AllUserInfos" runat="server" Width="100%" Rows="9" SelectionMode="Multiple">
                                                            </asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="text-align: center; vertical-align: middle; width: 80px">
                                                <input id="btnRightArrow_1" type="button" class="form-button-default" value="添 加 &gt;"
                                                    onclick="IS_AppendSelectedItem('AllUserInfos', 'UserInfoUnids', true)" runat="server" />
                                                <div>
                                                    &nbsp;</div>
                                                <input id="btnLeftArrow_1" type="button" class="form-button-default" value="&lt; 删 除"
                                                    onclick="IS_RemoveItem('UserInfoUnids')" runat="server" />
                                                <div>
                                                    &nbsp;</div>
                                                <input id="btnLeftAllArrow_1" type="button" class="form-button-default" value="&lt;&lt; 清 空"
                                                    onclick="IS_RemoveAllItem('UserInfoUnids')" runat="server" />
                                            </td>
                                            <td>
                                                <asp:ListBox ID="UserInfoUnids" runat="server" Width="100%" Rows="11" SelectionMode="Multiple">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <!--
                                <div id="divWorkLog" class="egd-tab-hidden">
                                    <iframe id="WORKLOG" scrolling="no" frameborder="0" class="egd-page-tab_IW" src="<%=ContextPath %>/egd_platform/common/Waiting.html"
                                        style="height: 200px; width: 99.7%;"></iframe>
                                </div>
                                -->
                            </div>
                            <asp:HiddenField ID="_ID" runat="server" />
                            <asp:HiddenField ID="Unid" runat="server" />
                            <asp:HiddenField ID="GroupStatus" runat="server" />
                            <asp:HiddenField ID="RankUnid" runat="server" />
                            <asp:HiddenField ID="RankName" runat="server" />
                            <asp:HiddenField ID="IsInner" runat="server" />
                            <asp:HiddenField ID="OUUnid" runat="server" />
                            <asp:HiddenField ID="OUCode" runat="server" />
                            <asp:HiddenField ID="OUName" runat="server" />
                            <asp:HiddenField ID="OUFullCode" runat="server" />
                            <asp:HiddenField ID="Field_1" runat="server" />
                            <asp:HiddenField ID="Field_2" runat="server" />
                            <asp:HiddenField ID="Field_3" runat="server" />
                            <asp:HiddenField ID="Field_4" runat="server" />
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
	    canEdit: <%=this.CanEdit.ToString().ToLower() %>
    };
</script>