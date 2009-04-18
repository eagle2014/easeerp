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
using TSCommon_Core.Organize.Domain;
using TSLibWeb.WEB;
using TSCommon_Core.Organize.Service;
using TSCommon_Core.TSWebContext;
using TSLib;
using TSLibWeb;

namespace TSCommon_Web.Organize
{
    public partial class OULevelForm : StrutsFormPage<OULevel>
    {
        #region 相关Service变量定义

        /// <summary>级别配置的Service</summary>
        private static IOULevelService ouLevelService;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数，初始化相关Service
        /// </summary>
        static OULevelForm()
        {
            ouLevelService = (IOULevelService)GetObject("OULevelService");
        }

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
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

                this.Name.ReadOnly = !value;
                this.Code.Enabled = value;
                if (value)
                {
                    this.Name.CssClass = this.BtField;
                    this.Code.CssClass = this.BtField;
                }
                else
                {
                    this.Name.CssClass = this.ZdField ;
                    this.Code.CssClass = this.ZdField;
                }
            }
        }

        /// <summary>
        /// 绑定级别列表
        /// </summary>
        /// <param name="toBind">要绑定到的列表</param>
        /// <param name="curLevelCode">当前级别的编码</param>
        /// <param name="curLevelName">当前级别的名称</param>
        public static void BindUseDropDownList(DropDownList toBind, string curLevelCode, string curLevelName)
        {
            IList list = ouLevelService.FindAll();

            //添加空白选项
            OULevel ouLevel = new OULevel();
            ouLevel.ID = Constants.BLANK_INT_VALUE;
            ouLevel.Unid = " ";
            ouLevel.Name = "未配置";
            ouLevel.Code = Constants.BLANK_STRING_VALUE;
            list.Insert(0, ouLevel);

            // 处理级别被删除的特例情况
            bool isIn = false;
            foreach (OULevel item in list)
            {
                if (item.Code == curLevelCode)
                {
                    isIn = true;
                    break;
                }
            }
            if (!isIn && !string.IsNullOrEmpty(curLevelCode) && !Constants.BLANK_STRING_VALUE.Equals(curLevelCode, StringComparison.OrdinalIgnoreCase))
            {
                // 创建一个已丢失的选项
                ouLevel = new OULevel();
                ouLevel.ID = Constants.BLANK_INT_VALUE;
                ouLevel.Unid = " ";
                ouLevel.Name = curLevelName + "[已被删除的级别]";
                ouLevel.Code = curLevelCode;
                list.Add(ouLevel);
            }

            toBind.DataSource = list;
            toBind.DataTextField = "Name";
            toBind.DataValueField = "Code";
            toBind.DataBind();
        }
    }
}
