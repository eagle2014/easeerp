// 选择人员信息
function selectUserInfo(filedNamePrefix,callback){
	// 首先清空可选人员的信息
	var result = selectUser();
	if(null == result)
		return;
    if (filedNamePrefix == null) filedNamePrefix = "";
	if(typeof callback == "function")
		callback.call(this,result[1]);
  else
  	findUserInfo(result[1],filedNamePrefix);
}

// 根据人员姓名查找人员信息
function findUserByName(userNameID, prefix){
	// 首先清空可选人员的信息
	clearUserListInfo();
	clearUserInfo(true, userNameID, prefix);

	// 如果人员姓名的位数小于2位那么不进行查询
	var userName = $F(userNameID);	
	if(userName.length < 1)
		return;
		
	//alert(userName);
	findUser("N", userName, prefix);
}

// 根据人员登录姓名查询人员信息
function findUserByLoginName(loginNameID, prefix){
	// 首先清空可选人员的信息
	clearUserListInfo();
	clearUserInfo(true, loginNameID, prefix);

	// 如果人员姓名的位数小于3位那么不进行查询
	var loginName = $F(loginNameID);
	if(loginName.length < 3)
		return;
	findUser("L", loginName, prefix);	
}

// 根据电话号码查询人员信息
function findUserByTelphone(telphoneID, prefix){
	// 首先清空可选人员的信息
	clearUserListInfo();
	clearUserInfo(true, telphoneID, prefix);

	// 如果电话号码的位数小于7位那么不进行查询
	var telphone = $F(telphoneID);
	if(telphone.length < 7)
		return;
	findUser("T", telphone, prefix);
}

// 查询人员信息
function findUser(type, _param, prefix){
	var strUrl = TS.rootPath + "Egd_Organize/UserInfoForm.aspx?action=findUser"
	strUrl += "&type=" + type;
	strUrl += "&param=" + _param;
	strUrl += "&timestamp" + new Date().getTime();
	var findUserAjax = new Ajax.Request(strUrl, {
			method: 'post', 
			onComplete: updateUserListInfo
		});
}

// 更新可选人员列表信息
function updateUserListInfo(request){	
	// 得到用户的数目
	var ajaxResponse = request.responseXML.documentElement;
	var userCount = getElementContent(ajaxResponse, "count");
	
	var userList = ajaxResponse.getElementsByTagName("user");
	var userInfo = null;
	for(var i = 0; i < userList.length; i++){
		userInfo = userList[i];
		selUserObj = $("selUser");
		len = selUserObj.length;
		selUserObj.options[len] = new Option(getElementContent(userInfo, "name"),
										     getElementContent(userInfo, "unid"));
	}
}

// 清空可选人员的信息
function clearUserListInfo(){
	var selUserObj = $("selUser");
	if(null == selUserObj)
		return;
	selUserObj.length = 0;
}

// 用户双击选择了人员后
function onSelUserDbl_Click(filedNamePrefix){
	var selID = $F("selUser");
	if(null == selID || selID == "")
		return;
    if (filedNamePrefix == null) filedNamePrefix = "";
	findUserInfo(selID,filedNamePrefix);
}

// 获取用户的信息
function findUserInfo(userID,filedNamePrefix){
	// 如果id为空，则退出
	if(userID.length < 0 || userID == "")
		return;

    if (filedNamePrefix == null) filedNamePrefix = "";
	var strUrl = TS.rootPath + "Egd_Organize/UserInfoForm.aspx?action=getUserInfo"
	strUrl += "&unid=" + userID;
	strUrl += "&filedNamePrefix=" + filedNamePrefix;
	strUrl += "&timestamp" + new Date().getTime();
	//alert('strUrl=' + strUrl);
	var findUserAjax = new Ajax.Request(strUrl, 
	    {   method: 'post', 
	        onComplete: function(request){
	            updateUserInfo.call(this,request,filedNamePrefix); 
	        }
	    });
}

// 更新用户的信息
function updateUserInfo(request,prefix){
	var ajaxResponse = request.responseXML.documentElement;
	var fp;
	if((typeof prefix != "undefined") && prefix != null && prefix.length > 0)
	    fp = prefix;
	else
	    fp = getElementContent(ajaxResponse, "FiledNamePrefix");
	    
	if($(fp + "Unid")) $(fp + "Unid").value = getElementContent(ajaxResponse, "Unid");
	if($(fp + "Name")) $(fp + "Name").value = getElementContent(ajaxResponse, "Name");
	if($(fp + "UnitName")) $(fp + "UnitName").value = getElementContent(ajaxResponse, "UnitName");
	if($(fp + "UnitUnid")) $(fp + "UnitUnid").value = getElementContent(ajaxResponse, "UnitUnid");
	if($(fp + "OUUnid")) $(fp + "OUUnid").value = getElementContent(ajaxResponse, "OUUnid");
	if($(fp + "OUName")) $(fp + "OUName").value = getElementContent(ajaxResponse, "OUName");
	if($(fp + "LoginName")) $(fp + "LoginName").value = getElementContent(ajaxResponse, "LoginID");
	if($(fp + "LoginID")) $(fp + "LoginID").value = getElementContent(ajaxResponse, "LoginID");
	if($(fp + "TelphoneNo")) $(fp + "TelphoneNo").value = getElementContent(ajaxResponse, "TelephoneNo");// ToDo Old
	if($(fp + "TelephoneNo")) $(fp + "TelephoneNo").value = getElementContent(ajaxResponse, "TelephoneNo");
	if($(fp + "JobTitleName")) $(fp + "JobTitleName").value = getElementContent(ajaxResponse, "JobTitleName");
	if($(fp + "Office")) $(fp + "Office").value = getElementContent(ajaxResponse, "Office");
	if($(fp + "Address")) $(fp + "Address").value = getElementContent(ajaxResponse, "Address");
	if($(fp + "Email")) $(fp + "Email").value = getElementContent(ajaxResponse, "Email");
	if($(fp + "Mobile")) $(fp + "Mobile").value = getElementContent(ajaxResponse, "Mobile");
	if($(fp + "EmployeeID")) $(fp + "EmployeeID").value = getElementContent(ajaxResponse, "EmployeeID");
	//alert('ajaxResponse=' + request.responseText);
}

// 清空用户的信息
function clearUserInfo(keepUnid, keepInfoID, prefix){
	if(keepInfoID != "RequesterName")
		if($("RequesterName")) $("RequesterName").value = "";
	if(keepInfoID != "RequesterLoginName")
		if($("RequesterLoginName")) $("RequesterLoginName").value = "";
	if(keepInfoID != "RequesterTelphoneNo")
		if($("RequesterTelphoneNo")) $("RequesterTelphoneNo").value = "";
	
	if($("RequesterUnitName")) $("RequesterUnitName").value = "";
	if($("RequesterOUUnid")) $("RequesterOUUnid").value = "";
	if($("RequesterUnitUnid")) $("RequesterUnitUnid").value = "";
	if($("RequesterOUName")) $("RequesterOUName").value = "";
	if($("RequesterJobTitleName")) $("RequesterJobTitleName").value = "";
	if($("RequesterOffice")) $("RequesterOffice").value = "";
	if($("RequesterAddress")) $("RequesterAddress").value = "";
	if($("RequesterEmail")) $("RequesterEmail").value = "";

	if(!keepUnid)
		if($("RequesterUnid")) $("RequesterUnid").value = "";

	if(prefix){
	    if($(prefix + "Name" && keepInfoID != (prefix + "Name"))) $(prefix + "Name").value = "";
	    if($(prefix + "Unid")) $(prefix + "Unid").value = "";
	    if($(prefix + "OUUnid")) $(prefix + "OUUnid").value = "";
	    if($(prefix + "OUName")) $(prefix + "OUName").value = "";
	    if($(prefix + "UnitUnid")) $(prefix + "UnitUnid").value = "";
	    if($(prefix + "UnitName")) $(prefix + "UnitName").value = "";
	    if($(prefix + "LoginID") && keepInfoID != (prefix + "LoginID")) $(prefix + "LoginID").value = "";
	    if($(prefix + "TelephoneNo" && keepInfoID != (prefix + "TelephoneNo"))) $(prefix + "TelephoneNo").value = "";
	    if($(prefix + "Email")) $(prefix + "Email").value = "";
	    if($(prefix + "Address")) $(prefix + "Address").value = "";
	    if($(prefix + "Office")) $(prefix + "Office").value = "";
	    if($(prefix + "JobTitleName")) $(prefix + "JobTitleName").value = "";
    }
}

function getElementContent(element, tagName){
	var childElement = element.getElementsByTagName(tagName)[0];
	return childElement.text != undefined ? childElement.text : childElement.textContent;
}