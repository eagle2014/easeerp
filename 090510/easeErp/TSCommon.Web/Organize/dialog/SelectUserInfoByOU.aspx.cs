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
using TSCommon.Core.Organize.Service;
using TSLibWeb.Utils;
using Newtonsoft.Json;
using TSCommon.Core.TSWebContext;
using TSLib;
using TSCommon.Core.Organize.Domain;
using TSLibWeb.Json;

namespace TSCommon.Web.Organize.dialog
{
    public partial class SelectUserInfoByOU :TSLibWeb.WEB.Page
    {
        #region 相关Service变量定义

        private IUserService userInfoService;         // 人员配置的Service

        #endregion

        #region 构造函数

        public SelectUserInfoByOU()
        {
            this.userInfoService = (IUserService)GetObject("UserService");
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ListSelectionMode selectionMode = "true".Equals(this.Request["singleSelect"]) ? ListSelectionMode.Single : ListSelectionMode.Multiple;
            this.Options.SelectionMode = selectionMode;

            // 设置根OU节点
            this.rootOUUnid = RequestUtils.GetStringParameter(this.Context, "rootOUUnid", TSWEBContext.Current.CurUserUnitUnid);
            this.rootOUName = HttpUtility.UrlDecode(RequestUtils.GetStringParameter(this.Context, "rootOUName", TSWEBContext.Current.CurUser.UnitFullName));
            this.userType = HttpUtility.UrlDecode(RequestUtils.GetStringParameter(this.Context, "userType", null));

            // 是否预加载所有符合条件的岗位信息
            bool preLoadAllUserInfo = RequestUtils.GetBoolParameter(this.Context, "preLoad", false);
            if (preLoadAllUserInfo)
                BindAllUserInfo();

            if (string.IsNullOrEmpty(userInfosJson))
                userInfosJson = JavaScriptConvert.SerializeObject(new JavaScriptArray());
        }


        private string rootOUName;
        /// <summary>
        /// 当前OU的全名
        /// </summary>
        public string RootOUName
        {
            get
            {
                return rootOUName;
            }
        }

        private string rootOUUnid;
        /// <summary>
        /// 当前OU的Unid
        /// </summary>
        public string RootOUUnid
        {
            get
            {
                return rootOUUnid;
            }
        }

        private string userType;
        /// <summary>
        /// 用户类型，默认为全部
        /// </summary>
        public string UserType
        {
            get
            {
                return userType;
            }
        }

        // 绑定人员列表
        private void BindAllUserInfo()
        {
            IList userInfos = this.userInfoService.FindByOU(rootOUUnid, userType);
            HtmlOption[] userInfoOptions = new HtmlOption[userInfos.Count];
            User userInfo;
            for (int i = 0; i < userInfos.Count; i++)
            {
                userInfo = userInfos[i] as User;
                userInfoOptions[i] = new HtmlOption(userInfo.Name + " [" + userInfo.OUFullName + "]", userInfo.Unid);
            }
            this.Options.DataSource = userInfoOptions;
            this.Options.DataTextField = "OptionName";
            this.Options.DataValueField = "OptionValue";
            this.Options.DataBind();

            // 创建人员列表的json字符串
            JavaScriptArray jsonArray = new JavaScriptArray();
            JavaScriptObject jsonObject;
            foreach (User userInfo1 in userInfos)
            {
                jsonObject = JsonUtils.CreateJsonObject(userInfo1);
                jsonArray.Add(jsonObject);
            }
            this.userInfosJson = JavaScriptConvert.SerializeObject(jsonArray);
        }

        private string userInfosJson;
        /// <summary>
        /// 人员列表的json字符串
        /// </summary>
        public string UserInfosJson
        {
            get
            {
                return userInfosJson;
            }
        }
    }
}
