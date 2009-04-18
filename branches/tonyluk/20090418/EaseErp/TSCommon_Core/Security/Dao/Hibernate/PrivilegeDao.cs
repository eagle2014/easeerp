/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 * 
 * Autor: CD826         2007-02-27
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Common.Logging;
using TSLib.Dao.Hibernate;
using TSCommon_Core.Security.Domain;

namespace TSCommon_Core.Security.Dao.Hibernate
{
    /// <summary>
    /// 权限Dao接口的NHibernate实现
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-06-01</date>
    public class PrivilegeDao : BaseDao<Privilege>, IPrivilegeDao
    {
        private static ILog logger = LogManager.GetLogger(typeof(PrivilegeDao));

        protected override string DefaultQueryString
        {
            get { return "from Privilege _alias order by _alias.OrderNo "; }
        }

        #region IPrivilegeDao 成员

        public IList FindByType(string privilegeType)
        {
            string hql = "from Privilege privilege where privilege.Type = ? order by privilege.OrderNo";
            return this.HibernateTemplate.Find(hql, privilegeType);
        }

        public IList FindByModel(long modelID)
        {
            string hql = "from Privilege privilege where privilege.Model.ID = ? order by privilege.OrderNo";
            return this.HibernateTemplate.Find(hql, modelID);
        }

        public IList FindByModelAndType(long modelID, string type)
        {
            string hql = "from Privilege privilege where privilege.Model.ID = ? and privilege.Type = ? order by privilege.OrderNo";
            return this.HibernateTemplate.Find(hql, new object[] { modelID, type });
        }

        public IList FindByModelAndType(string modelUnid, string type)
        {
            if (string.IsNullOrEmpty(modelUnid)) return new ArrayList();

            string hql = "from Privilege privilege where privilege.Model is not null and privilege.Model.Unid = ?";
            object[] args;
            if (!string.IsNullOrEmpty(type))
            {
                hql += " and privilege.Type = ?";
                args = new object[] { modelUnid, type };
            }
            else
            {
                args = new object[] { modelUnid };
            }
            hql += " order by privilege.OrderNo";
            return this.HibernateTemplate.Find(hql, args);
        }

        public Privilege LoadByCode(string code)
        {
            string hql = "from Privilege privilege where privilege.Code = ?";
            return this.FindUnique(hql, new object[] { code });
        }

        public bool IsUnique(Privilege privilege)
        {
            string hql = "from Privilege privilege where privilege.ID != ? and privilege.Code = ?";
            return this.IsUnique(hql, new object[] { privilege.ID, privilege.Code });
        }

        #endregion
    }
}
