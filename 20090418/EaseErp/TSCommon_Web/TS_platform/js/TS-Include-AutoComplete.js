/**
 * 动态加载日历相关js文件的js
 * Tony 2007-09-10
 * 引用范例：<script src="....../TS-Include-AutoComplete.js?rootPath=../" type="text/javascript"></script>
 * rootPath默认为../
 */
var TS_include_autoComplete = {
 	version: '1.0.0', 
	defaultRootPath: '../',
	doLoad: function(rootPath) {
		document.write('<script src="' + rootPath + 'TS_platform/widgets/autoComplete/autoComplete.js" type="text/javascript"></script>');
		document.write('<link href="' + rootPath + 'TS_platform/widgets/autoComplete/css/default/autoComplete.css" rel="stylesheet" type="text/css" />');
  	},
	load: function(){
	    var allScript = document.getElementsByTagName("script");
	    var theOne = null,s;
    	for (var i = 0, length = allScript.length; i < length; i++){
			s = allScript[i];
	    	if (s.src && s.src.match(/TS-Include-AutoComplete\.js(\?.*)?$/)){
	    		theOne = s;
				//alert("scr=" + s.src);
	    		break;
	    	}
	    }
	    if (theOne != null){
	      var rootPath = theOne.src.replace(/.*TS-Include-AutoComplete\.js(\?rootPath=)?/,'');
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
TS_include_autoComplete.load();