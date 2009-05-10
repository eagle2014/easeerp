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
				}
			},
			{
			    id:"btnEdit",
			    text:"修改口令",
			    iconClass:"egd-button-password",
			    handler:function(button){
			        onChangePassword_Click();
			    }
			}
		]);
	},
	
	/* 
	 * 保存
	 */
	doSave:function(){
	    var strUrl = TS.rootPath + "personalSettingAction.do?action=SaveByAjax";
	    strUrl += "&timeStamp=" + new Date().getTime();
	    
	    TS.Msg.wait({msg:'正在保存修改，请稍候...', iconCls: TS.Msg.IconCls.SAVIND});
		new Ajax.Request(strUrl, 
		{ 
			method:'post', 
			parameters: Form.serialize($("thisForm")),
			onSuccess: function(transport){
				var response = eval("("+transport.responseText+")");  
				if (response.result == true){
   				    TS.Msg.hide();// 隐藏保存提示窗口
    				TS.Msg.msgBox({title:"个人设置",msg: response.msg, iconCls: TS.Msg.IconCls.INFO});// 显示保存成功的提示信息
				}else{
    				TS.Msg.msgBox({title:"个人设置",msg: "输入信息有误，请输入正确的格式数据!", iconCls: TS.Msg.IconCls.ERROR});
				}
			},
			onFailure: function(transport){
				TS.Msg.msgBox({title:"个人设置",msg: '保存个人设置过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
			}
		});	
    }
}

function onChangePassword_Click()
{
    var result;
    var strUrl= TS.rootPath + "egd_platform/common/Dlg_InputPassword.aspx?timestamp" + new Date().getTime();  
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
function doUpdatePassword(password){
    var strUrl = TS.rootPath + "userInfoAction.do?action=UpdateUserInfo";
    strUrl += "&id=" + $F("_ID");
    strUrl += "&p=" + password;
    new Ajax.Request(strUrl, 
    { 
          asynchronous:false,
     	  onSuccess: function(transport){
	        var response = eval("("+transport.responseText+")");  
	        if (response.result == true){   // Ajax请求处理成功
                TS.Msg.msgBox({title:"口令更新",msg:"口令更新成功！"});
                //$("Password").value = response.newEncryptPassword;
                //alert($F("Password"));
		    }else{
		        TS.Msg.msgBox({title:"口令更新", msg:response.msg, iconCls: TS.Msg.IconCls.ERROR});
		    }
	    },
	    onFailure: function(transport){
		    TS.Msg.msgBox({title:"口令更新", msg:"口令更新失败！", iconCls: TS.Msg.IconCls.ERROR});
	    }
    });	
 }