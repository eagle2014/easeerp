using System;
using System.Collections.Generic;
using System.Text;
using EaseErp_WareHouse.WareHouse.Domain;
using TSLib.Service;
using TSLib;
using TSLib.Utils;
using EaseErp_WareHouse.WareHouse.Dao;

namespace EaseErp_WareHouse.WareHouse.Service
{
    public class WHUnitService :BaseService<WHUnit>,IWHUnitService
    {
        private IWHUnitDao wHUnitDao;         // Dao
        public IWHUnitDao WHUnitDao
        {
            set
            {
                wHUnitDao = value;
                base.BaseDao = value;
            }
        }       
    }
}
