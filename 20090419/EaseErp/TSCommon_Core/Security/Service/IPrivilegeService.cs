using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSLib.Service;
using TSCommon.Core.Security.Domain;
using TSCommon.Core.Organize.Domain;

namespace TSCommon.Core.Security.Service
{
    /// <summary>
    /// Ȩ������Service�Ľӿڶ���
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-06-01</date>
    public interface IPrivilegeService : IBaseService<Privilege>
    {
        /// <summary>
        /// ��ȡָ�����͵�Ȩ��������Ϣ
        /// </summary>
        /// <param name="privilegeType">Ȩ������</param>
        /// <returns>����������Ȩ�޼����б�</returns>
        IList FindByType(string privilegeType);

        /// <summary>
        /// ��ȡָ��ģ���µ�����Ȩ���б�
        /// </summary>
        /// <param name="modelID">ģ���ID</param>
        /// <returns>����������Ȩ�޼����б�</returns>
        IList FindByModel(long modelID);

        /// <summary>
        /// ��ȡָ��ģ����ָ�����͵�����Ȩ���б�
        /// </summary>
        /// <param name="modelID">ģ���ID</param>
        /// <param name="type">Ҫ��ȡ��Ȩ�޵�����</param>
        /// <returns>����������Ȩ�޼����б�</returns>
        IList FindByModelAndType(long modelID, string type);

        /// <summary>
        /// ��ȡָ��ģ����ָ�����͵�����Ȩ���б�
        /// </summary>
        /// <param name="modelUnid">ģ���Unid��Ϊ���򷵻ؿյ�Ȩ���б�</param>
        /// <param name="type">Ҫ��ȡ��Ȩ�޵����ͣ�Ϊ�մ���ȫ��</param>
        /// <returns>����������Ȩ�޼����б�</returns>
        IList FindByModelAndType(string modelUnid, string type);

        /// <summary>
        /// ����Ȩ�޵Ĵ��������Ӧ��Ȩ����Ϣ
        /// </summary>
        /// <param name="code">Ȩ�޵Ĵ���</param>
        /// <returns>��ӦȨ�޵�������Ϣ�����û���򷵻�NULL</returns>
        Privilege LoadByCode(string code);

        /// <summary>
        /// �ж�ָ�����û��Ƿ�ӵ��ָ����Ȩ��
        /// </summary>
        /// <param name="userInfo">Ҫ�жϵ��û�</param>
        /// <param name="privilegeKey">Ȩ�ޱ���</param>
        /// <returns></returns>
        bool HasPrivilege(User userInfo, string privilegeKey);

        /// <summary>
        /// �ж�ָ�����û��Ƿ�ӵ��ָ����Ȩ�޼��е���һȨ��
        /// </summary>
        /// <param name="userInfo">Ҫ�жϵ��û�</param>
        /// <param name="privilegeKeys">Ȩ�ޱ����б�</param>
        /// <returns></returns>
        bool HasAnyPrivilege(User userInfo, string[] privilegeKeys);

        /// <summary>
        /// �ж�ָ���ĸ�λ�Ƿ�ӵ��ָ����Ȩ�޼��е���һȨ��
        /// </summary>
        /// <param name="group">Ҫ�жϵĸ�λ</param>
        /// <param name="privilegeKeys">Ȩ�ޱ����б�</param>
        /// <returns></returns>
        bool HasAnyPrivilege(Group group, string[] privilegeKeys);

        /// <summary>
        /// �ж�ָ���Ľ�ɫ�Ƿ�ӵ��ָ����Ȩ�޼��е���һȨ��
        /// </summary>
        /// <param name="role">Ҫ�жϵĽ�ɫ</param>
        /// <param name="privilegeKeys">Ȩ�ޱ����б�</param>
        /// <returns></returns>
        bool HasAnyPrivilege(Role role, string[] privilegeKeys);

    }
}
