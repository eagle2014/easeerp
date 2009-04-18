using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using EaseErp_WareHouse.WareHouse.Dao;
using EaseErp_WareHouse.WareHouse.Domain;
using TSCommon_Core.Organize.RelationShips;

namespace EaseErp_WareHouse.WareHouse.Service
{
    public class WareHouseService:BaseService<WareHouse.Domain.WareHouse>, IWareHouseService
    {

        #region　相关Dao
        private IWareHouseDao wareHouseDao;
        private IRelationShipService relationShipService;

        public IWareHouseDao WareHouseDao
        {
            set { this.wareHouseDao = value; }
        }
        public IRelationShipService RelationShipService
        {
            set { this.relationShipService = value; }
        }
        #endregion

        #region IWareHouseService 成员

        public TSLib.PageInfo GetPageByPlace(int pageNo, int pageSize, string sortField, string sortDir,Place place)
        {
            return this.wareHouseDao.GetPageByPlace(pageNo, pageSize, sortField, sortDir, place);
        }

        #endregion
    }
}
