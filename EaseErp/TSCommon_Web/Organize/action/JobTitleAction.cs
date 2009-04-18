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
using TSCommon_Core.Organize.Domain;
using TSCommon_Core.Organize.Service;
using TSLib.Utils;
using TSLibStruts;
using log4net;
using TSLib;

namespace TSCommon_Web.Organize.action
{
    public class JobTitleAction : StrutsEntityAction<JobTitle>
    {
        private static ILog logger = LogManager.GetLogger(typeof(JobTitleAction));

        #region 相关Service变量定义

        /// <summary>职务配置的Service</summary>
        private IJobTitleService jobTitleService;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数，初始化相关Service
        /// </summary>
        public JobTitleAction()
        {
            this.jobTitleService = (IJobTitleService)GetObject("JobTitleService");
        }

        #endregion

        #region StrutsEntityAction的实现

        protected override JobTitle CreateEntity(ActionContext actionContext, HttpContext httpContext)
        {
            JobTitle jobTitle = new JobTitle();
            jobTitle.Level = Constants.BLANK_STRING_VALUE;
            jobTitle.LevelName = Constants.BLANK_STRING_VALUE;
            return jobTitle;
        }

        protected override JobTitle LoadEntity(ActionContext actionContext, HttpContext httpContext,string idValue, string idName)
        {
            JobTitle jobTitle;
            if ("unid".Equals(idName, StringComparison.OrdinalIgnoreCase))
            {
                jobTitle = this.jobTitleService.Load(idValue);
            }
            else
            {
                jobTitle = this.jobTitleService.Load(Convert.ToInt32(idValue));
            }
            return jobTitle;
        }

        protected override void SaveEntity(ActionContext actionContext, HttpContext httpContext,JobTitle entity)
        {
            this.jobTitleService.Save(entity);
        }

        protected override void Delete(ActionContext actionContext, HttpContext httpContext,string[] ids, string type)
        {
            if ("unid".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                // 删除指定unid集的电子公告
                this.jobTitleService.Delete(ids);
            }
            else
            {
                // 删除指定id集的电子公告
                this.jobTitleService.Delete(StringUtils.StringArray2LongArray(ids));
            }
        }

        protected override PageInfo GetPageInfo(ActionContext actionContext, HttpContext httpContext,
            int pageNo, int pageSize, string sortField, string sortDir)
        {
            return this.jobTitleService.GetPage(pageNo, pageSize, sortField, sortDir);
        }

        #endregion
    }
}
