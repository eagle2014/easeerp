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
		this.initTab();
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
	 * 初始化Tab
	 */
    initTab: function(){
	    if($("tabContainer")){
	        tabs = new TS.TabPanel("tabContainer",[
	            {
	                id:"divRole",
	                text:"角色列表"
	            },
	            {
	                id:"divMember",
	                text:"包含人员"
	            }
	            /*,
	            {
	                id:"divWorkLog",
	                text:"工作日志",
	                handler:function(){
	                    openWorkLog();
	                }
	            }
	            */
	        ]);
	        //tabs.active(0);
	    }
    },
	
	/* 
	 * 验证表单信息的完整性
	 */
	validate:function(){
		var fieldNames = "OUName;Name;Code";
		var fieldDescs = "所属组织;名称;编码";
        return !TS.Msg.isEmptyValue(fieldNames,fieldDescs);
	},
	
	/* 
	 * 保存
	 */
	doSave:function(){
	    // 必填域验证
	    if(!this.validate()) return;
	    
	    var strUrl = TS.rootPath + "groupAction.do?action=SaveByAjax";
	    strUrl += "&timeStamp=" + new Date().getTime();
	    
	    // 选中多选框的值
	    IS_SelectAllTotal();
	    
	    TS.Msg.wait({msg:'正在保存岗位，请稍候...', iconCls: TS.Msg.IconCls.SAVIND});
		new Ajax.Request(strUrl, 
		{ 
			method:'post', 
			parameters: Form.serialize($("thisForm")),
			asynchronous:false,
			onSuccess: function(transport){
				var response = eval("("+transport.responseText+")");  
				if (response.result == true){
				    var entity = response.entity;
 				    // 设置相关域的值
   				    $('_ID').value = entity.ID;

    				// 判断是否显示删除按钮
    				var canDelete = (my.isManager && $F("_ID") > 0);
    				if (canDelete){
    				    //thisPage.toolbar.show(["btnDelete"]);
    				}
    				
   				    TS.Msg.hide();// 隐藏保存提示窗口
    				TS.Msg.msgBox({title:"岗位配置",msg: response.msg, iconCls: TS.Msg.IconCls.INFO});// 显示保存成功的提示信息
				}else{
    				TS.Msg.msgBox({title:"岗位配置",msg: response.msg, iconCls: TS.Msg.IconCls.ERROR});
				}
			},
			onFailure: function(transport){
				TS.Msg.msgBox({title:"岗位配置",msg: '保存岗位过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
			}
		});	
    },
    
	/* 
	 * 编辑
	 */
	doEdit:function(){
	    TS.Msg.wait({msg:'正在重新加载岗位，请稍候...', iconCls: TS.Msg.IconCls.EDITING});
        var strUrl = TS.rootPath + "groupAction.do?action=Edit";
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
        TS.Msg.confirm({title:"岗位配置",msg:"确定要删除当前岗位吗？",onYes: function(){
	        var strUrl = TS.rootPath + "groupAction.do?action=Delete";
	        strUrl += "&ids=" + $F("_ID");
	        strUrl += "&timeStamp=" + new Date().getTime();
	        TS.Msg.wait({msg:'正在删除岗位，请稍候...', iconCls: TS.Msg.IconCls.DELETING});
		    new Ajax.Request(strUrl, 
		    { 
			    onSuccess: function(transport){
				    var response = eval("("+transport.responseText+")");  
 				    if (response.result == true){
    				    //TS.Msg.msgBox({title:'岗位',msg:response.msg});
    				    TS.Msg.hide();
    				    thisPage.closeMe();
				    }else{
    			        TS.Msg.msgBox({title:"岗位", msg:response.msg, iconCls: TS.Msg.IconCls.ERROR});
				    }
			    },
			    onFailure: function(transport){
 				    TS.Msg.msgBox({title:"岗位",msg: '删除岗位过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
			    }
		    });	
        }});
    },
    
 	/* 
	 * 选择所属组织
	 */
    selectBelongOU: function(){
        TS.Org.selectOUInfo({
            title: '选择所属组织',
            onOk: function(ouInfo){
                if($F("OUUnid") == ouInfo.Unid){
                    return;    
                }else{
                    $("OUUnid").value = ouInfo.Unid;
                    $("OUName").value = ouInfo.Name;
                    $("OUFullName").value = ouInfo.FullName;
                    $("OUCode").value = ouInfo.Code;
                    $("OUFullCode").value = ouInfo.FullCode;
                   
                    // 更新岗位的关联关系
                    updateOnOUChange();
                }
            }
        });
    },
   
	noUsed:false
}


// 当用户所选的OU信息改变时
function updateOnOUChange(){
    var strUrl = TS.rootPath + "groupAction.do?action=UpdateOnOUChange";
	strUrl += "&ouUnid=" + $F("OUUnid");
	strUrl += "&timestamp" + new Date().getTime();
	IS_RemoveAllItemEx("AllRoles");
	IS_RemoveAllItemEx("RoleUnids");
	IS_RemoveAllItemEx("AllUserInfos");
	IS_RemoveAllItemEx("OUInfos");
	IS_RemoveAllItemEx("UserInfoUnids");
	var findRoleAjax = new Ajax.Request(strUrl, {method: 'get', onComplete: updateListInfo});
}

// 更新可选列表信息
function updateListInfo(request){
	var ajaxResponse = request.responseXML.documentElement;
	var entryList = ajaxResponse.getElementsByTagName("roleEntry");
	var info = null;
	for(var i = 0; i < entryList.length; i++){
		info = entryList[i];
		selObj = $("AllRoles");
		len = selObj.length;
		selObj.options[len] = new Option(getElementContent(info, "name"),
										 getElementContent(info, "value"));
	}
	
	entryList = ajaxResponse.getElementsByTagName("ouInfoEntry");
	for(var i = 0; i < entryList.length; i++){
		info = entryList[i];
		selObj = $("OUInfos");
		len = selObj.length;
		selObj.options[len] = new Option(getElementContent(info, "name"),
										 getElementContent(info, "value"));
	}
	
	entryList = ajaxResponse.getElementsByTagName("userInfoEntry");
	for(var i = 0; i < entryList.length; i++){
		info = entryList[i];
		selObj = $("AllUserInfos");
		len = selObj.length;
		selObj.options[len] = new Option(getElementContent(info, "name"),
										 getElementContent(info, "value"));
	}
}

// 当人员的OU信息改变时
function onOUInfos_Changed(){
    var strUrl = TS.rootPath + "userInfoAction.do?action=FindUserInfoWithOUUnid";
	strUrl += "&ouUnid=" + $F("OUInfos");
	strUrl += "&timestamp" + new Date().getTime();
	IS_RemoveAllItemEx("AllUserInfos");
	var findUserInfoAjax = new Ajax.Request(strUrl, {method: 'get', onComplete: updateUserInfo});
}

// 更新可选人员信息列表信息
function updateUserInfo(request){
	var ajaxResponse = request.responseXML.documentElement;
	var entryList = ajaxResponse.getElementsByTagName("entry");
	var info = null;
	for(var i = 0; i < entryList.length; i++){
		info = entryList[i];
		if(IS_IsSelected("UserInfoUnids", getElementContent(info, "value")))
		    continue;
		selObj = $("AllUserInfos");
		len = selObj.length;
		selObj.options[len] = new Option(getElementContent(info, "name"),
										 getElementContent(info, "value"));
	}
}

/*
var isLoadWorkLog = false;
function openWorkLog(){
	if(isLoadWorkLog)
		return;
	var iframeObj = $("WORKLOG");
	if(null == iframeObj)
		return false;
		
	var parentUnid = $F("unid");
	if(null == parentUnid || parentUnid == ""){
		parentUnid = " ";
	}
	var strUrl = "<%=ContextPath %>/Egd_WorkLog/WorkLogView.aspx?punid=" + parentUnid;
	strUrl += "&edit=Y";
	strUrl += "&timeStamp=" + new Date().getTime();
	isLoadWorkLog = true;
	iframeObj.src = strUrl;
}
*/