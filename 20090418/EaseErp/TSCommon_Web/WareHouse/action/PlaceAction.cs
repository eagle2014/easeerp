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
using EaseErp_WareHouse.WareHouse.Domain;
using EaseErp_WareHouse.WareHouse.Service;
using TSLib.Utils;
using TSLibStruts;
using System.Collections;
using Newtonsoft.Json;
using log4net;
using TSLibWeb.Utils;

namespace TSCommon_Web.WareHouse.action
{
    public class PlaceAction : StrutsEntityAction<Place>
    {
        private ILog logger = LogManager.GetLogger(typeof(PlaceAction));
        #region 相关的Service

        private IPlaceService placeService;
        public PlaceAction()
        {
            this.placeService = (IPlaceService)GetObject("PlaceService");
        }

        #endregion

        protected override Place CreateEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext)
        {
            return new Place();
        }

        protected override void Delete(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string[] ids, string type)
        {
            if ("unid".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                this.placeService.Delete(ids);
            }
            else
            {
                this.placeService.Delete(StringUtils.StringArray2LongArray(ids));
            }
        }

        protected override TSLib.PageInfo GetPageInfo(TSLibStruts.ActionContext actionContext, HttpContext httpContext, int pageNo, int pageSize, string sortField, string sortDir)
        {
            string parentUnid = RequestUtils.GetStringParameter(httpContext, "parentUnid", "");
            if (parentUnid.Equals("-1", StringComparison.OrdinalIgnoreCase))
                parentUnid = "";
            return this.placeService.GetPageInfoByParentUnid(pageNo, pageSize, sortField, sortDir, parentUnid);
        }

        protected override Place LoadEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string idValue, string idName)
        {
            EaseErp_WareHouse.WareHouse.Domain.Place place;
            if ("unid".Equals(idName, StringComparison.OrdinalIgnoreCase))
            {
                place = this.placeService.Load(idValue);
            }
            else
            {
                place = this.placeService.Load(Convert.ToInt32(idValue));
            }
            return place;
        }

        protected override void SaveEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, Place entity)
        {
            if (entity.Parent.ID == -1)
                entity.Parent = null;
            this.placeService.Save(entity);
        }

        #region 自定义Action
        /**
         * 取得地点树
         * */
        public Forward GetPlaceNodes(ActionContext actionContext, HttpContext httpContext)
        {
            string placeParentUnid = RequestUtils.GetStringParameter(httpContext, "node", "-1");
            IList list = new ArrayList();
            list = this.placeService.GetPlacesByParentUnid(placeParentUnid);
            httpContext.Response.Write(JavaScriptConvert.SerializeObject(createPlacesJsonArray(list)));
            return null;
        }

        public Forward GetPlaceByAjax(ActionContext actionContext, HttpContext httpContext)
        {
            string[] ids = httpContext.Request.QueryString["ids"].Split(';');
            IList list = new ArrayList();
            list = this.placeService.Load(StringUtils.StringArray2LongArray(ids));
            JavaScriptObject json = new JavaScriptObject();
            json.Add("domains", null);
            if (list != null)
            {
                JavaScriptArray jsonArray;
                jsonArray = new JavaScriptArray();
                jsonArray = this.GetJsonArray<Place>(list);
                json["domains"] = jsonArray;
            }
            string jsonStr = JavaScriptConvert.SerializeObject(json);
            logger.Debug("jsonStr=" + jsonStr);
            httpContext.Response.Write(jsonStr);
            return null;
        }
        #endregion

        private static JavaScriptArray createPlacesJsonArray(IList placeList)
        {
            JavaScriptArray jsonArray = new JavaScriptArray();
            if (null != placeList && placeList.Count > 0)
            {
                foreach (Place place in placeList)
                {
                    JavaScriptObject jsonObj = new JavaScriptObject();
                    jsonObj.Add("id", place.Unid);
                    jsonObj.Add("text", place.Name);
                    jsonObj.Add("_id",place.ID);
                    jsonArray.Add(jsonObj);
                }
            }
            return jsonArray;
        }
    }
}
