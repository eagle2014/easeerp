/**
 * 附件组件
 * Tony 2008-08-16
 * singleton
 * 
 * 引用:TS.js,
 */
TS.namespace("TS.ATM");
TS.ATM = {
    /**
     * 选择文件并上传
     * @param {Object} config 配置信息
     * @config {String} punid 附件所属文档的Unid
     * @config {String} type 附件类型，通常为附件所属文档的类型
     * @config {String} caller 调用者的标识符
     * @config {String} filter 可选：可以上传的文件扩展名(不含符号“.”)，多个扩展名间用“,”隔开，为空代表支持任意扩展名的文件
     * @config {Number} maxSize 可选：上传文件的大小限制，单位为MB，为空代表使用服务器限制的大小
     */
    uploadFile: function(config){
		TS.applyIf(config,{
		    id: 'egdUploadFileDlg',
            title: '上传文件',
            width: 280,
            height: 180,
            refresh: true,
            afterOk: function(retult){
                TS.ATM.updateCaller(retult);   // 更新附件列表
            }
		});
		var url = TS.rootPath + 'ATM/UploadFile.aspx';
	    url += '?punid=' + config.punid;
		if(config.type)
	        url += '&type=' + config.type;
		if(config.caller)
	        url += '&caller=' + config.caller;
		if(config.filter)
	        url += '&filter=' + config.filter;
		if(config.maxSize)
	        url += '&maxSize=' + config.maxSize;
	        
        url += '&timeStamp=' + new Date().getTime();
	    config.url = url;
	    
	    if(!config.onOk)
            config.onOk = function(retult){};  
		
		// 显示对话框
        TS.Dlg.create(config);
    },
    
    /**
     * 下载附件
     * @param {Object} config 配置信息
     * @config {String} caller 调用者的标识符
     */
    downloadFile: function(config){
        var unid = TS.ATM.getSelected(config.caller);
        if(unid){
		    var url = TS.rootPath + 'ATM/DownloadFile.aspx?type=unid&id=' + unid;
            window.open(url,'blank');
        }else{
            TS.Msg.msgBox({msg: '请先选择要下载的附件！'});
        }
    },
    
    /**
     * 删除附件
     * @param {Object} config 配置信息
     * @config {String} caller 调用者的标识符
     */
    deleteFile: function(config){
        var unid = TS.ATM.getSelected(config.caller);
        if(unid){
		    var url = TS.rootPath + 'ATM.do?action=Delete&type=unid&ids=' + unid;
		    TS.Ajax.request({
		        url: url,
		        showSuccessMsg: false,
		        callback: function(result){
    		        // 删除选中的行
                    var dataTable = $(config.caller + '_DATAS');
		            var rows = dataTable.rows;
		            var deleteRowIndex = -1;
		            var i;
		            for(i = 1;i < rows.length ;i++){
		                if(rows[i].id == unid){
		                    deleteRowIndex = rows[i].rowIndex;
       	                    dataTable.deleteRow(deleteRowIndex);
       	                    break;
		                }
		            }
           	        
                    // 更新后面所有行的样式
                    rows = dataTable.rows;
		            var isJiShuRow;                     // 是否是奇数行
	                if(deleteRowIndex != -1 && rows.length > deleteRowIndex ){
		                for(i = deleteRowIndex;i < rows.length ;i++){
		                    isJiShuRow = ((i % 2) != 0);
			                rows[i].className = 'egd-atm-data-tr' + (isJiShuRow ? '0' : '1');
			            }
	                }
		        }
		    });
        }else{
            TS.Msg.msgBox({msg: '请先选择要删除的附件！'});
        }
    },
    
    /**
     * 选择文件并上传
     * @param {Object} config 配置信息
     * @config {string} caller 调用者的标识符
     * @punid {Array} atms 已上传的附件信息列表
     */
    updateCaller: function(config){
		var dataTable = $(config.caller + '_DATAS');
		var rows = dataTable.rows;
		var curRowCount = rows.length - 1;  // 减去标题行
		var atms = config.atms;
		var row,cell,atm,i,j;
		var isJiShuRow;                     // 是否是奇数行
		var cmLen = 5;                      // 列数
		for(i = 0;i < atms.length ;i++){
		    isJiShuRow = (((i + curRowCount) % 2) == 0);
		    atm = atms[i];
			row = dataTable.insertRow(-1);  // 插入新行
			row.id = atm.Unid;              // tr的id设为附件的unid
			row.className = 'egd-atm-data-tr' + (isJiShuRow ? '' : '1');
			
			// 插入单元格			
			for(j = 0;j < cmLen ;j++){
				cell = row.insertCell(-1);
			    cell.className = 'egd-atm-data-td' + (j == cmLen - 1  ? '-end' : '');
				switch (j){
                    case 0:
				        cell.setAttribute('align', 'center');
				        cell.setAttribute('width', '25');
				        cell.innerHTML = '<input name="' + config.caller + '_SELECT" type="radio" />';
                        break;
                    case 1:
				        cell.innerHTML = atm.Subject;
                        break;
                    case 2:
				        cell.setAttribute('width', '80');
				        cell.innerHTML = atm.FileSize;
                        break;
                    case 3:
				        cell.setAttribute('width', '80');
				        cell.innerHTML = atm.FileDate;
                        break;
                    case 4:
				        cell.setAttribute('width', '100');
				        cell.innerHTML = atm.Author;
                        break;
                }
			}
		}
    },
    
    /**
     * 删除附件
     * @param {String} caller 调用者的标识符
     * @return {String} 要删除的附件的Unid列表，多个Unid间用“,”连接
     */
    getSelected: function(caller){
	    var selectedRadio = TS.getSelectedRadio(caller + '_SELECT');
	    if(selectedRadio){
	        return selectedRadio.parentNode.parentNode.id;  // 所在行tr的id，即附件的Unid
	    }else{
	        return null;
	    }
    },
  downloadFileOne:function(value){
        var unid = value;
        if(unid){
		    var url = TS.rootPath + 'ATM/DownloadFile.aspx?type=unid&id=' + unid;
            window.open(url,'blank');
        }else{
            TS.Msg.msgBox({msg: '请先选择要下载的附件！'});
        }
    }
};