using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Dao;
using TSCommon.Core.Organize.Domain;
using System.Collections;

namespace TSCommon.Core.Organize.Dao
{
    /// <summary>
    /// 级别Dao接口的定义
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-05-25</date>
    public interface IOULevelDao : IBaseDao<OULevel>
    {
        /// <summary>
        /// 获取指定级别编码集的所有级别信息
        /// </summary>
        /// <param name="codes">级别的Level集</param>
        /// <returns>指定级别集的所有级别信息</returns>
        IList FindByCodes(string[] codes);

        /// <summary>
        /// 根据级别编码得到相应的级别配置
        /// </summary>
        /// <param name="code">级别编码</param>
        /// <returns>如果在数据库中进行了配置，则返回该配置，否则返回NULL</returns>
        OULevel LoadByCode(string code);

        /// <summary>
        /// 判断对象在数据库中是否唯一
        /// </summary>
        /// <param name="ouLevel">所要判断的对象</param>
        /// <returns>在数据库中是唯一的就返回true,否则返回false</returns>
        bool IsUnique(OULevel ouLevel);
    }
}
