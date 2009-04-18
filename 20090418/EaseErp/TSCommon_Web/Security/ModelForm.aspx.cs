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
using TSLibWeb;

namespace TSCommon_Web.Security
{
    public partial class ModelForm : StrutsFormPage<Model>
    {
        #region 相关Service变量定义

        private IModelService modelService;     // 模块配置的Service

        #endregion

        #region 构造函数

        static ModelForm()
        {
            if (TSLibStruts.Utils.ConvertUtils.Lookup(typeof(ModelTypes)) == null)
                TSLibStruts.Utils.ConvertUtils.Register(new ModelTypeConverter(), typeof(ModelTypes));
        }

        /// <summary>
        /// 构造函数，初始化相关Service
        /// </summary>
        public ModelForm()
        {
            this.modelService = (IModelService)GetObject("ModelService");
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
                this.Type.Enabled = !isReadonly;
                this.ParentID.Enabled = !isReadonly;
                if (isReadonly)
                {
                    this.Name.CssClass = this.ZdField;
                    this.Code.CssClass = this.ZdField;
                    this.OrderNo.CssClass = this.ZdField;
                    this.ParentID.CssClass = this.ZdField;
                }
                else
                {
                    this.Name.CssClass = this.BtField ;
                    this.Code.CssClass = this.BtField;
                    this.OrderNo.CssClass = this.BtField;
                    this.ParentID.CssClass = this.BtField;
                }
            }
        }

        private void BindDropDownList()
        {
            ArrayList modelList = new ArrayList();
            modelList.Add(new Model(Constants.BLANK_LONG_VALUE, SimpleResourceHelper.GetString("SELECT.EMPTY.LABEL")));
            modelList.AddRange(this.modelService.FindAllWithExclude(this.Domain.ID));

            ParentID.DataSource = modelList;
            ParentID.DataTextField = "Name";
            ParentID.DataValueField = "ID";
            ParentID.DataBind();
        }
    }
}
