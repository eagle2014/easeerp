// 主页中对话框及信息框函数的封装
// 广州市忆科计算机系统有限公司 2004-2007 版权所有
// Tony 2007-12-01
// 需引用Prototype.js,TS.js

/**
 * 打开一个窗口，如果指定的窗口已经存在则激活该窗口
 *
 * @config {String} title 窗口的标题，默认为tabID
 * @config {String} url 窗口的URL，默认为"..../egd_platform/common/Blank.aspx"
 *
 * @param {String} id 可选配置：窗口对应的ID，默认为'EWIN' + new Date().getTime()
 * @config {Boolean} closable 可选配置：窗口是否可被关闭，默认为true
 * @config {String} tabTip 可选配置：窗口的提示信息
 * @config {Boolean} refresh 可选配置：标识当关闭当前窗口后,是否应该通知父窗口需要执行更新操作 (默认为false,该属性只有在opener存在的情况下才有效)
 * @config {Boolean} closable 可选配置：标识创建的窗口是否可由用户关闭(默认为true)
 * @config {String} iconCls 可选配置：页签图标对应的样式名
 */
TS.openWindow = function(config){
    Try.these(
      function() {parent.openWindow(config)},
      function() {parent.parent.openWindow(config)},
      function() {parent.parent.parent.openWindow(config)}
    );
};

/**
 * 关闭一个窗口，如果没有指定任何参数，则关闭当前窗口
 */
TS.closeWindow = function(winId){
    Try.these(
      function() {parent.closeWindow(winId)},
      function() {parent.parent.closeWindow(winId)},
      function() {parent.parent.parent.closeWindow(winId)}
    );
};

/**
 * 关闭当前窗口
 */
TS.closeMe = function(){
    Try.these(
      function() {parent.closeMe()},
      function() {parent.parent.closeMe()},
      function() {parent.parent.parent.closeMe()}
    );
};

/**
 * 是否使用多窗口主页
 */
TS.isMDI = function(){
    return Try.these(
      function() {return isMDI()},
      function() {return parent.isMDI()},
      function() {return parent.parent.isMDI()},
      function() {return parent.parent.parent.isMDI()}
    );
};

/**
 * 关闭当前窗口
 */
TS.updateMe = function(newUrl){
    Try.these(
      function() {parent.updateMe(newUrl)},
      function() {parent.parent.updateMe(newUrl)},
      function() {parent.parent.parent.updateMe(newUrl)}
    );
};

/**
 * 转到登录页面重新登录
 */
TS.reLogin = function(){
    window.open(TS.rootPath + "Default.aspx", "_top");
};

TS.namespace("TS.Dlg");
/* iframe交互对话框 */
TS.apply(TS.Dlg,{
    /* 
     * 隐藏指定id的对话框
     */
    hide: function(dlgId){
        Try.these(
          function() {parent.hideDialog(dlgId)},
          function() {parent.parent.hideDialog(dlgId)},
          function() {parent.parent.parent.hideDialog(dlgId)}
        );
    },

    /* 
     * 关闭指定id的对话框
     */
    close: function(dlgId){
        Try.these(
          function() {parent.closeDialog(dlgId)},
          function() {parent.parent.closeDialog(dlgId)},
          function() {parent.parent.parent.closeDialog(dlgId)}
        );
    },

    /**
     * 创建iframe交互对话框
     *
     * @param {Object} config 对话框配置 
     * @config {String} dlgId 可选参数：id 对话框ID,同时也是缓存对话框的键值,一个id只会创建唯一一个对话框与之对应，默认为defaultDlgId
     * @config {String} title 对话框标题
     * @config {String} url 对话框内容的url
     * @config {Number} width 对话框宽度
     * @config {Number} height 对话框高度
     * @config {Boolean} modal 是否为模式对话框，默认为true
     * @config {Boolean} refresh 是否每次显示对话框后都强制刷新对话框内的内容，默认为false。如果对话框内的iframe内定义有refresh函数，会优先调用该函数，否则整个iframe重新加载
     * @param {Function} onOk 可选参数：默认的“确定”按钮的回调函数，函数的第一个参数为点击确认按钮所执行函数的返回值
     * @param {Boolean} showCancelButton 可选参数：是否添加默认的“取消”按钮，默认为true
     * @param {Array} buttons 可选参数：自定义对话框的所有按钮，如果配置该参数，将忽略onOk参数的配置
     *      数组内的元素为按钮button的配置，格式为{text:'保存', fnName:'TSDlgSaveFn',onOk:function(result){alert(result)}}
     *      @button {String} text 按钮显示的名称
     *      @button {String} fnName IFrame内获取按钮返回值的函数名称
     *      @button {Function} onOk 点击按钮的回调函数，函数的第一个参数为IFrame内fnName函数的返回值
     */
    create: function(config){
        Try.these(
          function() {return parent.createDialog(config)},
          function() {return parent.parent.createDialog(config)},
          function() {return parent.parent.parent.createDialog(config)}
        );
    },
    
    /**
     * 引发对话框按钮的点击处理事件
     *
     * @param {Number} btnIndex 按钮的索引号
     * @param {String} dlgId 可选参数：对话框ID,默认为defaultDlgId
     */
    fireButtonEvent: function(btnIndex, dlgId){
        Try.these(
          function() {return parent.fireDialogButtonEvent(btnIndex, dlgId)},
          function() {return parent.parent.fireDialogButtonEvent(btnIndex, dlgId)},
          function() {return parent.parent.parent.fireDialogButtonEvent(btnIndex, dlgId)}
        );
    }
});

TS.namespace("TS.Msg");
TS.apply(TS.Msg,{
    /**
     * 按钮类型常数，与Ext中的对应
     */
    Buttons: {
        OK: {ok:true},
        CANCEL: {cancel:true},
        OKCANCEL: {ok:true, cancel:true},
        YES: {yes:true},
        YESNO: {yes:true, no:true},
        YESNOCANCEL: {yes:true, no:true, cancel:true}
    },
    
    /**
     * 图标类型常数
     */
    IconCls: {
        INFO: 'egd-msgBox-info',
        QUESTION: 'egd-msgBox-question',
        WARNING: 'egd-msgBox-warning',
        ERROR: 'egd-msgBox-error',
        
        CREATING: 'egd-msgBox-wait-creating',
        SAVIND: 'egd-msgBox-wait-saving',
        DELETING: 'egd-msgBox-wait-deleting',
        EDITING: 'egd-msgBox-wait-editing',
        OPENING: 'egd-msgBox-wait-opening',
        LOADING: 'egd-msgBox-wait-loading'
    },
   
    /**
     * 通用信息框
     *
     * @param {Object} config 
     * @config {String} title 窗口的标题
     * @config {String} msg 提示信息
     * @config {Number} buttons 可选配置：按钮配置，参考TS.Msg.Buttons常数，默认为TS.Msg.Buttons.OK
     * @config {String} iconCls 可选配置，参考TS.Msg.IconCls常数，默认为TS.Msg.IconCls.INFO
     * @config {Function} onOk 可选配置：点击ok按钮的回调函数，只有当buttons中包含ok按钮时有效
     * @config {Function} onCancel 可选配置：点击cancel按钮的回调函数，只有当buttons中包含cancel按钮时有效
     * @config {Function} onYes 可选配置：点击yes按钮的回调函数，只有当buttons中包含yes按钮时有效
     * @config {Function} onNo 可选配置：点击no按钮的回调函数，只有当buttons中包含no按钮时有效
     * @config {Function} onClose 可选配置：信息框关闭后的回调函数，函数第一个参数为按钮的id，即ok|cancel|yes|no
     */
    msgBox: function(config){
        Try.these(
          function() {msgBox(config)},
          function() {parent.msgBox(config)},
          function() {parent.parent.msgBox(config)},
          function() {parent.parent.parent.msgBox(config)}
        );
    },

    /**
     * 确认信息框
     *
     * @param {Object} config 
     * @config {String} title 窗口的标题
     * @config {String} msg 提示信息
     * @config {Function} onYes 可选配置：点击yes按钮的回调函数
     * @config {Function} onNo 可选配置：点击no按钮或关闭按钮的回调函数
     * @config {Function} onClose 可选配置：信息框关闭后的回调函数，函数第一个参数为按钮的id，即yes|no
     */
    confirm: function(config){
        Try.these(
          function() {parent.confirmBox(config)},
          function() {parent.parent.confirmBox(config)},
          function() {parent.parent.parent.confirmBox(config)}
        );
    },

    /**
     * 警告信息框
     *
     * @param {Object} config 
     * @config {String} title 窗口的标题
     * @config {String} msg 提示信息
     * @config {Function} onOk 可选配置：点击ok按钮的回调函数
     * @config {Function} onClose 可选配置：信息框关闭后的回调函数，函数第一个参数为按钮的id，即ok|cancel
     */
    alert: function(config){
        Try.these(
          function() {parent.alarm(config)},
          function() {parent.parent.alarm(config)},
          function() {parent.parent.parent.alarm(config)}
        );
    },

    /**
     * 获取输入信息的信息框
     *
     * @param {Object} config 
     * @config {String} title 窗口的标题
     * @config {String} msg 提示信息
     * @config {Function} onOk 可选配置：点击ok按钮的回调函数
     * @config {Function} onCancel 可选配置：点击cancel按钮或关闭按钮的回调函数
     * @config {Function} onClose 可选配置：信息框关闭后的回调函数，函数第一个参数为按钮的id，即ok|cancel
     * @config {Boolean} multiline 可选配置：是否为多行提示信息，默认为true
     */
    prompt: function(config) {
        Try.these(
          function() {parent.prompt(config)},
          function() {parent.parent.prompt(config)},
          function() {parent.parent.parent.prompt(config)}
        );
    },

    /**
     * 等待信息框
     *
     * @param {Object} config 
     * @config {String} msg 提示信息
     * @config {String} progressText 可选配置：动画的背景文字
     * @config {String} icon 可选配置：图片的样式名，默认为egd-msgBox-wait
     * @config {Number} interval 可选配置：动画的时间间隔，以毫秒为单位，默认为200
     * @config {Number} closeTime 可选配置：自动隐藏的等待时间，以毫秒为单位
     */
    wait: function(config) {
        Try.these(
          function() {parent.wait(config)},
          function() {parent.parent.wait(config)},
          function() {parent.parent.parent.wait(config)}
        );
    },

    /**
     * 隐藏信息框
     */
    hide: function(config) {
        Try.these(
          function() {parent.hideMsg(config)},
          function() {parent.parent.hideMsg(config)},
          function() {parent.parent.parent.hideMsg(config)}
        );
    },

    /**
     * 判断指定的html控件集是否其值全部为空
     * @param {Array} ids 要判断的html控件的id数组
     * @param {Array} names 要判断的html控件的名称数组，索引与ids相互对应
     * @return {Boolean} 如果全部控件的值都为空则返回true，否则返回false
     */
    isEmptyValue: function(ids, names) {
        if(typeof ids == "string"){
            ids = ids.split(";");	
            names = names.split(";");	
        }
        
        for(var i = 0; i < ids.length; i++){
            if($F(ids[i]).trim() == ""){
                var msg = "必须填写“" + names[i] + "”的值！";
	            //MsgBox(msg);
	            //TS.Msg.alert({msg:msg});
    			TS.Msg.msgBox({msg:msg, iconCls: TS.Msg.IconCls.WARNING});
	            return true;
            }
        }
        return false;
    },

    /**
     * 判断指定的html控件的值是否为数字类型
     * @param {Array} ids 要判断的html控件的id数组
     * @param {Array} names 要判断的html控件的名称数组，索引与ids相互对应
     * @return {Boolean} 如果全部控件的值都为数字类型则返回true，否则返回false并提示用户
     */
    isNumber: function(ids, names) {
        if(typeof ids == "string"){
            ids = ids.split(";");	
            names = names.split(";");	
        }
        
        for(var i = 0; i < ids.length; i++){
					var _id = ids[i];
					var obj = $(_id);
					if(obj == null)
						continue;
					var _value = $F(_id);
					if(_value == "" ){
						$(_id).value = "0" ;	// 将空值改为数字0
					}else {
						if(_value.split(".").length > 2 ){
							TS.Msg.msgBox({msg:"“" + names[i] + "”的值必须为数值类型", iconCls: TS.Msg.IconCls.WARNING});
							return false;		
						}
						for(var j = 0 ; j < _value.length ; j ++ ){
							var temp = _value.substring(j , j + 1 );
							if(temp != "." && isNaN(parseInt(temp))){
								TS.Msg.msgBox({msg:"“" + names[i] + "”的值必须为数值类型", iconCls: TS.Msg.IconCls.WARNING});
								return false;
							}
						}
					}
        }
        return true;
    }
});

/**
 *解析Xml,根据元素、节点名称获得节点的值
 * @param {Array} element  要解析的元素
 * @param {String} tagName 要解析的节点名称
 */
function getElementContent(element, tagName){
    var childElement = element.getElementsByTagName(tagName)[0];
    return childElement.text != undefined ? childElement.text : childElement.textContent;
}

TS.namespace("TS.Calendar");
/* 主页的全局日历适配器 */
TS.apply(TS.Calendar,{
    /* 
     * 自动根据样式绑定日历的选择,样式的设置值为"egd-dateField"或"egd-dateTimeField"
     * @param {Array} config 要绑定的控件的配置数组，数组中的元素为item
     * @item {String} fieldId 要绑定的html控件的id
     * @item {String} iframeIdInParent 可选：当前窗口在父页面中的iframe的id，
     *      如果忽略表示页面为标准的多窗口内的页面，如果页面为窗口内的页面内的iframe内的页面，则必须提供该参数
     * @item {String} dlgId 可选：对话框的id，如果指定该参数将忽略iframeIdInParent参数，当在弹出对话框中使用时，必须指定该值
     * @item {Function} onOk 选择日期/时间信息后的回调函数，第一个参数为控件的id，第二个参数为日期/时间的值
     * @item {Function} onClear 点击“清空”按钮后的回调函数，第一个参数为控件的id
     * @item {Function} onCancel 点击“关闭”按钮后的回调函数，第一个参数为控件的id
     * @param {String} iframeIdInParent 可选：全局配置当前窗口在父页面中的iframe的id，
     *      如果指定该参数将覆盖item.iframeIdInParent参数的配置
     * @param {String} dlgId 可选：全局配置对话框的id，
     *      如果指定该参数将覆盖item.dlgId参数的配置
     */
	bind:function(config, iframeIdInParent, dlgId){
	    if(!(config instanceof Array)){
	        alert("TS.Calendar.bind方法的参数类型必须为数组！in TS.Calendar.bind");
	        return;
	    }
	    var _config=[];
	    var t;
		config.each(function(field){
	        if(typeof field == 'string'){
	            t = {fieldId: field};
	        }else if(typeof field == 'object'){
	            t = field;
	        }else{
	            alert("TS.Calendar.bind方法的config参数配置错误！in TS.Calendar.bind");
	            return;
	        }
            if(iframeIdInParent) t.iframeIdInParent = iframeIdInParent;
            if(dlgId) t.dlgId = dlgId;
            
	        if(iframeIdInParent && dlgId){
	            alert("TS.Calendar.bind方法的config参数配置错误,不支持同时配置iframeIdInParent和dlgId参数！fieldId=" + t.fieldId);
	            return;
	        }
            
            _config.push(t);
		});
	    
		_config.each(function(fieldConfig){
		    var element = $(fieldConfig.fieldId);
		    if(!element){
	            alert("id=" + fieldConfig.fieldId + "的要绑定日历选择的html控件不存在！in TS.Calendar.bind");
	            return;
		    }
    	    
    	    // 只处理包含样式"egd-dateTimeField"或"egd-dateField"的控件
		    if(element.hasClassName("egd-dateTimeField") || element.hasClassName("egd-dateField")){
		        // 侦听控件的鼠标点击事件
			    element.observe('mousedown',function(e){
			        var page = Position.page(element);// 获取控件的X(page[0])、Y(page[1])位置
			        var fieldSize = element.getDimensions();
	                TS.apply(fieldConfig,{
				        deep: parent ? (parent.parent ? 2 : 1) : 0, // 只支持0(主页面)、1(主页的子页面)、2(主页的子页面的子页面)三种深度
				        className: element.className,               // 样式名
				        curValue: element.value,                    // 当前的值
				        left: page[0],
				        top: page[1],
				        width: fieldSize.width,
				        height: fieldSize.height,
				        innerCallback: TS.Calendar.innerCallback
				    });
				    var format = element.readAttribute("format");   // 日期的显示格式
				    if(format && format.length > 0)
				        fieldConfig.format = format;
				        
				    // 如果是窗口内部的iframe，如表单中的页签中的iframe
				    //if(fieldConfig.dlgId) config.dlgId = dlgId;
				    if(fieldConfig.iframeIdInParent){
				        var ppage = parent.TS.Calendar.getIFramePage(fieldConfig.iframeIdInParent);    // 获取当前窗口在父页面的X(page[0])、Y(page[1])位置
                        fieldConfig.left += ppage[0];
                        fieldConfig.top += ppage[1];
				        parent.parent.showEgdCalendar(fieldConfig);
				    }else{
				        parent.showEgdCalendar(fieldConfig);
				    }
				        
			    },true);						
		    }else{
		        if(fieldConfig.debug == true)
	                alert('id=' + fieldConfig.fieldId + '的html控件不存在样式"egd-dateTimeField"或"egd-dateField"!');
		    }	
		});			
	},
	
    /* 
     * private
     * 在主页上选择日期/时间信息后的回调函数，用于设置本页面相应控件的值。
     * @param {String} fieldId html控件的id
     * @param {String} value 要设置的值
     */
	innerCallback:function(fieldId, value){
		var element = $(fieldId);
		if(!element){
	        alert("id=" + fieldId + "的要绑定日历选择的html控件不存在！in TS.Calendar.onOk");
	        return;
		}
		if(element.value != value)
		    element.value = value;
		//alert('TS.Calendar.onOk');
	},
	
    /* 
     * private
     * 获取iframe的X(page[0])、Y(page[1])位置
     * @param {String} iframeId iframe的id的值
     */
	getIFramePage:function(iframeId){
        //alert("iframeId=" + iframeId + ";" + $(iframeId));
		return Position.page($(iframeId));
	}
});
if(typeof isEgdIndexPage == "undefined"){
    Event.observe(document,'click', function(e){
	    var element = Event.element(e);
	    //alert(element.hasClassName);
        if(!element.hasClassName || (!element.hasClassName("egd-dateTimeField") && !element.hasClassName("egd-dateField"))){
            Try.these(
              function() {parent.hideEgdCalendar()},
              function() {parent.parent.hideEgdCalendar()}
            );
	    }				
    });
}