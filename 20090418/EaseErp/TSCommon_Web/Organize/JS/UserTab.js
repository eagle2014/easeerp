/*
 * 用户页签
 */
var dataGrid;
var toolbar;
function pageInit(){
    toolbar = new TS.TB("tbContainer", ["|", {
        id: "btnAssociate",
        text: "关联",
        iconClass: "egd-button-link",
        handler: function(button){
            userSelect();
        },
        showText: true,
        showIcon: true
    }, {
        id: "btnDisassociate",
        text: "解除关联",
        iconClass: "egd-button-unlink",
        handler: function(button){
            var ids = dataGrid.getSelected();
            if (ids.length > 0) {
                TS.Msg.confirm({
                    title: "用户信息",
                    msg: "确定要解除关联吗？",
                    onYes: function(){
                        var ajaxUrl = TS.rootPath + "userAction.do?action=DisassociateAction";
                        ajaxUrl += "&userIds=" + ids;
                        ajaxUrl += "&otherUnid=" + $F("otherUnid");
                        new Ajax.Request(ajaxUrl, {
                            method: "get",
                            onComplete: function(){
                                dataGrid.reload();
                            }
                        });
                    }
                });
            }
            else {
                Egd.Msg.msgBox("还没选择任何项！");
            }
        },
        showText: true,
        showIcon: true
    }, {
        id: "btnRefresh",
        text: "刷新",
        iconClass: "egd-button-refresh",
        handler: function(button){
            dataGrid.reload();
        },
        showText: true,
        showIcon: true
    }]);
    
    // 构造grid
    dataGrid = new TS.Grid("gridContainer", {
        data: getDataUrl(),
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
            width: 60,
            render: renderHref
        }, {
            id: "Name",
            text: "姓名",
            render: renderHref
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
            viewType: "checkbox"
        },
        defaultSort: {
            name: 'FileDate',
            direction: 'desc'
        },
        pageNo: 1,
        pageSize: 5,
        pageTB: {
            showTB: true
        }
    });
    
    // 渲染表格
    dataGrid.render(false);
}

function userSelect(){
    var strUrl = TS.rootPath + "Organize/select/UserSelect.aspx?singleSelect=true";
    strUrl += "&timestamp=" + new Date().getTime();
    TS.Dlg.create({
        title: '请选择用户',
        width: 460,
        height: 400,
        url: strUrl,
        onOk: function(domains){
            if (domains != null) {
                var ajaxUrl = TS.rootPath + "userAction.do?action=AssociateAction";
                var unids = "";
                for (var i = 0; i < domains.length; i++) {
                    if (i == domains.length - 1) {
                        unids += domains[i].Unid;
                    }
                    else {
                        unids += domains[i].Unid + ",";
                    }
                }
                ajaxUrl += "&userUnids=" + unids;
                ajaxUrl += "&otherUnid=" + $F("otherUnid");
                ajaxUrl += "&otherType=" + $F("otherType");
                new Ajax.Request(ajaxUrl, {
                    method: "get",
                    onComplete: function(){
                        dataGrid.reload();
                    }
                });
            }
        }
    });
}
