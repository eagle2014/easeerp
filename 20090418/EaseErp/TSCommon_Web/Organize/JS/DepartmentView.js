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
		
	    // 初始化tree
		this.initTree();
		
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
		                url: TS.rootPath +"departmentAction.do?action=Create", 
		                title: "新建部门",
		                id: "CreateDepartment"
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
	 	    data: TS.rootPath + "departmentAction.do?action=View",
	 	    reader: {root: 'rows',totalProperty: 'totalCount',id: 'ID'},
	 	    cm:[
	 		    {id: "OUStatus",text: "状态", width: 50, render: renderOUStatus },
	 		    {id: "FullName",text: "名称", render: renderHref},
	 		    {id: "FullCode",text: "编码", width: 150 },
	 		    {id: "OrderNo",text: "排序序号", width: 60 }
	 	    ],
	 	    idColumn: {type:"int",viewType:"checkbox"},
	 	    defaultSort:{name: 'OrderNo',direction: 'asc'},
		    dblClickRowHandler:function(id,rowData,row){
		        TS.Msg.wait({msg:'正在加载文档，请稍候...', iconCls: TS.Msg.IconCls.OPENING});
                TS.openWindow({
                    url: TS.rootPath +"departmentAction.do?action=Open&id=" + id, 
                    title: "部门：" + row.Name,
                    id: row.Unid,
                    tabTip: row.FullName + "[" + row.FullCode + "]"
                });
		    }
        });

        // 渲染表格
        dataGrid.render();
	},
	
	/* 
	 * 初始化tree
	 */
    initTree: function(){
        //定义树的加载类		
	    var treeLoader = new TS.Tree.TreeLoader({
		    dataUrl: TS.rootPath + 'ouInfoAction.do?action=GetDepartmentNodes',			//数据的获取地址			
		    loadexception:function(treeLoader , node, response){
			    //加载异常的处理函数
			    alert("加载OU树失败！");
		    }}
	    );
	    //定义树
	    this.tree = new TS.Tree.TreePanel('treeContainer', {
		    loader: treeLoader,
		    requestMethod:'get',
		    rootVisible:false
	    });
	    // 定义根节点
	    var root = new TS.Tree.AsyncTreeNode({
		    text: "系统",	//根节点的标题
		    id:'-1'
	    });
	    //设置根节点
	    this.tree.setRootNode(root);
	    // 渲染树
	    this.tree.render();
	    //是否张开所有子节点(如果为True,节点多了会卡)
	    root.expand(false,function(node){
		    //展开第一个节点的信息
	        if(node.firstChild) node.firstChild.expand();
	    });
	    this.tree.on('click',clickedNode);			
	    function clickedNode(node, e){
		    // 获取节点的属性
		    var myConfig={
		        parameters:{
		            punid:node.id
                }
		    }
		    TS.apply(dataGrid.config,myConfig);
            dataGrid.reload();
	    }
    },
    
	/* 
	 * 删除视图中选中的级别信息
	 */
	doDelete: function(){
	    var ids = dataGrid.getSelected();
        if(ids.length == 0){
            TS.Msg.msgBox({title:"部门配置",msg:"请先选择要删除的部门信息！"});
            return;
        }
        
        TS.Msg.confirm({title:"部门配置",msg:"确定要删除选中的部门信息吗？",onYes: function(){
            // 通过Ajax执行删除操作    
            var strUrl = TS.rootPath +"departmentAction.do?action=Delete";
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
    				    //TS.Msg.msgBox({title:'部门配置',msg:response.msg});
                    }else{
    			        TS.Msg.msgBox({title:"部门配置", msg:response.msg, iconCls: TS.Msg.IconCls.ERROR});
                    }
                },
                onFailure: function(transport){
 				    TS.Msg.msgBox({title:"部门配置",msg: '删除部门过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
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
	_href += "TS.openWindow({url:\"" + TS.rootPath + "departmentAction.do?action=Open&id=" + rowJson.ID + "\",";
	_href += "title:\"部门：" + rowJson.Name +  "\",";
	_href += "id:\"" + rowJson.Unid +  "\",";
	_href += "tabTip:\"" + rowJson.FullName +  "[" + rowJson.FullCode + "]\"";
	_href += "});";
	_href += "'>" + value + "</a>";
    return _href;
}

function renderOUStatus(value){
    if(value == "0")
        return "活动";
    else if(value == "1")
        return "禁用";
    else if(value == "8")
        return "已删除";
    else
        return "未知";
}