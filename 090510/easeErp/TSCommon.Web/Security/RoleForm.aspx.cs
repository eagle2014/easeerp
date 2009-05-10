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
using TSCommon.Core.Security.Domain;
using TSLibWeb.WEB;
using log4net;
using TSCommon.Core.Security.Service;
using TSCommon.Core.Organize.Service;
using TSCommon.Core.TSWebContext;
using TSLib;
using TSCommon.Web.Organize;

namespace TSCommon.Web.Security
{
    public partial class RoleForm : StrutsFormPage<Role>
    {
        private static ILog logger = LogManager.GetLogger(typeof(RoleForm));

        #region 相关Service变量定义

        private IRoleService roleService;           // 角色配置的Service
        private IOULevelService ouLevelService;     // 单位级别配置的Service
        private IPrivilegeService privilegeService; // 权限配置的Service
        private IModelService modelService;         // 模块配置的Service

        #endregion


        #region 构造函数

        /// <summary>
        /// 构造函数，初始化相关Service
        /// </summary>
        public RoleForm()
        {
            this.privilegeService = (IPrivilegeService)GetObject("PrivilegeService");

            this.modelService = (IModelService)GetObject("ModelService");
            this.roleService = (IRoleService)GetObject("RoleService");
            this.ouLevelService = (IOULevelService)GetObject("OULevelService");
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
                return TSWEBContext.Current.IsHasPrivilege(SimpleResourceHelper.GetString("PRIVILEDGE.CODE.ADMIN_ALL"));
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
                this.Level.Enabled = !isReadonly;
                this.Memo.ReadOnly = isReadonly;
                this.btnRightArrow.Visible = !isReadonly;
                this.btnLeftAllArrow.Visible = !isReadonly;
                this.btnLeftArrow.Visible = !isReadonly;
                this.Models.Enabled = value;
                this.AllPrivilege.Enabled = value;
                this.PrivilegeIDs.Enabled = value;
                if (isReadonly)
                {
                    this.Name.CssClass = this.ZdField;
                    this.Code.CssClass = this.ZdField;
                    this.Memo.CssClass = this.ZdField;
                    this.Models.CssClass = this.ZdField;
                    this.AllPrivilege.CssClass = this.ZdField;
                    this.PrivilegeIDs.CssClass = this.ZdField;
                }
                else
                {
                    this.Name.CssClass = this.BtField;
                    this.Code.CssClass = this.BtField;
                    this.Memo.CssClass = this.BtField;
                    this.Models.CssClass = this.BtField;
                    this.AllPrivilege.CssClass = this.BtField;
                    this.PrivilegeIDs.CssClass = this.BtField;
                }
            }
        }



        private void BindDropDownList()
        {
            Role role = this.Domain;
            OULevelForm.BindUseDropDownList(this.Level, this.Domain.Level, this.Domain.LevelName);

            // 绑定权限
            this.PrivilegeIDs.DataSource = role.Privileges;
            this.PrivilegeIDs.DataTextField = "Name";
            this.PrivilegeIDs.DataValueField = "ID";
            this.PrivilegeIDs.DataBind();

            // 绑定模块选项列表
            ArrayList modelList = new ArrayList();
            modelList.AddRange(this.modelService.FindAll());
            this.Models.DataSource = modelList;
            this.Models.DataTextField = "Name";
            this.Models.DataValueField = "ID";
            this.Models.DataBind();

            // 绑定尚未选择的权限列表
            if (null != modelList && modelList.Count > 0)
            {
                Model model = modelList[0] as Model;
                ArrayList privilegeList = new ArrayList();
                IList allPrivileges = this.privilegeService.FindByModel(model.ID);
                if (null != role.Privileges && role.Privileges.Count > 0)
                {
                    foreach (Privilege p in allPrivileges)
                    {
                        bool isHas = false;
                        foreach (Privilege p1 in role.Privileges)
                        {
                            if (p1.ID == p.ID)
                            {
                                isHas = true;
                                break;
                            }
                        }
                        if (!isHas)
                            privilegeList.Add(p);
                    }
                }
                else
                {
                    privilegeList.AddRange(allPrivileges);
                }
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("角色配置数量：" + privilegeList.Count.ToString());
                }
                this.AllPrivilege.DataSource = privilegeList;
                this.AllPrivilege.DataTextField = "Name";
                this.AllPrivilege.DataValueField = "ID";
                this.AllPrivilege.DataBind();
            }
        }
    }
}
