using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using TSCommon_Core.Organize.Domain;
using System.Collections;

namespace TSCommon_Core.Organize.Service
{
    public interface IOULevelService : IBaseService<OULevel>
    {
        /// <summary>
        /// ��ȡָ�����𼯵����м�����Ϣ
        /// </summary>
        /// <param name="levels">�����Level��</param>
        /// <returns>ָ�����𼯵����м�����Ϣ</returns>
        IList FindByLevels(string[] levels);
    }
}
