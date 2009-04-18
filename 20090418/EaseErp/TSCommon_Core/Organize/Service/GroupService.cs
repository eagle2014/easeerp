/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSLib.Service;
using TSCommon_Core.Organize.Domain;
using Common.Logging;
using TSCommon_Core.Organize.Dao;
using TSCommon_Core.Security.Dao;
using TSCommon_Core.Organize.RelationShips;
using TSLib;
using TSLib.Utils;
using TSCommon_Core.Security.Domain;
using TS.Exceptions;

namespace TSCommon_Core.Organize.Service
{
    /// <summary>
    /// 岗位配置Service的实现
    /// </summary>
    /// <author>CD826</author>
    /// <author>Tony</author>
    /// <date>2008-05-31</date>
    public class GroupService : BaseService<Group>, IGroupService
    {
        private ILog logger = LogManager.GetLogger(typeof(GroupService));
        private IOUInfoService ouInfoService;               // 组织架构配置的Service
        private IGroupDao groupDao;                         // 岗位配置的Dao
        private IRoleDao roleDao;                           // 角色配置的Dao
        private IUserDao userInfoDao;                   // 人员配置的Dao
        private IRelationShipService relationShipService;   // 关联关系的Service
        public IGroupDao GroupDao
        {
            set
            {
                this.groupDao = value;
                base.BaseDao = value;
            }
        }
        public IRoleDao RoleDao
        {
            set { this.roleDao = value; }
        }
        public IUserDao UserDao
        {
            set { this.userInfoDao = value; }
        }
        public IOUInfoService OUInfoService
        {
            set { this.ouInfoService = value; }
        }
        public IRelationShipService RelationShipService
        {
            set { this.relationShipService = value; }
        }

        #region IGroupService 成员

        public IList FindByOU(string ouUnid, bool includeUnit)
        {
            return this.FindByOU(ouUnid, includeUnit, false);
        }
                
        public IList FindAllSendTo(string ouUnid)
        {
            return this.groupDao.FindAllSendTo(ouUnid);
        }

        public IList FindAllSendTo(string ouUnid, bool includeChild)
        {
            IList ouInfoList = this.ouInfoService.FindOUInfoByParentUnid(ouUnid, includeChild, includeChild);
            IList ouUnidList = new ArrayList();
            foreach (OUInfo ouInfo in ouInfoList)
                ouUnidList.Add(ouInfo.Unid);
            return this.groupDao.FindAllSendTo(ListUtils.ListToStringArray(ouUnidList));
        }

        public IList FindByOU(string ouUnid, bool includeUnit, bool isCanDispatch)
        {
            return FindByOU(ouUnid, includeUnit, isCanDispatch ? "1":"2");
        }

        public IList FindByOU(string ouUnid, bool includeUnit, string groupType)
        {
            OUInfo ouInfo = this.ouInfoService.Load(ouUnid);
            if (null == ouInfo)
                return new ArrayList();

            string[] ouUnids;
            if (ouInfo.Type.Equals(OUInfo.OT_DEPARTMENT, StringComparison.OrdinalIgnoreCase))
            {
                if (includeUnit)
                {
                    if (ouInfo.ParentOUUnid != ouInfo.UnitUnid)
                    {
                        ouUnids = new string[] { ouUnid, ouInfo.ParentOUUnid, ouInfo.UnitUnid };
                    }
                    else
                    {
                        ouUnids = new string[] { ouUnid, ouInfo.UnitUnid };
                    }
                }
                else
                {
                    ouUnids = new string[] { ouUnid };
                }
            }
            else
            {
                ouUnids = new string[] { ouUnid };
            }
            return this.groupDao.FindByOU(ouUnids, groupType);
        }

        public IList FindByOU(string ouUnid, bool includeUnit, string groupType, bool includeChildOUGroup)
        {
            OUInfo ouInfo = this.ouInfoService.Load(ouUnid);
            if (null == ouInfo)
                return new ArrayList();
            IList childOUs = this.ouInfoService.FindChilds(ouUnid, OUInfo.OT_DEPARTMENT, includeChildOUGroup);

            IList ouUnids = new ArrayList();
            if (ouInfo.Type.Equals(OUInfo.OT_DEPARTMENT, StringComparison.OrdinalIgnoreCase))
            {
                if (includeUnit)
                {
                    if (ouInfo.ParentOUUnid != ouInfo.UnitUnid)
                    {
                        ouUnids.Add(ouUnid);
                        ouUnids.Add(ouInfo.ParentOUUnid);
                        ouUnids.Add(ouInfo.UnitUnid);
                    }
                    else
                    {
                        ouUnids.Add(ouUnid);
                        ouUnids.Add(ouInfo.UnitUnid);
                    }
                }
                else
                {
                    ouUnids.Add(ouUnid);
                }
            }
            else
            {
                ouUnids.Add(ouUnid);
            }
            foreach (OUInfo ou in childOUs)
            {
                ouUnids.Add(ou.Unid);
            }
            return this.groupDao.FindByOU(ListUtils.ListToStringArray(ouUnids), groupType);
        }

        public IList FindByName(string ouUnid, string groupName)
        {
            return this.groupDao.FindByName(ouUnid, groupName);
        }

        public IList FindByName(string ouUnid, string groupName, bool continueFindByUpperOU)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug("ouUnid=" + ouUnid);
                logger.Debug("groupName=" + groupName);
                logger.Debug("continueFindByUpperOU=" + continueFindByUpperOU);
            }
            OUInfo ouInfo = this.ouInfoService.Load(ouUnid);
            if (null == ouInfo)
            {
                return new ArrayList();
            }
            IList list = this.groupDao.FindByName(ouUnid, groupName);
            if (list != null && list.Count > 0)
            {
                
                return list;
            }
            // 如果不需要继续向上搜索就返回
            if (!continueFindByUpperOU)
                return new ArrayList();

            // 如果为单位则不做向上搜索的继续处理
            if (ouInfo.Type == OUInfo.OT_UNIT)  
                return new ArrayList();

            OUInfo upperOUInfo = null;
            string upperOUUnid = string.IsNullOrEmpty(ouInfo.ParentOUUnid) ? ouInfo.UnitUnid : ouInfo.ParentOUUnid;
            while (true)
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("  upperOUUnid=" + upperOUUnid);
                }
                list = this.groupDao.FindByName(upperOUUnid, groupName);
                if (list != null && list.Count > 0)
                {
                    break;
                }
                else
                {
                    upperOUInfo = this.ouInfoService.Load(upperOUUnid);
                    if (upperOUInfo == null || upperOUInfo.Type == OUInfo.OT_UNIT)
                    {
                        return new ArrayList();
                    }
                    else
                    {
                        upperOUUnid = string.IsNullOrEmpty(upperOUInfo.ParentOUUnid) ? upperOUInfo.UnitUnid : upperOUInfo.ParentOUUnid;
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug("  upperOU.Unid=" + upperOUUnid);
                            logger.Debug("  upperOU.Type=" + upperOUInfo.Type == OUInfo.OT_UNIT);
                        }
                    }
                }
            }

            return list;
        }

        public PageInfo GetPageByOU(string ouUnid, int pageNo, int pageSize, string sortField, string sortDir)
        {
            return this.groupDao.GetPageByOU(ouUnid, pageNo, pageSize, sortField, sortDir);
        }

        public override Group Load(long id)
        {
            Group group = this.groupDao.Load(id);
            if (null != group)
            {
                group.RoleLists = this.roleDao.FindByGroup(group.Unid);
                group.UserInfoLists = this.userInfoDao.FindByGroup(group.Unid);
            }
            return group;
        }

        public override Group Load(string unid)
        {
            Group group = this.groupDao.Load(unid);
            if (null != group)
            {
                group.RoleLists = this.roleDao.FindByGroup(group.Unid);
                group.UserInfoLists = this.userInfoDao.FindByGroup(group.Unid);
            }
            return group;
        }

        public void ParseSave(User userInfo, Group group)
        {
            if (!this.groupDao.IsUnique(group))
                throw new ResourceException("GROUP.EXCEPTION.HAD_EXIST", new string[] { group.Name, group.Code });

            // 保存与角色之间的关联关系
            IList roleUnids = group.RoleUnidLists;
            IList relationShips = new ArrayList();
            string relation = Group.RELATIONSHIP_CODE + "." + Role.RELATIONSHIP_CODE;
            if (null != roleUnids && roleUnids.Count > 0)
            {
                foreach (string role in roleUnids)
                {
                    RelationShip relationShip = new RelationShip(group.Unid, Group.RELATIONSHIP_CODE, role, Role.RELATIONSHIP_CODE,
                                                                 relation);
                    relationShips.Add(relationShip);
                }
            }
            this.relationShipService.UpdateRelationByParent(group.Unid, relation, relationShips);
            this.groupDao.Save(group);
        }

        public override void Save(Group group)
        {
            if (!this.groupDao.IsUnique(group))
                throw new ResourceException("GROUP.EXCEPTION.HAD_EXIST", new string[] { group.Name, group.Code });

            // 保存与角色之间的关联关系
            IList roleUnids = group.RoleUnidLists;
            IList relationShips = new ArrayList();
            string relation = Group.RELATIONSHIP_CODE + "." + Role.RELATIONSHIP_CODE;
            if (null != roleUnids && roleUnids.Count > 0)
            {
                foreach (string role in roleUnids)
                {
                    RelationShip relationShip = new RelationShip(group.Unid, Group.RELATIONSHIP_CODE, role, Role.RELATIONSHIP_CODE,
                                                                 relation);
                    relationShips.Add(relationShip);
                }
            }
            this.relationShipService.UpdateRelationByParent(group.Unid, relation, relationShips);

            // 保存与人员之间的关联关系
            IList UserInfoUnids = group.UserInfoUnidLists;
            relationShips = new ArrayList();
            relation = Group.RELATIONSHIP_CODE + "." + User.RELATIONSHIP_CODE;
            if (null != UserInfoUnids && UserInfoUnids.Count > 0)
            {
                foreach (string userUnid in UserInfoUnids)
                {
                    RelationShip relationShip = new RelationShip(group.Unid, Group.RELATIONSHIP_CODE, userUnid, User.RELATIONSHIP_CODE,
                                                                 relation);
                    relationShips.Add(relationShip);
                }
            }
            this.relationShipService.UpdateRelationByParent(group.Unid, relation, relationShips);

            // 同步上级单位的信息
            OUInfo ouInfo = this.ouInfoService.Load(group.OUUnid);
            if (null != ouInfo)
            {
                group.OUName = ouInfo.Name;
                group.OUCode = ouInfo.Code;
                group.OUFullName = ouInfo.FullName;
                group.OUFullCode = ouInfo.FullCode;
            }
            this.groupDao.Save(group);
        }

        public override void Delete(long id)
        {
            Group group = this.Load(id);
            if (null == group)
                return;

            // 判断是否是内置岗位
            if (group.IsInner.Equals(Constants.YESNO_YES, StringComparison.OrdinalIgnoreCase))
                throw new ResourceException("GROUP.EXCEPTION.IS_INNER", new string[] { group.Name, group.Code });

            group.GroupStatus = GroupStatuses.Delete;
            this.groupDao.Save(group);
        }

        public bool DoHaveGroupByCode(string code, string ouCode)
        {
            return this.groupDao.DoHaveGroupByCode(code, ouCode);
        }

        public void DeleteAllByOU(string ouUnid)
        {
            IList groupList = this.groupDao.FindByOU(ouUnid);
            foreach (Group group in groupList)
            {
                this.groupDao.Delete(group);
            }
        }

        public Group LoadByCode(string code)
        {
            return this.groupDao.LoadByCode(code);
        }

        #endregion
    }
}
