/* 链接渲染函数 */
function renderHref(value,rowData,row){
	var _href = "<a href='";
	_href += "javascript:TS.Msg.wait({msg:\"正在加载可选项，请稍候...\",iconCls: \"" + TS.Msg.IconCls.OPENING + "\"});";
	_href += "TS.openWindow({url:\"" + TS.rootPath + "optionItemAction.do?action=Open&id=" + row.ID + "\",";
	_href += "title:\"可选项：" + row.Name +  "\",";
	_href += "id:\"" + row.Unid +  "\",";
	_href += "tabTip:\"" + row.Name +  "[" + row.Code + "]\"";
	_href += "});";
	_href += "'>" + value + "</a>";
    return _href;
}

var dataGrid;
var thisPage = {
	/* 
	 * 页面初始化方法
	 */
	init:function(){
	    // 初始化工具条
		this.initTB();
		
	    // 初始化grid
		this.initGrid();
		
	    // 初始化工具条的下拉框
		this.initSelectType();
		
		TS.Msg.hide();
	},
 	
	/* 
	 * 初始化工具条
	 */
	initTB:function(){
        this.toolbar=new TS.TB("tbContainer",[
		    "|",
		     {
			    id:"btnNew",
			    text:"新建",
			    iconClass:"egd-button-new",
			    handler:function(button){
			        TS.Msg.wait({msg:'正在创建可选项，请稍候...', iconCls: TS.Msg.IconCls.CREATING});
			        TS.openWindow({
			            url: TS.rootPath +"optionItemAction.do?action=Create", 
			            title: "新建可选项",
		                id: "CreateModel"
			        });
			    },
			    // 管理员才能新建
			    hidden: !my.isManager
		    },
		    {
			    id:"btnDelete",
			    text:"删除",
			    iconClass:"egd-button-delete",
			    handler:function(button){
			        thisPage.doDelete();
			    },
			    // 管理员才能删除
			    hidden: !my.isManager
		    },
		    {
			    id: "btnType",
			    text: "<select id='selectType' onchange='refresh();'><option value=''>全部</option></select>",
			    iconClass: "",
			    showText: true,
			    showIcon: false,
			    unBindEvents: true
		    },
		    {
			    id:"btnRefresh",
			    text:"刷新",
			    iconClass:"egd-button-refresh",
			    handler:function(button){
				    dataGrid.reload();
			    }
		    }
	    ]);
	 },
	
	/* 
	 * 初始化grid
	 */
	initGrid: function(){
        // 构造grid
        dataGrid = new TS.Grid("gridContainer",{
            data: TS.rootPath + "optionItemAction.do?action=View",
            reader: {root: 'rows',totalProperty: 'totalCount',id: 'ID'},
	 	    cm:[
	 		    {id: "Type",text: "所属分类编码",width:120 },
	 		    {id: "TypeName",text: "所属分类名称",width:120 },
	 		    {id: "Code",text: "代码", width: 120, render: renderHref },
	 		    {id: "Name",text: "名称", render: renderHref },
	 		    {id: "OrderNo",text: "排序序号",width:70 }
	 	    ],
	 	    idColumn: {type:"int",viewType:"checkbox"},
	 	    defaultSort:{name: 'TypeName',direction: 'asc'},
            dblClickRowHandler:function(id,rowData,row){
		        TS.Msg.wait({msg:'正在加载可选项，请稍候...', iconCls: TS.Msg.IconCls.OPENING});
                TS.openWindow({
                    url: TS.rootPath +"optionItemAction.do?action=Open&id=" + id, 
                    title: "可选项：" + row.Name,
                    id: row.Unid,
                    tabTip: row.Name + "[" + row.Code + "]"
                });
            }
        });

        // 渲染表格
        dataGrid.render();
	},

	/* 
	 * 删除视图中选中的可选项信息
	 */
	doDelete: function(){
	    var ids = dataGrid.getSelected();
        if(ids.length == 0){
            TS.Msg.msgBox({title:"可选项",msg:"请先选择要删除的可选项信息！"});
            return;
        }
        
        // 提示不允许删除内置可选项
	    var isInners = dataGrid.getSelected({unJoinIds: true, fieldName: "IsInner"});
	    for(var i = 0; i < isInners.length; i++){
	        if(isInners[i] == "Y" || isInners[i] == "y"){
                TS.Msg.msgBox({title:"可选项",msg:"所要删除的可选项中包含有内置可选项，内置可选项不允许删除！"});
                return;
	        }
	    }
        
        TS.Msg.confirm({title:"可选项",msg:"确定要删除选中的可选项信息吗？",onYes: function(){
            // 通过Ajax执行删除操作    
            var strUrl = TS.rootPath +"optionItemAction.do?action=Delete";
            strUrl += "&timestamp=" + new Date().getTime();
            new Ajax.Request(strUrl, 
            { 
                method:'post', 
                parameters: {ids: ids},
                onSuccess: function(transport){
                    var response = eval("("+transport.responseText+")");  
                    if (response.result == true){
                        refresh();
                        TS.Msg.hide();
    				    //TS.Msg.msgBox({title:'可选项',msg:response.msg});
                    }else{
    			        TS.Msg.msgBox({title:"可选项", msg:response.msg, iconCls: TS.Msg.IconCls.ERROR});
                    }
                },
                onFailure: function(transport){
 				    TS.Msg.msgBox({title:"可选项",msg: '删除可选项过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
                }
            });	
        }});
    },
    
	/* 
	 * 初始化工具条的下拉框
	 */
    initSelectType: function(){
        var selObj = $('selectType');
        var options = selObj.options;
        var array = my.allSelectTypeJsonString;
        for(var i = 0; i < array.length; i++){
		    options[selObj.length] = new Option(array[i].Name, array[i].Value);
        }
    }
}

/* 自定义页面的刷新行为 */
function refresh(){
    dataGrid.config.data = TS.rootPath + "optionItemAction.do?action=View&type=" + $F('selectType'),
    dataGrid.reload();
}
