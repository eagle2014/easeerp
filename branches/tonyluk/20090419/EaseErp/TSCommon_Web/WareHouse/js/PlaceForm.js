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
				    common_Save();
				},
			    // 管理员才能保存
			    hidden: !(my.isManager && my.canEdit)
			},
			{
				id:"btnEdit",
				text:"编辑",
				iconClass:"egd-button-edit",
				handler:function(button){
				    common_Edit();
				},
			    // 管理员才能编辑
			    hidden: !(my.isManager && !my.canEdit)
			},
			{
				id:"btnDelete",
				text:"删除",
				iconClass:"egd-button-delete",
				handler:function(button){
					common_Delete();
				},
			    // 管理员才能删除
			    hidden: !(my.isManager && ($F("_ID") > 0))
			}
		]);
	}
}

	/* 
	 * 验证表单信息的完整性
	 */
	function validate(){
		var fieldNames = "Name;Code;Memo";
		var fieldDescs = "名称;代码;备注";
        return !TS.Msg.isEmptyValue(fieldNames,fieldDescs);
	}