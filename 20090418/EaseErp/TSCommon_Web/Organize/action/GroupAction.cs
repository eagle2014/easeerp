using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using TSCommon_Core.Organize.Domain;
using TSLibWeb.Struts;
using TSCommon_Core.Organize.Service;
using TSCommon_Core.Security.Service;
using log4net;
using TSLibStruts;
using TSLib.Utils;
using TSLibWeb.Utils;
using System.Collections;
using TSLib;
using TSCommon_Core.SystemPolicy;
using System.Text;
using TSCommon_Core.TSWebContext;
using Newtonsoft.Json;
using TSLibWeb.Json;
using TSCommon_Core.Security.Domain;

namespace TSCommon_Web.Organize.action
{
    public class GroupAction : StrutsEntityAction<Group>
    {
        private static ILog logger = LogManager.GetLogger(typeof(GroupAction));

        #region 相关Service变量定义

        private IOUInfoService ouInfoService;       // 组织架构配置的Service
        private IUserService userService;   // 人员配置的Service
        private IGroupService groupService;         // 岗位配置的Service
        private IRoleService roleService;           // 岗位配置的Service

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数，初始化相关Service
        /// </summary>
        public GroupAction()
        {
            this.groupService = (IGroupService)GetObject("GroupService");
            this.ouInfoService = (IOUInfoService)GetObject("OUInfoService");
            this.userService = (IUserService)GetObject("UserService");
            this.roleService = (IRoleService)GetObject("RoleService");
        }

        #endregion

        #region StrutsEntityAction的实现

        protected override Group CreateEntity(ActionContext actionContext, HttpContext httpContext)
        {
            Group group = new Group();
            group.ID = Constants.BLANK_LONG_VALUE;
            group.GroupStatus = GroupStatuses.Enable;
            group.IsCanDispatch = Constants.YESNO_NO;
            group.IsInner = Constants.YESNO_NO;
            return group;
        }

        protected override Group LoadEntity(ActionContext actionContext, HttpContext httpContext,string idValue, string idName)
        {
            Group group;
            if ("unid".Equals(idName, StringComparison.OrdinalIgnoreCase))
            {
                group = this.groupService.Load(idValue);
            }
            else
            {
                group = this.groupService.Load(Convert.ToInt32(idValue));
            }
            return group;
        }

        protected override void SaveEntity(ActionContext actionContext, HttpContext httpContext,Group entity)
        {
            string selectedRoleUnids = RequestUtils.GetStringParameter(httpContext, "RoleUnids", null);
            string selectedUserInfoUnids = RequestUtils.GetStringParameter(httpContext, "UserInfoUnids", null);
            if (logger.IsDebugEnabled)
            {
                logger.Debug("selectedRoleUnids=" + selectedRoleUnids);
                logger.Debug("selectedUserInfoUnids=" + selectedUserInfoUnids);
            }

            if (!string.IsNullOrEmpty(selectedRoleUnids))
            {
                IList unids = new ArrayList();
                string[] roles = System.Text.RegularExpressions.Regex.Split(selectedRoleUnids, ",");
                foreach (string unid in roles)
                    unids.Add(unid);
                entity.RoleUnidLists = unids;
            }
            else
            {
                entity.RoleUnidLists = new ArrayList();
            }

            if (!string.IsNullOrEmpty(selectedUserInfoUnids))
            {
                IList unids = new ArrayList();
                string[] user = System.Text.RegularExpressions.Regex.Split(selectedUserInfoUnids, ",");
                foreach (string unid in user)
                    unids.Add(unid);
                entity.UserInfoUnidLists = unids;
            }
            else
                entity.UserInfoUnidLists = new ArrayList();

            this.groupService.Save(entity);
        }

        protected override void Delete(ActionContext actionContext, HttpContext httpContext,string[] ids, string type)
        {
            if ("unid".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                // 删除指定unid集的
                this.groupService.Delete(ids);
            }
            else
            {
                // 删除指定id集的
                this.groupService.Delete(StringUtils.StringArray2LongArray(ids));
            }
        }

        protected override PageInfo GetPageInfo(ActionContext actionContext, HttpContext httpContext,int pageNo, int pageSize, string sortField, string sortDir)
        {
            string type = RequestUtils.GetStringParameter(httpContext, "type", "all");
            if (logger.IsDebugEnabled)
            {
                logger.Debug("type=" + type);
            }

            if ("bySearch".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                return null;// this.GetPageInfoBySearch(httpContext, curUserInfo, pageNo, pageSize, sortField, sortDir);
            }
            else if ("all".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                string ouUnid = httpContext.Request.Params["ouUnid"];
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("ouUnid=" + ouUnid);
                }
                if (!string.IsNullOrEmpty(ouUnid) && ouUnid != "root" && ouUnid != "-1")
                {
                    return this.groupService.GetPageByOU(ouUnid, pageNo, pageSize, sortField, sortDir);
                }
                else
                {
                    return this.groupService.GetPageByOU(string.Empty, pageNo, pageSize, sortField, sortDir);
                }
            }
            else
            {
                return new PageInfo();
            }
        }

        #endregion

        #region 自定义的Action方法

        /// <summary>
        /// 通过OU选择岗位页面
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="httpContext"></param>
        /// <returns>通过OU选择岗位页面的Forward</returns>
        public Forward SelectGroupByOU(ActionContext actionContext, HttpContext httpContext)
        {
            // 获取参数
            string ouUnid = RequestUtils.GetStringParameter(httpContext, "ouUnid", TSWEBContext.Current.CurUserUnitUnid);
            string type = RequestUtils.GetStringParameter(httpContext, "type", "0");
            bool singleSelect = RequestUtils.GetBoolParameter(httpContext, "singleSelect", true);

            // 重设参数
            RequestUtils.ResetParameter(httpContext, "ouUnid");
            RequestUtils.ResetParameter(httpContext, "type");
            RequestUtils.ResetParameter(httpContext, "singleSelect");

            return actionContext.Forwards["selectGroupByOU"];
        }

        /// <summary>
        /// 通过OU选择岗位
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="httpContext"></param>
        /// <returns>null</returns>
        public Forward FindGroupByOU(ActionContext actionContext, HttpContext httpContext)
        {
            AjaxForwardUtils.InitResponse(httpContext.Response);
            JavaScriptArray jsonArray = new JavaScriptArray();
            string ouUnid = RequestUtils.GetStringParameter(httpContext, "ouUnid", null);
            string groupType = RequestUtils.GetStringParameter(httpContext, "groupType", "0");// 岗位类型：0--全部类型(默认)，1--可派单岗位，2--不可派单岗位
            if (string.IsNullOrEmpty(ouUnid))
                throw new Exception("OUUnid不能为空！需要在请求参数中包含有效的OUUnid值。");

            // 获取该OU中的岗位信息
            IList groups = this.groupService.FindByOU(ouUnid, false, groupType);

            if (logger.IsDebugEnabled)
            {
                logger.Debug("ouUnid=" + ouUnid);
                logger.Debug("groupType=" + groupType);
                logger.Debug("count=" + groups.Count);
            }

            // 组合信息
            if (groups != null && groups.Count > 0)
            {
                foreach (Group group in groups)
                {
                    jsonArray.Add(JsonUtils.CreateJsonObject(group));
                }
            }

            httpContext.Response.Write(JavaScriptConvert.SerializeObject(jsonArray));
            return null;
        }

        /// <summary>
        /// 通过OU选择岗位
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="httpContext"></param>
        /// <returns>null</returns>
        public Forward UpdateOnOUChange(ActionContext actionContext, HttpContext httpContext)
        {
            httpContext.Response.ContentType = ("text/xml;charset=UTF-8");
            httpContext.Response.Charset = "UTF-8";

            string ouUnid = httpContext.Request.Params["ouUnid"];
            if (string.IsNullOrEmpty(ouUnid))
            {
                httpContext.Response.Write("");
                return null;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            sb.Append("<response>");

            // 更新角色信息
            IList roleList = roleService.FindByOU(ouUnid);
            foreach (Role role in roleList)
            {
                sb.Append("<roleEntry>");
                sb.Append("<name>" + role.Name + "</name>");
                sb.Append("<value>" + role.Unid + "</value>");
                sb.Append("</roleEntry>");
            }

            // 更新OU信息
            string policyValue = PolicyHelper.GetPolicyValue("9001");
            if (string.IsNullOrEmpty(policyValue))
                policyValue = Constants.YESNO_YES;
            IList ouList;
            if (policyValue.Equals(Constants.YESNO_YES, StringComparison.OrdinalIgnoreCase))
                ouList = this.ouInfoService.FindOUInfoByParentUnid(ouUnid, true, true);
            else
                ouList = this.ouInfoService.FindOUInfoByParentUnid(ouUnid, true, false);
            foreach (OUInfo ouInfo in ouList)
            {
                sb.Append("<ouInfoEntry>");
                sb.Append("<name>" + ouInfo.FullName + "</name>");
                sb.Append("<value>" + ouInfo.Unid + "</value>");
                sb.Append("</ouInfoEntry>");
            }

            // 更新人员信息
            IList userInfoList = this.userService.FindByOU(ouUnid);
            foreach (User userInfo in userInfoList)
            {
                sb.Append("<userInfoEntry>");
                sb.Append("<name>" + userInfo.Name + "</name>");
                sb.Append("<value>" + userInfo.Unid + "</value>");
                sb.Append("</userInfoEntry>");
            }
            sb.Append("</response>");
            httpContext.Response.Write(sb.ToString());
            return null;
        }

        /// <summary>
        /// 通过OU选择岗位
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="httpContext"></param>
        /// <returns>null</returns>
        public Forward FindGroupsWithOUUnid(ActionContext actionContext, HttpContext httpContext)
        {
            string ouUnid = httpContext.Request.Params["ouUnid"];
            if (string.IsNullOrEmpty(ouUnid))
            {
                httpContext.Response.Write("");
                return null;
            }

            IList list = this.groupService.FindByOU(ouUnid, true);
            if (null == list || list.Count == 0)
            {
                httpContext.Response.Write("");
                return null;
            }

            StringBuilder sb = new StringBuilder();
            httpContext.Response.ContentType = ("text/xml;charset=UTF-8");
            httpContext.Response.Charset = "UTF-8";
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            sb.Append("<response>");
            foreach (Group group in list)
            {
                sb.Append("<entry>");
                sb.Append("<name>" + group.Name + "</name>");
                sb.Append("<value>" + group.Unid + "</value>");
                sb.Append("</entry>");
            }
            sb.Append("</response>");
            if (logger.IsDebugEnabled)
            {
                logger.Debug("岗位数量：" + list.Count.ToString());
                logger.Debug("Xml: " + sb.ToString());
            }
            httpContext.Response.Write(sb.ToString());
            return null;
        }

        #endregion
    }
}
