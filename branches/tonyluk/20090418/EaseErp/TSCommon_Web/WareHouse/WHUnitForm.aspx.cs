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
using EaseErp_WareHouse.WareHouse.Domain;
using TSLibWeb.WEB;
using TSCommon_Core.TSWebContext;
using TSLib;

namespace TSCommon_Web.WareHouse
{
    public partial class WHUnitForm : StrutsFormPage<WHUnit>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
                this.Memo.ReadOnly = isReadonly;
                if (isReadonly)
                {
                    this.Name.CssClass = this.ZdField;
                    this.Code.CssClass = this.ZdField;
                    this.Memo.CssClass = this.ZdField;
                }
                else
                {
                    this.Name.CssClass =this.BtField;
                    this.Code.CssClass = this.BtField;
                    this.Memo.CssClass = this.BtField;
                }
            }
        }
    }
}
