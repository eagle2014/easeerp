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
using TSCommon_Core.TSWebContext;
using log4net;

namespace TSCommon_Web
{
    public partial class LoginSuccess : TSLibWeb.WEB.Page
    {
        private static ILog logger = LogManager.GetLogger(typeof(LoginSuccess));
        protected void Page_Load(object sender, EventArgs e)
        {
            // 输出登录信息
            string msg = "用户成功登录系统";
            try
            {
                msg += "：";
                msg += TSWEBContext.Current.CurUser.Name;
                msg += "[" + TSWEBContext.Current.CurUser.LoginID + "]";
                msg += "[" + TSWEBContext.Current.CurUser.OUFullName + "]";
            }
            catch (Exception)
            {
                msg += "!";
            }
            logger.Info(msg);
        }
    }
}
