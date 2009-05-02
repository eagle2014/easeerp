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
        // 系统策略缓存FQN的定义
        public static string CACHE_FQN = "policies";
        public static string PVT_INPUT = "input";
        public static string PVT_SELECT = "select";

        #region 字段定义

        private string subject;             // 策略名称
        private string code;                // 策略的编码，唯一编码，系统根据该编码进行使用
        private string type;                // 策略值选择的类型：input/select
        private string value;               // 策略的值
        private string optionNames;         // 策略的可选选项的名称列表
        private string optionValues;        // 策略的可选选项的值列表
        private string valueType;           // 策略值的类型：int/string/list

        #endregion

        #region 属性定义

        private string belongModule;
        /// <summary>
        /// 所属模块,如系统管理、事件管理
        /// </summary>
        public string BelongModule
        {
            get { return belongModule; }
            set { belongModule = value; }
        }

        private string orderNo;
        /// <summary>
        /// 排序序号
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
        /// 默认构造函数
        /// </summary>        
        public Policy()
        {
        }

        /// <summary>
        /// 根据指定用户新建一个系统策略
        /// </summary>
        /// <param name="userInfo">用户信息</param>
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
