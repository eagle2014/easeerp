/*
 * 广州市忆科计算机系统有限公司 版权所有 2006- 2007 
 * 作者：kingchen chenjingcheng@ico.com.cn
 * 日期：2007-09-01
 * 功能：动态树
 */
 
if (typeof TS == "undefined") TS = {};

/**
 * 基本树
 */
TS.Tree = function(node){
	this.nodeHash = {};
	this.root = null;		
	if(node) 
	this.setRootNode(node);
	this.events={};
};

TS.Tree.prototype = {	
	pathSeparator: "/",
	
  	getRootNode : function(){
        return this.root;
    },
  	setRootNode:function(node){
  		this.root = node;
        node.ownerTree = this;
        node.isRoot = true;
        this.registerNode(node);
        return node;
  	},
  	getNodeById : function(id){
        return this.nodeHash[id];
    },
  	registerNode:function(node){
  		this.nodeHash[node.id] = node;
  	},

    unregisterNode : function(node){
        delete this.nodeHash[node.id];
    },
    toString : function(){
        return "[Tree"+(this.id?" "+this.id:"")+"]";
    },
    on :function(eventName,fn)
	{
		this.events[eventName] = fn.bind(this);
	},
	un : function(eventName){
		this.events[eventName] = null;
		delete this.events[eventName];
	},
	fireEvent : function(){
		var args = $A(arguments);
		var event = this.events[args.shift()];
		if(!event || typeof event!="function") return;		
        return event.apply(this,args);
    }
}

/**
 * 树控件,继承基本树结构
 */
TS.Tree.TreePanel = function(el,config) {	
	Object.extend(this,config||{});	
	TS.Tree.TreePanel.superclass.constructor.call(this);
	this.el = $(el);
	this.id = this.el.id;
	    
};

TS.extend(TS.Tree.TreePanel,TS.Tree,{
  	rootVisible : true,
	multiSelection : false,
	lines : true ,
    // private override
    setRootNode : function(node){
        TS.Tree.TreePanel.superclass.setRootNode.call(this, node);
        if(!this.rootVisible){
            node.ui = new TS.Tree.RootTreeNodeUI(node);
        }
        return node;
    },
    /**
     * 获取容器
     */
    getEl : function(){
        return this.el;
    },
    /**
     * 获取数据加载器
     */
    getLoader : function(){
        return this.loader;
    },
    /**
     * 展开所有节点
     */
    expandAll : function(){
        this.root.expand(true);
    },
    /**
     * 收起所有节点
     */
    collapseAll : function(){
        this.root.collapse(true);
    },
    /**
     * 获取选择的节点
     */
    getChecked : function(a, startNode){
        startNode = startNode || this.root;
        var r = [];
        var f = function(){
            if(this.attributes.checked){
                r.push(!a ? this : (a == 'id' ? this.id : this.attributes[a]));
            }
        }
        startNode.cascade(f);
        return r;
    },
    /**
     * 更加路径展开节点
     */
    expandPath : function(path, attr, callback){
        attr = attr || "id";
        var keys = path.split(this.pathSeparator);
        var curNode = this.root;
        if(curNode.attributes[attr] != keys[1]){ // invalid root
            if(callback){
                callback(false, null);
            }
            return;
        }
        var index = 1;
        var f = function(){
            if(++index == keys.length){
                if(callback){
                    callback(true, curNode);
                }
                return;
            }
            var c = curNode.findChild(attr, keys[index]);
            if(!c){
                if(callback){
                    callback(false, curNode);
                }
                return;
            }
            curNode = c;
            c.expand(false, false, f);
        };
        curNode.expand(false, false, f);
    },
    /**
     * 根据路径选择节点
     */
    selectPath : function(path, attr, callback){
        attr = attr || "id";
        var keys = path.split(this.pathSeparator);
        var v = keys.pop();
        if(keys.length > 0){
            var f = function(success, node){
                if(success && node){
                    var n = node.findChild(attr, v);
                    if(n){
                        n.select();
                        if(callback){
                            callback(true, n);
                        }
                    }else if(callback){
                        callback(false, n);
                    }
                }else{
                    if(callback){
                        callback(false, n);
                    }
                }
            };
            this.expandPath(keys.join(this.pathSeparator), attr, f);
        }else{
            this.root.select();
            if(callback){
                callback(true, this.root);
            }
        }
    },
    /**
     * 获取树的容器
     */
    getTreeEl : function(){
        return this.el;
    },
    /**
     * 树的渲染函数
     */
    render : function(){    	
        this.innerCt = this.el.appendChild(
        	$(document.createElement("ul")).addClassName("egd-tree-root-ct " + (this.lines ? "egd-tree-lines" : "egd-tree-no-lines")+(TS.isIE?" ext-ie":""))
        )
        this.root.render();
        if(!this.rootVisible){
            this.root.renderChildren();
        }
        return this;
    },
	unselect : function(){
		this.selNode = undefined;
	}
})

/**
 * 树的动态数据加载类
 */
TS.Tree.TreeLoader = function(config){
	this.baseParams = {};
    this.requestMethod = "POST";
    Object.extend(this,config||{});
};
TS.Tree.TreeLoader.prototype = {
	uiProviders : {},
    clearOnLoad : true,
    load : function(node, callback){
        if(this.clearOnLoad){
            while(node.firstChild){
                node.removeChild(node.firstChild);
            }
        }
        if(node.attributes.children){ // preloaded json children
            var cs = node.attributes.children;
            for(var i = 0, len = cs.length; i < len; i++){
                node.appendChild(this.createNode(cs[i]));
            }
            if(typeof callback == "function"){
                callback();
            }
        }else if(this.dataUrl){
            this.requestData(node, callback);
        }
    },
    getParams: function(node){
        var buf = [], bp = this.baseParams;
        for(var key in bp){
            if(typeof bp[key] != "function"){
                buf.push(encodeURIComponent(key), "=", encodeURIComponent(bp[key]), "&");
            }
        }
        buf.push("node=", encodeURIComponent(node.id));
        return buf.join("");
    },
	/**
	 * 请求数据
	 */
    requestData : function(node, callback){
    		var loader = this; 
            var o = {
                method: this.requestMethod,
                parameters: this.getParams(node) || '',
                timeout: 5000,
                onSuccess: function(xhr){
		            loader.handleResponse.call(loader, {
		                responseText: xhr.responseText,
		                responseXML : xhr.responseXML,
		                argument: {callback: callback, node: node}
		            });
		         },
                onFailure: function(xhr){
		            loader.handleFailure.call(loader, {
		                responseText: xhr.responseText,
		                responseXML : xhr.responseXML,
		                argument: {callback: callback, node: node}
		            });
		         }
            };
            new Ajax.Request(this.dataUrl||this.url, o);
    },

    /**
    * Override this function for custom TreeNode node implementation
    */
    createNode : function(attr){
        // apply baseAttrs, nice idea Corey!
        if(this.baseAttrs){
            TS.applyIf(attr, this.baseAttrs);
        }
        if(this.applyLoader !== false){
            attr.loader = this;
        }
        if(typeof attr.uiProvider == 'string'){
           attr.uiProvider = this.uiProviders[attr.uiProvider] || eval(attr.uiProvider);
        }
        return(attr.leaf ? new TS.Tree.TreeNode(attr) : new TS.Tree.AsyncTreeNode(attr));
    },

    processResponse : function(response, node, callback){
        var json = response.responseText;
        try {
            var o = eval("("+json+")");
            for(var i = 0, len = o.length; i < len; i++){
                var n = this.createNode(o[i]);
                if(n){
                    node.appendChild(n);
                }
            }
            if(typeof callback == "function"){
                callback(this, node);
            }
        }catch(e){
            this.handleFailure(response);
        }
    },
    handleResponse : function(response){
        this.transId = false;
        var a = response.argument;
        this.processResponse(response, a.node, a.callback);
    },
    handleFailure : function(response){    	
        this.transId = false;
        var a = response.argument;
        if(typeof this.loadexception == "function")
			this.loadexception(this,a.node, response);
        if(typeof a.callback == "function"){
            a.callback(this, a.node);
        }
    }
}

if (typeof TS == "undefined") TS = {};

//节点基类
TS.Node = function(attributes){
	this.attributes = attributes || {};
    this.leaf = this.attributes.leaf;
    this.id = this.attributes.id;
    if(!this.id){
        this.id = TS.id();
        this.attributes.id = this.id;
    }
    this.childNodes = [];
    if(!this.childNodes.indexOf){ // indexOf is a must
        this.childNodes.indexOf = function(o){
            for(var i = 0, len = this.length; i < len; i++){
                if(this[i] == o) return i;
            }
            return -1;
        };
    }
    this.parentNode = null;
    this.firstChild = null;
    this.lastChild = null;
    this.previousSibling = null;
    this.nextSibling = null;
};

TS.Node.prototype = {
	fireEvent : function(){
		return this.getOwnerTree().fireEvent.apply(this.getOwnerTree(), arguments);
    },    
    isLeaf : function(){
        return this.leaf === true;
    },
    // private
    setFirstChild : function(node){
        this.firstChild = node;
    },
    //private
    setLastChild : function(node){
        this.lastChild = node;
    },
    /**
     * Returns true if this node is the last child of its parent
     * @return {Boolean}
     */
    isLast : function(){
       return (!this.parentNode ? true : this.parentNode.lastChild == this);
    },
    /**
     * Returns true if this node is the first child of its parent
     * @return {Boolean}
     */
    isFirst : function(){
       return (!this.parentNode ? true : this.parentNode.firstChild == this);
    },
    hasChildNodes : function(){
        return !this.isLeaf() && this.childNodes.length > 0;
    }, 
    /**
     * 添加一个节点
     */
    appendChild : function(node){
        var multi = false;
        if(node instanceof Array){
            multi = node;
        }else if(arguments.length > 1){
            multi = arguments;
        }
        // if passed an array or multiple args do them one by one
        if(multi){
            for(var i = 0, len = multi.length; i < len; i++) {
            	this.appendChild(multi[i]);
            }
        }else{
            var index = this.childNodes.length;
            var oldParent = node.parentNode;
            // it's a move, make sure we move it cleanly
            if(oldParent){
                oldParent.removeChild(node);
            }
            index = this.childNodes.length;
            if(index == 0){
                this.setFirstChild(node);
            }
            this.childNodes.push(node);
            node.parentNode = this;
            var ps = this.childNodes[index-1];
            if(ps){
                node.previousSibling = ps;
                ps.nextSibling = node;
            }else{
                node.previousSibling = null;
            }
            node.nextSibling = null;
            this.setLastChild(node);
            node.setOwnerTree(this.getOwnerTree());
            return node;
        }
    },
    removeChild : function(node){
        var index = this.childNodes.indexOf(node);
        if(index == -1){
            return false;
        }

        // remove it from childNodes collection
        this.childNodes.splice(index, 1);

        // update siblings
        if(node.previousSibling){
            node.previousSibling.nextSibling = node.nextSibling;
        }
        if(node.nextSibling){
            node.nextSibling.previousSibling = node.previousSibling;
        }

        // update child refs
        if(this.firstChild == node){
            this.setFirstChild(node.nextSibling);
        }
        if(this.lastChild == node){
            this.setLastChild(node.previousSibling);
        }

        node.setOwnerTree(null);
        // clear any references from the node
        node.parentNode = null;
        node.previousSibling = null;
        node.nextSibling = null;
        return node;
    },

    insertBefore : function(node, refNode){
        if(!refNode){ // like standard Dom, refNode can be null for append
            return this.appendChild(node);
        }
        // nothing to do
        if(node == refNode){
            return false;
        }
        var index = this.childNodes.indexOf(refNode);
        var oldParent = node.parentNode;
        var refIndex = index;

        // when moving internally, indexes will change after remove
        if(oldParent == this && this.childNodes.indexOf(node) < index){
            refIndex--;
        }

        // it's a move, make sure we move it cleanly
        if(oldParent){
            oldParent.removeChild(node);
        }
        if(refIndex == 0){
            this.setFirstChild(node);
        }
        this.childNodes.splice(refIndex, 0, node);
        node.parentNode = this;
        var ps = this.childNodes[refIndex-1];
        if(ps){
            node.previousSibling = ps;
            ps.nextSibling = node;
        }else{
            node.previousSibling = null;
        }
        node.nextSibling = refNode;
        refNode.previousSibling = node;
        node.setOwnerTree(this.getOwnerTree());
        return node;
    },
    
    item : function(index){
        return this.childNodes[index];
    },
    replaceChild : function(newChild, oldChild){
        this.insertBefore(newChild, oldChild);
        this.removeChild(oldChild);
        return oldChild;
    },
    indexOf : function(child){
        return this.childNodes.indexOf(child);
    },
    getOwnerTree : function(){
        // if it doesn't have one, look for one
        if(!this.ownerTree){
            var p = this;
            while(p){
                if(p.ownerTree){
                    this.ownerTree = p.ownerTree;
                    break;
                }
                p = p.parentNode;
            }
        }
        return this.ownerTree;
    },
    /**
     * 获取几点深度
     */
    getDepth : function(){
        var depth = 0;
        var p = this;
        while(p.parentNode){
            ++depth;
            p = p.parentNode;
        }
        return depth;
    },
    setOwnerTree : function(tree){
        // if it's move, we need to update everyone
        if(tree != this.ownerTree){
            if(this.ownerTree){
                this.ownerTree.unregisterNode(this);
            }
            this.ownerTree = tree;
            var cs = this.childNodes;
            for(var i = 0, len = cs.length; i < len; i++) {
            	cs[i].setOwnerTree(tree);
            }
            if(tree){
                tree.registerNode(this);
            }
        }
    },
    /**
     * 获取节点路径
     */
    getPath : function(attr){
        attr = attr || "id";
        var p = this.parentNode;
        var b = [this.attributes[attr]];
        while(p){
            b.unshift(p.attributes[attr]);
            p = p.parentNode;
        }
        var sep = this.getOwnerTree().pathSeparator;
        return sep + b.join(sep);
    },
    bubble : function(fn, scope, args){
        var p = this;
        while(p){
            if(fn.call(scope || p, args || p) === false){
                break;
            }
            p = p.parentNode;
        }
    },
    /**
     * 递归所有下级节点
     */
    cascade : function(fn, scope, args){
        if(fn.call(scope || this, args || this) !== false){
            var cs = this.childNodes;
            for(var i = 0, len = cs.length; i < len; i++) {
            	cs[i].cascade(fn, scope, args);
            }
        }
    },
    /**
     * 所有子节点
     */
    eachChild : function(fn, scope, args){
        var cs = this.childNodes;
        for(var i = 0, len = cs.length; i < len; i++) {
        	if(fn.call(scope || this, args || cs[i]) === false){
        	    break;
        	}
        }
    },
    /**
     * 更加属性名和值查找节点
     */
    findChild : function(attribute, value){
        var cs = this.childNodes;
        for(var i = 0, len = cs.length; i < len; i++) {
        	if(cs[i].attributes[attribute] == value){
        	    return cs[i];
        	}
        }
        return null;
    },
    /**
     * 根据函数查找节点
     */
    findChildBy : function(fn, scope){
        var cs = this.childNodes;
        for(var i = 0, len = cs.length; i < len; i++) {
        	if(fn.call(scope||cs[i], cs[i]) === true){
        	    return cs[i];
        	}
        }
        return null;
    },
    /**
     * 是否保护该节点
     */
    contains : function(node){
        return node.isAncestor(this);
    },
    /**
     * 是否是祖先节点
     */
    isAncestor : function(node){
        var p = this.parentNode;
        while(p){
            if(p == node){
                return true;
            }
            p = p.parentNode;
        }
        return false;
    },
    toString : function(){
        return "[Node"+(this.id?" "+this.id:"")+"]";
    }
}

/**
 * 树的普通节点类
 */
TS.Tree.TreeNode = function(attributes){
	attributes = attributes || {};
    if(typeof attributes == "string"){
        attributes = {text: attributes};
    }
    this.childrenRendered = false;
    this.rendered = false;
    TS.Tree.TreeNode.superclass.constructor.call(this, attributes);
    this.expanded = attributes.expanded === true;
    this.isTarget = attributes.isTarget !== false;
    this.allowChildren = attributes.allowChildren !== false;
    this.text = attributes.text;
    this.disabled = attributes.disabled === true;
    var uiClass = this.attributes.uiProvider || TS.Tree.TreeNodeUI;
    this.ui = new uiClass(this);
};

TS.extend(TS.Tree.TreeNode, TS.Node, {
	isExpanded : function(){
        return this.expanded;
    },
    setFirstChild : function(node){
        var of = this.firstChild;
        TS.Tree.TreeNode.superclass.setFirstChild.call(this, node);
        if(this.childrenRendered && of && node != of){
            of.renderIndent(true, true);
        }
        if(this.rendered){
            this.renderIndent(true, true);
        }
    }, 
    // private override
    setLastChild : function(node){
        var ol = this.lastChild;
        TS.Tree.TreeNode.superclass.setLastChild.call(this, node);
        if(this.childrenRendered && ol && node != ol){
            ol.renderIndent(true, true);
        }
        if(this.rendered){
            this.renderIndent(true, true);
        }
    },

    // these methods are overridden to provide lazy rendering support
    // private override
    appendChild : function(){
        var node = TS.Tree.TreeNode.superclass.appendChild.apply(this, arguments);
        if(node && this.childrenRendered){
            node.render();
        }
        this.ui.updateExpandIcon();
        return node;
    },

    // private override
    removeChild : function(node){        
        TS.Tree.TreeNode.superclass.removeChild.apply(this, arguments);
        // if it's been rendered remove dom node
        if(this.childrenRendered){
            node.ui.remove();
        }
        if(this.childNodes.length < 1){
            this.collapse(false, false);
        }else{
            this.ui.updateExpandIcon();
        }
        return node;
    },

    // private override
    insertBefore : function(node, refNode){
        var newNode = TS.Tree.TreeNode.superclass.insertBefore.apply(this, arguments);
        if(newNode && refNode && this.childrenRendered){
            node.render();
        }
        this.ui.updateExpandIcon();
        return newNode;
    },
    setText : function(text){
        var oldText = this.text;
        this.text = text;
        this.attributes.text = text;
        if(this.rendered){ // event without subscribing
            this.ui.onTextChange(this, text, oldText);
        }
    },
    select : function(){
    	var last = this.getOwnerTree().selNode;
        if(last != this){
            if(last){
                last.ui.onSelectedChange(false);
            }
            this.getOwnerTree().selNode = this;
            this.ui.onSelectedChange(true);
        }else{
        	//this.unselect();
        }
    },
    unselect : function(){
        var n = this.getOwnerTree().selNode;
        if(n){
            n.ui.onSelectedChange(false);
            this.getOwnerTree().selNode = null;
        }
    },
    isSelected : function(){
        return this.getOwnerTree().selNode == node; ;
    },
    expand : function(deep,callback){
        if(!this.expanded){
            if(!this.childrenRendered){
                this.renderChildren();
            }
            this.expanded = true;
            this.ui.expand();
        }
        if(typeof callback == "function"){
               callback(this);
        }
        if(deep === true){
            this.expandChildNodes(true);
        }
    },

    isHiddenRoot : function(){
        return this.isRoot && !this.getOwnerTree().rootVisible;
    },
    collapse : function(deep){
        if(this.expanded && !this.isHiddenRoot()){
            this.expanded = false;
            this.ui.collapse();
        }
        if(deep === true){
            var cs = this.childNodes;
            for(var i = 0, len = cs.length; i < len; i++) {
            	cs[i].collapse(true, false);
            }
        }
    },
    toggle : function(){
        if(this.expanded){
            this.collapse();
        }else{
            this.expand();
        }
    },
    /**
     * 展开所有孩子节点
     */
    expandChildNodes : function(deep){
        var cs = this.childNodes;
        for(var i = 0, len = cs.length; i < len; i++) {
        	cs[i].expand(deep);
        }
    },
    /**
     * 收起所有孩子的节点
     */
    collapseChildNodes : function(deep){
        var cs = this.childNodes;
        for(var i = 0, len = cs.length; i < len; i++) {
        	cs[i].collapse(deep);
        }
    },
    // private
    renderChildren : function(suppressEvent){
        var cs = this.childNodes;
        for(var i = 0, len = cs.length; i < len; i++){
            cs[i].render(true);
        }
        this.childrenRendered = true;
    },
    // private
    render : function(bulkRender){
        this.ui.render(bulkRender);
        if(!this.rendered){
            this.rendered = true;
            if(this.expanded){
                this.expanded = false;
                this.expand(false);
            }
        }
    },
    // private
    renderIndent : function(deep, refresh){
        if(refresh){
            this.ui.childIndent = null;
        }
        this.ui.renderIndent();
        if(deep === true && this.childrenRendered){
            var cs = this.childNodes;
            for(var i = 0, len = cs.length; i < len; i++){
                cs[i].renderIndent(true, refresh);
            }
        }
    }
});

/**
 * 树的动态节点类
 */
TS.Tree.AsyncTreeNode = function(config){
    this.loaded = false;
    this.loading = false;
    TS.Tree.AsyncTreeNode.superclass.constructor.apply(this, arguments);
};
TS.extend(TS.Tree.AsyncTreeNode, TS.Tree.TreeNode, {
    expand : function(deep, callback){
        if(this.loading){ // if an async load is already running, waiting til it's done
            var timer;
            var f = function(){
                if(!this.loading){ // done loading
                    clearInterval(timer);
                    this.expand(deep,callback);
                }
            }.bind(this);
            timer = setInterval(f, 200);
            return;
        }
        if(!this.loaded){
            this.loading = true;
            this.ui.beforeLoad(this);
            var loader = this.loader || this.attributes.loader || this.getOwnerTree().getLoader();
            if(loader){
                loader.load(this, this.loadComplete.bind(this, deep, callback));
                return;
            }
        }
        TS.Tree.AsyncTreeNode.superclass.expand.call(this, deep,  callback);
    },
    isLoading : function(){
        return this.loading;  
    },
    loadComplete : function(deep, callback){
        this.loading = false;
        this.loaded = true;
        this.ui.afterLoad(this);
        this.fireEvent("load", this);
        this.expand(deep, callback);
    },
    isLoaded : function(){
        return this.loaded;
    },
    
    hasChildNodes : function(){
        if(!this.isLeaf() && !this.loaded){
            return true;
        }else{
            return TS.Tree.AsyncTreeNode.superclass.hasChildNodes.call(this);
        }
    },
    reload : function(callback){
		if(this.ownerTree && this.ownerTree.selNode && this.contains(this.ownerTree.selNode)){
			this.ownerTree.unselect(); //kingchen 2007-11-28
		}
        this.collapse(false, false);
        while(this.firstChild){
            this.removeChild(this.firstChild);
        }
        this.childrenRendered = false;
        this.loaded = false;
        if(this.isHiddenRoot()){
            this.expanded = false;
        }
        this.expand(false, false, callback);
    }
});


///////////////////////////////////////////// TreeNodeUI ///////////////////////////////////////////////////////

/**
 * 数的界面绘画类
 */
TS.Tree.TreeNodeUI = function(node){
    this.node = node;
    this.rendered = false;
    this.emptyIcon = TS.BLANK_IMAGE_URL;
};
TS.Tree.TreeNodeUI.prototype = {
    removeChild : function(node){
        if(this.rendered){
            this.ctNode.removeChild(node.ui.getEl());
        }
    },
    beforeLoad : function(){
         this.addClass("egd-tree-node-loading");
    },

    afterLoad : function(){
         this.removeClass("egd-tree-node-loading");
    },

    onTextChange : function(node, text, oldText){
        if(this.rendered){
            this.textNode.innerHTML = text;
        }
    },

    onSelectedChange : function(state){
        if(state){
            this.focus();
            this.addClass("egd-tree-selected");
        }else{
            //this.blur();
            this.removeClass("egd-tree-selected");
        }
    },
	fireEvent : function(){
		 return this.node.fireEvent.apply(this.node, arguments);
    },
    addClass : function(cls){
        if(this.elNode){
            $(this.elNode).addClassName(cls);
        }
    },

    removeClass : function(cls){
        if(this.elNode){
            $(this.elNode).removeClassName(cls);
        }
    },

    remove : function(){
        if(this.rendered){
            this.holder = document.createElement("div");
            this.holder.appendChild(this.wrap);
        }
    },
    initEvents : function(){
        var a = this.anchor;
        var el = $(a);

        if(TS.isOpera){ // opera render bug ignores the CSS
            el.setStyle({"text-decoration":"none"});
        }

        el.observe("click", this.onClick.bind(this));
        el.observe("dblclick", this.onDblClick.bind(this));

        if(this.checkbox){
            Event.observe(this.checkbox, "change", this.onCheckChange.bind(this));
        }

        var icon = $(this.iconNode);
        icon.observe("click", this.onClick.bind(this));
        icon.observe("dblclick", this.onDblClick.bind(this));

        Event.observe(this.ecNode, "click", this.ecClick.bind(this), true);

        if(this.node.disabled){
            this.addClass("egd-tree-node-disabled");
        }
        if(this.node.hidden){
            this.addClass("egd-tree-node-disabled");
        }
        var ot = this.node.getOwnerTree();
    },
    hide : function(){
        if(this.rendered){
            this.wrap.style.display = "none";
        }
    },
    show : function(){
        if(this.rendered){
            this.wrap.style.display = "";
        }
    },
    onClick : function(e){
            Event.stop(e);            
            this.node.select();            
            if(this.node.attributes.href){
                this.fireEvent("click", this.node, e);
                return;
            }
            if(this.node.attributes.singleClickExpand && this.node.hasChildNodes()){
                this.node.toggle();
            }
            this.fireEvent("click", this.node, e);
    },

    onDblClick : function(e){
        Event.stop(e);
        if(this.disabled){
            return;
        }
        if(this.checkbox){
            this.toggleCheck();
        }
        if(this.node.hasChildNodes()){
            this.node.toggle();
        }
        this.fireEvent("dblclick", this.node, e);
        
    },

    onCheckChange : function(){
        var checked = this.checkbox.checked;
        this.node.attributes.checked = checked;
        this.fireEvent('checkchange', this.node, checked);
    },

    ecClick : function(e){
        if(this.node.hasChildNodes()){
            this.node.toggle();
        }
    },

    expand : function(){
        this.updateExpandIcon();
        if(!this.wasLeaf) this.ctNode.style.display = "";
    },

    focus : function(){
        if(!this.node.preventHScroll){
            try{this.anchor.focus();
            }catch(e){}
        }else if(!TS.isIE){
            try{
                var noscroll = this.node.getOwnerTree().getTreeEl();
                var l = noscroll.scrollLeft;
                this.anchor.focus();
                noscroll.scrollLeft = l;
            }catch(e){}
        }
    },

    toggleCheck : function(value){
        var cb = this.checkbox;
        if(cb){
            cb.checked = (value === undefined ? !cb.checked : value);
        }
    },

    blur : function(){
        try{
            this.anchor.blur();
        }catch(e){}
    },
    highlight : function(){
        var tree = this.node.getOwnerTree();
        $(this.wrap).highlight(
            tree.hlColor || "C3DAF9",
            {endColor: tree.hlBaseColor}
        );
    },

    collapse : function(){
        this.updateExpandIcon();
        this.ctNode.style.display = "none";
    },
    
    getContainer : function(){
        return this.ctNode;
    },

    getEl : function(){
        return this.wrap;
    },

    onRender : function(){
        this.render();
    },

    render : function(bulkRender){
        var n = this.node, a = n.attributes;
        var targetNode = n.parentNode ?
              n.parentNode.ui.getContainer() : n.ownerTree.innerCt;

        if(!this.rendered){
            this.rendered = true;

            this.renderElements(n, a, targetNode, bulkRender);

            if(a.qtip){
               if(this.textNode.setAttributeNS){
                   this.textNode.setAttributeNS("ext", "qtip", a.qtip);
                   if(a.qtipTitle){
                       this.textNode.setAttributeNS("ext", "qtitle", a.qtipTitle);
                   }
               }else{
                   this.textNode.setAttribute("ext:qtip", a.qtip);
                   if(a.qtipTitle){
                       this.textNode.setAttribute("ext:qtitle", a.qtipTitle);
                   }
               }
            }else if(a.qtipCfg){
                a.qtipCfg.target = TS.id(this.textNode);
                TS.QuickTips.register(a.qtipCfg);
            }
            this.initEvents();
            if(!this.node.expanded){
                this.updateExpandIcon();
            }
        }else{
            if(bulkRender === true) {
                targetNode.appendChild(this.wrap);
            }
        }
    },

    renderElements : function(n, a, targetNode, bulkRender){
        // add some indent caching, this helps performance when rendering a large tree
        this.indentMarkup = n.parentNode ? n.parentNode.ui.getChildIndent() : '';

        var cb = typeof a.checked == 'boolean';

        var buf = ['<li class="egd-tree-node"><div class="egd-tree-node-el ', a.cls,'">',
            '<span class="egd-tree-node-indent">',this.indentMarkup,"</span>",
            '<img src="', this.emptyIcon, '" class="egd-tree-ec-icon" />',
            '<img src="', a.icon || this.emptyIcon, '" class="egd-tree-node-icon',(a.icon ? " egd-tree-node-inline-icon" : ""),(a.iconCls ? " "+a.iconCls : ""),'" unselectable="on" />',
            cb ? ('<input class="egd-tree-node-cb" type="checkbox" ' + (a.checked ? 'checked="checked" />' : ' />')) : '',
            '<a hidefocus="on" href="',a.href ? a.href : "#",'" tabIndex="1" ',
             a.hrefTarget ? ' target="'+a.hrefTarget+'"' : "", '><span unselectable="on">',n.text,"</span></a></div>",
            '<ul class="egd-tree-node-ct" style="display:none;"></ul>',
            "</li>"];

        if(bulkRender !== true && n.nextSibling && n.nextSibling.ui.getEl()){
            new Insertion.Before(n.nextSibling.ui.getEl(), buf.join(""));
            this.wrap = n.nextSibling.ui.getEl().previousSibling;
        }else{
            new Insertion.Bottom(targetNode, buf.join(""));
            this.wrap = targetNode.lastChild;
        }
        this.elNode = this.wrap.childNodes[0];
        this.ctNode = this.wrap.childNodes[1];
        var cs = this.elNode.childNodes;
        this.indentNode = cs[0];
        this.ecNode = cs[1];
        this.iconNode = cs[2];
        var index = 3;
        if(cb){
            this.checkbox = cs[3];
            index++;
        }
        this.anchor = cs[index];
        this.textNode = cs[index].firstChild;
    },

    getAnchor : function(){
        return this.anchor;
    },

    getTextEl : function(){
        return this.textNode;
    },

    getIconEl : function(){
        return this.iconNode;
    },

    isChecked : function(){
        return this.checkbox ? this.checkbox.checked : false;
    },

    updateExpandIcon : function(){
        if(this.rendered){
            var n = this.node, c1, c2;
            var cls = n.isLast() ? "egd-tree-elbow-end" : "egd-tree-elbow";
            var hasChild = n.hasChildNodes();
            if(hasChild){
                if(n.expanded){
                    cls += "-minus";
                    c1 = "egd-tree-node-collapsed";
                    c2 = "egd-tree-node-expanded";
                }else{
                    cls += "-plus";
                    c1 = "egd-tree-node-expanded";
                    c2 = "egd-tree-node-collapsed";
                }
                if(this.wasLeaf){
                    this.removeClass("egd-tree-node-leaf");
                    this.wasLeaf = false;
                }
                if(this.c1 != c1 || this.c2 != c2){
                	$(this.elNode).removeClassName(c1);
                	$(this.elNode).addClassName(c2)
                   // $(this.elNode).replaceClass(c1, c2);
                    this.c1 = c1; this.c2 = c2;
                }
            }else{
                if(!this.wasLeaf){
                	$(this.elNode).removeClassName("egd-tree-node-expanded");
                	$(this.elNode).addClassName("egd-tree-node-leaf")
                    //TS.fly(this.elNode).replaceClass("egd-tree-node-expanded", "egd-tree-node-leaf");
                    delete this.c1;
                    delete this.c2;
                    this.wasLeaf = true;
                }
            }
            var ecc = "egd-tree-ec-icon "+cls;
            if(this.ecc != ecc){
                this.ecNode.className = ecc;
                this.ecc = ecc;
            }
        }
    },

    getChildIndent : function(){
        if(!this.childIndent){
            var buf = [];
            var p = this.node;
            while(p){
                if(!p.isRoot || (p.isRoot && p.ownerTree.rootVisible)){
                    if(!p.isLast()) {
                        buf.unshift('<img src="'+this.emptyIcon+'" class="egd-tree-elbow-line">');
                    } else {
                        buf.unshift('<img src="'+this.emptyIcon+'" class="egd-tree-icon">');
                    }
                }
                p = p.parentNode;
            }
            this.childIndent = buf.join("");
        }
        return this.childIndent;
    },

    renderIndent : function(){
        if(this.rendered){
            var indent = "";
            var p = this.node.parentNode;
            if(p){
                indent = p.ui.getChildIndent();
            }
            if(this.indentMarkup != indent){ // don't rerender if not required
                this.indentNode.innerHTML = indent;
                this.indentMarkup = indent;
            }
            this.updateExpandIcon();
        }
    }
};

/**
 * 根节点绘画类
 */
TS.Tree.RootTreeNodeUI = function(){
    TS.Tree.RootTreeNodeUI.superclass.constructor.apply(this, arguments);
};
TS.extend(TS.Tree.RootTreeNodeUI, TS.Tree.TreeNodeUI, {
    render : function(){
        if(!this.rendered){
            var targetNode = this.node.ownerTree.innerCt;
            this.node.expanded = true;
            targetNode.innerHTML = '<div class="egd-tree-root-node"></div>';
            this.wrap = this.ctNode = targetNode.firstChild;
        }
    },
    collapse : function(){
    },
    expand : function(){
    }
});