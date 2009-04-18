/**
 * 组织结构处理
 * dragon 2007-06-16
 * singleton
 *
 * 引用:TS.js,
 */
TS.namespace("TS.Org");
TS.Org = {
    /* 通过OU选择UserInfo
     * @param {Object} config 配置
     * @config {Function} onOk 点击ok按钮的回调函数，第一个参数为包含人员详细信息的json对象,
     *		json对象属性与UserInfo Domain对应
     * @config {String} title 可选：窗口的标题,
     * @config {String} ouType 可选：所选择OU的类型，BM--选择部门，DW--选择单位，空--全部(默认)
     * @config {String} userType 可选：用户类型：空--全部(默认)，"0"--普通用户，"1"--服务台用户，"2"--服务支持用户
     * @config {String} rootOUUnid 可选：根节点对应OU的Unid
     * @config {String} rootOUName 可选：根节点对应OU的Name
     * @config {String} rootIconCls 可选：根节点对应OU的图片样式
     * @config {String} singleClickExpand 可选：是否单击OU节点就展开，默认为true
     * @config {String} rootVisible 可选：OU根节点是否可见，默认为false
     * @config {String} singleSelect 可选：单选还是多选：false--多选,true--单选(默认)
     * @config {String | Array} copyTo 可选：是否将选中的用户信息自动复制到对应的html控件
     *     1)如果copyTo为String类型，表示copyTo是相关域的前缀,
     *           如你的Domain名称属性为Requester_Name则域前缀为"Requester_"
     *           多个前缀间使用逗号分隔，如"Requester_,Submitter_"
     *     2)如果copyTo为Array类型，表示copyTo是要设置的域及相关映射配置，
     *         格式为[{name: fieldName,mapping:jsonFieldName},...],mapping为可选配置，默认与name的值相同；
     *         如果全部都可忽略mapping配置，可以使用简易格式[fieldName1, fieldName2, ,...]
     */
    selectUserInfoByOU: function(config){
        TS.applyIf(config, {
            id: 'selectUserInfoByOU',
            title: '选择用户',
            width: 410,
            height: 347,
            minWidth: 410,
            minHeight: 347,
            singleClickExpand: false,
            rootVisible: false,
            singleSelect: true
        });
        var url = TS.rootPath + 'Organize/dialog/SelectUserInfoByOU.aspx?t=0';
        config.url = TS.Org.buildOUUrl(url, config);
        
        // 自定义afterOk函数，按需自动复制属性值到html控件
        if (typeof config.afterOk != 'function') {
            if (typeof config.copyTo == 'string') {
                //alert("config.copyTo=" + config.copyTo);
                config.afterOk = function(userInfo){
                    TS.Org.setDomainInfoToFields(userInfo, config.copyTo, null);
                };
            }
            else 
                if (config.copyTo instanceof Array) {
                    config.afterOk = function(userInfo){
                        TS.Org.setDomainInfoToFields(userInfo, null, config.copyTo);
                    };
                }
            if ((typeof config.afterOk == 'function') && (typeof config.onOk != 'function')) 
                config.onOk = function(){
                };// 强制生成空的onOk处理函数，保证确认按钮的显示
        }
        
        // 显示选择人员的对话框
        TS.Dlg.create(config);
    },
    
    /* 通过OU选择UserInfo
     * @param {Object} config 配置
     * @config {Function} onOk 点击ok按钮的回调函数，第一个参数为包含人员详细信息的json对象,
     *		json对象属性与UserInfo Domain对应
     * @config {String} title 可选：窗口的标题,
     * @config {String} ouType 可选：所选择OU的类型，BM--选择部门，DW--选择单位，空--全部(默认)
     * @config {String} userType 可选：用户类型：空--全部(默认)，"0"--普通用户，"1"--服务台用户，"2"--服务支持用户
     * @config {String} rootOUUnid 可选：根节点对应OU的Unid
     * @config {String} rootOUName 可选：根节点对应OU的Name
     * @config {String} rootIconCls 可选：根节点对应OU的图片样式
     * @config {String} singleClickExpand 可选：是否单击OU节点就展开，默认为true
     * @config {String} rootVisible 可选：OU根节点是否可见，默认为false
     * @config {String} singleSelect 可选：单选还是多选：false--多选,true--单选(默认)
     * @config {String | Array} copyTo 可选：是否将选中的用户信息自动复制到对应的html控件
     *     1)如果copyTo为String类型，表示copyTo是相关域的前缀,
     *           如你的Domain名称属性为Requester_Name则域前缀为"Requester_"
     *           多个前缀间使用逗号分隔，如"Requester_,Submitter_"
     *     2)如果copyTo为Array类型，表示copyTo是要设置的域及相关映射配置，
     *         格式为[{name: fieldName,mapping:jsonFieldName},...],mapping为可选配置，默认与name的值相同；
     *         如果全部都可忽略mapping配置，可以使用简易格式[fieldName1, fieldName2, ,...]
     */
    selectUserInfoByGroup: function(config){
        TS.applyIf(config, {
            id: 'selectUserInfoByGroup',
            title: '选择岗位用户',
            width: 410,
            height: 347,
            minWidth: 410,
            minHeight: 347,
            singleClickExpand: false,
            rootVisible: false,
            singleSelect: true
        });
        var url = TS.rootPath + 'Organize/dialog/SelectUserInfoByGroup.aspx?t=0';
        config.url = TS.Org.buildOUUrl(url, config);
        
        // 自定义afterOk函数，按需自动复制属性值到html控件
        if (typeof config.afterOk != 'function') { // afterOk函数没有被自定义，则处理copyTo的配置
            if (typeof config.copyTo == 'string') {
                config.afterOk = function(userInfo){
                    TS.Org.setDomainInfoToFields(userInfo, config.copyTo, null);
                };
            }
            else 
                if (config.copyTo instanceof Array) {
                    config.afterOk = function(userInfo){
                        TS.Org.setDomainInfoToFields(userInfo, null, config.copyTo);
                    };
                }
            if ((typeof config.afterOk == 'function') && (typeof config.onOk != 'function')) 
                config.onOk = function(){
                };// 强制生成空的onOk处理函数，保证确认按钮的显示
        }
        
        // 显示选择人员的对话框
        TS.Dlg.create(config);
    },
    
    /* 选择OUInfo
     * @param {Object} config 配置
     * @config {Function} onOk 点击ok按钮的回调函数，第一个参数为包含人员详细信息的json对象,
     *		json对象属性与UserInfo Domain对应
     * @config {String} title 可选：窗口的标题
     * @config {String} ouType 可选：所选择OU的类型，BM--选择部门，DW--选择单位，空--全部(默认)
     * @config {String} rootOUUnid 可选：根节点对应OU的Unid
     * @config {String} rootOUName 可选：根节点对应OU的Name
     * @config {String} rootIconCls 可选：根节点对应OU的图片样式
     * @config {String} singleClickExpand 可选：是否单击OU节点就展开，默认为true
     * @config {String} rootVisible 可选：OU根节点是否可见，默认为false
     * @config {String} singleSelect 可选：单选还是多选：false--多选,true--单选(默认)
     * @config {String | Array} copyTo 可选：是否将选中的OU信息自动复制到对应的html控件
     *     1)如果copyTo为String类型，表示copyTo是相关域的前缀,
     *           如你的Domain名称属性为OU_Name则域前缀为"OU_"
     *           多个前缀间使用逗号分隔，如"OU1_,OU2_"
     *     2)如果copyTo为Array类型，表示copyTo是要设置的域及相关映射配置，
     *         格式为[{name: fieldName,mapping:jsonFieldName},...],mapping为可选配置，默认与name的值相同；
     *         如果全部都可忽略mapping配置，可以使用简易格式[fieldName1, fieldName2, ,...]
     */
    selectOUInfo: function(config){
        TS.applyIf(config, {
            id: 'selectOUInfo',
            title: '选择' + (config.ouType == 'BM' ? '部门' : (config.ouType == 'DW' ? '单位' : '组织')),
            width: 250,
            height: 250,
            minWidth: 100,
            minHeight: 100,
            singleClickExpand: false,
            rootVisible: false,
            singleSelect: true
        });
        var url = TS.rootPath + 'Organize/dialog/SelectOUInfo.aspx?t=0';
        config.url = TS.Org.buildOUUrl(url, config);
        
        // 自定义afterOk函数，按需自动复制属性值到html控件
        if (typeof config.afterOk != 'function') {
            if (typeof config.copyTo == 'string') {
                config.afterOk = function(doamin){
                    TS.Org.setDomainInfoToFields(doamin, config.copyTo, null);
                };
            }
            else 
                if (config.copyTo instanceof Array) {
                    config.afterOk = function(doamin){
                        TS.Org.setDomainInfoToFields(doamin, null, config.copyTo);
                    };
                }
            if ((typeof config.afterOk == 'function') && (typeof config.onOk != 'function')) 
                config.onOk = function(){
                };// 强制生成空的onOk处理函数，保证确认按钮的显示
        }
        
        // 显示选择对话框
        TS.Dlg.create(config);
    },
    
    /* 通过OU选择岗位
     * @param {Object} config 配置
     * @config {Function} onOk 点击ok按钮的回调函数，第一个参数为包含岗位详细信息的json对象,
     *		json对象属性与Group Domain对应
     * @config {String} title 可选：窗口的标题,
     * @config {String} ouType 可选：所选择OU的类型，BM--选择部门，DW--选择单位，空--全部(默认)
     * @config {String} rootOUUnid 可选：根节点对应OU的Unid
     * @config {String} rootOUName 可选：根节点对应OU的Name
     * @config {String} rootIconCls 可选：根节点对应OU的图片样式
     * @config {String} singleClickExpand 可选：是否单击OU节点就展开，默认为true
     * @config {String} rootVisible 可选：OU根节点是否可见，默认为false
     * @config {String} groupType 可选：岗位类型：0--全部类型(默认)，1--可派单岗位，2--不可派单岗位
     * @config {String} singleSelect 可选：单选还是多选：false--多选,true--单选(默认)
     */
    selectGroupByOU: function(config){
        TS.applyIf(config, {
            id: 'selectGroupByOU',
            title: '选择岗位',
            width: 410,
            height: 347,
            minWidth: 410,
            minHeight: 347,
            singleClickExpand: false,
            rootVisible: false,
            singleSelect: true,
            preLoad: false
        });
        var url = TS.rootPath + 'Organize/dialog/SelectGroupByOU.aspx?t=0';
        config.url = TS.Org.buildOUUrl(url, config);
        TS.Dlg.create(config);
    },
    
    /* private: 重建url参数
     */
    buildOUUrl: function(url, config){
        if (config.singleSelect == true || config.singleSelect == false) {
            url += '&singleSelect=' + config.singleSelect;
            delete config.singleSelect;
        }
        if (config.ouType) {
            url += '&ouType=' + config.ouType;
            delete config.ouType;
        }
        if (config.rootOUUnid) {
            url += '&rootOUUnid=' + config.rootOUUnid;
            delete config.rootOUUnid;
        }
        if (config.rootOUName) {
            url += '&rootOUName=' + config.rootOUName;
            delete config.rootOUName;
        }
        if (config.rootIconCls) {
            url += '&rootIconCls=' + config.rootIconCls;
            delete config.rootIconCls;
        }
        if (config.singleClickExpand == true || config.singleClickExpand == false) {
            url += '&singleClickExpand=' + config.singleClickExpand;
            delete config.singleClickExpand;
        }
        if (config.rootVisible == true || config.rootVisible == false) {
            url += '&rootVisible=' + config.rootVisible;
            delete config.rootVisible;
        }
        if (config.type) {
            url += '&type=' + config.type;
            delete config.type;
        }
        if (config.groupType) {
            url += '&groupType=' + config.groupType;
            delete config.groupType;
        }
        if (config.userType) {
            url += '&userType=' + config.userType;
            delete config.userType;
        }
        if (config.preLoad) { // 是否预加载所有符合条件的信息
            url += '&preLoad=' + config.preLoad;
            delete config.preLoad;
        }
        return url;
    },
    
    /* 打开新页签查看人员信息
     * @param {String} userInfoUnid 用户的Unid
     * @param {String} userInfoName 用户的Name
     */
    viewUserInfo: function(userInfoUnid, userInfoName){
        var title = '查看人员信息';
        var tabTip = title;
        if (userInfoName) 
            tabTip = userInfoName;
        TS.openWindow({
            url: TS.rootPath + 'userAction.do?action=Open&idName=unid&id=' + userInfoUnid,
            title: title,
            tabTip: tabTip,
            id: userInfoUnid,
            refresh: false
        });
    },
    
    /* 打开新页签查看部门信息
     * @param {String} departmentUnid 部门的Unid
     * @param {String} departmentName 部门的Name
     */
    viewDepartment: function(departmentUnid, departmentName){
        var title = '查看部门信息';
        var tabTip = title;
        if (departmentName) 
            tabTip = departmentName;
        TS.openWindow({
            url: TS.rootPath + 'departmentAction.do?action=Open&idName=unid&id=' + departmentUnid,
            title: title,
            tabTip: tabTip,
            id: departmentUnid,
            refresh: false
        });
    },
    
    /* 打开新页签查看单位信息
     * @param {String} unitUnid 单位的Unid
     * @param {String} unitName 单位的Name
     */
    viewUnit: function(unitUnid, unitName){
        var title = '查看单位信息';
        var tabTip = title;
        if (unitName) 
            tabTip = unitName;
        TS.openWindow({
            url: TS.rootPath + 'unitAction.do?action=Open&idName=unid&id=' + unitUnid,
            title: title,
            tabTip: tabTip,
            id: unitUnid,
            refresh: false
        });
    },
    
    /* 打开新页签查看岗位信息
     * @param {String} groupUnid 岗位的Unid
     * @param {String} groupName 岗位的Name
     */
    viewGroup: function(groupUnid, groupName){
        var title = '查看岗位信息';
        var tabTip = title;
        if (groupName) 
            tabTip = groupName;
        TS.openWindow({
            url: TS.rootPath + 'groupAction.do?action=Open&idName=unid&id=' + groupUnid,
            title: title,
            tabTip: tabTip,
            id: groupUnid,
            refresh: false
        });
    },
    
    /* 填充Domain的信息到相关域
     * @param {Object} domainInfo Domain信息组成的json对象,json对象包含的属性与Domain类的属性对应
     * @param {String} fieldPrefix 相关域的前缀,如你的Domain名称属性为Requester_Name则域前缀为"Requester_"
     * @param {Array} fields 要设置的域及相关映射配置，
     *      格式为[{name: fieldName,mapping:jsonFieldName},...],mapping为可选配置，默认与name的值相同；
     *      如果全部都可忽略mapping配置，可以使用简易格式[fieldName1, fieldName2, ,...]
     */
    setDomainInfoToFields: function(domainInfo, fieldPrefix, fields){
        if (!domainInfo) 
            return;
        
        // 通过域前缀设置相关页面控件的值
        if (typeof fieldPrefix == 'string') {
            var allPrefix = fieldPrefix.split(',');
            for (var i = 0; i < allPrefix.length; i++) {
                var field, value;
                for (var key in domainInfo) {
                    field = document.getElementById(allPrefix[i] + key);
                    if (field) {
                        value = domainInfo[key];
                        //alert(key + "=[" + value + "];fieldName=" + fieldPrefix + key);
                        if (value) {
                            field.value = value;
                        }
                        else {
                            field.value = "";
                        }
                    }
                }
            }
        }
        
        // 通过映射设置相关页面控件的值
        if (fields instanceof Array) {
            var name, value;
            for (var i = 0; i < fields.length; i++) {
                if (typeof fields[i] == "object") {
                    name = fields[i].name;
                    value = domainInfo[fields[i].mapping ? fields[i].mapping : name];
                }
                else {// 当作字符串处理
                    name = fields[i];
                    value = domainInfo[name];
                }
                //alert("name=" + name);
                $(name).value = value;
            }
        }
    },
    
    /* 获取人员的详细信息，更多参数参考getDomainDetail的说明
     * @param {Object} config 配置选项，不能为空
     * @config {String} unid 人员的unid
     * @return {Object} 与getDomainDetail的返回值相同
     */
    getUserInfoDetail: function(config){
        config.url = TS.rootPath + "userAction.do?action=GetDetail&unid=" + config.unid;
        return TS.Org.getDomainDetail(config);
    },
    
    /* 获取OUInfo的详细信息，更多参数参考getDomainDetail的说明
     * @param {Object} config 配置选项，不能为空
     * @config {String} unid OUInfo的unid
     * @return 与getDomainDetail的返回值相同
     */
    getOUInfoDetail: function(config){
        config.url = TS.rootPath + "ouInfoAction.do?action=GetDetail&unid=" + config.unid;
        return TS.Org.getDomainDetail(config);
    },
    
    /* 获取岗位的详细信息，更多参数参考getDomainDetail的说明
     * @param {Object} config 配置选项，不能为空
     * @config {String} unid 岗位的unid
     * @return 与getDomainDetail的返回值相同
     */
    getGroupDetail: function(config){
        config.url = TS.rootPath + "groupAction.do?action=GetDetail&unid=" + config.unid;
        return TS.Org.getDomainDetail(config);
    },
    
    /* 通过Unid获取Domain的详细信息
     * @param {Object} config 配置选项，不能为空
     * @config {String} url 获取Domain详细信息的基本url，相关参数请放到参数parameters中
     * @config {String} async 是否使用异步方式执行Ajax(默认为true)
     * @config {Object} parameters 请求的参数，如{unid:myUnid}
     * @config {Function} callback 回调函数,第一个参数为包含Domain详细信息的json对象或null,
     *		json对象属性与Domain对应
     * @config {Boolean} addTimeStamp 是否在url后添加时间戳(默认为true)
     * @return {Object} 如果config.async==true,返回null；否则返回与config.callback的第一个参数相同的对象
     */
    getDomainDetail: function(config){
        if (!config) 
            return;
        var _config = {};
        TS.apply(_config, config, {
            async: true,
            addTimeStamp: true
        });
        
        // 添加时间戳
        if (_config.addTimeStamp == true && _config.url.indexOf("timeStamp") < 0) {
            if (_config.url.indexOf("?") > -1) 
                _config.url += "&timeStamp=" + new Date().getTime();
            else 
                _config.url += "?timeStamp=" + new Date().getTime();
        }
        
        var _result = null;
        new Ajax.Request(_config.url, {
            method: 'post',
            asynchronous: _config.async,
            parameters: _config.parameters ||
            {},
            onSuccess: function(transport){
                var response = eval("(" + transport.responseText + ")");
                if (response.result != false) {
                    if (_config.async == false) 
                        _result = response;
                    if (typeof _config.callback == "function") {
                        _config.callback.call(transport, response);
                    }
                }
                else {
                    MsgBox(response.msg, '获取对象信息', g_OkOnly, g_ErrorIcon);
                }
            },
            onFailure: function(transport){
                MsgBox('获取对象信息过程出现异常!', '获取对象信息', g_OkOnly, g_ErrorIcon);
            }
        });
        return _result;
    },
    /*
     * 选择单个用户
     */
    selectSingleUser: function(){
        var strUrl = TS.rootPath + "Organize/select/UserSelect.aspx?singleSelect=true";
        var option = "status:no;resizable:no;unadorne:yes;help:no;scroll:no;dialogHeight:400px;dialogWidth:490px";
        var domains = showModalDialog(strUrl, "", option);
        if (domains && domains[0]) {
            return domains[0];
        }
        else {
            return null;
        }
    },
    /*
     * 选择多个用户
     */
    selectMultiUser: function(){
        var strUrl = TS.rootPath + "Organize/select/UserSelect.aspx?singleSelect=false";
        var option = "status:no;resizable:no;unadorne:yes;help:no;scroll:no;dialogHeight:400px;dialogWidth:490px";
        var domains = showModalDialog(strUrl, "", option);
        if (domains && domains[0]) {
            return domains;
        }
        else {
            return null;
        }
    },
    noUsed: false
};

