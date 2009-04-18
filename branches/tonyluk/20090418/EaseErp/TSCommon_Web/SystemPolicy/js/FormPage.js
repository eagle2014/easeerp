var thisPage = {
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
				hidden: !(my.canEdit)
			},
			{
				id:"btnEdit",
				text:"编辑",
				iconClass:"egd-button-edit",
				handler:function(button){
				    thisPage.doEdit();
				},
				hidden: !(!my.canEdit)
			}
		]);
    },
  	/* 
	 * 保存
	 */
	doSave:function(){
	    if(!validate()) return;
        TS.Ajax.request({
            url: 'policyAction.do?action=SaveByAjax', 
	        parameters: TS.Form.serialize($("thisForm")),
	        callback: function(json){
		        // 设置相关域信息
		        var domain = json.entity;
		        $("_ID").value = domain.ID;
	        }
        });
    },
    
	/* 
	 * 编辑
	 */
	doEdit:function(){
	    TS.Msg.wait({msg:'正在重新加载文档，请稍候...', iconCls: TS.Msg.IconCls.EDITING});
        var strUrl = TS.rootPath + "policyAction.do?action=Edit";
        strUrl += "&id=" + $F("_ID");
        strUrl += "&timeStamp=" + new Date().getTime();
        //window.open(strUrl,"_self");	// 该方法会导致整个主页闪烁一下
        TS.updateMe(strUrl);
    }
 };
 
 function SetFormData()
 {
      if($F("Type") == "select"){
            $("Value").value = $F("PolicyValue_2");
      }else{
            $("Value").value = $F("PolicyValue_1");
      } 
 }
 /* 
 * 验证表单信息的完整性
 */
function validate(){
    SetFormData();
	var fieldNames = "BelongModule;Value";
	var fieldDescs = "所属模块;策略值";
	//if(!validate_txt(fieldNames,fieldDescs)) 
	//    return false;
	//return true;
    return !TS.Msg.isEmptyValue(fieldNames,fieldDescs);
}