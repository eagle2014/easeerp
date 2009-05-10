var dataGrid;
var tree;
var thisPage = {
	/* 
	 * 页面初始化方法
	 */
	init:function(){
	    // 初始化工具条
		this.initTB();
		
		this.initTree();
		
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
		                url: TS.rootPath +"placeAction.do?action=Create", 
		                title: "新建地点",
		                id: "CreateUnit"
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
	*初始化树
	*/ 
	initTree:function(){
	    var treeLoader=new TS.Tree.TreeLoader({
	    dataUrl:TS.rootPath+'placeAction.do?action=GetPlaceNodes',
	    loadexception:function(treeLoader,node,response){alert('加载树失败');}
	    });
	    this.tree=new TS.Tree.TreePanel('treeContainer',{
	    loader:treeLoader,
	    requestMethod:'get',
	    rootVisible:true
	    });
	    var root=new TS.Tree.AsyncTreeNode({text:"总部",id:'-1'});
	    //
	    this.tree.setRootNode(root);
	    this.tree.render();
	    //是否张开所有子节点(如果为True,节点多了会卡)
	    root.expand(false,function(node){
		    //展开第一个节点的信息
	        if(node.firstChild) node.firstChild.expand();
	    });
	    this.tree.on('click',clickNode);
	    function clickNode(node,e){
	    //获取节点的属性
	    var myConfig={parameters:{parentUnid:node.id}};
	    dataGrid.config.data=TS.rootPath+'placeAction.do?action=View',
	    TS.apply(dataGrid.config,myConfig);
	    dataGrid.reload();
	    }
	},
	/* 
	 * 初始化grid
	 */
	initGrid: function(){
        // 构造grid
        dataGrid = new TS.Grid("gridContainer",{
	 	    data: TS.rootPath + "placeAction.do?action=View",
	 	    reader: {root: 'rows',totalProperty: 'totalCount',id: 'ID'},
	 	    cm:[
	 		    {id: "Name",text: "名称",render: renderHref},
	 		    {id: "Code",text: "编码", width: 80},
	 		    {id: "Memo",text: "备注",width:200}
	 	    ],
	 	    idColumn: {type:"int",viewType:"checkbox"},
	 	    defaultSort:{name: 'Code',direction: 'asc'},
		    dblClickRowHandler:function(id,rowData,row){
		        TS.Msg.wait({msg:'正在加载文档，请稍候...', iconCls: TS.Msg.IconCls.OPENING});
                TS.openWindow({
                    url: TS.rootPath +"placeAction.do?action=Open&id=" + id, 
                    title: "地点：" + row.Name,
                    id: row.Unid,
                    tabTip: row.Name + "[" + row.Code + "]"
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
            TS.Msg.msgBox({title:"地点配置",msg:"请先选择要删除的地点信息！"});
            return;
        }
        
        TS.Msg.confirm({title:"地点配置",msg:"确定要删除选中的地点信息吗？",onYes: function(){
            // 通过Ajax执行删除操作    
            var strUrl = TS.rootPath +"placeAction.do?action=Delete";
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
    				    //TS.Msg.msgBox({title:'地点配置',msg:response.msg});
                    }else{
    			        TS.Msg.msgBox({title:"地点配置", msg:response.msg, iconCls: TS.Msg.IconCls.ERROR});
                    }
                },
                onFailure: function(transport){
 				    TS.Msg.msgBox({title:"地点配置",msg: '删除地点过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
                }
            });	
        }});
    }
}
/* 自定义页面的刷新行为 */
function refresh(){
    dataGrid.reload();
}

/* 链接渲染函数 */
function renderHref(value,rowData,rowJson){
	var _href = "<a href='";
	_href += "javascript:TS.Msg.wait({msg:\"正在加载文档，请稍候...\",iconCls: \"" + TS.Msg.IconCls.OPENING + "\"});";
	_href += "TS.openWindow({url:\"" + TS.rootPath + "placeAction.do?action=Open&id=" + rowJson.ID + "\",";
	_href += "title:\"地点：" + rowJson.Name +  "\",";
	_href += "id:\"" + rowJson.Unid +  "\",";
	_href += "tabTip:\"" + rowJson.FullName +  "[" + rowJson.FullCode + "]\"";
	_href += "});";
	_href += "'>" + value + "</a>";
    return _href;
}
