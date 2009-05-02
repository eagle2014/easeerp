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
	    if($("tabContainer")){
	        tabs = new TS.TabPanel("tabContainer",[
	            {
	                id:"divGroup",
	                text:"岗位列表",
	                handler:function(){}
	            }
	        ]);
	        tabs.active(0);
	    }
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
			},
		    {
			    id:"btnActivate",
			    text:"激活",
			    iconClass:"egd-button-activate",
			    handler:function(button){
			        //onEnabled_Click();
			        doEnabledUser();
			    },
			    // 管理员才能编辑
			    hidden: !(my.isManager && my.canEdit && !my.isUnidEmpty && $F('UserStatus') != 'Enable')
		    },
		    {
			    id:"btnForbid",
			    text:"禁止",
			    iconClass:"egd-button-forbid",
			    handler:function(button){
			      //  onDisabled_Click();
			      doDisabledUser();
			    },
			    // 管理员才能编辑
			    hidden: !(my.isManager && my.canEdit && !my.isUnidEmpty && $F('UserStatus') == 'Enable')
		    },
		    {
			    id:"btnModifyPassword",
			    text:"修改口令",
			    iconClass:"egd-button-password",
			    handler:function(button){
			        onChangePassword_Click();
			    },
			    // 管理员才能编辑
			    hidden: !(my.isManager && my.canEdit && !my.isUnidEmpty)
		    }
		]);
	},
	
	/* 
	 * 验证表单信息的完整性
	 */
	validate:function(){
		var fieldNames = "OUFullName;Name;LoginID;JobTitleUnid;OrderNo";
		var fieldDescs = "所属组织;姓名;登录帐号;职务;排序序号";
        return !TS.Msg.isEmptyValue(fieldNames,fieldDescs);
	},
	
	/* 
	 * 保存
	 */
	doSave:function(){
	    // 必填域验证
	    if(!this.validate()) return;
	    
	    var strUrl = TS.rootPath + "userAction.do?action=SaveByAjax";
	    strUrl += "&timeStamp=" + new Date().getTime();
	    
	    // 选中多选框的值
	    IS_SelectAllTotal();
	    
	    TS.Msg.wait({msg:'正在保存文档，请稍候...', iconCls: TS.Msg.IconCls.SAVIND});
		new Ajax.Request(strUrl, 
		{ 
			method:'post', 
			parameters: Form.serialize($("thisForm")),
			onSuccess: function(transport){
				var response = eval("("+transport.responseText+")");  
				if (response.result == true){
				    thisPage.toolbar.show(["btnModifyPassword"]);
				    if($F('UserStatus') == 'Enable'){
				        thisPage.toolbar.show(["btnForbid"]);
				        thisPage.toolbar.hide(["btnActivate"]);
				    }else{
				        thisPage.toolbar.hide(["btnForbid"]);
				        thisPage.toolbar.show(["btnActivate"]);
				    }
				    
				    var entity = response.entity;
 				    // 设置相关域的值
   				    $('_ID').value = entity.ID;
    				
    				// 判断是否显示删除按钮
    				var canDelete = (my.isManager && $F("_ID") > 0);
    				if (canDelete){
    				    thisPage.toolbar.show(["btnDelete"]);
    				}
    				
   				    //TS.Msg.hide();// 隐藏保存提示窗口
    			    TS.Msg.msgBox({title:"人员配置",msg: response.msg, iconCls: TS.Msg.IconCls.INFO});// 显示保存成功的提示信息
				}else{
    				TS.Msg.msgBox({title:"人员配置",msg: response.msg, iconCls: TS.Msg.IconCls.ERROR});
				}
			},
			onFailure: function(transport){
				TS.Msg.msgBox({title:"人员配置",msg: '保存人员过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
			}
		});	
    },
    
	/* 
	 * 编辑
	 */
	doEdit:function(){
	    TS.Msg.wait({msg:'正在重新加载文档，请稍候...', iconCls: TS.Msg.IconCls.EDITING});
        var strUrl = TS.rootPath + "userAction.do?action=Edit";
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
        TS.Msg.confirm({title:"人员配置",msg:"确定要删除当前人员吗？",onYes: function(){
	        var strUrl = TS.rootPath + "userAction.do?action=Delete";
	        strUrl += "&ids=" + $F("_ID");
	        strUrl += "&timeStamp=" + new Date().getTime();
	        TS.Msg.wait({msg:'正在删除人员，请稍候...', iconCls: TS.Msg.IconCls.DELETING});
		    new Ajax.Request(strUrl, 
		    { 
			    onSuccess: function(transport){
				    var response = eval("("+transport.responseText+")");  
 				    if (response.result == true){
    				    //TS.Msg.msgBox({title:'人员',msg:response.msg});
    				    TS.Msg.hide();
    				    thisPage.closeMe();
				    }else{
    			        TS.Msg.msgBox({title:"人员", msg:response.msg, iconCls: TS.Msg.IconCls.ERROR});
				    }
			    },
			    onFailure: function(transport){
 				    TS.Msg.msgBox({title:"人员",msg: '删除人员过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
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
                    
                    $("UnitUnid").value = ouInfo.UnitUnid;
                    $("UnitName").value = ouInfo.UnitName;
                    $("UnitFullName").value = ouInfo.UnitFullName;
                    $("UnitFullCode").value = ouInfo.UnitFullCode;
                    
                    // 更新岗位的关联关系
                    updateGroupInfos();
                }
            }
        });
    },
    
 	/* 
	 * 更改职务的选择
	 */
    changeJobTitle: function(){
        $('JobTitleName').value =$('JobTitleUnid').options[$('JobTitleUnid').selectedIndex].text;
    },
   
	noUsed:false
}

// 当用户所选的OU信息改变时
function updateGroupInfos(){
	var strUrl = TS.rootPath + "Organize/groupAction.do?action=FindGroupsWithOUUnid";
	strUrl += "&ouUnid=" + $F("OUUnid");
	strUrl += "&timestamp" + new Date().getTime();
	var selObj = $("AllGroups");
	if(null != selObj)
	    selObj.length = 0;
	selObj = $("GroupUnids");
	if(null != selObj)
	    selObj.length = 0;
	var findRoleAjax = new Ajax.Request(strUrl, {method: 'get', onComplete: updateRoleListInfo});
}

// 更新可选角色列表信息
function updateRoleListInfo(request){
	var ajaxResponse = request.responseXML.documentElement;
	var entryList = ajaxResponse.getElementsByTagName("entry");
	var info = null;
	for(var i = 0; i < entryList.length; i++){
		info = entryList[i];
		selObj = $("AllGroups");
		len = selObj.length;
		selObj.options[len] = new Option(getElementContent(info, "name"), getElementContent(info, "value"));
	}
}

/*
 *激活人员配置信息成功
 */
function doEnabledUser(){
    TS.Msg.confirm({title:"提示",msg:"确定要激活吗？",onYes: function(){
        var strUrl = TS.rootPath + "userAction.do?action=EnabledUser";
        strUrl += "&id=" + $F("_ID");
	    new Ajax.Request(strUrl, 
	    { 
	          asynchronous:false,
	     	  onSuccess: function(transport){
			    var response = transport.responseText;  
			    if (response == "ok"){
                    TS.Msg.msgBox({title:"提示",msg:"激活人员配置信息成功！"});
                    $('UserStatus').value = 'Enable';
			        thisPage.toolbar.show(["btnForbid"]);
			        thisPage.toolbar.hide(["btnActivate"]);
			    }else{
			        TS.Msg.msgBox({title:"提示", msg:'请选择激活人员', iconCls: TS.Msg.IconCls.ERROR});
			    }
		    },
		    onFailure: function(transport){
			    //TS.Msg.msgBox({title:"激活人员配置信息",msg: '激活人员配置信息[ID=' + $F('_ID') + ']过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
		    }
	    });	
    }});
 }
 
 /*
 *禁止人员配置信息成功
 */
function  doDisabledUser(){
    TS.Msg.confirm({title:"提示",msg:"确定要禁止吗？",onYes: function(){
        var strUrl = TS.rootPath + "userAction.do?action=DisabledUser";
        strUrl += "&id=" + $F("_ID");
	    new Ajax.Request(strUrl, 
	    { 
	          asynchronous:false,
	     	  onSuccess: function(transport){
			    var response = transport.responseText;  
			    if (response == "ok"){
                    TS.Msg.msgBox({title:"提示",msg:"禁止人员配置信息成功！"});
                    $('UserStatus').value = 'Disable';
			        thisPage.toolbar.hide(["btnForbid"]);
			        thisPage.toolbar.show(["btnActivate"]);
			    }else{
			        TS.Msg.msgBox({title:"提示", msg:'请选择禁止人员', iconCls: TS.Msg.IconCls.ERROR});
			    }
		    },
		    onFailure: function(transport){
			    //TS.Msg.msgBox({title:"禁止人员配置信息",msg: '禁止人员配置信息[ID=' + $F('_ID') + ']过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
		    }
	    });	
    }});
 }
 

function onChangePassword_Click()
{
    var result;
    var strUrl=TS.rootPath +"Organize/common/Dlg_InputPassword.aspx?timestamp" + new Date().getTime();  
	TS.Dlg.create({
            title: '请输入登录口令',
            width: 350,
            height: 150,
            minWidth: 350,
            minHeight:150,
            url: strUrl,
            asynchronous:false,
            onOk: function(value){                
                result=value;  
               doUpdatePassword(result);
               
            }
        });            

}
 
 /*
 *修改密码
 */
function  doUpdatePassword(password){
        var strUrl = TS.rootPath + "userAction.do?action=UpdateUser";
        strUrl += "&id=" + $F("_ID");
        strUrl += "&p=" + password;
	    new Ajax.Request(strUrl, 
	    { 
            asynchronous:false,
            onSuccess: function(transport){
                var response = eval("("+transport.responseText+")");  
                if (response.result == true){   // Ajax请求处理成功
                    TS.Msg.msgBox({title:"口令更新",msg:"口令更新成功！"});
                    $("Password").value = response.newEncryptPassword;
                }else{
                    TS.Msg.msgBox({title:"口令更新", msg:response.msg, iconCls: TS.Msg.IconCls.ERROR});
                }
		    },
		    onFailure: function(transport){
			    //TS.Msg.msgBox({title:"口令更新",msg: '口令更新[ID=' + $F('_ID') + ']过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
		    }
	    });	
    
 }