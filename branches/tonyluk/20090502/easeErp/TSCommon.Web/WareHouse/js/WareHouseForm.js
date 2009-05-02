var thisPage={
    init:function(){
        //初始化工具条
        thisPage.initTB();
    },
    //初始化工具条
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
    //保存
    doSave:function(){
        
    },
    //编辑
    doEdit:function(){
        
    },
    //删除
    doDelete:functon(){
        
    },
    /* 
	 * 验证表单信息的完整性
	 */
	validate:function(){
		var fieldNames = "Name;Code;Memo";
		var fieldDescs = "名称;代码;备注";
        return !TS.Msg.isEmptyValue(fieldNames,fieldDescs);
	},
};