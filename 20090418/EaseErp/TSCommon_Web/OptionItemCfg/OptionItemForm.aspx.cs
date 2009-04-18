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
using TSCommon_Core.OptionItemCfg.Domain;
using TSCommon_Core.OptionItemCfg.Service;
using log4net;
using TSCommon_Core.TSWebContext;
using TSLib;

namespace TSCommon_Web.OptionItemCfg
{
    public partial class OptionItemForm : StrutsFormPage<OptionItem>
    {
        private static ILog logger = LogManager.GetLogger(typeof(OptionItemForm));

        #region 相关Service变量定义

        /// <summary>业务对象配置Service</summary>
        private IOptionItemService optionItemService;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数，初始化相关Service
        /// </summary>
        public OptionItemForm()
        {
            this.optionItemService = (IOptionItemService)GetObject("OptionItemService");
        }

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            BindType();
        }

        /// <summary>
        /// 绑定所属分类下拉框
        /// </summary>
        private void BindType()
        {
            Type.DataSource = this.optionItemService.GetTypeOptions();
            Type.DataTextField = "OptionName";
            Type.DataValueField = "OptionValue";
            Type.DataBind();
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
                this.Type.Enabled = value;
                this.OrderNo.ReadOnly = !value;
                this.Name.ReadOnly = !value;
                this.Code.ReadOnly = !value;

                if (value)
                {
                    this.Type.CssClass = "egd-form-btField";
                    this.OrderNo.CssClass = "egd-form-btField";
                    this.Name.CssClass = "egd-form-btField";
                    this.Code.CssClass = "egd-form-btField";
                }
                else
                {
                    this.Type.CssClass = "egd-form-zdField";
                    this.OrderNo.CssClass = "egd-form-zdField";
                    this.Name.CssClass = "egd-form-zdField";
                    this.Code.CssClass = "egd-form-zdField";
                }
            }
        }
    }
}
