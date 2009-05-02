using System;
using System.Collections.Generic;
using System.Text;
using Lib;

namespace EaseErp.IC.Domain
{
    /// <summary>
    /// 地方实体
    /// </summary>
    public class Place : Entry
    {
        private string name;
        private string code;
        private string memo;
        private Place parent;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        /// <summary>
        /// 代码
        /// </summary>
        public string Code
        {
            get { return this.code; }
            set { this.code = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo
        {
            get { return this.memo; }
            set { this.memo = value; }
        }
        /// <summary>
        /// 父级别
        /// </summary>
        public Place Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }
    }
}
