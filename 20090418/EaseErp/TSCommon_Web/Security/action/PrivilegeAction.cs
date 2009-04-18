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
using TSLibWeb;
using TSCommon_Core.Security.Service;
using TSLib.Utils;
using TSLib;
using TSLibStruts;
using TSLibWeb.Utils;
using log4net;
using Newtonsoft.Json;
using System.Collections;
using TSCommon_Core.Organize.Domain;

namespace TSCommon_Web.Security.Action
{
    /// <summary>
    /// 权限action
    /// </summary>
    public class PrivilegeAction : StrutsEntityAction<Privilege>
    {
        private ILog logger = LogManager.GetLogger(typeof(PrivilegeAction));

        #region 相关Service
        private IModelService modelService;
        private IPrivilegeService privilegeService;
        public PrivilegeAction()
        {
            this.privilegeService = (IPrivilegeService)GetObject("PrivilegeService"); 
            this.modelService = (IModelService)GetObject("ModelService");
        }
        #endregion
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="actionContext">action上下文</param>
        /// <param name="httpContext">httpContext上下文</param>
        /// <returns>对象</returns>
        protected override Privilege CreateEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext)
        {
            Privilege entity = new Privilege();
            entity.ID = TSLibWeb.Constants.BLANK_LONG_VALUE;
            entity.IsInner = TSLibWeb.Constants.YESNO_NO;
            entity.ModelID = TSLibWeb.Constants.BLANK_LONG_VALUE;
            entity.Type = Privilege.PRIVILEGETYPE_MODEL;
            return entity;
        }
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="actionContext">action上下文</param>
        /// <param name="httpContext">httpContext上下文</param>
        /// <param name="ids">id集合</param>
        /// <param name="type">删除类型</param>
        protected override void Delete(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string[] ids, string type)
        {
            if ("unid".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                privilegeService.Delete(ids);
            }
            else
            {
                privilegeService.Delete(StringUtils.StringArray2LongArray(ids));
            }
        }
        /// <summary>
        /// 取视图数据
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="httpContext"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDir"></param>
        /// <returns></returns>
        protected override TSLib.PageInfo GetPageInfo(TSLibStruts.ActionContext actionContext, HttpContext httpContext, int pageNo, int pageSize, string sortField, string sortDir)
        {
            PageInfo pageInfo = new PageInfo();
            pageInfo = privilegeService.GetPage(pageNo, pageSize, sortField, sortDir);
            return pageInfo;
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="httpContext"></param>
        /// <param name="idValue"></param>
        /// <param name="idName"></param>
        /// <returns></returns>
        protected override Privilege LoadEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string idValue, string idName)
        {
            if ("unid".Equals(idName, StringComparison.OrdinalIgnoreCase))
            {
                return privilegeService.Load(idValue);
            }
            else
            {
                return privilegeService.Load(Convert.ToInt32(idValue));
            }
        }
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="httpContext"></param>
        /// <param name="entity"></param>
        protected override void SaveEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, Privilege entity)
        {
            this.privilegeService.Save(entity);
        }
        #region 其它Action
        /// <summary>
        /// 获取权限节点
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Forward GetPrivilegeNodes(ActionContext actionContext, HttpContext httpContext)
        {
            logger.Debug("begin load Privilege's nodes");
            AjaxForwardUtils.InitResponse(httpContext.Response);
            JavaScriptArray jsonArray = new JavaScriptArray();
            JavaScriptObject json;
            string node = RequestUtils.GetStringParameter(httpContext, "node", "root");
            if (logger.IsDebugEnabled)
            {
                logger.Debug("node=" + node);
            }
            if ("root".Equals(node, StringComparison.OrdinalIgnoreCase))
                node = null;

            ArrayList modelsList = new ArrayList();
            ArrayList privilegesList = new ArrayList();
            modelsList.AddRange(this.modelService.FindChildren(node));
            privilegesList.AddRange(this.privilegeService.FindByModelAndType(node, Privilege.PRIVILEGETYPE_MODEL));
            if (modelsList.Count == 0 && privilegesList.Count == 0)
            {
                logger.Error("modelsList或privilegesList为空！");
                httpContext.Response.Write(JavaScriptConvert.SerializeObject(jsonArray));
                return null;
            }

            // 先获取当前用户所拥有的全部权限列表
            SessionHelper sessionHelper = new SessionHelper();
            User userInfo = sessionHelper.GetSession(TSLibWeb.Constants.Session_User) as User;
            if (null == userInfo)
            {
                return null;
            }
            ArrayList userPrivilegesList = new ArrayList();
            userPrivilegesList.AddRange(userInfo.Privileges.Values);
            if (null == userPrivilegesList || userPrivilegesList.Count == 0)
            {
                logger.Error("userPrivilegesList为空！");
                httpContext.Response.Write(JavaScriptConvert.SerializeObject(jsonArray));
                return null;
            }

            // 首先处理模块权限
            Model model, tempModel;
            Privilege privilege;
            for (int i = 0; i < modelsList.Count; i++)
            {
                model = (Model)modelsList[i];
                if (null == model)
                    continue;

                // 判断用户所拥有的权限是否在本模块中
                bool isAdd = false;
                for (int j = 0; j < userPrivilegesList.Count; j++)
                {
                    privilege = (Privilege)userPrivilegesList[j];
                    if (null == privilege || !Privilege.PRIVILEGETYPE_MODEL.Equals(privilege.Type, StringComparison.OrdinalIgnoreCase))
                        continue;
                    tempModel = privilege.Model;
                    while (null != tempModel)
                    {
                        if (tempModel.ID == model.ID)
                        {
                            isAdd = true;
                            break;
                        }

                        tempModel = tempModel.Parent;
                    }
                    if (isAdd)
                        break;
                }

                if (isAdd)
                {
                    json = new JavaScriptObject();
                    json.Add("text", model.Name);
                    json.Add("id", model.Unid);
                    //json.Add("unid", model.Unid);
                    json.Add("isModel", true);
                    json.Add("singleClickExpand", true);
                    // TODO: 更改该样式
                    json.Add("cls", "folder");
                    jsonArray.Add(json);
                }
            }

            // 再处理功能权限
            string rootPath = TSLibWeb.WEB.Page.GetContextPath(httpContext.Request) + "/";
            for (int i = 0; i < privilegesList.Count; i++)
            {
                privilege = (Privilege)privilegesList[i];
                if (null == privilege)
                    continue;
                if (!Privilege.PRIVILEGETYPE_MODEL.Equals(privilege.Type, StringComparison.OrdinalIgnoreCase))
                    continue;

                // 判断用户所拥有的权限是否在本模块中
                bool isAdd = false;
                for (int j = 0; j < userPrivilegesList.Count; j++)
                {
                    Privilege privilegeTmp = (Privilege)userPrivilegesList[j];
                    if (null == privilegeTmp)
                        continue;
                    if (privilegeTmp.ID == privilege.ID)
                    {
                        isAdd = true;
                        break;
                    }
                }
                if (isAdd)
                {
                    json = new JavaScriptObject();
                    json.Add("text", privilege.Name);
                    json.Add("id", privilege.Unid);
                    //json.Add("unid", privilege.Unid);
                    json.Add("isPrivilege", true);
                    json.Add("leaf", true);
                    // TODO: 更改该样式
                    json.Add("cls", "cls");
                    json.Add("iconCls", "egd-icon-privilege");
                    string urlPath = privilege.UrlPath;
                    if (urlPath != null)
                    {
                        urlPath = urlPath.Replace("../", rootPath);
                        urlPath = urlPath.Replace("{rootPath}", rootPath);
                        json.Add("urlPath", urlPath);
                        json.Add("href", urlPath);
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug("href old=" + privilege.UrlPath);
                            logger.Debug("href new=" + urlPath);
                        }
                    }

                    // 该代码会导致IE6中打开的页面为空
                    //json.Add("href", "javascript:openUrl('" + privilege.UrlPath + "," + privilege.Name + "')");
                    jsonArray.Add(json);
                }
            }

            if (logger.IsDebugEnabled)
            {
                logger.Debug("json=" + JavaScriptConvert.SerializeObject(jsonArray));
            }

            httpContext.Response.Write(JavaScriptConvert.SerializeObject(jsonArray));
            return null;
        }
        #endregion
    }
}
