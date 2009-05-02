<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WareHouseForm.aspx.cs" Inherits="TSCommon.Web.WareHouse.WareHouseForm" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
        <title>
        <%= TSLib.SimpleResourceHelper.GetSystemTitle() %>
    </title>

    <script type="text/javascript" language="javascript">var contextPath = "<%=ContextPath %>";</script>

    <script src="<%=ContextPath %>/TS_platform/js/TS-Include-FormPage.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script type="text/javascript" src="<%=ContextPath %>/WareHouse/js/WareHouseForm.js"></script>
</head>
<body class="egd-body">
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
