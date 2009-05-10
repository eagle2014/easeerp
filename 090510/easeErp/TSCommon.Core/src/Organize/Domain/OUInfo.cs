/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using Lib;
using TSLibStruts.Utils;

namespace TSCommon.Core.Organize.Domain
{
    /// <summary>
    /// 组织状态
    /// </summary>
    public enum OUStatuses
    {
        Undefined = -1, Enable = 0, Disable = 1, Add = 2, Modified = 4, Delete = 8
    }

    /// <summary>
    /// 用于组织状态的Struts转换器
    /// </summary>
    public class OUStatusConverter : IConverter
    {
        public object Convert(Type type, object value)
        {
            if (value != null && !value.Equals(""))
                return Enum.Parse(typeof(OUStatuses), value.ToString());
            else
                return OUStatuses.Undefined;
        }
    }

    /// <summary>
    /// 单位、部门等信息的Domain
    /// <author>CD826</author>
    /// </summary>
    public class OUInfo : Entry
    {
        /// <summary>OU类型：单位</summary>
        public const string OT_UNIT         = "DW";
        /// <summary>OU类型：部门</summary>
        public const string OT_DEPARTMENT = "BM";

        #region 字段定义

        private int ouStatus;               // 组织的状态，定义代码如下：0－启用    1－禁用     8－删除
                                            //                           2－添加    4－修改     
        private string parentOUUnid;        // 上一级部门或单位的UNID
        private string name;                // 组织的名称
        private string code;                // 组织的标示
        private string description;         // 对于组织的简单描述
        private string type;                // 组织结构的类型：DW-单位  BM-部门
        private string level;               // 组织的级别
        private string levelName;           // 组织级别的名称
        private string fullName;            // 组织的全称，格式为：广州市忆科计算机系统有限公司.软件应用部
        private string fullCode;            // 组织的全标示，格式为：Egrand.SGA
        private string isTmpOU;             // 是否是临时组织，Y－临时  N－非临时
        private string orderNo;                // 组织的排序序号
        private string oldImportDataID;     //记录以前数据ID
        private string unitUnid;            // 所属单位的UNID
        private string unitName;            // 所属单位的名称
        private string unitFullName;        // 所属单位的全称
        private string unitFullCode;        // 所属单位的全编码
        private string field_1;             // 组织的扩展信息1
        private string field_2;             // 组织的扩展信息2
        private string field_3;             // 组织的扩展信息3
        private string field_4;             // 组织的扩展信息4
        private string field_5;             // 组织的扩展信息5
        #endregion

        #region 属性定义

        public OUStatuses OUStatus
        {
            get { return (OUStatuses)this.ouStatus; }
            set { this.ouStatus = (int)value; }
        }

        public string ParentOUUnid
        {
            get { return this.parentOUUnid; }
            set { this.parentOUUnid = value; }
        }

        public string ParentOUName
        {
            get
            {
                if (null == this.FullName)
                    return "";
                int pos = this.FullName.LastIndexOf(".");
                if (pos >= 0)
                    return this.FullName.Substring(0, pos);
                else
                    return "";
            }
            set { }
        }

        public string UnitUnid
        {
            get { return this.unitUnid; }
            set { this.unitUnid = value; }
        }

        public string UnitName
        {
            get { return this.unitName; }
            set { this.unitName = value; }
        }

        public string UnitFullName
        {
            get { return this.unitFullName; }
            set { this.unitFullName = value; }
        }

        public string UnitFullCode
        {
            get { return this.unitFullCode; }
            set { this.unitFullCode = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        public string Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public string TypeName
        {
            get 
            { 
                if(OT_UNIT.Equals(this.type, StringComparison.OrdinalIgnoreCase))
                    return "单位信息";
                else
                    return "部门信息";
            }
        }

        public string Level
        {
            get { return this.level; }
            set { this.level = value; }
        }

        public string LevelName
        {
            get { return this.levelName; }
            set { this.levelName = value; }
        }

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public string FullCode
        {
            get { return fullCode; }
            set { fullCode = value; }
        }

        public string IsTmpOU
        {
            get { return isTmpOU; }
            set { isTmpOU = value; }
        }

        public string OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }

        public string OldImportDataID
        {
            get { return oldImportDataID; }
            set { oldImportDataID = value; }
        }

        public string Field_1
        {
            get { return field_1; }
            set { field_1 = value; }
        }

        public string Field_2
        {
            get { return field_2; }
            set { field_2 = value; }
        }

        public string Field_3
        {
            get { return field_3; }
            set { field_3 = value; }
        }

        public string Field_4
        {
            get { return field_4; }
            set { field_4 = value; }
        }
        public string Field_5
        {
            get { return field_5; }
            set { field_5 = value; }
        }
        #endregion

        public override string ToString()
        {
            return this.Name;
        }
    }  
       
}
