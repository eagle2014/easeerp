using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Dao.Hibernate;
using TSCommon.Core.SystemPolicy.Domain;
using Common.Logging;

namespace TSCommon.Core.SystemPolicy.Dao.Hibernate
{
    /// <summary>
    /// 系统策略DAO Hibernate的实现
    /// </summary>
    public class PolicyDao : BaseDao<Policy>, IPolicyDao
    {
        private static ILog logger = LogManager.GetLogger(typeof(PolicyDao));

        protected override string DefaultQueryString
        {
            get { return "from Policy _alias order by _alias.BelongModule, _alias.OrderNo "; }
        }

        #region IPolicyDao 成员

        public Policy LoadByCode(string code)
        {
            string hql = "from Policy policy where policy.Code = ? ";
            return this.FindUnique(hql, new object[] { code });
        }

        public bool IsUnique(Policy policy)
        {
            string hql = "from Policy policy where policy.ID != ? and policy.Code = ? ";
            return this.IsUnique(hql, new object[] { policy.ID, policy.Code });
        }

        #endregion
    }
}
