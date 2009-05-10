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
using log4net;
using TSLib;
using TSLib.Utils;
using TSCommon.Core.Organize.Service;
using TSCommon.Core.Organize.Domain;
using TSLibWeb.Utils;
using System.Collections;
using TSLibStruts;
using Newtonsoft.Json;
using TSCommon.Core.Organize.RelationShips;
using Lib;

namespace TSCommon.Web.Organize.Action
{
    public class UserAction : StrutsEntityAction<User>
    {
        private ILog logger = LogManager.GetLogger(typeof(UserAction));

        #region 相关的Service
        private IUserService userService;
        private IJobTitleService jobTitleService;
        private IRelationShipService relationShipService;

        public UserAction()
        {
            userService = (IUserService)GetObject("UserService");
            jobTitleService = (IJobTitleService)GetObject("JobTitleService");
            relationShipService = (IRelationShipService)GetObject("RelationShipService");
        }
        #endregion

        protected override User CreateEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext)
        {
            User user=new User ();
            DateTime now=DateTime.Now;
            user.ID=Constants.BLANK_INT_VALUE;
            user.UserStatus=UserStatuses.Disable;
            user.ValidityEndDate=now;
            user.ValidityStartDate=now;
            // 设置默认的职务
            // 绑定职务列表
            IList allJobTitle = this.jobTitleService.FindAll();
            string defaultJobtitlecode=SimpleResourceHelper.GetString("USER.DEFAULT_JOBTITLE_CODE");
            if (!string.IsNullOrEmpty(defaultJobtitlecode))
            {
                JobTitle defaultJobTitle = this.jobTitleService.LoadByCode(defaultJobtitlecode);
                if (defaultJobTitle != null)
                {
                    foreach (JobTitle item in allJobTitle)
                    {
                        if (item.Unid == defaultJobTitle.Unid)
                        {
                            user.JobTitleName = defaultJobTitle.Name;
                            user.JobTitleUnid = defaultJobTitle.Unid;
                            break;
                        }
                    }
                }
            }
            return user;
        }

        protected override User LoadEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string idValue, string idName)
        {
            User user;
            if ("unid".Equals(idName, StringComparison.OrdinalIgnoreCase))
            {
                user = this.userService.Load(idValue);
            }
            else
            {
                user = this.userService.Load(Convert.ToInt32(idValue));
            }
            return user;
        }

        protected override void SaveEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, User entity)
        {
            string selectedGroupUnids = RequestUtils.GetStringParameter(httpContext, "GroupUnids", null);
            if (logger.IsDebugEnabled)
            {
                logger.Debug("selectedGroupUnids=" + selectedGroupUnids);
            }
            if (!string.IsNullOrEmpty(selectedGroupUnids))
            {
                IList unids = new ArrayList();
                string[] groups = System.Text.RegularExpressions.Regex.Split(selectedGroupUnids, ",");
                foreach (string unid in groups)
                    unids.Add(unid);
                entity.GroupUnidLists = unids;
            }
            else
            {
                entity.GroupUnidLists = new ArrayList();
            }

            this.userService.Save(entity);
        }

        protected override void Delete(TSLibStruts.ActionContext actionContext, HttpContext httpContext, string[] ids, string type)
        {
            if ("unid".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                // 删除指定unid集的
                this.userService.Delete(ids);
            }
            else
            {
                // 删除指定id集的
                this.userService.Delete(StringUtils.StringArray2LongArray(ids));
            }
        }

        protected override PageInfo GetPageInfo(TSLibStruts.ActionContext actionContext, HttpContext httpContext, int pageNo, int pageSize, string sortField, string sortDir)
        {
            if ("UserTypeDesc".Equals(sortField))
                sortField = "UserType";
            string type = RequestUtils.GetStringParameter(httpContext, "type", "all");
            if (logger.IsDebugEnabled)
            {
                logger.Debug("type=" + type);
            }

            if ("bySearch".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetPageInfoBySearch(httpContext, pageNo, pageSize, sortField, sortDir);
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
                    return this.userService.GetPageByOU(ouUnid, pageNo, pageSize, sortField, sortDir);
                }
                else
                {
                    return this.userService.GetPageByOU(string.Empty, pageNo, pageSize, sortField, sortDir);
                }
            }
            else
            {
                return new PageInfo();
            }
        }

        /// <summary>
        /// 激活人员配置信息
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Forward EnabledUser(ActionContext actionContext, HttpContext httpContext)
        {
            long userID = long.Parse(httpContext.Request.Params["id"]);
            if (-1 != userID)
            {
                this.userService.EnabledUser(null, userService.Load(userID));
                httpContext.Response.Write("ok");
            }
            if (logger.IsDebugEnabled)
            {
                logger.Debug("EnabledUser:");
            }
            return null;
        }

        /// <summary>
        /// 禁止人员配置信息
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Forward DisabledUser(ActionContext actionContext, HttpContext httpContext)
        {
            long userID = long.Parse(httpContext.Request.Params["id"]);
            if (-1 != userID)
            {
                this.userService.DisabledUser(null, userService.Load(userID));
                httpContext.Response.Write("ok");
            }
            if (logger.IsDebugEnabled)
            {
                logger.Debug("EnabledUser:");
            }
            return null;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Forward UpdateUser(ActionContext actionContext, HttpContext httpContext)
        {
            AjaxForwardUtils.InitResponse(httpContext.Response);
            JavaScriptObject jsonObj = new JavaScriptObject();
            try
            {
                long userID = long.Parse(httpContext.Request.Params["id"]);
                if (-1 != userID)
                {
                    string password = httpContext.Request.Params["p"];
                    string newEncryptPassword = this.userService.ChangeUserPasswordOne(null, userID, password);
                    //  Domain = userInfoService.Load(userInfoID);
                    //httpContext.Response.Write("ok");
                    jsonObj.Add("newEncryptPassword", newEncryptPassword);
                    return AjaxForwardUtils.SuccessForward(httpContext, jsonObj, "USERINFO.CHANGE_PASSWORD.WEB.SUCCESS", null);
                }
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("update:" + userID);
                }
                return null;
            }
            catch (Exception e)
            {
                return AjaxForwardUtils.ExceptionForward(httpContext, jsonObj, e);
            }
        }

        /// <summary>
        /// 为查询获取分页信息，通过Url="userAction.do?action=View&type=bySearch"调用
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="curUserInfo"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDir"></param>
        /// <returns></returns>
        private PageInfo GetPageInfoBySearch(HttpContext httpContext, int pageNo, int pageSize, string sortField, string sortDir)
        {
            PageInfo pageInfo;

            // 获取过滤条件
            bool canEmpty = RequestUtils.GetBoolParameter(httpContext, "canEmpty", true);
            string[] filterNames = RequestUtils.GetStringArrayParameter(httpContext,
                "filterNames", Constants.ITEM_SEPARATOR, new string[0]);
            string[] filterValues = RequestUtils.GetStringArrayParameter(httpContext,
                "filterValues", Constants.ITEM_SEPARATOR, null);
            string[] filterTypes = RequestUtils.GetStringArrayParameter(httpContext,
                "filterTypes", Constants.ITEM_SEPARATOR, null);
            if (logger.IsDebugEnabled)
            {
                logger.Debug("In GetPageInfoBySearch:");
                logger.Debug("  canEmpty=" + RequestUtils.GetStringParameter(httpContext, "canEmpty", "null"));
                logger.Debug("  filterNames="
                    + RequestUtils.GetStringParameter(httpContext, "filterNames", "null"));
                logger.Debug("  filterValues="
                    + RequestUtils.GetStringParameter(httpContext, "filterValues", "null"));
                logger.Debug("  filterTypes="
                    + RequestUtils.GetStringParameter(httpContext, "filterTypes", "null"));
                logger.Debug("  pageNo=" + pageNo);
                logger.Debug("  pageSize=" + pageSize);
                logger.Debug("  sortField=" + sortField);
                logger.Debug("  sortDir=" + sortDir);
            }

            // 创建过滤条件
            IList filters = new ArrayList();
            for (int i = 0; i < filterNames.Length; i++)
            {
                filters.Add(new FilterParameter(filterNames[i], filterValues[i], filterTypes[i]));
            }
            if (canEmpty)
            {
                pageInfo = this.userService.GetPage(pageNo, pageSize, sortField, sortDir, filters);
            }
            else
            {
                pageInfo = new PageInfo();
            }

            return pageInfo;
        }

        /// <summary>
        /// 关联
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="httpContext"></param>
        public void AssociateAction(ActionContext actionContext, HttpContext httpContext)
        {
            string otherUnid = httpContext.Request.Params["otherUnid"];
            string userUnids = httpContext.Request.Params["userUnids"];
            string otherType = httpContext.Request.Params["otherType"];
            string[] unids = userUnids.Split(',');

            if (!(string.IsNullOrEmpty(otherUnid) || string.IsNullOrEmpty(otherType) || unids == null || unids.Length == 0))
            {
                foreach (string userUnid in unids)
                {
                    RelationShip rs = this.relationShipService.Get(otherUnid, userUnid);
                    if (rs == null)
                    {
                        rs = new RelationShip();
                        rs.ParentType = otherType;
                        rs.ParentUnid = otherUnid;
                        rs.ChildType = User.RELATIONSHIP_CODE;
                        rs.ChildUnid = userUnid;
                        rs.RelationShipType = rs.ParentType + "." + rs.ChildType;
                        this.relationShipService.Save(rs);
                    }
                }
            }
        }

        /// <summary>
        /// 解除关联
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="httpContext"></param>
        public void DisassociateAction(ActionContext actionContext, HttpContext httpContext)
        {
            string otherUnid = httpContext.Request.Params["otherUnid"];
            string userIds = httpContext.Request.Params["userIds"];

            if (!(string.IsNullOrEmpty(otherUnid) || string.IsNullOrEmpty(userIds)))
            {
                string[] ids = userIds.Split(';');

                foreach (string id in ids)
                {
                    string userUnid = this.userService.Load(Convert.ToInt64(id)).Unid;

                    this.relationShipService.Delete(otherUnid, userUnid);
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public void GetGridData(ActionContext actionContext, HttpContext httpContext)
        {
            string otherUnid = httpContext.Request.Params["otherUnid"];
            int pageNo = int.Parse(httpContext.Request.Params["pageNo"].Trim());
            int pageSize = int.Parse(httpContext.Request.Params["pageSize"].Trim());
            Newtonsoft.Json.JavaScriptObject jsonObject = new Newtonsoft.Json.JavaScriptObject();
            jsonObject.Add("rows", new Newtonsoft.Json.JavaScriptArray());
            jsonObject.Add("totalCount", 0);
            jsonObject.Add("id", "ID");
            PageInfo pageInfo = this.userService.FindAllByRelationShipParentUnid(otherUnid, pageNo, pageSize);
            if (!string.IsNullOrEmpty(otherUnid))
            {
                if (pageInfo != null)
                {
                    IList list = pageInfo.Objs;
                    if (!(list == null || list.Count == 0))
                    {
                        Newtonsoft.Json.JavaScriptArray jsonArray = this.GetArray<User>(list);
                        jsonObject["rows"] = jsonArray;
                        jsonObject["totalCount"] = pageInfo.TotalCount;
                    }
                }
            }
            httpContext.Response.ContentType = ("text/plain;charset=UTF-8");
            httpContext.Response.AppendHeader("Cache-Control", "no-cache");
            httpContext.Response.Write(Newtonsoft.Json.JavaScriptConvert.SerializeObject(jsonObject));
        }

        /// <summary>
        /// IList 转换成 JavascriptArray
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        private Newtonsoft.Json.JavaScriptArray GetArray<T>(IList list) where T : Entry
        {
            Newtonsoft.Json.JavaScriptArray jsonArray = new Newtonsoft.Json.JavaScriptArray();

            if (!(null == list || list.Count == 0))
            {
                foreach (object obj in list)
                {
                    if (obj != null)
                    {
                        T t = obj as T;
                        jsonArray.Add(t);
                    }
                }
            }

            return jsonArray;
        }
    }
}
