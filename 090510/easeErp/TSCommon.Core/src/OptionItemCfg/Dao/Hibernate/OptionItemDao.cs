using System;
using System.Collections.Generic;
using System.Text;
using TSCommon.Core.OptionItemCfg.Domain;
using TSLib.Dao.Hibernate;
using System.Collections;
using TSLib;
using TSLib.DBUtils;

namespace TSCommon.Core.OptionItemCfg.Dao.Hibernate
{
    public class OptionItemDao:BaseDao<OptionItem>,IOptionItemDao
    {
        #region IOptionItemDao 成员

        public IList FindAll(string type)
        {
            // 组合查询语句
            StringBuilder hql = new StringBuilder();
            hql.Append(" from OptionItem optionItem ");
            hql.Append(" where optionItem.Type = ? ");
            hql.Append(" order by optionItem.FileDate");

            // 进行查询
            return this.HibernateTemplate.Find(hql.ToString(), type);
        }

        new public PageInfo GetPage(int firstNo, int maxResult, string sortField, string sortDir)
        {
            // 组合查询语句
            StringBuilder hql = new StringBuilder();
            hql.Append(" from OptionItem optionItem ");

            // 进行查询
            return NHibernateHelper.GetPage(this.Session, firstNo, maxResult, sortField, sortDir,
                                            "optionItem", hql.ToString(), null, "Name");
        }

        public PageInfo GetPage(int firstNo, int maxResult, string sortField, string sortDir, string type)
        {
            // 组合查询语句
            StringBuilder hql = new StringBuilder();
            IList args = null;
            hql.Append(" from OptionItem optionItem ");
            if (!string.IsNullOrEmpty(type))
            {
                hql.Append(" where optionItem.Type = ? ");
                args = new ArrayList();
                args.Add(type);
            }

            // 进行查询
            return NHibernateHelper.GetPage(this.Session, firstNo, maxResult, sortField, sortDir,
                                            "optionItem", hql.ToString(), new object[] { args }, "Name");
        }

        public IList FindAllType()
        {
            // 组合查询条件
            string hql = "select distinct optionItem.Type, optionItem.TypeName ";
            hql += "from OptionItem optionItem ";
            return this.HibernateTemplate.Find(hql);
        }

        #endregion
    }
}
