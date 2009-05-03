var groupDomains = my.groupsJson;
var userInfoDomains = [];

/*
 * 返回选中值的函数
 */
function DlgOkFn(){
    // 判断是否已经选择用户
    var userInfoOptionsObj = $("AllUserInfo");
    if(userInfoOptionsObj.selectedIndex == -1){
        TS.Msg.msgBox({msg:"请先选择用户！", iconCls: TS.Msg.IconCls.WARNING});
	    return null;
    }
    
    // 判断是否已经选择岗位
    var groupOptionsObj = $("AllGroup");
    if(groupOptionsObj.selectedIndex == -1){
        TS.Msg.msgBox({msg:"请先选择岗位！", iconCls: TS.Msg.IconCls.WARNING});
	    return null;
    }
    
    // 获取选择的岗位
    var group = null;
    var value = groupOptionsObj.value;
    for(var i = 0; i < groupDomains.length; i++){
        if(value == groupDomains[i].Unid){
            group = groupDomains[i];
            break;
        }
    }
    
    if(my.singleSelect){    // 单选处理
        var userInfo = null;
        var value = userInfoOptionsObj.value;
        for(var i = 0; i < userInfoDomains.length; i++){
            if(value == userInfoDomains[i].Unid){
                userInfo = userInfoDomains[i];
                break;
            }
        }
        return {group: group, userInfo: userInfo}
    }else{                  // 多选处理
        var _options = userInfoOptionsObj.options;
        var userInfos = new Array();
        for(var i = 0; i < _options.length; i++){
            if(_options[i].selected){
                userInfos.push(userInfoDomains[i]);
            }
        }
        return {group: group, userInfos: userInfos}
    }
}

var thisPage = {
    init: function(){
        //thisPage.initTree();
    },
    
    /*
     * 选择OU。OU变动后，重新加载岗位列表
     */
    selectOU: function(groupOptionsObj){
        TS.Org.selectOUInfo({
            copyTo:[{name:'OUName',mapping:'FullName'},{name:'OUUnid',mapping:'Unid'}],
            onOk: function(ouInfo){
                if(!(ouInfo.Unid) || ouInfo.Unid == $F('OUUnid') || ouInfo.Unid.length == 0)
                    return;
                
                // 重新加载岗位列表
	            var strUrl = TS.rootPath + "groupAction.do?action=FindGroupByOU&ouUnid=" + ouInfo.Unid;
	            if(my.groupType.length > 0) 
	                strUrl += "&groupType=" + my.groupType;
        
                //alert("strUrl=" + strUrl);
	            new Ajax.Request(strUrl, {	
	  	            onSuccess: function(transport){
	  		            groupDomains = eval("(" + transport.responseText + ")");
			            //alert(transport.responseText);
                        var groupOptionsObj = $("AllGroup");
	                    groupOptionsObj.length = 0;
	                    $("AllUserInfo").length = 0;
			            for(var i = 0; i < groupDomains.length; i++){
				            groupOptionsObj.options[groupOptionsObj.length] = new Option(groupDomains[i].Name + " [" + groupDomains[i].OUFullName + "]", groupDomains[i].Unid);
			            }
		            },
		            onFailure: function(transport){
    			        TS.Msg.msgBox({msg: '加载岗位信息出错！', iconCls: TS.Msg.IconCls.ERROR});
		            }
                });
            }
        });
    },
    
    /*
     * 岗位选择变动后，重新加载人员列表
     */
    onChangeGroup: function(groupOptionsObj){
        var strUrl = TS.rootPath + "userInfoAction.do?action=FindUserInfoByGroup&groupUnid=" + $F('AllGroup');
	    if(my.userType.length > 0) strUrl += "&userType=" + my.userType;
        //alert("strUrl=" + strUrl);
        new Ajax.Request(strUrl, {	
            onSuccess: function(transport){
	            userInfoDomains = eval("(" + transport.responseText + ")");
	            //alert(transport.responseText);
                var userInfoOptionsObj = $("AllUserInfo");
                userInfoOptionsObj.length = 0;
	            for(var i = 0; i < userInfoDomains.length; i++){
		            userInfoOptionsObj.options[userInfoOptionsObj.length] = new Option(userInfoDomains[i].Name, userInfoDomains[i].Unid);
	            }
            },
            onFailure: function(transport){
                var msg = (transport && transport.responseText) ? transport.responseText : '加载用户信息出错！';
		        TS.Msg.msgBox({msg: msg, iconCls: TS.Msg.IconCls.ERROR});
            }
        });
    }
};