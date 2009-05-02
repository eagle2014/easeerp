using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSLib.Dao;
using TSLib;
using TSCommon.Core.Security.Domain;

namespace TSCommon.Core.Security.Dao
{
    /// <summary>
    /// ��ɫ����Dao�Ľӿڶ���
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-06-01</date>
    public interface IRoleDao : IBaseDao<Role>
    {
        /// <summary>
        /// ��ȡָ������Ľ�ɫ������Ϣ
        /// </summary>
        /// <param name="level">��ɫ�������</param>
        /// <param name="hasChild">�Ƿ�����Ӽ�</param>
        /// <returns>���������Ľ�ɫ�����б�</returns>
        IList FindByLevel(string level, bool hasChild);

        /// <summary>
        /// ��ȡָ����λ���������Ľ�ɫ��Ϣ
        /// </summary>
        /// <param name="grouUnid">��λ��Unid</param>
        /// <returns>���������Ľ�ɫ�����б�</returns>
        IList FindByGroup(string grouUnid);

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

        /// <summary>
        /// �ж϶��������ݿ����Ƿ�Ψһ
        /// </summary>
        /// <param name="role">��Ҫ�жϵĶ���</param>
        /// <returns>�����ݿ�����Ψһ�ľͷ���true,���򷵻�false</returns>
        bool IsUnique(Role role);

        /// <summary>
        /// ��ȡָ������Ľ�ɫ�ķ�ҳ��Ϣ��
        /// </summary>
        /// <param name="level">��ɫ�ļ������</param>
        /// <param name="pageNo">ҳ��,��ʼҳ���1��ʼ</param>
        /// <param name="pageSize">ÿҳ�ļ�¼��</param>
        /// <param name="sortField">Ҫ�����������</param>
        /// <param name="sortDir">������ֵ</param>
        /// <returns>ָ��ҳ��ķ�ҳ����,����Ҳ����κν���򷵻ؿ�ҳ</returns>
        PageInfo GetPage(int level, int pageNo, int pageSize, string sortField, string sortDir);
    }
}
