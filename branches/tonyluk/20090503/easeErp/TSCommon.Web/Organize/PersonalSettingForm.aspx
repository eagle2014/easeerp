<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalSettingForm.aspx.cs" Inherits="TSCommon.Web.Organize.PersonalSettingForm" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%= TSLib.SimpleResourceHelper.GetSystemTitle() %>
    </title>

    <script type="text/javascript" language="javascript">var contextPath="<%=ContextPath%>";</script>

    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-FormPage.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script type="text/javascript" src="<%=ContextPath %>/Organize/js/PersonalSettingForm.js"></script>

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
                                个人信息</td>
                        </tr>
                    </table>
                    <!-- 表单区 -->
                    <div class="egd-form-pageWidth egd-page-form-wrap">
                        <!-- 表单边颜色和背景颜色 -->
                        <div class="egd-page-form-tableBorder">
                            <form id="thisForm" runat="server" class="egd-form">
                                <table width="100%" cellpadding="1" cellspacing="2">
                                    <tr class="egd-form-emptyTR">
                                        <td width="80">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            姓&nbsp;&nbsp;&nbsp;&nbsp;名：</td>
                                        <td>
                                            <asp:TextBox ID="Name" CssClass="egd-form-zdField" ReadOnly="true" Width="100%" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            电子邮件：</td>
                                        <td>
                                            <asp:TextBox ID="Email" CssClass="egd-form-field" Width="100%" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            手&nbsp;&nbsp;&nbsp;&nbsp;机：</td>
                                        <td>
                                            <asp:TextBox ID="Mobile" CssClass="egd-form-field" Width="100%" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            固定电话：</td>
                                        <td>
                                            <asp:TextBox ID="TelephoneNo" CssClass="egd-form-field" Width="100%" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            地&nbsp;&nbsp;&nbsp;&nbsp;址：</td>
                                        <td>
                                            <asp:TextBox ID="Address" CssClass="egd-form-field" Width="100%" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            办&nbsp;公&nbsp;室：</td>
                                        <td>
                                            <asp:TextBox ID="Office" CssClass="egd-form-field" Width="100%" runat="server"></asp:TextBox>
                                            <div>
                                                <asp:HiddenField ID="_ID" runat="server" />
                                                <asp:HiddenField ID="Unid" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </form>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</body>
</html>