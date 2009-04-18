using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using TSLib;
using EaseErp_WareHouse.WareHouse.Domain;

namespace EaseErp_WareHouse.WareHouse.Service
{
    public interface IWareHouseService:IBaseService<WareHouse.Domain.WareHouse>
    {
        /// <summary>
        /// 获取WareHouse
        /// </summary>
        /// <param name="pageNo">页码,起始页码从1开始</param>
        /// <param name="pageSize">每页的记录数</param>
        /// <param name="sortField">要排序的属性名</param>
        /// <param name="sortDir">排序方向</param>
        /// <returns>该页的信息</returns>
        PageInfo GetPageByPlace(int pageNo, int pageSize, string sortField, string sortDir,Place place);
    }
}
