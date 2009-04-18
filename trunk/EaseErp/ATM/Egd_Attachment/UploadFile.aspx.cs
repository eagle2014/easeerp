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
using Common.Logging;
using Egrand.Json;
using Newtonsoft.Json;
using Egrand.Util;
using Egrand.Attachment.Service;
using Egrand.Util.ResourceHelper;
using System.IO;
using System.Collections.Generic;
using Egrand.Exceptions;

namespace Egrand.Attachment.Web
{
    /// <summary>
    /// �ϴ�����
    /// </summary>
    public partial class UploadFile : Egrand.Web.UI.Page
    {
        private static ILog logger = LogManager.GetLogger(typeof(UploadFile));

        #region ���Service��������

        private IAttachmentService attachmentService;

        #endregion

        #region ���캯��

        public UploadFile()
        {
            this.attachmentService = (IAttachmentService)GetObject("AttachmentService");
        }

        #endregion


        private string atmsJson;
        /// <summary>
        /// �������ϴ���������Ϣ��Json�ַ�����������Attachment��Ӧ
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

            // ��ȡ���������ĵ���Unid
            // this.punid = Request["punid"];

            // �����ϴ��ĸ���
            try
            {
                logger.Debug("begin1");
                if (this.CurUserInfo == null)
                    throw new ResourceException("SYS.SESSION_OUT_DATE");
                this.saveUploadFile();
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message, ee);
                this.ErrorMsg.Value = ee.Message;
            }
        }

        /// <summary>
        /// �����ϴ��ĸ���
        /// </summary>
        private void saveUploadFile()
        {
            if (Request.Files.Count <= 0) return;

            // ��ȡ�ϴ����ļ�
            HttpPostedFile file = Request.Files[0];
            //if (string.IsNullOrEmpty(file.FileName)) return;
            if (logger.IsDebugEnabled)
            {
                logger.Debug("ContentType=" + file.ContentType);
                logger.Debug("ContentLength=" + file.ContentLength);
                logger.Debug("ClientFileName=" + file.FileName);
            }

            // ����Ϊ����
            IList attachments = saveFiles();

            // ��¼����ĸ�������Ϣ
            JavaScriptArray jsonArray = new JavaScriptArray();
            JavaScriptObject jsonObj;
            foreach (Egrand.Attachment.Domain.Attachment attachment in attachments)
            {
                jsonObj = new JavaScriptObject();
                jsonObj.Add("ID",attachment.ID);
                jsonObj.Add("Unid", attachment.Unid);
                jsonObj.Add("ParentUnid", attachment.ParentUnid);
                jsonObj.Add("Subject", attachment.Subject);
                jsonObj.Add("Author",attachment.Author.Name);
                jsonObj.Add("FileSize",attachment.FileSize);
                jsonObj.Add("FileDate",attachment.FileDate.ToString("yyyy-MM-dd"));
                jsonObj.Add("FileFormat",attachment.FileFormat);
                jsonObj.Add("FileName", attachment.FileName);
                jsonObj.Add("Type",attachment.Type);
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

            // ����·��
            string serverPath = FileUtils.GetAbsolutePathName(SimpleResourceHelper.GetString("ATTACHMENT.DIRECTORY"));
            serverPath = Path.Combine(serverPath, this.PUnid.Value);
            if (!Directory.Exists(serverPath))
                Directory.CreateDirectory(serverPath);
            logger.Debug("begin2" + serverPath);
            HttpPostedFile file;
            for (int i = 0; i < Request.Files.Count; i++ )
            {
                file = Request.Files[i];
                if (string.IsNullOrEmpty(file.FileName))
                    continue;

                int index = file.FileName.LastIndexOf(".");
                if (index == -1)
                {
                    throw new Exception("�������ϴ�������չ�����ļ���");
                }

                string extension = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);         // ��չ��
                string clientFileName = file.FileName.Substring(file.FileName.LastIndexOf(@"\") + 1);   // �ļ���+��չ��

                // �ϴ��ļ�
                string serverFile = Path.Combine(serverPath, clientFileName);
                if (logger.IsInfoEnabled)
                {
                    logger.Info("serverFile=" + serverFile);
                }
                // ���������ļ�
                file.SaveAs(serverFile);

                // ����������Ϣ
                atms.Add(this.createAttachment(clientFileName, clientFileName, extension, file.ContentLength));
            }
            this.attachmentService.Save(atms);

            return atms;
        }

        protected Egrand.Attachment.Domain.Attachment createAttachment(string subject, string fileName, string fileFormat, int fileSize)
        {
            Egrand.Attachment.Domain.Attachment attachment = new Egrand.Attachment.Domain.Attachment();
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
            attachment.Author = this.CurUserInfo;
            attachment.SavePath = SimpleResourceHelper.GetString("ATTACHMENT.DIRECTORY");
            return attachment;
        }

    }
}
