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
using TSLibWeb.WEB;

namespace TSCommon.Web
{
    public partial class test : StrutsCorePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.ContentType = "APPLICATION/OCTET-STREAM";
            //Response.AddHeader("Content-Disposition", "attachment; filename=" +DateTime.Now.ToString("yyyyMMdd")+".txt");
            //Response.Write("hello,world");
            //Response.End();
        }
    }
}
