/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 * 
 * Autor: CD826         2007-02-27
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSLib.Dao;
using TSCommon.Core.Security.Domain;

namespace TSCommon.Core.Security.Dao
{
    /// <summary>
    /// 权限Dao的接口定义
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-06-01</date>
    public interface IPrivilegeDao : IBaseDao<Privilege>
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
        /// 判断对象在数据库中是否唯一
        /// </summary>
        /// <param name="privilege">所要判断的对象</param>
        /// <returns>在数据库中是唯一的就返回true,否则返回false</returns>
        bool IsUnique(Privilege privilege);
    }
}
