/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSCommon.Core.Organize.Domain;
using TSLib.Service;

namespace TSCommon.Core.Organize.RelationShips
{
    /// <summary>
    /// 关联关系Service接口的定义
    /// <author>CD826</author>
    /// </summary>
    public interface IRelationShipService : IBaseService<RelationShip>
    {
        IList GetRelationItems();
        /// <summary>
        /// 根据父文档的UNID更新所有的关联关系
        /// </summary>
        /// <param name="parentUnid"></param>
        /// <param name="relationShipType"></param>
        /// <param name="relationShips"></param>
        void UpdateRelationByParent(string parentUnid, string relationShipType, IList relationShips);

        /// <summary>
        /// 根据父文档的UNID更新所有的关联关系
        /// </summary>
        /// <param name="childUnid"></param>
        /// <param name="relationShipType"></param>
        /// <param name="relationShips"></param>
        void UpdateRelationByChild(string childUnid, string relationShipType, IList relationShips);

        /// <summary>
        /// 根据父文档的UNID更新所有的关联关系
        /// </summary>
        /// <param name="parentUnid"></param>
        /// <param name="relationShips"></param>
        /// <param name="type"></param>
        void UpdateRelation(string parentUnid, IList relationShips, string type);       
        
        /// <summary>
        /// 获取一个关联关系
        /// </summary>
        /// <param name="parentUnid">关联关系父文档的UNID</param>
        /// <param name="childUnid">关联关系子文档的UNID</param>
        /// <returns>如果存在则返回相应的关联关系，否则返回一个NULL</returns>
        RelationShip Get(string parentUnid, string childUnid);

        /// <summary>
        /// 获取关联关系
        /// </summary>
        /// <param name="childUnid">子文档的UNID</param>
        /// <param name="type">关联关系的类型</param>
        /// <returns>如果存在则返回相应的关联关系，否则返回一个NULL</returns>
        IList GetByChild(string childUnid, string type);

        /// <summary>
        /// 获取关联关系
        /// </summary>
        /// <param name="parentUnid">父文档的UNID</param>
        /// <param name="type">关联关系的类型</param>
        /// <returns>如果存在则返回相应的关联关系，否则返回一个NULL</returns>
        IList GetByParent(string parentUnid, string type);

        /// <summary>
        /// 根据给定的父文档和子文档的UNID，删除他们直接的关系
        /// </summary>
        /// <param name="parentUnid">父文档的UNID</param>
        /// <param name="childUnid">子文档的UNID</param>
        void Delete(string parentUnid, string childUnid);

        /// <summary>
        /// 根据给定的父文档和子文档的UNID，删除他们直接的关系
        /// </summary>
        /// <param name="parentUnid">父文档的UNID</param>
        /// <param name="childUnids">子文档的UNID集</param>
        int Delete(string parentUnid, string[] childUnids);
        
        /// <summary>
        /// 删除指定关联到父文档的所有关联关系
        /// </summary>
        /// <param name="parentUnid">父文档的UNID</param>
        void DeleteAllByParent(string parentUnid);

        /// <summary>
        /// 删除指定关联到父文档和类型的所有关联关系
        /// </summary>
        /// <param name="parentUnid">父文档的UNID</param>
        /// <param name="type">关联关系的类型</param>
        void DeleteAllByParent(string parentUnid, string type);

        /// <summary>
        /// 删除和指定子文档的所有关联关系
        /// </summary>
        /// <param name="childUnid">子文档的UNID</param>
        void DeleteAllByChild(string childUnid);

        /// <summary>
        /// 删除和指定子文档、关联关系的所有关联关系
        /// </summary>
        /// <param name="childUnid">子文档的UNID</param>
        /// <param name="type">关联关系的类型</param>
        void DeleteAllByChild(string childUnid, string type);

        /// <summary>
        /// 删除和指定文档的所有关联关系
        /// </summary>
        /// <param name="unid">文档的UNID</param>
        void DeleteAll(string unid);

        /// <summary>
        /// 删除和指定文档、关联关系的所有关联关系
        /// </summary>
        /// <param name="unid">文档的UNID</param>
        /// <param name="type">关联关系的类型</param>
        void DeleteAll(string unid, string type);

        /// <summary>
        /// 根据父文档和子文档信息构建关联关系
        /// </summary>
        /// <param name="parentUnid">父文档unid</param>
        /// <param name="parentType">父文档关联代码</param>
        /// <param name="childUnids">子文档unid集</param>
        /// <param name="childType">子文档关联代码</param>
        /// <returns>成功构建的关联关系的数量</returns>
        int Build(string parentUnid, string parentType, string[] childUnids, string childType);
    }
}
