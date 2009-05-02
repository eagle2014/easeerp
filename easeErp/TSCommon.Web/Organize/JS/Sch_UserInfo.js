function renderName(value,row){ 
	var _href = "<a href='";
	_href += "javascript:openFileEx(\"" + TS.rootPath + "Egd_Organize/UserInfoForm.aspx?action=open&id=" + row[0] + "\", \"人员配置-正文\")";
	_href += "'>" + value + "</a>";
    return _href;
}
var dataGrid,searchBtnsTB;
var ITEM_SEPARATOR = "!~~!";// 条目分隔符

var thisPage = {
	/* 
	 * 页面初始化方法
	 */
	init:function(){
		this.initTB();
		this.initGrid();
	    TS.Msg.hide();		
	},
	
	/* 
	 * 初始化工具条
	 */
	initTB:function(){
		searchBtnsTB = new TS.TB("searchButtonsContainer",[
			{
				id:"btnSearch",
			    text:"查询",
			    iconClass:"egd-button-search",
				handler:function(button){
                    thisPage.doSearch();
				}
			},
			{
				id:"btnReset",
				text:"清除条件",
			    iconClass:"egd-button-delete",
				handler:function(button){
                    Form.reset('thisForm');
				}
			}
		]);
	},
		
	/* 
	 * 初始化grid
	 */
	initGrid:function(){
        // 构造grid
        dataGrid = new TS.Grid("gridContainer",{
            data: TS.rootPath + "userInfoAction.do?action=View&type=bySearch",
            reader: {root: 'rows',totalProperty: 'totalCount',id: 'ID'},
            cm:[
	 		    {id: "UserStatus",text: "状态", width: 40, render: renderUserStatus },
	 		    {id: "OUFullName",text: "部门"},
	 		    {id: "Name",text: "姓名", width: 80 },
	 		    {id: "LoginID",text: "登录名", width: 100 },
	 		    {id: "JobTitleName",text: "职务", width: 80 },
	 		    {id: "Mobile",text: "手机", width: 90 },
	 		    {id: "TelephoneNo",text: "电话", width: 65 },
	 		    {id: "Email",text: "Email", width: 120 }
	 	    ],
            idColumn: {type:"int",viewType:"checkbox"},
            defaultSort:{name: 'Name',direction: 'asc'},
            pageTB: {
	            showTB:true,
	            showText:false
            },
	        dblClickRowHandler:function(id,rowData,row){
		        TS.Msg.wait({msg:'正在加载文档，请稍候...', iconCls: TS.Msg.IconCls.OPENING});
                TS.openWindow({
                    url: TS.rootPath +"userInfoAction.do?action=Open&id=" + id, 
                    title: "人员：" + row.Name,
                    id: row.Unid,
                    tabTip: row.FullName + "[" + row.FullCode + "]"
                });
		    }
	     });
    	 
	     dataGrid.render(true);
	},

	/* 
	 * 查询
	 */
	doSearch:function(){
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
		value = $F("UnitName");
		if (value.length > 0){
			filterNames += "UnitName" + ITEM_SEPARATOR;
			filterValues += value + ITEM_SEPARATOR;
			filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
		}
		
		// 用户隶属的组织的名称
		value = $F("OUName");
		if (value.length > 0){
			filterNames += "OUName" + ITEM_SEPARATOR;
			filterValues += value + ITEM_SEPARATOR;
			filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
		}
		
		// 用户职务名称
		value = $F("JobTitleName");
		if (value.length > 0){
			filterNames += "JobTitleName" + ITEM_SEPARATOR;
			filterValues += value + ITEM_SEPARATOR;
			filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
		}
		
		/*
		// 用户政治面貌
		value = $F("UserLigion");
		if (value.length > 0){
			filterNames += "UserLigion" + ITEM_SEPARATOR;
			filterValues += value + ITEM_SEPARATOR;
			filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
		}
		
		// 性别
		value = $F("Gender");
		if (value){
			filterNames += "Gender" + ITEM_SEPARATOR;
			filterValues += value + ITEM_SEPARATOR;
			filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
		}
		*/
		
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
        }else{
            parameters = {};
        }
	    TS.apply(dataGrid.config,{parameters: parameters});
	    
	    //MsgBox("filterNames=" + filterNames + "<br/>" + "filterTypes=" + filterTypes + "<br/>" + "filterValues=" + filterValues);
        dataGrid.reload();
	}
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

/*
 *选择部门
 */
function selectOUInfo()
{
    var result;
    var strUrl = TS.rootPath + "Egd_Organize/dialog/SelectOUInfo.aspx?ouType=BM";
	  TS.Dlg.create({
            title: '请选择部门',
            width: 380,
            height: 280,
            minWidth: 380,
            minHeight:280,
            url: strUrl,
            onOk: function(value){                
                result=value.Name;          
                $("OUName").value=result;
                $("OUUnid").value=value.Unid;
            },
            modal:false,
            maskDisabled: true,
            plain: true
        });         
}

/*
 *选择单位
 */
function selectSingleOU()
{
    var result;
    var strUrl = TS.rootPath + "Egd_Organize/dialog/SelectOUInfo.aspx?ouType=DW";
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
}

/* 
 * 选择上级单位/部门
 * @param {String} selectType 选择OU的类型：BM--选择部门，DW--选择单位
 * @param {String} punid 上级OU的Unid
 * 页面调用selectUpper('BM',$F('UnitUnid'),$F('UnitName'))
 */
function selectUpper(selectType,punid,pname)
{
    if(selectType == 'BM')
    {
        var unitUnid = $F("UnitUnid");
        if(null == unitUnid || unitUnid == "")
        {
            TS.Msg.msgBox({title:"提示",msg: "请先选择所属单位！", iconCls: TS.Msg.IconCls.WARN});
            return;
        }
    }
    var strUrl = TS.rootPath + 'Egd_Organize/dialog/SelectOUInfo.aspx?type=all';
    strUrl += '&ouType=' + selectType + '&singleSelect=true';
    if(punid !="")strUrl += '&parent=' + punid;
    if(pname !="")strUrl += '&parentName=' + escape(pname);
    strUrl += '&parentIconCls=' + (punid ? 'egd-treeNode-unit':'egd-treeNode-department');// 根节点的样式
    strUrl += '&rootVisible=' + (punid ? 'true' : 'false');// 根节点的可见性
    strUrl += '&singleClickExpand=true';// 单击就展开节点
    strUrl += '&timeStamp=' + new Date().getTime();
    TS.Dlg.create({
            title: '请选择单位',
            width: 380,
            height: 280,
            minWidth: 380,
            minHeight:280,
            url: strUrl,
            onOk: function(value){                
                result=value.Name;          
                $("OUName").value=result;              
            },
            modal:false,
            maskDisabled: true,
            plain: true
        });         
}


