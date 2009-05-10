var dataGrid;
var ITEM_SEPARATOR = "!~~!";// 条目分隔符
var thisPage = {
    initUrl: "",
    /* 
     * 页面初始化方法
     */
    init: function(){
        thisPage.initUrl = TS.rootPath + "userAction.do?action=View&type=bySearch";
        // 初始化工具条
        this.initTB();
        
        // 初始化grid
        this.initGrid();
        
        TS.Msg.hide();
    },
    
    /* 
     * 初始化工具条
     */
    initTB: function(){
        searchBtnsTB = new Egd.TB("searchButtonsContainer", [{
            id: "btnSearch",
            text: "&nbsp;查 询&nbsp;",
            handler: function(button){
                thisPage.doSearch();
            },
            showIcon: false
        }, {
            id: "btnReset",
            text: "&nbsp;清空条件&nbsp;",
            handler: function(button){
                thisPage.doClean();
            },
            showIcon: false
        }], TS.TB.DLG_CSSTpl);
    },
    
    /* 
     * 初始化grid
     */
    initGrid: function(){
        // 构造grid
        dataGrid = new TS.Grid("gridContainer", {
            data: thisPage.initUrl,
            reader: {
                root: 'rows',
                totalProperty: 'totalCount',
                id: 'ID'
            },
            cm: [{
                id: "UserStatus",
                text: "状态",
                width: 40,
                render: renderUserStatus
            }, {
                id: "UserType",
                text: "类型",
                width: 60,
                mapping: 'UserTypeDesc'
            }, {
                id: "LoginID",
                text: "登录名",
                width: 60
            }, {
                id: "Name",
                text: "姓名"
            }, {
                id: "JobTitleName",
                text: "职务",
                width: 60
            }, {
                id: "Mobile",
                text: "手机",
                width: 90
            }, {
                id: "TelephoneNo",
                text: "电话",
                width: 65
            }, {
                id: "Email",
                text: "E-Mail",
                width: 120
            }],
            idColumn: {
                type: "int",
                viewType: "checkbox"
            },
            defaultSort: {
                name: 'OUFullName',
                direction: 'asc'
            }
        });
        
        // 渲染表格
        dataGrid.render();
    },
    showHideSearchForm: function(){
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
    buildSearchParameters: function(){
        // 过滤条件类型的定义,参考Egrand.Util.FilterType的定义
        var FilterType = {
            Equal: "Equal", // 相等匹配的任何类型
            TxtLike: "TxtLike", // 左右两侧模糊匹配的文本
            TxtLikeLeft: "TxtLikeLeft", // 左侧模糊匹配的文本
            TxtLikeRight: "TxtLikeRight", // 右侧模糊匹配的文本
            Range: "Range" // 范围匹配的任何类型
        }
        // 获取要查找的关键字
        var filterNames = "", filterValues = "", filterTypes = "";
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
        if (value && value.length > 0) {
            filterNames += "TelephoneNo" + ITEM_SEPARATOR;
            filterValues += value + ITEM_SEPARATOR;
            filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
        }
        
        // 用户的员工卡号
        value = $F("CardID");
        if (value.length > 0) {
            filterNames += "CardID" + ITEM_SEPARATOR;
            filterValues += value + ITEM_SEPARATOR;
            filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
        }
        
        // 用户的联系地址
        value = $F("Address");
        if (value.length > 0) {
            filterNames += "Address" + ITEM_SEPARATOR;
            filterValues += value + ITEM_SEPARATOR;
            filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
        }
        
        // 用户所在的办公室
        value = $F("Office");
        if (value.length > 0) {
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
        if (value.length > 0) {
            filterNames += "OUFullName" + ITEM_SEPARATOR;
            filterValues += $F("OUFullName") + ITEM_SEPARATOR;
            filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
        }
        
        // 用户职务名称
        value = $F("JobTitleName");
        if (value.length > 0) {
            filterNames += "JobTitleName" + ITEM_SEPARATOR;
            filterValues += value + ITEM_SEPARATOR;
            filterTypes += FilterType.TxtLike + ITEM_SEPARATOR;
        }
        var len = ITEM_SEPARATOR.length;
        if (filterNames.length > 0) {
            filterNames = filterNames.substring(0, filterNames.length - len);
            filterValues = filterValues.substring(0, filterValues.length - len);
            filterTypes = filterTypes.substring(0, filterTypes.length - len);
        }
        
        var parameters;
        if (filterNames.length > 0) {
            parameters = {
                filterNames: filterNames,
                filterValues: filterValues,
                filterTypes: filterTypes
            };
            dataGrid.config.data = thisPage.initUrl
        }
        else {
            parameters = {};
            dataGrid.config.data = thisPage.initUrl;
        }
        return parameters;
    },
    doClean: function(){
        $("LoginID").value = "";
        $("Name").value = "";
        $("Email").value = "";
        $("Mobile").value = "";
        $("TelephoneNo").value = "";
        $("CardID").value = "";
        $("Address").value = "";
        $("Office").value = "";
        $("OUFullName").value = "";
        $("OUUnid").value = "";
        $("JobTitleName").value = "";
        $("JobTitleUnid").selectedIndex = 0;
        
        this.doSearch();
    },
    /*
     *选择部门
     */
    selectBelongOU: function(){
        TS.Org.selectOUInfo({
            title: '选择所属组织',
            onOk: function(ouInfo){
                if ($F("OUUnid") == ouInfo.Unid) {
                    return;
                }
                else {
                    $("OUFullName").value = ouInfo.FullName;
                    $("OUUnid").value = ouInfo.Unid;
                }
            }
        });
    },
    changeJobTitle: function(){
        $('JobTitleName').value = $('JobTitleUnid').options[$('JobTitleUnid').selectedIndex].text;
    },
    /*
     *选择单位
     */
    selectSingleOU: function(){
        var result;
        var strUrl = TS.rootPath + "Organize/dialog/SelectOUInfo.aspx?ouType=DW";
        TS.Dlg.create({
            title: '请选择单位',
            width: 380,
            height: 280,
            minWidth: 380,
            minHeight: 280,
            url: strUrl,
            onOk: function(value){
                result = value.Name;
                $("UnitName").value = result;
                $("UnitUnid").value = value.Unid;
            },
            modal: false,
            maskDisabled: true,
            plain: true
        });
    }
}

/* 自定义页面的刷新行为 */
function refresh(){
    dataGrid.reload();
}

function renderUserStatus(value){
    if (value == "0") 
        return "活动";
    else 
        if (value == "1") 
            return "禁用";
        else 
            if (value == "8") 
                return "已删除";
            else 
                return "未知";
}
