using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSLib;

namespace TSCommon.Core.OptionItemCfg.Domain
{
    public class OptionItem:FileEntry
    {
        #region 服务目录的常量的定义

        /// <summary>
        /// 业务对象配置在缓存中的全局名称
        /// </summary>
        /// <remarks>
        /// 值为：OPTION_ITEM_CFG
        /// </remarks>
        public const string CACHE_FQN = "OPTION_ITEM_CFG";

        #endregion

        #region 字段定义

        private string type;            // 配置所属分类        
        private string typeName;        // 配置所属分类的名称
        private string name;            // 配置的名称
        private string code;            // 配置的代码
        private string orderNo;

        #endregion

        #region 属性定义

        /// <summary>
        /// 配置所属的分类
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// 配置所属分类的名称
        /// </summary>
        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        /// <summary>
        /// 配置的名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 配置的代码
        /// </summary>
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public string OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }

        #endregion

        #region 静态公共方法

        /// <summary>
        /// 插入“== 请选择 ==”的空白选项到指定列表的最前端
        /// </summary>
        /// <param name="optionItems">源选项列表</param>
        public static void InsertPleaseSelectOptionItemToFirst(IList optionItems)
        {
            string emptyLabel = SimpleResourceHelper.GetString("SELECT.EMPTY.LABEL");
            InsertOptionItemToFirst(optionItems, emptyLabel, "");
        }

        /// <summary>
        /// 插入“== 任意 ==”的空白选项到指定列表的最前端
        /// </summary>
        /// <param name="optionItems">源选项列表</param>
        public static void InsertAnyOptionItemToFirst(IList optionItems)
        {
            string emptyLabel = SimpleResourceHelper.GetString("SELECT.EMPTY.LABEL.ANY");
            InsertOptionItemToFirst(optionItems, emptyLabel, "");
        }

        /// <summary>
        /// 插入空白选项到指定列表的最前端
        /// </summary>
        /// <param name="optionItems">源选项列表</param>
        public static void InsertBlankOptionItemToFirst(IList optionItems)
        {
            InsertOptionItemToFirst(optionItems, "", "");
        }

        /// <summary>
        /// 插入指定选项到指定列表的最前端
        /// </summary>
        /// <param name="optionItems">源选项列表</param>
        /// <param name="emptyLabel">空白选项显示的文本</param>
        /// <param name="emptyValue">空白选项对应的值</param>
        public static void InsertOptionItemToFirst(IList optionItems, string emptyLabel, string emptyValue)
        {
            if (optionItems == null) return;
            OptionItem option = new OptionItem();
            option.Name = emptyLabel;
            option.Code = emptyValue;
            optionItems.Insert(0, option);
        }

        #endregion
    }
}
