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
using TSCommon_Core.Organize.Domain;
using log4net;
using TSCommon_Core.Organize.Service;
using TSCommon_Core.Security.Service;
using TSLibStruts.Utils;
using TSCommon_Core.TSWebContext;
using TSLib;
using System.Text;
using TSLib.Utils;
using TSCommon_Core.SystemPolicy;

namespace TSCommon_Web.Organize
{
    public partial class GroupForm : StrutsFormPage<Group>
    {
        ILog log = LogManager.GetLogger(typeof(GroupForm));

        #region 相关Service变量定义

        private IOUInfoService ouInfoService;       // 组织架构配置的Service
        private IUserService userInfoService;   // 人员配置的Service
        private IGroupService groupService;         // 岗位配置的Service
        private IRoleService roleService;           // 岗位配置的Service

        #endregion

        #region 构造函数

        static GroupForm()
        {
            if (ConvertUtils.Lookup(typeof(GroupStatuses)) == null)
                ConvertUtils.Register(new GroupStatusConverter(), typeof(GroupStatuses));
        }

        public GroupForm()
        {
            this.groupService = (IGroupService)GetObject("GroupService");
            this.ouInfoService = (IOUInfoService)GetObject("OUInfoService");
            this.userInfoService = (IUserService)GetObject("UserService");
            this.roleService = (IRoleService)GetObject("RoleService");
        }

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            BindDropDownList();
        }

        /// <summary>
        /// 判断当前用户是否为组织结构管理员
        /// </summary>
        public bool IsManager
        {
            get
            {
                TSWEBContext tsContext = TSWEBContext.Current;
                User curUser = tsContext.CurUser;
                if (tsContext.IsHasPrivilege(SimpleResourceHelper.GetString("PRIVILEDGE.CODE.ADMIN_ALL"))
                    || tsContext.IsHasPrivilege(SimpleResourceHelper.GetString("PRIVILEDGE.CODE.ADMIN_LOCAL_AND_CHILD"))
                    || tsContext.IsHasPrivilege(SimpleResourceHelper.GetString("PRIVILEDGE.CODE.ADMIN_LOCAL")))
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 获取或设置页面的编辑状态
        /// </summary>
        public override bool CanEdit
        {
            get
            {
                return base.CanEdit;
            }
            set
            {
                base.CanEdit = value;
                bool isReadonly = !value;
                this.Name.ReadOnly = isReadonly;
                this.Code.ReadOnly = isReadonly;
                this.IsCanDispatch.Enabled = !isReadonly;
                this.Memo.ReadOnly = isReadonly;
                this.ImgBtnSelectOU.Visible = !isReadonly;
                this.btnLeftAllArrow.Visible = !isReadonly;
                this.btnLeftArrow.Visible = !isReadonly;
                this.btnRightArrow.Visible = !isReadonly;

                this.btnLeftAllArrow_1.Visible = !isReadonly;
                this.btnLeftArrow_1.Visible = !isReadonly;
                this.btnRightArrow_1.Visible = !isReadonly;

                this.OUInfos.Enabled = value;

                if (isReadonly)
                {
                    this.OUFullName.CssClass = this.ZdField;
                    this.Name.CssClass = this.ZdField;
                    this.Code.CssClass = this.ZdField;
                    this.Memo.CssClass = this.ZdField;

                    this.AllRoles.CssClass = this.ZdField;
                    this.RoleUnids.CssClass = this.ZdField;
                    this.OUInfos.CssClass = this.ZdField;
                    this.AllUserInfos.CssClass = this.ZdField;
                    this.UserInfoUnids.CssClass = this.ZdField;
                }
                else
                {
                    this.OUFullName.CssClass = this.BtField;
                    this.Name.CssClass = this.BtField;
                    this.Code.CssClass = this.BtField;
                    this.Memo.CssClass = "egd-form-field";

                    this.AllRoles.CssClass = "egd-form-field";
                    this.RoleUnids.CssClass = "egd-form-field";
                    this.OUInfos.CssClass = "egd-form-field";
                    this.AllUserInfos.CssClass = "egd-form-field";
                    this.UserInfoUnids.CssClass = "egd-form-field";
                }
            }
        }

        #region 下拉框绑定

        private void BindDropDownList()
        {
            Group group = this.Domain;

            // 绑定已拥有的角色列表
            this.RoleUnids.DataSource = group.RoleLists;
            this.RoleUnids.DataTextField = "Name";
            this.RoleUnids.DataValueField = "Unid";
            this.RoleUnids.DataBind();

            // 绑定已包含的人员列表
            this.UserInfoUnids.DataSource = group.UserInfoLists;
            this.UserInfoUnids.DataTextField = "Name";
            this.UserInfoUnids.DataValueField = "Unid";
            this.UserInfoUnids.DataBind();

            // 绑定所有角色和OU的信息
            if (!string.IsNullOrEmpty(group.OUUnid))
            {
                IList roleList = ListUtils.WeedList(roleService.FindByOU(group.OUUnid), group.RoleLists);
                this.AllRoles.DataSource = roleList;
                this.AllRoles.DataTextField = "Name";
                this.AllRoles.DataValueField = "Unid";
                this.AllRoles.DataBind();

                string policyValue = PolicyHelper.GetPolicyValue("9001");
                if (string.IsNullOrEmpty(policyValue))
                    policyValue = TSLibWeb.Constants.YESNO_YES;
                IList ouList;
                if (policyValue.Equals(TSLibWeb.Constants.YESNO_YES, StringComparison.OrdinalIgnoreCase))
                    ouList = this.ouInfoService.FindOUInfoByParentUnid(group.OUUnid, true, true);
                else
                    ouList = this.ouInfoService.FindOUInfoByParentUnid(group.OUUnid, true, false);
                this.OUInfos.DataSource = ouList;
                this.OUInfos.DataTextField = "FullName";
                this.OUInfos.DataValueField = "Unid";
                this.OUInfos.DataBind();
                this.OUInfos.Text = group.OUUnid;

                // 获取该OU中的人员信息
                IList userInfoList = ListUtils.WeedList(this.userInfoService.FindByOU(group.OUUnid), group.UserInfoLists);
                this.AllUserInfos.DataSource = userInfoList;
                this.AllUserInfos.DataTextField = "Name";
                this.AllUserInfos.DataValueField = "Unid";
                this.AllUserInfos.DataBind();
            }
        }

        #endregion

        #region Controller Methods

        protected void SelectGroupWithOU()
        {
            string ouUnid = this.Request.Params["ouUnid"];
            string selectModel = this.Request.Params["selectModel"];
            if (string.IsNullOrEmpty(ouUnid))
            {
                //this.EgrandMessageId = "2004";
                SetResult("showMessage");
            }

            if (string.IsNullOrEmpty(selectModel))
                selectModel = "single";
            /*
            IList list = this.groupService.FindAllDepartment(ouUnid);
            if (null == list || list.Count == 0)
                this.Session["options"] = new ArrayList();
            else
            {
                IList departs = new ArrayList();
                foreach (OUInfo tempInfo in list)
                {
                    HtmlOption option = new HtmlOption(tempInfo.FullName, tempInfo.Unid);
                    departs.Add(option);
                }
                this.Session["options"] = departs;
            }
            this.Session["title"] = "请选择部门信息：";
            this.Session["selectModel"] = selectModel;
            SetResult("CommonSelect"); */
        }

        protected void FindSendToGroupsWithOUUnid()
        {
            string ouUnid = this.Request.Params["ouUnid"];
            if (string.IsNullOrEmpty(ouUnid))
            {
                Response.Write("");
                Response.End();
            }

            IList list = this.groupService.FindAllSendTo(ouUnid);
            if (null == list || list.Count == 0)
            {
                Response.Write("");
                Response.End();
            }

            StringBuilder sb = new StringBuilder();
            Response.ContentType = ("text/xml;charset=UTF-8");
            Response.Charset = "UTF-8";
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            sb.Append("<response>");
            foreach (Group group in list)
            {
                sb.Append("<entry>");
                sb.Append("<name>" + group.Name + "</name>");
                sb.Append("<value>" + group.Unid + "</value>");
                sb.Append("</entry>");
            }
            sb.Append("</response>");
            Response.Write(sb.ToString());
            Response.End();
        }

        #endregion
    }
}
