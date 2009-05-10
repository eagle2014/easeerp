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
using TSCommon.Core.Organize.Domain;

namespace TSCommon.Core.Organize.Service
{
    /// <summary>
    /// ��Ա����Service�Ľӿڶ���
    /// </summary>
    /// <author>CD826</author>
    /// <author>zzh</author>
    /// <author>Tony</author>
    /// <date>2008-05-31</date>
    public interface IUserService : IBaseService<User>
    {
        /// <summary>
        /// ��ȡָ��OU��������Ա���õ�������Ϣ
        /// </summary>
        /// <param name="ouUnid">OU��Unid</param>
        /// <returns>��OU�������õ���Ա���õ���Ϣ</returns>
        IList FindByOU(string ouUnid);

        /// <summary>
        /// ��ȡָ��OU��ָ�����͵���Ա��Ϣ
        /// </summary>
        /// <param name="ouUnid">OU��Unid</param>
        /// <param name="userType">�û�����</param>
        /// <returns>��OU�������õ���Ա���õ���Ϣ</returns>
        IList FindByOU(string ouUnid, string userType);

        /// <summary>
        /// ��ȡָ��OU��ָ�����͵���Ա��Ϣ
        /// </summary>
        /// <param name="ouUnid">OU��Unid</param>
        /// <param name="userType">�û�����</param>
        /// <param name="includeChildOU">�Ƿ�����¼����¼����¼�OU���û�</param>
        /// <returns>��OU�������õ���Ա���õ���Ϣ</returns>
        IList FindByOU(string ouUnid, string userType, bool includeChildOU);

        /// <summary>
        /// ��ȡ��Ա���õ�������Ϣ
        /// </summary>
        /// <param name="ouUnid">����OU��UNID</param>
        /// <param name="pageNo">ҳ��,��ʼҳ���1��ʼ</param>
        /// <param name="pageSize">ÿҳ�ļ�¼��</param>
        /// <param name="sortField">Ҫ�����������</param>
        /// <param name="sortDir">������</param>
        /// <returns>��ҳ����Ϣ</returns>
        PageInfo GetPageByOU(string ouUnid, int pageNo, int pageSize, string sortField, string sortDir);

        /// <summary>
        /// ��ָ֤������Ϣ�Ƿ���ȷ  
        /// </summary>
        /// <param name="loginName">��Ҫ��֤���û���</param>
        /// <param name="password">��֤�Ŀ���</param>
        /// <returns>�����֤�ɹ����ظ��û�����Ϣ�������׳�AuthorizeException</returns>
        User Authorize(string loginName, string password);

        /// <summary>
        /// ����ָ������Ա����������Ϣ
        /// </summary>
        /// <param name="curUser">��ǰ��������Ա</param>
        /// <param name="userInfo">��Ҫ�������Ա����������Ϣ</param>
        void EnabledUser(User curUser, User userInfo);

        /// <summary>
        /// ����ָ������Ա����������Ϣ
        /// </summary>
        /// <param name="curUser">��ǰ��������Ա</param>
        /// <param name="userInfo">��Ҫ���õ���Ա����������Ϣ</param>
        void DisabledUser(User curUser, User userInfo);

        /// <summary>
        /// ֱ���޸��û��Ŀ���
        /// </summary>
        /// <param name="curUser">��ǰ�������û�</param>
        /// <param name="userInfoID">��Ҫ�޸��û���ID</param>
        /// <param name="password">�µ�password</param>
        void ChangeUserPassword(User curUser, long userInfoID, string password);

        /// <summary>
        /// �޸��û��Ŀ���
        /// </summary>
        /// <param name="curUser">��ǰ�������û�</param>
        /// <param name="userInfoID">��Ҫ�޸��û���ID</param>
        /// <param name="password">�µ�password</param>
        /// <param name="oldPassword">���û�ԭ���Ŀ���</param>
        void ChangeUserPassword(User curUser, long userInfoID, string password, string oldPassword);

        /// <summary>
        /// ������messageGroupUser�в����ڵ���
        /// </summary>
        /// <param name="ouUnid"></param>
        /// <param name="userUnid"></param>
        /// <returns></returns>
        IList FindAllWithOutMessageUser(string ouUnid, string userUnid);

        /// <summary>
        /// ���ݵ绰�����ѯָ������Ա��Ϣ
        /// </summary>
        /// <param name="telephoneNo">��Ҫ��ѯ�ĵ绰����</param>
        /// <returns>������������Ա��Ϣ�б�</returns>
        IList FindAllByTelephone(string telephoneNo);

        /// <summary>
        /// ���ݵ�¼������ѯ��Ա��Ϣ
        /// </summary>
        /// <param name="loginID">��¼����</param>
        /// <returns>������������Ա��Ϣ�б�</returns>
        IList FindAllByLoginID(string loginID);

        /// <summary>
        /// ����������ѯ��Ա��Ϣ
        /// </summary>
        /// <param name="name">����</param>
        /// <returns>������������Ա��Ϣ�б�</returns>
        IList FindAllByName(string name);

        /// <summary>
        /// ����ָ����λ������������Ա�б�
        /// </summary>
        /// <param name="groupUnid">Ⱥ���Unid</param>
        /// <returns>���ط�����������Ա������Ϣ�ļ���</returns>
        IList FindByGroup(string groupUnid);

        /// <summary>
        /// ����ָ����λ����������ָ�����͵���Ա�б�
        /// </summary>
        /// <param name="groupUnid">Ⱥ���Unid</param>
        /// <param name="userType">�û�����</param>
        /// <returns>���ط�����������Ա������Ϣ�ļ���</returns>
        IList FindByGroup(string groupUnid, string userType);

        /// <summary>
        /// ��ȡ�û�������Ȩ�޷�Χ
        /// </summary>
        /// <param name="userInfo">��ǰ��������Ա</param>
        /// <param name="privilege">����Ȩ������</param>
        /// <returns>����Ա�����е�����Ȩ�޷�Χ�������ȫ���򷵻�һ���յ����飬Ĭ�Ϸ��ص�ǰ�û����ڵ�λ��Unid</returns>
        string[] FindDataScope(User userInfo, string privilege);

        bool ParseSave(User curUser, User userInfo);

        /// <summary>
        /// ����ԭ��employeeID��������
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        User GetUserInfoByEmployeeID(string employeeID);

        /// <summary>
        /// �������EmployeeID,�ö��Ÿ���
        /// </summary>
        /// <returns></returns>
        string GetAllEmployeeID();

        void DeleteByEmployeeID(string employeeID);

        /// <summary>
        /// �����û�������
        /// </summary>
        /// <param name="curUser"></param>
        /// <param name="userInfoID"></param>
        /// <param name="password">��������</param>
        /// <returns>�µ��Ѽ��ܵ�����</returns>
        string ChangeUserPasswordOne(User curUser, long userInfoID, string password);

        /// <summary>
        /// �����û��ĵ�¼��������Ӧ����Ա��Ϣ
        /// </summary>
        /// <param name="loginID">�û��ĵ�¼ID</param>
        /// <returns>������ڸ��û�����Ϣ�򷵻أ����򷵻�NULL</returns>
        User LoadByLoginID(string loginID);

        /// <summary>
        /// ��ȡָ��Unid����������Ա��Ϣ
        /// </summary>
        /// <param name="unids">��ԱUnid��</param>
        /// <returns>ָ��Unid����������Ա��Ϣ</returns>
        IList FindByUnids(string[] unids);

        /// <summary>
        /// ���ݹ�ϵ��parentUnid�����û��б�
        /// </summary>
        /// <param name="relationShipParentUnid"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageInfo FindAllByRelationShipParentUnid(string relationShipParentUnid, int pageNo, int pageSize);
    }
}
