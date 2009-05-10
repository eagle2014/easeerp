/**
 * 表格组件
 * 
 * dragon 2007-08-20
 * 
 * 引用Prototype.js,TS.js
 */
 
if (typeof TS == "undefined") TS = {};

/* 
 * 表格构造函数
 * <br><br>使用范例:<pre><code>
 * // 构造grid
 * var myGrid = new TS.Grid("gridContainerId",{
 * 	   data: "../action/myAction?action=getMyData",
 * 	   reader: {root: 'rows',totalProperty: 'totalCount',id: 'id'},
 * 	   cm:[
 *         {id: "author_name",label: "作者", width: 100, mapping:"author.name" },
 *         {id: "code",label: "编码" }
 * 	   ],
 * 	   idColumn: {type:"number",viewType:"int",hidden:false},
 * 	   defaultSort:{name: 'code',direction: 'asc'}
 * });
 * // 渲染表格
 * myGrid.render();
 * </code></pre>
 * @constructor
 * @param {String} container 表格容器的html元素id值
 * @param {Object} config 所要创建的表格的配置
 * @config {String|Array} data 加载条目数据的url或直接包含表格行数据的数组
 * 如果为数组，格式为[[1,"name1"],[2,"name2"],...],
 * @config {Boolean} remoteSort 是否使用远程排序(默认为true)
 * @config {Boolean} singleSelect 行是否为单选(默认为false) 
 * @config {Number} pageNo 页码(从1开始,默认为1) 
 * @config {Number} pageSize 页容量(默认为25) 
 * @config {Object} pagingToolBar 分页条配置(默认为不显示分页条)
 * @config {Object} parameters 提交时附加提交的参数
 * @config {Array} cm 列映射定义
 * @cm {String} id 列标识符
 * @cm {String} text 表格头显示的文字
 * @cm {String} mapping 对应JSON数据中的字段映射名(可选配置，默认为与id的值相同)
 * @cm {Function} render 单元格所要显示的内容的渲染函数(可选配置，默认为直接显示JSON数据中对应字段的值),
 *      函数的上下文为grid，第1个参数为实际的值，第2个参数为整行数据组成的数组。
 * @cm {Number} width 列宽(可选配置)
 * @cm {Boolean} resizable 列是否可通过拖拉来改变列宽(可选配置，默认为true)
 * @cm {Boolean} sortable 列是否可排序(可选配置，默认为true)
 * @cm {Boolean} hidden 列是隐藏不显示(可选配置，默认为false)
 * @cm {String} textAlign 表格头中显示的文字的水平对齐方式："center"、"left"或"right"(可选配置，默认为center)
 * @cm {String} cellAlign 单元格中显示的文字的水平对齐方式："center"、"left"或"right"(可选配置，默认为left)
 * @cm {String} type 单元格值的类型："int"或"string"(可选配置，默认为"string")
 * @config {Object} reader JSON读取器配置,JSON数据格式类似为：{totalCount:2,rows:[{id:myid1,name:myName1}]}，
 * 如果配置参数的data为url，则默认为{root: 'rows',totalProperty: 'totalCount',id: 'ID'};
 * 如果配置参数的data为Array，则格式为["id","name",...],元素与data数组中的数组项中的元素一一对应
 * 其中：
 * id----data数组中的数组项的唯一标识
 * type----data数组中的数组项的数据类型："int"、"float"、"date"、"string",默认为"string"
 * @reader {String} root JSON中包含行数据的数组的字段名
 * @reader {String} totalProperty JSON中包含总条目数的字段名
 * @reader {String} id JSON中每个条目的唯一标识的字段名
 * @config {Object} idColumn 首列配置，格式为：{type:"",viewType:"",hidden:false,imgPath:""}，默认为：{type:"number",hidden:false}
 * @idColumn {String} viewType id列显示的类型："checkbox"、"number"或"img" 
 * @idColumn {String} type id列对应的数据的类型："int"、或"string",默认为"int" 
 * @idColumn {Boolean} hidden 是否隐藏id列，默认为false
 * @idColumn {Function} render id列的渲染函数，会根据type值的不同渲染不同显示方式
 * @idColumn {String} imgPath id列类型为"img"时对应表格头显示的图标的路径,默认为空
 * @config {String} autoExpandColumn 自动扩展列宽的列名(未支持) 
 * @config {Object} defaultSort 可选配置：默认排序字段的配置
 * @defaultSort {String} name 默认排序的列名
 * @defaultSort {direction} direction 默认排序的方向("asc" | "desc"),默认为"asc"
 * @config {Object} pageTB 分页条配置（如不配置则不显示分页条）
 * @pageTB {Boolean} showTB 是否显示分页条(默认为true)
 * @pageTB {Boolean} showText 是否显示按钮文字(默认为true)
 * @pageTB {Boolean} excell 是否显示导出为Excell按钮(默认为false)
 * @pageTB {Function} excellHandler 点击excell按钮的回调函数
 * @pageTB {Boolean} pdf 是否显示导出为PDF按钮(默认为false)
 * @pageTB {Function} pdfHandler 点击pdf按钮的回调函数
 * @config {Boolean} autoHeight 自动根据条目的数量调整grid的高度(默认为false)
 * @config {Boolean} loadMask 加载数据时使用模式面罩(默认为false)
 * @config {Boolean} trackMouseOver 跟随鼠标的行高亮显示(默认为true)
 */
TS.Grid = function(container,config){
	this.id = container;
	this.el = $(this.id);
	this.config = {};
	TS.apply(this.config,config,this.defaultConfig);
		
	if (typeof this.config.data == "string"){
	    this.defaultConfig.idColumn.id = this.config.reader.id;
	}

	TS.applyIf(this.config.idColumn,this.defaultConfig.idColumn);
	TS.applyIf(this.config.pageTB,this.defaultConfig.pageTB);
	
	// 数据有效性检测
	if(!this.validate) return;
	
	// 初始化
	this.init(this.config);
};

/**
 * 默认的id列渲染函数
 * @param {String|Number} value 原始值
 * @param {Number} rowIndex 行索引号
 * @param {Number} columnIndex 列索引号
 * @param {Array} rowData 包含整行数据的数组
 * @return {String|Number} 渲染后的值
 */
var TS_Grid_idRender = function(value,rowData,rowIndex,columnIndex){
	var spanName = this.id + "chkSpan";
	if (this.config.idColumn.viewType == "number"){
		return rowIndex + 1;
	}else if (this.config.idColumn.viewType == "checkbox"){
		if (this.config.singleSelect == true)
			return '<div name="' + spanName + '" id="' + spanName + '" class="' + this.tplFormater.rIdOptionClass + '"></div>';
		else
			return '<div name="' + spanName + '" id="' + spanName + '" class="' + this.tplFormater.rIdCheckboxClass + '"></div>';
	}else if (this.config.idColumn.viewType == "img"){
		var _html = '<div name="' + spanName + ' id="' + spanName + '" " class="' + this.tplFormater.rIdImgClass + '"></div>';
		return _html;
	}else{
		return value;
	}
}

/**
 * TS.Grid的原型定义
 */
TS.Grid.prototype = {
	/* 
	 * 数据有效性检测
	 * private
	 */
	validate: function(){
		if(!this.config){
			TS.Grid.showMsg("不允许config为空，无法创建表格！");
			return false;
		}
		
		return true;
	},
	
	/* 
	 * 默认配置
	 * private
	 */
	defaultConfig: {
		remoteSort: true,
		singleSelect: false,
		pageNo: 1,
		pageSize: 25,
		reader: {root: 'rows',totalProperty: 'totalCount',id: 'ID'},
		autoHeight: false,
		loadMask: false,
		filter: false,
		idColumn:{
			id: "ID",
			text: "", 
			width: 25, 
			textAlign: "center",
			cellAlign: "center",
			type: "int",
			resizable: false, 
			sortable: false,
			hidden: false,
			render: TS_Grid_idRender,
			viewType: "number"
		},
		pageTB: {
			showTB:true,
			showText:false,
			excell:false,
			pdf:false
		},
		trackMouseOver: true
	},
	
	/* 
	 * 初始化
	 * private
	 */
	init: function(){
		this.initSeparateLine();
		// 处理特殊的首列
		this.config.cm.unshift(this.config.idColumn);
		var hasAuto = false;
		this.config.cm.each(function(item){
			if(!item.textAlign) item.textAlign = 'center'; 	// 默认值：表头内容居中
			if(!item.cellAlign) item.cellAlign = 'left'; 	// 默认值：单元格内容左对齐
			if(!item.type) item.type = 'string'; 			// 默认值：单元格数据的类型为字符串
			if(item.resizable != false) item.resizable = true; 	// 默认值：可改变列宽
			if(item.sortable != false) item.sortable = true; 	// 默认值：可排序列
			if(item.hidden != true) item.hidden = false; 		// 默认值：不隐藏列
			if(!item.width) item.width="200" ;
		});
		if(1){
			this.config.cm.push(
				{
					id: "blankCm",
					text: "", 
					textAlign: "center",
					cellAlign: "center",
					type: "string",
					resizable: false, 
					sortable: false,
					hidden: false
				}
			)
		}
		// 记录所有id
		this.tplFormater = {
			headerDivId:this.id +'headerDiv',
			bodyDivId:this.id +'bodyDiv',
			headerId: this.id + '_header', headerClass: 'egd-grid-header',
			hTableId: this.id + '_hTable', hTableClass: 'egd-grid-header-table',
			hTrId: this.id + '_hTr', hTrClass: 'egd-grid-header-tr',
			hTdClass: 'egd-grid-header-td', hTdClassEnd: 'egd-grid-header-td-end',
			hCSeparatorIdPrefix: this.id + '_s', hCSeparatorClass: 'egd-grid-columnSeparator',
			hCResizeableSeparatorClass: 'egd-grid-columnResizeableSeparator',
			hTitleClass: 'egd-grid-headerTitle',hSortEmptyClass: 'egd-grid-header-sort-empty',
			hSortAscClass: 'egd-grid-header-sort-asc',hSortDescClass: 'egd-grid-header-sort-desc',
			
			contentId: this.id + '_content', contentClass: 'egd-grid-content',
			cTableId: this.id + '_cTable', cTableClass: 'egd-grid-content-table',
			cTrClass: 'egd-grid-content-tr',cTrClass1: 'egd-grid-content-tr1',
			cTdClass: 'egd-grid-content-td',cTdClassEnd: 'egd-grid-content-td-end',
			cTdClass1: 'egd-grid-content-td1',cTdClassEnd1: 'egd-grid-content-td-end1',
			cTrClass_mOver: 'egd-grid-content-tr-mouseover',cTrClass_selected: 'egd-grid-content-tr-selected',
			cTextClass: 'egd-grid-content-td-text',
			
			pageClass: 'egd-grid-page',
			pageTBId: this.id + '_page',
			pageTBClass: 'egd-grid-pageTB',
			
			rIdImgClass: 'egd-grid-content-id-img',rIdCheckboxClass: 'egd-grid-content-id-checkbox',
			rIdOptionClass: 'egd-grid-content-id-option',
			
			bgColor_Over: 'rgb(217,232,251)', //#D9E8FB
			bgColor_Selected: 'rgb(154,189,242)', //9ABDF2
			bgColor_ODD: 'rgb(255,255,255)',  //#ffffff
			bgColor_Even: 'rgb(239,239,239)'		//#efefef	
		};
		
		// 创建基本结构(未加载数据)
		this.el.setStyle({overflow:"hidden"});
		var gridHTML = this.getTemplate();
		new Insertion.Top(this.el,gridHTML);
		this.initHeaderEvent();
		//alert(this.el.innerHTML);
		if ((this.config.data instanceof Array) && this.config.defaultSort){// 从内存中加载数据
			var isAsc = (this.config.defaultSort.direction == "asc");
			this.sortData.call(this,this.config.defaultSort.name,!isAsc);
		}else{		// TODO 从URL异步获取数据
			
		}
		if(this.config.defaultSort){
			this.sort = this.config.defaultSort.name;
			this.dir = this.config.defaultSort.direction;
		}
		this.headerDiv = $(this.tplFormater.headerDivId);
		this.bodyDiv = $(this.tplFormater.bodyDivId);
		Event.observe(window, 'resize', this.onResizeEvent.bind(this));
		Event.observe(this.bodyDiv, 'scroll', this.onScrollEvent.bind(this));
		// 初始化分页条
		if(this.config.pageTB.showTB == true)
			this.initPageTB();
		this.onResizeEvent();
	},
	onResizeEvent:function(){
		this.headerDiv.hide();
		this.bodyDiv.hide();
		var width = this.el.getWidth();
		this.headerDiv.setStyle({width:width});
		this.bodyDiv.setStyle({width:width});
		this.headerDiv.show();
		this.bodyDiv.show();
	},
	onScrollEvent:function(){
		this.headerDiv.scrollLeft = this.bodyDiv.scrollLeft;
		
	},
	/*
	 * 初始化表格头的事件
	 */
	initHeaderEvent: function(){
		// 表头中分隔条的鼠标事件处理
		var separators = $$('#' + this.tplFormater.hTrId + ' span');
		this.sFmousedown = [];
		this.sFmouseup = [];
		separators.each(function(separator){
			if (!Element.hasClassName(separator,this.tplFormater.hCResizeableSeparatorClass)){
				this.sFmousedown[this.sFmousedown.length] = null;
				return;
			}
			// 鼠标按下事件
			Event.observe(separator,'mousedown',this.startResize.bindAsEventListener(this));
			
			// 释放鼠标按键事件
			Event.observe(separator,'mouseup',this.endResize.bindAsEventListener(this));
		}.bind(this));
		
		// 表头单元格的鼠标事件处理
		var tdDivs = $$('#' + this.tplFormater.hTrId + ' td div');
		tdDivs.each(function(tdDiv){
			if(this.config.cm[tdDiv.parentNode.cellIndex].sortable == true){
				tdDiv.style.cursor = 'pointer';
				// 释放鼠标按键事件
				Event.observe(tdDiv,'click',this.sortColumn.bindAsEventListener(this));
			}
		}.bind(this));
		
		/*
		var headerRow = $(this.tplFormater.hTrId);
		Event.observe(headerRow,'mousemove',function(event){
			var cell = Event.findElement(event,"td");
			if (cell == document) return;
			row = cell.parentNode;
			//alert(row);
			var eX = Event.pointerX(event);
			$("msgPanel").innerHTML = "eX=" + eX
				 + ";cellWidth=" + cell.width
				 + ";getWidth=" + Element.getWidth(cell)
				 + ";offsetLeft=" + cell.offsetLeft
				 + ";pLeft=" + TS.getLeft(row)
				 + ";cMarginLeft=" + this.el.style.marginLeft + "<br/>"
				 + ";left=" + TS.getLeft(cell)
				 + ";style.left=" + cell.style.pixelLeft
				 + ";style.marginLeft=" + cell.style.marginLeft;
			//if((eX > (cell.width + cell.offsetLeft - 2)) || (eX < cell.offsetLeft + 2))
		 	if((eX > (Element.getWidth(cell) + parseInt(cell.offsetLeft) - 2))
		 		|| (eX < (parseInt(cell.offsetLeft) + 5)))
				cell.style.cursor = "e-resize";
			else
				cell.style.cursor = "default";
		}.bindAsEventListener(this));
		*/
		
	},
	
	getTemplate: function(){
		var tpl = '';
		// 表格头容器
		tpl += '<table onselectstart="return false;" width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">';
		tpl += '<tr class="#{headerClass}"><td><div id="#{headerDivId}" style="overflow:hidden;width:100%;height:100%;">';
		// 表格头table
		tpl += '<table id="#{hTableId}" class="#{hTableClass}" border="0" cellspacing="0" cellpadding="0">';
		tpl += '<tr id="#{hTrId}" class="#{hTrClass}">';
		// 表格头table的单元格
		var i = 0;
		var ei = this.config.cm.length - 1;
		this.config.cm.each(function(item){
			if(i == ei){
				tpl += '<td class="#{hTdClassEnd}" UNSELECTABLE="on"';
			}else{
				tpl += '<td class="#{hTdClass}" UNSELECTABLE="on"';
			}
			// 列宽
			if(item.width)tpl += ' width="' + item.width + 'px"';
			
			// 对齐方式
			tpl += ' align="' + item.textAlign + '">';
			
			// 列分隔条(最后列不添加列分隔条)
			if (i != ei){
				tpl += '<span id="#{hCSeparatorIdPrefix}' + i + '"';
				tpl += ' class="#{hCSeparatorClass}';
				if (item.resizable == true){
					tpl += ' #{hCResizeableSeparatorClass}';
				}
				tpl += '">';
				tpl += '&#160;';
				tpl += '</span>';
			}
			
			// 表格头显示的排序方向图标
			var sortIcon = '',classId = "hSortEmptyClass";
			if(this.config.defaultSort){
				var sortIndex = this.getColumnIndex(this.config.defaultSort.name);
				if (sortIndex == i){
					if (this.config.defaultSort.direction == "asc")
						classId = "hSortAscClass";
					else
						classId = "hSortDescClass";
				}
				this.sort = this.config.defaultSort.name;
				this.dir = this.config.defaultSort.direction;
				
			}
			
			// 添加全选/反选checkbox处理
			//if(i == 0 && this.config.idColumn.viewType == "checkbox" 
			//	&& !(this.config.singleSelect == true)){
			//	classId = "rIdCheckboxClass";
			//}
			
			sortIcon = '<span class="#{' + classId + '}"></span>';
			//alert(i + ";" + sortIcon);

			
			// 表格头显示的文字
			tpl += '<div class="#{hTitleClass}">' + item.text + sortIcon + '</div>';		
			tpl += '</td>';
			i++;
		}.bind(this));
		tpl += '<td class="#{hTdClass}" UNSELECTABLE="on" style="width:40">&#160;</td></tr>';
		tpl += '</table>';
		tpl += '</div></td></tr>';
		
		// 表格内容容器
		tpl += '<tr class="#{contentClass}"><td>';
		tpl += '<div id="#{bodyDivId}" style="overflow:auto;width:100%;height:100%;">';
		tpl += '<table id="#{cTableId}" class="#{cTableClass}" valign="top" border="0" cellspacing="0" cellpadding="0">';
		this.config.cm.each(function(item){
			tpl += '<td style="height:0px;border:0px"';
			// 列宽
			if(item.width)tpl += ' width="' + item.width + 'px"';
			// 对齐方式
			tpl += '>';
			tpl += '</td>';
			i++;
		}.bind(this));
		tpl += '</table>';
		tpl += '</div>';
		tpl += '</td></tr>';
		
		// 表格分页条容器
		if(this.config.pageTB.showTB == true){
			tpl += '<tr class="#{pageClass}"><td>';
			tpl += '<div id="#{pageTBId}" class="#{pageTBClass}"';
			tpl += '</div>';
			tpl += '</td></tr>';
		}
		

		tpl += '</table>';
  		return new Template(tpl).evaluate(this.tplFormater);
	},
	
	/*
	 * 渲染数组
	 * @param unLoadData 是否仅渲染表格而不加载数据，默认为false
	 */
	render: function(unLoadData){
		//alert("render=" + (this.config.reader.length));
		if(unLoadData == true) return;
		
		// 加载数据
		this.showPage();
		//alert(this.el.innerHTML);
	},
	
	/* 
	 * 清空当前显示的数据
	 * private
	 */
	clear: function(){
		var cTable = $(this.tplFormater.cTableId);
		/*cTable.rows.length = 0;*/
		
		var rows = cTable.rows;
		while (cTable.rows.length > 1){
			cTable.deleteRow(cTable.rows.length - 1);
		}
		
		
		//for(var i = 0;i < rows.length ;i++){
		//	cTable.deleteRow(rows[i].rowIndex);
		//};
		//alert("clear:" + cTable.rows.length);
	},
	
	/* 
	 * 显示分页数据
	 * private
	 */
	showPageInner: function(page){
		this.curPage = page;
		var cTable = $(this.tplFormater.cTableId);
		var row,cell;
		var cmLen = this.config.cm.length; 
		for(var i = 0;i < page.length ;i++){
			row = cTable.insertRow(-1);
			if((i % 2) == 0)		// 奇数行
				row.className = this.tplFormater.cTrClass;
			else					// 偶数行
				row.className = this.tplFormater.cTrClass1;
						
			for(var j = 0;j < cmLen ;j++){
				cell = row.insertCell(-1);
				if((i % 2) == 0){	// 奇数行的单元格
					if(j < cmLen - 1){
						cell.className = this.tplFormater.cTdClass;
					}else{
						cell.className = this.tplFormater.cTdClassEnd;// 最后列的单元格
					}
				}else{				// 偶数行的单元格
					if(j < cmLen - 1){
						cell.className = this.tplFormater.cTdClass1;
					}else{
						cell.className = this.tplFormater.cTdClassEnd1;// 最后列的单元格
					}
				}
				
				if(this.config.cm[j].width)
					cell.setAttribute("width", this.config.cm[j].width + 'px');
				if(this.config.cm[j].cellAlign)
					cell.setAttribute("align", this.config.cm[j].cellAlign);
					
				// 设置单元格要显示的内容
				var cellContent = '<div class="' + this.tplFormater.cTextClass + '">';
				// 
				
				if(typeof this.config.cm[j].render == "function"){
					var sourceRowJson = null;
					if (this.responseJSON){
					    sourceRowJson = this.responseJSON[this.config.reader.root][i];
					    //alert(sourceRow.Id);
					}
					cellContent += this.config.cm[j].render.call(this,page[i][j],page[i],sourceRowJson,i);
				}else{
					cellContent += page[i][j];
				}				
				cellContent += '</div>';
				// alert(cellContent);
				cell.innerHTML = cellContent;
				
				if(this.config.cm[j].id == "id"){
					// alert(cellContent);
				}
			}
			//侦听行的事件，改变行的样式(未处理行已被选择的情况)
			this.bindRowEvent(i,row);
		}
		
		

		
		//处理id列为checkbox情况下的选择事件
		var allCheckbox = document.getElementsByName(this.id + "chkSpan");
		if(null != allCheckbox){
			for(var i = 0;i < allCheckbox.length ;i++){
				Event.observe(allCheckbox[i],'click',function(event){
					var checkbox = Event.element(event);
					var row = Event.findElement(event,"tr");
					this.selectedRowInner(row,true,checkbox);
				}.bindAsEventListener(this));
			}
		}
		//alert("page.length=" + page.length + ";rows.length=" + cTable.rows.length);
		
		// 处理分页条的显示
		if(this.config.pageTB.showTB == true){
		    var tbid = this.tplFormater.pageTBId;
		    
		    // 分页按钮的处理
		    if (this.hasNextPage()){
			    this.pageTB.enable([tbid + "_btnNext",tbid + "_btnLast"]);
		    }
		    if (this.hasPrePage()){
			    this.pageTB.enable([tbid + "_btnFirst",tbid + "_btnPrev"]);
		    }
    		
		    // 分页信息的显示
		    this.pageTB.pageNoObj.value = this.getPageNo();
		    this.pageTB.pageCountObj.innerText = this.getPageCount();
		    this.pageTB.curPageStartObj.innerText = this.getCurPageStart();
		    this.pageTB.curPageEndObj.innerText = this.getCurPageEnd();
		    this.pageTB.pageTotalCountObj.innerText = this.getTotalCount();
		}
	},
	bindRowEvent:function(i,row){
		// 鼠标悬停事件--显示鼠标悬停的行样式
			Event.observe(row,'mouseover',function(event){
				var _row = Event.findElement(event,"tr");
				if(_row.selected != true){
					// _row.className = this.tplFormater.cTrClass_mOver;
					_row.style.backgroundColor=this.tplFormater.bgColor_Over;
				}
			}.bindAsEventListener(this));
			
			// 鼠标离开事件--回复原来的行样式
			Event.observe(row,'mouseout',function(event){
				var _row = Event.findElement(event,"tr");
				if(_row.selected != true){
					if((_row.rowIndex % 2) == 1)	// 奇数行
						// _row.className = this.tplFormater.cTrClass;
						_row.style.backgroundColor=this.tplFormater.bgColor_ODD;
					else							// 偶数行
						// _row.className = this.tplFormater.cTrClass1;
						_row.style.backgroundColor=this.tplFormater.bgColor_Even;
				}
			}.bindAsEventListener(this));
			
			// 鼠标点击事件--设置为选择状态的样式(未处理按住ctrl和shift键)
			Event.observe(row,'click',function(event){
				var curRow = Event.findElement(event,"tr");
				var eventElement = Event.element(event);
				var checkbox = null;
				if (this.config.idColumn.viewType == "checkbox"){
					if (this.config.singleSelect == true)
						checkbox = curRow.down('.' + this.tplFormater.rIdOptionClass);
					else
						checkbox = curRow.down('.' + this.tplFormater.rIdCheckboxClass);
				}
				
				var isCheckboxClick = (eventElement.className == this.tplFormater.rIdCheckboxClass 
					|| eventElement.className == this.tplFormater.rIdOptionClass);
				// 如果是通过点击checkbox引发的事件，则不作处理，交由checkbox的click事件处理
				if(!isCheckboxClick)this.selectedRowInner(curRow,isCheckboxClick,checkbox);
			}.bindAsEventListener(this));
			
			// 鼠标双击击事件
			if(typeof this.config.dblClickRowHandler == "function"){
				row.ondblclick = function(event){
					var curRow = Event.findElement(event,"tr");
					var eventElement = Event.element(event);
					var rowIndex = curRow.rowIndex - 1;
					var rowData = this.curPage[rowIndex];
					var sourceRow = null;
					if (this.responseJSON){
					    sourceRow = this.responseJSON[this.config.reader.root][rowIndex];
					    //alert(sourceRow.Id);
					}
					this.config.dblClickRowHandler.call(this,rowData[0],rowData,sourceRow,rowIndex);
				}.bindAsEventListener(this);
			}			
	},
	/* 
	 * 行的选择事件
	 * private
	 * @param curRow 当前要处理的表格行
	 * @param isCheckboxClick 是否点击了checkbox对象而引发的事件
	 * @param checkbox checkbox对象 
	 */
	selectedRowInner: function(curRow,isCheckboxClick,checkbox){
		// 切换checkbox的状态
		if(isCheckboxClick == true || this.config.singleSelect != true){
			//var bp = Element.getStyle(checkbox,"background-position-x");
			var curBgColor = curRow.style.backgroundColor.toString().toLowerCase().replace(/(\s)/g,"");
			var tplBgColor = this.tplFormater.bgColor_Selected.toLowerCase().replace(/(\s)/g,"");
			var oldChecked = (curBgColor==tplBgColor);
			//alert("oldChecked=" + oldChecked + ";" + curBgColor + ";" + tplBgColor );
			if (!oldChecked){
				this.setCheckboxStatus(checkbox,true);
				// curRow.className = this.tplFormater.cTrClass_selected;
				curRow.style.backgroundColor=this.tplFormater.bgColor_Selected;
				//alert(curRow.style.backgroundColor.toLowerCase().replace(" ",""));
				curRow.selected = true;
			}else{
				this.setCheckboxStatus(checkbox,false);
				// curRow.className = this.tplFormater.cTrClass_mOver;
				curRow.style.backgroundColor=this.tplFormater.bgColor_Over;
				curRow.selected = false;
			}
		}else{
			if(checkbox) this.setCheckboxStatus(checkbox,true);
			// curRow.className = this.tplFormater.cTrClass_selected;
			curRow.style.backgroundColor=this.tplFormater.bgColor_Selected;
			curRow.selected = true;
		}
			
		// 切换其它行的状态	
		if (this.config.singleSelect == true){// 单选
			var rows = $$('#' + this.tplFormater.cTableId + ' tr');// 获取所有行
			rows.each(function(row){
				if(row.rowIndex != curRow.rowIndex && row.selected == true){
					// 处理checkbox的选择
					var _checkbox = null;
					if (this.config.idColumn.viewType == "checkbox"){
						if (this.config.singleSelect == true)
							_checkbox = row.down('.' + this.tplFormater.rIdOptionClass);
						else
							_checkbox = row.down('.' + this.tplFormater.rIdCheckboxClass);
					}
					if(_checkbox) this.setCheckboxStatus(_checkbox,false);
					
					// 处理行的选择
					row.selected = false;
					if((row.rowIndex % 2) == 0)	// 奇数行
						// row.className = this.tplFormater.cTrClass;
						row.style.backgroundColor=this.tplFormater.bgColor_ODD;
					else							// 偶数行
						// row.className = this.tplFormater.cTrClass1;
						row.style.backgroundColor=this.tplFormater.bgColor_Even;
				}
			}.bind(this));
		}
		//alert(isCheckboxClick + ";" + checkbox);
	},
	
	/* 
	 * 设置checkbox对象的选择状态
	 * @param {HTMLElement} checkbox checkbox对象 
	 * @param {Boolean} checked true---选中,false--未选中
	 */
	setCheckboxStatus: function(checkbox,checked){
		if(!checkbox) return;
		if (checked == true){
			//alert(checkbox.style.backgroundPosition);
			checkbox.style.backgroundPosition = '-25px 0px';
			//alert(checkbox.style.backgroundPosition);
		}else{
			//alert("2");
			checkbox.setStyle({backgroundPosition:'0px 0px'});
		}
	},
	
	/* 
	 * 显示首页数据
	 */
	showFirstPage: function(){
		var tbid = this.tplFormater.pageTBId;
		if (this.hasPrePage()){
			this.config.pageNo = 1;
			this.showPage();
		}
		this.pageTB.disable([tbid + "_btnFirst",tbid + "_btnPrev"]);
		if (this.hasNextPage()){
			this.pageTB.enable([tbid + "_btnNext",tbid + "_btnLast"]);
		}
	},
	
	/* 
	 * 显示前一页数据
	 */
	showPrePage: function(){
		var tbid = this.tplFormater.pageTBId;
		if (this.hasPrePage()){
			this.config.pageNo --;
			this.showPage();
			this.pageTB.enable([tbid + "_btnNext",tbid + "_btnLast"]);
		}
		if (!this.hasPrePage()){
			this.pageTB.disable([tbid + "_btnFirst",tbid + "_btnPrev"]);
		}
	},
	
	/* 
	 * 显示下一页数据
	 */
	showNextPage: function(){
		var tbid = this.tplFormater.pageTBId;
		if (this.hasNextPage()){
			this.config.pageNo ++;
			this.showPage();
			this.pageTB.enable([tbid + "_btnFirst",tbid + "_btnPrev"]);
		}
		if (!this.hasNextPage()){
			this.pageTB.disable([tbid + "_btnNext",tbid + "_btnLast"]);
		}
	},
	
	/* 
	 * 显示最后一页数据
	 */
	showLastPage: function(){
		var tbid = this.tplFormater.pageTBId;
		if (this.hasNextPage()){
			this.config.pageNo = Math.ceil(this.getTotalCount()/this.config.pageSize);
			this.showPage();
		}
		this.pageTB.disable([tbid + "_btnNext",tbid + "_btnLast"]);
		if (this.hasPrePage()){
			this.pageTB.enable([tbid + "_btnFirst",tbid + "_btnPrev"]);
		}
	},
	
	hasNextPage: function(){
		return this.config.pageNo * this.config.pageSize <= this.getTotalCount();
	},
	
	hasPrePage: function(){
		return this.config.pageNo > 1;
	},
	
	/* 获取总的条目数 */
	getTotalCount: function(){
		var cdata = this.config.data;
		if(cdata instanceof Array){// 从内存中加载数据
			return cdata.length;
		}else{
			try{
				var totalCount = eval("(this.responseJSON." + this.config.reader.totalProperty + ")");
				//alert("getTotalCount=" + totalCount);
				return totalCount;
			}catch(e){
				return 0;
			}
		}
	},
	
	/* 获取当前页码 */
	getPageNo: function(){
		return this.config.pageNo;
	},
	
	/* 获取每页最多显示的条目数 */
	getPageSize: function(){
		return this.config.pageSize;
	},
	
	/* 获取总页数 */
	getPageCount: function(){
		return Math.ceil(this.getTotalCount() / this.getPageSize());
	},
	
	/* 获取当前页第一条数据的总索引号(从1开始计算) */
	getCurPageStart: function(){
		return (this.getTotalCount() > 0) ? ((this.getPageNo() - 1) * this.getPageSize() + 1) : 0;
	},
	
	/* 获取当前页最后一条数据的总索引号(从1开始计算) */
	getCurPageEnd: function(){
	    var curPageStart = this.getCurPageStart();
		return (curPageStart > 0) ? (curPageStart + this.curPage.length - 1) : 0;
	},
	
	/*
	 * 获取所选中的行的id值组成的用分号连接在一起的字符串或数组,
	 * 如果传入参数等于true则返回数组，否则返回用分号连接在一起的字符串
     * @param {Object} config 
     * @config {Boolean} unJoinIds true返回id数组，false返回用";"链接的id串，默认为false
     * @config {Boolean} fieldName 返回的值对应的属性名
     * @config {Boolean} returnJson true返回json对象数组，此参数若为true则unJoinIds和fieldName参数将被忽略
	 */
	getSelected: function(config, returnJson){
		var allSelected = [];
		var _config = {unJoinIds: false, fieldName: this.config.reader.id, returnJson: false};
		if (typeof config == "object"){
		    TS.apply(_config,config);
		}else if (config == true){
		    _config.unJoinIds = true;
		}
		
		var cRows = $(this.tplFormater.cTableId).rows;
		var rowJson = null;
		for(var i = 1;i < cRows.length ;i++){
		    //alert("className=" + cRows[i].className);
		    var curBgColor = cRows[i].style.backgroundColor.toString().toLowerCase();
			var oldChecked = (curBgColor==this.tplFormater.bgColor_Selected.toLowerCase());
			if(curBgColor==this.tplFormater.bgColor_Selected.toLowerCase()){
				if (this.responseJSON){
				    rowJson = this.responseJSON[this.config.reader.root][i-1];
				    allSelected.push(rowJson[_config.fieldName]);
				}else{
				    allSelected.push(this.curPage[cRows[i].rowIndex -1][0]);
				}
			}
			
			//if(cRows[i].className == this.tplFormater.cTrClass_selected){
			//	allSelected.push(this.curPage[cRows[i].rowIndex][0]);
			//}
		}
		//alert("join:" + allSelected.join(";"));
		return (_config.unJoinIds == true) ? allSelected : allSelected.join(";");
	},
	
	/* 
	 * 重新加载数据
	 */
	reload: function(url){
		if(url) this.config.data = url;
		this.showPage();
	},
	
	/* 
	 * 显示分页数据
	 * private
	 */
	showPage: function(){
		// 先清空数据
		this.clear();
		
		// 显示数据
		this.getPage(this.showPageInner);
	},
	
	/* 
	 * 获取分页数据，格式为:[[value01,value02,...],[value11,value12,...],...]
	 * private
	 * @param showPageCallback 获取数据后的回调函数，用于显示数据
	 */
	getPage: function(showPageCallback){
		var cdata = this.config.data;
		var start = (this.config.pageNo - 1) * this.config.pageSize;// 起始数
		var end = start + this.config.pageSize;// 结束数
		if(cdata instanceof Array){// 从内存中加载数据
			//alert("start=" + start + ";end=" + (end -1));
			var page = [];
			for(var i = start;i < end;i++){
				if(i < cdata.length){
					page[page.length] = cdata[i];
				}
			}
			showPageCallback.call(this,page);
		}else{// 通过url加载数据
			return this.loadPage(start,this.config.pageSize,this.sort,this.dir,showPageCallback);
		}
	},
	
	/* 
	 * 异步获取分页数据
	 * private
	 * @param start 
	 * @param limit 
	 * @param sort 
	 * @param dir 
	 * @param showPageCallback 异步获取数据后的回调函数，用于显示数据
	 */
	loadPage: function(start,limit,sort,dir,showPageCallback){
		//alert("start=" + start + ";limit=" + limit + ";sort=" + sort + ";dir=" + dir + ";pageNo=" + pageNo);
		var strUrl = this.config.data;
		if(strUrl.indexOf("timeStamp") == -1){
			if(strUrl.indexOf("?") != -1)
				strUrl += "&timeStamp=" + new Date().getTime();
			else
				strUrl += "?timeStamp=" + new Date().getTime();
		}
		var oThis = this;
		//alert(MsgBox);
		var _parameters = {start:start,limit:limit,sort:this.getColumnMapping(sort),dir:dir,pageNo:this.config.pageNo,pageSize:limit};
		if(this.config.parameters)
		    TS.apply(_parameters,this.config.parameters);
		new Ajax.Request(strUrl, 
		{ 
			method:'post', 
			parameters: _parameters,
			onSuccess: function(transport){
				//alert(transport.responseText);
				var response = eval("("+transport.responseText+")");  
				
				oThis.responseJSON = response;
				showPageCallback.call(oThis,oThis.createPageByJSON(response));
			},
			onFailure: function(transport){
    			MsgBox('获取Grid数据过程出现异常!' + TS.getDebugInfo(transport),"",g_OkOnly,g_ErrorIcon);
			}
		});	
	},
	
	/* 
	 * 获取分页数据导出 并且，格式为:[[value01,value02,...],[value11,value12,...],...]   HuangJunMing
	 * private
	 * @param showPageCallback 获取数据后的回调函数，用于显示数据
	 */
	exportDataPage: function(format){
		var start = (this.config.pageNo - 1) * this.config.pageSize;// 起始数
		var end = start + this.config.pageSize;// 结束数
		var sort = this.sort;
		var dir = this.dir;
		var strUrl = this.config.exportData;
		if(strUrl.indexOf("timeStamp") == -1){
			if(strUrl.indexOf("?") != -1)
				strUrl += "&timeStamp=" + new Date().getTime();
			else
				strUrl += "?timeStamp=" + new Date().getTime();
		}
		var oThis = this;
		var _parameters = {start:start,limit:this.config.pageSize,sort:this.getColumnMapping(sort),dir:dir,pageNo:this.config.pageNo,pageSize:this.config.pageSize,__format:format};
		if(this.config.parameters)
		    TS.apply(_parameters,this.config.parameters);
		var parameters_str = $H(_parameters);
		strUrl += "&"+parameters_str.toQueryString();
		var tabID = 'BTAB' + new Date().getTime();
		window.open(strUrl);
		//TS.openWindow({id:tabID,title:"数据导出",url:strUrl,tabTip:"数据导出",refresh:false});
	},
	
	/* 
	 * 获取数据的标识名数组
	 * private
	 * @param json 包含异步获取的所有数据的JSON对象 
	 * @return 分页数据，格式为:[[value01,value02,...],[value11,value12,...],...]
	 */
	createPageByJSON: function(json){
		//reader: {root: 'rows',totalProperty: 'totalCount',id: 'id'}
		var page = [];
		var row;
		var jsonRows = eval("(json." + this.config.reader.root + ")");
		var totalCount = eval("(json." + this.config.reader.totalProperty + ")");
		//alert(totalCount+ ";" + jsonRows.length);
		var cm = this.config.cm;
		var cell;
		for(var i = 0;i < jsonRows.length;i++){
			row = [];
			for(var j = 0;j < cm.length;j++){
			    // alert(":::" + "(jsonRows[" + i + "]." + (cm[j].mapping || cm[j].id) + ")");
				cell = eval("(jsonRows[" + i + "]." + (cm[j].mapping || cm[j].id) + ")");
				if((typeof cell == "undefined") || (cell == "null") || (cell == null))
					row[row.length] = "";
				else
					row[row.length] = cell;
					
			}
			page[page.length] = row;
			//alert("---" + row[0]);
		}
		return page;
	},
	
	/* 显示信息 */
	showMsg: function(msg){
		alert(msg);
	},
	
	/* 初始化改变表格列宽时显示的分隔线 */
	initSeparateLine:function(){
		var temp=document.getElementById("egdGridSeparateLine");
		if (!temp){
			TS.Grid.separateLine=document.createElement("div");
			TS.Grid.separateLine.id="separateLine";
			TS.Grid.separateLine.className="egd-grid-separateLine";
			TS.Grid.separateLine.style.display="none";
			document.body.appendChild(TS.Grid.separateLine);
		}
	},
	
	startResize:function(_event){
		var separator = Event.findElement(_event,"span");
		if (separator == document) return;
		// 让表头分隔条获取焦点
		separator.focus();
		// 改变文档的鼠标样式
		document.body.style.cursor = "e-resize";
		// 显示列分隔条
		var eX = Event.pointerX(_event);
		var eventElement = Event.element(_event);
		TS.Grid.separateLine.style.top = Position.cumulativeOffset(eventElement.parentNode)[1] + 'px';
		var cell = separator.parentNode;
		var oldLeft = eX;//TS.getLeft(cell) + cell.clientWidth;
		TS.Grid.separateLine.style.left = oldLeft + 'px';
		var height = $(this.tplFormater.hTableId).getHeight() + $(this.bodyDiv).getHeight();
		TS.Grid.separateLine.style.height = height + 'px';
		TS.Grid.separateLine.style.display = "block";
		TS.Grid.separateLine.oldLeft = oldLeft;
		TS.Grid.separateLine.cell = cell;
		
		// 鼠标移动的处理准备
		var oThis = this;
		document.onmousemove = this.doResize.bindAsEventListener(this);
		//Event.observe(document,'mouseup',this.endResize.bind(this,true));
		document.onmouseup = function(_event){
			if(_event)
				oThis.endResize.call(oThis,_event,true);
			else
				oThis.endResize.call(oThis);
		};
		//this.endResize.bindAsEventListener(this,true);
		document.body.ondrag = function() {return false;};
	    document.body.onselectstart = function() {
			return false;
		//	//return ECSideUtil.Dragobj==null && ECSideUtil.startDragobj==false;
	    };
		
	},
	
	endResize:function(_event,fromDoc){
		// 恢复文档的鼠标样式
		document.body.style.cursor = "";
		
		// 隐藏列分隔条
		TS.Grid.separateLine.style.display = "none";
		
		_event = _event || event || window.event;
		
		// 改变列宽
		this.handleResize(TS.Grid.separateLine.cell,Event.pointerX(_event) - TS.Grid.separateLine.oldLeft);
		
		// 解除document的事件侦听
		document.onmousemove = null;
		document.onmouseup = null;
		document.body.ondrag = null;
		document.body.onselectstart = null;
		this.onScrollEvent();
	},
	
	doResize:function(_event){
		TS.Grid.separateLine.style.left =  Event.pointerX(_event) + 'px';
		//$("msgPanel").innerHTML = "eX=" + Event.pointerX(_event);
	},
		
	/* 
	 * 执行列宽的变动处理
	 * @param {Object} cell 当前操作的单元格td对象
	 * @param {Number} deltaX 列宽的变动量，正值代表加款，负值代表变窄
	 */
	handleResize: function(cell,deltaX){
		var width = cell.clientWidth + deltaX;
		if (width < TS.Grid.minColWidth) width = TS.Grid.minColWidth;
		
		var cRows = $(this.tplFormater.cTableId).rows;
		var hRows = $(this.tplFormater.hTableId).rows;
		var cellIndex = cell.cellIndex;
		for(var i = 0;i < hRows.length ;i++){
			hRows[i].cells[cellIndex].style.width = width;
		}
		for(var i = 0;i < cRows.length ;i++){
			cRows[i].cells[cellIndex].style.width = width;
		}
		
		// 更新列宽数据
		var cmItem = this.config.cm[cellIndex];
		if(cmItem.width) cmItem.width = width;
		
		//$("msgPanel").innerHTML = "width=" + width;
	},
		
	/* 
	 * 获取指定列的索引号
	 * @param {String} columnName 列名
	 * @return {Number} 列所在位置的索引号，如果找不到该列则返回-1
	 */
	getColumnIndex: function(columnName){
		for(var i = 0;i < this.config.cm.length ;i++){
			if(this.config.cm[i].id == columnName)
				return i;
		}
		return -1;
	},
		
	/* 
	 * 获取指定索引号列的列名
	 * @param {Number} 列所在位置的索引号
	 * @return {String} 列名,如果找不到则返回null
	 */
	getColumnName: function(columnIndex){
		for(var i = 0;i < this.config.cm.length ;i++){
			if(i == columnIndex)
				return this.config.cm[i].id;
		}
		return null;
	},
		
	/* 
	 * 获取指定列的映射名
	 * @param {String} columnName 列名
	 * @return {String} 列的映射名，如果找不到该列则返回columnName
	 */
	getColumnMapping: function(columnName){
		for(var i = 0;i < this.config.cm.length ;i++){
			if(this.config.cm[i].id == columnName)
				return this.config.cm[i].mapping || columnName;
		}
		return columnName;
	},
		
	/* 
	 * 执行指定列的排序处理
	 * private
	 * @param {String} columnName 要排序的列名
	 * @param {Boolean} isAsc 是否为正向排序
	 */
	sortData: function(columnName,_isAsc){
		if (typeof columnName == "undefined") return;
		var data = this.config.data;
		if(!(data instanceof Array)) return;
		
		var isAsc = (_isAsc == true || (typeof _isAsc == "undefined"));
		var index = this.getColumnIndex(columnName);
		var type = this.config.cm[index].type;
		//alert(columnName + ";" + index + ";" + isAsc + ";" + type);
		data.sort(function(arg1,arg2){
			//alert(arg1[index] + ";" + arg2[index]);
			var v;
			if(type == 'int'){// 数字排序
				v = Math.ceil(parseFloat(arg1[index]) - parseFloat(arg2[index]));
			}else{// 字符排序
				v = arg1[index].localeCompare(arg2[index]);
			}
			return isAsc ? -v : v;
		});
	},
		
	/* 
	 * 执行指定列的排序处理
	 * @param {Object} _event prototype的事件对象
	 */
	sortColumn: function(_event){
		var td = Event.findElement(_event,"td");// 所要排序列的单元格
		var tdDiv = Event.findElement(_event,"div");// 包含表头文字的div
		var sortSpan = Element.down(tdDiv,"span");// 排序符号
		if (tdDiv == document || td == document) return;
		var index = td.cellIndex;
		var columnName = this.getColumnName(index);
		var isAsc = (this.dir == "asc" || (typeof this.dir == "undefined"));//默认为从小到大
		if(this.config.data instanceof Array){// 从内存中加载数据
			this.sortData.call(this,columnName,isAsc);
		}
		this.sort = columnName;
		this.dir = (isAsc == true ? "desc" : "asc");
		
		// 显示排序符号
		var cells = td.parentNode.cells;
		var _sortSpan;
		for(var i = 0;i < cells.length-1;i++){
			if(cells[i] != td ){
				_sortSpan = Element.down(Element.down(cells[i],"div"),"span");
				//alert(_sortSpan.className);
				if(_sortSpan.className != this.tplFormater.hSortEmptyClass){
					_sortSpan.className = this.tplFormater.hSortEmptyClass;
					//cells[i].asc = true;
				}
			}
		}
		if(this.dir == "asc") 
			sortSpan.className = this.tplFormater.hSortAscClass;
		else
			sortSpan.className = this.tplFormater.hSortDescClass;
			
		// 更新数据的显示
		this.showPage();
		//alert(sortSpan.className + ";" + this.config.data);
	},
	
	/* 初始化分页条 */
	initPageTB: function(){
		var cTBId = this.tplFormater.pageTBId;
		var grid = this;
		
		// 必须的分页按钮
		var buttonsConfig = [
			{
				id: cTBId + "_btnFirst",
				text:"首页",
				iconClass:"egd-page-first",
				handler:function(button){
					grid.showFirstPage();
				},
				showText:(grid.config.pageTB.showText == true),
				showIcon:true
			},
			{
				id: cTBId + "_btnPrev",
				text:"上一页",
				iconClass:"egd-page-prev",
				handler:function(button){
					grid.showPrePage();
				},
				showText:(grid.config.pageTB.showText == true),
				showIcon:true
			},"-",
			{
				id: cTBId + "_btnPageNo",
				text:'<div style="margin:0px;line-height:18px;float:left;">页&nbsp;</div>' 
				    + '<input type="text" id="' + cTBId + '_pageNo" style="width:30px;height:19px;margin:-1px;padding:1px;float:left;" value="1" UNSELECTABLE="off" />'
				    + '<input type="text" style="display:none;" />'
				    + '<div style="margin:0px;line-height:18px;float:left;">&nbsp;/&nbsp;'
				    + '<span id="' + cTBId + '_pageCount" >1</span>'
				    + '</div>',
				showText:true,
				showIcon:false,
				unBindEvents: true
			},"-",
			{
				id: cTBId + "_btnNext",
				text:"下一页",
				iconClass:"egd-page-next",
				handler:function(button){
					grid.showNextPage();
				},
				showText:(grid.config.pageTB.showText == true),
				showIcon:true
			},
			{
				id: cTBId + "_btnLast",
				text:"尾页",
				iconClass:"egd-page-last",
				handler:function(button){
					grid.showLastPage();
				},
				showText:(grid.config.pageTB.showText == true),
				showIcon:true
			},"-",
			{
				id: cTBId + "_btnReload",
				text:"刷新",
				iconClass:"egd-page-reload",
				handler:function(button){
					grid.showPage(); 
				},
				showText:(grid.config.pageTB.showText == true),
				showIcon:true
			}
		];
		
		// 导出为Excell的按钮 
		if ( this.config.pageTB.excell == true)buttonsConfig.push({
			id: cTBId + "_btnExportToExcell",
			text:"导出为Excell",
			iconClass:"egd-page-exportToExcell",
			handler:function(button){
				if(typeof grid.config.pageTB.excellHandler == "function")
					grid.config.pageTB.excellHandler.call(grid,button);
			},
			showText:(grid.config.pageTB.showText == true),
			showIcon:true
		});
		
		// 导出为pdf的按钮
		if (  this.config.pageTB.pdf == true)buttonsConfig.push({
			id: cTBId + "_btnExportToPdf",
			text:"导出为PDF",
			iconClass:"egd-page-exportToPdf",
			handler:function(button){
				if(typeof grid.config.pageTB.pdfHandler == "function")
					grid.config.pageTB.pdfHandler.call(grid,button);
			},
			showText:(grid.config.pageTB.showText == true),
			showIcon:true
		});
		
		// 显示分页信息的文本
		buttonsConfig.push({
			id: cTBId + "_btnPageInfo",
			text:'当前显示&nbsp;'
			    + '<span id="' + cTBId + '_curPageStart" >0</span>'
			    + '&nbsp;-&nbsp;<span id="' + cTBId + '_curPageEnd" >0</span>'
			    + '&nbsp;条，共&nbsp;<span id="' + cTBId + '_pageTotalCount">0</span>'
			    + '&nbsp;条',
			showIcon:false,
			unBindEvents: true,
			styleFloat: "right"
		});
		
		// 创建分页条
		this.pageTB = new TS.TB(cTBId,buttonsConfig,TS.TB.PAGE_CSSTpl);
		var tbid = this.tplFormater.pageTBId;
		if (this.hasNextPage()){
			this.config.pageNo = Math.ceil(this.getTotalCount()/this.config.pageSize);
			this.showPage();
		}
		
		// 禁用所有基本按钮
		this.pageTB.disable([tbid + "_btnNext",tbid + "_btnLast",tbid + "_btnFirst",tbid + "_btnPrev"]);
		
		this.pageTB.pageNoObj = $(cTBId + '_pageNo');
		this.pageTB.pageCountObj = $(cTBId + '_pageCount');
		this.pageTB.curPageStartObj = $(cTBId + '_curPageStart');
		this.pageTB.curPageEndObj = $(cTBId + '_curPageEnd');
		this.pageTB.pageTotalCountObj = $(cTBId + '_pageTotalCount');
		
        // 键盘输入页码的事件处理
		var oThis = this;
        var pageNoObj = this.pageTB.pageNoObj;
        pageNoObj.onkeyup= function(e){
		    // 获取事件对象
		    // IE6----window.event.srcElement
		    // FireFox,Opera----所存入的第一个参数:e.target(MouseEvent.target)
		    var _event = (window.event ? window.event : e);	
		    //alert("keyCode=" + _event.keyCode);
		    
		    // 如果不是按下回车键则不做处理
           if(_event.keyCode != 13) return;
           
           // 检测输入的是否是数字
           var newPageNo = parseInt(pageNoObj.value);
           if(!newPageNo){
                alert("页码值必须为整数类型！");
                return;
           }
           
            // 加载新页的数据
            oThis.config.pageNo = newPageNo;
            oThis.showPage();
        }
	}
};

/* 最小列宽 */
TS.Grid.minColWidth = 20;
