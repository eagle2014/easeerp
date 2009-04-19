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
    /// �����ؼ�
    /// </summary>
    public class ATM : TSLibWeb.WEBControl.UI.Control
    {
        private static ILog logger = LogManager.GetLogger(typeof(ATM));

        private IATMService atmService;
        /// <summary>
        /// ���캯��
        /// </summary>
        public ATM()
        {
            this.atmService = (IATMService)GetObject("ATMService");

            // Ĭ������
            this.style = "width:100%;height:100;";
        }

        #region ��������

        private string subject;
        /// <summary>
        /// ��������
        /// </summary>
        [
        Bindable(true),
        Category("�����ؼ�"),
        DefaultValue(""),
        Description("��������"),
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
        /// ��������
        /// </summary>
        [
        Bindable(true),
        Category("�����ؼ�"),
        DefaultValue(""),
        Description("��������"),
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
        /// ���������ĵ���Unid
        /// </summary>
        [
        Bindable(true),
        Category("�����ؼ�"),
        DefaultValue(""),
        Description("���������ĵ�Unid"),
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
        /// ���������ĵ���Unid
        /// </summary>
        [
        Bindable(true),
        Category("�����ؼ�"),
        DefaultValue(""),
        Description("�Ƿ�����༭����"),
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
        /// ����ʽ
        /// </summary>
        [
        Bindable(true),
        Category("�����ؼ�"),
        DefaultValue(""),
        Description("����ʽ"),
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
        /// ���ֿؼ�������
        /// </summary>
        /// <param name="writer">Ҫ�ڿͻ��˳��� HTML ���ݵ������</param>
        protected override void Render(HtmlTextWriter writer)
        {
            // ��ȡ�����б�
            IList atms = this.atmService.GetATM(this.PUnid, this.Type);

            // �����ؼ���html
            StringBuilder html = new StringBuilder(SimpleResourceHelper.GetString("ATM.WEB_CONTROL.TPL"));
            StringBuilder temp = new StringBuilder();

            // �������������������ı�������
            if (!string.IsNullOrEmpty(this.Subject))
            {
                temp.Append(SimpleResourceHelper.GetString("ATM.WEB_CONTROL.TPL.TB.SUBJECT"));
                temp.Replace("{SUBJECT}", this.Subject);
            }

            // ��������������������ť
            if (this.readOnly)
            {
                temp.Append(SimpleResourceHelper.GetString("ATM.WEB_CONTROL.TPL.TB.BUTTON.DOWNLOAD"));   // ����
            }
            else
            {
                temp.Append(SimpleResourceHelper.GetString("ATM.WEB_CONTROL.TPL.TB.BUTTON.UPLOAD"));     // �ϴ�
                temp.Append(SimpleResourceHelper.GetString("ATM.WEB_CONTROL.TPL.TB.BUTTON.DOWNLOAD"));   // ����
                temp.Append(SimpleResourceHelper.GetString("ATM.WEB_CONTROL.TPL.TB.BUTTON.DELETE"));     // ɾ��
            }
            html.Replace("{TB}", temp.ToString());


            // �������������������ı�������
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

            // ȫ���滻
            html.Replace("{MAIN_ID}", this.ID);
            html.Replace("{TYPE}", this.Type);
            html.Replace("{PUNID}", this.PUnid);
            html.Replace("{STYLE}", this.Style);
            html.Replace("{EMPTY}", @"&nbsp;");

            // ����ؼ�����
            writer.Write(html.ToString());

            if (logger.IsDebugEnabled)
                logger.Debug("html=" + html.ToString());
        }
    }
}
