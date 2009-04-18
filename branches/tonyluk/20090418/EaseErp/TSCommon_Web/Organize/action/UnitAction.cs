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
using log4net;
using TSCommon_Core.Organize.Service;
using TSLibWeb;
using TSLibStruts;
using TSLib.Utils;
using TSLib;
using TSCommon_Core.TSWebContext;

namespace TSCommon_Web.Organize.action
{
    public class UnitAction : StrutsEntityAction<OUInfo>
    {
        private static ILog logger = LogManager.GetLogger(typeof(UnitAction));

        #region 相关Service变量定义

        /// <summary>OU配置的Service</summary>
        private IOUInfoService ouInfoService;

        /// <summary>级别配置的Service</summary>
        private IOULevelService ouLevelService;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数，初始化相关Service
        /// </summary>
        public UnitAction()
        {
            this.ouInfoService = (IOUInfoService)GetObject("OUInfoService");
            this.ouLevelService = (IOULevelService)GetObject("OULevelService");
        }

        #endregion

        protected override string DomainName
        {
            get
            {
                return "Unit";
            }
        }

        #region StrutsEntityAction的实现

        protected override OUInfo CreateEntity(ActionContext actionContext, HttpContext httpContext)
        {
            OUInfo ouInfo = new OUInfo();
            ouInfo.Type = OUInfo.OT_UNIT;
            ouInfo.OUStatus = OUStatuses.Enable;
            ouInfo.IsTmpOU = TSLib.Utils.Constants.YESNO_NO;
            ouInfo.Level = TSLib.Utils.Constants.BLANK_STRING_VALUE;
            return ouInfo;
        }

        protected override OUInfo LoadEntity(ActionContext actionContext, HttpContext httpContext,
            string idValue, string idName)
        {
            OUInfo ouInfo;
            if ("unid".Equals(idName, StringComparison.OrdinalIgnoreCase))
            {
                ouInfo = this.ouInfoService.Load(idValue);
            }
            else
            {
                ouInfo = this.ouInfoService.Load(Convert.ToInt32(idValue));
            }
            return ouInfo;
        }

        protected override void SaveEntity(ActionContext actionContext, HttpContext httpContext,OUInfo entity)
        {
            this.ouInfoService.Save(entity);
        }

        protected override void Delete(ActionContext actionContext, HttpContext httpContext,string[] ids, string type)
        {
            if ("unid".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                // 删除指定unid集的
                this.ouInfoService.Delete(ids);
            }
            else
            {
                // 删除指定id集的
                this.ouInfoService.Delete(StringUtils.StringArray2LongArray(ids));
            }
        }

        protected override PageInfo GetPageInfo(ActionContext actionContext, HttpContext httpContext,
            int pageNo, int pageSize, string sortField, string sortDir)
        {
            return this.ouInfoService.GetPageByType(TSWEBContext.Current.CurUser, OUInfo.OT_UNIT, pageNo, pageSize, sortField, sortDir, null);
        }

        #endregion
    }
}
