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
using TSCommon.Core.Organize.Domain;
using TSLibWeb.WEB;
using TSCommon.Core.Organize.Service;
using TSCommon.Core.TSWebContext;
using TSLib;

namespace TSCommon.Web.Organize
{
    /// <summary>
    /// 职务Form处理
    /// </summary>
    /// <author>dragon</author>
    /// <date>2008-05-31</date>
    public partial class JobTitleForm : StrutsFormPage<JobTitle>
    {
        #region 相关Service变量定义

        /// <summary>职务配置的Service</summary>
        private IJobTitleService jobTitleService;
        /// <summary>级别配置的Service</summary>
        private IOULevelService ouLevelService;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数，初始化相关Service
        /// </summary>
        public JobTitleForm()
        {
            this.jobTitleService = (IJobTitleService)GetObject("JobTitleService");
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
                this.Code.ReadOnly = isReadonly;
                this.Level.Enabled = value;

                this.Name.ReadOnly = isReadonly;
                if (isReadonly)
                {
                    this.Name.CssClass = this.ZdField;
                    this.Code.CssClass = this.ZdField;
                    this.Level.CssClass = this.ZdField;
                }
                else
                {
                    this.Name.CssClass = this.BtField;
                    this.Code.CssClass = this.BtField;
                    this.Level.CssClass = this.BtField;
                }
            }
        }

        #region 下拉框绑定

        private void BindDropDownList()
        {
            OULevelForm.BindUseDropDownList(this.Level, this.Domain.Level, this.Domain.LevelName);
        }

        #endregion
    }
}
