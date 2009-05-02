/* 链接渲染函数 */
function renderHref(value,rowData,rowJson){
	var _href = "<a href='";
	_href += "javascript:TS.Msg.wait({msg:\"正在加载文档，请稍候...\",iconCls: \"" + TS.Msg.IconCls.OPENING + "\"});";
	_href += "TS.openWindow({url:\"" + TS.rootPath + "ouLevelAction.do?action=Open&id=" + rowJson.ID + "\",";
	_href += "title:\"级别：" + rowJson.Name +  "\",";
	_href += "id:\"" + rowJson.Unid +  "\",";
	_href += "tabTip:\"名称：" + rowJson.Name +  "</br>编码：" + rowJson.Code + "\"";
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
			        TS.Msg.wait({msg:'正在创建文档，请稍候...', iconCls: TS.Msg.IconCls.CREATING});
			        TS.openWindow({
			            url: TS.rootPath +"ouLevelAction.do?action=Create", 
			            title: "新建级别",
		                id: "CreateOULevel"
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
            data: TS.rootPath + "ouLevelAction.do?action=View",
            reader: {root: 'rows',totalProperty: 'totalCount',id: 'ID'},
	 	    cm:[
	 		    {id: "Name",text: "名称", render: renderHref },
	 		    {id: "Code",text: "编码", width: 100 }
	 	    ],
	 	    idColumn: {type:"int",viewType:"checkbox"},
	 	    defaultSort:{name: 'Code',direction: 'asc'},
            dblClickRowHandler:function(id,rowData,row){
		        TS.Msg.wait({msg:'正在加载文档，请稍候...', iconCls: TS.Msg.IconCls.OPENING});
                TS.openWindow({
                    url: TS.rootPath +"ouLevelAction.do?action=Open&id=" + id, 
                    title: "级别：" + row.Name,
                    id: row.Unid,
                    tabTip: "名称：" + row.Name + "</br>编码：" + row.Code
                });
            }
        });

        // 渲染表格
        dataGrid.render();
	},

	/* 
	 * 删除视图中选中的级别信息
	 */
	doDelete: function(){
	    var ids = dataGrid.getSelected();
        if(ids.length == 0){
            TS.Msg.msgBox({title:"级别",msg:"请先选择要删除的级别信息！"});
            return;
        }
        
        TS.Msg.confirm({title:"级别",msg:"确定要删除选中的级别信息吗？",onYes: function(){
            // 通过Ajax执行删除操作    
            var strUrl = TS.rootPath +"ouLevelAction.do?action=Delete";
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
    				    //TS.Msg.msgBox({title:'级别',msg:response.msg});
                    }else{
    			        TS.Msg.msgBox({title:"级别", msg:response.msg, iconCls: TS.Msg.IconCls.ERROR});
                    }
                },
                onFailure: function(transport){
 				    TS.Msg.msgBox({title:"级别",msg: '删除级别过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
                }
            });	
        }});
    }
}

/* 自定义页面的刷新行为 */
function refresh(){
    dataGrid.reload();
}
