using System;
using System.Collections.Generic;
using System.Text;
using EaseErp_WareHouse.WareHouse.Domain;
using TSLib.Service;
using System.Collections;
using TSLib;

namespace EaseErp_WareHouse.WareHouse.Service
{
    public interface IPlaceService:IBaseService<Place>
    {
        /// <summary>
        /// 装载对象
        /// </summary>
        /// <param name="ids">ids</param>
        /// <returns>装载对象</returns>
        IList Load(long[] ids);

        /// <summary>
        /// 装载对象
        /// </summary>
        /// <param name="unids">unids</param>
        /// <returns>装载对象</returns>
        IList Load(string[] unids);

        /// <summary>
        /// 由父Unid返回地点列表
        /// </summary>
        /// <param name="parentUnid"></param>
        /// <returns></returns>
        IList GetPlacesByParentUnid(string parentUnid);

        /// <summary>
        /// get pageInfo by parentUnid
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDir"></param>
        /// <param name="parentUnid"></param>
        /// <returns></returns>
        PageInfo GetPageInfoByParentUnid(int pageNo, int pageSize, string sortField, string sortDir,string parentUnid);
    }
}
