using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using TS.Exceptions;
using TSCommon_Core.Organize.Domain;
using System.Collections;
using TSLibWeb;
using TSCommon_Core.Security.Domain;
using System.Text.RegularExpressions;
using TSLib.Utils;

namespace TSCommon_Core.TSWebContext
{
    /// <summary>
    /// WEB上下文
    /// </summary>
    public class TSWEBContext
    {
        private HttpContext httpContext = null;
        private NameValueCollection paramsCollection;
        private Uri requestUrl;
        private string requestRawUrl;
        public HttpContext HttpContext
        {
            get
            {
                return httpContext;
            }
        }

        #region Initialize

        public TSWEBContext(HttpContext httpContext)
        {
            this.httpContext = httpContext;
            Initialize(new NameValueCollection(httpContext.Request.QueryString), 
                                               httpContext.Request.Url,
                                               httpContext.Request.RawUrl);
        }

        public TSWEBContext(Uri requestUrl, string requestRawUrl)
        {
            Initialize(new NameValueCollection(), requestUrl, requestRawUrl);
        }

        public TSWEBContext(NameValueCollection paramsCollection, Uri requestUrl, string requestRawUrl)
        {
            Initialize(paramsCollection, requestUrl, requestRawUrl);
        }

        public void Initialize(NameValueCollection paramsCollection, Uri requestUrl, string requestRawUrl)
        {
            this.paramsCollection = paramsCollection;
            this.requestUrl = requestUrl;
            this.requestRawUrl = requestRawUrl;
        }
        
        #endregion

        #region 静态变量,以后直接在程序中使用TSWEBContext.Current就可以引用当前类

        public static TSWEBContext Current
        {
            get
            {
                HttpContext curHttpContext = HttpContext.Current;
                if (curHttpContext == null)
                    throw new ResourceException("HTTP_CONTEXT.EXCEPTION.NOT_EXIST");
                TSWEBContext egrandContext = new TSWEBContext(curHttpContext);
                return egrandContext;
            }
        }

        #endregion

        #region 设置/获取当前用户的信息
        
        /// <summary>
        /// 设置当前用户的信息
        /// </summary>
        /// <param name="curUser"></param>
        public void SetCurUser(User curUser)
        {
            if (null != this.httpContext.Session)
            {
                this.httpContext.Session[TSLibWeb.Constants.Session_User] = curUser;
            }
            else
            {
                throw new ResourceException("HTTP_CONTEXT.EXCEPTION.NOT_EXIST");
            }
        }

        /// <summary>
        /// 设置当前用户所拥有的岗位信息
        /// </summary>
        /// <param name="groupList">该用户所拥有的岗位信息列表</param>
        public void SetCurUserGroupInfo(IList groupList)
        {
            if (null != this.httpContext.Session)
            {
                this.httpContext.Session[TSLibWeb.Constants.Session_Groups] = groupList;
            }
            else
            {
                throw new ResourceException("HTTP_CONTEXT.EXCEPTION.NOT_EXIST");
            }
        }

        /// <summary>
        /// 获取当前用户所拥有的岗位信息
        /// </summary>
        public IList GetCurUserGroupInfo()
        {
            if (null != this.httpContext.Session)
            {
                return (IList)this.httpContext.Session[TSLibWeb.Constants.Session_Groups];
            }
            return new ArrayList();
        }

        /// <summary>
        /// 设置当前用户所拥有的角色信息
        /// </summary>
        /// <param name="roleList">该用户所拥有的角色信息列表</param>
        public void SetCurUserRoleInfo(IList roleList)
        {
            if (null != this.httpContext.Session)
            {
                this.httpContext.Session[TSLibWeb.Constants.Session_Role] = roleList;

                // 获取相应的权限集合
                IList list = new ArrayList();
                Hashtable privilegeTable = new Hashtable();
                foreach (Role role in roleList)
                {
                    IList privilegeLists = role.Privileges;
                    if (null == privilegeLists || privilegeLists.Count == 0)
                        continue;
                    foreach (Privilege privilege in privilegeLists)
                    {
                        if (privilegeTable.ContainsKey(privilege.Code))
                            continue;
                        privilegeTable.Add(privilege.Code, privilege);
                    }
                }
                this.httpContext.Session[TSLibWeb.Constants.Session_Privilege] = privilegeTable;

                // 设置当前用户所拥有的权限列表
                User curUser = CurUser;
                if (curUser == null) throw new Exception("Session中还没有配置当前用户信息！");
                curUser.Privileges = privilegeTable;
                this.httpContext.Session.Remove(TSLibWeb.Constants.Session_User);
                this.httpContext.Session[TSLibWeb.Constants.Session_User] = curUser;
            }
            else
            {
                throw new ResourceException("HTTP_CONTEXT.EXCEPTION.NOT_EXIST");
            }
        }

        /// <summary>
        /// 获取当前用户所拥有的角色信息
        /// </summary>
        public IList GetCurUserRoleInfo()
        {
            if (null != this.httpContext.Session)
            {
                return (IList)this.httpContext.Session[TSLibWeb.Constants.Session_Role];
            }
            return new ArrayList();
        }

        /// <summary>
        /// 设置当前用户所拥有的权限信息
        /// </summary>
        /// <param name="privilegeTable">该用户所拥有的权限信息列表</param>
        public void SetCurUserPrivilegeInfo(Hashtable privilegeTable)
        {
            if (null != this.httpContext.Session)
            {
                this.httpContext.Session[TSLibWeb.Constants.Session_Privilege] = privilegeTable;
            }
            else
            {
                throw new ResourceException("HTTP_CONTEXT.EXCEPTION.NOT_EXIST");
            }
        }

        /// <summary>
        /// 获取当前用户所拥有的权限信息
        /// </summary>
        public Hashtable GetCurUserPrivilegeInfo()
        {
            if (null != this.httpContext.Session)
            {
                return (Hashtable)this.httpContext.Session[TSLibWeb.Constants.Session_Privilege];
            }
            return new Hashtable();
        }

        /// <summary>
        /// 获取当前用户所拥有的权限信息
        /// </summary>
        public IList GetCurUserPrivileges()
        {
            if (null != this.httpContext.Session)
            {
                Hashtable privileges = (Hashtable)this.httpContext.Session[TSLibWeb.Constants.Session_Privilege];
                if (null == privileges || privileges.Count == 0)
                    return new ArrayList();
                else
                {
                    ArrayList privilegeLists = new ArrayList();
                    privilegeLists.AddRange(privileges.Values);
                    return privilegeLists;
                }
            }
            return new ArrayList();
        }

        /// <summary>
        /// 判断当前用户是否具有某个指定的权限
        /// </summary>
        /// <param name="privilegeCode">权限的编码</param>
        /// <returns>如果具有则返回True，否则返回False</returns>
        public bool IsHasPrivilege(string privilegeCode)
        {
            Hashtable privilegeTable = this.GetCurUserPrivilegeInfo();
            if (null == privilegeTable || privilegeTable.Count == 0)
                return false;
            return privilegeTable.ContainsKey(privilegeCode);
        }

        /// <summary>
        /// 判断当前用户是否具有某个指定的角色
        /// </summary>
        /// <param name="roleCode">角色的编码</param>
        /// <returns>如果具有则返回True，否则返回False</returns>
        public bool IsHasRole(string roleCode)
        {
            IList roles = this.GetCurUserRoleInfo();
            if (null == roles || roles.Count == 0)
                return false;
            foreach (Role role in roles)
            {
                if (role.Code.Equals(roleCode, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 判断当前用户是否具有某个指定的岗位
        /// </summary>
        /// <param name="groupCode">岗位的编码</param>
        /// <returns>如果具有则返回True，否则返回False</returns>
        public bool IsHasGroup(string groupCode)
        {
            IList groups = this.GetCurUserGroupInfo();
            if (null == groups || groups.Count == 0)
                return false;
            foreach (TSCommon_Core.Organize.Domain.Group group in groups)
            {
                if (group.Code.Equals(groupCode, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断当前用户是否具有某个指定unid的岗位
        /// </summary>
        /// <param name="groupUnid">岗位的unid</param>
        /// <returns>如果具有则返回True，否则返回False</returns>
        public bool IsHasGroupByUnid(string groupUnid)
        {
            IList groups = this.GetCurUserGroupInfo();
            if (null == groups || groups.Count == 0)
                return false;
            foreach (TSCommon_Core.Organize.Domain.Group group in groups)
            {
                if (group.Unid.Equals(groupUnid, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获得当前用户的信息
        /// </summary>
        public User CurUser
        {
            get
            {
                if (null != this.httpContext.Session)
                {
                    if (null != this.httpContext.Session[TSLibWeb.Constants.Session_User])
                        return (User)this.httpContext.Session[TSLibWeb.Constants.Session_User];
                }
                return null;
            }
        }

        /// <summary>
        /// 获取当前用户的Unid
        /// </summary>
        public string CurUserUnid
        {
            get
            {
                if (null != this.CurUser)
                {
                    return this.CurUser.Unid;
                }
                return "";
            }
        }

        /// <summary>
        /// 获取当前用户所属单位的Unid
        /// </summary>
        public string CurUserUnitUnid
        {
            get
            {
                if (null != this.CurUser)
                {
                    return this.CurUser.UnitUnid;
                }
                return "";
            }
        }

        /// <summary>
        /// 获取当前用户所属OU的Unid
        /// </summary>
        public string CurUserOUUnid
        {
            get
            {
                if (null != this.CurUser)
                {
                    return this.CurUser.OUUnid;
                }
                return "";
            }
        }

        /// <summary>
        /// 判断指定的用户是否进行重复登录
        /// </summary>
        /// <param name="userUnid">所要判断的用户的Unid</param>
        /// <returns>如果已经登录，那么返回True，否则返回False</returns>
        public bool CheckReLogin(string userUnid)
        {
            if (string.IsNullOrEmpty(userUnid))
                return false;

            if (!this.UserSessionIDS.ContainsKey(userUnid))
                return false;

            if (string.IsNullOrEmpty(this.UserSessionIDS[userUnid]))
                return false;

            if (this.UserSessionIDS[userUnid].Equals(this.httpContext.Session.SessionID, StringComparison.OrdinalIgnoreCase))
                return false;
            return true;
        }

        #endregion

        #region 当前用户所访问的页面记录的处理

        /// <summary>
        /// 当前访问Url
        /// </summary>
        public Uri Url
        {
            get { return this.requestUrl; }
        }

        /// <summary>
        /// 原始请求URL
        /// </summary>
        public string RawUrl
        {
            get { return this.requestRawUrl; }
        }

        /// <summary>
        /// 用栈来记录当前用户访问的页面
        /// </summary>
        public Stack<string> ViewPagesStack
        {
            get
            {
                if (null != this.httpContext.Session)
                {
                    if (null == this.httpContext.Session["ViewPages"])
                        this.httpContext.Session["ViewPages"] = new Stack<string>();

                    return (Stack<string>)this.httpContext.Session["ViewPages"];
                }
                return null;
            }
        }

        #endregion

        #region Applicaion中活动人员记录信息

        /// <summary>
        /// 存储当前Application记录当前登陆的所有用户
        /// </summary>
        public Dictionary<string, string> UserSessionIDS
        {
            get
            {
                if (null != this.httpContext.Application)
                {
                    if (null == this.httpContext.Application["UserSession"])
                        this.httpContext.Application["UserSession"] = new Dictionary<string, string>();

                    return (Dictionary<string, string>)this.httpContext.Application["UserSession"];
                }
                return null;
            }
            set
            {
                this.httpContext.Application["UserSession"] = value;
            }
        }

        /// <summary>
        /// 保存当前用户SessionId到Application
        /// </summary>
        public void SaveUserSessionToApplication()
        {
            if (string.IsNullOrEmpty(this.CurUserUnid))
                return;

            Dictionary<string, string> sessionInfos = this.UserSessionIDS;
            if (sessionInfos.ContainsKey(this.CurUserUnid))
                sessionInfos.Remove(this.CurUserUnid);

            sessionInfos[this.CurUserUnid] = this.httpContext.Session.SessionID;
            this.UserSessionIDS = sessionInfos;
        }

        /// <summary>
        /// 删除Application中的指定用户
        /// </summary>
        public void RemoveSessionFromApplication()
        {
            if (string.IsNullOrEmpty(this.CurUserUnid))
                return;

            Dictionary<string, string> sessionInfos = this.UserSessionIDS;
            if (sessionInfos.ContainsKey(this.CurUserUnid))
                sessionInfos.Remove(this.CurUserUnid);
            this.UserSessionIDS = sessionInfos;
        }

        #endregion

        #region 保存聊天在线人员信息
        /// <summary>
        /// 存储当前Application记录当前登陆聊天的所有用户
        /// </summary>
        public Dictionary<string, string> MessageOnLine
        {
            get
            {
                if (null != this.httpContext.Application)
                {
                    if (null == this.httpContext.Application["MessageOnLine"])
                        this.httpContext.Application["MessageOnLine"] = new Dictionary<string, string>();

                    return (Dictionary<string, string>)this.httpContext.Application["MessageOnLine"];
                }
                return null;
            }
            set
            {
                this.httpContext.Application["MessageOnLine"] = value;
            }
        }

        /// <summary>
        /// 保存当前用户到聊天Application
        /// </summary>
        public void SaveMessageOnLineToApplication(string unid)
        {
            if (string.IsNullOrEmpty(unid))
                return;

            Dictionary<string, string> messageOnLine = this.MessageOnLine;
            if (messageOnLine.ContainsKey(this.CurUserUnid))
                messageOnLine.Remove(this.CurUserUnid);

            MessageOnLine[this.CurUserUnid] = unid;
            this.MessageOnLine = messageOnLine;
        }

        /// <summary>
        /// 删除聊天Application中的指定用户
        /// </summary>
        public void RemoveMessageOnLineFromApplication()
        {
            if (string.IsNullOrEmpty(this.CurUserUnid))
                return;

            Dictionary<string, string> messageOnLine = this.MessageOnLine;
            if (messageOnLine.ContainsKey(this.CurUserUnid))
                messageOnLine.Remove(this.CurUserUnid);
            this.MessageOnLine = messageOnLine;
        }
        /// <summary>
        /// 根据组织结构unid获得在线人员个数
        /// </summary>
        /// <param name="unid">组织结构unid</param>
        /// <returns>在线人员个数</returns>
        public int GetMessageOnLineCountByOUUnid(string unid)
        {
            int count=0;
            Dictionary<string, string> messageOnLine = this.MessageOnLine;
            
            Dictionary<string, string>.ValueCollection values = messageOnLine.Values;
            Dictionary<string, string>.ValueCollection.Enumerator enumerator = values.GetEnumerator(); 
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Equals(unid))
                    count++;
            }
            return count;
        }

        /// <summary>
        /// 判断指定用户是否进入聊天
        /// </summary>
        /// <param name="userUnid">用户unid</param>
        /// <returns>是否在线（true|false）</returns>
        public bool CheckHaveUser(string userUnid)
        {
            Dictionary<string, string> messageOnLine = this.MessageOnLine;
            if (messageOnLine.ContainsKey(userUnid))
                return true;
            return false;
        }

        /// <summary>
        /// 设置个人聊天形式(0:部门形式  1:自定义形式)
        /// </summary>
        /// <param name="style"></param>
        public void SetCurUserMessageStyle(string style)
        {
            if (null != this.httpContext.Session)
            {
                this.httpContext.Session["MessageStyle"] = style;
            }
        }

        /// <summary>
        /// 改变个人聊天形式(0:部门形式  1:自定义形式)
        /// </summary>
        public void ChangeCurUserMessageStyle()
        {
            if (null != this.httpContext.Session)
            {
                string style = this.MessageStyle();
                if (style == null)
                    this.httpContext.Session["MessageStyle"] = "1";
                else
                {
                    if (style.Equals("0"))
                    {
                        this.httpContext.Session["MessageStyle"] = "1";
                    }
                    else
                    {
                        this.httpContext.Session["MessageStyle"] = "0";
                    }
                }
            }
        }
        /// <summary>
        /// 获得个人聊天形式
        /// </summary>
        /// <returns></returns>
        public string MessageStyle()
        {
            if (null != this.httpContext.Session)
            {
                return (string)this.httpContext.Session["MessageStyle"];
            }
            return null;
        }
        #endregion

        #region 对请求中的参数进行统一的处理

        /// <summary>
        /// 读取当前的请求的URL中String类型的参数值
        /// </summary>
        /// <param name="paramName">参数的名称</param>
        /// <param name="defaultValue">缺省的值</param>
        /// <returns></returns>
        public string GetStringValue(string paramName, string defaultValue)
        {
            string returnValue = this.paramsCollection[paramName];
            if(string.IsNullOrEmpty(returnValue))
                return defaultValue;
            else
                return returnValue;
        }

        public string[] GetStringArrayParameter(string parameterName, string splitSymbol, string[] defaultValue)
        {
            if (string.IsNullOrEmpty(parameterName))
                return defaultValue;
            string returnValue = this.paramsCollection[parameterName];
            if (string.IsNullOrEmpty(returnValue))
                return defaultValue;
            string[] returnValues = Regex.Split(returnValue, splitSymbol);
                return returnValues;
        }

        /// <summary>
        /// 获得当前请求的URL中bool类型参数值
        /// </summary>
        /// <param name="paramName">参数的名称</param>
        /// <param name="defaultValue">缺省的值</param>
        /// <returns></returns>
        public bool GetBoolValue(string paramName, bool defaultValue)
        {
            return TSUtils.GetSafeBool(this.paramsCollection[paramName], defaultValue);
        }

        /// <summary>
        /// 获得当前请求的URL中int类型参数值
        /// </summary>
        /// <param name="paramName">参数的名称</param>
        /// <param name="defaultValue">缺省的值</param>
        /// <returns></returns>
        public int GetIntValue(string paramName, int defaultValue)
        {
            return TSUtils.GetSafeInt(this.paramsCollection[paramName], defaultValue);
        }

        /// <summary>
        /// 获得当前请求的URL中int类型参数值
        /// </summary>
        /// <param name="paramName">参数的名称</param>
        /// <param name="defaultValue">缺省的值</param>
        /// <returns></returns>
        public long GetLongValue(string paramName, long defaultValue)
        {
            return TSUtils.GetSafeLong(this.paramsCollection[paramName], defaultValue);
        }

        /// <summary>
        /// 获得当前请求的URL中float类型参数值
        /// </summary>
        /// <param name="paramName">参数的名称</param>
        /// <param name="defaultValue">缺省的值</param>
        /// <returns></returns>
        public float GetFloatValue(string paramName, float defaultValue)
        {
            return TSUtils.GetSafeFloat(this.paramsCollection[paramName], defaultValue);
        }

        /// <summary>
        /// 获得当前请求的URL中double类型参数值
        /// </summary>
        /// <param name="paramName">参数的名称</param>
        /// <param name="defaultValue">缺省的值</param>
        /// <returns></returns>
        public double GetDoubleValue(string paramName, double defaultValue)
        {
            return TSUtils.GetSafeDouble(this.paramsCollection[paramName], defaultValue);
        }

        #endregion

        /// <summary>
        /// 获取指定的请求参数名列表组成的url参数-值字符串
        /// </summary>
        /// <param name="Request">请求</param>
        /// <param name="paramNames">请求参数名列表</param>
        /// <returns>指定的参数列表组成的url参数-值字符串</returns>
        public static string GetParamsUrl(HttpRequest Request, string[] paramNames)
        {
            if (paramNames == null || paramNames.Length == 0) return "";

            string urlSuffix = "";
            string paramValue;
            bool isFirst = true;
            foreach (string paramName in paramNames)
            {
                paramValue = Request.Params[paramName];
                if (!string.IsNullOrEmpty(paramValue))
                {
                    urlSuffix += (isFirst ? "" : "&") + paramName + "=" + paramValue;
                    isFirst = false;
                }
            }
            return urlSuffix;
        }
    }
}
