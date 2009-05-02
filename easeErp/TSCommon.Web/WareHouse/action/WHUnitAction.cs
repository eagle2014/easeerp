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
using EaseErp.IC.Service;
using TSLib.Utils;

namespace TSCommon.Web.WareHouse.action
{
    public class WHUnitAction : StrutsEntityAction<EaseErp.IC.Domain.WHUnit>
    {
        #region Ïà¹ØService

        private IWHUnitService whunitService;

        public WHUnitAction()
        {
            this.whunitService = (IWHUnitService)GetObject("WHUnitService");
        }

        #endregion

        protected override EaseErp.IC.Domain.WHUnit CreateEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext)
        {
            return new EaseErp.IC.Domain.WHUnit();
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

        protected override EaseErp.IC.Domain.WHUnit LoadEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string idValue, string idName)
        {
            EaseErp.IC.Domain.WHUnit unit;
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

        protected override void SaveEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, EaseErp.IC.Domain.WHUnit entity)
        {
            this.whunitService.Save(entity);
        }
    }
}
