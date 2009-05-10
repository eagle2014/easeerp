<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnitForm.aspx.cs" Inherits="TSCommon.Web.Organize.UnitForm" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%= TSLib.SimpleResourceHelper.GetSystemTitle() %>
    </title>
    <script type="text/javascript" language="javascript">var contextPath = "<%=ContextPath %>";</script>

    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-FormPage.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script type="text/javascript" src="<%=ContextPath %>/Organize/js/UnitForm.js"></script>

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
                                单位配置</td>
                        </tr>
                    </table>
                    <!-- 表单区 -->
                    <div class="egd-page-form-wrap">
                        <!-- 表单边颜色和背景颜色 -->
                        <div class="egd-page-form-tableBorder egd-form-pageWidth">
                            <form id="thisForm" runat="server">
                                <table width="100%" cellpadding="1" cellspacing="2">
                                    <tr class="egd-form-emptyTR">
                                        <td width="80">
                                        </td>
                                        <td width="270">
                                        </td>
                                        <td width="80">
                                        </td>
                                        <td width="260">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            上级单位：</td>
                                        <td colspan="3" class="egd-property">
                                            <asp:TextBox ID="ParentOUName" CssClass="egd-form-zdField" runat="server" Width="580px"
                                                ReadOnly="true"></asp:TextBox>
                                            <img src="../TS_platform/images/select/search.gif" alt="选择上一级单位" id="ImgBtnSelectOU"
                                                style="vertical-align: middle; cursor: pointer;" onclick="thisPage.selectUpper()"
                                                runat="server" /><img id="ImgBtnClear" alt="清除上一级单位" style="vertical-align: middle;
                                                    cursor: pointer" src="../TS_platform/images/select/del.gif" runat="server" onclick="thisPage.clearUpper()" />
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
                                            级&nbsp;&nbsp;&nbsp;&nbsp;别：</td>
                                        <td class="egd-property">
                                            <asp:DropDownList ID="Level" runat="server" onchange="thisPage.changeLevel()">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="egd-form-label">
                                            排序序号：</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="OrderNo" CssClass="egd-form-btField" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            状&nbsp;&nbsp;&nbsp;&nbsp;态：</td>
                                        <td class="egd-property">
                                            <asp:DropDownList ID="OUStatus" runat="server" Width="100%">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="egd-form-label">
                                            备注信息：</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="Description" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <asp:HiddenField ID="_ID" runat="server" />
                                <asp:HiddenField ID="Unid" runat="server" />
                                <asp:HiddenField ID="ParentOUUnid" runat="server" />
                                <asp:HiddenField ID="Type" runat="server" />
                                <asp:HiddenField ID="IsTmpOU" runat="server" />
                                <asp:HiddenField ID="LevelName" runat="server" />
                                <asp:HiddenField ID="FullName" runat="server" />
                                <asp:HiddenField ID="FullCode" runat="server" />
                                <asp:HiddenField ID="Field_1" runat="server" />
                                <asp:HiddenField ID="Field_2" runat="server" />
                                <asp:HiddenField ID="Field_3" runat="server" />
                                <asp:HiddenField ID="Field_4" runat="server" />
                                <asp:HiddenField ID="Field_5" runat="server" />
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