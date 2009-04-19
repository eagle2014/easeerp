/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using System.Collections;
using TSLib;
using TSCommon.Core.Organize.Domain;

namespace TSCommon.Core.Organize.Service
{
    /// <summary>
    /// 人员配置Service的接口定义
    /// </summary>
    /// <author>CD826</author>
    /// <author>zzh</author>
    /// <author>Tony</author>
    /// <date>2008-05-31</date>
    public interface IUserService : IBaseService<User>
    {
        /// <summary>
        /// 获取指定OU中所有人员配置的配置信息
        /// </summary>
        /// <param name="ouUnid">OU的Unid</param>
        /// <returns>该OU中所配置的人员配置的信息</returns>
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
        /// <param name="ouUnid">OU的Unid</param>
        /// <param name="userType">用户类型</param>
        /// <param name="includeChildOU">是否包含下级及下级的下级OU的用户</param>
        /// <returns>该OU中所配置的人员配置的信息</returns>
        IList FindByOU(string ouUnid, string userType, bool includeChildOU);

        /// <summary>
        /// 获取人员配置的配置信息
        /// </summary>
        /// <param name="ouUnid">所属OU的UNID</param>
        /// <param name="pageNo">页码,起始页码从1开始</param>
        /// <param name="pageSize">每页的记录数</param>
        /// <param name="sortField">要排序的属性名</param>
        /// <param name="sortDir">排序方向</param>
        /// <returns>该页的信息</returns>
        PageInfo GetPageByOU(string ouUnid, int pageNo, int pageSize, string sortField, string sortDir);

        /// <summary>
        /// 验证指定的信息是否正确  
        /// </summary>
        /// <param name="loginName">所要验证的用户名</param>
        /// <param name="password">验证的口令</param>
        /// <returns>如果验证成功返回该用户的信息，否则抛出AuthorizeException</returns>
        User Authorize(string loginName, string password);

        /// <summary>
        /// 激活指定的人员配置配置信息
        /// </summary>
        /// <param name="curUser">当前操作的人员</param>
        /// <param name="userInfo">所要激活的人员配置配置信息</param>
        void EnabledUser(User curUser, User userInfo);

        /// <summary>
        /// 禁用指定的人员配置配置信息
        /// </summary>
        /// <param name="curUser">当前操作的人员</param>
        /// <param name="userInfo">所要禁用的人员配置配置信息</param>
        void DisabledUser(User curUser, User userInfo);

        /// <summary>
        /// 直接修改用户的口令
        /// </summary>
        /// <param name="curUser">当前操作的用户</param>
        /// <param name="userInfoID">所要修改用户的ID</param>
        /// <param name="password">新的password</param>
        void ChangeUserPassword(User curUser, long userInfoID, string password);

        /// <summary>
        /// 修改用户的口令
        /// </summary>
        /// <param name="curUser">当前操作的用户</param>
        /// <param name="userInfoID">所要修改用户的ID</param>
        /// <param name="password">新的password</param>
        /// <param name="oldPassword">该用户原来的口令</param>
        void ChangeUserPassword(User curUser, long userInfoID, string password, string oldPassword);

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
        /// 获取用户的数据权限范围
        /// </summary>
        /// <param name="userInfo">当前操作的人员</param>
        /// <param name="privilege">数据权限名称</param>
        /// <returns>该人员所具有的数据权限范围，如果是全部则返回一个空的数组，默认返回当前用户所在单位的Unid</returns>
        string[] FindDataScope(User userInfo, string privilege);

        bool ParseSave(User curUser, User userInfo);

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

        void DeleteByEmployeeID(string employeeID);

        /// <summary>
        /// 更改用户的密码
        /// </summary>
        /// <param name="curUser"></param>
        /// <param name="userInfoID"></param>
        /// <param name="password">明文密码</param>
        /// <returns>新的已加密的密码</returns>
        string ChangeUserPasswordOne(User curUser, long userInfoID, string password);

        /// <summary>
        /// 根据用户的登录名加载相应的人员信息
        /// </summary>
        /// <param name="loginID">用户的登录ID</param>
        /// <returns>如果存在该用户的信息则返回，否则返回NULL</returns>
        User LoadByLoginID(string loginID);

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
