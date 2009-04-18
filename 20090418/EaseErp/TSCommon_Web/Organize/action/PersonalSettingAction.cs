using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using TSCommon_Core.Organize.Domain;
using TSLibWeb.Struts;
using log4net;
using TSLibStruts;
using TSCommon_Core.Organize.Service;
using TSLib;
using TSCommon_Core.TSWebContext;

namespace TSCommon_Web.Organize.action
{
    public class PersonalSettingAction : StrutsEntityAction<User>
    {
        private static ILog logger = LogManager.GetLogger(typeof(PersonalSettingAction));

        public override Forward Unspecified(ActionContext actionContext, HttpContext context)
        {
            return this.Edit(actionContext, context);
        }

        #region 相关Service变量定义

        /// <summary>人员配置Service</summary>
        private IUserService userService;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数，初始化相关Service
        /// </summary>
        public PersonalSettingAction()
        {
            this.userService = (IUserService)GetObject("UserService");
        }

        #endregion

        #region StrutsEntityAction的实现

        private string errorMsg = "不支持此方法！";

        protected override User LoadEntity(ActionContext actionContext, HttpContext httpContext,string idValue, string idName)
        {
            // 返回当前用户
            return TSWEBContext.Current.CurUser;
        }

        protected override void SaveEntity(ActionContext actionContext, HttpContext httpContext, User entity)
        {
            User oldUser = this.userService.Load(entity.ID);
            oldUser.Email = entity.Email;
            oldUser.TelephoneNo = entity.TelephoneNo;
            oldUser.Mobile = entity.Mobile;
            oldUser.Address = entity.Address;
            oldUser.Office = entity.Office;

            User curUser = TSWEBContext.Current.CurUser;
            this.userService.ParseSave(curUser, oldUser);

            // 更新当前用户的信息
            curUser.Email = oldUser.Email;
            curUser.TelephoneNo = oldUser.TelephoneNo;
            curUser.Mobile = oldUser.Mobile;
            curUser.Address = oldUser.Address;
            curUser.Office = oldUser.Office;
        }

        protected override User CreateEntity(ActionContext actionContext, HttpContext httpContext)
        {
            throw new Exception(errorMsg);
        }

        protected override void Delete(ActionContext actionContext, HttpContext httpContext,string[] ids, string type)
        {
            throw new Exception(errorMsg);
        }

        protected override PageInfo GetPageInfo(ActionContext actionContext, HttpContext httpContext,int pageNo, int pageSize, string sortField, string sortDir)
        {
            throw new Exception(errorMsg);
        }

        #endregion
    }
}
