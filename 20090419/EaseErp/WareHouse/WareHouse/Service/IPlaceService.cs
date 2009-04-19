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
        /// װ�ض���
        /// </summary>
        /// <param name="ids">ids</param>
        /// <returns>װ�ض���</returns>
        IList Load(long[] ids);

        /// <summary>
        /// װ�ض���
        /// </summary>
        /// <param name="unids">unids</param>
        /// <returns>װ�ض���</returns>
        IList Load(string[] unids);

        /// <summary>
        /// �ɸ�Unid���صص��б�
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
