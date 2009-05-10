<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WHUnitForm.aspx.cs" Inherits="TSCommon.Web.WareHouse.WHUnitForm" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>
        <%= TSLib.SimpleResourceHelper.GetSystemTitle() %>
    </title>
    <script type="text/javascript" language="javascript">var contextPath = "<%=ContextPath %>";</script>

    <script language="javascript" type="text/javascript">var actionUrl = "wHUnitAction.do";</script>
    
    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-FormPage.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script type="text/javascript" src="<%=ContextPath %>/WareHouse/js/WHUnitForm.js"></script>
</head>
<body class="egd-body" onload="thisPage.init();">
    <table class="egd-page-table" cellspacing="0" cellpadding="0" border="0">
        <!-- �������� -->
        <tr class="egd-page-tb-tr">
            <td class="egd-page-tb-td">
                <div id="tbContainer" class="egd-page-content-tbContainer">
                </div>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <!-- �Զ����ɹ����� -->
                <div class="egd-page-overflow egd-form-align">
                    <!-- ������� -->
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="egd-form-title-warp egd-form-pageWidth">
                        <tr>
                            <td class="egd-page-title-text">
                                �ص����</td>
                        </tr>
                    </table>
                    <!-- ���� -->
                    <div class="egd-page-form-wrap">
                        <!-- ������ɫ�ͱ�����ɫ -->
                        <div class="egd-page-form-tableBorder egd-form-pageWidth">
                            <form id="thisForm" runat="server">
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
                                            ���ƣ�</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="Name" CssClass="egd-form-btField" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="egd-form-label">
                                            ���룺</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="Code" CssClass="egd-form-btField" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            ��ע��</td>
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
        // ��ǰ�û��Ƿ��ǹ���Ա
	    isManager: <%=this.IsManager.ToString().ToLower() %>,
        
        // ��ǰҳ���Ƿ��ڿɱ༭״̬
	    canEdit: <%=this.CanEdit.ToString().ToLower() %>
    };
</script>
