using System;
using System.Collections.Generic;
using System.Text;
using Common.Logging;
using TSLib.Dao.Hibernate;
using TSCommon_Core.Organize.Domain;
using System.Collections;

namespace TSCommon_Core.Organize.Dao.Hibernate
{
    public class JobTitleDao : BaseDao<JobTitle>, IJobTitleDao
    {
        private static ILog logger = LogManager.GetLogger(typeof(JobTitleDao));
        protected override string DefaultQueryString
        {
            get { return "from JobTitle _alias order by _alias.Level, _alias.Code"; }
        }

        #region IJobTitleDao ≥…‘±

        public JobTitle LoadByCode(string jobTitleCode)
        {
            string hql = "from JobTitle _alias where _alias.Code = ?";
            return this.FindUnique(hql, new object[] { jobTitleCode });
        }

        public IList FindAllByLevel(string[] levels)
        {
            if (levels == null || levels.Length == 0) return new ArrayList();

            StringBuilder hql = new StringBuilder("from JobTitle _alias where _alias.Level in ( ?");
            for (int i = 1; i < levels.Length; i++)
            {
                hql.Append(", ?");
            }
            hql.Append(" ) order by _alias.Level, _alias.Code");
            return base.HibernateTemplate.Find(hql.ToString(), levels);
        }

        public bool IsUnique(JobTitle jobTitle)
        {
            string hql = "from JobTitle _alias where _alias.ID != ? and (_alias.Code = ? or _alias.Name = ?)";
            if (logger.IsDebugEnabled)
            {
                logger.Debug("hql=" + hql);
                logger.Debug("jobTitle.ID=" + jobTitle.ID);
                logger.Debug("jobTitle.Code=" + jobTitle.Code);
                logger.Debug("jobTitle.Name=" + jobTitle.Name);
            }
            return this.IsUnique(hql, new object[] { jobTitle.ID, jobTitle.Code, jobTitle.Name });
        }

        #endregion
    }
}
