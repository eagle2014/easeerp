/* 链接渲染函数 */
function renderHref(value,rowData,row){
	var _href = "<a href='";
	_href += "javascript:TS.Msg.wait({msg:\"正在加载模块，请稍候...\",iconCls: \"" + TS.Msg.IconCls.OPENING + "\"});";
	_href += "TS.openWindow({url:\"" + TS.rootPath + "modelAction.do?action=Open&id=" + row.ID + "\",";
	_href += "title:\"模块：" + row.Name +  "\",";
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
			        TS.Msg.wait({msg:'正在创建模块，请稍候...', iconCls: TS.Msg.IconCls.CREATING});
			        TS.openWindow({
			            url: TS.rootPath +"modelAction.do?action=Create", 
			            title: "新建模块",
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
            data: TS.rootPath + "modelAction.do?action=View",
            reader: {root: 'rows',totalProperty: 'totalCount',id: 'ID'},
	 	    cm:[
	 		    {id: "Code",text: "模块代码", width: 100, render: renderHref },
	 		    {id: "Name",text: "模块名称", render: renderHref },
	 		    {id: "Type",text: "模块类型", width: 100, render: renderType },
	 		    {id: "OrderNo",text: "模块顺序号", width: 100 },
	 		    {id: "ParentName",text: "主模块名称", width: 100, mapping:"ParentName",sortable: false },
	 		    {id: "IsInner",text: "内置模块", width: 70, render: renderYN }
	 	    ],
	 	    idColumn: {type:"int",viewType:"checkbox"},
	 	    defaultSort:{name: 'OrderNo',direction: 'asc'},
            dblClickRowHandler:function(id,rowData,row){
		        TS.Msg.wait({msg:'正在加载模块，请稍候...', iconCls: TS.Msg.IconCls.OPENING});
                TS.openWindow({
                    url: TS.rootPath +"modelAction.do?action=Open&id=" + id, 
                    title: "模块：" + row.Name,
                    id: row.Unid,
                    tabTip: row.Name + "[" + row.Code + "]"
                });
            }
        });

        // 渲染表格
        dataGrid.render();
	},

	/* 
	 * 删除视图中选中的模块信息
	 */
	doDelete: function(){
	    var ids = dataGrid.getSelected();
        if(ids.length == 0){
            TS.Msg.msgBox({title:"模块",msg:"请先选择要删除的模块信息！"});
            return;
        }
        
        // 提示不允许删除内置模块
	    var isInners = dataGrid.getSelected({unJoinIds: true, fieldName: "IsInner"});
	    for(var i = 0; i < isInners.length; i++){
	        if(isInners[i] == "Y" || isInners[i] == "y"){
                TS.Msg.msgBox({title:"模块",msg:"所要删除的模块中包含有内置模块，内置模块不允许删除！"});
                return;
	        }
	    }
        TS.Msg.confirm({title:"模块",msg:"确定要删除选中的模块信息吗？",onYes: function(){
            // 通过Ajax执行删除操作    
            var strUrl = TS.rootPath +"modelAction.do?action=Delete";
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
    				    //TS.Msg.msgBox({title:'模块',msg:response.msg});
                    }else{
    			        TS.Msg.msgBox({title:"模块", msg:'模块中包含子级内容，请先删除子级。', iconCls: TS.Msg.IconCls.ERROR});
                    }
                },
                onFailure: function(transport){
 				    TS.Msg.msgBox({title:"模块",msg: '删除模块过程出现异常！', iconCls: TS.Msg.IconCls.ERROR});
                }
            });	
        }});
    }
}

/* 自定义页面的刷新行为 */
function refresh(){
    dataGrid.reload();
}

function renderType(value){
    if(value == "0")
        return "主模块";
    else if(value == "1")
        return "子模块";
    else
        return "未知";
}

function renderYN(value){
    if(value == "Y" || value == "y")
        return "是";
    else
        return "否";
}