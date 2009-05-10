using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSLib.Dao;
using TSLib;
using TSCommon.Core.Security.Domain;

namespace TSCommon.Core.Security.Dao
{
    /// <summary>
    /// 角色配置Dao的接口定义
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-06-01</date>
    public interface IRoleDao : IBaseDao<Role>
    {
        /// <summary>
        /// 获取指定级别的角色配置信息
        /// </summary>
        /// <param name="level">角色级别编码</param>
        /// <param name="hasChild">是否包含子集</param>
        /// <returns>符合条件的角色集合列表</returns>
        IList FindByLevel(string level, bool hasChild);

        /// <summary>
        /// 获取指定岗位中所包含的角色信息
        /// </summary>
        /// <param name="grouUnid">岗位的Unid</param>
        /// <returns>符合条件的角色集合列表</returns>
        IList FindByGroup(string grouUnid);

        /// <summary>
        /// 获取指定人员所拥有的角色信息
        /// </summary>
        /// <param name="userInfoUnid">人员的Unid</param>
        /// <returns>符合条件的角色集合列表</returns>
        IList FindByUser(string userInfoUnid);

        /// <summary>
        /// 根据角色的代码加载相应的角色信息
        /// </summary>
        /// <param name="code">角色的代码</param>
        /// <returns>相应角色的配置信息，如果没有则返回NULL</returns>
        Role LoadByCode(string code);

        /// <summary>
        /// 判断对象在数据库中是否唯一
        /// </summary>
        /// <param name="role">所要判断的对象</param>
        /// <returns>在数据库中是唯一的就返回true,否则返回false</returns>
        bool IsUnique(Role role);

        /// <summary>
        /// 获取指定级别的角色的分页信息。
        /// </summary>
        /// <param name="level">角色的级别代码</param>
        /// <param name="pageNo">页码,起始页码从1开始</param>
        /// <param name="pageSize">每页的记录数</param>
        /// <param name="sortField">要排序的属性名</param>
        /// <param name="sortDir">排序方向值</param>
        /// <returns>指定页码的分页对象,如果找不到任何结果则返回空页</returns>
        PageInfo GetPage(int level, int pageNo, int pageSize, string sortField, string sortDir);
    }
}
