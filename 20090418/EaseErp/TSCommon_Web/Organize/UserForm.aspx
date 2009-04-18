<%@ Page Language="C#" AutoEventWireup="true" Codebehind="UserForm.aspx.cs" Inherits="TSCommon_Web.Organize.UserForm" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%= TSLib.SimpleResourceHelper.GetSystemTitle() %>
    </title>

    <script type="text/javascript" language="javascript">var contextPath = "<%=ContextPath %>";</script>

    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-FormPage.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script src="<%=ContextPath %>/Organize/js/TS-Org.js" type="text/javascript"></script>

    <script type="text/javascript" src="<%=ContextPath %>/Organize/js/UserForm.js"></script>

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
                                ��Ա����</td>
                        </tr>
                    </table>
                    <!-- ���� -->
                    <div class="egd-page-form-wrap">
                        <!-- ������ɫ�ͱ�����ɫ -->
                        <form id="thisForm" runat="server" class="egd-form">
                            <div class="egd-page-form-tableBorder egd-form-pageWidth">
                                <table width="100%" border="0" cellpadding="1" cellspacing="2">
                                    <tr class="egd-form-emptyTR">
                                        <td width="80">
                                        </td>
                                        <td width="260">
                                        </td>
                                        <td width="80">
                                        </td>
                                        <td width="270">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            ������֯��</td>
                                        <td colspan="3" class="egd-property">
                                            <asp:TextBox ID="OUFullName" CssClass="egd-form-zdField" runat="server" Width="600px"
                                                ReadOnly="true"></asp:TextBox>
                                            <img id="ImgBtnSelectOU" alt="ѡ��������λ����" style="vertical-align: middle; cursor: hand"
                                                src="../TS_platform/images/select/search.gif" runat="server" onclick="thisPage.selectBelongOU()" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            ��&nbsp;&nbsp;&nbsp;&nbsp;����</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="Name" CssClass="egd-form-btField" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="egd-form-label">
                                            ��¼�ʺţ�</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="LoginID" CssClass="egd-form-btField" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            ��&nbsp;&nbsp;&nbsp;&nbsp;�ţ�</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="CardID" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="egd-form-label">
                                            ������ţ�</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="OrderNo" CssClass="egd-form-btField" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            ְ&nbsp;&nbsp;&nbsp;&nbsp;��</td>
                                        <td class="egd-property">
                                            <asp:DropDownList ID="JobTitleUnid" runat="server" Width="100%" onchange="thisPage.changeJobTitle()">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="egd-form-label">
                                            �����绰��</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="TelephoneNo" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            �ֻ����룺</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="Mobile" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="egd-form-label">
                                            E-Mail��</td>
                                        <td class="egd-property">
                                            <asp:TextBox ID="Email" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            ��&nbsp;��&nbsp;�ң�</td>
                                        <td colspan="3" class="egd-property">
                                            <asp:TextBox ID="Office" CssClass="egd-form-field" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            ��ϵ��ַ��</td>
                                        <td colspan="3" class="egd-property">
                                            <asp:TextBox ID="Address" CssClass="egd-form-field" runat="server" Width="100%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="egd-form-label">
                                            �û����ͣ�</td>
                                        <td colspan="3">
                                            <asp:RadioButtonList ID="UserType" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="��ͨ�û�" Value="0" Selected="True" />
                                                <asp:ListItem Text="����̨" Value="1" />
                                                <asp:ListItem Text="����֧��" Value="2" />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!--���ʼ -->
                            <div id="tabContainer" class="egd-form-pageWidth egd-page_partitionTop">
                                <div id="divGroup" class="egd-tab-hidden">
                                    <table cellpadding="3" cellspacing="3">
                                        <tr>
                                            <td class="egd-page-tabTitleTopBg" style="width: 320px;">
                                                ��δӵ�еĸ�λ�б�</td>
                                            <td style="width: 80px">
                                            </td>
                                            <td class="egd-page-tabTitleTopBg">
                                                ��ӵ�еĸ�λ�б�</td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top; width: 320px;">
                                                <asp:ListBox ID="AllGroups" runat="server" Width="100%" Rows="9" SelectionMode="Multiple">
                                                </asp:ListBox>
                                            </td>
                                            <td style="text-align: center; vertical-align: middle; width: 80px">
                                                <input id="btnRightArrow" type="button" class="form-button-default" value="�� �� &gt;"
                                                    onclick="IS_AppendSelectedItem('AllGroups', 'GroupUnids', true)" runat="server" />
                                                <div>
                                                    &nbsp;</div>
                                                <input type="button" class="form-button-default" id="btnLeftArrow" onclick="IS_AppendSelectedItem('GroupUnids', 'AllGroups', true)"
                                                    value="&lt; ɾ ��" runat="server" />
                                                <div>
                                                    &nbsp;</div>
                                                <input id="btnLeftAllArrow" type="button" class="form-button-default" value="&lt;&lt; �� ��"
                                                    onclick="IS_AppendAllItem('GroupUnids', 'AllGroups', true)" runat="server" />
                                            </td>
                                            <td>
                                                <asp:ListBox ID="GroupUnids" runat="server" Width="100%" Rows="9" SelectionMode="Multiple">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <asp:HiddenField ID="_ID" runat="server" />
                            <asp:HiddenField ID="Unid" runat="server" />
                            <asp:HiddenField ID="UserStatus" runat="server" />
                            <asp:HiddenField ID="Password" runat="server" />
                            <asp:HiddenField ID="FullName" runat="server" />
                            <asp:HiddenField ID="Gender" runat="server" />
                            <asp:HiddenField ID="Birthday" runat="server" />
                            <asp:HiddenField ID="UnitUnid" runat="server" />
                            <asp:HiddenField ID="UnitName" runat="server" />
                            <asp:HiddenField ID="UnitFullName" runat="server" />
                            <asp:HiddenField ID="UnitCode" runat="server" />
                            <asp:HiddenField ID="UnitFullCode" runat="server" />
                            <asp:HiddenField ID="OUUnid" runat="server" />
                            <asp:HiddenField ID="OUCode" runat="server" />
                            <asp:HiddenField ID="OUFullCode" runat="server" />
                            <asp:HiddenField ID="OUName" runat="server" />
                            <asp:HiddenField ID="JobTypeUnid" runat="server" />
                            <asp:HiddenField ID="JobTypeName" runat="server" />
                            <asp:HiddenField ID="JobTitleName" runat="server" />
                            <asp:HiddenField ID="UserLigion" runat="server" />
                            <asp:HiddenField ID="IsTmpUser" runat="server" />
                            <asp:HiddenField ID="ValidityStartDate" runat="server" />
                            <asp:HiddenField ID="ValidityEndDate" runat="server" />
                            <asp:HiddenField ID="Field_1" runat="server" />
                            <asp:HiddenField ID="Field_2" runat="server" />
                            <asp:HiddenField ID="Field_3" runat="server" />
                            <asp:HiddenField ID="Field_4" runat="server" />
                            <asp:HiddenField ID="Field_5" runat="server" />
                            <asp:HiddenField ID="EmployeeID" runat="server" />
                            <asp:HiddenField ID="DepartmentID" runat="server" />
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
        // ��ǰ�û��Ƿ��ǹ���Ա
	    isManager: <%=this.IsManager.ToString().ToLower() %>,
        
        // ��ǰҳ���Ƿ��ڿɱ༭״̬
	    canEdit: <%=this.CanEdit.ToString().ToLower() %>,
	    //�ü�¼�Ƿ��ѱ���
	    isUnidEmpty:<%=this.IsUnidEmpty.ToString().ToLower() %>
    };

// ���ǰ�û�
function onEnabled_Click(){
    var strUrl = "<%=ContextPath %>/Organize/UserForm.aspx?action=enabledUser";
	strUrl +="&timeStamp=" + new Date().getTime();
//	IS_SelectAllTotal();
	document.forms[0].action = strUrl;
	document.forms[0].target = "_self";
	document.forms[0].submit();
}

// ���õ�ǰ�û�
function onDisabled_Click(){
    var strUrl = "<%=ContextPath %>/Organize/UserForm.aspx?action=disabledUser";
	strUrl +="&timeStamp=" + new Date().getTime();
//	IS_SelectAllTotal();
	document.forms[0].action = strUrl;
	document.forms[0].target = "_self";
	document.forms[0].submit();
}
</script>

