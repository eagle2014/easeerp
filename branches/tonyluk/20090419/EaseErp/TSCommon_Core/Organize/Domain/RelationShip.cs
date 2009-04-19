/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 * 
 * Autor: CD826         2007-02-17
 */
using System;
using System.Collections.Generic;
using System.Text;
using Lib;

namespace TSCommon.Core.Organize.Domain
{
    /// <summary>
    /// 关联关系的记录的Domain
    /// </summary>
    public class RelationShip : Entry
    {
        /// <summary>
        /// 分隔符
        /// </summary>
        public const string RS_SPLITFLAG = ".";

        #region 字段定义
        
        private string parentUnid;              // 父文档的UNID
        private string parentType;              // 父文档的类型
        private string childUnid;               // 子文档的UNID
        private string childType;               // 子文档的类型
        private string relationShipType;        // 关联关系描述，一般为parentType.childType

        #endregion

        #region 属性定义
        
        /// <summary>
        /// 父文档的UNID
        /// </summary>
        public string ParentUnid
        {
            get { return parentUnid; }
            set { parentUnid = value; }
        }
        /// <summary>
        /// 父文档的类型
        /// </summary>
        public string ParentType
        {
            get { return parentType; }
            set { parentType = value; }
        }
        /// <summary>
        /// 子文档的UNID
        /// </summary>
        public string ChildUnid
        {
            get { return childUnid; }
            set { childUnid = value; }
        }
        /// <summary>
        /// 子文档的类型
        /// </summary>
        public string ChildType
        {
            get { return childType; }
            set { childType = value; }
        }
        /// <summary>
        /// 关联关系描述，一般为parentType.childType
        /// </summary>
        public string RelationShipType
        {
            get { return relationShipType; }
            set { relationShipType = value; }
        }

        #endregion

        /// <summary>
        /// 默认构造函数
        /// </summary>        
        public RelationShip()
        {            
        }

        /// <summary>
        /// 根据指定信息创建一个关联关系的记录
        /// </summary>
        /// <param name="parentUnid">父文档的UNID</param>
        /// <param name="parentType">父文档的类型</param>
        /// <param name="childUnid">子文档的UNID</param>
        /// <param name="childType">子文档的类型</param>
        public RelationShip(string parentUnid, string parentType, string childUnid,
                            string childType)
        {
            this.parentType = parentType;
            this.parentUnid = parentUnid;
            this.childType = childType;
            this.childUnid = childUnid;
            this.relationShipType = BuildRelationShipCode(parentType, childType);
        }

        /// <summary>
        /// 根据指定信息创建一个关联关系的记录
        /// </summary>
        /// <param name="parentUnid">父文档的UNID</param>
        /// <param name="parentType">父文档的类型</param>
        /// <param name="childUnid">子文档的UNID</param>
        /// <param name="childType">子文档的类型</param>
        /// <param name="relationShipType">关联关系描述，一般为parentType.childType</param>
        public RelationShip(string parentUnid, string parentType, string childUnid,
                            string childType, string relationShipType)
        {
            this.parentType = parentType;
            this.parentUnid = parentUnid;
            this.childType = childType;
            this.childUnid = childUnid;
            this.relationShipType = relationShipType;
        }

        /// <summary>
        /// 构造关联关系
        /// </summary>
        /// <param name="parentType">父文档的类型</param>
        /// <param name="childType">子文档的类型</param>
        /// <returns>关联关系</returns>
        public static string BuildRelationShipCode(string parentType, string childType)
        {
            return parentType + RS_SPLITFLAG + childType;
        }

        /// <summary>
        /// 复制关联关系信息
        /// </summary>
        /// <param name="relationShip">要复制的关联关系</param>
        public void Copy(RelationShip relationShip)
        {
            this.parentUnid = relationShip.ParentUnid;
            this.parentType = relationShip.ParentType;
            this.childUnid = relationShip.ChildUnid;
            this.childType = relationShip.ChildType;
            this.relationShipType = relationShip.RelationShipType;
        }

        /// <summary>
        /// 更新关联关系信息
        /// </summary>
        /// <param name="parentUnid"></param>
        /// <param name="parentType"></param>
        /// <param name="childUnid"></param>
        /// <param name="childType"></param>
        public void Update(string parentUnid, string parentType, string childUnid, string childType)
        {
            this.parentUnid = parentUnid;
            this.parentType = parentType;
            this.childUnid = childUnid;
            this.childType = childType;
            this.relationShipType = BuildRelationShipCode(parentType, childType);
        }
    }
}
