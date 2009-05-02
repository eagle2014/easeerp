using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Dao;
using TSCommon.Core.Organize.Domain;
using System.Collections;

namespace TSCommon.Core.Organize.Dao
{
    /// <summary>
    /// ����Dao�ӿڵĶ���
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-05-25</date>
    public interface IOULevelDao : IBaseDao<OULevel>
    {
        /// <summary>
        /// ��ȡָ��������뼯�����м�����Ϣ
        /// </summary>
        /// <param name="codes">�����Level��</param>
        /// <returns>ָ�����𼯵����м�����Ϣ</returns>
        IList FindByCodes(string[] codes);

        /// <summary>
        /// ���ݼ������õ���Ӧ�ļ�������
        /// </summary>
        /// <param name="code">�������</param>
        /// <returns>��������ݿ��н��������ã��򷵻ظ����ã����򷵻�NULL</returns>
        OULevel LoadByCode(string code);

        /// <summary>
        /// �ж϶��������ݿ����Ƿ�Ψһ
        /// </summary>
        /// <param name="ouLevel">��Ҫ�жϵĶ���</param>
        /// <returns>�����ݿ�����Ψһ�ľͷ���true,���򷵻�false</returns>
        bool IsUnique(OULevel ouLevel);
    }
}
