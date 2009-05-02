/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 * 
 * Autor: CD826         2007-02-28
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSLib.Service;
using TSCommon.Core.Security.Domain;

namespace TSCommon.Core.Security.Service
{
    /// <summary>
    /// 角色配置Service的接口定义
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-06-01</date>
    public interface IRoleService : IBaseService<Role>
    {
        /// <summary>
        /// 获取指定组织中角色的配置信息
        /// </summary>
        /// <param name="ouUnid">组织的Unid</param>
        /// <returns>相应配置的角色信息</returns>
        IList FindByOU(string ouUnid);

        /// <summary>
        /// 获取指定级别的角色配置信息
        /// </summary>
        /// <param name="level">角色级别编码</param>
        /// <param name="hasChild">是否包含子集</param>
        /// <returns>符合条件的角色集合列表</returns>
        IList FindByLevel(string level, bool hasChild);

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
    }
}
