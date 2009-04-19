using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using TSCommon.Core.SystemPolicy.Domain;
using TSCommon.Core.Organize.Domain;

namespace TSCommon.Core.SystemPolicy.Service
{
    /// <summary>
    /// 系统策略
    /// </summary>
    public interface IPolicyService : IBaseService<Policy>
    {
        /// <summary>
        /// 装载指定编码的系统策略信息
        /// </summary>
        /// <param name="code">系统策略的编码</param>
        /// <returns>相应的策略，如果没有则返回NULL</returns>
        Policy LoadByCode(string code);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="user"></param>
        /// <param name="policy"></param>
        void Save(User user, Policy policy);
    }
}
