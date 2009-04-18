var dataGrid;
var ITEM_SEPARATOR = "!~~!";// 条目分隔符
var thisPage = {
    initUrl:"",
    searchUrl:"", 
	/* 
	 * 页面初始化方法
	 */
	init:function(){
	    thisPage.initUrl=TS.rootPath + "userAction.do?action=View";
	    thisPage.searchUrl=TS.rootPath + "userAction.do?action=View&type=bySearch";
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
		                url: TS.rootPath +"userAction.do?action=Create", 
		                title: "新建人员",
		                id: "CreateUserInfo"
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
			    id:"btnSearch",
			    text:"查询",
			    iconClass:"egd-button-search",
			     handler: function(button){
                thisPage.showHideSearchForm();
            }
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
	 		    {id: "UserStatus",text: "状态", width: 40, render: renderUserStatus },
	 		    {id: "UserType",text: "类型", width: 60, mapping:'UserTypeDesc' },
	 		    {id: "LoginID",text: "登录名", width: 60, render: renderHref },
	 		    {id: "Name",text: "姓名", render: renderHref },
	 		    {id: "JobTitleName",text: "职务", width: 60 },
	 		    {id: "Mobile",text: "手机", width: 90 },
	 		    {id: "TelephoneNo",text: "电话", width: 65 },
	 		    {id: "Email",text: "E-Mail", width: 120 }
	 	    ],
	 	    idColumn: {type:"int",viewType:"checkbox"},
	 	    defaultSort:{name: 'OUFullName',direction: 'asc'},
		    dblClickRowHandler:function(id,rowData,row){
		        TS.Msg.wait({msg:'正在加载文档，请稍候...', iconCls: TS.Msg.IconCls.OPENING});
                TS.openWindow({
                    url: TS.rootPath +"userAction.do?action=Open&id=" + id, 
                    title: "人员：" + row.Name,
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
		            ouUnid:node.id
                }
		    }
		    dataGrid.config.data = thisPage.initUrl;  
		    TS.apply(dataGrid.config,myConfig);
            dataGrid.reload();
	    }
    },
    showHideSearchForm:function()
   {
        var searchForm = $("searchToolbar");
        var visible = $("searchToolbar").visible();
        if (visible) 
            searchForm.hide();
        else 
            searchForm.show();
   }, 
   /* 
     * 查询
     */
    doSearch: function(){
        var parameters = thisPage.buildSearchParameters();
        TS.apply(dataGrid.config, {
            parameters: parameters
        });
        dataGrid.reload();
    },
    buildSearchParameters:function()
   {
    // 过滤条件类型的定义,参考Egrand.Util.FilterType的定义
	    var FilterType = {
	        Equal: "Equal",                     // 相等匹配的任何类型
	        TxtLike: "TxtLike",                 // 左右两侧模糊匹配的文本
	        TxtLikeLeft: "TxtLikeLeft",         // 左侧模糊匹配的文本
	        TxtLikeRight: "TxtLikeRight",       // 右侧模糊匹配的文本
	        Range: "Range"                      // 范围匹配的任何类型
	    }
		// 获取要查找的关键字
		var filterNames = "",filterValues = "",filterTypes = "";
		var value;
		
		// 用户登陆帐号
		value = $F("LoginID");
		if (value.length > 0) {
			filterNames += "LoginID" + ITEM_SEPARATOR;
			filterValues += value + ITEM_SEPARATOR;
			filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
		}
		
		// 用户的名称
		value = $F("Name");
		if (value.length > 0) {
			filterNames += "Name" + ITEM_SEPARATOR;
			filterValues += value + ITEM_SEPARATOR;
			filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
		}
		
		// email
		value = $F("Email");
		if (value.length > 0) {
			filterNames += "Email" + ITEM_SEPARATOR;
			filterValues += value + ITEM_SEPARATOR;
			filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
		}
		
		// 用户的手机号码
		value = $F("Mobile");
		if (value.length > 0) {
			filterNames += "Mobile" + ITEM_SEPARATOR;
			filterValues += value + ITEM_SEPARATOR;
			filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
		}
		
		// 用户的工作电话
		value = $F("TelephoneNo");
		if (value && value.length > 0){
			filterNames += "TelephoneNo" + ITEM_SEPARATOR;
			filterValues += value + ITEM_SEPARATOR;
			filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
		}
		
		// 用户的员工卡号
		value = $F("CardID");
		if (value.length > 0){
			filterNames += "CardID" + ITEM_SEPARATOR;
			filterValues += value + ITEM_SEPARATOR;
			filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
		}
		
		// 用户的联系地址
		value = $F("Address");
		if (value.length > 0){
			filterNames += "Address" + ITEM_SEPARATOR;
			filterValues += value + ITEM_SEPARATOR;
			filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
		}
		
		// 用户所在的办公室
		value = $F("Office");
		if (value.length > 0){
			filterNames += "Office" + ITEM_SEPARATOR;
			filterValues += value + ITEM_SEPARATOR;
			filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
		}
		
		// 所属单位的名称
//		value = $F("UnitUnid");
//		if (value.length > 0){
//			filterNames += "UnitUnid" + ITEM_SEPARATOR;
//			filterValues += value + ITEM_SEPARATOR;
//			filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
//		}
		
		// 用户隶属的组织的名称
		value = $F("OUUnid");
		if (value.length > 0){
			filterNames += "OUFullName" + ITEM_SEPARATOR;
			filterValues += $F("OUFullName") + ITEM_SEPARATOR;
			filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
		}
		
		// 用户职务名称
		value = $F("JobTitleName");
		if (value.length > 0){
			filterNames += "JobTitleName" + ITEM_SEPARATOR;
			filterValues += value + ITEM_SEPARATOR;
			filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
		}		
		var len = ITEM_SEPARATOR.length;
		if (filterNames.length > 0){
			filterNames = filterNames.substring(0,filterNames.length - len);
			filterValues = filterValues.substring(0,filterValues.length - len);
			filterTypes = filterTypes.substring(0,filterTypes.length - len);
		}

	    var parameters;
		if (filterNames.length > 0){
	        parameters = {
                filterNames: filterNames,
                filterValues: filterValues,
                filterTypes: filterTypes
            };
            dataGrid.config.data = thisPage.searchUrl 
        }else{
            parameters = {};
           dataGrid.config.data = thisPage.initUrl; 
        }
    return parameters;
   }, 
  doClean:function()
  {
    $("LoginID").value="";
    $("Name").value="";
    $("Email").value="";
    $("Mobile").value="";
    $("TelephoneNo").value="";
    $("CardID").value="";
    $("Address").value="";
    $("Office").value="";
    $("OUFullName").value="";
    $("OUUnid").value="";
    $("JobTitleName").value="";
    $("JobTitleUnid").selectedIndex=0;
   
   this.doSearch(); 
  }, 
   /*
 *选择部门
 */
selectBelongOU:function()
{
   TS.Org.selectOUInfo({
            title: '选择所属组织',
            onOk: function(ouInfo){
                if($F("OUUnid") == ouInfo.Unid){
                    return;    
                }else{
                    $("OUFullName").value = ouInfo.FullName;
                    $("OUUnid").value = ouInfo.Unid;
                }
            }
        });
},
changeJobTitle:function()
{
    $('JobTitleName').value = $('JobTitleUnid').options[$('JobTitleUnid').selectedIndex].text;
},
/*
 *选择单位
 */
 selectSingleOU:function()
{
    var result;
    var strUrl = TS.rootPath + "Organize/dialog/SelectOUInfo.aspx?ouType=DW";
	  TS.Dlg.create({
            title: '请选择单位',
            width: 380,
            height: 280,
            minWidth: 380,
            minHeight:280,
            url: strUrl,
            onOk: function(value){                
                result=value.Name;          
                $("UnitName").value=result;
                $("UnitUnid").value=value.Unid;
            },
            modal:false,
            maskDisabled: true,
            plain: true
        });         
},
	/* 
	 * 删除视图中选中的级别信息
	 */
	doDelete: function(){
	    var ids = dataGrid.getSelected();
        if(ids.length == 0){
            TS.Msg.msgBox({title:"人员配置",msg:"请先选择要删除的人员信息！"});
            return;
        }
        
        TS.Msg.confirm({title:"人员配置",msg:"确定要删除选中的人员信息吗？",onYes: function(){
            // 通过Ajax执行删除操作    
            var strUrl = TS.rootPath +"userAction.do?action=Delete";
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
    			        TS.Msg.msgBox({title:"人员配置", msg:response.msg, iconCls: TS.Msg.IconCls.ERROR});
                    }
                },
                onFailure: function(transport){
 				    TS.Msg.msgBox({title:"人员配置",msg: '删除人员过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
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
	_href += "TS.openWindow({url:\"" + TS.rootPath + "userAction.do?action=Open&id=" + rowJson.ID + "\",";
	_href += "title:\"人员：" + rowJson.Name +  "\",";
	_href += "id:\"" + rowJson.Unid +  "\",";
	_href += "tabTip:\"" + rowJson.Name +  "[" + rowJson.LoginID + "]\"";
	_href += "});";
	_href += "'>" + value + "</a>";
    return _href;
}

function renderUserStatus(value){
    if(value == "0")
        return "活动";
    else if(value == "1")
        return "禁用";
    else if(value == "8")
        return "已删除";
    else
        return "未知";
}