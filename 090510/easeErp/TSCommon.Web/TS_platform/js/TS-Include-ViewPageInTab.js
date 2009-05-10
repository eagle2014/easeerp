/**
 * 动态加载视图相关js文件的js
 * Tony 2007-09-10
 * 引用范例：<script src="....../TS-Include-ViewPageInTab.js?rootPath=../" type="text/javascript"></script>
 * rootPath默认为../
 */
var egrand_include_viewPageInTab = {
 	version: '1.0.0', 
	defaultRootPath: '../',
	doLoad: function(rootPath) {
		document.write('<meta http-equiv="Cache-Control" content="no-store"/>');
		document.write('<meta http-equiv="Pragma" content="no-cache"/>');
		document.write('<meta http-equiv="Expires" content="0"/>');
		
		/* 其它的声明 */
		document.write('<script src="' + rootPath + 'TS_platform/js/Prototype.js" type="text/javascript"></script>');
		//document.write('<script src="' + rootPath + 'TS_platform/js/scriptaculous/scriptaculous.js?load=effects" type="text/javascript"></script>');
		
		/* 忆科通用css的声明 */
		document.write('<link href="' + rootPath + 'TS_platform/css/Common.css" rel="stylesheet" type="text/css" />');
		document.write('<link href="' + rootPath + 'TS_platform/css/Page.css" rel="stylesheet" type="text/css" />');
		
		
		/* 忆科通用js的声明 */
		document.write('<script src="' + rootPath + 'TS_platform/js/TS.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'TS_platform/js/TS-Dialog.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'TS_platform/js/Common.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'TS_platform/js/MsgBox.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'TS_platform/js/TS-SystemConfig.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'TS_platform/js/TS-MainPageUtils.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'TS_platform/js/Validate.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'TS_platform/js/CommonSelect.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'TS_platform/js/ItemSelectFunction.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'TS_platform/js/FormFunction.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'TS_platform/js/ViewFunction.js" type="text/javascript"></script>');
		
		/* 忆科widgets */
		document.write('<script src="' + rootPath + 'TS_platform/widgets/toolbar/toolbar.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'egd_platform/widgets/tabPanel/tabPanel.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'TS_platform/widgets/grid/grid.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'TS_platform/widgets/tree/Tree.js" type="text/javascript"></script>');
		
		document.write('<link href="' + rootPath + 'TS_platform/widgets/toolbar/css/default/toolbar.css" rel="stylesheet" type="text/css" />');
		document.write('<link href="' + rootPath + 'TS_platform/widgets/toolbar/css/tab/toolbar.css" rel="stylesheet" type="text/css" />');
		document.write('<link href="' + rootPath + 'TS_platform/widgets/toolbar/css/default/buttons.css" rel="stylesheet" type="text/css" />');
		document.write('<link href="' + rootPath + 'TS_platform/widgets/toolbar/css/tab/toolbar.css" rel="stylesheet" type="text/css" />');
		document.write('<link href="' + rootPath + 'TS_platform/widgets/tabPanel/css/default/tabs.css" rel="stylesheet" type="text/css" />');
		document.write('<link href="' + rootPath + 'TS_platform/widgets/grid/css/default/grid.css" rel="stylesheet" type="text/css" />');
		document.write('<link href="' + rootPath + 'TS_platform/widgets/toolbar/css/page/toolbar.css" rel="stylesheet" type="text/css" />');
		document.write('<link href="' + rootPath + 'TS_platform/widgets/toolbar/css/page/buttons.css" rel="stylesheet" type="text/css" />');
		document.write('<link href="' + rootPath + 'TS_platform/widgets/tree/css/default/Tree.css" rel="stylesheet" type="text/css" />');
  	},
	load: function(){
	    var allScript = document.getElementsByTagName("script");
	    var theOne = null,s;
    	for (var i = 0, length = allScript.length; i < length; i++){
			s = allScript[i];
	    	if (s.src && s.src.match(/TS-Include-ViewPageInTab\.js(\?.*)?$/)){
	    		theOne = s;
				//alert("scr=" + s.src);
	    		break;
	    	}
	    }
	    if (theOne != null){
	      var rootPath = theOne.src.replace(/.*TS-Include-ViewPageInTab\.js(\?rootPath=)?/,'');
	      if (rootPath == "")
	      	rootPath = this.defaultRootPath;
	      else if (rootPath == "1")
	      	rootPath = '../';
	      else if (rootPath == "2")
	      	rootPath = '../../';
	      else if (rootPath == "3")
	      	rootPath = '../../../';
	      this.doLoad(rootPath);
	    }
	}
};
TS_include_viewPageInTab.load();