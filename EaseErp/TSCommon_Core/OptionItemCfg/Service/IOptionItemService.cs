using System;
using System.Collections.Generic;
using System.Text;
using TSCommon_Core.OptionItemCfg.Domain;
using TSLib.Service;
using System.Collections;
using TSLib;

namespace TSCommon_Core.OptionItemCfg.Service
{
    public interface IOptionItemService:IBaseService<OptionItem>
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
        PageInfo GetPage(int firstNo, int maxResult, string sortField, string sortDir);

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
        /// 删除所有的业务对象配置项
        /// </summary>
        new void DeleteAll();

        /// <summary>
        /// 删除指定类型的业务对象配置项
        /// </summary>
        /// <param name="type">指定的类型</param>
        void DeleteAll( string type);

        /// <summary>
        /// 取得指定类型业务对象配置的选项列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>业务对象的选项列表</returns>
        HtmlOption[] GetOptions(string type);

        /// <summary>
        /// 取得指定类型业务对象配置的选项列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="curValue">当前所选择的值，如果在配置列表中没有该值那么添加一个选项，否则不添加</param>
        /// <returns>业务对象的选项列表</returns>
        HtmlOption[] GetOptions(string type, string curValue);

        /// <summary>
        /// 获取类型的名称
        /// </summary>
        /// <param name="type">类型的值</param>
        /// <returns>该类型的描述</returns>
        string GetTypeName(string type);

        /// <summary>
        /// 获取所有可选的类型信息
        /// </summary>
        /// <returns></returns>
        IList FindAllType();

        /// <summary>
        /// 获取所有可选的类型信息选项列表 
        /// </summary>
        /// <returns></returns>
        HtmlOption[] GetTypeOptions();
    }
}
