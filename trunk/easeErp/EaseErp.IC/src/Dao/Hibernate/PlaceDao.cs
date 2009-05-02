using System;
using System.Collections.Generic;
using System.Text;
using EaseErp.IC.Domain;
using TSLib.Dao.Hibernate;
using System.Collections;
using TSLib.DBUtils;

namespace EaseErp.IC.Dao.Hibernate
{
    public class PlaceDao : BaseDao<Place>, IPlaceDao
    {
        #region IPlaceDao 成员

        public System.Collections.IList Load(long[] ids)
        {
            IList list = new ArrayList();
            foreach (long id in ids)
            {
                list.Add(base.Load(id));
            }
            return list;
        }

        public System.Collections.IList Load(string[] unids)
        {
            IList list = new ArrayList();
            foreach (string unid in unids)
            {
                list.Add(base.Load(unid));
            }
            return list;
        }

        #endregion

        #region IPlaceDao 成员


        public IList GetPlacesByParentUnid(string parentUnid)
        {
            string hql = "from Place place";
            if (!string.IsNullOrEmpty(parentUnid) && !parentUnid.Equals("-1") && !parentUnid.Equals("root"))
            {
                hql += " where place.Parent.Unid=?";
                return this.HibernateTemplate.Find(hql, parentUnid);
            }
            else
            {
                hql += " where place.Parent is null";
                return this.HibernateTemplate.Find(hql);
            }
        }

        #endregion

        #region IPlaceDao 成员


        public TSLib.PageInfo GetPageInfoByParentUnid(int pageNo, int pageSize, string sortField, string sortDir, string parentUnid)
        {
            string hql="from Place place";
            if (string.IsNullOrEmpty(parentUnid))
                return NHibernateHelper.GetPage(this.Session, pageNo, pageSize, hql, new object[] { });
            else 
            {
                hql += " where place.Parent.Unid=?";
                return NHibernateHelper.GetPage(this.Session, pageNo, pageSize, hql, new object[] { parentUnid });
            }
        }

        #endregion
    }
}
