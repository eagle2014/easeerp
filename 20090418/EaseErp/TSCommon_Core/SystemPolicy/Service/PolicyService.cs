using System;
using System.Collections.Generic;
using System.Text;
using TSCommon_Core.SystemPolicy.Domain;
using TSLib.Service;
using Common.Logging;
using TSCommon_Core.SystemPolicy.Dao;
using TSCommon_Core.Organize.Domain;
using System.Collections;
using Spring.Caching;

namespace TSCommon_Core.SystemPolicy.Service
{
    public class PolicyService : BaseService<Policy>, IPolicyService
    {
        private static ILog logger = LogManager.GetLogger(typeof(PolicyService));

        private IPolicyDao policyDao;           // 系统策略DAO
        public IPolicyDao PolicyDao
        {
            set
            {
                policyDao = value;
                BaseDao = value;
            }
        }

        private ICache cacheManager;
        public ICache CacheManager
        {
            set { cacheManager = value; }
        }

        #region IPolicyService 成员

        public Policy LoadByCode(string code)
        {
            return this.policyDao.LoadByCode(code);
        }

        public void Save(User user, Policy policy)
        {
            if (null != user)
            {
                policy.SetLastModifiedInfo(user);
                policy.FileDate = DateTime.Now;
            }
            this.policyDao.Save(policy);

            //更新缓存
            //this.UpdateCache();
        }

        public override void Delete(long id)
        {
            this.policyDao.Delete(id);
        }

        public override void Delete(IList systemPolicyList)
        {
            this.policyDao.Delete(systemPolicyList);
        }

        public override void Delete(long[] ids)
        {
            this.policyDao.Delete(ids);
        }

        public override void Delete(string[] unids)
        {
            this.policyDao.Delete(unids);
        }
        #endregion
    }
}
