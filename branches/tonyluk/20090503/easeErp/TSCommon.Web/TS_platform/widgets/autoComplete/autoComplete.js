/**
 * 自动完成组件
 * 
 * dragon 2007-10-30
 * 引用:Egd.js,prototype.js 
 * 
 */
 
/* 自动完成组件构造函数
 * @param {Object} config 配置项
 * @config {String} field 要填充的域的id
 * @config {String|Array} data 获取条目数据的url或直接包含行数据的数组
 *      如果为url，该url请求返回的数据必须为数组,数组格式如下：
 *      [{id:"id1", text:"text1"},{id:"id2", text:"text2"},...]，数组内的每个项的属性可由程序灵活控制，
 *      如可以为整个domain的所有属性，具体界面上显示的是那个属性的值由mapping属性决定
 * 
 * @config {Boolean} hightLight 可选配置：是否高亮显示匹配的文本,默认为true
 * @config {Number} minLen 可选配置：起始匹配长度，默认为1
 * @config {Number} height 可选配置：容器的高度，默认为100
 * @config {Function} afterSelected 可选配置：选中列表中的值后的回调函数，上下文为当前实例，第一个参数为选中行对应的json对象
 * @config {Object} defaultSort 可选配置：默认排序字段的配置
 * @defaultSort {String} name 默认排序的字段名
 * @defaultSort {direction} direction 默认排序的方向("asc" | "desc"),默认为"asc"
 * @config {String} mapping 可选参数：值的映射配置，默认为"text"
 * @config {String} tpl 可选参数：列表中显示的条目的文本的格式化模板，
 *      默认为"#{mappingName}"，其中mappingName为参数mapping的值，格式参考Prototype的Template类；
 *      其中#{text}中的text可以为url返回的数组中的项的任意属性名
 * @config {Boolean} addTimeStamp 可选参数：是否在url后添加时间挫，默认为true
 * @config {Function} loadException 可选参数：加载数据出现异常时调用的可选函数，上下文为当前Egd.AutoComplete
 *      的实例，第一个参数为服务器返回的transport对象，第二个参数为自动完成的基础参数(即field控件的当前值)
 * @config {String} method 可选参数：ajax提交请求时使用的方法'get'或'post',默认为'post'
 * @config {Object} parameters 可选参数：ajax提交请求时附加使用的可选参数，必须为json格式，不要使用字符串参数格式
 * 
 * @return {Object} 自动完成组件的实例
 */
Egd.AutoComplete = Class.create();
Egd.AutoComplete.prototype = {
	/* 颜色定义 */
	color: {
	    bg : "white",
	    font: "black",
	    bg_selected: "#3366CC",
	    font_selected: "#FFFFFF"
	},
	
	/* 默认配置容器的控件ID */
	containerID: "egd_autoComplete_container",
	
	/* 
	 * 构造函数
	 * @param {Object} _config 配置参数
	 */
	initialize:function(_config){
	    this.config = {};
 	    this.config = Egd.apply(this.config,_config,this.defaultConfig);
 	    if(this.config.defaultSort){
 	        if(!this.config.defaultSort.name){
     	        this.config.defaultSort.name = "";
 	        }
 	        if(!this.config.defaultSort.direction){
     	        this.config.defaultSort.direction = "asc";
 	        }
 	    }else{
 	        this.config.defaultSort = {name: "", direction: "asc"};
 	    }
 	    if(this.config.minLen < 1) this.config.minLen = 1;
 	    if(!this.config.tpl) 
 	        this.config.tpl = '<div class="egd-autoComplete-item">#{' + this.config.mapping + '}</div>';
 	    else
 	        this.config.tpl = '<div class="egd-autoComplete-item">' + this.config.tpl + '</div>';
 	    this.sort = this.config.defaultSort.name;
 	    this.dir = this.config.defaultSort.direction;
    	
 	    this.init();
 	    this.hidden = true;
 	    
 	    this.msg = $("msg");// 调试用
	},
	
	/* 默认配置 */
 	defaultConfig : {
 		method: "post",
 		mapping: "text",
 		hightLight:true,        // 是否高亮显示匹配的文本
 		minLen: 1,              // 起始匹配长度
 		height: 85,
 		addTimeStamp: true,
 		pageNo: 1,
 		pageSize: 10
 	},
	
	/* 创建包含列表值的容器 */
	createContainer : function(){
	    if(!Egd.AutoComplete.container){
	        var container = document.createElement("div");
	        container.id = Egd.AutoComplete.containerID;
	        container.className = "egd-autoComplete-container";
	        container.style.display = "none";
            document.body.appendChild(container);
            Egd.AutoComplete.container = container;// singleton
        }
	},
 	
	/* 初始化 */
 	init:function(){
 	    // 创建容器
 	    this.createContainer();
 	    
 		if (!this.config.field) {
 			alert("错误：Egd.AutoComplete的构造函数参数中没有配置field的值!");
 			return;
 		}
 		if (!this.config.data) {
 			alert("错误：Egd.AutoComplete的构造函数参数中没有配置data的值!");
 			return;
 		}
 		
 		this.field = $(this.config.field);
 		
 		// 侦听事件
 		this.bindFieldEvents();
 	},
	
	/* debug */
	displayDebugInfo: function(msg){
        if(this.msg) this.msg.innerHTML = msg;
	},
	
	/* 清空列表 */
	clear: function(){
        Egd.AutoComplete.container.innerHTML += "";
        this.data = [];
        this.selectIndex = -1;
	},
	
	/* 隐藏列表 */
	hide: function(){
	    Egd.AutoComplete.container.style.display = "none";
	    this.hidden = true;
	},
	
	/* 显示列表 */
	show: function(){
		// 先清空数据
		this.clear();
		
        if(this.field.value.length < this.config.minLen){
            return;
        }
            
		// 显示数据
		var cdata = this.config.data;
		var start = (this.config.pageNo - 1) * this.config.pageSize;// 起始数
		var end = start + this.config.pageSize;// 结束数
		if(cdata instanceof Array){// 从内存中加载数据
			this.data = cdata;
			this.showDataInner.call(this,cdata);
		}else{// 通过url加载数据
			return this.loadData(start,this.config.pageSize,this.sort,this.dir,this.showDataInner);
		}
	},
	
	/* 
	 * 显示列表数据
	 * private
	 */
	showDataInner: function(data){
	    if (data.length == 0) return;
	    
		var content = "",temp;
		var curMatchIndex = -1;
		for(var i = 0;i < data.length ;i++){
		    temp = this.getRowHtml(data[i]);
            if (curMatchIndex == -1 && temp.indexOf(this.field.value) > -1)
                curMatchIndex = i;
            content += temp;
		}
		if (this.config.hightLight)
		    content = content.replace(new RegExp(this.field.value,'g'), '<span class="egd-autoComplete-matchRegion">' + this.field.value + '</span>');

		//this.displayDebugInfo("in showDataInner:content1=" + content);
		Egd.AutoComplete.container.innerHTML = content;
		
		//var xy = Egd.getXY(this.field);
		var xy = Position.page(this.field);
		Egd.AutoComplete.container.style.left = xy[0] + 1;
		Egd.AutoComplete.container.style.top = xy[1] + this.field.offsetHeight + 2;
		Egd.AutoComplete.container.style.height = this.config.height;
		Egd.AutoComplete.container.style.width = this.field.offsetWidth - 1;
	    Egd.AutoComplete.container.style.display = "";
		this.hidden = false;
		
		// 选中首个匹配的项
		this.moveTo(-1,curMatchIndex);
	},
	
	/* 
	 * 获取经格式化处理后的行代码
	 * private
	 */
	getRowHtml: function(row){
		var tpl = this.config.tpl;
	    if(!row) return tpl;
	    
  		return new Template(tpl).evaluate(row);
	},
		
	/* 帧听输入框的事件 */
	bindFieldEvents: function(){
		var field = this.field;
		var oThis = this;
        
        // 双击文本框的事件处理
        field.ondblclick = function(){
            oThis.show();
        }
        
        // 键盘输入文本的事件处理
        field.onkeyup= function(e){
            oThis.displayDebugInfo(oThis.field.value);
            if(oThis.field.value.length < oThis.config.minLen){
                oThis.hide();
                return;
            }
                  
		    // 获取激发事件的html元素对象
		    // IE6----window.event.srcElement
		    // FireFox,Opera----所存入的第一个参数:e.target(MouseEvent.target)
		    var _event = (window.event ? window.event : e);	
		    //alert("keyCode=" + _event.keyCode);
           switch(_event.keyCode){
                case 38://Up Arrow
                    oThis.move(false);
                    break;
                case 40://Down Arrow
                    oThis.move(true);
                    break;
                case 13:// Enter
                    oThis.finishSelect();
                    break;
                default:
                    oThis.show();
           }
        }
        
        // 失去焦点的事件处理
        field.onblur=function(){
            oThis.hide();
            oThis.clear();
        }
	},
		
	/* 解除帧听输入框的事件 */
	dispose: function(){
	    this.field.onblur = null;
	    this.field.onkeydown = null;
	    this.field.ondblclick = null;
	},
	
	/* 
	 * 上下箭头键的处理，选中上一项或下一项
	 * @param {Boolean} isNext 是否向下选择，true--向下选择，false--向上选择
	 */
    move: function(isNext){
        if (this.hidden) return;
        
        var preSelectIndex = this.selectIndex;
        if(isNext){
            this.selectIndex++;                     // 选下一项
        }else{
            this.selectIndex--;                     // 选上一项
        }
        if(this.selectIndex > this.data.length - 1){
            this.selectIndex = 0;                   // 循环选择，选第一项
        }
        if(this.selectIndex < 0){
            this.selectIndex = this.data.length - 1;  // 循环选择，选最后项
        }

        this.moveTo(preSelectIndex, this.selectIndex);
    },
 	
	/* 
	 * 上下箭头键的处理，选中上一项或下一项
	 * @param {Number} preIndex 前一选中项
	 * @param {Number} index 当前选中项
	 */
    moveTo: function(preIndex, index){
        if (this.hidden) return;
        if(index > this.data.length - 1 || index < 0) return;

        // debug
        this.displayDebugInfo("preIndex=" + preIndex
             + ";index=" + index
             + ";childNodes=" + Egd.AutoComplete.container.childNodes.length);
             
        var row;
        // 恢复前一选中项的样式
        if(preIndex >= 0){
            row = Egd.AutoComplete.container.childNodes[preIndex];
            row.style.backgroundColor = this.color.bg;
            row.style.color = this.color.font;
        }
        
        // 高亮显示当前选中项
        this.selectIndex = index; 
        row = Egd.AutoComplete.container.childNodes[index];
		row.style.backgroundColor = this.color.bg_selected;
		row.style.color = this.color.font_selected;
		row.scrollIntoView(false);// 滚动至使当前项处于可见位置
    },
   
	/* 
	 * 完成选择
	 */
    finishSelect: function(){
        if(this.selectIndex >= 0){
            var rowJson = this.data[this.selectIndex];
            this.field.value = rowJson[this.config.mapping];
            
            // debug
            this.displayDebugInfo("selectIndex=" + this.selectIndex + ";selectValue=" + this.field.value);
        	
            this.hide();
            this.clear();
            this.field.focus();
            
            // 调用回调函数
            if(typeof(this.config.afterSelected) == "function")
                this.config.afterSelected.call(this,rowJson);
        }
    }, 
	
	/* 
	 * 异步获取数据
	 * private
	 * @param start 
	 * @param limit 
	 * @param sort 
	 * @param dir 
	 * @param afterLoadData 异步获取数据后的回调函数
	 */
	loadData: function(start,limit,sort,dir,afterLoadData){
		//alert("start=" + start + ";limit=" + limit + ";sort=" + sort + ";dir=" + dir + ";pageNo=" + this.config.pageNo);
		var strUrl = this.config.data;
		if (this.config.addTimeStamp == true) strUrl = Egd.addTimeStamp(strUrl);
		var oThis = this;
		//alert(MsgBox);
		var _parameters = {start:start,limit:limit,sort:sort,dir:dir,pageNo:this.config.pageNo,pageSize:limit};
		_parameters.keyword = this.field.value;
		if(this.config.parameters)
		    Egd.apply(_parameters,this.config.parameters);
		new Ajax.Request(strUrl, 
		{ 
			method: (this.config.method ? this.config.method : 'post'), 
			parameters: _parameters,
			onSuccess: function(transport){
				//alert(transport.responseText);
				var response = eval("("+transport.responseText+")");  
				
				oThis.data = response;
				afterLoadData.call(oThis,oThis.data);
			},
			onFailure: function(transport){
    			MsgBox('获取自动完成数据过程出现异常!',"",g_OkOnly,g_ErrorIcon);
			}
		});	
	}
 };