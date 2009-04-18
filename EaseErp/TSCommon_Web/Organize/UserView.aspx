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
                <!-- �������� -->
                <tr class="egd-page-tb-tr">
                    <td class="egd-page-tb-td">
                        <div id="tbContainer" class="egd-page-content-tbContainer">
                        </div>
                    </td>
                </tr>
                <!-- ������ -->
                <tr class="egd-page-content-tr">
                    <td class="egd-page-content-td">
                        <table class="egd-page-table" cellspacing="0" cellpadding="0">
                            <tr>
                                <!-- ������ -->
                                <td class="egd-page-content-tree-td">
                                    <div id="treeContainer" class="egd-page-content-treeContainer">
                                    </div>
                                </td>
                                <!-- �ָ��� -->
                                <td class="egd-page-content-seperator-td">
                                </td>
                                <td>
                                </td>
                                <!-- ��� -->
                                <td>
                                    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
                                        <!-- ��ѯ���� -->
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
                                                            ��½�ʺţ�</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="LoginID" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="egd-form-label">
                                                            ��&nbsp;&nbsp;&nbsp;&nbsp;�ƣ�</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="Name" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="egd-form-label">
                                                            ְ&nbsp;&nbsp;&nbsp;&nbsp;��</td>
                                                        <td class="egd-property">
                                                            <asp:DropDownList ID="JobTitleUnid" runat="server" Width="100%" onchange="thisPage.changeJobTitle()">
                                                            </asp:DropDownList>
                                                           <asp:HiddenField ID="JobTitleName" runat="server" /> 
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="egd-form-label">
                                                            E-Mail��</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="Email" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="egd-form-label">
                                                            �ֻ����룺</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="Mobile" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="egd-form-label">
                                                            �����绰��</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="TelephoneNo" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="egd-form-label">
                                                            ��&nbsp;&nbsp;&nbsp;&nbsp;�ţ�</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="CardID" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="egd-form-label">
                                                            ��ϵ��ַ��</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="Address" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="egd-form-label">
                                                            ��&nbsp;��&nbsp;�ң�</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="Office" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="egd-form-label">
                                                            ������֯��</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="OUFullName" runat="server" ReadOnly="true" CssClass="egd-form-zdField"
                                                                Width="135px" />
                                                            <img src="<%=ContextPath %>/TS_platform/images/select/search_4.gif" alt="ѡ��������λ����" style="border: 0px;
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
                                                            ������ò��</td>
                                                        <td class="egd-property">
                                                            <asp:TextBox ID="UserLigion" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="egd-form-label">
                                                            �Ա�</td>
                                                        <td class="egd-property">
                                                            <select id="Gender">
                                                                <option value="0">== ���� ==</option>
                                                                <option value="1">��</option>
                                                                <option value="2">Ů</option>
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
        // ��ǰ�û��Ƿ��ǹ���Ա
	    isManager: <%=this.IsManager.ToString().ToLower() %>
    };
</script>
