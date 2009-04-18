/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 * 
 * Autor: CD826         2007-02-26
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Common.Logging;
using TSLib.Dao.Hibernate;
using TSLib;
using TSLib.Utils;
using TSLib.DBUtils;
using TSCommon_Core.Organize.Domain;

namespace TSCommon_Core.Organize.Dao.Hibernate
{
    /// <summary>
    /// 组织架构Dao的NHibernate的实现
    /// </summary>
    public class OUInfoDao : BaseDao<OUInfo>, IOUInfoDao
    {
        private static ILog logger = LogManager.GetLogger(typeof(OUInfoDao));
        #region IOUInfoDao 成员

        /// <summary>
        /// 所有单位
        /// </summary>
        /// <returns></returns>
        public new IList FindAll()
        {
            // 如果ou所属的单位不是活动的，将不会包含在返回结果中
            string hql = "from OUInfo ou1";
            hql += " where ou1.OUStatus = ?";
            hql += " and ((ou1.UnitUnid is null or ou1.UnitUnid = '')";
            hql += " or (ou1.Unid in (select ou2.Unid from OUInfo ou2 where ou2.OUStatus = ? and ou2.Type = ?))";
            hql += " )";
            hql += " order by ou1.OrderNo,ou1.FullCode";
            logger.Debug("hql=" + hql);
            return this.HibernateTemplate.Find(hql, new object[] { OUStatuses.Enable, OUStatuses.Enable, OUInfo.OT_UNIT });

            //string hql = "from OUInfo ouInfo where ouInfo.OUStatus = ? order by ouInfo.OrderNo,ouInfo.FullCode";
            //return this.HibernateTemplate.Find(hql, OUStatuses.Enable);
        }

        public IList FindAllByType(string type)
        {
            string hql = "from OUInfo ouInfo where ouInfo.OUStatus = ? and ouInfo.Type = ? order by ouInfo.OrderNo,ouInfo.FullCode";
            return this.HibernateTemplate.Find(hql, new object[] { OUStatuses.Enable, type });
        }

        public IList FindChildByParentUnid(string parentUnid, string ouType)
        {
            IList args = new ArrayList();
            StringBuilder hql = new StringBuilder("from OUInfo ouInfo where ouInfo.OUStatus = ?");
            args.Add(OUStatuses.Enable);
            if (!string.IsNullOrEmpty(ouType))
            {
                hql.Append(" and ouInfo.Type = ?");
                args.Add(ouType);
            }
            if (!string.IsNullOrEmpty(parentUnid))
            {
                hql.Append(" and (ouInfo.ParentOUUnid = ?");// 下级单位或部门下的子部门
                hql.Append(" or ((ouInfo.ParentOUUnid is null or ouInfo.ParentOUUnid = '') and ouInfo.UnitUnid = ?))");// 单位下的直接部门
                args.Add(parentUnid);
                args.Add(parentUnid);
            }
            else
            {
                hql.Append(" and (ouInfo.ParentOUUnid is null or ouInfo.ParentOUUnid = '')");
            }

            hql.Append(" order by ouInfo.Type desc, ouInfo.OrderNo,ouInfo.FullCode");
            if (args.Count == 0) args = null;
            return this.HibernateTemplate.Find(hql.ToString(), ListUtils.ListToObjectArray(args));
        }

        public IList FindChildByParentUnid(string parentUnid, string ouType, bool includeChildOU)
        {
            IList args = new ArrayList();
            StringBuilder hql = new StringBuilder("from OUInfo ouInfo where ouInfo.OUStatus = ?");
            args.Add(OUStatuses.Enable);
            if (!string.IsNullOrEmpty(ouType))
            {
                hql.Append(" and ouInfo.Type = ?");
                args.Add(ouType);
            }
            OUInfo pou = null;
            if (!string.IsNullOrEmpty(parentUnid))
            {
                pou = this.Load(parentUnid);
                if (pou == null) return new ArrayList();
                if (includeChildOU) // 包含下级的下级OU
                {
                    hql.Append(" and (ouInfo.ParentOUUnid = ?");// 下级单位或部门下的子部门
                    hql.Append(" or ((ouInfo.ParentOUUnid is null or ouInfo.ParentOUUnid = '') and ouInfo.UnitUnid = ?)");// 单位下的直接部门
                    hql.Append(" or (ouInfo.FullCode like ? and ouInfo.UnitUnid = ?))");// 下级的下级OU
                    args.Add(parentUnid);
                    args.Add(parentUnid);
                    args.Add(pou.FullCode + ".%");
                    args.Add(OUInfo.OT_DEPARTMENT.Equals(pou.Type,StringComparison.OrdinalIgnoreCase) ? pou.UnitUnid : pou.Unid);
                }
                else
                {
                    hql.Append(" and (ouInfo.ParentOUUnid = ?");// 下级单位或部门下的子部门
                    hql.Append(" or ((ouInfo.ParentOUUnid is null or ouInfo.ParentOUUnid = '') and ouInfo.UnitUnid = ?))");// 单位下的直接部门
                    args.Add(parentUnid);
                    args.Add(parentUnid);
                }
            }
            else
            {
                hql.Append(" and (ouInfo.ParentOUUnid is null or ouInfo.ParentOUUnid = '')");
            }

            hql.Append(" order by ouInfo.Type desc, ouInfo.OrderNo,ouInfo.FullCode");
            if (args.Count == 0) args = null;
            IList list = this.HibernateTemplate.Find(hql.ToString(), ListUtils.ListToObjectArray(args));
            if (logger.IsDebugEnabled)
            {
                logger.Debug("parentOU.FullCode=" + (pou == null ? "null" : pou.FullCode));
                foreach (OUInfo ou in list)
                {
                    logger.Debug("childOU.FullCode=" + ou.FullCode);
                }
            }
            return list;
        }

        public IList FindChildByParentCode(string parentCode, string ouType)
        {
            object[] args = null;
            string hql = "from OUInfo ouInfo ";
            hql += " where ouInfo.FullCode like ? and ouInfo.OUStatus = ? ";
            if (!string.IsNullOrEmpty(ouType))
            {
                hql += " and ouInfo.Type = ? ";
                args = new object[] { parentCode + ".%", OUStatuses.Enable, ouType };
            }
            else
                args = new object[] { parentCode + ".%", OUStatuses.Enable };

            hql += " order by ouInfo.OrderNo,ouInfo.FullCode";
            return this.HibernateTemplate.Find(hql, args);
        }

        public IList FindByLevel(int level, bool hasChild)
        {
            string hql = "from OUInfo ouInfo ";
            if (hasChild)
                hql += " where ouInfo.Level >= ?";
            else
                hql += " where ouInfo.Level = ?";
            hql += " and ouInfo.OUStatus = ? ";
            hql += " order by ouInfo.OrderNo,ouInfo.FullCode";
            return this.HibernateTemplate.Find(hql, new object[] { level, OUStatuses.Enable });
        }
                
        public IList FindAllDepartment(string unitUnid)
        {
            string hql = "from OUInfo ouInfo where ouInfo.Type = ? ";
            hql += " and ouInfo.OUStatus = ? ";
            hql += " and ouInfo.UnitUnid = ? ";
            hql += " order by ouInfo.OrderNo,ouInfo.FullCode";
            return this.HibernateTemplate.Find(hql, new object[] { OUInfo.OT_DEPARTMENT, OUStatuses.Enable, unitUnid });
        }

        public OUInfo LoadByFullCode(string fullCode)
        {
            string hql = "from OUInfo ouInfo where ouInfo.FullCode = ?";
            hql += " and ouInfo.OUStatus = ? ";
            return this.FindUnique(hql, new object[] { fullCode, OUStatuses.Enable });
        }

        public bool IsUnique(OUInfo ouInfo)
        {
            string hql = "from OUInfo ouInfo where ouInfo.ID != ? and (ouInfo.FullCode = ? or ouInfo.FullName = ?)";
            if (logger.IsDebugEnabled)
            {
                logger.Debug("hql=" + hql);
                logger.Debug("ouInfo.ID=" + ouInfo.ID);
                logger.Debug("ouInfo.FullCode=" + ouInfo.FullCode);
                logger.Debug("ouInfo.FullName=" + ouInfo.FullName);
            }
            return this.IsUnique(hql, new object[] { ouInfo.ID, ouInfo.FullCode, ouInfo.FullName });
        }

        public PageInfo GetPageByType(string type, int pageNo, int pageSize, string sortField, string sortDir, object[] ouStatuses)
        {
            StringBuilder hql = new StringBuilder("from OUInfo _alias where 1=1");
            ArrayList args = new ArrayList();
            if (!string.IsNullOrEmpty(type))
            {
                hql.Append(" and _alias.Type = ?");
                args.Add(type);
            }
            expandStatusCondition(ouStatuses, hql, args);
            if (string.IsNullOrEmpty(sortField))
            {
                hql.Append(" order by _alias.OrderNo, _alias.FullCode");
            }
            return NHibernateHelper.GetPage(this.Session, pageNo, pageSize, sortField, sortDir, "_alias", hql.ToString(), ListUtils.ListToObjectArray(args), null);
        }

        public PageInfo GetDepartmentPage(string punid, int pageNo, int pageSize, string sortField, string sortDir, object[] ouStatuses)
        {
            StringBuilder hql = new StringBuilder("from OUInfo _alias");
            ArrayList args = new ArrayList();
            hql.Append(" where _alias.Type = ?");
            args.Add(OUInfo.OT_DEPARTMENT);
            if (!string.IsNullOrEmpty(punid))
            {
                hql.Append(" and ((_alias.UnitUnid = ? and (_alias.ParentOUUnid is null or _alias.ParentOUUnid = ''))");
                hql.Append(" or _alias.ParentOUUnid = ?");
                hql.Append(" )");
                args.Add(punid);
                args.Add(punid);
            }
            expandStatusCondition(ouStatuses,hql,args);
            if (string.IsNullOrEmpty(sortField))
            {
                hql.Append(" order by _alias.OrderNo, _alias.FullCode");
            }
            return NHibernateHelper.GetPage(this.Session, pageNo, pageSize, sortField, sortDir, "_alias", hql.ToString(), ListUtils.ListToObjectArray(args), "OrderNo");
        }

        public OUInfo FindOUInfoByOldImportDataID(string oldImportDataID)
        {
            string hql = "from OUInfo ouInfo where ouInfo.OldImportDataID = ?";
            IList list = this.HibernateTemplate.Find(hql, oldImportDataID);
            if (list == null || list.Count == 0)
                return null;
            return (OUInfo)list[0];
        }

        public string GetAllOldImportDataID()
        {
            string allOldImportDataID = "";
            string hql = "from OUInfo ouInfo ";
            hql += " where ouInfo.OUStatus = ?";
            IList list = this.HibernateTemplate.Find(hql, OUStatuses.Enable);
            for (int i = 0; i < list.Count; i++)
            {
                OUInfo oUInfo = (OUInfo)list[i];
                if (oUInfo.OldImportDataID == null)
                    continue;
                if (allOldImportDataID.Equals(""))
                    allOldImportDataID = oUInfo.OldImportDataID;
                else
                    allOldImportDataID = allOldImportDataID + "," + oUInfo.OldImportDataID;
            }
            return allOldImportDataID;
        }

        public IList FindDepartmentByUnit(string parentUnid, bool firstLevel)
        {
            object[] args = null;
            string hql = "from OUInfo ouInfo ";
            hql += "where ouInfo.UnitUnid = ? ";
            hql += "and ouInfo.OUStatus = ? ";
            hql += "and ouInfo.Type = ? ";
            if (firstLevel)
            {
                hql += "and (ouInfo.ParentOUUnid is null or ouInfo.ParentOUUnid = ?) ";
                args = new object[] { parentUnid, OUStatuses.Enable, OUInfo.OT_DEPARTMENT, "" };
            }
            else
            {
                args = new object[] { parentUnid, OUStatuses.Enable, OUInfo.OT_DEPARTMENT };
            }
            hql += "order by ouInfo.OrderNo,ouInfo.FullCode";
            return this.HibernateTemplate.Find(hql, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList FindAll_OrgSync()
        {
            string hql = "from OUInfo ouInfo";
            return this.HibernateTemplate.Find(hql);
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 扩展OU状态的查询条件
        /// </summary>
        /// <param name="statuses">状态列表</param>
        /// <param name="hql">要扩展的查询字符串，该值不能为空</param>
        /// <param name="args">要扩展的绑定参数，该值不能为空</param>
        private static void expandStatusCondition(object[] statuses, StringBuilder hql, ArrayList args)
        {
            if (null != statuses && statuses.Length > 0)
            {
                hql.Append(" and _alias.OUStatus in (");
                for (int i = 0; i < statuses.Length; i++)
                {
                    hql.Append(i > 0 ? " ,?" : " ?");
                    args.Add(statuses[i]);
                }
                hql.Append(" )");
            }
        }

        #endregion

    }
}
