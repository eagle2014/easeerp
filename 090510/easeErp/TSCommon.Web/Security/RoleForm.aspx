<%@ Page Language="C#" AutoEventWireup="true" Codebehind="RoleForm.aspx.cs" Inherits="TSCommon.Web.Security.RoleForm" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%= TSLib.SimpleResourceHelper.GetSystemTitle() %>
    </title>

    <script type="text/javascript" language="javascript">var contextPath = "<%=ContextPath %>";</script>

    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-FormPage.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script type="text/javascript" src="<%=ContextPath %>/Security/js/RoleForm.js"></script>

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
                                角色配置</td>
                        </tr>
                    </table>
                    <!-- 表单区 -->
                    <form id="thisForm" runat="server">
                        <div class="egd-page-form-wrap">
                            <!-- 表单边颜色和背景颜色 -->
                            <div class="egd-page-form-tableBorder egd-form-pageWidth">
                                <table width="100%" cellpadding="1" cellspacing="2">
                                    <tr class="egd-form-emptyTR">
                                        <td width="80">
                                        </td>
                                        <td width="270">
                                        </td>
                                        <td width="80">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            角色名称：</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="Name" CssClass="egd-form-btField" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="egd-form-label">
                                            角色代码：</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="Code" CssClass="egd-form-btField" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            级&nbsp;&nbsp;&nbsp;&nbsp;别：</td>
                                        <td colspan="3" class="egd-property">
                                            <asp:DropDownList ID="Level" runat="server" onchange="thisPage.changeLevel()">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            备注信息：</td>
                                        <td colspan="3" class="egd-property">
                                            <asp:TextBox ID="Memo" CssClass="egd-form-Field" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!-- 卡项开始 -->
                            <div id="tabContainer" class="egd-form-pageWidth egd-page_partitionTop">
                                <div id="divRole" class="egd-tab-hidden">
                                    <table width="100%" cellpadding="3" cellspacing="3">
                                        <tr>
                                            <td width="50%" class="egd-page-tabTitleTopBg">
                                                尚未拥有的权限列表</td>
                                            <td width="80">
                                            </td>
                                            <td width="50%" class="egd-page-tabTitleTopBg">
                                                已拥有的权限列表</td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="Models" runat="server" Width="100%" onchange="onModel_Changed()">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 8px">
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:ListBox ID="AllPrivilege" runat="server" Width="100%" Rows="11" SelectionMode="Multiple">
                                                            </asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="text-align: center; vertical-align: middle; width: 80px">
                                                <input id="btnRightArrow" type="button" class="form-button-default" value="添 加 &gt;"
                                                    onclick="IS_AppendSelectedItem('AllPrivilege', 'PrivilegeIDs', true)" runat="server" />
                                                <div>
                                                    &nbsp;</div>
                                                <input id="btnLeftArrow" type="button" class="form-button-default" value="&lt; 删 除"
                                                    onclick="IS_RemoveItem('PrivilegeIDs')" runat="server" />
                                                <div>
                                                    &nbsp;</div>
                                                <input id="btnLeftAllArrow" type="button" class="form-button-default" value="&lt;&lt; 清 空"
                                                    onclick="IS_RemoveAllItem('PrivilegeIDs')" runat="server" />
                                            </td>
                                            <td>
                                                <asp:ListBox ID="PrivilegeIDs" runat="server" Width="100%" Rows="12" SelectionMode="Multiple">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <asp:HiddenField ID="_ID" runat="server" />
                            <asp:HiddenField ID="Unid" runat="server" />
                            <asp:HiddenField ID="IsInner" runat="server" />
                            <asp:HiddenField ID="LevelName" runat="server" />
                        </div>
                    </form>
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