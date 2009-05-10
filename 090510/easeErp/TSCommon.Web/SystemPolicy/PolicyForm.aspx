<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PolicyForm.aspx.cs" Inherits="TSCommon.Web.SystemPolicy.PolicyForm" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%= TSLib.SimpleResourceHelper.GetSystemTitle() %>
    </title>

    <script type="text/javascript" language="javascript">var contextPath = "<%=ContextPath %>";</script>

    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-FormPage.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script type="text/javascript">
	var my = {        
        // 当前页面是否处于可编辑状态
	    canEdit: <%=this.CanEdit.ToString().ToLower() %>
    };        
    </script>

    <script src="<%=ContextPath %>/SystemPolicy/js/FormPage.js" type="text/javascript"></script>

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
                    <form id="thisForm" runat="server" class="egd-form">
                        <!-- 大标题区 -->
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="egd-form-title-warp egd-form-pageWidth">
                            <tr>
                                <td class="egd-page-title-text">
                                    系统策略 (编号：<span id="codeInfo"><%=Domain.Code %></span>)</td>
                            </tr>
                        </table>
                        <!-- 表单区 -->
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
                                            所属模块：</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="BelongModule" ReadOnly="true" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="egd-form-label">
                                            排序序号：</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="OrderNo" ReadOnly="true" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            策略名称：</td>
                                        <td colspan="3" class="egd-property">
                                            <asp:TextBox ID="Subject" ReadOnly="true" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="TR_TEXT" runat="server">
                                        <td class="egd-form-label">
                                            策&nbsp;略&nbsp;值：</td>
                                        <td colspan="3" class="egd-property">
                                            <asp:TextBox ID="PolicyValue_1" runat="server" CssClass="egd-form-field"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="TR_SELECT" runat="server">
                                        <td class="egd-form-label">
                                            策&nbsp;略&nbsp;值：</td>
                                        <td colspan="3" class="egd-property">
                                            <asp:DropDownList ID="PolicyValue_2" runat="server" CssClass="egd-form-field">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <asp:HiddenField ID="_ID" runat="server" />
                                <asp:HiddenField ID="Unid" runat="server" />
                                <asp:HiddenField ID="Code" runat="server" />
                                <asp:HiddenField ID="ValueType" runat="server" />
                                <asp:HiddenField ID="OptionNames" runat="server" />
                                <asp:HiddenField ID="OptionValues" runat="server" />
                                <asp:HiddenField ID="Value" runat="server" />
                                <asp:HiddenField ID="Type" runat="server" />
                                <asp:HiddenField ID="Author_Unid" runat="server" />
                                <asp:HiddenField ID="Author_Name" runat="server" />
                                <asp:HiddenField ID="Author_OUUnid" runat="server" />
                                <asp:HiddenField ID="Author_OUName" runat="server" />
                                <asp:HiddenField ID="Author_OUCode" runat="server" />
                                <asp:HiddenField ID="Author_OUFullName" runat="server" />
                            </div>
                        </div>
                    </form>
                </div>
            </td>
        </tr>
    </table>
</body>
</html>
