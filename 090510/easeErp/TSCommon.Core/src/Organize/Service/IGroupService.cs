/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSLib.Service;
using TSLib;
using TSCommon.Core.Organize.Domain;

namespace TSCommon.Core.Organize.Service
{
    /// <summary>
    /// 岗位配置Service的接口定义
    /// </summary>
    /// <author>CD826</author>
    /// <author>Tony</author>
    /// <date>2008-05-31</date>
    public interface IGroupService : IBaseService<Group>
    {
        /// <summary>
        /// 获取指定OU中所配置的岗位信息
        /// </summary>
        /// <param name="ouUnid">OU的Unid</param>
        /// <param name="includeUnit">是否包含所属单位的岗位信息</param>
        /// <returns>返回符合条件的岗位信息集合</returns>
        IList FindByOU(string ouUnid, bool includeUnit);

        /// <summary>
        /// 根据所属OU的Unid查询所配置的可派单的岗位信息
        /// </summary>
        /// <param name="ouUnid">所属OU的Unid</param>
        /// <returns>返回符合条件的岗位配置信息的集合</returns>
        IList FindAllSendTo(string ouUnid);

        /// <summary>
        /// 根据所属OU的Unid查询所配置的可派单的岗位信息
        /// </summary>
        /// <param name="ouUnid">所属OU的Unid</param>
        /// <param name="includeChild">是否搜索子部门的岗位</param>
        /// <returns></returns>
        IList FindAllSendTo(string ouUnid, bool includeChild);

        /// <summary>
        /// 获取指定OU中所配置的岗位信息
        /// </summary>
        /// <param name="ouUnid">OU的Unid</param>
        /// <param name="includeUnit">是否包含所属单位的岗位信息</param>
        /// <param name="isCanDispatch">是否是查询需要派单的岗位, false表示查询所有</param>
        /// <returns>返回符合条件的岗位信息集合</returns>
        IList FindByOU(string ouUnid, bool includeUnit, bool isCanDispatch);

        /// <summary>
        /// 获取指定OU中所配置的岗位信息
        /// </summary>
        /// <param name="ouUnid">OU的Unid</param>
        /// <param name="includeUnit">是否包含所属单位的岗位信息</param>
        /// <param name="groupType">岗位类型：0--全部类型，1--可派单岗位，2--不可派单岗位</param>
        /// <returns>返回符合条件的岗位信息集合</returns>
        IList FindByOU(string ouUnid, bool includeUnit, string groupType);

        /// <summary>
        /// 获取指定OU中所配置的岗位信息
        /// </summary>
        /// <param name="ouUnid">OU的Unid</param>
        /// <param name="includeUnit">是否包含所属单位的岗位信息</param>
        /// <param name="groupType">岗位类型：0--全部类型，1--可派单岗位，2--不可派单岗位</param>
        /// <param name="includeChildOUGroup">是否包含下级部门（包含部门的子部门）的岗位信息</param>
        /// <returns>返回符合条件的岗位信息集合</returns>
        IList FindByOU(string ouUnid, bool includeUnit, string groupType, bool includeChildOUGroup);

        /// <summary>
        /// 获取指定OU中所配置的指定名称的岗位信息
        /// </summary>
        /// <param name="ouUnid">OU的Unid</param>
        /// <param name="groupName">岗位名称</param>
        /// <returns>返回符合条件的岗位信息集合</returns>
        IList FindByName(string ouUnid, string groupName);

        /// <summary>
        /// 获取指定OU中所配置的指定名称的岗位信息
        /// </summary>
        /// <param name="ouUnid">OU的Unid</param>
        /// <param name="groupName">岗位名称</param>
        /// <param name="continueFindByUpperOU">如果在指定的部门中找不到该岗位，是否在其上级部门中继续向上找，直到单位为止</param>
        /// <returns>返回符合条件的岗位信息集合</returns>
        IList FindByName(string ouUnid, string groupName, bool continueFindByUpperOU);
       
        /// <summary>
        /// 获取岗位配置的配置信息
        /// </summary>
        /// <param name="ouUnid">所属OU的UNID</param>
        /// <param name="pageNo">页码,起始页码从1开始</param>
        /// <param name="pageSize">每页的记录数</param>
        /// <param name="sortField">要排序的属性名</param>
        /// <param name="sortDir">排序方向</param>
        /// <returns>该页的信息</returns>
        PageInfo GetPageByOU(string ouUnid, int pageNo, int pageSize, string sortField, string sortDir);

        /// <summary>
        /// 根据Code加载岗位配置配置信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Group LoadByCode(string code);

        /// <summary>
        /// 为人员同步提供保存操作
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="group"></param>
        void ParseSave(User userInfo, Group group);

        /// <summary>
        /// 按照岗位编码和组织OUCode判断是否有该岗位
        /// </summary>
        /// <param name="code"></param>
        /// <param name="ouCode"></param>
        /// <returns></returns>
        bool DoHaveGroupByCode(string code, string ouCode);

        /// <summary>
        /// 按照ouUnid删除所有岗位
        /// </summary>
        /// <param name="ouUnid"></param>
        void DeleteAllByOU(string ouUnid);
    }
}
