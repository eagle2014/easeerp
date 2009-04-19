using System;
using System.Collections.Generic;
using System.Text;
using TSCommon_Core.Organize.Domain;
using System.Collections;
using TSLib.Dao;

namespace TSCommon_Core.Organize.Dao
{
    public interface IJobTitleDao : IBaseDao<JobTitle>
    {
        /// <summary>
        /// 根据职务编码获得职务信息
        /// </summary>
        /// <param name="jobTitleCode">职务编码</param>
        /// <returns>指定编码的职务信息</returns>
        JobTitle LoadByCode(string jobTitleCode);

        /// <summary>
        /// 获取指定级别范围的所有职务信息
        /// </summary>
        /// <param name="levels">级别的值列表，为空则返回空的集合</param>
        /// <returns>指定级别范围的所有职务信息</returns>
        IList FindAllByLevel(string[] levels);

        /// <summary>
        /// 判断对象在数据库中是否唯一
        /// </summary>
        /// <param name="jobTitle">所要判断的对象</param>
        /// <returns>在数据库中是唯一的就返回true,否则返回false</returns>
        bool IsUnique(JobTitle jobTitle);
    }
}
