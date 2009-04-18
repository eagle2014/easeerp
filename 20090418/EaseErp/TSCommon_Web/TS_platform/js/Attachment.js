
function onAddAttaches_Click(type,unid,twoFolder,fieldName,attachmentCount){
  var result = showModalDialog(TS.rootPath + "TS_Attachment/AttachmentForm.aspx?type=" + type + "&unid=" + unid + "&twoFolder=" + twoFolder + "&attachmentCount=" + attachmentCount + "&DateTime=" + Date(),"","status:no;resizable:no;dialogHeight:275px;dialogWidth:540px;help:no");


    if(result == null)
		return;
    // 更新列表值
	var attachmeObj = $(fieldName);
	if(null != attachmeObj){
		attachmeObj.length = 0;
		for(var i = 0; i < result[0].length; i++)
			IS_AppendItem(fieldName, result[1][i], result[0][i]);
	}
}

// 将指定源列表选中的选中的选项添加到指定的目标列表中
function IS_AppendItem(selectID, itemText, itemValue){ 
	// 首先取得指定的列表选项
	var sourceObj = $(selectID);
	
	if (null == sourceObj)
		return;
	
	// 如果尚未添加，那么添加
	if(!IS_IsSelected(selectID, itemValue)){
		var len = sourceObj.length;
		sourceObj.options[len] = new Option(itemText, itemValue);
	}
}

// 判断指定的列表中是否存在指定的值
function IS_IsSelected(selectID, selValue){ 
	if(selectID == "" || selValue == "")
		return false;
	
	// 循环每一个值判断是否相等
	var selectObj = $(selectID);
	for(j = 0; j < selectObj.length; j++){
		//alert(selectObj.options[j].value + "," + selValue);
		if(selectObj.options[j].value == selValue)
			return true;
	}
	
	return false;
}

function btnFinish_Click(fieldName){
	// 得到用户所选择的值
	var names = new Array();
	var values = new Array();
	var index = 0;
	var files = $(fieldName);
	for(i = 0; i < files.length; i++){
		if(files.options[i].value != "" && files.options[i].value != "undefined"){
			names[index] = files.options[i].text;
			values[index] = files.options[i].value;
			index++;
		}
	}
	var result = new Array();
	result[0] = values;
	result[1] = names;
	returnValue = result;	
	window.close();
}

function downloadFile(fieldName,unid){
    var sourceObj = $(fieldName);
    var fileName="";
    for (i = 0; i < sourceObj.length; i++){
		if (sourceObj.options[i].selected){
			fileName = sourceObj.options[i].value;
			break;
		}
	}
    if (fileName == null || fileName=="")
        return;
    var values=fileName.split("/");
    var id = values[1];
    window.open(TS.rootPath + "ATM/DownloadAttachment.aspx?id=" + id + "&unid=" + unid,"blank");
}

function delFile(fieldName, unid){
    var sourceObj = $(fieldName);
    var fileName="";
    var index = -1;
    for (i = 0; i < sourceObj.length; i++){
		if (sourceObj.options[i].selected){
			fileName = sourceObj.options[i].value;
			index = i;
			break;
		}
	}
    if (fileName == null || fileName=="")
        return;
    var values=fileName.split("/");
    var id = values[1];
    var ajaxUrl = "attachmentAction.do";
    var params={
        action:"DelAttachment",
        id:id,
        timestamp:new Date().getTime()
    };
    new Ajax.Request(ajaxUrl,{
        method:"post",
        parameters:params,
        onSuccess:function(transport){
            if(index != -1)
                sourceObj.options.remove(index);
        }
    });
}

function downloadFileOne(id,unid){
    window.open(TS.rootPath + "ATM/DownloadAttachment.aspx?id=" + id + "&unid=" + unid,"blank");
}