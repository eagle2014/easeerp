using System;
using System.Collections.Generic;
using System.Text;
using EaseErp_WareHouse.WareHouse.Domain;
using TSLib.Service;
using EaseErp_WareHouse.WareHouse.Dao;

namespace EaseErp_WareHouse.WareHouse.Service
{
    public class ThingService:BaseService<Thing>,IThingService
    {
        private IThingDao thingDao;         // Dao
        public IThingDao ThingDao
        {
            set
            {
                thingDao = value;
                base.BaseDao = value;
            }
        } 
    }
}
