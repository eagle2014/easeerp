using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using TSCommon.Core.Organize.Domain;
using System.Collections;

namespace TSCommon.Core.Organize.Service
{
    public interface IJobTitleService : IBaseService<JobTitle>
    {
        /// <summary>
        /// ����ְ�������ְ����Ϣ
        /// </summary>
        /// <param name="jobTitleCode">ְ�����</param>
        /// <returns>ָ�������ְ����Ϣ</returns>
        JobTitle LoadByCode(string jobTitleCode);

        /// <summary>
        /// ��ȡָ�����������ְ����Ϣ
        /// </summary>
        /// <param name="level">�����ֵ</param>
        /// <returns>ָ�����������ְ����Ϣ</returns>
        IList FindAllByLevel(string level);

        /// <summary>
        /// ��ȡָ������Χ������ְ����Ϣ
        /// </summary>
        /// <param name="levels">�����ֵ�б�</param>
        /// <returns>ָ������Χ������ְ����Ϣ</returns>
        IList FindAllByLevel(string[] levels);
    }
}
