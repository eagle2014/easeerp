////////////////////////////////////////////////////////////////////////////////
// 
// 作者：CD826
// 日期：2006-07-10
// 函数功能： 
//     本函数库提供对Combox处理的功能。
// 2006-7-10
// 注意：
//     1、本函数库必须与prototype.js函数库相结合来使用
// 
////////////////////////////////////////////////////////////////////////////////

// 判断指定的列表中是否存在指定的值
function IS_IsSelected(selectID, selValue){ 
	if(selectID == "" || selValue == "")
		return false;
	
	// 循环每一个值判断是否相等
	var selectObj = $(selectID);
	if(null == selectObj)
		return false;
	for(j = 0; j < selectObj.length; j++){
		if(selectObj.options[j].value == selValue)
			return true;
	}
	return false;
}

// 将指定源列表选中的选中的选项添加到指定的目标列表中
function IS_AppendSelectedItem(selectID, destID, bClear){
	// 首先取得指定的列表选项
	var sourceObj = $(selectID);
	var destObj	  = $(destID);
	
	if (null == sourceObj || null == destObj)
		return;
	
	// 首先清空空白选项
	IS_RemoveBlankItem(destID);
	
	// 循环用户选定的每一个项目，将该项添加到指定列表中
	for (i = 0; i < sourceObj.length; i++){
		if (sourceObj.options[i].selected){
			var selValue = sourceObj.options[i].value;
			
			// 判断是否已经添加
			if(!IS_IsSelected(destObj, selValue)){
				len = destObj.length;
				destObj.options[len] = new Option(sourceObj.options[i].text, sourceObj.options[i].value);
			}
		}
	}
	
     // CLEAR
	 if(bClear)
	 	IS_RemoveItem(selectID); 
}

// 将指定源列表选中的所有选项添加到指定的目标列表中
function IS_AppendAllItem(selectID, destID, bClear){
	IS_SelectTotal(selectID);
	IS_AppendSelectedItem(selectID, destID, bClear);
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

// 将列表中选中的选项删除
function IS_RemoveItem(selectID){
	var selectObj = $(selectID);
	if(null == selectObj)
		return;

	var chkIndex = new Array();
	for(i = selectObj.length -1; i >= 0; i--){
		if(selectObj.options[i].selected){
			chkIndex[i] = true;
			selectObj.options[i].selected = false;
		}else
			chkIndex[i] = false;
	}
	
	// 移除选项
	for(i = selectObj.length -1; i >= 0; i--){
		if(chkIndex[i])
			selectObj.options[i] = null;
	}
}

// 判断指定列表中是否选择选项
function IS_isSelectAnyItem(selectID){
	if(selectID == "")
		return false;
	
	var selectObj = $(selectID);
	if(null == selectObj)
		return false;
	
	// 循环每一个值判断是否选中
	for(i = 0; i < selectObj.length; i++){
		if (selectObj.options[i].selected){
			if(selectObj.options[i].value != "")
				return true;
		}
	}
	return false;
}

// 判断指定列表中是否有选项
function IS_isHasAnyItem(selectID){
	if(selectID == "")
		return false;
	
	var selectObj = $(selectID);
	if(null == selectObj)
		return false;
		
	// 循环每一个值判断是否选中
	for(i = 0; i < selectObj.length; i++){
		if(selectObj.options[i].value != "")
			return true;
	}
	return false;
}

// 将列表中的空白选项删除
function IS_RemoveBlankItem(selectID){
	if(selectID == "")
		return ;
	
	var selectObj = $(selectID);
	if(null == selectObj)
		return ;
		
	var blankIndex = new Array();
	for(i = selectObj.length -1; i >= 0; i--){
		if(selectObj.options[i].text == ""){
			blankIndex[i] = true;
			selectObj.options[i].selected = false;
		}else
			blankIndex[i] = false;
	}

	// 移除选项
	for(i = selectObj.length -1; i >= 0; i--){
		if(blankIndex[i])
			selectObj.options[i] = null;
	}
}

// 将列表中选中的选项置为非选中
function IS_ClearItem(selectID){
	if(selectID == "")
		return ;
	
	var selectObj = $(selectID);
	if(null == selectObj)
		return ;
	
	for(i = selectObj.length -1; i >= 0; i--){
		selectObj.options[i].selected = false;
	}
}

// 全部选中指定列表中的选项
function IS_SelectTotal(selectID){
	if(selectID == "")
		return ;
	
	var selectObj = $(selectID);
	if(null == selectObj)
		return ;

 	for(nIndex = 0; nIndex < selectObj.length; nIndex++)
   		selectObj.options[nIndex].selected = true;
}

// 删除全部内容
function IS_RemoveAllItem(selectID){
	// 选中全部选项
	IS_SelectTotal(selectID);
	
	// CLEAR
	IS_RemoveItem(selectID);	
}

// 选中所有类型为"select0－multiple"域中的选项
function IS_SelectAllTotal(){
	// 选中所有选项
	for(var ObjID=0; ObjID < document.forms[0].elements.length; ObjID++){
		try{		
			if (document.forms[0].elements[ObjID].type.toLowerCase() == "select-multiple" ){
				for (nIndex = 0; nIndex < document.forms[0].elements[ObjID].length; nIndex++)
					document.forms[0].elements[ObjID].options[nIndex].selected = true;
			}
		}catch(e){
		}
	}
}

// 将列表中选中的选项删除
function IS_RemoveAllItemEx(selectID){
	if(selectID == "")
		return ;
	
	var selectObj = $(selectID);
	if(null == selectObj)
		return ;
	
	// 移除选项
	selectObj.length = 0;
}