/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using Spring.Data.NHibernate.Support;
using System.Collections;
using NHibernate;
using TSLib.DBUtils;
using NHibernate.Type;
using TSLib.Utils;
using TSCommon.Core.Organize.Domain;
using TSLib.Dao.Hibernate;

namespace TSCommon.Core.Organize.Dao.RelationShips
{
    /// <summary>
    /// 关联关系DAO Hibernate的实现
    /// <author>CD826</author>
    /// </summary>
    public class RelationShipDao :BaseDao<RelationShip>, IRelationShipDao
    {
        #region IRelationShipDao 成员

        public RelationShip Get(string parentUnid, string childUnid)
        {
            string hql = "from RelationShip relationShip where relationShip.ParentUnid = ? and relationShip.ChildUnid = ? ";
            return (RelationShip)NHibernateHelper.FindUnique(this.Session, hql, new object[] { parentUnid, childUnid });
        }

        public IList GetByChild(string childUnid, string type)
        {
            string hql = "from RelationShip _alias where _alias.ChildUnid = ?  and _alias.RelationShipType = ? ";
            return NHibernateHelper.Find(this.Session, hql, new object[] { childUnid, type });
        }

        public IList GetByParent(string parentUnid, string type)
        {
            string hql = "from RelationShip _alias where _alias.ParentUnid = ?  and _alias.RelationShipType = ? ";
            return NHibernateHelper.Find(this.Session, hql, new object[] { parentUnid, type });
        }

        public void Delete(string parentUnid, string childUnid)
        {
            string hql = "from RelationShip relationShip where relationShip.ParentUnid = ? and relationShip.ChildUnid = ? ";
            this.HibernateTemplate.Delete(hql, new object[] { parentUnid, childUnid },
                new NHibernate.Type.IType[] { NHibernateUtil.String, NHibernateUtil.String });
        }

        public int Delete(string parentUnid, string[] childUnids)
        {
            if(childUnids == null || childUnids.Length == 0) return 0;

            IList args = new ArrayList();
            IList types = new ArrayList();
            string hql = "from RelationShip relationShip where relationShip.ParentUnid = ?";
            args.Add(parentUnid);
            types.Add(NHibernateUtil.String);
            hql += " and (relationShip.ChildUnid = ?";
            args.Add(childUnids[0]);
            types.Add(NHibernateUtil.String);
            for (int i = 1; i < childUnids.Length; i++)
            {
                hql += " or relationShip.ChildUnid = ?";
                args.Add(childUnids[i]);
                types.Add(NHibernateUtil.String);
            }
            hql += " )";

            IType[] _types = new IType[types.Count];
            types.CopyTo(_types,0);
            return this.HibernateTemplate.Delete(hql, ListUtils.ListToObjectArray(args), _types);
        }

        public void DeleteAllByParent(string parentUnid)
        {
            string hql = "from RelationShip relationShip where relationShip.ParentUnid = ? ";
            this.HibernateTemplate.Delete(hql, parentUnid, NHibernateUtil.String);
        }

        public void DeleteAllByParent(string parentUnid, string type)
        {
            string hql = "from RelationShip relationShip where relationShip.ParentUnid = ? and relationShip.RelationShipType = ? ";
            this.HibernateTemplate.Delete(hql, new object[] { parentUnid, type },
                new NHibernate.Type.IType[] { NHibernateUtil.String, NHibernateUtil.String });
        }

        public void DeleteAllByChild(string childUnid)
        {
            string hql = "from RelationShip relationShip where relationShip.ChildUnid = ? ";
            this.HibernateTemplate.Delete(hql, childUnid, NHibernateUtil.String);
        }

        public void DeleteAllByChild(string childUnid, string type)
        {
            string hql = "from RelationShip relationShip where relationShip.ChildUnid = ? and relationShip.RelationShipType = ? ";
            this.HibernateTemplate.Delete(hql, new object[] { childUnid, type },
                new NHibernate.Type.IType[] { NHibernateUtil.String, NHibernateUtil.String });
        }

        public void DeleteAll(string unid)
        {
            this.DeleteAll(unid, null);
        }

        public void DeleteAll(string unid, string otherType)
        {
            string hql = "from RelationShip relationShip ";
            object[] args = null;
            IType[] types = null;
            if (null != otherType && otherType.Length > 0)
            {
                hql += "where (relationShip.ParentUnid = ? and relationShip.ChildType = ? ) or ";
                hql += "(relationShip.ChildUnid = ? and relationShip.ParentType = ? )";
                args = new object[] { unid, otherType, unid, otherType };
                types = new IType[] { NHibernateUtil.String, NHibernateUtil.String, NHibernateUtil.String, NHibernateUtil.String };
            }
            else
            {
                hql += "where relationShip.ParentUnid = ? or relationShip.ChildUnid = ? ";
                args = new object[] { unid, unid };
                types = new IType[] { NHibernateUtil.String, NHibernateUtil.String };
            }
            this.HibernateTemplate.Delete(hql, args, types);
        }

        #endregion
    }
}
