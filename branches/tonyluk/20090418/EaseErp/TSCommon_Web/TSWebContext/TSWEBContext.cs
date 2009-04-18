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
    /// WEB������
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

        #region ��̬����,�Ժ�ֱ���ڳ�����ʹ��TSWEBContext.Current�Ϳ������õ�ǰ��

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

        #region ����/��ȡ��ǰ�û�����Ϣ
        
        /// <summary>
        /// ���õ�ǰ�û�����Ϣ
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
        /// ���õ�ǰ�û���ӵ�еĸ�λ��Ϣ
        /// </summary>
        /// <param name="groupList">���û���ӵ�еĸ�λ��Ϣ�б�</param>
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
        /// ��ȡ��ǰ�û���ӵ�еĸ�λ��Ϣ
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
        /// ���õ�ǰ�û���ӵ�еĽ�ɫ��Ϣ
        /// </summary>
        /// <param name="roleList">���û���ӵ�еĽ�ɫ��Ϣ�б�</param>
        public void SetCurUserRoleInfo(IList roleList)
        {
            if (null != this.httpContext.Session)
            {
                this.httpContext.Session[TSLibWeb.Constants.Session_Role] = roleList;

                // ��ȡ��Ӧ��Ȩ�޼���
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

                // ���õ�ǰ�û���ӵ�е�Ȩ���б�
                User curUser = CurUser;
                if (curUser == null) throw new Exception("Session�л�û�����õ�ǰ�û���Ϣ��");
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
        /// ��ȡ��ǰ�û���ӵ�еĽ�ɫ��Ϣ
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
        /// ���õ�ǰ�û���ӵ�е�Ȩ����Ϣ
        /// </summary>
        /// <param name="privilegeTable">���û���ӵ�е�Ȩ����Ϣ�б�</param>
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
        /// ��ȡ��ǰ�û���ӵ�е�Ȩ����Ϣ
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
        /// ��ȡ��ǰ�û���ӵ�е�Ȩ����Ϣ
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
        /// �жϵ�ǰ�û��Ƿ����ĳ��ָ����Ȩ��
        /// </summary>
        /// <param name="privilegeCode">Ȩ�޵ı���</param>
        /// <returns>��������򷵻�True�����򷵻�False</returns>
        public bool IsHasPrivilege(string privilegeCode)
        {
            Hashtable privilegeTable = this.GetCurUserPrivilegeInfo();
            if (null == privilegeTable || privilegeTable.Count == 0)
                return false;
            return privilegeTable.ContainsKey(privilegeCode);
        }

        /// <summary>
        /// �жϵ�ǰ�û��Ƿ����ĳ��ָ���Ľ�ɫ
        /// </summary>
        /// <param name="roleCode">��ɫ�ı���</param>
        /// <returns>��������򷵻�True�����򷵻�False</returns>
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
        /// �жϵ�ǰ�û��Ƿ����ĳ��ָ���ĸ�λ
        /// </summary>
        /// <param name="groupCode">��λ�ı���</param>
        /// <returns>��������򷵻�True�����򷵻�False</returns>
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
        /// �жϵ�ǰ�û��Ƿ����ĳ��ָ��unid�ĸ�λ
        /// </summary>
        /// <param name="groupUnid">��λ��unid</param>
        /// <returns>��������򷵻�True�����򷵻�False</returns>
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
        /// ��õ�ǰ�û�����Ϣ
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
        /// ��ȡ��ǰ�û���Unid
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
        /// ��ȡ��ǰ�û�������λ��Unid
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
        /// ��ȡ��ǰ�û�����OU��Unid
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
        /// �ж�ָ�����û��Ƿ�����ظ���¼
        /// </summary>
        /// <param name="userUnid">��Ҫ�жϵ��û���Unid</param>
        /// <returns>����Ѿ���¼����ô����True�����򷵻�False</returns>
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

        #region ��ǰ�û������ʵ�ҳ���¼�Ĵ���

        /// <summary>
        /// ��ǰ����Url
        /// </summary>
        public Uri Url
        {
            get { return this.requestUrl; }
        }

        /// <summary>
        /// ԭʼ����URL
        /// </summary>
        public string RawUrl
        {
            get { return this.requestRawUrl; }
        }

        /// <summary>
        /// ��ջ����¼��ǰ�û����ʵ�ҳ��
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

        #region Applicaion�л��Ա��¼��Ϣ

        /// <summary>
        /// �洢��ǰApplication��¼��ǰ��½�������û�
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
        /// ���浱ǰ�û�SessionId��Application
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
        /// ɾ��Application�е�ָ���û�
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

        #region ��������������Ա��Ϣ
        /// <summary>
        /// �洢��ǰApplication��¼��ǰ��½����������û�
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
        /// ���浱ǰ�û�������Application
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
        /// ɾ������Application�е�ָ���û�
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
        /// ������֯�ṹunid���������Ա����
        /// </summary>
        /// <param name="unid">��֯�ṹunid</param>
        /// <returns>������Ա����</returns>
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
        /// �ж�ָ���û��Ƿ��������
        /// </summary>
        /// <param name="userUnid">�û�unid</param>
        /// <returns>�Ƿ����ߣ�true|false��</returns>
        public bool CheckHaveUser(string userUnid)
        {
            Dictionary<string, string> messageOnLine = this.MessageOnLine;
            if (messageOnLine.ContainsKey(userUnid))
                return true;
            return false;
        }

        /// <summary>
        /// ���ø���������ʽ(0:������ʽ  1:�Զ�����ʽ)
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
        /// �ı����������ʽ(0:������ʽ  1:�Զ�����ʽ)
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
        /// ��ø���������ʽ
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

        #region �������еĲ�������ͳһ�Ĵ���

        /// <summary>
        /// ��ȡ��ǰ�������URL��String���͵Ĳ���ֵ
        /// </summary>
        /// <param name="paramName">����������</param>
        /// <param name="defaultValue">ȱʡ��ֵ</param>
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
        /// ��õ�ǰ�����URL��bool���Ͳ���ֵ
        /// </summary>
        /// <param name="paramName">����������</param>
        /// <param name="defaultValue">ȱʡ��ֵ</param>
        /// <returns></returns>
        public bool GetBoolValue(string paramName, bool defaultValue)
        {
            return TSUtils.GetSafeBool(this.paramsCollection[paramName], defaultValue);
        }

        /// <summary>
        /// ��õ�ǰ�����URL��int���Ͳ���ֵ
        /// </summary>
        /// <param name="paramName">����������</param>
        /// <param name="defaultValue">ȱʡ��ֵ</param>
        /// <returns></returns>
        public int GetIntValue(string paramName, int defaultValue)
        {
            return TSUtils.GetSafeInt(this.paramsCollection[paramName], defaultValue);
        }

        /// <summary>
        /// ��õ�ǰ�����URL��int���Ͳ���ֵ
        /// </summary>
        /// <param name="paramName">����������</param>
        /// <param name="defaultValue">ȱʡ��ֵ</param>
        /// <returns></returns>
        public long GetLongValue(string paramName, long defaultValue)
        {
            return TSUtils.GetSafeLong(this.paramsCollection[paramName], defaultValue);
        }

        /// <summary>
        /// ��õ�ǰ�����URL��float���Ͳ���ֵ
        /// </summary>
        /// <param name="paramName">����������</param>
        /// <param name="defaultValue">ȱʡ��ֵ</param>
        /// <returns></returns>
        public float GetFloatValue(string paramName, float defaultValue)
        {
            return TSUtils.GetSafeFloat(this.paramsCollection[paramName], defaultValue);
        }

        /// <summary>
        /// ��õ�ǰ�����URL��double���Ͳ���ֵ
        /// </summary>
        /// <param name="paramName">����������</param>
        /// <param name="defaultValue">ȱʡ��ֵ</param>
        /// <returns></returns>
        public double GetDoubleValue(string paramName, double defaultValue)
        {
            return TSUtils.GetSafeDouble(this.paramsCollection[paramName], defaultValue);
        }

        #endregion

        /// <summary>
        /// ��ȡָ��������������б���ɵ�url����-ֵ�ַ���
        /// </summary>
        /// <param name="Request">����</param>
        /// <param name="paramNames">����������б�</param>
        /// <returns>ָ���Ĳ����б���ɵ�url����-ֵ�ַ���</returns>
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
