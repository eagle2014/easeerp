using System;
using System.Collections.Generic;
using System.Text;

namespace TSCommon.Core.Organize.Domain
{
    public class RelationShipItem
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>        
        public RelationShipItem()
        {            
        }

        private string name;

        /// <summary>
        /// 关联名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string url;

        /// <summary>
        /// 关联页面的url
        /// </summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        private string type;
        /// <summary>
        /// 关联类型
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

    }
}
