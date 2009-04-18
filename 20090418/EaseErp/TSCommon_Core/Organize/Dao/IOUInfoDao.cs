/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 * 
 * Autor: CD826         2007-02-15
 */
using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Dao;
using System.Collections;
using TSLib;
using TSCommon_Core.Organize.Domain;

namespace TSCommon_Core.Organize.Dao
{
    /// <summary>
    /// 单位组织信息的DAO接口定义
    /// </summary>
    public interface IOUInfoDao : IBaseDao<OUInfo>
    {
        /// <summary>
        /// 获取全部组织架构的配置信息
        /// </summary>
        /// <returns>所有配置的组织架构信息</returns>
        new IList FindAll();

        /// <summary>
        /// 获取指定类型的组织结构配置信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList FindAllByType(string type);

        /// <summary>
        /// 获取指定OU下指定类型的所有子OU的配置信息
        /// </summary>
        /// <param name="parentUnid">上级OU的Unid</param>
        /// <param name="ouType">OU的类型，为null表示查询所有类型</param>
        /// <returns>指定OU下指定类型的所有子OU的配置信息</returns>
        IList FindChildByParentUnid(string parentUnid, string ouType);

        /// <summary>
        /// 获取指定OU下指定类型的所有子OU的配置信息
        /// </summary>
        /// <param name="parentUnid">上级OU的Unid</param>
        /// <param name="ouType">OU的类型，为null表示查询所有类型</param>
        /// <param name="includeChildOU">是否包含下级OU（包含下级的下级等）的信息</param>
        /// <returns>指定OU下指定类型的所有子OU的配置信息</returns>
        IList FindChildByParentUnid(string parentUnid, string ouType,bool includeChildOU);

        /// <summary>
        /// 获取指定OU下指定类型的所有子OU的配置信息
        /// </summary>
        /// <param name="parentCode">上级OU的编码(编码全称)</param>
        /// <param name="ouType">OU的类型</param>
        /// <returns>指定OU下指定类型的所有子OU的配置信息</returns>
        IList FindChildByParentCode(string parentCode, string ouType);

        /// <summary>
        /// 获取指定级别的组织架构配置信息
        /// </summary>
        /// <param name="level">组织架构级别编码</param>
        /// <param name="hasChild">是否包含子集</param>
        /// <returns>符合条件的组织架构集合列表</returns>
        IList FindByLevel(int level, bool hasChild);

        /// <summary>
        /// 取得指定单位下所有部门的信息
        /// </summary>
        /// <param name="unitUnid">单位的UNID</param>
        /// <returns>返回符合条件的集合列表</returns>
        IList FindAllDepartment(string unitUnid);

        /// <summary>
        /// 根据组织架构的代码加载相应的组织架构信息
        /// </summary>
        /// <param name="fullCode">组织架构的代码</param>
        /// <returns>相应组织架构的配置信息，如果没有则返回NULL</returns>
        OUInfo LoadByFullCode(string fullCode);

        /// <summary>
        /// 判断对象在数据库中是否唯一
        /// </summary>
        /// <param name="ouInfo">所要判断的对象</param>
        /// <returns>在数据库中是唯一的就返回true,否则返回false</returns>
        bool IsUnique(OUInfo ouInfo);

        /// <summary>
        /// 获取指定类型组织架构的页面信息
        /// </summary>
        /// <param name="type">组织架构的类型</param>
        /// <param name="pageNo">页码,起始页码从1开始</param>
        /// <param name="pageSize">每页的记录数</param>
        /// <param name="sortField">要排序的属性名</param>
        /// <param name="sortDir">排序方向</param>
        /// <param name="ouStatuses">状态列表，为空代表所有状态</param>
        /// <returns>符合条件的页面信息</returns>
        PageInfo GetPageByType(string type, int pageNo, int pageSize, string sortField, string sortDir, object[] ouStatuses);

        /// <summary>
        /// 获取指定类型组织架构的页面信息
        /// </summary>
        /// <param name="unitUnid">部门所属单位的Unid</param>
        /// <param name="pageNo">页码,起始页码从1开始</param>
        /// <param name="pageSize">每页的记录数</param>
        /// <param name="sortField">要排序的属性名</param>
        /// <param name="sortDir">排序方向</param>
        /// <param name="ouStatuses">状态列表，为空代表所有状态</param>
        /// <returns>符合条件的页面信息</returns>
        PageInfo GetDepartmentPage(string unitUnid, int pageNo, int pageSize, string sortField, string sortDir, object[] ouStatuses);

        /// <summary>
        /// 查找数据库中原来导入的数据
        /// </summary>
        /// <param name="oldImportDataID"></param>
        /// <returns></returns>
        OUInfo FindOUInfoByOldImportDataID(string oldImportDataID);

        /// <summary>
        /// 获得所有的原来DeparmentId,用逗号隔开
        /// </summary>
        /// <returns></returns>
        string GetAllOldImportDataID();

        /// <summary>
        /// 获取指定单位中所包含的所有部门信息
        /// </summary>
        /// <param name="parentUnid">所属单位的Unid</param>
        /// <param name="firstLevel">是否是第一层的部门信息</param>
        /// <returns>如果存在子那么返回，否则返回空值</returns>
        IList FindDepartmentByUnit(string parentUnid, bool firstLevel);

        /// <summary>
        /// 用于数据同步，搜索出所有组织架构
        /// </summary>
        /// <returns></returns>
        IList FindAll_OrgSync();
    }
}
