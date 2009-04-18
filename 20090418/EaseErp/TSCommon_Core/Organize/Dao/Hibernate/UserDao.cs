/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Dao.Hibernate;
using Common.Logging;
using System.Collections;
using TSLib;
using TSLib.Utils;
using TSLib.DBUtils;
using TSCommon_Core.Organize.Domain;

namespace TSCommon_Core.Organize.Dao.Hibernate
{
    /// <summary>
    /// 人员配置信息Dao Hibernate的实现
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-05-31</date>
    public class UserDao : BaseDao<User>, IUserDao
    {
        private static ILog logger = LogManager.GetLogger(typeof(UserDao));
        protected override string DefaultQueryString
        {
            get { return "from User _alias order by _alias.OrderNo"; }
        }

        #region IUserInfoDao 成员

        public IList FindByOU(string ouUnid)
        {
            return FindByOU(ouUnid, null);
        }

        public IList FindByOU(string ouUnid, string userType)
        {
            IList args = new ArrayList();
            StringBuilder hql = new StringBuilder("from User userInfo");
            hql.Append(" where (userInfo.UserStatus = ? or userInfo.UserStatus = ? ) ");
            args.Add(UserStatuses.Enable);
            args.Add(UserStatuses.Disable);
            hql.Append(" and userInfo.OUUnid = ?");
            args.Add(ouUnid);
            if (!string.IsNullOrEmpty(userType))
            {
                hql.Append(" and userInfo.UserType = ?");
                args.Add(userType);
            }
            hql.Append(" order by userInfo.OrderNo");
            return this.HibernateTemplate.Find(hql.ToString(), ListUtils.ListToObjectArray(args));
        }

        public IList FindByOU(string[] ouUnids, string userType)
        {
            IList args = new ArrayList();
            StringBuilder hql = new StringBuilder("from User userInfo");
            hql.Append(" where (userInfo.UserStatus = ? or userInfo.UserStatus = ? ) ");
            args.Add(UserStatuses.Enable);
            args.Add(UserStatuses.Disable);

            if (ouUnids != null && ouUnids.Length > 0)
            {
                hql.Append(" and ( ");
                bool isFirst = true;
                for (int i = 0; i < ouUnids.Length; i++)
                {
                    if (!string.IsNullOrEmpty(ouUnids[i]))
                    {
                        if (isFirst)
                            hql.Append(" userInfo.OUUnid = ? ");
                        else
                            hql.Append(" or userInfo.OUUnid = ? ");
                        args.Add(ouUnids[i]);
                        isFirst = false;
                    }
                }
                hql.Append(" )");
            }

            if (!string.IsNullOrEmpty(userType))
            {
                hql.Append(" and userInfo.UserType = ?");
                args.Add(userType);
            }
            hql.Append(" order by userInfo.OrderNo");
            if (logger.IsDebugEnabled)
                logger.Debug("hql=" + hql.ToString());
            return this.HibernateTemplate.Find(hql.ToString(), ListUtils.ListToObjectArray(args));
        }

        public IList FindByGroup(string groupUnid)
        {
            return FindByGroup(groupUnid, null);
        }

        public IList FindByGroup(string groupUnid, string userType)
        {
            IList args = new ArrayList();
            StringBuilder hql = new StringBuilder("select userInfo ");
            hql.Append("from User userInfo, RelationShip relationShip ");
            hql.Append("where relationShip.ParentUnid = ? and relationShip.ChildType = ? ");
            hql.Append("and relationShip.ChildUnid = userInfo.Unid ");
            args.Add(groupUnid);
            args.Add(User.RELATIONSHIP_CODE);
            if (!string.IsNullOrEmpty(userType))
            {
                hql.Append(" and userInfo.UserType = ?");
                args.Add(userType);
            }
            hql.Append("order by userInfo.OrderNo");
            return this.HibernateTemplate.Find(hql.ToString(), ListUtils.ListToObjectArray(args));
        }

        public PageInfo GetPageByOU(string ouUnid, int pageNo, int pageSize, string sortField, string sortDir)
        {
            if (string.IsNullOrEmpty(ouUnid))
            {

                string hql = "from User userInfo";
                //hql += "userInfo.UserStatus = ? or userInfo.UserStatus = ?";
                //new object[] { UserStatuses.Enable, UserStatuses.Disable }
                return NHibernateHelper.GetPage(this.Session, pageNo, pageSize, sortField, sortDir,
                    "userInfo", hql, (object[])null, null);
            }
            else
            {
                string hql = "from User userInfo where userInfo.OUUnid = ?";
                //string hql = "from UserInfo userInfo where userInfo.UserStatus = ? and userInfo.OUUnid = ?";
                //new object[] { UserStatuses.Enable, ouUnid }
                return NHibernateHelper.GetPage(this.Session, pageNo, pageSize, sortField, sortDir,
                    "userInfo", hql, new object[] { ouUnid } , null);
            }
        }

        public User LoadByLoginID(string loginID)
        {
            string hql = "from User userInfo ";
            hql += " where userInfo.UserStatus = ? and userInfo.LoginID = ? ";
            return this.FindUnique(hql, new object[] { UserStatuses.Enable, loginID });
        }

        public bool IsUnique(User userInfo)
        {
            string hql = "from User userInfo where userInfo.ID != ? and userInfo.LoginID = ?";
            return this.IsUnique(hql, new object[] { userInfo.ID, userInfo.LoginID });
        }

        public IList FindAllWithOutMessageUser(string ouUnid, string userUnid)
        {
            string hql = "from User userInfo ";
            hql += " where (userInfo.UserStatus = ? or userInfo.UserStatus = ? ) ";
            hql += " and userInfo.OUUnid = ? and userInfo.Unid not in (select messageGroupUser.UserUnid from MessageGroupUser messageGroupUser where messageGroupUser.GroupUnid in (select messageGroup.Unid from MessageGroup messageGroup where messageGroup.UserUnid = ?))";
            return this.HibernateTemplate.Find(hql, new object[] { UserStatuses.Enable, UserStatuses.Disable, ouUnid, userUnid });
        }

        public IList FindAllByTelephone(string telephoneNo)
        {
            string hql = "from User userInfo ";
            hql += " where userInfo.UserStatus = ? and userInfo.TelephoneNo like ? ";
            return this.HibernateTemplate.Find(hql, new object[] { UserStatuses.Enable, telephoneNo + "%" });
        }

        public IList FindAllByLoginID(string loginID)
        {
            string hql = "from User userInfo ";
            hql += " where userInfo.UserStatus = ? and userInfo.LoginID like ? ";
            return this.HibernateTemplate.Find(hql, new object[] { UserStatuses.Enable, loginID + "%" });
        }

        public IList FindAllByName(string name)
        {
            string hql = "from User userInfo ";
            hql += " where userInfo.UserStatus = ? and userInfo.Name like ? ";
            return this.HibernateTemplate.Find(hql, new object[] { UserStatuses.Enable, name + "%" });
        }

        public User GetUserInfoByEmployeeID(string employeeID)
        {
            string hql = "from User userInfo where userInfo.EmployeeID = ?";
            IList list = this.HibernateTemplate.Find(hql, employeeID);
            if (list == null || list.Count == 0)
                return null;
            return (User)list[0];
        }

        public string GetAllEmployeeID()
        {
            string employeeID = "";
            string hql = "from User userInfo where userInfo.UserStatus = ?";
            IList list = this.HibernateTemplate.Find(hql, UserStatuses.Enable);
            for (int i = 0; i < list.Count; i++)
            {
                User userInfo = (User)list[i];
                if (userInfo.EmployeeID == null)
                    continue;
                if (employeeID.Equals(""))
                    employeeID = userInfo.EmployeeID;
                else
                    employeeID = employeeID + "," + userInfo.EmployeeID;
            }
            return employeeID;
        }

        public IList FindByUnids(string[] unids)
        {
            if (unids == null || unids.Length == 0)
                return new ArrayList();

            IList args = new ArrayList();
            StringBuilder hql = new StringBuilder("from User _alias where _alias.Unid in (");
            int i = 0;
            foreach (string unid in unids)
            {
                args.Add(unid);
                hql.Append(i == 0 ? " ?" : " ,?");
                i++;
            }
            hql.Append(" ) order by _alias.OrderNo");
            if (logger.IsDebugEnabled)
                logger.Debug("hql=" + hql.ToString());
            return this.HibernateTemplate.Find(hql.ToString(), ListUtils.ListToObjectArray(args));
        }

        #endregion

        #region IUserDao 成员

        /// <summary>
        /// 根据关系的parentUnid返回用户列表
        /// </summary>
        /// <param name="relationShipParentUnid"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PageInfo FindAllByRelationShipParentUnid(string relationShipParentUnid, int pageNo, int pageSize)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select user ");
            sb.Append("from User user, RelationShip relationShip ");
            sb.Append("where (relationShip.ParentUnid = ? and relationShip.ChildType = ? and user.Unid = relationShip.ChildUnid ) ");
            sb.Append("or (relationShip.ChildUnid = ? and relationShip.ParentType = ? and user.Unid = relationShip.ParentUnid) ");
            sb.Append("order by user.FileDate desc");
            object[] values = new object[] { relationShipParentUnid, User.RELATIONSHIP_CODE, relationShipParentUnid, User.RELATIONSHIP_CODE };
            try
            {
                PageInfo pageInfo = NHibernateHelper.GetPage(this.Session, pageNo, pageSize, sb.ToString(), values);
                return pageInfo;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}
