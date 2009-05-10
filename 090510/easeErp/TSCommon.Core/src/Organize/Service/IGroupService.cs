/*
 * Copyright (c) 2005, 2007 ��������Ƽ����ϵͳ���޹�˾
 * ��Ȩ����
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSLib.Service;
using TSLib;
using TSCommon.Core.Organize.Domain;

namespace TSCommon.Core.Organize.Service
{
    /// <summary>
    /// ��λ����Service�Ľӿڶ���
    /// </summary>
    /// <author>CD826</author>
    /// <author>Tony</author>
    /// <date>2008-05-31</date>
    public interface IGroupService : IBaseService<Group>
    {
        /// <summary>
        /// ��ȡָ��OU�������õĸ�λ��Ϣ
        /// </summary>
        /// <param name="ouUnid">OU��Unid</param>
        /// <param name="includeUnit">�Ƿ����������λ�ĸ�λ��Ϣ</param>
        /// <returns>���ط��������ĸ�λ��Ϣ����</returns>
        IList FindByOU(string ouUnid, bool includeUnit);

        /// <summary>
        /// ��������OU��Unid��ѯ�����õĿ��ɵ��ĸ�λ��Ϣ
        /// </summary>
        /// <param name="ouUnid">����OU��Unid</param>
        /// <returns>���ط��������ĸ�λ������Ϣ�ļ���</returns>
        IList FindAllSendTo(string ouUnid);

        /// <summary>
        /// ��������OU��Unid��ѯ�����õĿ��ɵ��ĸ�λ��Ϣ
        /// </summary>
        /// <param name="ouUnid">����OU��Unid</param>
        /// <param name="includeChild">�Ƿ������Ӳ��ŵĸ�λ</param>
        /// <returns></returns>
        IList FindAllSendTo(string ouUnid, bool includeChild);

        /// <summary>
        /// ��ȡָ��OU�������õĸ�λ��Ϣ
        /// </summary>
        /// <param name="ouUnid">OU��Unid</param>
        /// <param name="includeUnit">�Ƿ����������λ�ĸ�λ��Ϣ</param>
        /// <param name="isCanDispatch">�Ƿ��ǲ�ѯ��Ҫ�ɵ��ĸ�λ, false��ʾ��ѯ����</param>
        /// <returns>���ط��������ĸ�λ��Ϣ����</returns>
        IList FindByOU(string ouUnid, bool includeUnit, bool isCanDispatch);

        /// <summary>
        /// ��ȡָ��OU�������õĸ�λ��Ϣ
        /// </summary>
        /// <param name="ouUnid">OU��Unid</param>
        /// <param name="includeUnit">�Ƿ����������λ�ĸ�λ��Ϣ</param>
        /// <param name="groupType">��λ���ͣ�0--ȫ�����ͣ�1--���ɵ���λ��2--�����ɵ���λ</param>
        /// <returns>���ط��������ĸ�λ��Ϣ����</returns>
        IList FindByOU(string ouUnid, bool includeUnit, string groupType);

        /// <summary>
        /// ��ȡָ��OU�������õĸ�λ��Ϣ
        /// </summary>
        /// <param name="ouUnid">OU��Unid</param>
        /// <param name="includeUnit">�Ƿ����������λ�ĸ�λ��Ϣ</param>
        /// <param name="groupType">��λ���ͣ�0--ȫ�����ͣ�1--���ɵ���λ��2--�����ɵ���λ</param>
        /// <param name="includeChildOUGroup">�Ƿ�����¼����ţ��������ŵ��Ӳ��ţ��ĸ�λ��Ϣ</param>
        /// <returns>���ط��������ĸ�λ��Ϣ����</returns>
        IList FindByOU(string ouUnid, bool includeUnit, string groupType, bool includeChildOUGroup);

        /// <summary>
        /// ��ȡָ��OU�������õ�ָ�����Ƶĸ�λ��Ϣ
        /// </summary>
        /// <param name="ouUnid">OU��Unid</param>
        /// <param name="groupName">��λ����</param>
        /// <returns>���ط��������ĸ�λ��Ϣ����</returns>
        IList FindByName(string ouUnid, string groupName);

        /// <summary>
        /// ��ȡָ��OU�������õ�ָ�����Ƶĸ�λ��Ϣ
        /// </summary>
        /// <param name="ouUnid">OU��Unid</param>
        /// <param name="groupName">��λ����</param>
        /// <param name="continueFindByUpperOU">�����ָ���Ĳ������Ҳ����ø�λ���Ƿ������ϼ������м��������ң�ֱ����λΪֹ</param>
        /// <returns>���ط��������ĸ�λ��Ϣ����</returns>
        IList FindByName(string ouUnid, string groupName, bool continueFindByUpperOU);
       
        /// <summary>
        /// ��ȡ��λ���õ�������Ϣ
        /// </summary>
        /// <param name="ouUnid">����OU��UNID</param>
        /// <param name="pageNo">ҳ��,��ʼҳ���1��ʼ</param>
        /// <param name="pageSize">ÿҳ�ļ�¼��</param>
        /// <param name="sortField">Ҫ�����������</param>
        /// <param name="sortDir">������</param>
        /// <returns>��ҳ����Ϣ</returns>
        PageInfo GetPageByOU(string ouUnid, int pageNo, int pageSize, string sortField, string sortDir);

        /// <summary>
        /// ����Code���ظ�λ����������Ϣ
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Group LoadByCode(string code);

        /// <summary>
        /// Ϊ��Աͬ���ṩ�������
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="group"></param>
        void ParseSave(User userInfo, Group group);

        /// <summary>
        /// ���ո�λ�������֯OUCode�ж��Ƿ��иø�λ
        /// </summary>
        /// <param name="code"></param>
        /// <param name="ouCode"></param>
        /// <returns></returns>
        bool DoHaveGroupByCode(string code, string ouCode);

        /// <summary>
        /// ����ouUnidɾ�����и�λ
        /// </summary>
        /// <param name="ouUnid"></param>
        void DeleteAllByOU(string ouUnid);
    }
}
