using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Egrand.Util.Configurations;
using Egrand.Web;
using Egrand.Util;
using Egrand.Attachment.Domain;
using Egrand.Attachment.Service;
using Egrand.Util.ResourceHelper;
using Common.Logging;
using System.IO;

namespace Egrand.Attachment.Web
{
    /// <summary>
    /// 下载附件
    /// </summary>
    public partial class DownloadFile : Egrand.Web.UI.Page
    {
        private static ILog logger = LogManager.GetLogger(typeof(DownloadFile));

        #region 相关Service变量定义

        private IAttachmentService attachmentService;

        #endregion

        #region 构造函数

        public DownloadFile()
        {
            this.attachmentService = (IAttachmentService)GetObject("AttachmentService");
        }

        #endregion

        private string errorMsg;
        /// <summary>
        /// 下载附件异常的信息
        /// </summary>
        public string ErrorMsg
        {
            get
            {
                return errorMsg;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request["id"];
            string type = Request["type"];
            if (logger.IsDebugEnabled)
            {
                logger.Debug("type=" + type);
                logger.Debug("id=" + id);
            }

            // 获取要下载的附件
            Egrand.Attachment.Domain.Attachment attachment;
            if ("unid".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                attachment = this.attachmentService.Load(id);
            }
            else
            {
                attachment = this.attachmentService.Load(long.Parse(id));
            }

            // 附件信息不存在的处理
            if (null == attachment)
            {
                this.errorMsg = SimpleResourceHelper.GetString("ATTACHMENT.EXCEPTION.ATM_NOT_EXIST");
                logger.Error(errorMsg + "id=" + id);
                return;
            }

            // 下载文件
            string savePath = attachment.SavePath;
            if (string.IsNullOrEmpty(savePath))
                savePath = SimpleResourceHelper.GetString("ATTACHMENT.DIRECTORY");
            string fileFullPath = FileUtils.GetAbsolutePathName(savePath);
            fileFullPath = Path.Combine(fileFullPath, attachment.ParentUnid);
            fileFullPath = Path.Combine(fileFullPath, attachment.FileName);
            if (logger.IsDebugEnabled)
            {
                logger.Debug("file=" + fileFullPath);
            }
            if (File.Exists(fileFullPath))
            {
                Response.ContentType = "APPLICATION/OCTET-STREAM";
                Response.AddHeader("Content-Disposition", "attachment; filename=" 
                    + HttpUtility.UrlEncode(attachment.FileName, System.Text.Encoding.UTF8));
                Response.WriteFile(fileFullPath);
                Response.End(); // 必须的语句
                return;
            }
            else
            {
                // 文件不存在的处理
                this.errorMsg = SimpleResourceHelper.GetString("ATTACHMENT.EXCEPTION.FILE_NOT_EXIST");
                logger.Error(errorMsg + "file=" + fileFullPath);
                return;
            }
        }
    }
}