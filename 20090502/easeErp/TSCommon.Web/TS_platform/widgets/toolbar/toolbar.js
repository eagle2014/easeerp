/**
 * 工具条组件
 * 
 * dragon 2007-08-31 
 * 
 * 引用Prototype.js,Egd.js
 */
 
if (typeof TS == "undefined") TS = {};
/* 
 * 工具条使用范例:<pre><code>
 * // 构造工具条
 * var myTB = new Egd.TB("tbContainerId",[
 * ]);
 * </code></pre>
 */
TS.TB = Class.create();
TS.TB.prototype={
	/* 
	 * 构造函数
	 * @param {String} container 工具条容器的id
	 * @param {Array} buttons 按钮数组
	 * @param {Object} cssTpl 样式模板,默认为Egd.TB.DEFAULT_CSSTpl
	 */
	initialize:function(container,buttons,cssTpl){
	    if(!cssTpl) cssTpl = TS.TB.DEFAULT_CSSTpl;// 如果没配置则使用默认样式
		this.CSSTpl = cssTpl;
		
		//alert(this.CSSTpl.container);
		// 单窗口主页时自动添加“返回”按钮
		if(!TS.isMDI() && cssTpl==TS.TB.DEFAULT_CSSTpl){
		    var btnDefaultBack = {
			    id:"btnBack",
			    text:"返回",
			    iconClass:"egd-button-back",
			    handler:function(button){
				    TS.closeMe();
			    }
		    };
            buttons.unshift(btnDefaultBack);
		}
	    
		this.el = $(container);
		this.buttons = [];
		for(var i = 0;i < buttons.length;i++){
			this.addButton(buttons[i],true);
		}
		this.render();
	},
	
	addButton:function(config,unRender){
        if(config instanceof Array){
            for(var i = 0, len = config.length; i < len; i++) {
                this.addButton(TS.apply(config[i],{tb: this}),unRender);// 设置按钮所在的工具条对象，用于统一样式控制(container.CSSTpl)
            }
        }else{
	        var btn = config;
	        if(!(config instanceof TS.TB.Button)){
	            btn = new TS.TB.Button(TS.apply(config,{tb: this}));
	        }else{
	            btn.config.tb = this;
	        }
	        this.buttons.push(btn);
			if(!(unRender == true)) this.renderButton(btn);
        }
	},
	render:function(){
		for(var i = 0;i < this.buttons.length;i++){
			this.renderButton(this.buttons[i]);
		}
	},
	renderButton:function(button){
		this.el.appendChild(button.el);
	},
	
	getButton:function(buttonId){
		for(var i = 0;i < this.buttons.length;i++){
			if(this.buttons[i].el.id == buttonId)
				return this.buttons[i];
		}
		return null;
	},
	
	disable:function(buttonIds){
		var button;
		for(var i = 0;i < buttonIds.length;i++){
			button = this.getButton(buttonIds[i]);
			if(button != null) button.disable();
		}
	},
	
	enable:function(buttonIds){
		var button;
		for(var i = 0;i < buttonIds.length;i++){
			button = this.getButton(buttonIds[i]);
			if(button != null) button.enable();
		}
	},
	
	show:function(buttonIds){
		var button;
		for(var i = 0;i < buttonIds.length;i++){
			button = this.getButton(buttonIds[i]);
			if(button != null) button.show();
		}
	},
	
	hide:function(buttonIds){
		var button;
		for(var i = 0;i < buttonIds.length;i++){
			button = this.getButton(buttonIds[i]);
			if(button != null) button.hide();
		}
	}
};

/* 默认的工具条按钮样式模板 */
TS.TB.DEFAULT_CSSTpl = {
    container : "egd-button-container",         // 按钮容器
    left : "egd-button-left",                   // 按钮左侧区域
    right : "egd-button-right",                 // 按钮右侧区域
    center : "egd-button-center",               // 按钮文字和图标的容器
    centerIcon : "egd-button-center-icon",      // 按钮图标
    centerText : "egd-button-center-text",      // 按钮文字
    centerArrow : "egd-button-center-arrow",    // 下拉框的箭头图标
    separator : "egd-button-separator"          // 分割条
};

/* 页签中的工具条按钮样式模板 */
TS.TB.TAB_CSSTpl = {
    container : "egd-tabButton-container",         // 按钮容器
    left : "egd-tabButton-left",                   // 按钮左侧区域
    right : "egd-tabButton-right",                 // 按钮右侧区域
    center : "egd-tabButton-center",               // 按钮文字和图标的容器
    centerIcon : "egd-tabButton-center-icon",      // 按钮图标
    centerText : "egd-tabButton-center-text",      // 按钮文字
    centerArrow : "egd-tabButton-center-arrow",    // 下拉框的箭头图标
    separator : "egd-tabButton-separator"          // 分割条
};

/* 对话框中的工具条按钮样式模板 */
TS.TB.DLG_CSSTpl = {
    container : "egd-dlgButton-container",         // 按钮容器
    left : "egd-dlgButton-left",                   // 按钮左侧区域
    right : "egd-dlgButton-right",                 // 按钮右侧区域
    center : "egd-dlgButton-center",               // 按钮文字和图标的容器
    centerIcon : "egd-dlgButton-center-icon",      // 按钮图标
    centerText : "egd-dlgButton-center-text",      // 按钮文字
    centerArrow : "egd-dlgButton-center-arrow",    // 下拉框的箭头图标
    separator : "egd-dlgButton-separator"          // 分割条
};

/* 视图分页条的工具条按钮样式模板 */
TS.TB.PAGE_CSSTpl = {
    container : "egd-pageButton-container",         // 按钮容器
    left : "egd-pageButton-left",                   // 按钮左侧区域
    right : "egd-pageButton-right",                 // 按钮右侧区域
    center : "egd-pageButton-center",               // 按钮文字和图标的容器
    centerIcon : "egd-pageButton-center-icon",      // 按钮图标
    centerText : "egd-pageButton-center-text",      // 按钮文字
    centerArrow : "egd-pageButton-center-arrow",    // 下拉框的箭头图标
    separator : "egd-pageButton-separator"          // 分割条
};

/* 创建工具条的分隔条 */
TS.TB.createSeparator = function(){
	var el = document.createElement("div");
	el.className = TS.TB.DEFAULT_CSSTpl.separator;
    return el;
};

/* 工具条按钮类的定义 */
TS.TB.Button = Class.create();
TS.TB.Button.prototype={
	/* 
	 * 默认配置
	 */
	defaultConfig: {
		iconClass:"egd-button-default",
		hidden:false,
		showText:true,
		showIcon:true
	},
	
	/* 
	 * 构造函数
	 * @param {Object} config 配置
	 * @config {String} id 按钮id
	 * @config {String} text 按钮显示的文字
	 * @config {Object} tb 按钮所在的工具条对象
	 * @config {String} iconClass 可选配置：按钮图标的样式名，默认为egd-button-default
	 * @config {Function} handler 可选配置：点击按钮的处理函数，上下文为按钮对象，第一个参数为按钮对象
	 * @config {Boolean} hidden 可选配置：是否隐藏按钮不显示，默认为false
	 * @config {Boolean} showText 可选配置：是否相识按钮文字，默认为true
	 * @config {Boolean} showIcon 可选配置：是否相识按钮图标，默认为true
	 */
	initialize:function(config){
		this.config = {};
		if (typeof config == "string"){
			this.config = config;
			this.el = TS.TB.createSeparator();
		}else{
			TS.apply(this.config,config,this.defaultConfig);
			this.el = this.createContainer(config.id);
			var _html = this.getTemplate();
			if(this.config.hidden == true){
			    this.el.style.display = "none"; // 隐藏按钮不显示
			}
			this.el.innerHTML = _html;
			this.bindEvents();
		}
	},
	
	createContainer : function(id){
		var el = document.createElement("div");
		el.className = this.config.tb.CSSTpl.container;
		if(this.config.styleFloat) el.style.styleFloat = this.config.styleFloat;
		if (id) el.id = id;
        return $(el);
    },
    	
	getTemplate: function(){		
		var tpl = '<div class="#{left}"></div>';
		tpl += '<div class="#{center}" UNSELECTABLE="on">';
		if(this.config.showIcon == true){
			tpl += '<div class="#{centerIcon}';
			if(this.config.iconClass)
				tpl += ' ' + this.config.iconClass;
			tpl += '" UNSELECTABLE="on"></div>';
		}
		if(this.config.showText == true){
			tpl += '<div class="#{centerText}" UNSELECTABLE="on">';
			tpl += this.config.text;
			tpl += '</div>';
		}
		tpl += '</div>';
		tpl += '<div class="#{right}"></div>';
  		return new Template(tpl).evaluate(this.config.tb.CSSTpl);
	},
	
	bindEvents: function(){
		if(this.config.unBindEvents == true) return;
		
		var button = this.el;
		var oThis = this;
		button.onmouseover = function(_event){
			button.className = oThis.config.tb.CSSTpl.container + ' ' + oThis.config.tb.CSSTpl.container + '-over';
		    //alert(button.className);
		};
		button.onmouseout = function(_event){
			button.className = oThis.config.tb.CSSTpl.container;
		};
		button.onmousedown = function(_event){
			button.className = oThis.config.tb.CSSTpl.container+' '+oThis.config.tb.CSSTpl.container+'-down';
		};
		button.onmouseup = function(_event){
			button.className = oThis.config.tb.CSSTpl.container+' '+oThis.config.tb.CSSTpl.container+'-over';
		};
		button.onclick = function(_event){
	 		if(typeof oThis.config.handler == "function")
	 			oThis.config.handler.call(oThis,oThis);
		};
	},
	
	unBindEvents: function(){
		var button = this.el;
		button.onmouseover = null;
		button.onmouseout = null;
		button.onmousedown = null;
		button.onmouseup = null;
		button.onclick = null;
	},
	
	/*
	 * 设置按钮的状态
	 * @param button 按钮对象
	 * @param status 要设置的状态值("normal"--常规,"over"--悬停,"down"--按下,"active"--激活,"disabled"--禁用)
	 */
	setStatus: function(status){
		this.el.className = this.config.tb.CSSTpl.container + ' ' + this.config.tb.CSSTpl.container + '-' + status;
	},
		
	disable:function(){
	 	if(!(this.config.disabled == true)){
		 	this.unBindEvents();
			this.el.className = this.config.tb.CSSTpl.container+' '+this.config.tb.CSSTpl.container+'-disabled';
		 	this.config.disabled = true;
		 	//alert(this.config.text);
	 	}
	},
	
	enable:function(){
	 	if(this.config.disabled == true){
		 	this.bindEvents();
			this.el.className = this.config.tb.CSSTpl.container;
		 	this.config.disabled = false;
		 	//alert(this.config.text);
	 	}
	},
	
	show:function(){
	 	if(this.config.hidden == true){
		 	this.el.style.display = "";
		 	this.config.hidden = false;
	 	}
	},
	
	hide:function(){
	 	if(!(this.config.hidden == true)){
		 	this.el.style.display = "none";
		 	this.config.hidden = true;
	 	}
	}
};
