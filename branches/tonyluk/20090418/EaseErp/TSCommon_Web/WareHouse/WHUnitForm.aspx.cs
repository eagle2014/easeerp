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
        /// �жϵ�ǰ�û��Ƿ�Ϊ��֯�ṹ����Ա
        /// </summary>
        public bool IsManager
        {
            get
            {
                return TSWEBContext.Current.IsHasPrivilege(SimpleResourceHelper.GetString("PRIVILEDGE.CODE.ADMIN_ALL"));
            }
        }

        /// <summary>
        /// ��ȡ������ҳ��ı༭״̬
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
