/**
 * 附件JS
 * 
 * zhouzhihu 2008-02-01 
 * 
 * 引用Prototype.js,Egd.js
 */
if (typeof Egd == "undefined") Egd = {};

/* 
 * 附件构造函数
 * <br><br>使用范例:<pre><code>
 * // 构造下拉框分类型attachment
 * var attachment = new Egd.Attachment("attachmentControlId",{
 * 	   punid : $F("Unid"),
 * 	   types : [
 *          {value : 'typeOne',text : '附件类型一'},
 *          {value : 'typeTwo',text : '附件类型二'}
 *     ],
 * 	   readOnly : this.readOnly
 * });
 * </code></pre>
 * @constructor
 * @param {String} controlId 页面上附件空间Id
 * @param {Object} config 所要创建的附件的配置
 * @config {String|Array} types 分类型显示附件类型，和下面的type属性是相互排斥的
 * 上面types为Array时格式为[{value : '',text : ''},{value : '',text : ''},...],其中value标识下拉框值,text标识下拉框显示文本
 * @config {String} filterEx 忽略指定的文件扩展名，多个用","号隔开
 * @config {Boolean} readOnly 是否该空间可以编辑
 */
Egd.Attachment = function(id,config){
    this.tbId = id + "Tb";
        this.gridId = id + "Grid";
        this.selectId = id + "selectType";
	    this.el = $(this.id);
	    this.config = {};
	    Egd.apply(this.config,config,this.defaultConfig);
	    this.init(this.config);
};

Egd.Attachment.prototype={
    defaultConfig: {
        typeHandler:function(button, value){
            button.reload(value);
        }
    },
    init: function(){
        this.initTb();
        this.loadData();
        this.bindEvent();
    },
    bindEvent : function(){
        var oThis = this;
        if(this.config.types && this.config.types != null && this.config.types != ""){
            if(this.config.types instanceof Array){
                $(this.selectId).onchange = function(_event){
			        if(typeof oThis.config.typeHandler == "function"){
	 			        oThis.config.typeHandler.call(oThis,oThis,$F(oThis.selectId));
	 			    }
		        };
		    }
		}
    },
    initTb : function(){
        var selectText = "";
        if(this.config.types && this.config.types != null && this.config.types != ""){
            if(this.config.types instanceof Array){
                selectText += "<select id='" + this.selectId + "' style='margin:-1px;padding:1px;height:19px;width:100px'>"
                var typeEl = this.config.types;
                for(var i=0;i<typeEl.length;i++){
                    selectText += "<option value=" + typeEl[i].value + ">" + typeEl[i].text + "</option>";
                }
                selectText += "</select>";
            }
        }
        if(!this.config.readOnly){
            this.toolbar = new Egd.TB(this.tbId,[
			    "|",
			    {
				    id:"btnUpload",
				    text:"上传",
				    iconClass:"egd-button-upload",
				    handler:function(button){
				        var strUrl = Egd.rootPath + "Egd_Attachment/AttachmentForm.aspx?a=1";
				        if(this.config.types && this.config.types != null && this.config.types != ""){
				            if(this.config.types instanceof Array){
				                strUrl += "&type=" + $F(this.selectId);
				            }else
				                strUrl += "&type=" + this.config.types;
				        }
				        if(this.config.filterEx && this.config.filterEx.length > 0){
                            strUrl += "&filterEx=" + this.config.filterEx;
                        }
				        strUrl += "&unid=" + this.config.punid + "&timeStamp=" + new Date().getTime();
				        /*
				         *创建一个新窗体
				         */
				        Egd.Dlg.create({
				            dlgId: 'attachmentId',
				            title: '上传附件',
				            url: strUrl,
				            width: 540,
				            height: 320,
							minWidth: 520,
							minHeight: 300,
				            modal: true,
				            onOk:function(){
				                this.dataGrid.reload();
				            }.bind(this)
				        });
				    }.bind(this)
			    },
			    {
				    id:"btnDelete",
			        text:"删除",
			        iconClass:"egd-button-delete",
			        handler:function(button){
			            var ids = this.dataGrid.getSelected();
			            if(ids.length == 0){
                            Egd.Msg.msgBox({title:"删除附件",msg:"请先选择要删除的附件！"});
                            return;
                        }
			            var oThis = this;
			            Egd.Msg.confirm({title:"删除附件",msg:"确定要删除选中的附件吗？",
                            onYes: function(){
                                oThis.del(ids);
			                    oThis.dataGrid.reload();
                            }
                        })
			        }.bind(this)
			    },
			    {
			        id:"btnDownload",
			        text:"下载",
			        iconClass:"egd-button-download",
			        handler:function(button){
			            var ids = this.dataGrid.getSelected();
				        if(ids.length == 0){
                            Egd.Msg.msgBox({title:"下载附件",msg:"请先选择要下载的附件！"});
                            return;
                        }
			            if(ids.split(";").length >1){
			                Egd.Msg.msgBox({title:"下载附件",msg:"只能一次下载一个文件！"});
                            return;
			            }
				        this.downloadFile(ids, this.config.punid);
			        }.bind(this)
			    },
			    {
			        id:"btnRefresh",
			        text:"刷新",
			        iconClass:"egd-button-refresh",
			        handler:function(button){
				        this.dataGrid.reload();
			        }.bind(this)
			    },
			    {
			        id:"btnType",
			        text: selectText,
			        iconClass:"",
			        showText:true,
			        showIcon:false,
			        unBindEvents: true
		        }
		    ])
        }else{
            this.toolbar = new Egd.TB(this.tbId,[
			    "|",
			    {
			        id:"btnRefresh",
			        text:"刷新",
			        iconClass:"egd-button-refresh",
			        handler:function(button){
				        this.dataGrid.reload();
			        }.bind(this)
			    },
			    {
			        id:"btndownload",
			        text:"下载",
			        iconClass:"egd-button-download",
			        handler:function(button){
				        var ids = this.dataGrid.getSelected();
				        if(ids.length == 0){
                            Egd.Msg.msgBox({title:"下载附件",msg:"请先选择要下载的附件！"});
                            return;
                        }
			            if(ids.split(";").length >1){
			                Egd.Msg.msgBox({title:"下载附件",msg:"只能一次下载一个文件！"});
                            return;
			            }
				        this.downloadFile(ids, this.config.punid);
			        }.bind(this)
			    },
			    {
			        id:"btnType",
			        text: selectText,
			        iconClass:"",
			        showText:true,
			        showIcon:false,
			        unBindEvents: true
		        }
		    ])
        }
    },
    loadData : function(){
        // 构造grid
        var oThis = this;
        var dataUrl = Egd.rootPath +"attachmentAction.do?action=View&parentUnid=" + this.config.punid;
        var typeValue = "";
        if(this.config.types && this.config.types != null && this.config.types != ""){
            if(this.config.types instanceof Array)
                typeValue = $F(this.selectId);
            else
                typeValue = this.config.types;
        }
        dataUrl += "&type=" + typeValue;
        dataUrl += "&timeStamp=" + new Date().getTime();
        this.dataGrid = new Egd.Grid(this.gridId,{
            data :  dataUrl,
            reader: {root: 'rows',totalProperty: 'totalCount',id: 'ID'},
            cm:[
                {id: "FileName",text: "文件名"},
                {id: "Author.Name",text: "作者", width: 100},
                {id: "FileSize",text: "附件大小", width: 100},
                {id: "FileDate",text: "上传时间", width: 150}
            ],
            idColumn: {type:"int",viewType:"checkbox"},
            defaultSort:{name: 'FileDate',direction: 'desc'},
            dblClickRowHandler:function(id,rowData,row){
                oThis.downloadFile(id, oThis.config.punid);
            },
            pageTB : {showTB : false}
        });
        // 渲染表格
        this.dataGrid.render();
    },
    del : function(id){
        var ajaxUrl = Egd.rootPath + "attachmentAction.do";
        var params={
            action:"DelAttachment",
            id:id,
            timestamp:new Date().getTime()
        };
        new Ajax.Request(ajaxUrl,{
            method:"post",
            parameters:params,
            onSuccess:function(transport){
                
            }
        });
    },
    downloadFile : function(id,unid){
        window.open(Egd.rootPath + "Egd_Attachment/DownloadAttachment.aspx?id=" + id + "&unid=" + unid,"blank");
    },
    reload : function(type){
        if(!type) type = "";
        var url = Egd.rootPath +"attachmentAction.do?action=View&parentUnid=" + this.config.punid + "&type=" + type
        this.dataGrid.reload(url);
    }
};