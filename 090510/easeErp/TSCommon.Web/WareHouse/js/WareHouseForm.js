var thisPage={
    init:function(){
        //��ʼ��������
        thisPage.initTB();
    },
    //��ʼ��������
    initTB:function(){
        this.toolbar = new TS.TB("tbContainer",[
			"|",
			{
				id:"btnSave",
				text:"����",
				iconClass:"egd-button-save",
				handler:function(button){
				    thisPage.doSave();
				},
			    // ����Ա���ܱ���
			    hidden: !(my.isManager && my.canEdit)
			},
			{
				id:"btnEdit",
				text:"�༭",
				iconClass:"egd-button-edit",
				handler:function(button){
				    thisPage.doEdit();
				},
			    // ����Ա���ܱ༭
			    hidden: !(my.isManager && !my.canEdit)
			},
			{
				id:"btnDelete",
				text:"ɾ��",
				iconClass:"egd-button-delete",
				handler:function(button){
					thisPage.doDelete();
				},
			    // ����Ա����ɾ��
			    hidden: !(my.isManager && ($F("_ID") > 0))
			}
		]);
    },
    //����
    doSave:function(){
        
    },
    //�༭
    doEdit:function(){
        
    },
    //ɾ��
    doDelete:functon(){
        
    },
    /* 
	 * ��֤����Ϣ��������
	 */
	validate:function(){
		var fieldNames = "Name;Code;Memo";
		var fieldDescs = "����;����;��ע";
        return !TS.Msg.isEmptyValue(fieldNames,fieldDescs);
	},
};