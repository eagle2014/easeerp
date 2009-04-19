using System;
using System.Collections.Generic;
using System.Text;
using EaseErp.IC.Domain;
using TSLib.Service;
using EaseErp.IC.Dao;

namespace EaseErp.IC.Service
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
