using System;
using System.Collections.Generic;
using System.Text;
using TSCommon.Core.Organize.Dao;
using TSLib.Service;
using TSCommon.Core.Organize.Domain;
using System.Collections;
using TS.Exceptions;

namespace TSCommon.Core.Organize.Service
{
    public class JobTitleService : BaseService<JobTitle>, IJobTitleService
    {
        private IJobTitleDao jobTitleDao;
        /// <summary>
        /// 职务的DAO
        /// </summary>
        public IJobTitleDao JobTitleDao
        {
            set
            {
                jobTitleDao = value;
                base.BaseDao = value;
            }
        }

        #region IJobTitleService 成员

        public JobTitle LoadByCode(string jobTitleCode)
        {
            return this.jobTitleDao.LoadByCode(jobTitleCode);
        }

        public IList FindAllByLevel(string level)
        {
            if (string.IsNullOrEmpty(level)) return new ArrayList();

            return this.jobTitleDao.FindAllByLevel(new string[] { level });
        }

        public IList FindAllByLevel(string[] levels)
        {
            return this.jobTitleDao.FindAllByLevel(levels);
        }

        public override void Save(JobTitle jobTitle)
        {
            // 检查是否在数据中唯一
            if (!this.jobTitleDao.IsUnique(jobTitle))
                throw new ResourceException("JOBTITLE.EXCEPTION.HAD_EXIST", new string[] { jobTitle.Name, jobTitle.Code });
            this.jobTitleDao.Save(jobTitle);
        }

        public override void Save(IList jobTitles)
        {
            foreach (JobTitle jobTitle in jobTitles)
            {
                this.Save(jobTitle);
            }
        }

        #endregion
    }
}
