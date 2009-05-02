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
    /// ��λ����DAO�Ľӿڶ���
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-05-31</date>
    public interface IGroupDao : IBaseDao<Group>
    {
        /// <summary>
        /// ��������OU��Unid��ѯ�����õĸ�λ��Ϣ
        /// </summary>
        /// <param name="ouUnid">����OU��Unid</param>
        /// <returns>���ط��������ĸ�λ������Ϣ�ļ���</returns>
        IList FindByOU(string ouUnid);

        /// <summary>
        /// ��������OU��Unid��ѯ�����õĸ�λ��Ϣ
        /// </summary>
        /// <param name="ouUnids">����OU��Unid�б�</param>
        /// <returns>���ط��������ĸ�λ������Ϣ�ļ���</returns>
        IList FindByOU(string[] ouUnids);

        /// <summary>
        /// ��������OU��Unid��ѯ�����õĸ�λ��Ϣ
        /// </summary>
        /// <param name="ouUnids">����OU��Unid�б�</param>
        /// <param name="isCanDispatch">�Ƿ�����ɵ�</param>
        /// <returns>���ط��������ĸ�λ������Ϣ�ļ���</returns>
        IList FindByOU(string[] ouUnids, bool isCanDispatch);

        /// <summary>
        /// ��������OU��Unid��ѯ�����õĸ�λ��Ϣ
        /// </summary>
        /// <param name="ouUnids">����OU��Unid�б�</param>
        /// <param name="groupType">��λ���ͣ�0--ȫ�����ͣ�1--���ɵ���λ��2--�����ɵ���λ</param>
        /// <returns>���ط��������ĸ�λ������Ϣ�ļ���</returns>
        IList FindByOU(string[] ouUnids, string groupType);

        /// <summary>
        /// ��ȡָ��OU�������õ�ָ�����Ƶĸ�λ��Ϣ
        /// </summary>
        /// <param name="ouUnid">OU��Unid</param>
        /// <param name="groupName">��λ����</param>
        /// <returns>���ط��������ĸ�λ��Ϣ����</returns>
        IList FindByName(string ouUnid, string groupName);

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
        /// <returns></returns>
        IList FindAllSendTo(string[] ouUnid);

        /// <summary>
        /// ����ָ����Ա��ӵ�еĸ�λ�б�
        /// </summary>
        /// <param name="userUnid">��Ա��Unid</param>
        /// <returns>���ط��������ĸ�λ������Ϣ�ļ���</returns>
        IList FindByUser(string userUnid);

        /// <summary>
        /// ��ȡ��λ���õ�������Ϣ
        /// </summary>
        /// <param name="ouUnid">����OU��UNID</param>
        /// <param name="pageNo">��Ҫ��ȡ��ҳ����</param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDir"></param>
        /// <returns>��ҳ����Ϣ</returns>
        PageInfo GetPageByOU(string ouUnid, int pageNo, int pageSize, string sortField, string sortDir);

        /// <summary>
        /// �ж϶��������ݿ����Ƿ�Ψһ
        /// </summary>
        /// <param name="group">��Ҫ�жϵĶ���</param>
        /// <returns>�����ݿ�����Ψһ�ľͷ���true,���򷵻�false</returns>
        bool IsUnique(Group group);

        /// <summary>
        /// ��ȡϵͳ��Ĭ�ϸ�λ������Աû�������κθ�λ��ϢʱϵͳĬ�ϸ���Աӵ�иø�λ
        /// </summary>
        /// <returns>ϵͳ��Ĭ�ϸ�λ</returns>
        Group GetSysDefaultGroup();

        /// <summary>
        /// ���ո�λ�������֯OUCode�ж��Ƿ��иø�λ
        /// </summary>
        /// <param name="code"></param>
        /// <param name="ouCode"></param>
        /// <returns></returns>
        bool DoHaveGroupByCode(string code, string ouCode);

        /// <summary>
        /// ����Code���ظ�λ����������Ϣ
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Group LoadByCode(string code);
    }
}
