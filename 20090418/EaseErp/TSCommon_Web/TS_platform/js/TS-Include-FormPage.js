/**
 * 动态加载表单相关js文件的js
 * Tony 2007-09-10
 * 引用范例：<script src="....../TS-include-formPage.js?rootPath=../" type="text/javascript"></script>
 * rootPath默认为../
 */
var TS_include_formPage = {
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
		document.write('<link href="' + rootPath + 'ATM/css/default/attachment.css" rel="stylesheet" type="text/css" />');
		
		/* 忆科通用js的声明 */
		document.write('<script src="' + rootPath + 'TS_platform/js/TS.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'Organize/js/TS-Org.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'TS_platform/js/TS-MainPageUtils.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'TS_platform/js/ItemSelectFunction.js" type="text/javascript"></script>');
		
		/* 忆科widgets */
		document.write('<script src="' + rootPath + 'TS_platform/widgets/toolbar/toolbar.js" type="text/javascript"></script>');
		document.write('<script src="' + rootPath + 'TS_platform/widgets/tabPanel/tabPanel.js" type="text/javascript"></script>');
		
		document.write('<link href="' + rootPath + 'TS_platform/widgets/toolbar/css/default/toolbar.css" rel="stylesheet" type="text/css" />');
		document.write('<link href="' + rootPath + 'TS_platform/widgets/toolbar/css/default/buttons.css" rel="stylesheet" type="text/css" />');
		document.write('<link href="' + rootPath + 'TS_platform/widgets/tabPanel/css/default/tabs.css" rel="stylesheet" type="text/css" />');
  	},
	load: function(){
	    var allScript = document.getElementsByTagName("script");
	    var theOne = null,s;
    	for (var i = 0, length = allScript.length; i < length; i++){
			s = allScript[i];
	    	if (s.src && s.src.match(/TS-Include-FormPage\.js(\?.*)?$/)){
	    		theOne = s;
				//alert("scr=" + s.src);
	    		break;
	    	}
	    }
	    if (theOne != null){
	      var rootPath = theOne.src.replace(/.*TS-Include-FormPage\.js(\?rootPath=)?/,'');
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
TS_include_formPage.load();
/*
*使用条件，在Form 页面定义变量如下：
*actionUrl=XXXXAction.do
*/
//通用保存方法
function common_Save(){
        if(typeof onBtnSave_Clicked=="function"){//如果用户自己定义保存方法则使用用户定义的，否则调用默认的方法
             onBtnSave_Clicked.call(this);
        }else{
            egd_DefaultBtnSave_Click(); 
        }
}
//默认保存方法
function egd_DefaultBtnSave_Click(){
         // 必填域验证
	    if(typeof validate=="function") {
	        if(!validate())
	            return ; 
	    };
	    
	    var strUrl = TS.rootPath + actionUrl+"?action=SaveByAjax";
	    strUrl += "&timeStamp=" + new Date().getTime();
	    
	    TS.Msg.wait({msg:'正在保存文档，请稍候...', iconCls: TS.Msg.IconCls.SAVIND});
		new Ajax.Request(strUrl, 
		{ 
			method:'post', 
			parameters: Form.serialize($("thisForm")),
			onSuccess: function(transport){
				var response = eval("("+transport.responseText+")");  
				if (response.result == true){
				    var entity = response.entity;
 				    // 设置相关域的值
   				    $('_ID').value = entity.ID;
    				
    				// 判断是否显示删除按钮
    				var canDelete = (my.isManager && $F("_ID") > 0);
    				if (canDelete){
    				    thisPage.toolbar.show(["btnDelete"]);
    				}
    				
   				    TS.Msg.hide();// 隐藏保存提示窗口
    				TS.Msg.msgBox({title:"系统提示",msg: response.msg, iconCls: TS.Msg.IconCls.INFO});// 显示保存成功的提示信息
				}else{
    				TS.Msg.msgBox({title:"系统提示",msg: response.msg, iconCls: TS.Msg.IconCls.ERROR});
				}
			},
			onFailure: function(transport){
				TS.Msg.msgBox({title:"系统提示",msg: '保存过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
			}
		});	
}
//通用编辑方法
function common_Edit(){
       TS.Msg.wait({msg:'正在重新加载文档，请稍候...', iconCls: TS.Msg.IconCls.EDITING});
        var strUrl = TS.rootPath + actionUrl +"?action=Edit";
        strUrl += "&id=" + $F("_ID");
        strUrl += "&timeStamp=" + new Date().getTime();
        //window.open(strUrl,"_self");	// 该方法会导致整个主页闪烁一下
        TS.updateMe(strUrl);
}
//通用删除方法
function common_Delete(){
     if(typeof onBtnDelete_Clicked=="function"){//如果用户自己定义保存方法则使用用户定义的，否则调用默认的方法
             onBtnDelete_Clicked.call(this);
        }else{
            egd_DefaultBtnDelete_Click(); 
        }
}
//默认的删除方法
function egd_DefaultBtnDelete_Click(){
     // 删除确定
        TS.Msg.confirm({title:"系统提示",msg:"确定要删除当前记录吗？",onYes: function(){
	        var strUrl = TS.rootPath+actionUrl+"?action=Delete";
	        strUrl += "&ids=" + $F("_ID");
	        strUrl += "&timeStamp=" + new Date().getTime();
	        TS.Msg.wait({msg:'正在删除文档，请稍候...', iconCls: TS.Msg.IconCls.DELETING});
		    new Ajax.Request(strUrl, 
		    { 
			    onSuccess: function(transport){
				    var response = eval("("+transport.responseText+")");  
 				    if (response.result == true){
    				    //TS.Msg.msgBox({title:'部门',msg:response.msg});
    				    TS.Msg.hide();
    				    thisPage.closeMe();
				    }else{
    			        TS.Msg.msgBox({title:"系统提示", msg:response.msg, iconCls: TS.Msg.IconCls.ERROR});
				    }
			    },
			    onFailure: function(transport){
 				    TS.Msg.msgBox({title:"系统提示",msg: '删除过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
			    }
		    });	
        }});
}