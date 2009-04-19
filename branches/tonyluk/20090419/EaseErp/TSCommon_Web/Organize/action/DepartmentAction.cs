using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using TSCommon_Core.Organize.Domain;
using TSLibWeb.Struts;
using log4net;
using TSCommon_Core.Organize.Service;
using TSLibStruts;
using TSLib;
using TSLib.Utils;

namespace TSCommon_Web.Organize.action
{
    public class DepartmentAction : StrutsEntityAction<OUInfo>
    {
        private static ILog logger = LogManager.GetLogger(typeof(DepartmentAction));

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
        public DepartmentAction()
        {
            this.ouInfoService = (IOUInfoService)GetObject("OUInfoService");
            this.ouLevelService = (IOULevelService)GetObject("OULevelService");
        }

        #endregion

        protected override string DomainName
        {
            get
            {
                return "Department";
            }
        }

        #region StrutsEntityAction的实现

        protected override OUInfo CreateEntity(ActionContext actionContext, HttpContext httpContext)
        {
            OUInfo ouInfo = new OUInfo();
            ouInfo.Type = OUInfo.OT_DEPARTMENT;
            ouInfo.OUStatus = OUStatuses.Enable;
            ouInfo.IsTmpOU = Constants.YESNO_NO;
            ouInfo.Level = SimpleResourceHelper.GetString("OUINFO.DEPARTMENT.DEFAULT_LEVEL");
            ouInfo.LevelName = SimpleResourceHelper.GetString("OUINFO.DEPARTMENT.DEFAULT_LEVEL_NAME");
            return ouInfo;
        }

        protected override OUInfo LoadEntity(ActionContext actionContext, HttpContext httpContext, string idValue, string idName)
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

        protected override PageInfo GetPageInfo(ActionContext actionContext, HttpContext httpContext,int pageNo, int pageSize, string sortField, string sortDir)
        {
            //return this.ouInfoService.GetPageByType(curUserInfo, OUInfo.OT_DEPARTMENT, pageNo, pageSize, sortField, sortDir, null);

            string punid = httpContext.Request.Params["punid"];
            logger.Debug("punid=" + punid);
            if (!string.IsNullOrEmpty(punid) && punid != "root" && punid != "-1")
            {
                return this.ouInfoService.GetDepartmentPage(punid, pageNo, pageSize, sortField, sortDir, null);
            }
            else
            {
                return this.ouInfoService.GetDepartmentPage(string.Empty, pageNo, pageSize, sortField, sortDir, null);
            }
        }

        #endregion
    }
}
