using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSLib.Service;
using TSCommon.Core.Security.Domain;
using TSCommon.Core.Organize.Domain;

namespace TSCommon.Core.Security.Service
{
    /// <summary>
    /// 权限配置Service的接口定义
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-06-01</date>
    public interface IPrivilegeService : IBaseService<Privilege>
    {
        /// <summary>
        /// 获取指定类型的权限配置信息
        /// </summary>
        /// <param name="privilegeType">权限类型</param>
        /// <returns>符合条件的权限集合列表</returns>
        IList FindByType(string privilegeType);

        /// <summary>
        /// 获取指定模块下的所有权限列表
        /// </summary>
        /// <param name="modelID">模块的ID</param>
        /// <returns>符合条件的权限集合列表</returns>
        IList FindByModel(long modelID);

        /// <summary>
        /// 获取指定模块下指定类型的所有权限列表
        /// </summary>
        /// <param name="modelID">模块的ID</param>
        /// <param name="type">要获取的权限的类型</param>
        /// <returns>符合条件的权限集合列表</returns>
        IList FindByModelAndType(long modelID, string type);

        /// <summary>
        /// 获取指定模块下指定类型的所有权限列表
        /// </summary>
        /// <param name="modelUnid">模块的Unid，为空则返回空的权限列表</param>
        /// <param name="type">要获取的权限的类型，为空代表全部</param>
        /// <returns>符合条件的权限集合列表</returns>
        IList FindByModelAndType(string modelUnid, string type);

        /// <summary>
        /// 根据权限的代码加载相应的权限信息
        /// </summary>
        /// <param name="code">权限的代码</param>
        /// <returns>相应权限的配置信息，如果没有则返回NULL</returns>
        Privilege LoadByCode(string code);

        /// <summary>
        /// 判断指定的用户是否拥有指定的权限
        /// </summary>
        /// <param name="userInfo">要判断的用户</param>
        /// <param name="privilegeKey">权限编码</param>
        /// <returns></returns>
        bool HasPrivilege(User userInfo, string privilegeKey);

        /// <summary>
        /// 判断指定的用户是否拥有指定的权限集中的任一权限
        /// </summary>
        /// <param name="userInfo">要判断的用户</param>
        /// <param name="privilegeKeys">权限编码列表</param>
        /// <returns></returns>
        bool HasAnyPrivilege(User userInfo, string[] privilegeKeys);

        /// <summary>
        /// 判断指定的岗位是否拥有指定的权限集中的任一权限
        /// </summary>
        /// <param name="group">要判断的岗位</param>
        /// <param name="privilegeKeys">权限编码列表</param>
        /// <returns></returns>
        bool HasAnyPrivilege(Group group, string[] privilegeKeys);

        /// <summary>
        /// 判断指定的角色是否拥有指定的权限集中的任一权限
        /// </summary>
        /// <param name="role">要判断的角色</param>
        /// <param name="privilegeKeys">权限编码列表</param>
        /// <returns></returns>
        bool HasAnyPrivilege(Role role, string[] privilegeKeys);

    }
}
