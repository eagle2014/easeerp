<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ThingForm.aspx.cs" Inherits="TSCommon.Web.WareHouse.ThingForm" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>
        <%= TSLib.SimpleResourceHelper.GetSystemTitle() %>
    </title>
    <script type="text/javascript" language="javascript">var contextPath = "<%=ContextPath %>";</script>
    <script type="text/javascript" language="javascript">var actionUrl = "thingAction.do";</script>
    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-FormPage.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script type="text/javascript" src="<%=ContextPath %>/WareHouse/js/ThingForm.js"></script>
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
                                地点管理</td>
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
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            名称：</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="Name" CssClass="egd-form-btField" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="egd-form-label">
                                            代码：</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="Code" CssClass="egd-form-btField" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            单位：</td>
                                        <td class="egd-property">
                                        <asp:DropDownList ID="WHUnit_ID" CssClass="egd-form-field" runat="server"></asp:DropDownList>
                                        </td>
                                        <td class="egd-form-label">
                                        </td>
                                        <td class="egd-property">
                                        </td>
                                    </tr>
                                    <tr id="TR_MODEL_ID">
                                        <td class="egd-form-label">
                                            备注：</td>
                                        <td colspan="3" class="egd-property">
                                            <asp:TextBox ID="Memo" CssClass="egd-form-btField" TextMode="MultiLine" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <asp:HiddenField ID="_ID" runat="server" />
                                <asp:HiddenField ID="Unid" runat="server" />
                                <asp:HiddenField ID="IsInner" runat="server" />
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