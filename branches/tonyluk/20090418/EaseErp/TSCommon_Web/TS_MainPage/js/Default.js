/* 
 * 主页相关
 * 2007-12-01 Tony
 */
var globalModal = false; // 对话框默认是否使用遮罩效果
/* 树加载器 */
var navTreeLoader = new Ext.tree.TreeLoader({
    preloadChildren: false,
    clearOnLoad: true,
    url: "../privilegeAction.do?action=GetPrivilegeNodes"
});
navTreeLoader.on("loadexception", function(treeLoader, node, response){
    alert("未能成功加载导航功能列表,请重新登录！");
});
/* 树的根节点 */
var navTreeRoot = new Ext.tree.AsyncTreeNode({
    text: sys_title,
    id: 'root',
    expanded: true,
    singleClickExpand: true
});
/* 展开第一个模块 */
navTreeRoot.on("expand", function(root){
    var firstNode = root.item(0);
    if (firstNode) 
        firstNode.expand(false, false);
});

/*
 * 导航树面板定义
 */
_NavPanel = function(){
    _NavPanel.superclass.constructor.call(this, {
        id: 'nav-panel',
        region: 'center',
        title: '操作中心',
        border: true,
        rootVisible: false,
        lines: false,
        autoScroll: true,
        animCollapse: false,
        animate: false,
        collapseMode: 'mini',
        loader: navTreeLoader,
        root: navTreeRoot,
        collapseFirst: false,
        iconCls: 'egd-icon-nav',
        cmargins: '0 0 0 0'
    });
    
    this.getSelectionModel().on('beforeselect', function(sm, node){
        return node.isLeaf();
    });
};

Ext.extend(_NavPanel, Ext.tree.TreePanel, {
    /*
     * 选中导航树中对应的节点
     */
    selectNode: function(nodePath){
        if (nodePath) {
            this.selectPath(nodePath);
        }
    }
});

/*
 * 单个窗口面板定义
 */
WinPanel = Ext.extend(Ext.Panel, {
    closable: true,
    autoScroll: true,
    iconCls: 'egd-icon-default'
});

/*
 * 多窗口面板定义
 */
_MainPanel = function(){
    _MainPanel.superclass.constructor.call(this, {
        id: 'main-panel',
        region: 'center',
        resizeTabs: true,
        minTabWidth: 135,
        tabWidth: 135,
        plugins: new Ext.ux.TabCloseMenu(),
        enableTabScroll: true,
        deferredRender: false,
        margins: '0 0 5 0',
        activeTab: 0,
        items: [{
            id: mainPage_firstPageTabId,
            title: mainPage_firstPageTabTitle,
            fitToFrame: true,
            html: {
                tag: 'iframe',
                id: mainPage_firstPageTabId + 'IFrame',
                src: mainPage_firstPage,
                frameborder: '0',
                scrolling: 'auto',
                style: 'width: 100%; height: 100%; margin: 0px; padding: 0px;'
            },
            autoScroll: true,
            iconCls: 'egd-icon-firstTab'
        }]
    });
};

Ext.extend(_MainPanel, Ext.TabPanel, {
    initEvents: function(){
        _MainPanel.superclass.initEvents.call(this);
        //this.body.on('click', function(){alert("999");}, this);
    },
    
    openWindow: function(config){
        var id = config.id || 'EWIN' + new Date().getTime();
        var tab = this.getComponent(id);
        if (tab) {
            // 激活现存的窗口
            this.setActiveTab(tab);
            hideMsg();
        }
        else {
            // 限制窗口的数量
            //alert(this.items.length);
            if (sys_maxWindowCount <= this.items.length) {
                //if(Ext.isIE)
                //    MsgBox(sys_maxWindowMsg);
                //else
                //    alert(sys_maxWindowMsg);
                
                msgBox({
                    title: "系统提示",
                    msg: sys_maxWindowMsg,
                    iconCls: TS.Msg.IconCls.WARN
                });
                return;
            }
            wait({
                msg: '正在加载文档，请稍候...',
                iconCls: TS.Msg.IconCls.OPENING
            });
            
            // 创建新的窗口并激活
            var _html = {
                tag: 'iframe',
                id: id + 'IFrame',
                src: config.url || "",
                frameborder: '0',
                scrolling: 'auto',
                style: 'width: 100%; height: 100%; margin: 0px; padding: 0px;background-color:#D0DEF4;'
            };
            var winDefaultConfig = {
                id: id,
                title: "无标题",
                html: _html,
                opener: this.getActiveTab().getId(),
                refresh: true,
                tabTip: config.title,
                addTimeStamp: true
            };
            var winConfig = {};
            Ext.apply(winConfig, config, winDefaultConfig);
            if (winConfig.addTimeStamp == true) 
                winConfig.url = TS.addTimeStamp(winConfig.url); // 添加时间戳
            winConfig.url = TS.getAbsoluteUrl(winConfig.url); // 将相对路径转换为绝对路径
            var p = this.add(new WinPanel(winConfig));
            this.setActiveTab(p);
            
            if (!sys_useMDI) {
                this.hideTabStripItem(id);// 单窗口处理
            }
        }
        //alert((config.title || "无标题") + ";" + config.url);
    }
});

var mainPanel;
var navPanel;
var viewport;
Ext.onReady(function(){
    Ext.QuickTips.init();
    
    mainPanel = new _MainPanel();
    // 切换页签后的处理函数
    mainPanel.on('tabchange', function(tp, tab){
        navPanel.selectNode(tab.navPath);
    });
    // 关闭窗口前的处理函数
    mainPanel.on('beforeremove', function(tp, tab){
        //alert(tab.id + ";" + tab.title); 
    });
    // 关闭窗口后的处理函数
    mainPanel.on('remove', function(tp, tab){
        var activeTab = this.getActiveTab();
        var openerTab = (tab.opener ? this.getComponent(tab.opener) : null);
        if (openerTab && tab.refresh == true) {
            // 刷新父窗口
            var iframeEl = $(openerTab.getId() + "IFrame");
            Try.these(function(){
                iframeEl.contentWindow.refresh();
            }, function(){
                iframeEl.contentWindow.thisPage.refresh();
            }, function(){
                iframeEl.contentWindow.dataGrid.reload();
            }, function(){
                iframeEl.src = iframeEl.src;
            });
        }
    });
    
    navPanel = new _NavPanel();
    // 点击树节点的处理函数
    navPanel.on('click', function(node, e){
        if (node.isLeaf()) {
            e.stopEvent();
            mainPanel.openWindow({
                id: node.id,
                title: node.text,
                url: node.attributes.href,
                navPath: node.getPath(),
                refresh: false
            });
        }
    });
    var leftPanelItems;
    if (mainPage_showSearchPage) {
        leftPanelItems = [navPanel, {
            id: 'search-panel',
            region: 'south',
            contentEl: 'search',
            title: '搜索',
            split: true,
            collapsible: true,
            height: 150,
            border: true,
            iconCls: 'egd-icon-search'
        }];
    }
    else {
        leftPanelItems = [navPanel]
        //var search = Ext.get('search');
        //if(search) search.remove();
    }
    
    viewport = new Ext.Viewport({
        layout: 'border',
        items: [new Ext.BoxComponent({ // raw
            region: 'north',
            el: 'hd',
            height: 80
        }), {
            region: 'south',
            contentEl: 'footer',
            height: 20,
            border: false,
            margins: '0 0 0 0'
        }, {
            region: 'west',
            id: 'left-panel',
            split: true,
            width: 180,
            minSize: 150,
            maxSize: 400,
            collapsible: true,
            margins: '0 0 5 0',
            layout: 'border',
            layoutConfig: {
                animate: true
            },
            border: false,
            items: leftPanelItems
        }, mainPanel]
    });
    
    setTimeout(function(){
        if (!sys_useMDI) 
            mainPanel.hideTabStripItem(mainPage_firstPageTabId);// 单窗口处理
        Ext.get('loading').remove();
        Ext.get('loading-mask').fadeOut({
            remove: true
        });
    }, 250);
    
    if (mainPage_showSearchPage) 
        $("searchFrame").src = mainPage_searchPage;
    
    if (toOpenUrl.length > 0) 
        openWindow({
            url: toOpenUrl,
            title: toOpenUrlTitle,
            tabTip: toOpenUrlTitle,
            id: "EgdAutoOpenTab",
            refresh: false
        });
});

/**
 * 打开一个窗口，如果指定的窗口已经存在则激活该窗口
 *
 * @param {Object} config
 * @config {String} title 窗口的标题，默认为tabID
 * @config {String} url 窗口的URL
 * @config {String} id 可选配置：窗口对应的ID，默认为'EWIN' + new Date().getTime()
 * @config {Boolean} closable 可选配置：窗口是否可被关闭，默认为true
 * @config {String} tabTip 可选配置：窗口的提示信息
 * @config {Boolean} refresh 可选配置：标识当关闭当前窗口后,是否应该通知父窗口需要执行更新操作 (默认为true)
 * @config {String} iconCls 可选配置：页签图标对应的样式名
 */
function openWindow(config, url){
    if (typeof config == "string") 
        config = {
            title: config,
            url: url
        };// 兼容旧版本方法的参数
    mainPanel.openWindow(config);
}

/**
 * 关闭一个窗口，如果没有指定任何参数，则关闭当前窗口
 * @param {Object|String} config 要关闭的窗口对象或其id
 */
function closeWindow(winId){
    var tab;
    if (winId) {
        tab = mainPanel.getItem(winId);
    }
    else {
        tab = mainPanel.getActiveTab();
    }
    if (tab) 
        mainPanel.remove(tab, true);
}

/**
 * 关闭当前激活的窗口
 */
function closeMe(){
    closeWindow();
}

/**
 * 更新当前激活的窗口对应的iframe的src为指定的url
 */
function updateMe(newUrl){
    var activeTab = mainPanel.getActiveTab();
    if (!activeTab) 
        return;
    
    var activeTabIFrame = $(activeTab.getId() + "IFrame")
    if (!activeTabIFrame) 
        return;
    if (newUrl) 
        activeTabIFrame.src = newUrl;
}

/**
 * 通用信息框
 *
 * @param {Object} config
 * @config {String} title 窗口的标题
 * @config {String} msg 提示信息
 * @config {Number} buttons 可选配置：按钮配置，参考TS.Msg.Buttons常数，默认为TS.Msg.Buttons.OK
 * @config {String} iconCls 可选配置，参考TS.Msg.IconCls常数，默认为TS.Msg.IconCls.INFO
 * @config {Function} onOk 可选配置：点击ok按钮的回调函数，只有当btnType中包含ok按钮时有效
 * @config {Function} onCancel 可选配置：点击cancel按钮的回调函数，只有当btnType中包含cancel按钮时有效
 * @config {Function} onYes 可选配置：点击yes按钮的回调函数，只有当btnType中包含yes按钮时有效
 * @config {Function} onNo 可选配置：点击no按钮的回调函数，只有当btnType中包含no按钮时有效
 * @config {Function} onClose 可选配置：信息框关闭后的回调函数，函数第一个参数为按钮的id，即ok|cancel|yes|no
 */
function msgBox(config){
    if (typeof config == "string") 
        config = {
            msg: config
        };
    
    if (!typeof config == "object") 
        return;
    
    var _config = {};
    var defaultConfig = {
        title: '系统提示',
        buttons: Ext.MessageBox.OK,
        icon: 'egd-msgBox-info',
        fn: function(btn){
            if (btn == 'ok') {
                if (typeof config.onOk == "function") 
                    config.onOk.call();
            }
            else 
                if (btn == 'cancel') {
                    if (typeof config.onCancel == "function") 
                        config.onCancel.call();
                }
                else 
                    if (btn == 'yes') {
                        if (typeof config.onYes == "function") 
                            config.onYes.call();
                    }
                    else 
                        if (btn == 'no') {
                            if (typeof config.onNo == "function") 
                                config.onNo.call();
                        }
            
            if (typeof config.onClose == "function") 
                config.onClose.call(this, btn);
        },
        title: '系统提示',
        modal: globalModal
    };
    Ext.apply(_config, config, defaultConfig);
    if (_config.iconCls) 
        _config.icon = _config.iconCls;
    
    //config
    if (!mainPage_showMaskMsg) {
        if (MsgBox(_config.msg || '', _config.title || '')) {
            Try.these(function(){
                config.onOk.call();
            }, function(){
                config.onYes.call();
            });
        }
        else {
            Try.these(function(){
                config.onCancel.call();
            }, function(){
                config.onNo.call();
            });
        }
        return;
    }
    Ext.MessageBox.show(_config);
}

/**
 * 确认信息框
 *
 * @param {Object} config
 * @config {String} title 窗口的标题
 * @config {String} msg 提示信息
 * @config {Function} onYes 可选配置：点击yes按钮的回调函数
 * @config {Function} onNo 可选配置：点击no按钮或关闭按钮的回调函数
 * @config {Function} onClose 可选配置：信息框关闭后的回调函数，函数第一个参数为按钮的id，即yes|no
 */
function confirmBox(config){
    if (!typeof config == "object") 
        return;
    
    var _config = {};
    var fn = function(btn){
        if (btn == 'yes') {
            if (typeof config.onYes == "function") 
                config.onYes.call();
        }
        else 
            if (btn == 'no') {
                if (typeof config.onNo == "function") 
                    config.onNo.call();
            }
        
        if (typeof config.onClose == "function") 
            config.onClose.call(this, btn);
    };
    
    //config
    if (!mainPage_showMaskMsg) {
        if (ConfirmBox(config.msg || '', config.title || '')) {
            Try.these(function(){
                config.onYes.call();
            });
        }
        else {
            Try.these(function(){
                config.onNo.call();
            });
        }
        return;
    }
    
    Ext.MessageBox.confirm(config.title || '系统提示', config.msg || '', fn);
}

/**
 * 警告信息框
 *
 * @param {Object} config
 * @config {String} title 窗口的标题
 * @config {String} msg 提示信息
 * @config {Function} onOk 可选配置：点击ok按钮的回调函数
 * @config {Function} onClose 可选配置：信息框关闭后的回调函数，函数第一个参数为按钮的id，即ok|cancel
 */
function alarm(config){
    if (!typeof config == "object") 
        return;
    
    var _config = {};
    var fn = function(btn){
        if (btn == 'ok') 
            if (typeof config.onOk == "function") 
                config.onOk.call();
        
        if (typeof config.onClose == "function") 
            config.onClose.call(this, btn);
    };
    
    //config
    if (!mainPage_showMaskMsg) {
        if (MsgBox(config.msg || '', config.title || '', g_OkOnly, g_CriticalIcon)) {
            Try.these(function(){
                config.onOk.call();
            });
        }
        return;
    }
    Ext.MessageBox.alert(config.title || '系统提示', config.msg || '', fn);
}


/**
 * 获取输入信息的信息框
 *
 * @param {Object} config
 * @config {String} title 窗口的标题
 * @config {String} msg 提示信息
 * @config {Function} onOk 可选配置：点击ok按钮的回调函数，第一个参数为输入的文本信息
 * @config {Function} onCancel 可选配置：点击cancel按钮或关闭按钮的回调函数，第一个参数为输入的文本信息
 * @config {Function} onClose 可选配置：信息框关闭后的回调函数，函数第一个参数为按钮的id，即ok|cancel，第二个参数为输入的文本信息
 * @config {Boolean} multiline 可选配置：是否为多行提示信息，默认为true
 */
function prompt(config){
    if (!(typeof config == "object")) 
        return;
    
    var _config = {};
    var defaultConfig = {
        multiline: true,
        fn: function(btn, text){
            if (btn == 'ok') {
                if (typeof config.onOk == "function") {
                    // 检测是否已经填写信息
                    if (text && text.length > 0) {
                        config.onOk.call(this, text);
                    }
                    else {
                        Ext.MessageBox.alert(config.title, "必须输入相应的信息！", function(btn){
                            prompt(config);
                        });
                    }
                }
            }
            else 
                if (btn == 'cancel') {
                    if (typeof config.onCancel == "function") 
                        config.onCancel.call(this, text);
                }
            
            if (typeof config.onClose == "function") 
                config.onClose.call(this, btn, text);
        },
        modal: false,
        buttons: Ext.MessageBox.OKCANCEL
    };
    Ext.apply(_config, config, defaultConfig);
    //alert(_config.multiline);
    if (_config.multiline == true) {
        _config.width = 300;
        Ext.MessageBox.show(_config);
    }
    else {
        Ext.MessageBox.prompt(_config.title || '', _config.msg || '', _config.fn);
    }
}

var startWaitTime = -1;
/**
 * 等待信息框
 *
 * @param {Object} config
 * @config {String} msg 提示信息
 * @config {String} progressText 动画的背景文字
 * @config {String} icon 图片的样式名
 * @config {Number} interval 动画的时间间隔，以毫秒为单位
 * @config {Number} closeTime 自动隐藏的等待时间，以毫秒为单位
 */
function wait(config){
    if (!(config)) 
        config = {};
    var _config = {
        msg: "正在处理，请稍候...",
        progressText: "",
        iconCls: "egd-msgBox-wait",
        width: 300,
        wait: true,
        lazyHide: false,
        modal: globalModal,
        must: mainPage_showMaskMsg
    };
    if (config.interval) 
        config.waitConfig = {
            interval: config.interval
        };
    else 
        config.waitConfig = {
            interval: 200
        };
    
    Ext.apply(_config, config);
    
    //config
    if (!_config.must) {
        return;
    }
    
    if (_config.iconCls) 
        _config.icon = _config.iconCls;
    Ext.MessageBox.show(_config);
    if (config.closeTime) {
        setTimeout(function(){
            Ext.MessageBox.hide();
        }, config.closeTime);
    }
    
    if (_config.lazyHide == true) 
        startWaitTime = new Date().getTime();
    else 
        startWaitTime = -1;
}

function hideMsg(config){
    var now = new Date().getTime();
    //alert(startWaitTime);
    if ((startWaitTime > 0) && ((now - startWaitTime) < 1000)) {
        setTimeout(function(){
            Ext.MessageBox.hide();
            startWaitTime = -1;
            if (config && typeof config == "function") 
                config.call(this);
        }, 1000 - (now - startWaitTime));
    }
    else {
        Ext.MessageBox.hide();
        startWaitTime = -1;
    }
}


/* 
 * 隐藏指定id的对话框
 */
function hideDialog(dlgId){
    for (var i = 0; i < edgDlgs.length; i++) {
        if (edgDlgs[i].id == dlgId) {
            edgDlgs[i].hide();
            break;
        }
    }
}

/* 
 * 关闭指定id的对话框
 */
function closeDialog(dlgId){
    for (var i = 0; i < edgDlgs.length; i++) {
        //alert(dlgId + ";" + edgDlgs.length);
        if (edgDlgs[i].id == dlgId) {
            edgDlgs[i].hide();
            edgDlgs[i].close();
            edgDlgs.splice(i, 1);
            break;
        }
    }
}

var edgDlgs = {}; // 对话框缓存
/**
 * 弹出对话框
 *
 * @param {Object} config 对话框配置
 * @config {String} id 对话框ID,同时也是缓存对话框的键值
 * @config {String} title 对话框标题
 * @config {String} url 对话框内容的url
 * @config {Number} width 对话框宽度
 * @config {Number} height 对话框高度
 * @config {Boolean} modal 是否为模式对话框，默认为true
 * @config {Boolean} showCancelButton 是否自动在最后追加“取消”按钮，默认为true
 * @param {Function} onOk 可选参数：回调函数，函数的第一个参数为点击确认按钮所执行函数的返回值
 * @param {Array} buttons 可选参数：自定义对话框的所有按钮
 */
function createDialog(config){
    if (typeof config != "object") 
        return null;
    if (!config.id) 
        config.id = "defaultDlgId"; // 创建默认的对话框id
    var cacheDlg = edgDlgs[config.id];
    if (!cacheDlg) { // 创建新的对话框
        //alert("newDlg:id=" + config.id);
        createDlgInner(config);
        // 显示对话框
        edgDlgs[config.id].show();
        return config.id;
    }
    else { // 显示已缓存的对话框
        //alert("cacheDlg:id=" + config.id);
        reConfigDlg(config, cacheDlg)
        
        cacheDlg.center();
        cacheDlg.show();
        cacheDlg.toFront();
        
        return config.id;
    }
}

function reConfigDlg(config, dlg){
    // 更新iframe的src(如果url被修改了)
    var cIFrame = $(dlg.id + "CIFrame");
    //alert(config.url);
    
    // 判断是否需要更新iframe
    var lastIndex = cIFrame.src.lastIndexOf(config.url);
    if (lastIndex == -1 || (cIFrame.src.substr(lastIndex) != config.url)) { // url变了的处理。不能用cIFrame.src != config.url,因为firefox会在src后面自动附件http://...
        cIFrame.src = (config.url || "");
    }
    else 
        if (config.refresh == true) { // 强制更新
            Try.these(function(){
                cIFrame.contentWindow.refresh();
            }, // 调用iframe内的refresh函数
 function(){
                iframeEl.src = (config.url || "");
            } // 强制更换iframe的src的url
);
        }
    
    // 更新对话框的参数
    var dlgSize = dlg.getSize();
    if (dlg.title != config.title) 
        dlg.setTitle(config.title || "无标题对话框");
    if (dlgSize.width != config.width) 
        dlg.setWidth(config.width || 400);
    if (dlgSize.height != config.height) 
        dlg.setHeight(config.height || 250);
    if (dlg.modal != config.modal) 
        dlg.modal = (config.modal || globalModal);
    if (dlg.iconCls != config.iconCls) 
        dlg.setIconClass(config.iconCls || "egd-icon-dlg");
    if (dlg.copyTo != config.copyTo) 
        dlg.copyTo = config.copyTo;
    
    // 更新按钮配置
    if (dlg.onOk != config.onOk) 
        dlg.onOk = config.onOk;
    if (dlg.afterOk != config.afterOk) 
        dlg.afterOk = config.afterOk;
    if (dlg.showCancelButton != config.showCancelButton) 
        dlg.showCancelButton = config.showCancelButton;
    
    var buttons = buildDlgButtons(config);
    if (buttons.length > 0) {
        dlg.buttons = buttons;
    }
    else {
        delete dlg.buttons;
    }
    //alert("XXXX:" + dlg.buttons);
}

function createDlgInner(config){
    if (!config) 
        config = {};
    Ext.applyIf(config, {
        showCancelButton: true
    });
    
    // 生成对话框按钮的配置
    
    // 创建对话框
    Ext.applyIf(config, {
        modal: globalModal,
        title: "无标题对话框",
        width: 400,
        height: 250,
        plain: false, // 是否显示透明的背景
        shim: true, // 是否使用iframe作为底罩
        shadow: false, // 是否显示对话框边框的阴影
        constrain: false, // 是否限制窗口的移动范围
        resizable: false, // 是否允许用户改变窗口的大小
        closeAction: 'hide',
        html: {
            tag: 'iframe',
            id: config.id + "CIFrame",
            src: config.url || "",
            frameborder: '0',
            style: 'width: 100%; height: 100%; margin: 0px; padding: 0px;'
        },
        iconCls: "egd-icon-dlg"
    });
    var buttons = buildDlgButtons(config);
    if (buttons.length > 0) 
        config.buttons = buttons;
    edgDlgs[config.id] = new Ext.Window(config);
    
    if (typeof config.closeFn == "function") {
        edgDlgs[config.id].on("close", function(){
            edgDlgs[config.id].closeFn.call(this, null);
        });
    }
    if (typeof config.hideFn == "function") {
        edgDlgs[config.id].on("hide", function(){
            edgDlgs[config.id].hideFn.call(this, null);
        });
    }
}

// 生成对话框按钮的配置
function buildDlgButtons(config){
    var buttons = config.buttons;
    if (!buttons) { // 使用默认的按钮配置
        if (typeof config.onOk == "function") {
            okBtn = {
                id: config.id + '_okButton',
                text: '确定',
                handler: buildDlgBtnHandler({
                    dlgId: config.id,
                    fnName: 'DlgOkFn',
                    btnIndex: 0,
                    keepShow: false
                }),
                onOk: config.onOk
            };
            if (typeof config.afterOk == "function") 
                okBtn.afterOk = config.afterOk;
            buttons = [okBtn];
        }
        else {
            buttons = [];
        }
    }
    else { // 使用自定义的按钮配置
        var btn;
        for (var i = 0; i < buttons.length; i++) {
            btn = buttons[i];
            btn.handler = buildDlgBtnHandler({
                dlgId: config.id,
                fnName: btn.fnName,
                btnIndex: i,
                keepShow: buttons[i].keepShow == true
            });
        }
    }
    
    // 追加“取消按钮”
    if (config.showCancelButton == true) {
        // 默认的“取消”按钮
        var cancleBtn = {
            text: '取消',
            handler: function(){
                edgDlgs[config.id].hide();
            }
        };
        buttons.push(cancleBtn);
    }
    return buttons;
}

/**
 * private 创建对话框按钮处理函数
 *
 * @param {Object} config 对话框按钮配置
 * @config {String} dlgId 对话框ID
 * @config {String} fnName 函数名
 * @config {Number} btnIndex 按钮的索引号
 * @return {Function} 构建好的处理函数
 */
function buildDlgBtnHandler(config){
    var btnHandler = function(){
        // 获取函数的返回值
        var contentWindow = $(config.dlgId + "CIFrame").contentWindow;
        //alert("start");
        var egdDlgFn = eval('(contentWindow.' + config.fnName + ')');
        //alert(config.fnName);
        if (typeof egdDlgFn == "function") {
            // 获取返回值
            var value = egdDlgFn.call(this, value);
            
            if (value) { // 只有存在返回值的情况下才关闭对话框并调用回调函数
                if (config.keepShow != true) // 成功后强制保留对话框显示
                    edgDlgs[config.dlgId].hide();
                var _onOk = edgDlgs[config.dlgId].buttons[config.btnIndex].onOk;
                if (typeof _onOk == "function") {
                    _onOk.call(null, value);
                    var _afterOk = edgDlgs[config.dlgId].buttons[config.btnIndex].afterOk;
                    if (typeof _afterOk == "function") {
                        _afterOk.call(null, value);
                    }
                }
            }
        }
        else {
            alert("对话框的IFrame内没有定义返回函数“" + config.fnName + "”！");
        }
    }
    return btnHandler;
}

/**
 * 引发对话框按钮的点击处理事件
 *
 * @param {Number} btnIndex 按钮的索引号
 * @param {String} dlgId 可选参数：对话框ID,默认为defaultDlgId
 */
function fireDialogButtonEvent(btnIndex, dlgId){
    var cacheDlg = edgDlgs[dlgId];
    if (!cacheDlg) 
        return false;
    
    var buttons = cacheDlg.buttons;
    if (!buttons || buttons.length == 0) 
        return false;
    
    var button = buttons[btnIndex];
    if (!button) 
        return false;
    if (typeof button.handler != 'function') 
        return false;
    
    var result = button.handler.call(null);
    return result;
}

/**
 * 是否使用多窗口主页
 */
function isMDI(){
    return sys_useMDI;
};

/**
 * 显示日历进行选择
 *
 * @param {Object} config 配置
 * @config {String} deep 控件所在页面相对于主页的层次深度，只支持0(为主页面)、1(为主页的子页面)、2(为主页的子页面的子页面)三种深度
 * @config {String} fieldId 控件的id
 * @config {String} left 控件在其所在页面的X位置
 * @config {String} top 控件在其所在页面的Y位置
 * @config {String} width 控件的宽度
 * @config {String} height 控件的高度
 * @config {String} className 控件的样式名
 * @config {String} format 日期的显示格式
 * @config {Function} onOk 选择日期/时间信息后的回调函数
 * @config {Function} onClear 点击“清空”按钮后的回调函数
 * @config {Function} onCancel 点击“关闭”按钮后的回调函数
 * @config {String} dlgId 可选：对话框的id
 * @config {Function} innerCallback private,内部回调函数
 */
function showEgdCalendar(config){
    //alert(TS.formatJSON(config));
    var dateObj = $('HiddenForDateCtrl');
    
    // 设置当前值
    if(dateObj.value != config.curValue)
        dateObj.value = config.curValue
    
    // 设置样式
    if(dateObj.className != config.className)
        dateObj.className = config.className;
    
    // 设置显示格式
    if(config.format && config.format.length > 0)
        dateObj.setAttribute("format",config.format);
    else
        dateObj.removeAttribute("format");
    
    // 定位日历控件
    var pIFrame,_id;
    if((typeof config.dlgId == "string") && config.dlgId.length > 0){   // 对话框
        _id = config.dlgId + 'IFrame';
	}else{                                                              // 窗口
        var curWin = mainPanel.getActiveTab();
        _id = curWin.id + 'IFrame';
	}
    pIFrame = $(_id);
    if(!pIFrame){
        alert("id=" + _id + '的iframe不存在！in Index.js,method=showEgdCalendar');
        return;
    }
    var page = Position.page(pIFrame);    // 获取iframe的X(page[0])、Y(page[1])位置
    var newLeft = config.left + page[0];    // X
    var newTop = config.top + page[1];      // Y
    dateObj.setStyle({
        left: newLeft,
        top: newTop
    });
    
    // 显示日历控件
    ECalendar.setDay(dateObj, dateObj, config);
}

/**
 * 隐藏当前显示的日历
 */
function hideEgdCalendar(){
    ECalendar.hide();
}