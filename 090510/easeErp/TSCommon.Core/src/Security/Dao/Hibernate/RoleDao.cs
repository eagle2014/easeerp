/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Common.Logging;
using TSLib.Dao.Hibernate;
using TSLib;
using TSCommon.Core.Security.Domain;
using TSCommon.Core.Organize.Domain;

namespace TSCommon.Core.Security.Dao.Hibernate
{
    /// <summary>
    /// 角色Dao的Hibernate的实现
    /// </summary>
    /// <author>Tony</author>
    /// <author>Tony</author>
    /// <date>2008-06-01</date>
    public class RoleDao : BaseDao<Role>, IRoleDao
    {
        private static ILog logger = LogManager.GetLogger(typeof(RoleDao));

        protected override string DefaultQueryString
        {
            get { return "from Role _alias order by _alias.Code "; }
        }

        #region IRoleDao 成员

        public PageInfo GetPage(int level, int pageNo, int pageSize, string sortField, string sortDir)
        {
            if (level == -1)
            {
                string hql = "from Role role order by role.Code";
                return this.GetPage(pageNo, pageSize, sortField, sortDir, hql, null);
            }
            else
            {
                string hql = "from Role role where role.Level >= ? order by role.Code";
                return this.GetPage(pageNo, pageSize, sortField, sortDir, hql, new object[] { level });
            }
        }

        public IList FindByLevel(string level, bool hasChild)
        {
            string hql = "from Role role ";
            if(hasChild)
                hql += "where role.Level >= ? ";
            else
                hql += "where role.Level = ? ";
            hql += "order by role.Code";
            return this.HibernateTemplate.Find(hql, level);
        }

        public IList FindByGroup(string grouUnid)
        {
            string hql = "select role ";
            hql += "from Role role, RelationShip relationShip ";
            hql += "where relationShip.ParentUnid = ? and relationShip.ChildType = ? ";
            hql += "and relationShip.ChildUnid = role.Unid ";
            hql += "order by role.Code";
            return this.HibernateTemplate.Find(hql, new object[]{ grouUnid, Role.RELATIONSHIP_CODE });
        }

        public IList FindByUser(string userInfoUnid)
        {
            string hql = "select role ";
            hql += "from Role role, RelationShip r1, RelationShip r2 ";
            hql += "where r1.ChildUnid = ? and r1.ParentType = ? ";
            hql += "and r1.ParentUnid = r2.ParentUnid and r2.ChildType = ? ";
            hql += "and r2.ChildUnid = role.Unid ";
            hql += "order by role.Code";
            return this.HibernateTemplate.Find(hql, new object[] { userInfoUnid, Group.RELATIONSHIP_CODE, Role.RELATIONSHIP_CODE });
        }

        public Role LoadByCode(string code)
        {
            string hql = "from Role role where role.Code = ?";
            return this.FindUnique(hql, new object[] { code });
        }

        public bool IsUnique(Role role)
        {
            string hql = "from Role role where role.ID != ? and (role.Code = ? or role.Name = ?)";
            return this.IsUnique(hql, new object[] { role.ID, role.Code, role.Name });
        }

        #endregion
    }
}
