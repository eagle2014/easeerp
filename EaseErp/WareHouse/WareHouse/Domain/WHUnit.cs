using System;
using System.Collections.Generic;
using System.Text;
using Lib;

namespace EaseErp_WareHouse.WareHouse.Domain
{
    /// <summary>
    /// 单位实体
    /// </summary>
    public class WHUnit:Entry
    {
        private string name;
        private string code;
        private string memo;

        public WHUnit(long id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        public WHUnit()
        { }
       
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
    }
}
