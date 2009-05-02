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
using EaseErp.IC.Domain;
using TSLibWeb.WEB;
using EaseErp.IC.Service;
using TSLib;
using TSLib.Utils;
using TSCommon.Core.TSWebContext;

namespace TSCommon.Web.WareHouse
{
    public partial class ThingForm : StrutsFormPage<Thing>
    {
        #region 相关Service
        private IWHUnitService whUnitService = null;
        #endregion

        public ThingForm()
        {
            this.whUnitService = (IWHUnitService)GetObject("WHUnitService");
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            BindDropDownList();
        }

        private void BindDropDownList()
        {
            ArrayList whunitList = new ArrayList();
            whunitList.Add(new WHUnit(Constants.BLANK_LONG_VALUE, SimpleResourceHelper.GetString("SELECT.EMPTY.LABEL")));
            whunitList.AddRange(this.whUnitService.FindAll());

            WHUnit_ID.DataSource = whunitList;
            WHUnit_ID.DataTextField = "Name";
            WHUnit_ID.DataValueField = "ID";
            WHUnit_ID.DataBind();
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
                this.WHUnit_ID.Enabled = !isReadonly;
                if (isReadonly)
                {
                    this.Name.CssClass = this.ZdField;
                    this.Code.CssClass = this.ZdField;
                    this.Memo.CssClass = this.ZdField;
                    this.WHUnit_ID.CssClass = this.ZdField;
                }
                else
                {
                    this.Name.CssClass = this.BtField;
                    this.Code.CssClass = this.BtField;
                    this.Memo.CssClass = this.BtField;
                    this.WHUnit_ID.CssClass = this.BtField;
                }
            }
        }
    }
}
