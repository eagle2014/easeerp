﻿var thisPage = {
	/* 
	 * 关闭当前页面
	 */
	closeMe:function(){
	    TS.closeMe();
	},
	
	/* 
	 * 页面初始化方法
	 */
	init:function(){
		this.initTB();
		TS.Msg.hide();
	},
	
	/* 
	 * 初始化工具条
	 */
	initTB:function(){
		this.toolbar = new TS.TB("tbContainer",[
			"|",
			{
				id:"btnSave",
				text:"保存",
				iconClass:"egd-button-save",
				handler:function(button){
				    thisPage.doSave();
				},
			    // 管理员才能保存
			    hidden: !(my.isManager && my.canEdit)
			},
			{
				id:"btnEdit",
				text:"编辑",
				iconClass:"egd-button-edit",
				handler:function(button){
				    thisPage.doEdit();
				},
			    // 管理员才能编辑
			    hidden: !(my.isManager && !my.canEdit)
			},
			{
				id:"btnDelete",
				text:"删除",
				iconClass:"egd-button-delete",
				handler:function(button){
					thisPage.doDelete();
				},
			    // 管理员才能删除
			    hidden: !(my.isManager && ($F("_ID") > 0))
			}
		]);
	},
	
	/* 
	 * 验证表单信息的完整性
	 */
	validate:function(){
		var fieldNames = "Type;Name;Code";
		var fieldDescs = "所属分类;名称;编码";
        return !TS.Msg.isEmptyValue(fieldNames,fieldDescs);
	},
	
	/* 
	 * 保存
	 */
	doSave:function(){
	    // 必填域验证
	    if(!this.validate()) return;
	    
	    var strUrl = TS.rootPath + "optionItemAction.do?action=SaveByAjax";
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
    				TS.Msg.msgBox({title:"可选项配置",msg: response.msg, iconCls: TS.Msg.IconCls.INFO});// 显示保存成功的提示信息
				}else{
    				TS.Msg.msgBox({title:"可选项配置",msg: response.msg, iconCls: TS.Msg.IconCls.ERROR});
				}
			},
			onFailure: function(transport){
				TS.Msg.msgBox({title:"可选项配置",msg: '保存可选项过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
			}
		});	
    },
    
	/* 
	 * 编辑
	 */
	doEdit:function(){
	    TS.Msg.wait({msg:'正在重新加载文档，请稍候...', iconCls: TS.Msg.IconCls.EDITING});
        var strUrl = TS.rootPath + "optionItemAction.do?action=Edit";
        strUrl += "&id=" + $F("_ID");
        strUrl += "&timeStamp=" + new Date().getTime();
        //window.open(strUrl,"_self");	// 该方法会导致整个主页闪烁一下
        TS.updateMe(strUrl);
    },
	
	/* 
	 * 删除
	 */
	doDelete:function(){
	    // 删除确定
        TS.Msg.confirm({title:"可选项配置",msg:"确定要删除当前可选项吗？",onYes: function(){
	        var strUrl = TS.rootPath + "optionItemAction.do?action=Delete";
	        strUrl += "&ids=" + $F("_ID");
	        strUrl += "&timeStamp=" + new Date().getTime();
	        TS.Msg.wait({msg:'正在删除文档，请稍候...', iconCls: TS.Msg.IconCls.DELETING});
		    new Ajax.Request(strUrl, 
		    { 
			    onSuccess: function(transport){
				    var response = eval("("+transport.responseText+")");  
 				    if (response.result == true){
    				    //TS.Msg.msgBox({title:'可选项',msg:response.msg});
    				    TS.Msg.hide();
    				    thisPage.closeMe();
				    }else{
    			        TS.Msg.msgBox({title:"可选项", msg:response.msg, iconCls: TS.Msg.IconCls.ERROR});
				    }
			    },
			    onFailure: function(transport){
 				    TS.Msg.msgBox({title:"可选项",msg: '删除可选项过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
			    }
		    });	
        }});
    },
    
	/* 
	 * 所属分类改变
	 */
	changeType:function(){
	    var typeObj = $("Type");
		$("TypeName").value = typeObj.options[typeObj.selectedIndex].text;
		//alert("TypeName=" + $("TypeName").value);
    },
    
	noUsed:false
}
