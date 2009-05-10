using System;
using System.Collections.Generic;
using System.Text;
using EaseErp.IC.Domain;
using TSLib.Service;
using TSLib;
using TSLib.Utils;
using EaseErp.IC.Dao;

namespace EaseErp.IC.Service
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
