using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using TSCommon.Core.OptionItemCfg.Domain;
using TSLibWeb.Struts;
using log4net;
using TSCommon.Core.OptionItemCfg.Service;
using TSLibStruts;
using TSCommon.Core.Organize.Domain;
using TSCommon.Core.TSWebContext;
using TSLib.Utils;
using TSLib;
using TSLibWeb.Utils;
using Newtonsoft.Json;
using TSLibWeb.Json;
using System.Collections;

namespace TSCommon.Web.OptionItemCfg.action
{
    public class OptionItemAction : StrutsEntityAction<OptionItem>
    {
        private static ILog logger = LogManager.GetLogger(typeof(OptionItemAction));

        #region 相关Service变量定义

        /// <summary>模块配置的Service</summary>
        private IOptionItemService optionItemService;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数，初始化相关Service
        /// </summary>
        public OptionItemAction()
        {
            this.optionItemService = (IOptionItemService)GetObject("OptionItemService");
        }

        #endregion

        #region StrutsEntityAction的实现

        protected override OptionItem CreateEntity(ActionContext actionContext, HttpContext httpContext)
        {
            OptionItem optionItem = new OptionItem();
            User curUser = TSWEBContext.Current.CurUser;
            optionItem.Init(curUser);
            return optionItem;
        }

        protected override OptionItem LoadEntity(ActionContext actionContext, HttpContext httpContext,string idValue, string idName)
        {
            OptionItem optionItem;
            if ("unid".Equals(idName, StringComparison.OrdinalIgnoreCase))
            {
                optionItem = this.optionItemService.Load(idValue);
            }
            else
            {
                optionItem = this.optionItemService.Load(Convert.ToInt32(idValue));
            }
            return optionItem;
        }

        protected override void SaveEntity(ActionContext actionContext, HttpContext httpContext,OptionItem entity)
        {
            User curUser = TSWEBContext.Current.CurUser;
            entity.SetLastModifiedInfo(curUser);
            this.optionItemService.Save(entity);
        }

        protected override void Delete(ActionContext actionContext, HttpContext httpContext, string[] ids, string type)
        {
            if ("unid".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                // 删除指定unid集的
                this.optionItemService.Delete(ids);
            }
            else
            {
                // 删除指定id集的
                this.optionItemService.Delete(StringUtils.StringArray2LongArray(ids));
            }
        }

        protected override PageInfo GetPageInfo(ActionContext actionContext, HttpContext httpContext,int pageNo, int pageSize, string sortField, string sortDir)
        {
            string type = RequestUtils.GetStringParameter(httpContext, "type", null);
            if (logger.IsDebugEnabled)
                logger.Debug("type=" + type);
            return this.optionItemService.GetPage(pageNo, pageSize, sortField, sortDir, type);
        }

        protected override JavaScriptObject CreatePageInfoItemJson(object pageInfoItem)
        {
            return JsonUtils.CreateJsonObject(pageInfoItem);
        }

        /// <summary>
        /// 获取类别列表的ajax Action
        /// </summary>
        /// <param name="actionContext">Action上下文</param>
        /// <param name="httpContext">Http上下文</param>
        /// <returns>null</returns>
        public void GetTypes(ActionContext actionContext, HttpContext httpContext)
        {
            logger.Debug("GetTypes...");
            string type = httpContext.Request.Params["type"];
            logger.Debug("GetTypes:type=" + type);
            IList list = this.optionItemService.FindAll(type);
            if (logger.IsDebugEnabled)
                logger.Debug("GetTypes:type=" + type);
            WriteOptionsJsonData(httpContext, list);

        }

        /// <summary>
        /// 获取类别列表的ajax Action
        /// </summary>
        /// <param name="actionContext">Action上下文</param>
        /// <param name="httpContext">Http上下文</param>
        /// <returns>null</returns>
        public void GetSysName(ActionContext actionContext, HttpContext httpContext)
        {
            string type = httpContext.Request.Params["type"];
            logger.Debug("GetSysName:type=" + type);
            IList list = this.optionItemService.FindAll(type);
            if (logger.IsDebugEnabled)
                logger.Debug("GetSysName:type=" + type);
            WriteOptionsJsonData(httpContext, list);

        }

        /// <summary>
        /// 生成列表对应的json字符串并写入到请求的响应中
        /// </summary>
        /// <param name="httpContext">Http上下文</param>
        /// <param name="lists">CTI列表</param>
        private static void WriteOptionsJsonData(HttpContext httpContext, IList lists)
        {
            JavaScriptArray jsonOptions = new JavaScriptArray();

            if (lists != null)
            {
                foreach (OptionItem item in lists)
                {
                    jsonOptions.Add(JsonUtils.CreateJsonObject(item));
                }
            }
            string jsonString = JavaScriptConvert.SerializeObject(jsonOptions);
            if (logger.IsDebugEnabled)
                logger.Debug("json=" + jsonString);

            AjaxForwardUtils.InitResponse(httpContext.Response);
            httpContext.Response.Write(jsonString);
        }

        #endregion
    }
}
