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
using System.Text.RegularExpressions;
using TSLib.Utils;
using Newtonsoft.Json;
using TSLibStruts;
using System.Collections;
using System.Text;

namespace TSCommon_Web.Security.action
{
    public class RoleAction : StrutsEntityAction<Role>
    {

        private static ILog logger = LogManager.GetLogger(typeof(RoleAction));

        #region 相关Service变量定义

        /// <summary>角色配置的Service</summary>
        private IRoleService roleService;
        /// <summary>权限配置的Service</summary>
        private IPrivilegeService privilegeService;

        #endregion

        /// <summary>
        /// 构造函数，初始化相关Service
        /// </summary>
        public RoleAction()
        {
            this.roleService = (IRoleService)GetObject("RoleService");
            this.privilegeService = (IPrivilegeService)GetObject("PrivilegeService");
        }

        protected override Role CreateEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext)
        {
            Role role = new Role();
            role.ID = TSLibWeb.Constants.BLANK_LONG_VALUE;
            role.IsInner = TSLibWeb.Constants.YESNO_NO;
            role.Level = TSLibWeb.Constants.BLANK_STRING_VALUE;
            role.LevelName = TSLibWeb.Constants.BLANK_STRING_VALUE;
            return role;
        }

        protected override void Delete(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string[] ids, string type)
        {
            if ("unid".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                // 删除指定unid集的
                this.roleService.Delete(ids);
            }
            else
            {
                // 删除指定id集的
                this.roleService.Delete(StringUtils.StringArray2LongArray(ids));
            }
        }

        protected override TSLib.PageInfo GetPageInfo(TSLibStruts.ActionContext actionContext, HttpContext httpContext, int pageNo, int pageSize, string sortField, string sortDir)
        {
            return this.roleService.GetPage(pageNo, pageSize, sortField, sortDir);
        }

        protected override Role LoadEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string idValue, string idName)
        {
            Role role;
            if ("unid".Equals(idName, StringComparison.OrdinalIgnoreCase))
            {
                role = this.roleService.Load(idValue);
            }
            else
            {
                role = this.roleService.Load(Convert.ToInt32(idValue));
            }
            return role;
        }

        protected override void SaveEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, Role entity)
        {
            string selectedPrivilegeIDs = httpContext.Request.Form.Get("PrivilegeIDs");
            if (!string.IsNullOrEmpty(selectedPrivilegeIDs))
            {
                entity.SetPrivilegeIDs(Regex.Split(selectedPrivilegeIDs, ","));
            }
            else
                entity.Privileges = new ArrayList();

            this.roleService.Save(entity);
        }

        protected override JavaScriptObject CreatePageInfoItemJson(object pageInfoItem)
        {
            Role role = pageInfoItem as Role;
            JavaScriptObject json = new JavaScriptObject();
            json.Add("ID", role.ID);
            json.Add("Unid", role.Unid);
            json.Add("Name", role.Name);
            json.Add("Code", role.Code);
            json.Add("Level", role.Level);
            json.Add("IsInner", role.IsInner);
            json.Add("RoleStatus", role.RoleStatus);
            return json;
        }

        #region 自定义Action

        public Forward FindPrivilege(ActionContext actionContext, HttpContext httpContext)
        {  
            if (logger.IsDebugEnabled)
            {
                logger.Debug("模块信息改变时......");
            }
            httpContext.Response.ContentType = ("text/xml;charset=UTF-8");
            httpContext.Response.Charset = "UTF-8";
         
            string modelID = httpContext.Request.Params["modelID"];
            if (string.IsNullOrEmpty(modelID))
            {
                httpContext.Response.Write("");
                return null;
            }
            if (logger.IsDebugEnabled)
            {
                logger.Debug("模块信息改变时......modelID: "+modelID);
            }

            IList list = this.privilegeService.FindByModel(Int64.Parse(modelID));
            if (logger.IsDebugEnabled)
            {
                logger.Debug("模块信息改变时，权限列表数量: "+list.Count.ToString());
            }
            if (null == list || list.Count == 0)
            {
                httpContext.Response.Write("");
                return null;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            sb.Append("<response>");
            foreach (Privilege privilege in list)
            {
                sb.Append("<entry>");
                sb.Append("<name>" + privilege.Name + "</name>");
                sb.Append("<value>" + privilege.ID.ToString() + "</value>");
                sb.Append("</entry>");
            }
            sb.Append("</response>");
            httpContext.Response.Write(sb.ToString());
            return null;
        }

        public Forward FindRolesWithOUUnid(ActionContext actionContext, HttpContext httpContext)
        {
            httpContext.Response.ContentType = ("text/xml;charset=UTF-8");
            httpContext.Response.Charset = "UTF-8";

            string ouUnid = httpContext.Request.Params["ouUnid"];
            if (string.IsNullOrEmpty(ouUnid))
            {
                httpContext.Response.Write("");
                return null;
            }

            IList list = this.roleService.FindByOU(ouUnid);
            if (null == list || list.Count == 0)
            {
                httpContext.Response.Write("");
                return null;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            sb.Append("<response>");
            foreach (Role role in list)
            {
                sb.Append("<entry>");
                sb.Append("<name>" + role.Name + "</name>");
                sb.Append("<value>" + role.Unid + "</value>");
                sb.Append("</entry>");
            }
            sb.Append("</response>");
            httpContext.Response.Write(sb.ToString());
            return null;
        }

        #endregion
    }
}
