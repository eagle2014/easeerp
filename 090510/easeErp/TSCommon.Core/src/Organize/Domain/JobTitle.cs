using System;
using System.Collections.Generic;
using System.Text;
using Lib;

namespace TSCommon.Core.Organize.Domain
{
    /// <summary>
    /// 职务Domain定义
    /// </summary>
    public class JobTitle : Entry
    {
        #region 字段定义

        private string name;		    // 职务名称
        private string code;		    // 职务编码
        private string level;		    // 职务级别
        private string levelName;

        #endregion

        #region 属性定义

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        public string Level
        {
            get { return level; }
            set { level = value; }
        }
        /// <summary>
        /// 级别名称
        /// </summary>
        public string LevelName
        {
            get { return levelName; }
            set { levelName = value; }
        }
        #endregion
    }
}
