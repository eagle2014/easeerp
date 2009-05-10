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
using log4net;
using TSCommon.Core.ATM.Service;
using TSLibWeb.Utils;
using Newtonsoft.Json;
using TSLib.Utils;
using TSLib;
using System.IO;
using Lib;
using TSCommon.Core.TSWebContext;

namespace TSCommon.Web.ATM
{
    public partial class UploadFile : TSLibWeb.WEB.Page
    {
        private static ILog logger = LogManager.GetLogger(typeof(UploadFile));

        #region 相关Service变量定义

        private IATMService atmService;

        #endregion

        #region 构造函数

        public UploadFile()
        {
            this.atmService = (IATMService)GetObject("ATMService");
        }

        #endregion


        private string atmsJson;
        /// <summary>
        /// 包含所上传附件的信息的Json字符串，属性与Attachment对应
        /// </summary>
        public string AtmsJson
        {
            get
            {
                return string.IsNullOrEmpty(atmsJson) ? "null" : atmsJson;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.PUnid.Value = RequestUtils.GetStringParameter(this.Context, "punid", string.Empty);
            this.Caller.Value = RequestUtils.GetStringParameter(this.Context, "caller", string.Empty);
            this.Type.Value = RequestUtils.GetStringParameter(this.Context, "type", string.Empty);
            this.Filter.Value = RequestUtils.GetStringParameter(this.Context, "filter", string.Empty);
            this.MaxSize.Value = RequestUtils.GetStringParameter(this.Context, "maxSize", string.Empty);
            this.ErrorMsg.Value = string.Empty;
            if (logger.IsInfoEnabled)
            {
                logger.Info("punid=" + this.PUnid.Value);
                logger.Info("caller=" + this.Caller.Value);
                logger.Info("type=" + this.Type.Value);
                logger.Info("filter=" + this.Filter.Value);
                logger.Info("maxSize=" + this.MaxSize.Value);
            }

            // 获取附件所属文档的Unid
            // this.punid = Request["punid"];

            // 保存上传的附件
            try
            {
                logger.Debug("begin1");
                this.saveUploadFile();
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message, ee);
                this.ErrorMsg.Value = ee.Message;
            }
        }

        /// <summary>
        /// 保存上传的附件
        /// </summary>
        private void saveUploadFile()
        {
            if (Request.Files.Count <= 0) return;

            // 获取上传的文件
            HttpPostedFile file = Request.Files[0];
            //if (string.IsNullOrEmpty(file.FileName)) return;
            if (logger.IsDebugEnabled)
            {
                logger.Debug("ContentType=" + file.ContentType);
                logger.Debug("ContentLength=" + file.ContentLength);
                logger.Debug("ClientFileName=" + file.FileName);
            }

            // 保存为附件
            IList attachments = saveFiles();

            // 记录保存的附件的信息
            JavaScriptArray jsonArray = new JavaScriptArray();
            JavaScriptObject jsonObj;
            foreach (TSCommon.Core.ATM.Domain.ATM attachment in attachments)
            {
                jsonObj = new JavaScriptObject();
                jsonObj.Add("ID", attachment.ID);
                jsonObj.Add("Unid", attachment.Unid);
                jsonObj.Add("ParentUnid", attachment.ParentUnid);
                jsonObj.Add("Subject", attachment.Subject);
                jsonObj.Add("Author", attachment.Author.Name);
                jsonObj.Add("FileSize", attachment.FileSize);
                jsonObj.Add("FileDate", attachment.FileDate.ToString("yyyy-MM-dd"));
                jsonObj.Add("FileFormat", attachment.FileFormat);
                jsonObj.Add("FileName", attachment.FileName);
                jsonObj.Add("Type", attachment.Type);
                jsonArray.Add(jsonObj);
            }
            this.atmsJson = (attachments != null && attachments.Count > 0) ? JavaScriptConvert.SerializeObject(jsonArray) : "null";

            if (logger.IsDebugEnabled)
            {
                logger.Debug("atmsJson=" + atmsJson);
            }
        }
        public IList saveFiles()
        {
            IList atms = new ArrayList();

            // 创建路径
            string serverPath = FileUtils.GetAbsolutePathName(SimpleResourceHelper.GetString("ATTACHMENT.DIRECTORY"));
            serverPath = Path.Combine(serverPath, this.PUnid.Value);
            if (!Directory.Exists(serverPath))
                Directory.CreateDirectory(serverPath);
            logger.Debug("begin2" + serverPath);
            HttpPostedFile file;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                file = Request.Files[i];
                if (string.IsNullOrEmpty(file.FileName))
                    continue;

                int index = file.FileName.LastIndexOf(".");
                if (index == -1)
                {
                    throw new Exception("不允许上传不明扩展名的文件！");
                }

                string extension = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);         // 扩展名
                string clientFileName = file.FileName.Substring(file.FileName.LastIndexOf(@"\") + 1);   // 文件名+扩展名

                // 上传文件
                string serverFile = Path.Combine(serverPath, clientFileName);
                if (logger.IsInfoEnabled)
                {
                    logger.Info("serverFile=" + serverFile);
                }
                // 保存物理文件
                file.SaveAs(serverFile);

                // 创建附件信息
                atms.Add(this.createAttachment(clientFileName, clientFileName, extension, file.ContentLength));
            }
            this.atmService.Save(atms);

            return atms;
        }

        protected TSCommon.Core.ATM.Domain.ATM createAttachment(string subject, string fileName, string fileFormat, int fileSize)
        {
            TSCommon.Core.ATM.Domain.ATM attachment = new TSCommon.Core.ATM.Domain.ATM();
            attachment.ID = -1;
            attachment.Unid = Entry.NewUnid;
            attachment.Type = this.Type.Value;
            attachment.ParentUnid = this.PUnid.Value;
            attachment.Subject = subject;
            attachment.FileName = fileName;
            attachment.FileFormat = fileFormat;
            if (fileSize / 1024 > 1)
                attachment.FileSize = (fileSize / 1024).ToString() + "KB";
            else
                attachment.FileSize = fileSize.ToString() + "B";
            attachment.FileDate = DateTime.Now;
            attachment.Author = TSWEBContext.Current.CurUser;
            attachment.SavePath = SimpleResourceHelper.GetString("ATM.DIRECTORY");
            return attachment;
        }
    }
}
