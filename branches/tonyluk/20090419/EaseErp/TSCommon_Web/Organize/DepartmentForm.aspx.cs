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
using TSCommon_Core.Organize.Service;
using TSLibStruts.Utils;
using TSCommon_Core.TSWebContext;
using TSLib;

namespace TSCommon_Web.Organize
{
    public partial class DepartmentForm : StrutsFormPage<OUInfo>
    {
        #region 相关Service变量定义

        private IOUInfoService ouInfoService;     // 组织架构配置的Service
        private IOULevelService ouLevelService;   // 单位级别配置的Service

        #endregion

        #region 构造函数

        static DepartmentForm()
        {
            if (ConvertUtils.Lookup(typeof(OUStatuses)) == null)
                ConvertUtils.Register(new OUStatusConverter(), typeof(OUStatuses));
        }

        public DepartmentForm()
        {
            this.ouInfoService = (IOUInfoService)GetObject("OUInfoService");
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
                this.OrderNo.ReadOnly = isReadonly;
                this.Description.ReadOnly = isReadonly;
                this.ImgBtnSelectOU.Visible = !isReadonly;
                this.ImgBtnSelectUpper.Visible = !isReadonly;
                this.ImgBtnClear.Visible = !isReadonly;

                if (isReadonly)
                {
                    this.Name.CssClass = this.ZdField;
                    this.Code.CssClass = this.ZdField;
                    this.OrderNo.CssClass = this.ZdField;
                    this.Description.CssClass = this.ZdField;
                }
                else
                {
                    this.Name.CssClass = this.BtField;
                    this.Code.CssClass = this.BtField;
                    this.OrderNo.CssClass = this.BtField;
                    this.Description.CssClass = this.BtField;
                }

                this.OUStatus.Enabled = !isReadonly;
                this.OUStatus.CssClass = isReadonly ? this.ZdField : this.BtField;
            }
        }

        #region 下拉框绑定

        private void BindDropDownList()
        {
            UnitForm.BindOUStatus(this.OUStatus);
        }

        #endregion
    }
}
