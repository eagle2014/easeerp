<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OptionItemForm.aspx.cs" Inherits="TSCommon_Web.OptionItemCfg.OptionItemForm" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title>
        <%= TSLib.SimpleResourceHelper.GetSystemTitle() %>
    </title>

    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-FormPage.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script type="text/javascript" language="javascript">rootPath="<%=ContextPath %>/";</script>

    <script type="text/javascript" src="<%=ContextPath %>/OptionItemCfg/js/OptionItemForm.js"></script>

</head>
<body class="egd-body" onload="thisPage.init();">
    <table class="egd-page-table" cellspacing="0" cellpadding="0" border="0">
        <!-- 工具条区 -->
        <tr class="egd-page-tb-tr">
            <td class="egd-page-tb-td">
                <div id="Div1" class="egd-page-content-tbContainer">
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
                                可选项配置</td>
                        </tr>
                    </table>
                    <!-- 表单区 -->
                    <div class="egd-page-form-wrap">
                        <!-- 表单边颜色和背景颜色 -->
                        <div class="egd-page-form-tableBorder egd-form-pageWidth">
                            <form id="Form1" runat="server" class="egd-form">
                                <table width="100%" cellpadding="1" cellspacing="2">
                                    <tr class="egd-form-emptyTR">
                                        <td width="80">
                                        </td>
                                        <td width="250">
                                        </td>
                                        <td width="80">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            所属分类：</td>
                                        <td class="egd-property">
                                            <asp:DropDownList ID="Type" CssClass="egd-form-btField" runat="server" onchange="thisPage.changeType()">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="egd-form-label">
                                            排序序号：</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="OrderNo" CssClass="egd-form-field" runat="server"></asp:TextBox>
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
                                </table>
                                <asp:HiddenField ID="_ID" runat="server" />
                                <asp:HiddenField ID="Unid" runat="server" />
                                <asp:HiddenField ID="TypeName" runat="server" />
                                <asp:HiddenField ID="FileDate" runat="server" />
                                <asp:HiddenField ID="LastModifiedDate" runat="server" />
                                <asp:HiddenField ID="LastChanger_Unid" runat="server" />
                                <asp:HiddenField ID="LastChanger_Name" runat="server" />
                                <asp:HiddenField ID="Author_Unid" runat="server" />
                                <asp:HiddenField ID="Author_Name" runat="server" />
                                <asp:HiddenField ID="Author_OUUnid" runat="server" />
                                <asp:HiddenField ID="Author_OUName" runat="server" />
                                <asp:HiddenField ID="Author_OUCode" runat="server" />
                                <asp:HiddenField ID="Author_OUFullName" runat="server" />
                                <asp:HiddenField ID="Author_OUFullCode" runat="server" />
                            </form>
                        </div>
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
