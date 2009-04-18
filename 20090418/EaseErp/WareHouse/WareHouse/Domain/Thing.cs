using System;
using System.Collections.Generic;
using System.Text;
using Lib;

namespace EaseErp_WareHouse.WareHouse.Domain
{
    /// <summary>
    /// 物品实体
    /// </summary>
    public class Thing:Entry
    {
        private string name;
        private string code;
        private string memo;
        private WHUnit whUnit;

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

        public string Memo
        {
            get { return this.memo; }
            set { this.memo = value; }
        }

        public WHUnit WHUnit
        {
            get { return this.whUnit; }
            set { this.whUnit = value; }
        }
    }
}
