var thisPage = {
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
		
	    var tabs = new TS.TabPanel("tabContainer",[{
	        id:"divRole",
	        text:"角色权限",
	        handler:function(){}
	    }]);
	    
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
		var fieldNames = "Name;Code";
		var fieldDescs = "角色名称;角色代码";
        return !TS.Msg.isEmptyValue(fieldNames,fieldDescs);
	},
	
	/* 
	 * 保存
	 */
	doSave:function(){
	    // 必填域验证
	    if(!this.validate()) return;
	    
	    // 选中多选框的值
	    IS_SelectAllTotal();
	    
	    var strUrl = TS.rootPath + "roleAction.do?action=SaveByAjax";
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
    				TS.Msg.msgBox({title:"角色配置",msg: response.msg, iconCls: TS.Msg.IconCls.INFO});// 显示保存成功的提示信息
				}else{
    				TS.Msg.msgBox({title:"角色配置",msg: response.msg, iconCls: TS.Msg.IconCls.ERROR});
				}
			},
			onFailure: function(transport){
				TS.Msg.msgBox({title:"角色配置",msg: '保存角色过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
			}
		});	
    },
    
	/* 
	 * 编辑
	 */
	doEdit:function(){
	    TS.Msg.wait({msg:'正在重新加载文档，请稍候...', iconCls: TS.Msg.IconCls.EDITING});
        var strUrl = TS.rootPath + "roleAction.do?action=Edit";
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
        TS.Msg.confirm({title:"角色配置",msg:"确定要删除当前角色吗？",onYes: function(){
	        var strUrl = TS.rootPath + "roleAction.do?action=Delete";
	        strUrl += "&ids=" + $F("_ID");
	        strUrl += "&timeStamp=" + new Date().getTime();
	        TS.Msg.wait({msg:'正在删除文档，请稍候...', iconCls: TS.Msg.IconCls.DELETING});
		    new Ajax.Request(strUrl, 
		    { 
			    onSuccess: function(transport){
				    var response = eval("("+transport.responseText+")");  
 				    if (response.result == true){
    				    //TS.Msg.msgBox({title:'角色',msg:response.msg});
    				    TS.Msg.hide();
    				    thisPage.closeMe();
				    }else{
    			        TS.Msg.msgBox({title:"角色", msg:response.msg, iconCls: TS.Msg.IconCls.ERROR});
				    }
			    },
			    onFailure: function(transport){
 				    TS.Msg.msgBox({title:"角色",msg: '删除角色过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
			    }
		    });	
        }});
    },
    
 	/* 
	 * 级别变动处理
	 */
    changeLevel: function(){
        var selObj = $("Level");
        var selOption = selObj.options[selObj.selectedIndex];
        $("LevelName").value = selOption.text;
    },
    
	noUsed:false
}

// 当用户所选的模块信息改变时
function onModel_Changed(){
	var strUrl = TS.rootPath + "roleAction.do?action=FindPrivilege";
	strUrl += "&modelID=" + $F("Models");
	strUrl += "&timestamp" + new Date().getTime();
	var selObj = $("AllPrivilege");
	if(null != selObj)
	    selObj.length = 0;
	var findPrivilegeAjax = new Ajax.Request(strUrl, {method: 'get', onComplete: updatePrivilegeListInfo});
}

// 更新可选角色列表信息
function updatePrivilegeListInfo(request){
	var ajaxResponse = request.responseXML.documentElement;
	var entryList = ajaxResponse.getElementsByTagName("entry");
	var info = null;
	for(var i = 0; i < entryList.length; i++){
		info = entryList[i];
		if(IS_IsSelected("PrivilegeIDs", getElementContent(info, "value")))
		    continue;
		var selObj = $("AllPrivilege");
		len = selObj.length;
		selObj.options[len] = new Option(getElementContent(info, "name"),
										 getElementContent(info, "value"));
	}
}

