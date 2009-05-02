using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TSLib.Utils;
using System.Collections;
using TSLib;
using TSCommon.Core.Organize.Domain;

namespace TSCommon.Core.SystemPolicy.Domain
{
    public class Policy : FileEntry
    {
        // ϵͳ���Ի���FQN�Ķ���
        public static string CACHE_FQN = "policies";
        public static string PVT_INPUT = "input";
        public static string PVT_SELECT = "select";

        #region �ֶζ���

        private string subject;             // ��������
        private string code;                // ���Եı��룬Ψһ���룬ϵͳ���ݸñ������ʹ��
        private string type;                // ����ֵѡ������ͣ�input/select
        private string value;               // ���Ե�ֵ
        private string optionNames;         // ���ԵĿ�ѡѡ��������б�
        private string optionValues;        // ���ԵĿ�ѡѡ���ֵ�б�
        private string valueType;           // ����ֵ�����ͣ�int/string/list

        #endregion

        #region ���Զ���

        private string belongModule;
        /// <summary>
        /// ����ģ��,��ϵͳ�����¼�����
        /// </summary>
        public string BelongModule
        {
            get { return belongModule; }
            set { belongModule = value; }
        }

        private string orderNo;
        /// <summary>
        /// �������
        /// </summary>
        public string OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
        public string ValueDesc
        {
            get
            {
                if (string.IsNullOrEmpty(this.optionNames))
                    return this.value;
                else
                {
                    string[] values = Regex.Split(this.optionValues, Constants.VALUE_SEPARATOR);
                    string[] names = Regex.Split(this.optionNames, Constants.VALUE_SEPARATOR);
                    if (null == values || null == names || values.Length != names.Length)
                        return this.value;
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (values[i].Equals(this.value, StringComparison.OrdinalIgnoreCase))
                            return names[i];
                    }
                    return this.value;
                }
            }
        }
        public string OptionNames
        {
            get { return optionNames; }
            set { optionNames = value; }
        }
        public string OptionValues
        {
            get { return optionValues; }
            set { optionValues = value; }
        }
        public string ValueType
        {
            get { return valueType; }
            set { valueType = value; }
        }

        #endregion

        /// <summary>
        /// Ĭ�Ϲ��캯��
        /// </summary>        
        public Policy()
        {
        }

        /// <summary>
        /// ����ָ���û��½�һ��ϵͳ����
        /// </summary>
        /// <param name="userInfo">�û���Ϣ</param>
        public Policy(User user)
            : base(user)
        {
        }

        public IList GetSelectOptions()
        {
            IList optionLists = new ArrayList();
            if (string.IsNullOrEmpty(this.OptionValues))
                return optionLists;
            else
            {
                string[] values = Regex.Split(this.OptionValues, Constants.VALUE_SEPARATOR);
                string[] names = Regex.Split(this.optionNames, Constants.VALUE_SEPARATOR);
                if (null == values || null == names || values.Length != names.Length)
                    return optionLists;
                for (int i = 0; i < values.Length; i++)
                {
                    optionLists.Add(new HtmlOption(names[i], values[i]));
                }
                return optionLists;
            }
        }
    }
}
