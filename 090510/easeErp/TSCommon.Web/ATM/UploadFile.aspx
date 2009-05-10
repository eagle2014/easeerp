<%@ Page Language="C#" AutoEventWireup="true" Codebehind="UploadFile.aspx.cs" Inherits="TSCommon.Web.ATM.UploadFile" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>文件</title>
        <script language="javascript" src="<%=ContextPath %>/TS_platform/js/TS-Include-FormPage.js?rootPath=<%=ContextPath %>/"
        type="text/javascript"></script>

    <script src="ATM/js/ATM.js" type="text/javascript"></script>

</head>
<body class="egd-form-dlgBg-WB" onload="init();" style="overflow: hidden;">
    <form id="thisForm" runat="server" enctype="multipart/form-data" method="post">
        <div style="vertical-align: middle; border: 0px solid blue;">
            <asp:FileUpload ID="egdFileUpload" runat="server" Style="display: none" />
            <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" style="table-layout: fixed;">
                <tr>
                    <td height="21px" class="atmTop">
                        <div class="atmTop-left">
                            已添加 <span id="FileNum" style="color: #993300;">0</span> 个文件</div>
                        <div class="atmTop-right">
                            <a id="container1" class="addfile" style="border: 0px; cursor: pointer;">
                                <input id="File1" name="file_0" type="file" style="width: 0px;" class="addfile" onchange="createnew();"
                                    runat="server" />
                            </a>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <input id="btnSubmit" type="button" value="提交" onclick="submitForm();" style="display: none;" />
                        <asp:HiddenField ID="PUnid" runat="server" />
                        <asp:HiddenField ID="Type" runat="server" />
                        <asp:HiddenField ID="Caller" runat="server" />
                        <asp:HiddenField ID="Filter" runat="server" />
                        <asp:HiddenField ID="MaxSize" runat="server" />
                        <asp:HiddenField ID="ErrorMsg" runat="server" />
                        <asp:HiddenField ID="AllPath" runat="server" />
                        <div class="fileList-overflow">
                            <div id="container2">
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

<script language="JavaScript" type="text/javascript">
    var fileNum=0;
    var fileNumber = 0;
    function createnew()
    {
        var c_a = $('container1');// 找到上传控件的a容器
        var c_div = $('container2');// 放置文件的容器
        var fileCtr = c_a.firstChild;// 上传控件
        
        if(navigator.appName.indexOf("Explorer") > -1){
            fileCtr = c_a.firstChild;
        } 
        else{
            fileCtr=getFirstFileCtrInFF(c_a);
        }
        
        var subDiv = document.createElement("div");// 将放置到c_div中的容器
		subDiv.className="subDiv";
        var span1 = document.createElement("span");// 上传的文件
        
        if(navigator.appName.indexOf("Explorer") > -1){
            span1.innerText =fileCtr.value.substring(fileCtr.value.lastIndexOf('\\')+1,fileCtr.value.length);
        } 
        else{
            span1.textContent =fileCtr.value.substring(fileCtr.value.lastIndexOf('\\')+1,fileCtr.value.length);
        }        
        
        var imgFileType=document.createElement("div");
        imgFileType.className="fileComm file-"+fileCtr.value.substring(fileCtr.value.lastIndexOf('.')+1,fileCtr.value.length);
        
        var img2 = document.createElement("img");// 删除图片按钮
        img2.src="images/default/delFile.gif";
		img2.border = "0";
		img2.align = "absmiddle";
        img2.onclick = function(){this.parentNode.parentNode.removeChild(this.parentNode);fileNum--;DisplayFileNum(fileNum);}
        
        fileCtr.style.height=0;
        
        subDiv.appendChild(imgFileType);
        subDiv.appendChild(span1);
        subDiv.appendChild(img2);        
        subDiv.appendChild(fileCtr);        
        
        
        c_div.appendChild(subDiv);
        fileNumber++;
        fileNum++;
        
        DisplayFileNum(fileNum);
        
        var newFileCtr = document.createElement("input");
        newFileCtr.type = "file";
        newFileCtr.className = "addfile";
        newFileCtr.runat = "server";
        newFileCtr.name = "file_"+fileNumber;
        newFileCtr.onchange = createnew;
        while(c_a.firstChild)
        {
            c_a.removeChild(c_a.firstChild);
        }
        
        c_a.appendChild(newFileCtr);
    }
  //在FF中取得上传input
  function getFirstFileCtrInFF(c_a)
  {
        var tmp;
        var fileCtrs = c_a.childNodes;
              for(var i=0; i<fileCtrs.length; i++) {
                 if(fileCtrs[i].type=="file")
                 {
                    return fileCtrs[i]; 
                 } 
             }
             return tmp;
  }
  //显示文件个数
  function DisplayFileNum(value)
  {
         if(navigator.appName.indexOf("Explorer") > -1){
            $("FileNum").innerText =value;
        } 
        else{
            $("FileNum").textContent =value;
        }
  }
  
  //是否有文件要上传
  function IfEmpty()
  {
        if(fileNum<=0)
            return false;
        else
            return true;  
  }
</script>

<script type="text/javascript">
/*
 * 提交后的文件信息,类型为Array
 */
var atms = <%=this.AtmsJson %>;

/*
 * 提交表单上传文件
 */
function submitForm(){
    if(validate())
        $("thisForm").submit();
}

function validate(){
    if(!IfEmpty()){
        TS.Msg.msgBox("请先选择要上传的文件！");
        return false;
    }
    return true;
}

/*
 * 更新调用者的信息
 */
function init(){
    fileArray=new Array();
    var errorMsg = $F('ErrorMsg');
    if(errorMsg.length > 0){
        TS.Msg.msgBox(errorMsg);
    }else{
        if(atms)    // 表单提交成功
            TS.Dlg.fireButtonEvent(0,'egdUploadFileDlg')
    }
}

/*
 * 返回上传结果的函数
 */
function DlgOkFn(){
    if(atms)    // 表单提交成功就返回
        return {
            atms: atms,
            caller: $F('Caller'),
            punid: $F('PUnid')
        };
    else
        submitForm();
    
    /*
    // 测试代码
    return {
        caller: 'ATM01',
        atms: [
            {Unid: 'newUnid01', Subject: '测试文档01.doc', Author: '测试用户01', FileSize: '12KB', FileDate: '2008-10-10'},
            {Unid: 'newUnid02', Subject: '测试文档02.doc', Author: '测试用户02', FileSize: '370KB', FileDate: '2008-11-11'}
        ]
    };
    */
}
</script>

