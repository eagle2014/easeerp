
var dataGrid;
var ITEM_SEPARATOR = "!~~!";// 条目分隔符
var thisPage = {
    initUrl:"",
    searchUrl:"", 
	/* 
	 * 页面初始化方法
	 */
	init:function(){
	    thisPage.initUrl=TS.rootPath + "wareHouseAction.do?action=View";
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
		                url: TS.rootPath +"wareHouseAction.do?action=Create", 
		                title: "新建人员",
		                id: "CreateWareHouse"
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
	 	    data: thisPage.initUrl,
	 	    reader: {root: 'rows',totalProperty: 'totalCount',id: 'ID'},
	 	    cm:[
	 		    {id: "Name",text: "名称", width: 80},
	 		    {id: "Code",text: "编码", width: 60, render: renderHref },
	 		    {id: "Status",text: "状态", render: renderWareHouseStatus },
	 		    {id: "OUName",text: "部门名称", width: 60 }
	 	    ],
	 	    idColumn: {type:"int",viewType:"checkbox"},
	 	    defaultSort:{name: 'OUFullName',direction: 'asc'},
		    dblClickRowHandler:function(id,rowData,row){
		        TS.Msg.wait({msg:'正在加载文档，请稍候...', iconCls: TS.Msg.IconCls.OPENING});
                TS.openWindow({
                    url: TS.rootPath +"wareHouseAction.do?action=Open&id=" + id, 
                    title: "仓库：" + row.Name,
                    id: row.Unid,
                    tabTip: row.Name + "[" + row.LoginID + "]"
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
		    dataUrl: TS.rootPath + 'placeAction.do?action=GetPlaceNodes',			//数据的获取地址			
		    loadexception:function(treeLoader , node, response){
			    //加载异常的处理函数
			    alert("加载OU树失败！");
		    }}
	    );
	    //定义树
	    this.tree = new TS.Tree.TreePanel('treeContainer', {
		    loader: treeLoader,
		    requestMethod:'get',
		    rootVisible:true
	    });
	    // 定义根节点
	    var root = new TS.Tree.AsyncTreeNode({
		    text: "所有",	//根节点的标题
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
		            placeUnid:node.id
                }
		    }
		    dataGrid.config.data = thisPage.initUrl;  
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
            TS.Msg.msgBox({title:"仓库配置",msg:"请先选择要删除的仓库！"});
            return;
        }
        
        TS.Msg.confirm({title:"仓库配置",msg:"确定要删除选中的仓库信息吗？",onYes: function(){
            // 通过Ajax执行删除操作    
            var strUrl = TS.rootPath +"wareHouseAction.do?action=Delete";
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
    				    //TS.Msg.msgBox({title:'人员配置',msg:response.msg});
                    }else{
    			        TS.Msg.msgBox({title:"仓库配置", msg:response.msg, iconCls: TS.Msg.IconCls.ERROR});
                    }
                },
                onFailure: function(transport){
 				    TS.Msg.msgBox({title:"仓库配置",msg: '删除仓库过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
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
	_href += "TS.openWindow({url:\"" + TS.rootPath + "wareHouseAction.do?action=Open&id=" + rowJson.ID + "\",";
	_href += "title:\"人员：" + rowJson.Name +  "\",";
	_href += "id:\"" + rowJson.Unid +  "\",";
	_href += "tabTip:\"" + rowJson.Name +  "[" + rowJson.LoginID + "]\"";
	_href += "});";
	_href += "'>" + value + "</a>";
    return _href;
}

function renderWareHouseStatus(value){
    if(value == "-1")
        return "失效";
    else if(value == "0")
        return "可用";
    else if(value == "1")
        return "已满";
    else if(value=="2")
        return "空的";
    else
        return "未知";
}