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
using TSCommon_Core.Security.Domain;
using TSLibWeb.WEB;
using TSCommon_Core.Security.Service;
using TSCommon_Core.TSWebContext;
using TSLib;

namespace TSCommon_Web.Security
{
    public partial class PrivilegeForm : StrutsFormPage<Privilege>
    {
        #region ���Service��������

        private IPrivilegeService privilegeService;     // Ȩ�����õ�Service
        private IModelService modelService;             // ģ�����õ�Service

        #endregion

        #region ���캯��

        /// <summary>
        /// ���캯������ʼ�����Service
        /// </summary>
        public PrivilegeForm()
        {
            this.privilegeService = (IPrivilegeService)GetObject("PrivilegeService");
            this.modelService = (IModelService)GetObject("ModelService");
        }
        #endregion
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            BindDropDownList();
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
                this.OrderNo.ReadOnly = isReadonly;
                this.UrlPath.ReadOnly = isReadonly;
                this.Type.Enabled = !isReadonly;
                this.ModelID.Enabled = !isReadonly;
                if (isReadonly)
                {
                    this.Name.CssClass = this.ZdField;
                    this.Code.CssClass = this.ZdField;
                    this.OrderNo.CssClass = this.ZdField;
                    this.UrlPath.CssClass = this.ZdField;
                    this.ModelID.CssClass = this.ZdField;
                }
                else
                {
                    this.Name.CssClass = this.BtField;
                    this.Code.CssClass = this.BtField;
                    this.OrderNo.CssClass = this.BtField;
                    this.UrlPath.CssClass = this.BtField;
                    this.ModelID.CssClass = this.BtField;
                }
            }
        }

        private void BindDropDownList()
        {
            ArrayList modelList = new ArrayList();
            modelList.AddRange(this.modelService.FindAll());

            ModelID.DataSource = modelList;
            ModelID.DataTextField = "Name";
            ModelID.DataValueField = "ID";
            ModelID.DataBind();
        }
    }
}
