using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using TSLibWeb.Struts;
using TSCommon_Core.ATM.Service;
using log4net;
using TSLib.Utils;
using TSLibWeb.Utils;

namespace TSCommon_Web.ATM.action
{
    public class ATMAction : StrutsEntityAction<TSCommon_Core.ATM.Domain.ATM>
    {
        private ILog logger = LogManager.GetLogger(typeof(ATMAction));
        #region 相关的Service
        private IATMService atmService;
        public ATMAction()
        {
            this.atmService = (IATMService)GetObject("ATMService");
        }
        #endregion

        protected override TSCommon_Core.ATM.Domain.ATM CreateEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void Delete(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string[] ids, string type)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug("type=" + type);
                logger.Debug("ids=" + StringUtils.Combine(ids, ","));
            }
            if ("unid".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                // 删除指定unid集的附件
                this.atmService.Delete(ids);
            }
            else
            {
                // 删除指定id集的附件
                this.atmService.Delete(StringUtils.StringArray2LongArray(ids));
            }
        }

        protected override TSLib.PageInfo GetPageInfo(TSLibStruts.ActionContext actionContext, HttpContext httpContext, int pageNo, int pageSize, string sortField, string sortDir)
        {
            string punid = RequestUtils.GetStringParameter(httpContext, "punid", "");
            string type = RequestUtils.GetStringParameter(httpContext, "type", "");
            if (logger.IsDebugEnabled)
                logger.Debug("punid = " + punid + " type = " + type);
            pageNo = 1;
            pageSize = int.MaxValue;
            if (string.IsNullOrEmpty(punid))
                return this.atmService.GetPage(pageNo, pageSize, sortField, sortDir);
            return this.atmService.GetPage(pageNo, pageSize, sortField, sortDir, punid, type);
        }

        protected override TSCommon_Core.ATM.Domain.ATM LoadEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string idValue, string idName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void SaveEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, TSCommon_Core.ATM.Domain.ATM entity)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
