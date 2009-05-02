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
using TSCommon.Core.Organize.Domain;
using log4net;
using TSCommon.Core.Organize.Service;
using TSLibStruts;
using TSLib.Utils;
using TSLib;

namespace TSCommon.Web.Organize.action
{
    public class OULevelAction : StrutsEntityAction<OULevel>
    {
        private static ILog logger = LogManager.GetLogger(typeof(OULevelAction));

        #region 相关Service变量定义

        /// <summary>级别配置的Service</summary>
        private IOULevelService ouLevelService;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数，初始化相关Service
        /// </summary>
        public OULevelAction()
        {
            this.ouLevelService = (IOULevelService)GetObject("OULevelService");
        }

        #endregion

        #region StrutsEntityAction的实现

        protected override OULevel CreateEntity(ActionContext actionContext, HttpContext httpContext)
        {
            OULevel ouLevel = new OULevel();
            return ouLevel;
        }

        protected override OULevel LoadEntity(ActionContext actionContext, HttpContext httpContext, string idValue, string idName)
        {
            OULevel ouLevel;
            if ("unid".Equals(idName, StringComparison.OrdinalIgnoreCase))
            {
                ouLevel = this.ouLevelService.Load(idValue);
            }
            else
            {
                ouLevel = this.ouLevelService.Load(Convert.ToInt32(idValue));
            }
            return ouLevel;
        }

        protected override void SaveEntity(ActionContext actionContext, HttpContext httpContext, OULevel entity)
        {
            this.ouLevelService.Save(entity);
        }

        protected override void Delete(ActionContext actionContext, HttpContext httpContext,
            string[] ids, string type)
        {
            if ("unid".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                // 删除指定unid集的
                this.ouLevelService.Delete(ids);
            }
            else
            {
                // 删除指定id集的
                this.ouLevelService.Delete(StringUtils.StringArray2LongArray(ids));
            }
        }

        protected override PageInfo GetPageInfo(ActionContext actionContext, HttpContext httpContext,
            int pageNo, int pageSize, string sortField, string sortDir)
        {
            return this.ouLevelService.GetPage(pageNo, pageSize, sortField, sortDir);
        }

        #endregion
    }
}
