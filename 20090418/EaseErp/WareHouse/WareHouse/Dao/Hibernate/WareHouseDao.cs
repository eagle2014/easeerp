using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Dao.Hibernate;
using TSLib.DBUtils;
using EaseErp_WareHouse.WareHouse.Domain;

namespace EaseErp_WareHouse.WareHouse.Dao.Hibernate
{
    public class WareHouseDao:BaseDao<WareHouse.Domain.WareHouse>,IWareHouseDao
    {
        #region IWareHouseDao 成员

        public TSLib.PageInfo GetPageByPlace(int pageNo, int pageSize, string sortField, string sortDir, Place place)
        {
            if (null==place)
            {
                string hql = "from WareHouse wareHouse";
                return NHibernateHelper.GetPage(this.Session, pageNo, pageSize, sortField, sortDir,
                    "wareHouse", hql, (object[])null, null);
            }
            else
            {
                string hql = "from WareHouse wareHouse where wareHouse.Place.id = ?";
                return NHibernateHelper.GetPage(this.Session, pageNo, pageSize, sortField, sortDir,
                    "wareHouse", hql, new object[] { place.ID } , null);
            }
        }

        #endregion
    }
}
