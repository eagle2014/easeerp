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
    /// Ȩ��action
    /// </summary>
    public class PrivilegeAction : StrutsEntityAction<Privilege>
    {
        private ILog logger = LogManager.GetLogger(typeof(PrivilegeAction));

        #region ���Service
        private IModelService modelService;
        private IPrivilegeService privilegeService;
        public PrivilegeAction()
        {
            this.privilegeService = (IPrivilegeService)GetObject("PrivilegeService"); 
            this.modelService = (IModelService)GetObject("ModelService");
        }
        #endregion
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="actionContext">action������</param>
        /// <param name="httpContext">httpContext������</param>
        /// <returns>����</returns>
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
        /// ɾ������
        /// </summary>
        /// <param name="actionContext">action������</param>
        /// <param name="httpContext">httpContext������</param>
        /// <param name="ids">id����</param>
        /// <param name="type">ɾ������</param>
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
        /// ȡ��ͼ����
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
        /// ��ȡ����
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
        /// �������
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="httpContext"></param>
        /// <param name="entity"></param>
        protected override void SaveEntity(TSLibStruts.ActionContext actionContext, HttpContext httpContext, Privilege entity)
        {
            this.privilegeService.Save(entity);
        }
        #region ����Action
        /// <summary>
        /// ��ȡȨ�޽ڵ�
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
                logger.Error("modelsList��privilegesListΪ�գ�");
                httpContext.Response.Write(JavaScriptConvert.SerializeObject(jsonArray));
                return null;
            }

            // �Ȼ�ȡ��ǰ�û���ӵ�е�ȫ��Ȩ���б�
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
                logger.Error("userPrivilegesListΪ�գ�");
                httpContext.Response.Write(JavaScriptConvert.SerializeObject(jsonArray));
                return null;
            }

            // ���ȴ���ģ��Ȩ��
            Model model, tempModel;
            Privilege privilege;
            for (int i = 0; i < modelsList.Count; i++)
            {
                model = (Model)modelsList[i];
                if (null == model)
                    continue;

                // �ж��û���ӵ�е�Ȩ���Ƿ��ڱ�ģ����
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
                    // TODO: ���ĸ���ʽ
                    json.Add("cls", "folder");
                    jsonArray.Add(json);
                }
            }

            // �ٴ�����Ȩ��
            string rootPath = TSLibWeb.WEB.Page.GetContextPath(httpContext.Request) + "/";
            for (int i = 0; i < privilegesList.Count; i++)
            {
                privilege = (Privilege)privilegesList[i];
                if (null == privilege)
                    continue;
                if (!Privilege.PRIVILEGETYPE_MODEL.Equals(privilege.Type, StringComparison.OrdinalIgnoreCase))
                    continue;

                // �ж��û���ӵ�е�Ȩ���Ƿ��ڱ�ģ����
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
                    // TODO: ���ĸ���ʽ
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

                    // �ô���ᵼ��IE6�д򿪵�ҳ��Ϊ��
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
