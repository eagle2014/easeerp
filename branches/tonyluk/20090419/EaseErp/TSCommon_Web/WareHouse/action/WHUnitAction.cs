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
using EaseErp_WareHouse.WareHouse.Service;
using TSLib.Utils;

namespace TSCommon_Web.WareHouse.action
{
    public class WHUnitAction : StrutsEntityAction<EaseErp_WareHouse.WareHouse.Domain.WHUnit>
    {
        #region Ïà¹ØService

        private IWHUnitService whunitService;

        public WHUnitAction()
        {
            this.whunitService = (IWHUnitService)GetObject("WHUnitService");
        }

        #endregion

        protected override EaseErp_WareHouse.WareHouse.Domain.WHUnit CreateEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext)
        {
            return new EaseErp_WareHouse.WareHouse.Domain.WHUnit();
        }

        protected override void Delete(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string[] ids, string type)
        {
            if ("unid".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                this.whunitService.Delete(ids);
            }
            else
            {
                this.whunitService.Delete(StringUtils.StringArray2LongArray(ids));
            }
        }

        protected override TSLib.PageInfo GetPageInfo(TSLibStruts.ActionContext actionContext, HttpContext httpContext, int pageNo, int pageSize, string sortField, string sortDir)
        {
                return this.whunitService.GetPage(pageNo, pageSize, sortField, sortDir);
        }

        protected override EaseErp_WareHouse.WareHouse.Domain.WHUnit LoadEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string idValue, string idName)
        {
            EaseErp_WareHouse.WareHouse.Domain.WHUnit unit;
            if ("unid".Equals(idName, StringComparison.OrdinalIgnoreCase))
            {
                unit = this.whunitService.Load(idValue);
            }
            else
            {
                unit = this.whunitService.Load(Convert.ToInt32(idValue));
            }
            return unit;
        }

        protected override void SaveEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, EaseErp_WareHouse.WareHouse.Domain.WHUnit entity)
        {
            this.whunitService.Save(entity);
        }
    }
}
