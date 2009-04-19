using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using TSCommon.Core.SystemPolicy.Domain;
using TSCommon.Core.Organize.Domain;

namespace TSCommon.Core.SystemPolicy.Service
{
    /// <summary>
    /// ϵͳ����
    /// </summary>
    public interface IPolicyService : IBaseService<Policy>
    {
        /// <summary>
        /// װ��ָ�������ϵͳ������Ϣ
        /// </summary>
        /// <param name="code">ϵͳ���Եı���</param>
        /// <returns>��Ӧ�Ĳ��ԣ����û���򷵻�NULL</returns>
        Policy LoadByCode(string code);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="user"></param>
        /// <param name="policy"></param>
        void Save(User user, Policy policy);
    }
}
