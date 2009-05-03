using System;
using System.Collections.Generic;
using System.Text;
using Lib;

namespace TSCommon.Core.Organize.Domain
{
    /// <summary>
    /// 单位级别的Domain
    /// <author>CD826</author>
    /// </summary>
    public class OULevel : Entry
    {
        #region 字段定义
        private string name;                // 单位级别的名称
        private string code;                  // 组织的级别

        #endregion

        #region 属性定义

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Code
        {
            get { return this.code; }
            set { this.code = value; }
        }

        #endregion
    } 
}
