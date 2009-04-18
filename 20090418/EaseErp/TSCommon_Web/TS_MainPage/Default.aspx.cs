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
using Common.Logging;
using TSLib;
using TSCommon_Core.Organize.Service;
using TSCommon_Core.Organize.Domain;

namespace TSCommon_Web.TS_MainPage
{
    public partial class _Default:TSLibWeb.WEB.Page
    {
        private ILog log = LogManager.GetLogger(typeof(_Default));
        private IUserService userService;
        public IUserService UserService
        {
            set { this.userService = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            log.Warn("hello");
        }

        protected void DO_Click(object sender, EventArgs e)
        {
            User user = new User();
            user.ID = -1;
            user.Address = "address";
            user.Email = "tonyluk@live.cn";
            user.Gender = 1;
            user.LoginID = "admin";
            user.Name = "admin";
            user.TelephoneNo = "0668";
            user.UserStatus = UserStatuses.Add;
            user.Mobile = "135";
            user.Password = "123456";
            log.Warn(user.Unid);
            userService.Save(user);
        }
        /// <summary>
        /// 页眉
        /// </summary>
        public string HeaderPage
        {
            get { return this.ContextPath + "/" + SimpleResourceHelper.GetString("MAIN_PAGE.HEADER_PAGE"); }
        }

        /// <summary>
        /// 页脚
        /// </summary>
        public string FooterPage
        {
            get { return this.ContextPath + "/" + SimpleResourceHelper.GetString("MAIN_PAGE.FOOTER_PAGE"); }
        }

        /// <summary>
        /// 首页
        /// </summary>
        public string FirstPage
        {
            get { return this.ContextPath + "/" + SimpleResourceHelper.GetString("MAIN_PAGE.FIRST_PAGE"); }
        }

        /// <summary>
        /// 首页页签的ID
        /// </summary>
        public string FirstPageTabId
        {
            get
            {
                string firstPageTabId = SimpleResourceHelper.GetString("MAIN_PAGE.FIRST_PAGE_TABID");
                if (string.IsNullOrEmpty(firstPageTabId))
                {
                    firstPageTabId = "firstPagePanel";
                }
                return firstPageTabId;
            }
        }

        /// <summary>
        /// 首页页签的Title
        /// </summary>
        public string FirstPageTabTitle
        {
            get
            {
                string firstPageTabTitle = SimpleResourceHelper.GetString("MAIN_PAGE.FIRST_PAGE_TABTITLE");
                if (string.IsNullOrEmpty(firstPageTabTitle))
                {
                    firstPageTabTitle = "首页";
                }
                return firstPageTabTitle;
            }
        }

        /// <summary>
        /// 是否显示搜索页
        /// </summary>
        public bool ShowSearchPage
        {
            get { return "true".Equals(SimpleResourceHelper.GetString("MAIN_PAGE.SHOW_SEARCH_PAGE"), StringComparison.OrdinalIgnoreCase); }
        }

        /// <summary>
        /// 搜索页
        /// </summary>
        public string SearchPage
        {
            get { return this.ContextPath + "/" + SimpleResourceHelper.GetString("MAIN_PAGE.SEARCH_PAGE"); }
        }

        /// <summary>
        /// 搜索页
        /// </summary>
        public bool ShowMaskMsg
        {
            get { return "true".Equals(SimpleResourceHelper.GetString("MAIN_PAGE.SHOW_MASK_MSG"), StringComparison.OrdinalIgnoreCase); }
        }

        /// <summary>
        /// 主页加载后需立即打开的页签的url
        /// </summary>
        public string ToOpenUrl
        {
            get
            {
                return string.Empty;
            }
        }

        private string toOpenUrlTitle = string.Empty;
        /// <summary>
        /// 主页加载后需立即打开的页签的url
        /// </summary>
        public string ToOpenUrlTitle
        {
            get
            {
                return toOpenUrlTitle;
            }
        }
    }
}
