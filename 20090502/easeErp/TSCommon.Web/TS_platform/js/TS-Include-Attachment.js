/**
 * 动态加载附件相关js文件的js
 * zhouzhihu 2008-02-02
 * 引用范例：<script src="....../TS-Include-Attachment.js?rootPath=../" type="text/javascript"></script>
 * rootPath默认为../
 */
var TS_include_attachment = {
 	version: '1.0.0', 
	defaultRootPath: '../',
	doLoad: function(rootPath) {
		/* 忆科widgets */
		document.write('<script src="' + rootPath + 'TS_platform/widgets/attachment/attachment.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'TS_platform/widgets/grid/grid.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'TS_platform/widgets/toolbar/toolbar.js" type="text/javascript"></script>');
		document.write('<link href="' + rootPath + 'TS_platform/widgets/grid/css/default/grid.css" rel="stylesheet" type="text/css" />');
		document.write('<link href="' + rootPath + 'TS_platform/widgets/toolbar/css/default/toolbar.css" rel="stylesheet" type="text/css" />');
		document.write('<link href="' + rootPath + 'TS_platform/widgets/toolbar/css/default/buttons.css" rel="stylesheet" type="text/css" />');
  	},
	load: function(){
	    var allScript = document.getElementsByTagName("script");
	    var theOne = null,s;
    	for (var i = 0, length = allScript.length; i < length; i++){
			s = allScript[i];
	    	if (s.src && s.src.match(/TS-Include-Attachment\.js(\?.*)?$/)){
	    		theOne = s;
				//alert("scr=" + s.src);
	    		break;
	    	}
	    }
	    if (theOne != null){
	      var rootPath = theOne.src.replace(/.*TS-Include-Attachment\.js(\?rootPath=)?/,'');
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
TS_include_attachment.load();