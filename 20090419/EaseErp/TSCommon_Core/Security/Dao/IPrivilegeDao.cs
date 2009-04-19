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
using TSLib.Dao;
using TSCommon.Core.Security.Domain;

namespace TSCommon.Core.Security.Dao
{
    /// <summary>
    /// Ȩ��Dao�Ľӿڶ���
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-06-01</date>
    public interface IPrivilegeDao : IBaseDao<Privilege>
    {
        /// <summary>
        /// ��ȡָ�����͵�Ȩ��������Ϣ
        /// </summary>
        /// <param name="privilegeType">Ȩ������</param>
        /// <returns>����������Ȩ�޼����б�</returns>
        IList FindByType(string privilegeType);

        /// <summary>
        /// ��ȡָ��ģ���µ�����Ȩ���б�
        /// </summary>
        /// <param name="modelID">ģ���ID</param>
        /// <returns>����������Ȩ�޼����б�</returns>
        IList FindByModel(long modelID);

        /// <summary>
        /// ��ȡָ��ģ����ָ�����͵�����Ȩ���б�
        /// </summary>
        /// <param name="modelID">ģ���ID</param>
        /// <param name="type">Ҫ��ȡ��Ȩ�޵�����</param>
        /// <returns>����������Ȩ�޼����б�</returns>
        IList FindByModelAndType(long modelID, string type);

        /// <summary>
        /// ��ȡָ��ģ����ָ�����͵�����Ȩ���б�
        /// </summary>
        /// <param name="modelUnid">ģ���Unid��Ϊ���򷵻ؿյ�Ȩ���б�</param>
        /// <param name="type">Ҫ��ȡ��Ȩ�޵����ͣ�Ϊ�մ���ȫ��</param>
        /// <returns>����������Ȩ�޼����б�</returns>
        IList FindByModelAndType(string modelUnid, string type);

        /// <summary>
        /// ����Ȩ�޵Ĵ��������Ӧ��Ȩ����Ϣ
        /// </summary>
        /// <param name="code">Ȩ�޵Ĵ���</param>
        /// <returns>��ӦȨ�޵�������Ϣ�����û���򷵻�NULL</returns>
        Privilege LoadByCode(string code);

        /// <summary>
        /// �ж϶��������ݿ����Ƿ�Ψһ
        /// </summary>
        /// <param name="privilege">��Ҫ�жϵĶ���</param>
        /// <returns>�����ݿ�����Ψһ�ľͷ���true,���򷵻�false</returns>
        bool IsUnique(Privilege privilege);
    }
}
