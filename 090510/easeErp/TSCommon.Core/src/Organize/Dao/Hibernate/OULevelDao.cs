using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Dao.Hibernate;
using TSCommon.Core.Organize.Domain;
using Common.Logging;
using System.Collections;
using TSLib.Utils;

namespace TSCommon.Core.Organize.Dao.Hibernate
{
    public class OULevelDao : BaseDao<OULevel>, IOULevelDao
    {
        private static ILog logger = LogManager.GetLogger(typeof(OULevelDao));

        protected override string DefaultQueryString
        {
            get { return "from OULevel _alias order by _alias.Code "; }
        }

        public OULevel LoadByCode(string code)
        {
            string hql = "from OULevel ouLevel where ouLevel.Code = ? ";
            return this.FindUnique(hql, new object[] { code });
        }

        public bool IsUnique(OULevel ouLevel)
        {
            string hql = "from OULevel ouLevel where ouLevel.ID != ? and ( ouLevel.Name = ? or ouLevel.Code = ? )";
            return this.IsUnique(hql, new object[] { ouLevel.ID, ouLevel.Name, ouLevel.Code });
        }

        public IList FindByCodes(string[] codes)
        {
            if (codes == null || codes.Length == 0)
                return new ArrayList();

            IList args = new ArrayList();
            StringBuilder hql = new StringBuilder("from OULevel _alias where _alias.Code in (");
            int i = 0;
            foreach (string code in codes)
            {
                args.Add(code);
                hql.Append(i == 0 ? " ?" : " ,?");
                i++;
            }
            hql.Append(" ) order by _alias.Code");
            if (logger.IsDebugEnabled)
                logger.Debug("hql=" + hql.ToString());
            return this.HibernateTemplate.Find(hql.ToString(), ListUtils.ListToObjectArray(args));
        }
    }
}
