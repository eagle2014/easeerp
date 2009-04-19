using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using TSCommon.Core.Organize.Domain;
using System.Collections;

namespace TSCommon.Core.Organize.Service
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
