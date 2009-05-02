/**
 * 动态加载Ext相关js文件的js
 * Tony 2007-09-10
 * 引用范例：<script src="....../TS-Include-Ext.js?rootPath=../" type="text/javascript"></script>
 * rootPath默认为../
 */
var TS_include_ext = {
 	version: '1.0.0', 
	defaultRootPath: '../',
	doLoad: function(rootPath) {
		document.write('<meta http-equiv="Cache-Control" content="no-store"/>');
		document.write('<meta http-equiv="Pragma" content="no-cache"/>');
		document.write('<meta http-equiv="Expires" content="0"/>');
		
		/* 其它的声明 */
		document.write('<script src="' + rootPath + 'TS_platform/js/Prototype.js" type="text/javascript"></script>');
		
		/* 忆科通用css的声明 */
		document.write('<link href="' + rootPath + 'TS_platform/css/Common.css" rel="stylesheet" type="text/css" />');
		document.write('<link href="' + rootPath + 'TS_platform/css/Page.css" rel="stylesheet" type="text/css" />');
		
		document.write('<link href="' + rootPath + 'TS_platform/ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />');
		document.write('<link href="' + rootPath + 'TS_platform/ext/resources/css/egd-tabs.css" rel="stylesheet" type="text/css" />');
		document.write('<script src="' + rootPath + 'TS_platform/ext/adapter/ext/ext-base.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'TS_platform/ext/ext-all.js" type="text/javascript"></script>');

  	},
	load: function(){
	    var allScript = document.getElementsByTagName("script");
	    var theOne = null,s;
    	for (var i = 0, length = allScript.length; i < length; i++){
			s = allScript[i];
	    	if (s.src && s.src.match(/TS-Include-Ext\.js(\?.*)?$/)){
	    		theOne = s;
				//alert("scr=" + s.src);
	    		break;
	    	}
	    }
	    if (theOne != null){
	      var rootPath = theOne.src.replace(/.*TS-Include-Ext\.js(\?rootPath=)?/,'');
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
TS_include_ext.load();