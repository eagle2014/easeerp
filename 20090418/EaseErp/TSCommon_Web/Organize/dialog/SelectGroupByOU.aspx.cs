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
using TSCommon_Core.Organize.Service;
using TSLibWeb.Utils;
using TSCommon_Core.TSWebContext;
using Newtonsoft.Json;
using TSLib;
using TSCommon_Core.Organize.Domain;
using TSLibWeb.Json;

namespace TSCommon_Web.Organize.dialog
{
    public partial class SelectGroupByOU :TSLibWeb.WEB.Page
    {
        #region 相关Service变量定义

        private IGroupService groupService;         // 岗位配置的Service

        #endregion

        #region 构造函数

        public SelectGroupByOU()
        {
            this.groupService = (IGroupService)GetObject("GroupService");
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ListSelectionMode selectionMode = "true".Equals(this.Request["singleSelect"]) ? ListSelectionMode.Single : ListSelectionMode.Multiple;
            this.Options.SelectionMode = selectionMode;

            // 设置根OU节点
            this.rootOUUnid = RequestUtils.GetStringParameter(this.Context, "rootOUUnid", TSWEBContext.Current.CurUser.UnitUnid);
            this.rootOUName = HttpUtility.UrlDecode(RequestUtils.GetStringParameter(this.Context, "rootOUName", TSWEBContext.Current.CurUser.UnitFullName));
            this.groupType = HttpUtility.UrlDecode(RequestUtils.GetStringParameter(this.Context, "groupType", "0"));

            // 是否预加载所有符合条件的岗位信息
            bool preLoadAllGroup = RequestUtils.GetBoolParameter(this.Context, "preLoad", false);
            if (preLoadAllGroup)
                BindAllGroup();

            if (string.IsNullOrEmpty(groupsJson))
                groupsJson = JavaScriptConvert.SerializeObject(new JavaScriptArray());
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

        private string groupType;
        /// <summary>
        /// 岗位类型，默认为"0"
        /// </summary>
        public string GroupType
        {
            get
            {
                return groupType;
            }
        }

        // 绑定岗位列表
        private void BindAllGroup()
        {
            IList groups = this.groupService.FindByOU(rootOUUnid, false, groupType, true);
            HtmlOption[] groupOptions = new HtmlOption[groups.Count];
            Group group;
            for (int i = 0; i < groups.Count; i++)
            {
                group = groups[i] as Group;
                groupOptions[i] = new HtmlOption(group.Name + " [" + group.OUFullName + "]", group.Unid);
            }
            this.Options.DataSource = groupOptions;
            this.Options.DataTextField = "OptionName";
            this.Options.DataValueField = "OptionValue";
            this.Options.DataBind();

            // 创建岗位列表的json字符串
            JavaScriptArray jsonArray = new JavaScriptArray();
            JavaScriptObject jsonObject;
            foreach (Group group1 in groups)
            {
                jsonObject = JsonUtils.CreateJsonObject(group1);
                jsonArray.Add(jsonObject);
            }
            this.groupsJson = JavaScriptConvert.SerializeObject(jsonArray);
        }

        private string groupsJson;
        /// <summary>
        /// 岗位列表的json字符串
        /// </summary>
        public string GroupsJson
        {
            get
            {
                return groupsJson;
            }
        }
    }
}
