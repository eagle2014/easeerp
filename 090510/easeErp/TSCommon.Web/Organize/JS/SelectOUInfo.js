/*
 * 返回选中值的函数
 */
function DlgOkFn(){
    if(curSelectNode == null){
        TS.Msg.msgBox({title:"选择组织信息", msg:"请先选择相应的组织信息！", iconCls: TS.Msg.IconCls.WARNING});
	    return null;
    }
    
    if(my.ouType.length > 0){
        if(curSelectNode.attributes['Type'] != my.ouType){
            TS.Msg.msgBox({title:"选择组织信息", msg:"必须选择" + (my.ouType == "DW" ? "单位" : "部门") + "信息！", iconCls: TS.Msg.IconCls.WARNING});
	        return null;
        }
    }
	
    var result = {
        Unid:curSelectNode.id, 
        Name:curSelectNode.text,
        FullName:curSelectNode.attributes['FullName'],
        Code:curSelectNode.attributes['Code'], 
        FullCode:curSelectNode.attributes['FullCode'],
        Type:curSelectNode.attributes['Type'],
        FullCode:curSelectNode.attributes['FullCode'],
        UnitUnid:curSelectNode.attributes['UnitUnid'],
        UnitName:curSelectNode.attributes['UnitName'],
        UnitFullName:curSelectNode.attributes['UnitFullName'],
        UnitFullCode:curSelectNode.attributes['UnitFullCode']
    };
    return result;
}

var curSelectNode = null;
var thisPage = {
    init: function(){
        thisPage.initTree(); 
    },
    
    /* 
     * 初始化导航树
     */
     initTree: function(){
        var dataUrl = TS.rootPath + 'ouInfoAction.do?action=GetDepartmentNodes&link=n';
        dataUrl += '&singleClickExpand=' + my.singleClickExpand; 
        if(my.ouType.length > 0)
            dataUrl += '&ouType=' + my.ouType; 
        if(my.type.length > 0)
            dataUrl += '&type=' + my.type; 

        /* 树加载器 */
        var navTreeLoader = new Ext.tree.TreeLoader({
	        preloadChildren: false,
	        clearOnLoad: true,
	        url: dataUrl,
	        baseAttrs: { uiProvider: Ext.tree.TreeCheckNodeUI } //添加 uiProvider 属性
        });
        navTreeLoader.on("loadexception",function(treeLoader , node, response){
            alert("未能成功加载OU列表,请重新登录！");
        });
        /* 树的根节点 */
        var navTreeRoot = new Ext.tree.AsyncTreeNode({
            text: (my.rootOUName.length > 0 ? my.rootOUName : '组织架构信息'),
            id: (my.rootOUUnid.length > 0 ? my.rootOUUnid : '-1'),
            expanded:true,
            singleClickExpand: my.singleClickExpand,
            iconCls: my.rootIconCls
        });
        /* 展开第一个模块 */
        navTreeRoot.on("expand", function(root){
            var firstNode = root.item(0);
            if(firstNode) firstNode.expand(false,false); 
        });

        var tree = new Ext.tree.TreePanel({
            el:'treeContainer',
            border:true,
            rootVisible: my.rootVisible,
            lines:false,
            autoScroll:false,
            containerScroll: false,
            border: false,
            autoHeight: true,
            animate: false,
            loader: navTreeLoader,
            root: navTreeRoot
        });
        tree.render();

        // 点击树节点的处理函数
        tree.on('click', thisPage.clickTreeNode);
    },
    
    clickTreeNode: function(node, e){
	    my.selectedValue = node.id;
	    curSelectNode = node;
    }
};