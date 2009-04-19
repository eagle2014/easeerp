/* 链接渲染函数 */
function renderHref(value,rowData,rowJson){
	var _href = "<a href='";
	_href += "javascript:TS.Msg.wait({msg:\"正在加载文档，请稍候...\",iconCls: \"" + TS.Msg.IconCls.OPENING + "\"});";
	_href += "TS.openWindow({url:\"" + TS.rootPath + "policyAction.do?action=Open&id=" + rowJson.ID + "\",";
	_href += "title:\"策略：" + rowJson.Subject +  "\",";
	_href += "id:\"" + rowJson.Unid +  "\",";
	_href += "tabTip:\"名称：" + rowJson.Subject +  "</br>编码：" + rowJson.Code + "\"";
	_href += "});";
	_href += "'>" + value + "</a>";
    return _href;
}

var ITEM_SEPARATOR = "!~~!";// 条目分隔符
var VALUE_SEPARATOR = "~~!!";// 值分隔符
var dataGrid;

thisPage = {
    initUrl : "",
    searchUrl : "",
	/* 
	 * 页面初始化方法
	 */
	 
	init:function(){
	thisPage.initUrl = TS.rootPath + "policyAction.do?action=View&type=all" ;
	thisPage.searchUrl = TS.rootPath + "policyAction.do?action=View&type=bySearch";
	    
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
			    //    TS.Msg.wait({msg:'正在创建文档，请稍候...', iconCls: TS.Msg.IconCls.CREATING});
			        TS.openWindow({
			            url: TS.rootPath +"policyAction.do?action=Create", 
			            title: "新建系统策略"
			        });
			    },hidden: true
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
	 	data: TS.rootPath + "policyAction.do?action=View",
	 	reader: {root: 'rows',totalProperty: 'totalCount',id: 'ID'},
	 	cm:[
	 		{id: "BelongModule",text: "所属模块", width: 70 },
	 		{id: "Code",text: "编码", width: 80 },
	 		{id: "Subject",text: "名称", width: 300, render: renderHref },
	 		{id: "ValueDesc",text: "策略值", width: 150 ,sortable: false},
	 		{id: "OrderNo",text: "排序序号", width: 70}
	 	],
	 	idColumn: {type:"int",viewType:"checkbox"},
	 	defaultSort:{name: 'BelongModule',direction: 'asc'},
	 	dblClickRowHandler:function(id,rowData,row){
                TS.openWindow({
                    url: TS.rootPath +"policyAction.do?action=Open&id=" + id, 
                    title: "系统策略-正文"
                });
            }
		
	 });

        // 渲染表格
        dataGrid.render();
	}
	
}
/* 自定义页面的刷新行为 */
function refresh(){
    thisPage.dataGrid.reload();
}

function openDownloadPage(strUrl){
    win = window.open(strUrl,'blank');
}
