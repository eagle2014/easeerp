<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserView.aspx.cs" Inherits="TSCommon_Web.Organize.UserView" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=TSLib.SimpleResourceHelper.GetSystemTitle()%>
    </title>

    <script type="text/javascript" language="javascript">var contextPath = "<%=ContextPath %>";</script>

    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-ViewPage.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script type="text/javascript" src="<%=ContextPath %>/Organize/js/UserView.js"></script>

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
                                <td>
                                </td>
                                <!-- 表格 -->
                                <td>
                                    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
                                        <!-- 查询表单区 -->
                                        <tr valign="top" id="searchToolbar" style="display: none;">
                                            <td valign="top" height="70" class="egd-view-search-bg">
                                                <div class="egd-view-search-toolbarBg">
                                                    <div class="search-startSearch" onclick="thisPage.doSearch();">
                                                    </div>
                                                    <div class="search-cleanCondition" onclick="thisPage.doClean();">
                                                    </div>
                                                </div>
                                                <table border="0" id="conditionTB" cellpadding="0" cellspacing="3" class="egd-form-pageWidth">
                                                    <tr class="egd-form-emptyTR">
                                                        <td width="80">
                                                        </td>
                                                        <td width="160">
                                                        </td>
                                                        <td width="80">
                                                        </td>
                                                        <td width="160">
                                                        </td>
                                                        <td width="80">
                                                        </td>
                                                        <td width="160">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="egd-form-label">
                                                            登陆帐号：</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="LoginID" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="egd-form-label">
                                                            名&nbsp;&nbsp;&nbsp;&nbsp;称：</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="Name" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="egd-form-label">
                                                            职&nbsp;&nbsp;&nbsp;&nbsp;务：</td>
                                                        <td class="egd-property">
                                                            <asp:DropDownList ID="JobTitleUnid" runat="server" Width="100%" onchange="thisPage.changeJobTitle()">
                                                            </asp:DropDownList>
                                                           <asp:HiddenField ID="JobTitleName" runat="server" /> 
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="egd-form-label">
                                                            E-Mail：</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="Email" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="egd-form-label">
                                                            手机号码：</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="Mobile" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="egd-form-label">
                                                            工作电话：</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="TelephoneNo" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="egd-form-label">
                                                            工&nbsp;&nbsp;&nbsp;&nbsp;号：</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="CardID" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="egd-form-label">
                                                            联系地址：</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="Address" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="egd-form-label">
                                                            办&nbsp;公&nbsp;室：</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="Office" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="egd-form-label">
                                                            所属组织：</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="OUFullName" runat="server" ReadOnly="true" CssClass="egd-form-zdField"
                                                                Width="135px" />
                                                            <img src="<%=ContextPath %>/TS_platform/images/select/search_4.gif" alt="选择所属单位或部门" style="border: 0px;
                                                                vertical-align: middle; cursor: pointer;" onclick="thisPage.selectBelongOU()" />
                                                            <asp:HiddenField ID="OUUnid" runat="server" />
                                                        </td>
                                                        <td class="egd-form-label">
                                                            </td>
                                                        <td class="egd-property">
                                                        </td>
                                                        <td class="egd-form-label" colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none;">
                                                        <td class="egd-form-label">
                                                            政治面貌：</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="UserLigion" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="egd-form-label">
                                                            性别：</td>
                                                        <td class="egd-property">
                                                            <select id="Gender">
                                                                <option value="0">== 不限 ==</option>
                                                                <option value="1">男</option>
                                                                <option value="2">女</option>
                                                            </select>
                                                        </td>
                                                        <td class="egd-form-label" colspan="2">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="egd-page-content-grid-td">
                                                <div id="gridContainer" class="egd-page-content-gridContainer">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
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
