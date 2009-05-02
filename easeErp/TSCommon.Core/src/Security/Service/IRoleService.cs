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
using System.Collections;
using TSLib.Service;
using TSCommon.Core.Security.Domain;

namespace TSCommon.Core.Security.Service
{
    /// <summary>
    /// ��ɫ����Service�Ľӿڶ���
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-06-01</date>
    public interface IRoleService : IBaseService<Role>
    {
        /// <summary>
        /// ��ȡָ����֯�н�ɫ��������Ϣ
        /// </summary>
        /// <param name="ouUnid">��֯��Unid</param>
        /// <returns>��Ӧ���õĽ�ɫ��Ϣ</returns>
        IList FindByOU(string ouUnid);

        /// <summary>
        /// ��ȡָ������Ľ�ɫ������Ϣ
        /// </summary>
        /// <param name="level">��ɫ�������</param>
        /// <param name="hasChild">�Ƿ�����Ӽ�</param>
        /// <returns>���������Ľ�ɫ�����б�</returns>
        IList FindByLevel(string level, bool hasChild);

        /// <summary>
        /// ��ȡָ����Ա��ӵ�еĽ�ɫ��Ϣ
        /// </summary>
        /// <param name="userInfoUnid">��Ա��Unid</param>
        /// <returns>���������Ľ�ɫ�����б�</returns>
        IList FindByUser(string userInfoUnid);

        /// <summary>
        /// ���ݽ�ɫ�Ĵ��������Ӧ�Ľ�ɫ��Ϣ
        /// </summary>
        /// <param name="code">��ɫ�Ĵ���</param>
        /// <returns>��Ӧ��ɫ��������Ϣ�����û���򷵻�NULL</returns>
        Role LoadByCode(string code);
    }
}
