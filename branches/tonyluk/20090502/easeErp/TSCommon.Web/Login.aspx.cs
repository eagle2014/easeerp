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
using log4net;
using TSCommon.Core.Security.Service;
using TSCommon.Core.Organize.Service;
using TSCommon.Core.TSWebContext;
using TSLib;

namespace TSCommon.Web
{
    public partial class Login : TSLibWeb.WEB.Page
    {
        private static ILog logger = LogManager.GetLogger(typeof(Login));

        #region 相关Service
        private string errorMessage;
        private IRoleService roleService;
        private IUserService userService;
        public IRoleService RoleService
        {
            set { this.roleService = value; }
        }
        public IUserService UserService
        {
            set { this.userService = value; }
        }
        public Login()
        {
        }
        #endregion
        /// <summary>
        /// 错误的提示信息
        /// </summary>
        protected string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string errorInfo = (string)this.Page.Request.Params["errorInfo"];
            if (!String.IsNullOrEmpty(errorInfo)) 
            {
                this.errorMessage = errorInfo;
            }
            this.UserName.Focus();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                login(UserName.Text.Trim(), UserPass.Text.Trim());
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.Message;
            }
        }

        private void login(string pUser, string pPassword)
        {
            TSCommon.Core.Organize.Domain.User user = this.userService.Authorize(pUser, pPassword);
            user = this.userService.Load(user.ID);
            TSWEBContext context = TSWEBContext.Current;
            context.SetCurUser(user);
            context.SetCurUserGroupInfo(user.GroupLists);
            context.SetCurUserRoleInfo(this.roleService.FindByUser(user.Unid));
            string loginSuccessPage = SimpleResourceHelper.GetString("LOGIN.SUCCESS.PAGE");
            if (string.IsNullOrEmpty(loginSuccessPage)) loginSuccessPage = "LoginSuccess.aspx";
            Response.Redirect(loginSuccessPage);
        }
    }
}
