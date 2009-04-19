using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Dao;
using TSCommon_Core.SystemPolicy.Domain;

namespace TSCommon_Core.SystemPolicy.Dao
{
    /// <summary>
    /// 系统策略DAO的接口定义
    /// </summary>
    public interface IPolicyDao : IBaseDao<Policy>
    {
        /// <summary>
        /// 装载指定编码的系统策略信息
        /// </summary>
        /// <param name="code">系统策略的编码</param>
        /// <returns>相应的策略，如果没有则返回NULL</returns>
        Policy LoadByCode(string code);

        /// <summary>
        /// 判断对象在数据库中是否唯一
        /// </summary>
        /// <param name="policy">所要判断的对象</param>
        /// <returns>在数据库中是唯一的就返回true,否则返回false</returns>
        bool IsUnique(Policy policy);
    }
}
