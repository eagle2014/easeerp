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
using System.Collections;
using TSLib.Dao;
using TSCommon_Core.Security.Domain;

namespace TSCommon_Core.Security.Dao
{
    /// <summary>
    /// 模块Dao的接口定义
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-06-01</date>
    public interface IModelDao : IBaseDao<Model>
    {
        /// <summary>
        /// 获取除指定模块以外的全部模块的配置信息
        /// </summary>
        /// <param name="id">所要排除的模块配置信息</param>
        /// <returns>所有配置的模块信息</returns>
        IList FindAllWithExclude(long id);

        /// <summary>
        /// 判断对象在数据库中是否唯一
        /// </summary>
        /// <param name="model">所要判断的对象</param>
        /// <returns>在数据库中是唯一的就返回true,否则返回false</returns>
        bool IsUnique(Model model);

        /// <summary>
        /// 获取全部主模块
        /// </summary>
        /// <returns>全部主模块</returns>
        IList FindAllMaster();

        /// <summary>
        /// 获取指定模块的所有子模块
        /// </summary>
        /// <param name="modelID">指定模块的主键</param>
        /// <returns>指定模块的所有子模块，如果modelID不大于0，则返回所有主模块</returns>
        IList FindChildren(long modelID);

        /// <summary>
        /// 获取指定模块的所有子模块
        /// </summary>
        /// <param name="modelUnid">指定模块的Unid</param>
        /// <returns>指定模块的所有子模块，如果modelUnid为空，则返回所有主模块</returns>
        IList FindChildren(string modelUnid);

        /// <summary>
        /// 根据编码返回模块
        /// </summary>
        /// <param name="code">模块编码</param>
        /// <returns></returns>
        Model GetByCode(string code);
    }
}
