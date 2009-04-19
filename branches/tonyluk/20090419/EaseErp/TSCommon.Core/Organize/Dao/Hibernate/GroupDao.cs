/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSLib.Dao.Hibernate;
using Common.Logging;
using TSLib;
using TSLib.DBUtils;
using TSLib.Utils;
using TSCommon_Core.Organize.Domain;

namespace TSCommon_Core.Organize.Dao.Hibernate
{
    /// <summary>
    /// 岗位配置Dao的NHbernate的实现
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-05-31</date>
    public class GroupDao : BaseDao<Group>, IGroupDao
    {
        private ILog logger = LogManager.GetLogger(typeof(GroupDao));

        protected override string DefaultQueryString
        {
            get { return "from Group _alias order by _alias.Code"; }
        }


        #region IGroupDao 成员

        public override IList FindAll()
        {
            string hql = "from Group g where g.GroupStatus = ? order by g.Code";
            return this.HibernateTemplate.Find(hql, GroupStatuses.Enable);
        }

        public IList FindByOU(string ouUnid)
        {
            return this.FindByOU(new string[] { ouUnid });
        }

        public IList FindAllSendTo(string ouUnid)
        {
            string hql = "from Group g where g.GroupStatus = ? and g.OUUnid = ? and g.IsCanDispatch = ? order by g.Code";
            return this.HibernateTemplate.Find(hql, new object[] { GroupStatuses.Enable, ouUnid, Constants.YESNO_YES });
        }

        public IList FindAllSendTo(string[] ouUnid)
        {
            IList argList = new ArrayList();
            string hql = "from Group g where g.GroupStatus = ? ";
            argList.Add(GroupStatuses.Enable);
            for (int i = 0; i < ouUnid.Length; i++)
            {
                if (i == 0)
                    hql += "and (";
                else
                    hql += "or ";
                hql += "g.OUUnid = ? ";
                argList.Add(ouUnid[i]);
            }
            if (argList.Count > 1)
                hql += ") ";
            hql += "and g.IsCanDispatch = ? order by g.Code";
            argList.Add(Constants.YESNO_YES);
            if (logger.IsDebugEnabled)
                logger.Debug("hql: " + hql);
            return this.HibernateTemplate.Find(hql, ListUtils.ListToObjectArray(argList));
        }

        public IList FindByOU(string[] ouUnids)
        {
            return this.FindByOU(ouUnids, false);
        }

        public IList FindByOU(string[] ouUnids, bool isCanDispatch)
        {
            return FindByOU(ouUnids, isCanDispatch ? "1" : "2");
        }

        public IList FindByName(string ouUnid, string groupName)
        {
            string hql = "from Group g where g.OUUnid = ? and g.Name = ?";
            return NHibernateHelper.Find(this.Session, hql, new object[] { ouUnid, groupName });
        }

        public IList FindByOU(string[] ouUnids, string groupType)
        {
            if (null == ouUnids || ouUnids.Length == 0)
                return new ArrayList();
            IList argsList = new ArrayList();
            string hql = "from Group g where g.GroupStatus = ? ";
            argsList.Add(GroupStatuses.Enable);
            if ("1" == groupType)       // 可派单岗位
            {
                hql += " and g.IsCanDispatch = ? ";
                argsList.Add(Constants.YESNO_YES);
            }
            else if ("2" == groupType) // 不可派单岗位
            {
                hql += " and g.IsCanDispatch = ? ";
                argsList.Add(Constants.YESNO_NO);
            }
            hql += " and ( ";
            bool isFirst = true;
            for (int i = 0; i < ouUnids.Length; i++)
            {
                if (!string.IsNullOrEmpty(ouUnids[i]))
                {
                    if (isFirst)
                        hql += " g.OUUnid = ? ";
                    else
                        hql += " or g.OUUnid = ? ";
                    argsList.Add(ouUnids[i]);
                    isFirst = false;
                }
            }
            hql += " ) order by g.Code";
            return this.HibernateTemplate.Find(hql, ListUtils.ListToObjectArray(argsList));
        }

        public IList FindByUser(string userUnid)
        {
            string hql = "select g ";
            hql += "from Group g, RelationShip relationShip ";
            hql += "where relationShip.ChildUnid = ? and relationShip.ParentType = ? ";
            hql += "and relationShip.ParentUnid = g.Unid ";
            hql += "order by g.Code";
            return this.HibernateTemplate.Find(hql, new object[] { userUnid, Group.RELATIONSHIP_CODE });
        }

        public PageInfo GetPageByOU(string ouUnid, int pageNo, int pageSize, string sortField, string sortDir)
        {
            if (string.IsNullOrEmpty(ouUnid))
            {

                string hql = "from Group g where g.GroupStatus = ?";
                return NHibernateHelper.GetPage(this.Session, pageNo, pageSize, sortField, sortDir, "g", hql, new object[] { GroupStatuses.Enable },null);
            }
            else
            {
                string hql = "from Group g where g.GroupStatus = ? and g.OUUnid = ?";
                return NHibernateHelper.GetPage(this.Session, pageNo, pageSize, sortField, sortDir, "g", hql, new object[] { GroupStatuses.Enable, ouUnid },null);
            }
        }

        public bool IsUnique(Group group)
        {
            string hql = "from Group g where g.ID != ? and g.Code = ? ";
            return this.IsUnique(hql, new object[] { group.ID, group.Code });
        }

        private string sysDefaultGroupCode = "Sys_DefaultGroup";
        /// <summary>
        /// 获取或设置系统默认岗位的编码的值
        /// </summary>
        public string SysDefaultGroupCode
        {
            get { return sysDefaultGroupCode; }
            set { sysDefaultGroupCode = value; }
        }

        public Group GetSysDefaultGroup()
        {
            string hql = "from Group g where g.Code = ?";
            IList list = this.HibernateTemplate.Find(hql, sysDefaultGroupCode);
            if (list != null && list.Count > 0)
            {
                if (list.Count > 1) logger.Error("找到多个编码为“" + sysDefaultGroupCode + "”的岗位，仅返回第一个！");
                return (Group)list[0];
            }
            else
            {
                logger.Error("系统没有配置编码为“" + sysDefaultGroupCode + "”的岗位！");
                return null;
            }
        }

        public bool DoHaveGroupByCode(string code,string ouCode)
        {
            string hql = "from Group g where g.Code = ? and g.OUCode = ?";
            IList list = this.HibernateTemplate.Find(hql, new object[] { code, ouCode });
            if (list == null || list.Count <= 0)
                return false;
            return true;
        }

        public Group LoadByCode(string code)
        {
            string hql = "from Group g where g.Code = ?";
            return (Group)NHibernateHelper.FindUnique(this.Session, hql, new object[] { code });
        }

        #endregion
    }
}
