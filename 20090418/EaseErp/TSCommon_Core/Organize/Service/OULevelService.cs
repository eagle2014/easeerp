using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using TSCommon_Core.Organize.Domain;
using System.Collections;
using TSCommon_Core.Organize.Dao;
using TS.Exceptions;

namespace TSCommon_Core.Organize.Service
{
    public class OULevelService: BaseService<OULevel>, IOULevelService
    {
        private IOULevelDao ouLevelDao;
        /// <summary>
        /// 级别的Dao
        /// </summary>
        public IOULevelDao OULevelDao
        {
            get { return this.ouLevelDao; }
            set
            {
                this.ouLevelDao = value;
                base.BaseDao = value;
            }
        }

        #region IOULevelService 成员

        public override void Save(OULevel ouLevel)
        {
            if (!this.ouLevelDao.IsUnique(ouLevel))
                throw new ResourceException("OULEVEL.EXCEPTION.NOT_UNIQUE");
            this.ouLevelDao.Save(ouLevel);
        }
        public override void Save(IList objs)
        {
            foreach (OULevel ouLevel in objs)
            {
                this.Save(ouLevel);
            }
        }

        public IList FindByLevels(string[] levels)
        {
            return this.ouLevelDao.FindByCodes(levels);
        }

        #endregion
    }
}
