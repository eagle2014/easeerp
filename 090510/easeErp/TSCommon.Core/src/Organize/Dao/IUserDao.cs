/*
 * Copyright (c) 2005, 2007 ��������Ƽ����ϵͳ���޹�˾
 * ��Ȩ����
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSLib.Dao;
using TSLib;
using TSCommon.Core.Organize.Domain;

namespace TSCommon.Core.Organize.Dao
{
    /// <summary>
    /// ��Ա����DAO�Ľӿڶ���
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-05-31</date>
    public interface IUserDao : IBaseDao<User>
    {
        /// <summary>
        /// ��������OU��Unid��ѯ�����õ���Ա��Ϣ
        /// </summary>
        /// <param name="ouUnid">����OU��Unid��</param>
        /// <returns>���ط�����������Ա������Ϣ�ļ���</returns>
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
        /// <param name="ouUnids">OU��Unid�б�</param>
        /// <param name="userType">�û�����</param>
        /// <returns>��OU�������õ���Ա���õ���Ϣ</returns>
        IList FindByOU(string[] ouUnids, string userType);

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
        /// ��ȡָ��OU�µ���Ա��Ϣ
        /// </summary>
        /// <param name="ouUnid">����OU��UNID��Ϊ�մ������</param>
        /// <param name="pageNo">ҳ��,��ʼҳ���1��ʼ</param>
        /// <param name="pageSize">ÿҳ�ļ�¼��</param>
        /// <param name="sortField">Ҫ�����������</param>
        /// <param name="sortDir">������</param>
        /// <returns>��ҳ����Ϣ</returns>
        PageInfo GetPageByOU(string ouUnid, int pageNo, int pageSize, string sortField, string sortDir);

        /// <summary>
        /// �����û��ĵ�¼��������Ӧ����Ա��Ϣ
        /// </summary>
        /// <param name="loginID">�û��ĵ�¼ID</param>
        /// <returns>������ڸ��û�����Ϣ�򷵻أ����򷵻�NULL</returns>
        User LoadByLoginID(string loginID);

        /// <summary>
        /// �ж϶��������ݿ����Ƿ�Ψһ
        /// </summary>
        /// <param name="userInfo">��Ҫ�жϵĶ���</param>
        /// <returns>�����ݿ�����Ψһ�ľͷ���true,���򷵻�false</returns>
        bool IsUnique(User userInfo);

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
