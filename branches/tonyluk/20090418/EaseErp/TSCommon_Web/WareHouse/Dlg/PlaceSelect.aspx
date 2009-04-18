<%@ Page Language="C#" AutoEventWireup="true" Codebehind="PlaceSelect.aspx.cs" Inherits="TSCommon_Web.WareHouse.Dlg.PlaceSelect" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>选择地点</title>
</head>

<script src="<%=ContextPath %>/TS_platform/js/TS-Include-ViewPage.js?rootPath=<%=ContextPath %>/"
    type="text/javascript"></script>

<script type="text/javascript" src="<%=ContextPath %>/WareHouse/Dlg/js/PlaceSelect.js"></script>

<body onload="init();">
    <form id="form1" runat="server">
        <div>
            <table class="egd-page-table" cellspacing="0" cellpadding="0">
                <!-- 内容区 -->
                <tr class="egd-page-content-tr">
                    <td class="egd-page-content-td">
                        <table class="egd-page-table" cellspacing="0" cellpadding="0">
                            <tr>
                                <!-- 表格 -->
                                <td class="egd-page-content-grid-td">
                                    <div id="gridContainer" class="egd-page-content-gridContainer">
                                    </div>
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
/*
初始化页面方法
*/
function init()
{
    initGrid();
    var singleSelect='<%=Request.Params["singleSelect"]%>';
    singleSelect=singleSelect=="true"?true:false;
    var myconfig={singleSelect:singleSelect};
    TS.apply(dataGrid.config,myconfig);
}
function DlgOkFn()
{
    var returnValue;
    var ajaxUrl=TS.rootPath+"placeAction.do?action=GetPlaceByAjax&ids="+dataGrid.getSelected();
    ajaxUrl += "&timestamp" + new Date().getTime();
    new Ajax.Request(ajaxUrl, {method: 'get',asynchronous:false, onComplete:function(transport){
                var result;
	            eval("result ="+transport.responseText);
	            if(result.domains){
	                 returnValue=result.domains[0];
                }
        }});
    return returnValue;
}
</script>

