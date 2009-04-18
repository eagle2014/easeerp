using System;
using System.Collections.Generic;
using System.Text;
using TSLibWeb;
using System.Collections;

namespace TSCommon_Core
{
    /// <summary>
    /// 为select提供选择项
    /// </summary>
    public class HtmlOption
    {
        private string optionName = "";
        private string optionValue = Constants.BLANK_STRING_VALUE;

        public string OptionName
        {
            get { return optionName; }
            set { optionName = value; }
        }
        public string OptionValue
        {
            get { return optionValue; }
            set { optionValue = value; }
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>        
        public HtmlOption()
        {
        }

        /// <summary>
        /// 根据参数中的信息构造一个HtmlOption
        /// </summary>
        /// <param name="name">option的name信息</param>
        /// <param name="value">option的value信息</param>
        public HtmlOption(string name, string value)
        {
            this.OptionName = name;
            this.OptionValue = value;
        }

        /// <summary>
        /// 产生一个长度为1的空白HtmlOption[] 
        /// </summary>
        /// <returns></returns>
        public static HtmlOption[] GetBlankHtmlOption()
        {
            HtmlOption[] options = new HtmlOption[1];
            options[0] = new HtmlOption("", "");
            return options;
        }

        /// <summary>
        /// 从字符串数组转换为HtmlOption数组 
        /// </summary>
        /// <param name="sourceValues">字符串数组</param>
        /// <returns>返回生成好的HtmlOption数组</returns>
        public static HtmlOption[] StringArray2HtmlOptions(string[] sourceValues)
        {
            return StringArray2HtmlOptions(sourceValues, false);
        }

        /// <summary>
        /// 从字符串数组转换为HtmlOption数组 
        /// </summary>
        /// <param name="sourceValues">字符串数组</param>
        /// <param name="isAddBlank">是否添加一个空白值</param>
        /// <returns>返回生成好的HtmlOption数组</returns>
        public static HtmlOption[] StringArray2HtmlOptions(string[] sourceValues, bool isAddBlank)
        {
            if (sourceValues == null || sourceValues.Length == 0)
                return GetBlankHtmlOption();

            HtmlOption[] htmlOptions = null;
            HtmlOption option = null;
            int index = 0;
            if (isAddBlank)
            {
                htmlOptions = new HtmlOption[sourceValues.Length + 1];
                htmlOptions[0] = new HtmlOption("", "");
                index++;
            }
            else
            {
                htmlOptions = new HtmlOption[sourceValues.Length];
            }

            String value = "";
            for (int i = 0; i < sourceValues.Length; i++)
            {
                value = sourceValues[i];
                option = new HtmlOption(value, value);
                htmlOptions[index] = option;
                index++;
            }
            return htmlOptions;
        }

        /// <summary>
        /// 将值列表转换成HtmlOption数组
        /// </summary>
        /// <param name="sourceList">值列表</param>
        /// <returns>返回转换好的HtmlOption数组，如果原来的列表为空，那么返回一个空白HtmlOption的数组</returns>
        public static HtmlOption[] List2HtmlOptions(IList sourceList)
        {
            return List2HtmlOptions(sourceList, "", false);
        }

        /// <summary>
        /// 将值列表转换成HtmlOption数组
        /// </summary>
        /// <param name="sourceList">值列表</param>
        /// <param name="isAddBlank">是否添加空白选项</param>
        /// <returns>返回转换好的HtmlOption数组，如果原来的列表为空，那么返回一个空白HtmlOption的数组</returns>
        public static HtmlOption[] List2HtmlOptions(IList sourceList, bool isAddBlank)
        {
            return List2HtmlOptions(sourceList, "", isAddBlank);
        }

        /// <summary>
        /// 将值列表转换成HtmlOption数组
        /// </summary>
        /// <param name="sourceList">值列表</param>
        /// <param name="preString">Option Value值的前缀</param>
        /// <returns>返回转换好的HtmlOption数组，如果原来的列表为空，那么返回一个空白HtmlOption的数组</returns>
        public static HtmlOption[] List2HtmlOptions(IList sourceList, string preString)
        {
            return List2HtmlOptions(sourceList, preString, false);
        }

        /// <summary>
        /// 将值列表转换成HtmlOption数组
        /// </summary>
        /// <param name="sourceList">值列表</param>
        /// <param name="preString">Option Value值的前缀</param>
        /// <param name="isAddBlank">是否添加空白选项</param>
        /// <returns>返回转换好的HtmlOption数组，如果原来的列表为空，那么返回一个空白HtmlOption的数组</returns>
        public static HtmlOption[] List2HtmlOptions(IList sourceList, string preString, bool isAddBlank)
        {
            if (null == sourceList || sourceList.Count == 0)
                return HtmlOption.GetBlankHtmlOption();

            HtmlOption[] options = new HtmlOption[sourceList.Count];
            for (int i = 0; i < sourceList.Count; i++)
            {
                string optionValue = (string)sourceList[i];
                options[i] = new HtmlOption(optionValue, preString + optionValue);
            }
            return options;
        }

        /// <summary>
        /// 将值列表转换成HtmlOption数组
        /// </summary>
        /// <param name="sourceList">值列表，每一个列表项为一个数组，第一个元素为OptionName的值，第二个元素为OptionValue的值</param>
        /// <returns>返回转换好的HtmlOption数组，如果原来的列表为空，那么返回一个空白HtmlOption的数组</returns>
        public static HtmlOption[] ArrayList2HtmlOptions(IList sourceList)
        {
            return ArrayList2HtmlOptions(sourceList, "", false);
        }

        /// <summary>
        /// 将值列表转换成HtmlOption数组
        /// </summary>
        /// <param name="sourceList">值列表，每一个列表项为一个数组，第一个元素为OptionName的值，第二个元素为OptionValue的值</param>
        /// <param name="isAddBlank">是否添加空白选项</param>
        /// <returns>返回转换好的HtmlOption数组，如果原来的列表为空，那么返回一个空白HtmlOption的数组</returns>
        public static HtmlOption[] ArrayList2HtmlOptions(IList sourceList, bool isAddBlank)
        {
            return ArrayList2HtmlOptions(sourceList, "", isAddBlank);
        }

        /// <summary>
        /// 将值列表转换成HtmlOption数组
        /// </summary>
        /// <param name="sourceList">值列表，每一个列表项为一个数组，第一个元素为OptionName的值，第二个元素为OptionValue的值</param>
        /// <param name="preString">Option Value值的前缀</param>
        /// <returns>返回转换好的HtmlOption数组，如果原来的列表为空，那么返回一个空白HtmlOption的数组</returns>
        public static HtmlOption[] ArrayList2HtmlOptions(IList sourceList, string preString)
        {
            return ArrayList2HtmlOptions(sourceList, preString, false);
        }

        /// <summary>
        /// 将值列表转换成HtmlOption数组
        /// </summary>
        /// <param name="sourceList">值列表，每一个列表项为一个数组，第一个元素为OptionName的值，第二个元素为OptionValue的值</param>
        /// <param name="preString">Option Value值的前缀</param>
        /// <param name="isAddBlank">是否添加空白选项</param>
        /// <returns>返回转换好的HtmlOption数组，如果原来的列表为空，那么返回一个空白HtmlOption的数组</returns>
        public static HtmlOption[] ArrayList2HtmlOptions(IList sourceList, string preString, bool isAddBlank)
        {
            if (null == sourceList || sourceList.Count == 0)
                return HtmlOption.GetBlankHtmlOption();

            HtmlOption[] options = new HtmlOption[sourceList.Count];
            for (int i = 0; i < sourceList.Count; i++)
            {
                string[] optionValue = (string[])sourceList[i];
                options[i] = new HtmlOption(optionValue[0], preString + optionValue[1]);
            }
            return options;
        }
    }
}
