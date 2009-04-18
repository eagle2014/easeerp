using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Utils;
using Lib;

namespace TSCommon_Core.Security.Domain
{
    /// <summary>
    /// 模块类型
    /// </summary>
    public enum ModelTypes { Undefined = -1, Master = 0, SubModel = 1 };

    /// <summary>
    /// 用于模块类型的Struts转换器
    /// </summary>
    public class ModelTypeConverter : TSLibStruts.Utils.IConverter
    {
        public object Convert(Type type, object value)
        {
            if (value != null && !value.Equals(""))
                return Enum.Parse(typeof(ModelTypes), value.ToString());
            else
                return ModelTypes.Undefined;
        }
    }

    /// <summary>
    /// 模块Domain定义
    /// </summary>
    public class Model : Entry
    {
        #region 字段定义

        private string name;                // 模块名称
        private string code;                // 模块代码
        private string orderNo;             // 模块的顺序号               
        private int type;                   // 模块的类型
        private string isInner;             // 是否是内建模块， Y－内建模块     N－非内建模块 
        private Model parent;               // 所属模块

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
        public ModelTypes Type
        {
            get { return (ModelTypes)type; }
            set { type = (int)((ModelTypes)value); }
        }
        public string IsInner
        {
            get { return isInner; }
            set { isInner = value; }
        }
        public Model Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        public long ParentID
        {
            get 
            {
                if (null == this.parent)
                    return Constants.BLANK_LONG_VALUE;
                else
                    return this.parent.ID;
            }
            set 
            {
                if (value == Constants.BLANK_LONG_VALUE)
                    this.parent = null;
                else
                {
                    this.parent = new Model();
                    this.parent.ID = value;
                }
            }
        }
        #endregion

        /// <summary>
        /// 默认构造函数
        /// </summary>        
        public Model()
        {            
        }
        
        /// <summary>
        /// 根据指定条件创建一个模块
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modelName"></param>
        public Model(long id, string modelName)
        {            
            this.ID = id;
            this.name = modelName;
        }

        /// <summary>
        /// 获取模块类型的名称
        /// </summary>
        /// <returns></returns>
        public string GetTypeName()
        {
            if (this.Type == ModelTypes.Master)
                return "主模块";
            else
                return "子模块";
        }

        /// <summary>
        /// 获取模块所属模块的名称
        /// </summary>
        /// <returns></returns>
        public string GetParentName()
        {
            if (null == this.Parent)
                return "";
            else
                return this.Parent.Name;
        }
    }
}
