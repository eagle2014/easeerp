<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlaceTree.aspx.cs" Inherits="TSCommon_Web.WareHouse.Dlg.PlaceTree" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>选择地点</title>
    <link rel="stylesheet" type="text/css" href="~/TS_platform/ext/resources/css/ext-all.css" />
</head>
<script src="<%=ContextPath %>/TS_platform/js/TS-Include-ViewPage.js?rootPath=<%=ContextPath %>/"
    type="text/javascript"></script>
<body  class="egd-body" onload="initPage();">
    <form id="thisPage" runat="server">
    <div class="egd-page-overflow egd-tree-wrap" style="background-color: #FFFFFF;">
    <div id="treeContainer" style="height: 100%; width: 100%;"></div>
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
var curNode;
function initPage(){
    var treeLoader=new TS.Tree.TreeLoader({
	    dataUrl:TS.rootPath+'placeAction.do?action=GetPlaceNodes',
	    loadexception:function(treeLoader,node,response){alert('加载树失败');}
	    });
	    this.tree=new TS.Tree.TreePanel('treeContainer',{
	    loader:treeLoader,
	    requestMethod:'get',
	    rootVisible:true
	    });
	    var root=new TS.Tree.AsyncTreeNode({text:"总部",id:'-1'});
	    //
	    this.tree.setRootNode(root);
	    this.tree.render();
	    this.tree.on('click',clickNode);
	    function clickNode(node,e){
	    //获取节点的属性
	    curNode=node;	    
	    }
}
function DlgOkFn()
{
    var returnValue;
    if(curNode.id==-1){
        TS.Msg.alert({msg:"请选择地点！"});
       return; 
    }
    returnValue={
    ID:curNode.attributes._id,
    Name:curNode.text
    }
    return returnValue;
} 
</script>
