/*
 * Copyright (c) 2005, 2007 ��������Ƽ����ϵͳ���޹�˾
 * ��Ȩ����
 * All rights reserved.
 * 
 * Autor: CD826         2007-02-28
 */
using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using TSCommon.Core.Security.Domain;
using TSCommon.Core.Organize.Service;
using TSCommon.Core.Security.Dao;
using TSCommon.Core.Organize.Dao;
using System.Collections;
using TSCommon.Core.Organize.Domain;
using TS.Exceptions;
using TSLib.Utils;

namespace TSCommon.Core.Security.Service
{
    /// <summary>
    /// ��ɫService��ʵ��
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-06-01</date>
    public class RoleService : BaseService<Role>, IRoleService
    {
        private IOUInfoService ouInfoService;       // ��֯�ܹ���Service
        private IRoleDao roleDao;                   // ��ɫ���õ�Dao
        private IGroupDao groupDao;                   // ��λ���õ�Dao
        public IGroupDao GroupDao
        {
            set { groupDao = value; }
        }
        public IRoleDao RoleDao
        {
            set
            {
                roleDao = value;
                base.BaseDao = value;
            }
        }
        public IOUInfoService OUInfoService
        {
            set { ouInfoService = value; }
        }

        #region IRoleService ��Ա

        public IList FindByOU(string ouUnid)
        {
            OUInfo ouInfo = this.ouInfoService.Load(ouUnid);
            if (null == ouInfo)
                return new ArrayList();
            if (ouInfo.Type.Equals(OUInfo.OT_DEPARTMENT, StringComparison.OrdinalIgnoreCase))
            {
                ouInfo = this.ouInfoService.Load(ouInfo.UnitUnid);
                if (null == ouInfo)
                    return new ArrayList();
            }
            return FindByLevel(ouInfo.Level, false);
        }

        public IList FindByLevel(string level, bool hasChild)
        {
            return this.roleDao.FindByLevel(level, hasChild);
        }

        public IList FindByUser(string userInfoUnid)
        {
            // ��ȡ�����õĽ�ɫ
            IList list = this.roleDao.FindByUser(userInfoUnid);
            if (list != null && list.Count > 0) return list;//�û������ù���ظ�λ��ɫ��ֱ�ӷ���

            // ��ȡϵͳĬ�ϸ�λӵ�еĽ�ɫ
            Group sysDefaultGroup = groupDao.GetSysDefaultGroup();
            if (sysDefaultGroup == null) return list;
            IList sysDefaultRoles = this.roleDao.FindByGroup(sysDefaultGroup.Unid);
            return sysDefaultRoles;
        }

        public Role LoadByCode(string code)
        {
            return this.roleDao.LoadByCode(code);
        }

        public override void Save(Role role)
        {
            if (!this.roleDao.IsUnique(role))
                throw new ResourceException("ROLE.EXCEPTION.HAD_EXIST", new string[] { role.Name, role.Code });
            this.roleDao.Save(role);
        }

        #endregion

        #region ɾ��������д

        public override void Delete(Role role)
        {
            if (null != role)
            {
                if (role.IsInner.Equals(Constants.YESNO_YES, StringComparison.OrdinalIgnoreCase))
                    throw new ResourceException("ROLE.EXCEPTION.IS_INNER", new string[] { role.Name, role.Code });
                this.roleDao.Delete(role);
            }
        }

        public override void Delete(long id)
        {
            Role role = this.roleDao.Load(id);
            this.Delete( role);
        }

        public override void Delete(string unid)
        {
            Role role = this.roleDao.Load(unid);
            this.Delete(role);
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
            foreach (Role obj in objs)
            {
                this.Delete(obj);
            }
        }

        #endregion
    }
}
