using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace TSCommon.Web.Organize.tab
{
    public partial class UserTab : System.Web.UI.Page
    {
        protected bool flag;
        protected void Page_Load(object sender, EventArgs e)
        {
            string strAction = Request.Params["action"];
            string otherUnid = Request.Params["otherUnid"];
            string otherType = Request.Params["otherType"];
            if (!(string.IsNullOrEmpty(otherUnid) || string.IsNullOrEmpty(otherType)))
            {
                this.otherUnid.Value = otherUnid;
                this.otherType.Value = otherType;
            }
            if (strAction.Equals("open", StringComparison.OrdinalIgnoreCase))
            {
                flag = false;
            }
            else
            {
                flag = true;
            }
        }
    }
}
