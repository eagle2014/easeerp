/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSLib.Dao;
using TSLib;
using TSCommon.Core.Organize.Domain;

namespace TSCommon.Core.Organize.Dao
{
    /// <summary>
    /// 人员配置DAO的接口定义
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-05-31</date>
    public interface IUserDao : IBaseDao<User>
    {
        /// <summary>
        /// 根据所属OU的Unid查询所配置的人员信息
        /// </summary>
        /// <param name="ouUnid">所属OU的Unid，</param>
        /// <returns>返回符合条件的人员配置信息的集合</returns>
        IList FindByOU(string ouUnid);

        /// <summary>
        /// 获取指定OU中指定类型的人员信息
        /// </summary>
        /// <param name="ouUnid">OU的Unid</param>
        /// <param name="userType">用户类型</param>
        /// <returns>该OU中所配置的人员配置的信息</returns>
        IList FindByOU(string ouUnid, string userType);

        /// <summary>
        /// 获取指定OU中指定类型的人员信息
        /// </summary>
        /// <param name="ouUnids">OU的Unid列表</param>
        /// <param name="userType">用户类型</param>
        /// <returns>该OU中所配置的人员配置的信息</returns>
        IList FindByOU(string[] ouUnids, string userType);

        /// <summary>
        /// 查找指定岗位中所包含的人员列表
        /// </summary>
        /// <param name="groupUnid">群组的Unid</param>
        /// <returns>返回符合条件的人员配置信息的集合</returns>
        IList FindByGroup(string groupUnid);

        /// <summary>
        /// 查找指定岗位中所包含的指定类型的人员列表
        /// </summary>
        /// <param name="groupUnid">群组的Unid</param>
        /// <param name="userType">用户类型</param>
        /// <returns>返回符合条件的人员配置信息的集合</returns>
        IList FindByGroup(string groupUnid, string userType);

        /// <summary>
        /// 获取指定OU下的人员信息
        /// </summary>
        /// <param name="ouUnid">所属OU的UNID，为空代表忽略</param>
        /// <param name="pageNo">页码,起始页码从1开始</param>
        /// <param name="pageSize">每页的记录数</param>
        /// <param name="sortField">要排序的属性名</param>
        /// <param name="sortDir">排序方向</param>
        /// <returns>该页的信息</returns>
        PageInfo GetPageByOU(string ouUnid, int pageNo, int pageSize, string sortField, string sortDir);

        /// <summary>
        /// 根据用户的登录名加载相应的人员信息
        /// </summary>
        /// <param name="loginID">用户的登录ID</param>
        /// <returns>如果存在该用户的信息则返回，否则返回NULL</returns>
        User LoadByLoginID(string loginID);

        /// <summary>
        /// 判断对象在数据库中是否唯一
        /// </summary>
        /// <param name="userInfo">所要判断的对象</param>
        /// <returns>在数据库中是唯一的就返回true,否则返回false</returns>
        bool IsUnique(User userInfo);

        /// <summary>
        /// 查找在messageGroupUser中不存在的人
        /// </summary>
        /// <param name="ouUnid"></param>
        /// <param name="userUnid"></param>
        /// <returns></returns>
        IList FindAllWithOutMessageUser(string ouUnid, string userUnid);

        /// <summary>
        /// 根据电话号码查询指定的人员信息
        /// </summary>
        /// <param name="telephoneNo">所要查询的电话号码</param>
        /// <returns>符合条件的人员信息列表</returns>
        IList FindAllByTelephone(string telephoneNo);

        /// <summary>
        /// 根据登录姓名查询人员信息
        /// </summary>
        /// <param name="loginID">登录姓名</param>
        /// <returns>符合条件的人员信息列表</returns>
        IList FindAllByLoginID(string loginID);

        /// <summary>
        /// 根据姓名查询人员信息
        /// </summary>
        /// <param name="name">姓名</param>
        /// <returns>符合条件的人员信息列表</returns>
        IList FindAllByName(string name);

        /// <summary>
        /// 根据原先employeeID查找数据
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        User GetUserInfoByEmployeeID(string employeeID);

        /// <summary>
        /// 获得所有EmployeeID,用逗号隔开
        /// </summary>
        /// <returns></returns>
        string GetAllEmployeeID();

        /// <summary>
        /// 获取指定Unid集的所有人员信息
        /// </summary>
        /// <param name="unids">人员Unid集</param>
        /// <returns>指定Unid集的所有人员信息</returns>
        IList FindByUnids(string[] unids);

        /// <summary>
        /// 根据关系的parentUnid返回用户列表
        /// </summary>
        /// <param name="relationShipParentUnid"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageInfo FindAllByRelationShipParentUnid(string relationShipParentUnid, int pageNo, int pageSize);
    }
}
