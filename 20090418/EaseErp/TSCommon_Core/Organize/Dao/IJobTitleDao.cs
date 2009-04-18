using System;
using System.Collections.Generic;
using System.Text;
using TSCommon_Core.Organize.Domain;
using System.Collections;
using TSLib.Dao;

namespace TSCommon_Core.Organize.Dao
{
    public interface IJobTitleDao : IBaseDao<JobTitle>
    {
        /// <summary>
        /// ����ְ�������ְ����Ϣ
        /// </summary>
        /// <param name="jobTitleCode">ְ�����</param>
        /// <returns>ָ�������ְ����Ϣ</returns>
        JobTitle LoadByCode(string jobTitleCode);

        /// <summary>
        /// ��ȡָ������Χ������ְ����Ϣ
        /// </summary>
        /// <param name="levels">�����ֵ�б�Ϊ���򷵻ؿյļ���</param>
        /// <returns>ָ������Χ������ְ����Ϣ</returns>
        IList FindAllByLevel(string[] levels);

        /// <summary>
        /// �ж϶��������ݿ����Ƿ�Ψһ
        /// </summary>
        /// <param name="jobTitle">��Ҫ�жϵĶ���</param>
        /// <returns>�����ݿ�����Ψһ�ľͷ���true,���򷵻�false</returns>
        bool IsUnique(JobTitle jobTitle);
    }
}
