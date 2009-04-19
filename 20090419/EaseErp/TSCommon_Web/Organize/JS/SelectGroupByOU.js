var groupDomains = my.groupsJson;
/*
 * 返回选中值的函数
 */
function DlgOkFn(){
    var optionsObj = $("Options");
    if(optionsObj.selectedIndex == -1){
        TS.Msg.msgBox({title:"选择岗位", msg:"请先选择岗位！", iconCls: TS.Msg.IconCls.WARNING});
	    return null;
    }
	
    if(my.singleSelect){
        var value = optionsObj.value;
        for(var i = 0; i < groupDomains.length; i++){
            if(value == groupDomains[i].Unid){
                return groupDomains[i];
            }
        }
    }else{
        var _options = optionsObj.options;
        var selectedDomains = new Array();
        for(var i = 0; i < _options.length; i++){
            if(_options[i].selected){
                selectedDomains.push(groupDomains[i]);
            }
        }
        return selectedDomains;
    }
}

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
	        url: dataUrl
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
            rootVisible:false,
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
	    var selOptionObj = $("Options");
	    selOptionObj.length = 0;
	    var strUrl = TS.rootPath + "groupAction.do?action=FindGroupByOU&ouUnid=" + node.id;
	    if(my.groupType.length > 0) 
	        strUrl += "&groupType=" + my.groupType;
	    new Ajax.Request(strUrl, {	
	        method: 'get', 
	  	    onSuccess: function(transport){
	  		    groupDomains = eval("(" + transport.responseText + ")");
			    //alert(transport.responseText);
			    for(var i = 0; i < groupDomains.length; i++){
				    selOptionObj.options[selOptionObj.length] = new Option(groupDomains[i].Name, groupDomains[i].Unid);
			    }
		    },
		    onFailure: function(transport){
                TS.Msg.msgBox({msg:"加载岗位信息出错！", iconCls: TS.Msg.IconCls.ERROR});
		    }
        });
    }
};