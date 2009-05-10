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
    /// 岗位配置DAO的接口定义
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-05-31</date>
    public interface IGroupDao : IBaseDao<Group>
    {
        /// <summary>
        /// 根据所属OU的Unid查询所配置的岗位信息
        /// </summary>
        /// <param name="ouUnid">所属OU的Unid</param>
        /// <returns>返回符合条件的岗位配置信息的集合</returns>
        IList FindByOU(string ouUnid);

        /// <summary>
        /// 根据所属OU的Unid查询所配置的岗位信息
        /// </summary>
        /// <param name="ouUnids">所属OU的Unid列表</param>
        /// <returns>返回符合条件的岗位配置信息的集合</returns>
        IList FindByOU(string[] ouUnids);

        /// <summary>
        /// 根据所属OU的Unid查询所配置的岗位信息
        /// </summary>
        /// <param name="ouUnids">所属OU的Unid列表</param>
        /// <param name="isCanDispatch">是否可以派单</param>
        /// <returns>返回符合条件的岗位配置信息的集合</returns>
        IList FindByOU(string[] ouUnids, bool isCanDispatch);

        /// <summary>
        /// 根据所属OU的Unid查询所配置的岗位信息
        /// </summary>
        /// <param name="ouUnids">所属OU的Unid列表</param>
        /// <param name="groupType">岗位类型：0--全部类型，1--可派单岗位，2--不可派单岗位</param>
        /// <returns>返回符合条件的岗位配置信息的集合</returns>
        IList FindByOU(string[] ouUnids, string groupType);

        /// <summary>
        /// 获取指定OU中所配置的指定名称的岗位信息
        /// </summary>
        /// <param name="ouUnid">OU的Unid</param>
        /// <param name="groupName">岗位名称</param>
        /// <returns>返回符合条件的岗位信息集合</returns>
        IList FindByName(string ouUnid, string groupName);

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
        /// <returns></returns>
        IList FindAllSendTo(string[] ouUnid);

        /// <summary>
        /// 查找指定人员所拥有的岗位列表
        /// </summary>
        /// <param name="userUnid">人员的Unid</param>
        /// <returns>返回符合条件的岗位配置信息的集合</returns>
        IList FindByUser(string userUnid);

        /// <summary>
        /// 获取岗位配置的配置信息
        /// </summary>
        /// <param name="ouUnid">所属OU的UNID</param>
        /// <param name="pageNo">所要获取的页面数</param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDir"></param>
        /// <returns>该页的信息</returns>
        PageInfo GetPageByOU(string ouUnid, int pageNo, int pageSize, string sortField, string sortDir);

        /// <summary>
        /// 判断对象在数据库中是否唯一
        /// </summary>
        /// <param name="group">所要判断的对象</param>
        /// <returns>在数据库中是唯一的就返回true,否则返回false</returns>
        bool IsUnique(Group group);

        /// <summary>
        /// 获取系统的默认岗位，当人员没有配置任何岗位信息时系统默认该人员拥有该岗位
        /// </summary>
        /// <returns>系统的默认岗位</returns>
        Group GetSysDefaultGroup();

        /// <summary>
        /// 按照岗位编码和组织OUCode判断是否有该岗位
        /// </summary>
        /// <param name="code"></param>
        /// <param name="ouCode"></param>
        /// <returns></returns>
        bool DoHaveGroupByCode(string code, string ouCode);

        /// <summary>
        /// 根据Code加载岗位配置配置信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Group LoadByCode(string code);
    }
}
