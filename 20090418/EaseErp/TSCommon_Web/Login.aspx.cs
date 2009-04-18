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
using TSCommon_Core.Security.Service;
using TSCommon_Core.Organize.Service;
using TSCommon_Core.TSWebContext;
using TSLib;

namespace TSCommon_Web
{
    public partial class Login :TSLibWeb.WEB.Page
    {
        private static ILog logger = LogManager.GetLogger(typeof(Login));

        #region Ïà¹ØService
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
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            saveRelationShip();
            Session["user"] = "user";
            Response.Redirect("Default.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            login(UserName.Text.Trim(), UserPass.Text.Trim());
        }

        private void login(string pUser, string pPassword)
        {
            TSCommon_Core.Organize.Domain.User user = this.userService.Authorize(pUser, pPassword);
            user = this.userService.Load(user.ID);
            TSWEBContext context = TSWEBContext.Current;
            context.SetCurUser(user);
            context.SetCurUserGroupInfo(user.GroupLists);
            context.SetCurUserRoleInfo(this.roleService.FindByUser(user.Unid));
            string LoginSuccessPage = SimpleResourceHelper.GetString("LOGIN.SUCCESS.PAGE");
            Response.Redirect(LoginSuccessPage);
        }

        private void saveRelationShip()
        {
            TSCommon_Core.Organize.Domain.RelationShip relation = new TSCommon_Core.Organize.Domain.RelationShip();
            TSCommon_Core.Organize.RelationShips.IRelationShipService service = (TSCommon_Core.Organize.RelationShips.IRelationShipService)GetObject("RelationShipService");
            relation.ParentUnid = Guid.NewGuid().ToString("N");
            relation.ParentType = "parent";
            relation.ChildUnid = Guid.NewGuid().ToString("N");
            relation.ChildType = "Child";
            relation.RelationShipType = "parent.child";
            service.Save(relation);
        }
    }
}
