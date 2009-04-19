using System;
using System.Collections.Generic;
using System.Text;
using TSCommon.Core.OptionItemCfg.Domain;
using TSLib.Dao;
using System.Collections;
using TSLib;

namespace TSCommon.Core.OptionItemCfg.Dao
{
    public interface IOptionItemDao:IBaseDao<OptionItem>
    {
        /// <summary>
        /// 获取指定类型的业务对象配置项
        /// </summary>
        /// <param name="type">业务对象类型</param>
        /// <returns>符合条件的业务对象类型列表</returns>
        IList FindAll(string type);

        /// <summary>
        /// 获取指定页面中业务对象配置项
        /// </summary>
        /// <param name="firstNo"></param>
        /// <param name="maxResult"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDir"></param>
        /// <returns></returns>
        new PageInfo GetPage(int firstNo, int maxResult, string sortField, string sortDir);

        /// <summary>
        /// 获取指定类型的业务对象配置项
        /// </summary>
        /// <param name="firstNo"></param>
        /// <param name="maxResult"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDir"></param>
        /// <param name="type">业务对象类型</param>
        /// <returns></returns>
        PageInfo GetPage(int firstNo, int maxResult, string sortField,
                         string sortDir, string type);

        /// <summary>
        /// 获取所有可选的类型信息
        /// </summary>
        /// <returns></returns>
        IList FindAllType();
    }
}
