/**
 * 页签组建
 * 
 * dragon 2007-06-21
 * 
 * 需引用Prototype.js
 */
 
if (typeof TS == "undefined") TS = {};

/* 
 * 创建页签
 * @param container 页签容器的html元素id值
 * @param config 所要创建的页签的配置数组[{id:"",text:"",disabled:false|true,handler:yourFunction},...]
 */
TS.TabPanel = function(container,config){
	var startEl,labelEl,closeEl,endEl,startClass,labelClass,closeClass,endClass,headerEl,tab;
	var len = (config ? config.length : 0);
	
	// 页签的容器
	this.el = $(container);
	//alert(container + ";" + $(container));
	
	// 创建所有页签头的容器
	this.headersEl = document.createElement("div");
	this.headersEl.className = "egd-tab-headers";
	this.el.insertBefore(this.headersEl,this.el.firstChild);
	
	// 为每个页签创建页签头
	this.tabs = [];
	this.activeTab = null;
	for(var i = 0;i < len;i++ ){
		// 创建单个页签头的容器
		headerEl = document.createElement("div");
		headerEl.className = "egd-tab-header" + (i == 0 ? "" : " egd-tab-cross");
		headerEl.index = i;
		headerEl.style.zIndex = len - i + 99;
		
		// 确定样式(第一个页签为激活的页签)
		startClass = "egd-tab-header-start";
		startClass += (i == 0 ? "-active" : (config[i].disabled == true ? "-disabled" : "-inactive"));
		labelClass = "egd-tab-header-label";
		labelClass += (i == 0 ? "-active" : (config[i].disabled == true ? "-disabled" : "-inactive"));
		closeClass = "egd-tab-header-close";
		closeClass += (i == 0 ? "-active" : (config[i].disabled == true ? "-disabled" : "-inactive"));
		endClass = "egd-tab-header-end";
		endClass += (i == 0 ? "-active" : (config[i].disabled == true ? "-disabled" : "-inactive"));
		
		// 创建HtmlElement
		startEl = document.createElement("div");
		startEl.className = startClass;
		startEl = headerEl.appendChild(startEl);
		labelEl = document.createElement("div");
		labelEl.className = labelClass;
		labelEl.innerHTML = config[i].text;
		labelEl = headerEl.appendChild(labelEl);
		closeEl = document.createElement("div");
		closeEl.className = closeClass;
		closeEl = headerEl.appendChild(closeEl);
		endEl = document.createElement("div");
		endEl.className = endClass;
		endEl = headerEl.appendChild(endEl);
		
		this.headersEl.appendChild(headerEl);

		// 创建页签对象
		tab = {
			header:{el:headerEl,startEl:startEl,labelEl:labelEl,closeEl:closeEl,endEl:endEl},
			content:{el:$(config[i].id)}	// 页签内容的容器
		};
		if (typeof config[i].handler == "function")
			tab.handler = config[i].handler;				// 点击页签头的回调函数
			
		if (i == 0){
			tab.active =  true;
			this.activeTab = tab;
		}else{
			tab.active =  false;
		}
		tab.disabled = (config[i].disabled == true);
		this.tabs[i] = tab;
		
		// 控制对应的内容容器的显示
		if (i == 0){
			if (tab.content.el.className != "egd-tab-content"){
				tab.content.el.className = "egd-tab-content";
			}
		}else{
			if (tab.content.el.className != "egd-tab-hidden"){
				tab.content.el.className = "egd-tab-hidden";
			}
		}
		
		// 点击页签头的处理
		var oThis = this;
		if (config[i].disabled != true){
			labelEl.onclick = function(e){
				// 获取激发事件的html元素对象
				// IE6----window.event.srcElement
				// FireFox,Opera----所存入的第一个参数:e.target(MouseEvent.target)
				var target = (window.event ? window.event.srcElement : e.target);	
				//alert("headerEl");
				
				oThis.clickedTab.call(oThis,target.parentNode.index);
			}
		}
	}
};

/**
 * TS.TabPanel的原型定义
 */
TS.TabPanel.prototype = {
	/* 
	 * 点击页签的处理
	 * private
	 * @param index 所点击的页签的索引号,从0开始
	 */
	clickedTab: function(index){
		// 如果点击的是当前激活的页签则不作处理
		var curTab = this.tabs[index];
		if (curTab.active == true) return;

		var startEl,labelEl,closeEl,endEl,startClass,labelClass,closeClass,endClass;
		var headerEl;
		var len = this.tabs.length;
		var preTab = this.activeTab;
		
		// 设置点中的页签为激活状态
		curTab.active = true;
		curTab.header.startEl.className = "egd-tab-header-start-active";
		curTab.header.labelEl.className = "egd-tab-header-label-active";
		curTab.header.closeEl.className = "egd-tab-header-close-active";
		curTab.header.endEl.className = "egd-tab-header-end-active";
		curTab.content.el.className = "egd-tab-content";
		this.activeTab = curTab;
		
		// 设置前一激活的页签为非激活状态
		preTab.active = false;
		preTab.header.startEl.className = "egd-tab-header-start" + (preTab.disabled == true ? "-disabled" : "-inactive");
		preTab.header.labelEl.className = "egd-tab-header-label" + (preTab.disabled == true ? "-disabled" : "-inactive");
		preTab.header.closeEl.className = "egd-tab-header-close" + (preTab.disabled == true ? "-disabled" : "-inactive");
		preTab.header.endEl.className = "egd-tab-header-end" + (preTab.disabled == true ? "-disabled" : "-inactive");
		preTab.content.el.className = "egd-tab-hidden";
		
		// 重新设置zIndex值
		var preTabIndex = preTab.header.index;
		for(var i = 0;i < len;i++ ){
			if(i != index)
				this.tabs[i].header.el.style.zIndex = len - i + 99;
			else{
				this.tabs[i].header.el.style.zIndex = len + 1000;
			}
		}
		
		// 调用回调函数
		if (curTab.handler)
			curTab.handler.call(this,curTab);
	},
	
	/* 
	 * 添加一个页签 
	 * @param config 页签的配置{id:"",text:"",disabled:false|true,handler:yourFunction}
	 */
	addTab: function(config){
		if(!config) return;
		
		// 检测是否存在同样id的页签
		for(var i = 0 ;i < this.tabs.length;i++){
			if (this.tabs[i].content.el.id == config.id){
				alert("id=" + config.id + "的页签已经存在!");
				return;
			}
		}

		var startEl,labelEl,closeEl,endEl;
		
		// 创建单个页签头的容器
		var headerEl = document.createElement("div");
		headerEl.className = "egd-tab-header" + (this.tabs.length == 0 ? "" : " egd-tab-cross");
		headerEl.index = this.tabs.length;
		headerEl.style.zIndex = (this.tabs.length == 0 ? 100 : this.tabs[this.tabs.length - 1].header.el.style.zIndex - 1);
		
		// 创建HtmlElement
		startEl = document.createElement("div");
		startEl.className = "egd-tab-header-start" + (config.disabled == true ? "-disabled" : "-inactive");
		startEl = headerEl.appendChild(startEl);
		labelEl = document.createElement("div");
		labelEl.className = "egd-tab-header-ladel" + (config.disabled == true ? "-disabled" : "-inactive");;
		labelEl.innerHTML = config.text;
		labelEl = headerEl.appendChild(labelEl);
		closeEl = document.createElement("div");
		closeEl.className = "egd-tab-header-close" + (config.disabled == true ? "-disabled" : "-inactive");;
		closeEl = headerEl.appendChild(closeEl);
		endEl = document.createElement("div");
		endEl.className = "egd-tab-header-end" + (config.disabled == true ? "-disabled" : "-inactive");;
		endEl = headerEl.appendChild(endEl);
		
		this.headersEl.appendChild(headerEl);

		// 创建页签对象
		var tab = {
			header:{el:headerEl,startEl:startEl,labelEl:labelEl,closeEl:closeEl,endEl:endEl},
			content:{el:$(config.id)},	// 页签内容的容器
			disabled:(config.disabled == true),
			active:false
		};
		if (typeof config.handler == "function")
			tab.handler = config.handler;				// 点击页签头的回调函数

		tab.content.el.className = "egd-tab-hidden";
		var oThis = this;
		tab.header.labelEl.onclick = function(e){
			// 获取激发事件的html元素对象
			// IE6----window.event.srcElement
			// FireFox,Opera----所存入的第一个参数:e.target(MouseEvent.target)
			var target = (window.event ? window.event.srcElement : e.target);	
			oThis.clickedTab.call(oThis,target.parentNode.index);
		}
		this.tabs[this.tabs.length] = tab;
		
		// 激活该页签
		if (config.disabled != true)
			this.active(this.tabs.length - 1);
	},
	
	/* 
	 * 激活一个页签
	 * @param index 页签的索引号
	 */
	active: function(index){
		if (this.tabs[index].disabled == true){
			alert("index=" + index + "的页签处于禁用状态,不能激活!");
			return;
		}else{
			this.clickedTab(index);
		}
	},
	
	/* 
	 * 禁用一个页签
	 * @param index 页签的索引号
	 */
	disabled: function(index){
		if (this.tabs[index].disabled == true){
			return;
		}else{
			var tab = this.tabs[index];
			tab.disabled = true;
			tab.active = false;
			tab.header.startEl.className = "egd-tab-header-start-disabled";
			tab.header.labelEl.className = "egd-tab-header-label-disabled";
			tab.header.closeEl.className = "egd-tab-header-close-disabled";
			tab.header.endEl.className = "egd-tab-header-end-disabled";
			tab.content.el.className = "egd-tab-hidden";
			if (this.activeTab.index == index)
				this.activeTab = null;
			tab.header.labelEl.onclick = null;
		}
	},
	
	/* 
	 * 解禁一个页签
	 * @param index 页签的索引号
	 */
	enabled: function(index){
		if (this.tabs[index].disabled != true){
			return;
		}else{
			var tab = this.tabs[index];
			tab.disabled = false;
			tab.active = false;
			tab.header.startEl.className = "egd-tab-header-start-inactive";
			tab.header.labelEl.className = "egd-tab-header-label-inactive";
			tab.header.closeEl.className = "egd-tab-header-close-inactive";
			tab.header.endEl.className = "egd-tab-header-end-inactive";
			//tab.content.el.className = "egd-tab-content";
			if (this.activeTab.index == index)
				this.activeTab = null;
			var oThis = this;
			tab.header.labelEl.onclick = function(e){
				// 获取激发事件的html元素对象
				// IE6----window.event.srcElement
				// FireFox,Opera----所存入的第一个参数:e.target(MouseEvent.target)
				var target = (window.event ? window.event.srcElement : e.target);	
				oThis.clickedTab.call(oThis,target.parentNode.index);
			}
		}
	},
	
	noUse:true
};
