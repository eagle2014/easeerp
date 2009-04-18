using System;
using System.Collections.Generic;
using System.Text;
using Lib;
using TSLib.Utils;

namespace TSCommon_Core.Security.Domain
{
    /// <summary>
    /// 权限定义
    /// </summary>
    public class Privilege : Entry
    {
        /// <summary>关联关系的定义，值为“privilege”</summary>
        public static string RELATIONSHIP_CODE = "privilege";

        /// <summary>功能按钮权限，值为“btn”</summary>
        public static string PRIVILEGETYPE_BUTTON   = "btn";

        /// <summary>功能模块权限，值为“url”</summary>
        public static string PRIVILEGETYPE_MODEL = "url";

        #region 字段定义
        
        private string name;                                        // 权限名称
        private string code;                                        // 权限代码
        private string orderNo;                                     // 权限序号
        private string type;                                        // 权限类别
        private string urlPath;                                     // url路径
        private Model model;                                        // 所属模块
        private string isInner;                                     // 是否是内建权限， Y－内建权限     N－非内建权限

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
        public string OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string TypeName
        {
            get 
            {
                if (PRIVILEGETYPE_MODEL.Equals(this.type, StringComparison.OrdinalIgnoreCase))
                    return "模块权限";
                else
                    return "按钮权限";
            }
        }
        public string UrlPath
        {
            get { return urlPath; }
            set { urlPath = value; }
        }
        public Model Model
        {
            get { return model; }
            set { model = value; }
        }
        public long ModelID
        {
            get
            {
                if (null == this.model)
                    return Constants.BLANK_LONG_VALUE;
                else
                    return this.model.ID;
            }
            set
            {
                if (value == Constants.BLANK_LONG_VALUE)
                    this.model = null;
                else
                {
                    this.model = new Model();
                    this.model.ID = value;
                }
            }
        }
        public string ModelName
        {
            get
            {
                if (null == this.model)
                    return Constants.BLANK_STRING_VALUE;
                else
                    return this.model.Name;
            }
        }
        public string IsInner
        {
            get { return isInner; }
            set { isInner = value; }
        }

        #endregion
    }
}
