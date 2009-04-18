using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Dao;
using TSCommon_Core.SystemPolicy.Domain;

namespace TSCommon_Core.SystemPolicy.Dao
{
    /// <summary>
    /// ϵͳ����DAO�Ľӿڶ���
    /// </summary>
    public interface IPolicyDao : IBaseDao<Policy>
    {
        /// <summary>
        /// װ��ָ�������ϵͳ������Ϣ
        /// </summary>
        /// <param name="code">ϵͳ���Եı���</param>
        /// <returns>��Ӧ�Ĳ��ԣ����û���򷵻�NULL</returns>
        Policy LoadByCode(string code);

        /// <summary>
        /// �ж϶��������ݿ����Ƿ�Ψһ
        /// </summary>
        /// <param name="policy">��Ҫ�жϵĶ���</param>
        /// <returns>�����ݿ�����Ψһ�ľͷ���true,���򷵻�false</returns>
        bool IsUnique(Policy policy);
    }
}
