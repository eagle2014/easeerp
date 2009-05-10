using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using TSCommon.Core.SystemPolicy.Domain;
using TSLibWeb.Struts;
using TSCommon.Core.SystemPolicy.Service;
using log4net;
using TSCommon.Core.Organize.Domain;
using TSCommon.Core.TSWebContext;
using TSLib.Utils;

namespace TSCommon.Web.SystemPolicy.action
{
    public class PolicyAction : StrutsEntityAction<Policy>
    {
        private static ILog logger = LogManager.GetLogger(typeof(PolicyAction));
        #region 相关Service变量定义

        /// <summary>系统策略配置的Service</summary>
        private IPolicyService policyService;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数，初始化相关Service
        /// </summary>
        public PolicyAction()
        {
            this.policyService = (IPolicyService)GetObject("PolicyService");
        }

        #endregion

        protected override Policy CreateEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext)
        {
            User user = TSWEBContext.Current.CurUser;
            Policy policy = new Policy(user);
            return policy;
        }

        protected override void Delete(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string[] ids, string type)
        {
            if ("unid".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                // 删除指定unid集的
                this.policyService.Delete(ids);
            }
            else
            {
                // 删除指定id集的
                this.policyService.Delete(StringUtils.StringArray2LongArray(ids));
            }
        }

        protected override TSLib.PageInfo GetPageInfo(TSLibStruts.ActionContext actionContext, HttpContext httpContext, int pageNo, int pageSize, string sortField, string sortDir)
        {
            return this.policyService.GetPage(pageNo, pageSize, sortField, sortDir);
        }

        protected override Policy LoadEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string idValue, string idName)
        {
            Policy policy;
            if ("unid".Equals(idName, StringComparison.OrdinalIgnoreCase))
            {
                policy = this.policyService.Load(idValue);
            }
            else
            {
                policy = this.policyService.Load(Convert.ToInt32(idValue));
            }
            return policy;
        }

        protected override void SaveEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, Policy entity)
        {
            User user = TSWEBContext.Current.CurUser;
            this.policyService.Save(user, entity);
        }
    }
}
