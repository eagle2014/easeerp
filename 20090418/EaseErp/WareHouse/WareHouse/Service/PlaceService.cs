using System;
using System.Collections.Generic;
using System.Text;
using EaseErp_WareHouse.WareHouse.Domain;
using TSLib.Service;
using EaseErp_WareHouse.WareHouse.Dao;
using System.Collections;

namespace EaseErp_WareHouse.WareHouse.Service
{
    public class PlaceService:BaseService<Place>,IPlaceService
    {
        private IPlaceDao placeDao;         // Dao
        public IPlaceDao PlaceDao
        {
            set
            {
                placeDao = value;
                base.BaseDao = value;
            }
        }


        #region IPlaceService 成员

        public IList Load(long[] ids)
        {
            return this.placeDao.Load(ids);
        }

        public IList Load(string[] unids)
        {
            return this.placeDao.Load(unids);
        }

        #endregion

        #region IPlaceService 成员


        public IList GetPlacesByParentUnid(string parentUnid)
        {
            return this.placeDao.GetPlacesByParentUnid(parentUnid);
        }

        #endregion

        #region IPlaceService 成员


        public TSLib.PageInfo GetPageInfoByParentUnid(int pageNo, int pageSize, string sortField, string sortDir, string parentUnid)
        {
            return this.placeDao.GetPageInfoByParentUnid(pageNo, pageSize, sortField, sortDir, parentUnid);
        }

        #endregion
    }
}
