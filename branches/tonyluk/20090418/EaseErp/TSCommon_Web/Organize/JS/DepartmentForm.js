var thisPage = {
	/* 
	 * 关闭当前页面
	 */
	closeMe:function(){
	    TS.closeMe();
	},
	
	/* 
	 * 页面初始化方法
	 */
	init:function(){
		this.initTB();
		TS.Msg.hide();
	},
	
	/* 
	 * 初始化工具条
	 */
	initTB:function(){
		this.toolbar = new TS.TB("tbContainer",[
			"|",
			{
				id:"btnSave",
				text:"保存",
				iconClass:"egd-button-save",
				handler:function(button){
				    thisPage.doSave();
				},
			    // 管理员才能保存
			    hidden: !(my.isManager && my.canEdit)
			},
			{
				id:"btnEdit",
				text:"编辑",
				iconClass:"egd-button-edit",
				handler:function(button){
				    thisPage.doEdit();
				},
			    // 管理员才能编辑
			    hidden: !(my.isManager && !my.canEdit)
			},
			{
				id:"btnDelete",
				text:"删除",
				iconClass:"egd-button-delete",
				handler:function(button){
					thisPage.doDelete();
				},
			    // 管理员才能删除
			    hidden: !(my.isManager && ($F("_ID") > 0))
			}
		]);
	},
	
	/* 
	 * 验证表单信息的完整性
	 */
	validate:function(){
	    // 排出选择上级部门为自己
	    if($F("ParentOUUnid") == $F("Unid")){
    	    TS.Msg.msgBox({title:"部门配置",msg: "不能选择自己作为上级部门！", iconCls: TS.Msg.IconCls.ERROR});
    	    return false;
	    }
	    
	    // 排序序号必须为数字
        //if(isNaN($F('OrderNo'))){
		//	TS.Msg.msgBox({title:"部门配置", msg: "排序序号必须为数字类型！", iconCls: TS.Msg.IconCls.WARNING});
		//	return false;
        //}
	    
		var fieldNames = "UnitName;Name;Code;OrderNo";
		var fieldDescs = "所属单位;名称;编码;排序序号";
        return !TS.Msg.isEmptyValue(fieldNames,fieldDescs);
	},
	
	/* 
	 * 保存
	 */
	doSave:function(){
	    // 必填域验证
	    if(!this.validate()) return;
	    
	    var strUrl = TS.rootPath + "departmentAction.do?action=SaveByAjax";
	    strUrl += "&timeStamp=" + new Date().getTime();
	    
	    TS.Msg.wait({msg:'正在保存文档，请稍候...', iconCls: TS.Msg.IconCls.SAVIND});
		new Ajax.Request(strUrl, 
		{ 
			method:'post', 
			parameters: Form.serialize($("thisForm")),
			onSuccess: function(transport){
				var response = eval("("+transport.responseText+")");  
				if (response.result == true){
				    var entity = response.entity;
 				    // 设置相关域的值
   				    $('_ID').value = entity.ID;
    				
    				// 判断是否显示删除按钮
    				var canDelete = (my.isManager && $F("_ID") > 0);
    				if (canDelete){
    				    thisPage.toolbar.show(["btnDelete"]);
    				}
    				
   				    TS.Msg.hide();// 隐藏保存提示窗口
    				TS.Msg.msgBox({title:"部门配置",msg: response.msg, iconCls: TS.Msg.IconCls.INFO});// 显示保存成功的提示信息
				}else{
    				TS.Msg.msgBox({title:"部门配置",msg: response.msg, iconCls: TS.Msg.IconCls.ERROR});
				}
			},
			onFailure: function(transport){
				TS.Msg.msgBox({title:"部门配置",msg: '保存部门过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
			}
		});	
    },
    
	/* 
	 * 编辑
	 */
	doEdit:function(){
	    TS.Msg.wait({msg:'正在重新加载文档，请稍候...', iconCls: TS.Msg.IconCls.EDITING});
        var strUrl = TS.rootPath + "departmentAction.do?action=Edit";
        strUrl += "&id=" + $F("_ID");
        strUrl += "&timeStamp=" + new Date().getTime();
        //window.open(strUrl,"_self");	// 该方法会导致整个主页闪烁一下
        TS.updateMe(strUrl);
    },
	
	/* 
	 * 删除
	 */
	doDelete:function(){
	    // 删除确定
        TS.Msg.confirm({title:"部门配置",msg:"确定要删除当前部门吗？",onYes: function(){
	        var strUrl = TS.rootPath + "departmentAction.do?action=Delete";
	        strUrl += "&ids=" + $F("_ID");
	        strUrl += "&timeStamp=" + new Date().getTime();
	        TS.Msg.wait({msg:'正在删除文档，请稍候...', iconCls: TS.Msg.IconCls.DELETING});
		    new Ajax.Request(strUrl, 
		    { 
			    onSuccess: function(transport){
				    var response = eval("("+transport.responseText+")");  
 				    if (response.result == true){
    				    //TS.Msg.msgBox({title:'部门',msg:response.msg});
    				    TS.Msg.hide();
    				    thisPage.closeMe();
				    }else{
    			        TS.Msg.msgBox({title:"部门", msg:response.msg, iconCls: TS.Msg.IconCls.ERROR});
				    }
			    },
			    onFailure: function(transport){
 				    TS.Msg.msgBox({title:"部门",msg: '删除部门过程出现异常!', iconCls: TS.Msg.IconCls.ERROR});
			    }
		    });	
        }});
    },
    
 	/* 
	 * 选择上级单位/部门
     * @param {String} selectType 选择OU的类型：BM--选择部门，DW--选择单位
     * @param {String} punid 上级OU的Unid
	 */
    selectUpper: function(selectType,punid,pname){
        if(selectType == 'BM'){
            var unitUnid = $F("UnitUnid");
            if(null == unitUnid || unitUnid == ""){
                TS.Msg.msgBox({title:"部门配置",msg: "请先选择所属单位！", iconCls: TS.Msg.IconCls.WARN});
                return;
            }
        }

        TS.Org.selectOUInfo({
            title: '选择' + (selectType == 'BM' ? '上级部门' : '所属单位'),
            ouType: selectType,
            rootOUUnid: punid ? punid : '',
            rootOUName: pname ? pname : '',
            parentIconCls: punid ? 'egd-treeNode-unit' : 'egd-treeNode-department',// 根节点的样式
            rootVisible: punid ? 'true' : 'false',// 根节点的可见性
            onOk: function(ouInfo){
                if(selectType == 'BM'){
	                // 排除选择上级部门为自己
	                if(ouInfo.Unid == $F("Unid")){
    	                TS.Msg.msgBox({title:"部门配置",msg: "不能选择自己作为上级部门！", iconCls: TS.Msg.IconCls.ERROR});
    	                return false;
	                }else{
                        $("ParentOUUnid").value = ouInfo.Unid;
                        $("ParentOUName").value = ouInfo.FullName;
                    }
                }else{
                    $("UnitUnid").value = ouInfo.Unid;
                    $("UnitName").value = ouInfo.Name;
                    $("UnitFullCode").value = ouInfo.FullCode;
                    $("UnitFullName").value = ouInfo.FullName;
                    $("ParentOUUnid").value = '';
                    $("ParentOUName").value = '';
                }
           }
        });
    },
    
 	/* 
	 * 清空上级部门
	 */
    clearUpper: function(){
        $("ParentOUUnid").value = "";
        $("ParentOUName").value = "";
    },
   
	noUsed:false
}