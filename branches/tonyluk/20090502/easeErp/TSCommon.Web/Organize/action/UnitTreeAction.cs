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
using TSCommon.Core.Organize.Service;
using TSLibStruts;
using System.Collections;
using TSCommon.Core.TSWebContext;
using TSLib.Utils;
using TSLibWeb.Utils;
using TSCommon.Core.Organize.Domain;

namespace TSCommon.Web.Organize.action
{
    public class UnitTreeAction : StrutsCoreAction
    {
        private static ILog logger = LogManager.GetLogger(typeof(UnitTreeAction));

        public UnitTreeAction()
        {
            this.ouInfoService = GetObject("OUInfoService") as IOUInfoService;
        }

        // 组织架构配置的Service
        private IOUInfoService ouInfoService;
        public IOUInfoService OUInfoService
        {
            set { ouInfoService = value; }
        }

        public virtual Forward GetUnitTree(ActionContext actionContext, HttpContext httpContext)
        {
            string node = httpContext.Request.Params["node"];
            IList unitTree = null;
            User curUser = TSWEBContext.Current.CurUser;
            if (TSWEBContext.Current.IsHasPrivilege(Constants.ADMIN_ALL))
                unitTree = this.ouInfoService.GetUnitTree(curUser, Constants.OT_ALL);
            else if (TSWEBContext.Current.IsHasPrivilege(Constants.ADMIN_LOCALANDCHILD))
                unitTree = this.ouInfoService.GetUnitTree(curUser, Constants.OT_LOCALANDCHILD);
            else if (TSWEBContext.Current.IsHasPrivilege(Constants.ADMIN_LOCAL))
                unitTree = this.ouInfoService.GetUnitTree(curUser, Constants.OT_LOCAL);

            Newtonsoft.Json.JavaScriptArray jsonArray = new Newtonsoft.Json.JavaScriptArray();
            if (!(string.IsNullOrEmpty(node) || unitTree == null))
            {
                foreach (object obj in unitTree)
                {
                    string[] nodeInfo = obj as string[];
                    if (node.Equals(nodeInfo[1], StringComparison.OrdinalIgnoreCase))
                    {
                        Newtonsoft.Json.JavaScriptObject jsonObject = new Newtonsoft.Json.JavaScriptObject();
                        jsonObject.Add("id", nodeInfo[0]);
                        jsonObject.Add("text", nodeInfo[2]);
                        jsonArray.Add(jsonObject);
                    }
                }
            }

            AjaxForwardUtils.InitResponse(httpContext.Response);
            httpContext.Response.Write(Newtonsoft.Json.JavaScriptConvert.SerializeObject(jsonArray));
            return null;
        }
    }
}
