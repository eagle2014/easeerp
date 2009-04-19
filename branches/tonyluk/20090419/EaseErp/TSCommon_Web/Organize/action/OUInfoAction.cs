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
using TSCommon_Core.Organize.Service;
using TSLibStruts;
using TSLibWeb.Utils;
using System.Collections;
using TSCommon_Core.Organize.Domain;
using TSCommon_Core.TSWebContext;
using TSLib.Utils;
using Newtonsoft.Json;
using TS.Exceptions;

namespace TSCommon_Web.Organize.action
{
    public class OUInfoAction : StrutsCoreAction
    {
        private static ILog logger = LogManager.GetLogger(typeof(OUInfoAction));

        #region 相关Service变量定义

        private IOUInfoService ouInfoService;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数，初始化相关Service
        /// </summary>
        public OUInfoAction()
        {
            this.ouInfoService = (IOUInfoService)GetObject("OUInfoService");
        }

        #endregion

        #region 自定义的Action方法

        /// <summary>
        /// 通过OU选择岗位
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="httpContext"></param>
        /// <returns>null</returns>
        public Forward GetDepartmentNodes(ActionContext actionContext, HttpContext httpContext)
        {
            AjaxForwardUtils.InitResponse(httpContext.Response);
            string node = RequestUtils.GetStringParameter(httpContext, "node", "");// 指定的ou的unid（点击树节点）
            string type = RequestUtils.GetStringParameter(httpContext, "type", "all");
            string ouType = RequestUtils.GetStringParameter(httpContext, "ouType", null);
            if (logger.IsDebugEnabled)
            {
                logger.Debug("node=" + node);
                logger.Debug("type=" + type);
                logger.Debug("ouType=" + ouType);
            }
            bool isRoot = false;

            // 判断当前用户可选的权限范围
            IList ouInfoLists = new ArrayList();
            User userInfo = TSWEBContext.Current.CurUser;
            if (userInfo.HasPrivilege(Constants.DP_ALL))    // 用户拥有查看所有单位结构的权限
            {
                if (node == "-1" || node == "root" || node == "")
                {
                    isRoot = true;
                    ouInfoLists = this.ouInfoService.FindChilds("", OUInfo.OT_UNIT);// 所有单位信息
                }
                else
                {
                    ouInfoLists = getChildOUInfos(node, type, ouType);
                }
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Constants.DP_ALL");
                    logger.Debug("Count1=" + ouInfoLists.Count.ToString());
                }
            }
            else if (userInfo.HasPrivilege(Constants.DP_LOCALANDCHILD)) // 用户拥有查看本级单位结构的权限
            {
                if (node == "-1" || node == "root" || node == "")
                {
                    isRoot = true;
                    OUInfo ouInfo = this.ouInfoService.Load(userInfo.UnitUnid);// 当前用户所在单位
                    if (null != ouInfo)
                        ouInfoLists.Add(ouInfo);
                }
                else
                {
                    ouInfoLists = getChildOUInfos(node, type, ouType);
                }
                if (logger.IsDebugEnabled)
                    logger.Debug("Constants.DP_LOCALANDCHILD");
            }
            else                                                    // 用户只拥有查看本单位结构的权限
            {
                if (node == "-1" || node == "root" || node == "")
                {
                    isRoot = true;
                    OUInfo ouInfo = this.ouInfoService.Load(userInfo.UnitUnid);// 当前用户所在单位
                    if (null != ouInfo)
                        ouInfoLists.Add(ouInfo);
                }
                else
                {
                    if (!OUInfo.OT_UNIT.Equals(ouType, StringComparison.OrdinalIgnoreCase))
                        ouInfoLists = this.ouInfoService.FindChilds(node, OUInfo.OT_DEPARTMENT);// ou下的所有子部门
                }
                if (logger.IsDebugEnabled)
                    logger.Debug("Constants.DP_LOCAL");
            }

            // 获取是否需要产生链接（不推荐使用）
            string link = RequestUtils.GetStringParameter(httpContext, "link", "N");
            bool isLink = false;
            if (link.Equals(Constants.YESNO_YES, StringComparison.OrdinalIgnoreCase))
                isLink = true;

            bool singleClickExpand = RequestUtils.GetBoolParameter(httpContext, "singleClickExpand", false);
            // 组合返回的信息
            JavaScriptArray jsonArray = createOUInfosJsonArray(ouInfoLists, isLink, singleClickExpand);
            if (logger.IsDebugEnabled)
            {
                logger.Debug("Count2=" + ouInfoLists.Count.ToString());
            }

            // 如果是加载根节点的子节点，则同时预加载第一个子节点的下一级子节点
            if (isRoot && ouInfoLists.Count > 0)
            {
                OUInfo ouInfo = (OUInfo)ouInfoLists[0];
                IList ouInfos2 = getChildOUInfos(ouInfo.Unid, type, ouType);
                JavaScriptArray childJsonArray = createOUInfosJsonArray(ouInfos2, isLink, singleClickExpand);
                ((JavaScriptObject)jsonArray[0]).Add("children", childJsonArray);
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("firstNode.children.Count=" + ouInfos2.Count.ToString());
                }
            }

            string jsonStr = JavaScriptConvert.SerializeObject(jsonArray);
            if (logger.IsDebugEnabled)
            {
                logger.Debug("isRoot=" + isRoot.ToString());
                logger.Debug("json=" + jsonStr);
            }
            httpContext.Response.Write(jsonStr);
            return null;
        }

        private static JavaScriptArray createOUInfosJsonArray(IList ouInfoLists, bool isLink, bool singleClickExpand)
        {
            JavaScriptArray jsonArray = new JavaScriptArray();
            if (ouInfoLists != null)
            {
                JavaScriptObject json;
                for (int i = 0; i < ouInfoLists.Count; i++)
                {
                    OUInfo ouInfo = (OUInfo)ouInfoLists[i];
                    json = new JavaScriptObject();
                    json.Add("text", ouInfo.Name);
                    json.Add("id", ouInfo.Unid);
                    json.Add("dbID", ouInfo.ID);

                    json.Add("cls", "folder");
                    if (OUInfo.OT_UNIT.Equals(ouInfo.Type, StringComparison.OrdinalIgnoreCase))
                        json.Add("iconCls", "egd-treeNode-unit");
                    else if (OUInfo.OT_DEPARTMENT.Equals(ouInfo.Type, StringComparison.OrdinalIgnoreCase))
                        json.Add("iconCls", "egd-treeNode-department");

                    json.Add("FullName", ouInfo.FullName);
                    json.Add("Code", ouInfo.Code);
                    json.Add("FullCode", ouInfo.FullCode);
                    //json.Add("isLeaf", ouInfo.);
                    json.Add("Type", ouInfo.Type);

                    json.Add("UnitUnid", ouInfo.UnitUnid);
                    json.Add("UnitName", ouInfo.UnitName);
                    json.Add("UnitFullName", ouInfo.UnitFullName);
                    json.Add("UnitFullCode", ouInfo.UnitFullCode);

                    json.Add("singleClickExpand", singleClickExpand);
                    if (isLink)
                        json.Add("href", "javascript:onTreeNode_Click('" + ouInfo.Unid + "')");// 不推荐使用
                    jsonArray.Add(json);
                }
            }
            return jsonArray;
        }

        private IList getChildOUInfos(string node, string type, string ouType)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug("getChildOUInfos:node=" + node);
                logger.Debug("getChildOUInfos:type=" + type);
                logger.Debug("getChildOUInfos:ouType=" + ouType);
            }
            IList ouInfoLists;
            if (type.Equals("all"))
            {
                ouInfoLists = this.ouInfoService.FindChilds(node, ouType);// ou下的所有子ou
            }
            else
            {
                OUInfo ouInfo = this.ouInfoService.Load(node);
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("getChildOUInfos:ouInfo.Unid=" + node);
                    logger.Debug("getChildOUInfos:ouInfo.Name=" + ouInfo.Name);
                    logger.Debug("getChildOUInfos:ouInfo.Type=" + ouInfo.Type);
                }
                if (ouInfo.Type.Equals(OUInfo.OT_UNIT))
                {
                    ouInfoLists = this.ouInfoService.FindDepartmentByUnit(node, true);// 单位下的所有部门
                }
                else
                    ouInfoLists = this.ouInfoService.FindChilds(node, OUInfo.OT_DEPARTMENT);// 部门下的所有子部门
            }
            return ouInfoLists;
        }

        /// <summary>
        /// 获取OUInfo的详细信息
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="httpContext"></param>
        /// <returns>null</returns>
        public Forward GetDetail(ActionContext actionContext, HttpContext httpContext)
        {
            AjaxForwardUtils.InitResponse(httpContext.Response);
            JavaScriptObject json = new JavaScriptObject();
            try
            {
                string unid = RequestUtils.GetStringParameter(httpContext, "unid", null);
                logger.Debug("unid:" + unid);
                if (string.IsNullOrEmpty(unid))
                {
                    MsgException e = new MsgException("OUInfo的Unid不能为空，需要在请求参数中包含有效的OUInfo的Unid值！");
                    logger.Error(e.Message, e);
                    return AjaxForwardUtils.ExceptionForward(httpContext, json, e);
                }

                OUInfo ouInfo = this.ouInfoService.Load(unid);
                if (ouInfo == null)
                {
                    MsgException e = new MsgException("指定Unid的OUInfo在系统中不存在:unid=" + unid);
                    logger.Error(e.Message, e);
                    return AjaxForwardUtils.ExceptionForward(httpContext, json, e);
                }

                string jsonStr = JavaScriptConvert.SerializeObject(ouInfo);
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("json=" + jsonStr);
                }
                httpContext.Response.Write(jsonStr);
                return null;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                return AjaxForwardUtils.ExceptionForward(httpContext, json, e);
            }
        }

        #endregion
    }
}
