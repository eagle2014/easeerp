/* 链接渲染函数 */
function renderHref(value,rowData,rowJson){
	var _href = "<a href='";
	_href += "javascript:TS.Msg.wait({msg:\"正在加载文档，请稍候...\",iconCls: \"" + TS.Msg.IconCls.OPENING + "\"});";
	_href += "TS.openWindow({url:\"" + TS.rootPath + "jobTitleAction.do?action=Open&id=" + rowJson.ID + "\",";
	_href += "title:\"职务：" + rowJson.Name +  "\",";
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
			            url: TS.rootPath +"jobTitleAction.do?action=Create", 
			            title: "新建职务",
		                id: "CreateJobTitle"
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
            data: TS.rootPath + "jobTitleAction.do?action=View",
            reader: {root: 'rows',totalProperty: 'totalCount',id: 'ID'},
	 	    cm:[
	 		    {id: "Name",text: "名称", render: renderHref },
	 		    {id: "Code",text: "编码", width: 100 },
	 		    {id: "Level",text: "级别", width: 100 }
	 	    ],
	 	    idColumn: {type:"int",viewType:"checkbox"},
	 	    defaultSort:{name: 'Level',direction: 'asc'},
            dblClickRowHandler:function(id,rowData,row){
		        TS.Msg.wait({msg:'正在加载文档，请稍候...', iconCls: TS.Msg.IconCls.OPENING});
                TS.openWindow({
                    url: TS.rootPath +"jobTitleAction.do?action=Open&id=" + id, 
                    title: "职务：" + row.Name,
                    id: row.Unid,
                    tabTip: row.Name + "[" + row.Code + "]"
                });
            }
        });

        // 渲染表格
        dataGrid.render();
	},

	/* 
	 * 删除视图中选中的职务信息
	 */
	doDelete: function(){
	    var ids = dataGrid.getSelected();
        if(ids.length == 0){
            TS.Msg.msgBox({title:"职务",msg:"请先选择要删除的职务信息！"});
            return;
        }
        
        TS.Msg.confirm({title:"职务",msg:"确定要删除选中的职务信息吗？",onYes: function(){
            // 通过Ajax执行删除操作    
            var strUrl = TS.rootPath +"jobTitleAction.do?action=Delete";
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
    				    //TS.Msg.msgBox({title:'职务',msg:response.msg});
                    }else{
    			        TS.Msg.msgBox({title:"职务", msg:response.msg, iconCls: TS.Msg.IconCls.ERROR});
                    }
                },
                onFailure: function(transport){
 				    TS.Msg.msgBox({title:"职务",msg: '删除职务过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
                }
            });	
        }});
    }
}

/* 自定义页面的刷新行为 */
function refresh(){
    dataGrid.reload();
}