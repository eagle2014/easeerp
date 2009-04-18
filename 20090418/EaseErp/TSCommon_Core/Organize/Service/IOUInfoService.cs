/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 * 
 * Autor: CD826         2007-02-26
 */
using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using TSLib;
using System.Collections;
using TSCommon_Core.Organize.Domain;

namespace TSCommon_Core.Organize.Service
{
    /// <summary>
    /// 组织架构Service的接口定义
    /// </summary>
    public interface IOUInfoService : IBaseService<OUInfo>
    {        
        /// <summary>
        /// 获取组织架构的配置信息
        /// </summary>
        /// <param name="userInfo">当前操作的人员</param>
        /// <param name="type">组织架构的类型</param>
        /// <param name="pageNo">页码,起始页码从1开始</param>
        /// <param name="pageSize">每页的记录数</param>
        /// <param name="sortField">要排序的属性名</param>
        /// <param name="sortDir">排序方向</param>
        /// <param name="ouStatuses">状态列表，为空代表所有状态</param>
        /// <returns>该页的信息</returns>
        PageInfo GetPageByType(User userInfo, string type, int pageNo, int pageSize, string sortField, string sortDir, object[] ouStatuses);

        /// <summary>
        /// 获取组织架构的配置信息
        /// </summary>        
        /// <param name="unitUnid">部门所属单位的Unid</param>
        /// <param name="pageNo">页码,起始页码从1开始</param>
        /// <param name="pageSize">每页的记录数</param>
        /// <param name="sortField">要排序的属性名</param>
        /// <param name="sortDir">排序方向</param>
        /// <param name="ouStatuses">状态列表，为空代表所有状态</param>
        /// <returns>该页的信息</returns>
        PageInfo GetDepartmentPage(string unitUnid, int pageNo, int pageSize, string sortField, string sortDir,object[] ouStatuses);

        /// <summary>
        /// 获取指定的单位结构信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="privilegeType"></param>
        /// <returns></returns>
        IList GetUnitTree(User userInfo, int privilegeType);

        /// <summary>
        /// 获取指定单位的树型结构列表
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="unitUnid">单位的Unid</param>
        /// <returns></returns>
        IList GetOUTree(User userInfo, string unitUnid);

        /// <summary>
        /// 获取组织架构的树型结构信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="privilegeType"></param>
        /// <returns></returns>
        IList GetOUTree(User userInfo, int privilegeType);

        /// <summary>
        /// 获取指定的组织架构信息列表
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IList FindAllByType(User userInfo, string type);

        /// <summary>
        /// 查询指定单位下所包含的所有子单位
        /// </summary>
        /// <param name="parentUnid">当前单位的Unid</param>
        /// <param name="includeSelf">返回值中是否包含本单位</param>
        /// <returns>符合条件的单位列表</returns>
        IList FindUnitByParentUnid(string parentUnid, bool includeSelf);

        /// <summary>
        /// 取得指定单位下所有部门的信息
        /// </summary>
        /// <param name="unitUnid">单位的UNID</param>
        /// <returns>返回符合条件的集合列表</returns>
        IList FindAllDepartment(string unitUnid);

        /// <summary>
        /// 取得指定OU中所包含的所有OU的信息
        /// </summary>
        /// <param name="parentUnid">该OU的Unid</param>
        /// <param name="hasDepartment">如果该OU是一个单位，则指示是否需要获取其下的部门</param>
        /// <param name="hasChildDepartment">如果该OU是一个部门，则指示是否需要获取其子部门</param>
        /// <returns></returns>
        IList FindOUInfoByParentUnid(string parentUnid, bool hasDepartment, bool hasChildDepartment);

        /// <summary>
        /// 根据组织架构的代码加载相应的组织架构信息
        /// </summary>
        /// <param name="fullCode">组织架构的代码</param>
        /// <returns>相应组织架构的配置信息，如果没有则返回NULL</returns>
        OUInfo LoadByFullCode(string fullCode);

        /// <summary>
        /// 查找数据库中原来导入的数据
        /// </summary>
        /// <param name="oldImportDataID"></param>
        /// <returns></returns>
        OUInfo FindOUInfoByOldImportDataID(string oldImportDataID);

        void ParseSave(User userInfo, OUInfo ouInfo);

        /// <summary>
        /// 获得所有的原来DeparmentId,用逗号隔开
        /// </summary>
        /// <returns></returns>
        string GetAllOldImportDataID();

        /// <summary>
        /// 根据原来的编号删除记录 
        /// </summary>
        /// <param name="oldImportDataID"></param>
        void DeleteByOldImportDataID(string oldImportDataID);

        IList FindAll();

        /// <summary>
        /// 获取指定OU下指定类型的所有子OU的配置信息
        /// </summary>
        /// <param name="parentUnid">上级OU的Unid</param>
        /// <param name="ouType">OU的类型，为null表示查询所有类型</param>
        /// <returns>指定OU下指定类型的所有子OU的配置信息</returns>
        IList FindChilds(string parentUnid, string ouType);

        /// <summary>
        /// 获取指定OU下指定类型的所有子OU的配置信息
        /// </summary>
        /// <param name="parentUnid">上级OU的Unid</param>
        /// <param name="ouType">OU的类型，为null表示查询所有类型</param>
        /// <param name="includeChildOU">是否包含下级OU（包含下级的下级等）的信息</param>
        /// <returns>指定OU下指定类型的所有子OU的配置信息</returns>
        IList FindChilds(string parentUnid, string ouType, bool includeChildOU);

        /// <summary>
        /// 获取指定OU下的所有子OU的配置信息
        /// </summary>
        /// <param name="parentUnid">上级OU的Unid</param>
        /// <returns>指定OU下的所有子OU的配置信息</returns>
        IList FindChilds(string parentUnid);

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
