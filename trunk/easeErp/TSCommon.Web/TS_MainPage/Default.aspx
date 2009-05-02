<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Default.aspx.cs" Inherits="TSCommon.Web.TS_MainPage._Default" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%= TSLib.SimpleResourceHelper.GetSystemTitle() %>
    </title>
    <link rel="stylesheet" type="text/css" href="../TS_platform/ext/resources/css/ext-all.css" />
    <link rel="stylesheet" type="text/css" href="../TS_platform/ext/resources/css/egd-tabs.css" />
    <link rel="stylesheet" type="text/css" href="../TS_platform/css/Common.css" />
    <link rel="stylesheet" type="text/css" href="css/Top.css" />
    <link rel="stylesheet" type="text/css" href="css/Index.css" />
    <link rel="stylesheet" type="text/css" href="../TS_MainPage/css/TabCloseMenu.css" />
    <link rel="stylesheet" type="text/css" href="../TS_MainPage/css/Dlg.css" />

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/js/Prototype.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/ext/adapter/ext/ext-base.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/ext/ext-all.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/ext/ext-extend.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/ext/source/locale/ext-lang-zh_CN.js"></script>

    <script type="text/javascript" language="javascript">
        Ext.BLANK_IMAGE_URL = '<%=ContextPath %>/TS_platform/ext/resources/images/default/s.gif';
        var sys_maxWindowCount = <%= TSLib.SimpleResourceHelper.GetString("SYSTEM.WINDOW.MAX_COUNT") %>;
        var sys_maxWindowMsg = '<%= TSLib.SimpleResourceHelper.GetString("SYSTEM.WINDOW.MAX_MSG") %>';
        var sys_title = '<%= TSLib.SimpleResourceHelper.GetSystemTitle() %>';
        
        var mainPage_firstPage = '<%=this.FirstPage %>';
        var mainPage_firstPageTabId = '<%=this.FirstPageTabId %>';
        var mainPage_firstPageTabTitle = '<%=this.FirstPageTabTitle %>';
        var mainPage_showSearchPage = <%=this.ShowSearchPage.ToString().ToLower() %>;
        var mainPage_searchPage = '<%=this.SearchPage %>';
        var mainPage_showMaskMsg = <%=this.ShowMaskMsg.ToString().ToLower() %>;
        var toOpenUrl = '<%=this.ToOpenUrl %>';
        var toOpenUrlTitle = '<%=this.ToOpenUrlTitle %>';
    </script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_MainPage/js/TabCloseMenu.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_MainPage/js/Default.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/js/TS.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_platform/js/TS-MainPageUtils.js"></script>

    <script type="text/javascript" src="<%=ContextPath %>/TS_MainPage/js/TS_swfobject.js"></script>

    <script type="text/javascript" language="javascript">
        TS.rootPath = '<%=ContextPath %>/';
        var sys_useMDI = '<%= TSLib.SimpleResourceHelper.GetString("SYSTEM.WINDOW.ISMDI") %>'.toLowerCase() != 'false';
        
    </script>

</head>
<body style="background-color: #DFE8F6;">
    <div id="loading-mask" style="">
    </div>
    <div id="loading">
        <div class="loading-indicator">
            <img src="images/loading32.gif" width="32" height="32" style="margin-right: 8px;"
                align="absmiddle" />正在加载...</div>
    </div>
    <div id="hd">
        <div class="top-warp">
            <div class="top-logo">
                <div id="flashcontent" style="width: 376px; height: 73px">
                </div>

                <script type="text/javascript"> 
                    /*
                    var fo = new FlashObject("images/top/logobg.swf", "mymovie", "376px", "73px", "7", "#FFFFFF"); 
                    fo.addParam("quality", "low"); 
                    fo.addParam("wmode", "transparent"); 
                    fo.addParam("salign", "t"); 
                    fo.addParam("scale", "noscale"); 
                    fo.addParam("loop", "false"); 
                    fo.write("flashcontent"); 
                    */
                </script>

            </div>
            <div class="top-right">
                <div class="top-rightWarp">
                    <div class="top-exit" onclick="onBtnLogout_Click()">
                    </div>
                    <div class="top-help" onclick="onBtnHelp_Click()">
                    </div>
                </div>
                <div class="top-infoText">
                    当前用户：<a href="javascript:showMe()" class="top-userText"><%= TSCommon.Core.TSWebContext.TSWEBContext.Current.CurUser.Name %></a>
                    部门：<span class="top-deptText"><%= TSCommon.Core.TSWebContext.TSWEBContext.Current.CurUser.OUName%></span>
                </div>
            </div>
        </div>
        <div class="topbottom">
            &nbsp;</div>
    </div>
    <div id="nav">
        <div id="search">
            <iframe id="searchFrame" style="width: 100%; height: 100%" src="" scrolling="no"
                frameborder="0"></iframe>
        </div>
    </div>
    <div id="footer">
        <table class="bottom" width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="100">
                    &nbsp;
                </td>
                <td align="center">
                    &copy;2008版权所有</td>
                <td width="100">
                    <img src="<%=ContextPath %>/TS_MainPage/images/bottom/pbe.gif" width="100" height="7"></td>
            </tr>
        </table>
    </div>
    <!-- 空白框架，通常用于下载附件 -->
    <iframe id="blank" name="blank" style="width: 0; height: 0; visibility: hidden;"
        src="#" scrolling="no" frameborder="0"></iframe>
</body>
</html>

<script type="text/javascript" language="javascript">
function onBtnHelp_Click(){
    msgBox("3.1！");
}

function onBtnLogout_Click(){
    var strUrl = "<%=ContextPath %>/Login.aspx";
	strUrl +="?timeStamp=" + new Date().getTime();
	//alert(strUrl);
	document.location = strUrl;
}

function showMe(){
	openWindow({
	    id:'9AF26025E7154411A32271A873139C03',
	    title: '<%= TSCommon.Core.TSWebContext.TSWEBContext.Current.CurUser.Name %>',
	    url: "<%=ContextPath %>/personalSettingAction.do?action=Edit"
	});
}
</script>

