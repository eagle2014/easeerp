using System;
using System.Collections.Generic;
using System.Text;
using Common.Logging;
using TSCommon_Core.ATM.Service;
using System.ComponentModel;
using System.Web.UI;
using TSLib;
using System.Collections;

namespace TSCommon_Core.Controls
{
    /// <summary>
    /// 附件控件
    /// </summary>
    public class ATM : TSLibWeb.WEBControl.UI.Control
    {
        private static ILog logger = LogManager.GetLogger(typeof(ATM));

        private IATMService atmService;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ATM()
        {
            this.atmService = (IATMService)GetObject("ATMService");

            // 默认配置
            this.style = "width:100%;height:100;";
        }

        #region 属性配置

        private string subject;
        /// <summary>
        /// 附件标题
        /// </summary>
        [
        Bindable(true),
        Category("附件控件"),
        DefaultValue(""),
        Description("附件标题"),
        Localizable(true)
        ]
        public string Subject
        {
            get
            {
                return (subject == null) ? String.Empty : subject;
            }
            set
            {
                subject = value;
            }
        }

        private string type;
        /// <summary>
        /// 附件类型
        /// </summary>
        [
        Bindable(true),
        Category("附件控件"),
        DefaultValue(""),
        Description("附件类型"),
        Localizable(true)
        ]
        public string Type
        {
            get
            {
                return (type == null) ? String.Empty : type;
            }
            set
            {
                type = value;
            }
        }

        private string punid = null;
        /// <summary>
        /// 附件所属文档的Unid
        /// </summary>
        [
        Bindable(true),
        Category("附件控件"),
        DefaultValue(""),
        Description("附件所属文档Unid"),
        Localizable(true)
        ]
        public string PUnid
        {
            get
            {
                return punid;
            }
            set
            {
                punid = value;
            }
        }

        private bool readOnly = false;
        /// <summary>
        /// 附件所属文档的Unid
        /// </summary>
        [
        Bindable(true),
        Category("附件控件"),
        DefaultValue(""),
        Description("是否允许编辑附件"),
        Localizable(true)
        ]
        public bool ReadOnly
        {
            get
            {
                return readOnly;
            }
            set
            {
                readOnly = value;
            }
        }

        private string style;
        /// <summary>
        /// 总样式
        /// </summary>
        [
        Bindable(true),
        Category("附件控件"),
        DefaultValue(""),
        Description("总样式"),
        Localizable(true)
        ]
        public string Style
        {
            get
            {
                return style;
            }
            set
            {
                style = value;
            }
        }

        #endregion

        /// <summary>
        /// 呈现控件的内容
        /// </summary>
        /// <param name="writer">要在客户端呈现 HTML 内容的输出流</param>
        protected override void Render(HtmlTextWriter writer)
        {
            // 获取附件列表
            IList atms = this.atmService.GetATM(this.PUnid, this.Type);

            // 构建控件的html
            StringBuilder html = new StringBuilder(SimpleResourceHelper.GetString("ATM.WEB_CONTROL.TPL"));
            StringBuilder temp = new StringBuilder();

            // 构建工具条：工具条的标题文字
            if (!string.IsNullOrEmpty(this.Subject))
            {
                temp.Append(SimpleResourceHelper.GetString("ATM.WEB_CONTROL.TPL.TB.SUBJECT"));
                temp.Replace("{SUBJECT}", this.Subject);
            }

            // 构建工具条：工具条按钮
            if (this.readOnly)
            {
                temp.Append(SimpleResourceHelper.GetString("ATM.WEB_CONTROL.TPL.TB.BUTTON.DOWNLOAD"));   // 下载
            }
            else
            {
                temp.Append(SimpleResourceHelper.GetString("ATM.WEB_CONTROL.TPL.TB.BUTTON.UPLOAD"));     // 上传
                temp.Append(SimpleResourceHelper.GetString("ATM.WEB_CONTROL.TPL.TB.BUTTON.DOWNLOAD"));   // 下载
                temp.Append(SimpleResourceHelper.GetString("ATM.WEB_CONTROL.TPL.TB.BUTTON.DELETE"));     // 删除
            }
            html.Replace("{TB}", temp.ToString());


            // 构建工具条：工具条的标题文字
            temp.Remove(0, temp.Length);
            string dataTRTpl = SimpleResourceHelper.GetString("ATM.WEB_CONTROL.TPL.DATA_TR");
            int i = 1;
            bool isJiShuRow;
            StringBuilder temp1 = new StringBuilder();
            foreach (TSCommon_Core.ATM.Domain.ATM atm in atms)
            {
                isJiShuRow = (i % 2 == 0);
                temp1.Remove(0, temp1.Length);
                temp1.Append(dataTRTpl);
                temp1.Replace("{N}", isJiShuRow ? string.Empty : "1");
                temp1.Replace("{UNID}", atm.Unid);
                temp1.Replace("{DATA_SUBJECT}", atm.Subject);
                temp1.Replace("{DATA_FILESIZE}", atm.FileSize);
                temp1.Replace("{DATA_FILEDATE}", atm.FileDate.ToString("yyyy-MM-dd"));
                temp1.Replace("{DATA_AUTHOR}", atm.Author.Name != null ? atm.Author.Name : string.Empty);

                temp.Append(temp1);
                i++;
            }
            html.Replace("{DATA_TRS}", temp.ToString());

            // 全局替换
            html.Replace("{MAIN_ID}", this.ID);
            html.Replace("{TYPE}", this.Type);
            html.Replace("{PUNID}", this.PUnid);
            html.Replace("{STYLE}", this.Style);
            html.Replace("{EMPTY}", @"&nbsp;");

            // 输出控件内容
            writer.Write(html.ToString());

            if (logger.IsDebugEnabled)
                logger.Debug("html=" + html.ToString());
        }
    }
}
