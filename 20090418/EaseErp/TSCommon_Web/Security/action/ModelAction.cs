using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using TSCommon_Core.Security.Domain;
using TSLibWeb.Struts;
using log4net;
using TSCommon_Core.Security.Service;
using TSLibWeb;
using TSLib.Utils;
using Newtonsoft.Json;

namespace TSCommon_Web.Security.action
{
    public class ModelAction : StrutsEntityAction<Model>
    {
        private static ILog logger = LogManager.GetLogger(typeof(ModelAction));

        #region 相关Service变量定义

        /// <summary>模块配置的Service</summary>
        private IModelService modelService;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数，初始化相关Service
        /// </summary>
        public ModelAction()
        {
            this.modelService = (IModelService)GetObject("ModelService");
        }

        #endregion

        protected override Model CreateEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext)
        {
            Model model = new Model();
            model.ID = TSLibWeb.Constants.BLANK_LONG_VALUE;
            model.IsInner = TSLibWeb.Constants.YESNO_NO;
            model.ParentID = TSLibWeb.Constants.BLANK_LONG_VALUE;
            model.Type = ModelTypes.SubModel;
            return model;
        }

        protected override void Delete(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string[] ids, string type)
        {
            if ("unid".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                // 删除指定unid集的
                this.modelService.Delete(ids);
            }
            else
            {
                // 删除指定id集的
                this.modelService.Delete(StringUtils.StringArray2LongArray(ids));
            }
        }

        protected override TSLib.PageInfo GetPageInfo(TSLibStruts.ActionContext actionContext, HttpContext httpContext, int pageNo, int pageSize, string sortField, string sortDir)
        {
            return this.modelService.GetPage(pageNo, pageSize, sortField, sortDir);
        }

        protected override Model LoadEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string idValue, string idName)
        {
            Model model;
            if ("unid".Equals(idName, StringComparison.OrdinalIgnoreCase))
            {
                model = this.modelService.Load(idValue);
            }
            else
            {
                model = this.modelService.Load(Convert.ToInt32(idValue));
            }
            return model;
        }

        protected override void SaveEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, Model entity)
        {
            this.modelService.Save(entity);
        }

        protected override JavaScriptObject CreatePageInfoItemJson(object pageInfoItem)
        {
            Model model = pageInfoItem as Model;
            JavaScriptObject json = new JavaScriptObject();
            json.Add("ID", model.ID);
            json.Add("Unid", model.Unid);
            json.Add("Name", model.Name);
            json.Add("Code", model.Code);
            json.Add("Type", model.Type);
            json.Add("OrderNo", model.OrderNo);
            json.Add("IsInner", model.IsInner);
            json.Add("ParentName", model.Parent != null ? model.Parent.Name : "");
            return json;
        }
    }
}
