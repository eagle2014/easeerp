/*
 * Copyright (c) 2005, 2007 ��������Ƽ����ϵͳ���޹�˾
 * ��Ȩ����
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using System.Collections;
using TSLib;
using TSLib.Utils;
using TS.Exceptions;
using TSCommon.Core.Organize.Domain;
using TSCommon.Core.Organize.RelationShips;
using TSCommon.Core.Organize.Dao;

namespace TSCommon.Core.Organize.Service
{
    /// <summary>
    /// ��Ա������ϢService��ʵ��
    /// </summary>
    /// <author>CD826</author>
    /// <author>Tony</author>
    /// <date>2008-05-31</date>
    public class UserService : BaseService<User>, IUserService
    {
        #region Spring IOC

        private IRelationShipService relationShipService;   // ������ϵ��Service
        private IOUInfoService ouInfoService;               // OU��Service
        private IUserDao userDao;
        private IGroupDao groupDao;
        public IUserDao UserDao
        {
            set
            {
                userDao = value;
                base.BaseDao = value;       // ����ĸ�ֵ����
            }
        }
        public IGroupDao GroupDao
        {
            set { groupDao = value; }
        }
        public IRelationShipService RelationShipService
        {
            set { this.relationShipService = value; }
        }
        public IOUInfoService OUInfoService
        {
            set { this.ouInfoService = value; }
        }

        #endregion

        #region IUserInfoService ��Ա

        public IList FindByOU(string ouUnid)
        {
            return this.userDao.FindByOU(ouUnid);
        }

        public IList FindByOU(string ouUnid, string userType)
        {
            return this.userDao.FindByOU(ouUnid, userType);
        }

        public IList FindByOU(string ouUnid, string userType, bool includeChildOU)
        {
            OUInfo ouInfo = this.ouInfoService.Load(ouUnid);
            if (null == ouInfo)
                return new ArrayList();
            IList childOUs = this.ouInfoService.FindChilds(ouUnid, OUInfo.OT_DEPARTMENT, includeChildOU);

            IList ouUnids = new ArrayList();
            ouUnids.Add(ouUnid);
            foreach (OUInfo ou in childOUs)
            {
                ouUnids.Add(ou.Unid);
            }
            return this.userDao.FindByOU(ListUtils.ListToStringArray(ouUnids), userType);
        }

        public PageInfo GetPageByOU(string ouUnid, int pageNo, int pageSize, string sortField, string sortDir)
        {
            return this.userDao.GetPageByOU(ouUnid, pageNo, pageSize, sortField, sortDir);
        }

        public User Authorize(string loginName, string password)
        {
            if (string.IsNullOrEmpty(loginName))
                throw new AuthorizeException();

            User user = this.userDao.LoadByLoginID(loginName);
            if (null == user)
                throw new AuthorizeException();

            password = this.EncryptPassword(user, password);
            if (password.Equals(user.Password, StringComparison.OrdinalIgnoreCase))
                return user;
            else
                throw new AuthorizeException();
        }

        public override User Load(long id)
        {
            User user = this.userDao.Load(id);
            if (null != user)
                user.GroupLists = this.groupDao.FindByUser(user.Unid);

            return user;
        }

        public User LoadByLoginID(string loginID)
        {
            return this.userDao.LoadByLoginID(loginID);
        }

        public IList FindByUnids(string[] unids)
        {
            return this.userDao.FindByUnids(unids);
        }

        public bool ParseSave(User curUser, User userInfo)
        {
            this.userDao.Save(userInfo);
            return true;
        }

        public override void Save(User user)
        {
            // �ж��Ƿ�Ψһ
            if (!this.userDao.IsUnique(user))
                throw new ResourceException("USERINFO.EXCEPTION.HAD_EXIST", new string[] { user.Name, user.LoginID });

            // �������λ֮��Ĺ�����ϵ
            IList groupUnids = user.GroupUnidLists;
            IList relationShips = new ArrayList();
            string relation = Group.RELATIONSHIP_CODE + "." + User.RELATIONSHIP_CODE;
            if (null != groupUnids && groupUnids.Count > 0)
            {
                foreach (string group in groupUnids)
                {
                    RelationShip relationShip = new RelationShip(group, Group.RELATIONSHIP_CODE, user.Unid, User.RELATIONSHIP_CODE,
                                                                 relation);
                    relationShips.Add(relationShip);
                }
            }
            this.relationShipService.UpdateRelationByChild(user.Unid, relation, relationShips);

            // ͬ��OU����Ϣ
            OUInfo ouInfo = this.ouInfoService.Load(user.OUUnid);
            if (null != ouInfo)
            {
                user.OUName = ouInfo.Name;
                user.OUCode = ouInfo.Code;
                user.OUFullName = ouInfo.FullName;
                if (ouInfo.Type.Equals(OUInfo.OT_DEPARTMENT, StringComparison.OrdinalIgnoreCase))
                {
                    user.UnitUnid = ouInfo.UnitUnid;
                    user.UnitName = ouInfo.UnitName;
                }
                else
                {
                    user.UnitUnid = ouInfo.Unid;
                    user.UnitName = ouInfo.Name;
                }
            }
            this.userDao.Save(user);
        }

        public IList FindAllWithOutMessageUser(string ouUnid, string userUnid)
        {
            return this.userDao.FindAllWithOutMessageUser(ouUnid, userUnid);
        }

        public IList FindAllByTelephone(string telephoneNo)
        {
            return this.userDao.FindAllByTelephone(telephoneNo);
        }

        public IList FindAllByLoginID(string loginID)
        {
            return this.userDao.FindAllByLoginID(loginID);
        }

        public IList FindAllByName(string name)
        {
            return this.userDao.FindAllByName(name);
        }

        public IList FindByGroup(string groupUnid)
        {
            return this.userDao.FindByGroup(groupUnid);
        }

        public IList FindByGroup(string groupUnid, string userType)
        {
            return this.userDao.FindByGroup(groupUnid, userType);
        }

        public string[] FindDataScope(User userInfo, string privilege)
        {
            if (String.IsNullOrEmpty(privilege) || privilege.Equals(Constants.DP_LOCAL, StringComparison.OrdinalIgnoreCase))
                return new string[] { userInfo.UnitUnid };
            if (privilege.Equals(Constants.DP_ALL, StringComparison.OrdinalIgnoreCase))
                return null;

            IList ous = this.ouInfoService.FindUnitByParentUnid(userInfo.UnitUnid, true);
            if (null == ous || ous.Count == 0)
                return new string[] { userInfo.UnitUnid };
            else
            {
                string[] ouUnids = new string[ous.Count];
                for (int i = 0; i < ous.Count; i++)
                {
                    OUInfo ouInfo = (OUInfo)ous[i];
                    ouUnids[i] = ouInfo.Unid;
                }
                return ouUnids;
            }
        }

        #endregion

        #region ����/�����û�

        public void EnabledUser(User curUser, User user)
        {
            // �ж�Password�Ƿ��Ѿ����ܣ����û������м���
            string password = user.Password;
            if (!password.StartsWith("("))
            {
                user.Password = EncryptPassword(user, password); ;
            }
            user.UserStatus = UserStatuses.Enable;
            this.Save(user);
        }

        public void DisabledUser(User curUser, User userInfo)
        {
            userInfo.UserStatus = UserStatuses.Disable;
            this.Save(userInfo);
        }

        #endregion

        #region �޸�����

        public void ChangeUserPassword(User curUser, long userID, string password)
        {
            User user = this.userDao.Load(userID);
            if (null == user)
                throw new ResourceException("USERINFO.EXCEPTION.NOT_EXIST");

            // �Կ�����м���
            user.Password = EncryptPassword(user, password);
            this.userDao.Save(user);
        }

        public string ChangeUserPasswordOne(User curUser, long userID, string password)
        {
            User user = this.userDao.Load(userID);
            if (null == user)
                throw new ResourceException("USERINFO.EXCEPTION.NOT_EXIST");

            // �Կ�����м���
            user.Password = EncryptPassword(user, password);
            this.userDao.Save(user);
            return user.Password;
        }

        public void ChangeUserPassword(User curUser, long userID, string password, string oldPassword)
        {
            User user = this.userDao.Load(userID);
            if (null == user)
                throw new ResourceException("USERINFO.EXCEPTION.NOT_EXIST");
            oldPassword = EncryptPassword(user, oldPassword);
            if (oldPassword.Equals(user.Password, StringComparison.OrdinalIgnoreCase))
                throw new ResourceException("USERINFO.EXCEPTION.ERROR_OLD_PASSWORD");

            // �Կ�����м���
            user.Password = EncryptPassword(user, password);
            this.userDao.Save(user);
        }

        /// <summary>
        /// �Կ�����м���
        /// </summary>
        /// <param name="userInfo">��ǰ�û�����Ϣ</param>
        /// <param name="password">�µĿ���</param>
        /// <returns>���ܺ�Ŀ���</returns>
        private string EncryptPassword(User user, string password)
        {
            return User.CreateEncryptPassword(password, null);
        }

        #endregion

        #region ɾ��������д��ֻ��״̬��ʵ��ɾ��

        public override void Delete(long id)
        {
            User user = this.userDao.Load(id);
            if (null == user)
                return;
            user.UserStatus = UserStatuses.Delete;
            this.userDao.Save(user);
        }

        public override void Delete(string unid)
        {
            User user = this.userDao.Load(unid);
            if (null == user)
                return;
            user.UserStatus = UserStatuses.Delete;
            this.userDao.Save(user);
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

        public override void Delete(User obj)
        {
            obj.UserStatus = UserStatuses.Delete;
            this.userDao.Save(obj);
        }

        public override void Delete(IList objs)
        {
            foreach (User obj in objs)
            {
                this.Delete(obj);
            }
        }

        #endregion

        #region ByEmployeeID

        public User GetUserInfoByEmployeeID(string employeeID)
        {
            return this.userDao.GetUserInfoByEmployeeID(employeeID);
        }

        public string GetAllEmployeeID()
        {
            return this.userDao.GetAllEmployeeID();
        }

        public void DeleteByEmployeeID(string employeeID)
        {
            User user = GetUserInfoByEmployeeID(employeeID);
            if (null == user)
                return;
            user.UserStatus = UserStatuses.Delete;
            this.userDao.Save(user);
        }

        #endregion

        #region IUserService ��Ա


        public PageInfo FindAllByRelationShipParentUnid(string relationShipParentUnid, int pageNo, int pageSize)
        {
            return this.userDao.FindAllByRelationShipParentUnid(relationShipParentUnid, pageNo, pageSize);
        }

        #endregion
    }
}
