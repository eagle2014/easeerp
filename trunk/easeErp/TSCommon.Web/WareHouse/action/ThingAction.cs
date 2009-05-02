using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using EaseErp.IC.Domain;
using TSLibWeb.Struts;
using EaseErp.IC.Service;
using TSLib.Utils;
using log4net;

namespace TSCommon.Web.WareHouse.action
{
    public class ThingAction:StrutsEntityAction<Thing>
    {
        private ILog logger = LogManager.GetLogger(typeof(ThingAction));
        #region Ïà¹ØµÄService

        private IThingService thingService;
        public ThingAction()
        {
            this.thingService = (IThingService)GetObject("ThingService");
        }
        #endregion

        protected override Thing CreateEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext)
        {
            return new Thing();
        }

        protected override void Delete(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string[] ids, string type)
        {
            if ("unid".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                this.thingService.Delete(ids);
            }
            else
            {
                this.thingService.Delete(StringUtils.StringArray2LongArray(ids));
            }
        }

        protected override TSLib.PageInfo GetPageInfo(TSLibStruts.ActionContext actionContext, HttpContext httpContext, int pageNo, int pageSize, string sortField, string sortDir)
        {
            return this.thingService.GetPage(pageNo, pageSize, sortField, sortDir);
        }

        protected override Thing LoadEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string idValue, string idName)
        {
            EaseErp.IC.Domain.Thing thing;
            if ("unid".Equals(idName, StringComparison.OrdinalIgnoreCase))
            {
                thing = this.thingService.Load(idValue);
            }
            else
            {
                thing = this.thingService.Load(Convert.ToInt32(idValue));
            }
            return thing;
        }

        protected override void SaveEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, Thing entity)
        {
            if (entity.WHUnit.ID == -1)
                entity.WHUnit = null;
            this.thingService.Save(entity);
        }
    }
}
