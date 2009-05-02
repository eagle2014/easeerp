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
using TSCommon.Core.TSWebContext;
using EaseErp.IC.Service;
using TSCommon.Core.Organize.RelationShips;
using TSLib.Utils;
using TSLib;

namespace TSCommon.Web.WareHouse.action
{
    public class WareHouseAction : StrutsEntityAction<EaseErp.IC.Domain.WareHouse>
    {
        #region 相关Service
        private IWareHouseService wareHouseService;
        private IRelationShipService relationShipService;
        private IPlaceService placeService;
        public WareHouseAction()
        {
            this.wareHouseService = (IWareHouseService)GetObject("WareHouseService");
            this.relationShipService = (IRelationShipService)GetObject("RelationShipService");
            this.placeService = (IPlaceService)GetObject("PlaceService");
        }
        #endregion

        protected override EaseErp.IC.Domain.WareHouse CreateEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext)
        {
            EaseErp.IC.Domain.WareHouse wareHouse = new EaseErp.IC.Domain.WareHouse();
            wareHouse.SetAuthorInfo(TSWEBContext.Current.CurUser);
            wareHouse.FileDate = DateTime.Now;
            return wareHouse;
        }

        protected override void Delete(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string[] ids, string type)
        {
            if ("unid".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                // 删除指定unid集的
                foreach (string parentUnid in ids)
                {
                    //删除关系表
                    this.relationShipService.DeleteAllByParent(parentUnid,
                        EaseErp.IC.Domain.WareHouse.RELATIONSHIP_CODE);
                }
                this.wareHouseService.Delete(ids);
            }
            else
            {
                foreach (string parentid in ids)
                {
                    this.relationShipService.DeleteAllByParent(this.wareHouseService.Load(Convert.ToInt64(parentid)).Unid,
                        EaseErp.IC.Domain.WareHouse.RELATIONSHIP_CODE);
                }
                // 删除指定id集的
                this.wareHouseService.Delete(StringUtils.StringArray2LongArray(ids));
            }
        }

        protected override TSLib.PageInfo GetPageInfo(TSLibStruts.ActionContext actionContext, HttpContext httpContext, int pageNo, int pageSize, string sortField, string sortDir)
        {
            string placeUnid = httpContext.Request.Params["placeUnid"];
            if (!string.IsNullOrEmpty(placeUnid) && placeUnid != "root" && placeUnid != "-1")
            {
                return this.wareHouseService.GetPageByPlace(pageNo, pageSize, sortField, sortDir, null);
            }
            else
            {
                return this.wareHouseService.GetPageByPlace(pageNo, pageSize, sortField, sortDir, this.placeService.Load(long.Parse(placeUnid)));
            }
        }

        protected override EaseErp.IC.Domain.WareHouse LoadEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string idValue, string idName)
        {
            EaseErp.IC.Domain.WareHouse wareHouse;
            if ("unid".Equals(idName, StringComparison.OrdinalIgnoreCase))
            {
                wareHouse = this.wareHouseService.Load(idValue);
            }
            else
            {
                wareHouse = this.wareHouseService.Load(Convert.ToInt32(idValue));
            }
            return wareHouse;
        }

        protected override void SaveEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, EaseErp.IC.Domain.WareHouse entity)
        {
            this.wareHouseService.Save(entity);
        }
    }
}
