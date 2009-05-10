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
using TSCommon.Core.Organize.Domain;
using TSCommon.Core.TSWebContext;
using TSLib.Utils;
using TSLibWeb.Utils;

namespace TSCommon.Web.Organize.action
{
    public class OUTreeAction : StrutsCoreAction
    {
        private static ILog logger = LogManager.GetLogger(typeof(OUTreeAction));

        // 组织架构配置的Service
        private IOUInfoService ouInfoService;
        public IOUInfoService OUInfoService
        {
            set { ouInfoService = value; }
        }

        public OUTreeAction()
        {
            this.ouInfoService = GetObject("OUInfoService") as IOUInfoService;
        }

        public virtual Forward GetOUTree(ActionContext actionContext, HttpContext httpContext)
        {
            string node = httpContext.Request.Params["node"];
            IList ouTree = null;
            User curUser = TSWEBContext.Current.CurUser;
            if (TSWEBContext.Current.IsHasPrivilege(Constants.ADMIN_ALL))
                ouTree = this.ouInfoService.GetOUTree(curUser, Constants.OT_ALL);
            else if (TSWEBContext.Current.IsHasPrivilege(Constants.ADMIN_LOCALANDCHILD))
                ouTree = this.ouInfoService.GetOUTree(curUser, Constants.OT_LOCALANDCHILD);
            else if (TSWEBContext.Current.IsHasPrivilege(Constants.ADMIN_LOCAL))
                ouTree = this.ouInfoService.GetOUTree(curUser, Constants.OT_LOCAL);

            Newtonsoft.Json.JavaScriptArray jsonArray = new Newtonsoft.Json.JavaScriptArray();
            if (!(string.IsNullOrEmpty(node) || ouTree == null))
            {
                foreach (object obj in ouTree)
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
