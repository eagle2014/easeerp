using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using TSCommon.Core.Organize.Domain;
using System.Collections;

namespace TSCommon.Core.Organize.Service
{
    public interface IJobTitleService : IBaseService<JobTitle>
    {
        /// <summary>
        /// 根据职务编码获得职务信息
        /// </summary>
        /// <param name="jobTitleCode">职务编码</param>
        /// <returns>指定编码的职务信息</returns>
        JobTitle LoadByCode(string jobTitleCode);

        /// <summary>
        /// 获取指定级别的所有职务信息
        /// </summary>
        /// <param name="level">级别的值</param>
        /// <returns>指定级别的所有职务信息</returns>
        IList FindAllByLevel(string level);

        /// <summary>
        /// 获取指定级别范围的所有职务信息
        /// </summary>
        /// <param name="levels">级别的值列表</param>
        /// <returns>指定级别范围的所有职务信息</returns>
        IList FindAllByLevel(string[] levels);
    }
}
