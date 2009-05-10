using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using TSCommon.Core.Organize.Domain;
using System.Collections;

namespace TSCommon.Core.Organize.Service
{
    public interface IOULevelService : IBaseService<OULevel>
    {
        /// <summary>
        /// 获取指定级别集的所有级别信息
        /// </summary>
        /// <param name="levels">级别的Level集</param>
        /// <returns>指定级别集的所有级别信息</returns>
        IList FindByLevels(string[] levels);
    }
}
