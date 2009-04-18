using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Dao.Hibernate;
using System.Collections;

namespace TSCommon_Core.ATM.Dao.Hibernate
{
    public class ATMDao : BaseDao<ATM.Domain.ATM>, IATMDao
    {
        #region IATMDao 成员

        public IList GetATM(string parentUnid)
        {
            string hql = "from ATM atm where atm.ParentUnid=?";
            return this.HibernateTemplate.Find(hql, parentUnid);
        }

        public IList GetATM(string parentUnid, string type)
        {
            string hql = "from ATM atm where atm.ParentUnid=?";
            if (!string.IsNullOrEmpty(type))
            {
                hql += "and atm.Type=?";
                return this.HibernateTemplate.Find(hql, new object[] { parentUnid, type });
            }
            else
                return this.HibernateTemplate.Find(hql, parentUnid);
        }

        public void DeleteAll(IList list)
        {
            base.Delete(list);
        }

        public void DeleteAll(string parentUnid)
        {
            IList list = this.GetATM(parentUnid);
            this.Delete(list);
        }

        public void DeleteAll(string parent, string type)
        {
            IList list = this.GetATM(parent, type);
            this.Delete(list);
        }

        public TSLib.PageInfo GetPage(int pageNo, int pageSize, string sortField, string sortDir, string parent, string type)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
