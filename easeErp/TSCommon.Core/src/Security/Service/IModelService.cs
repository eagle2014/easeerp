using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSLib.Service;
using TSCommon.Core.Security.Domain;

namespace TSCommon.Core.Security.Service
{
    /// <summary>
    /// ģ��Service�Ľӿڶ���
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-06-01</date>
    public interface IModelService : IBaseService<Model>
    {
        /// <summary>
        /// ��ȡ��ָ��ģ�������ȫ��ģ���������Ϣ
        /// </summary>
        /// <param name="id">��Ҫ�ų���ģ��������Ϣ</param>
        /// <returns>�������õ�ģ����Ϣ</returns>
        IList FindAllWithExclude(long id);

        /// <summary>
        /// ��ȡȫ����ģ��
        /// </summary>
        /// <returns>ȫ����ģ��</returns>
        IList FindAllMaster();

        /// <summary>
        /// ��ȡָ��ģ���������ģ��
        /// </summary>
        /// <param name="modelID">ָ��ģ�������</param>
        /// <returns>ָ��ģ���������ģ�飬���modelID������0���򷵻�������ģ��</returns>
        IList FindChildren(long modelID);

        /// <summary>
        /// ��ȡָ��ģ���������ģ��
        /// </summary>
        /// <param name="modelUnid">ָ��ģ���Unid</param>
        /// <returns>ָ��ģ���������ģ�飬���modelUnidΪ�գ��򷵻�������ģ��</returns>
        IList FindChildren(string modelUnid);
        
        /// <summary>
        /// ���ݱ��뷵��ģ��
        /// </summary>
        /// <param name="code">ģ�����</param>
        /// <returns></returns>
        Model GetByCode(string code);
    }
}
