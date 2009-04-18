using System;
using System.Web;
using Common.Logging;
using System.Collections;
using Newtonsoft.Json;
using Egrand.Json;
using Egrand.Struts;
using Egrand.Organize.Domain;
using Egrand.Util;
using Egrand.Organize.Service;
using Egrand.Util.ResourceHelper;
using Egrand.Attachment.Service;
using Egrand.Exceptions;

namespace Egrand.Attachment.Action
{
    /// <summary>
    /// ��������Action
    /// </summary>
    /// <author>dragon</author>
    /// <date>2008-08-16</date>
    public class AttachmentAction : Egrand.Web.StrutsEntityAction<Egrand.Attachment.Domain.Attachment>
    {
        private static ILog logger = LogManager.GetLogger(typeof(AttachmentAction));

        #region ���Service��������

        /// <summary>OU���õ�Service</summary>
        private IAttachmentService attachmentService;

        #endregion

        #region ���캯��

        /// <summary>
        /// ���캯������ʼ�����Service
        /// </summary>
        public AttachmentAction()
        {
            this.attachmentService = (IAttachmentService)GetObject("AttachmentService");
        }

        #endregion

        #region StrutsEntityAction��ʵ��

        protected override string ResourceKey_DeleteSuccessMsg
        {
            get
            {
                return "ATTACHMENT.DELETE.WEB.SUCCESS";
            }
        }
        protected override void Delete(ActionContext actionContext, HttpContext httpContext, UserInfo curUserInfo, string[] ids, string type)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug("type=" + type);
                logger.Debug("ids=" + StringUtils.Combine(ids,","));
            }
            if ("unid".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                // ɾ��ָ��unid���ĸ���
                this.attachmentService.Delete(curUserInfo, ids);
            }
            else
            {
                // ɾ��ָ��id���ĸ���
                this.attachmentService.Delete(curUserInfo, Egrand.Util.StringUtils.StringArray2LongArray(ids));
            }
        }

        protected override PageInfo GetPageInfo(ActionContext actionContext, HttpContext httpContext, UserInfo curUserInfo, int pageNo, int pageSize, string sortField, string sortDir)
        {
            string punid = RequestUtils.GetStringParameter(httpContext, "punid", "");
            string type = RequestUtils.GetStringParameter(httpContext, "type", "");
            if (logger.IsDebugEnabled)
                logger.Debug("punid = " + punid + " type = " + type);
            pageNo = 1;
            pageSize = int.MaxValue;
            if (string.IsNullOrEmpty(punid))
                return this.attachmentService.GetPage(pageNo, pageSize, sortField, sortDir, type);
            return this.attachmentService.GetPage(pageNo, pageSize, sortField, sortDir, punid, type);
        }

        protected override Egrand.Attachment.Domain.Attachment CreateEntity(ActionContext actionContext, HttpContext httpContext, UserInfo curUserInfo)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override Egrand.Attachment.Domain.Attachment LoadEntity(ActionContext actionContext, HttpContext httpContext, UserInfo curUserInfo, string idValue, string idName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void SaveEntity(ActionContext actionContext, HttpContext httpContext, UserInfo curUserInfo, Egrand.Attachment.Domain.Attachment entity)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}

