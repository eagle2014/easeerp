using System;
using System.Collections.Generic;
using System.Text;
using TSCommon.Core;

namespace EaseErp.IC.Domain
{
    public class WareHouse : FileEntry
    {
        public static string RELATIONSHIP_CODE = "wareHouse";

        private string name;
        private string code;
        private string memo;
        private int status;
        private Place place;

        /// <summary>
        /// 新建
        /// </summary>
        public static int STATUS_NEW = 0;

        /// <summary>
        /// 正常
        /// </summary>
        public static int STATUS_NORMAL = 1;

        /// <summary>
        /// 作废
        /// </summary>
        public static int STATUS_INVALID = -1;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        ///  代码
        /// </summary>
        public string Code
        {
            get { return this.code; }
            set { this.code = value; }
        }
        /// <summary>
        ///  备注
        /// </summary>
        public string Memo
        {
            get { return this.memo; }
            set { this.memo = value; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status
        {
            get { return this.status; }
            set { this.status = value; }
        }
        /// <summary>
        /// 所在地
        /// </summary>
        public Place Place
        {
            get { return place; }
            set { place = value; }
        }
        /// <summary>
        /// 状态的描述
        /// </summary>
        public string StatusDesc
        {
            get 
            {
                if (this.status == STATUS_NEW)
                    return "新建";
                else if (this.status == STATUS_INVALID)
                    return "作废";
                else if (this.status == STATUS_NORMAL)
                    return "正常";
                else
                    return "未知";
            }
        }
    }
}
