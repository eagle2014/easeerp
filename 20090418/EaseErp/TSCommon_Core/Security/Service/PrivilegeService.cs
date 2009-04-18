/*
 * Copyright (c) 2005, 2007 ��������Ƽ����ϵͳ���޹�˾
 * ��Ȩ����
 * All rights reserved.
 * 
 * Autor: CD826         2007-02-27
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSLib.Service;
using TSCommon_Core.Security.Domain;
using TSCommon_Core.Security.Dao;
using TSCommon_Core.Organize.Dao;
using TSCommon_Core.Organize.Domain;
using TS.Exceptions;
using TSLib.Utils;

namespace TSCommon_Core.Security.Service
{
    /// <summary>
    /// Ȩ��Service��ʵ��
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-06-01</date>
    public class PrivilegeService : BaseService<Privilege>, IPrivilegeService
    {
        private IPrivilegeDao privilegeDao;     // Ȩ�����õ�Dao
        public IPrivilegeDao PrivilegeDao
        {
            set
            {
                privilegeDao = value;
                base.BaseDao = value;
            }
        }
        private IGroupDao groupDao;
        public IGroupDao GroupDao
        {
            set { groupDao = value; }
        }
        private IRoleDao roleDao;                           // ��ɫ���õ�Dao
        public IRoleDao RoleDao
        {
            set { this.roleDao = value; }
        }

        #region IPrivilegeService ��Ա

        public IList FindByType(string privilegeType)
        {
            return this.privilegeDao.FindByType(privilegeType);
        }

        public IList FindByModel(long modelID)
        {
            return this.privilegeDao.FindByModel(modelID);
        }

        public IList FindByModelAndType(long modelID, string type)
        {
            return this.privilegeDao.FindByModelAndType(modelID, type);
        }

        public IList FindByModelAndType(string modelUnid, string type)
        {
            return this.privilegeDao.FindByModelAndType(modelUnid, type);
        }

        public Privilege LoadByCode(string code)
        {
            return this.privilegeDao.LoadByCode(code);
        }

        public override void Save(Privilege privilege)
        {
            if (!this.privilegeDao.IsUnique(privilege))
                throw new ResourceException("PRIVILEGE.EXCEPTION.HAD_EXIST", new string[] { privilege.Name, privilege.Code });
            this.privilegeDao.Save(privilege);
        }

        public bool HasPrivilege(User userInfo, string privilegeKey)
        {
            return this.HasAnyPrivilege(userInfo, string.IsNullOrEmpty(privilegeKey) ? null : new string[] { privilegeKey });
        }

        public bool HasAnyPrivilege(User userInfo, string[] privilegeKeys)
        {
            if (null == userInfo || null == privilegeKeys || privilegeKeys.Length == 0) return false;

            // ��ȡ����Ա�����и�λ
            userInfo.GroupLists = this.groupDao.FindByUser(userInfo.Unid);
            IList groupSet = userInfo.GroupLists;
            if (null == groupSet || groupSet.Count == 0) return false;

            // ѭ��ÿһ����λ�����ж�
            foreach (Group group in groupSet)
            {
                if (null == group) continue;
                if (this.HasAnyPrivilege(group, privilegeKeys))
                    return true;
            }
            return false;
        }

        public bool HasAnyPrivilege(Group group, string[] privilegeKeys)
        {
            if (null == group || null == privilegeKeys || privilegeKeys.Length == 0) return false;

            // ���¼���Group
            //group = this.cfgService.GetGroup(group.Id);

            // ��ȡ�ø�λ�����н�ɫ
            group.RoleLists = this.roleDao.FindByGroup(group.Unid);
            IList roles = group.RoleLists;
            if (null == roles || roles.Count == 0) return false;

            // ѭ��ÿһ����ɫ�����ж�
            foreach (Role role in roles)
            {
                if (null == role) continue;
                if (this.HasAnyPrivilege(role, privilegeKeys))
                    return true;
            }
            return false;
        }

        public bool HasAnyPrivilege(Role role, string[] privilegeKeys)
        {
            if (null == role || null == privilegeKeys || privilegeKeys.Length == 0) return false;

            // ���¼���Role
            //role = this.cfgService.GetRole(role.Id);

            // ��ȡ�ý�ɫ������Ȩ��
            IList privileges = role.Privileges;
            if (null == privileges || privileges.Count == 0) return false;

            // ѭ��ÿһ��Ȩ�޽����ж�
            foreach (Privilege privilege in privileges)
            {
                if (null == privilege) continue;
                foreach (string privilegeKey in privilegeKeys)
                {
                    if (privilege.Code == privilegeKey)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region ɾ��������д

        public override void Delete(Privilege privilege)
        {
            if (null != privilege)
            {
                if (privilege.IsInner.Equals(Constants.YESNO_YES, StringComparison.OrdinalIgnoreCase))
                    throw new ResourceException("PRIVILEGE.EXCEPTION.IS_INNER", new string[] { privilege.Name, privilege.Code });
                this.privilegeDao.Delete(privilege);
            }
        }

        public override void Delete(long id)
        {
            Privilege privilege = this.privilegeDao.Load(id);
            this.Delete(privilege);
        }

        public override void Delete(string unid)
        {
            Privilege privilege = this.privilegeDao.Load(unid);
            this.Delete(privilege);
        }

        public override void Delete(long[] ids)
        {
            foreach (long id in ids)
            {
                this.Delete(id);
            }
        }

        public override void Delete(string[] unids)
        {
            foreach (string unid in unids)
            {
                this.Delete(unid);
            }
        }

        public override void Delete(IList objs)
        {
            foreach (Privilege obj in objs)
            {
                this.Delete(obj);
            }
        }

        #endregion
    }
}
