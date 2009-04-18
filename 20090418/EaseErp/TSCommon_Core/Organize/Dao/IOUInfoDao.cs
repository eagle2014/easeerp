/*
 * Copyright (c) 2005, 2007 ��������Ƽ����ϵͳ���޹�˾
 * ��Ȩ����
 * All rights reserved.
 * 
 * Autor: CD826         2007-02-15
 */
using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Dao;
using System.Collections;
using TSLib;
using TSCommon_Core.Organize.Domain;

namespace TSCommon_Core.Organize.Dao
{
    /// <summary>
    /// ��λ��֯��Ϣ��DAO�ӿڶ���
    /// </summary>
    public interface IOUInfoDao : IBaseDao<OUInfo>
    {
        /// <summary>
        /// ��ȡȫ����֯�ܹ���������Ϣ
        /// </summary>
        /// <returns>�������õ���֯�ܹ���Ϣ</returns>
        new IList FindAll();

        /// <summary>
        /// ��ȡָ�����͵���֯�ṹ������Ϣ
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList FindAllByType(string type);

        /// <summary>
        /// ��ȡָ��OU��ָ�����͵�������OU��������Ϣ
        /// </summary>
        /// <param name="parentUnid">�ϼ�OU��Unid</param>
        /// <param name="ouType">OU�����ͣ�Ϊnull��ʾ��ѯ��������</param>
        /// <returns>ָ��OU��ָ�����͵�������OU��������Ϣ</returns>
        IList FindChildByParentUnid(string parentUnid, string ouType);

        /// <summary>
        /// ��ȡָ��OU��ָ�����͵�������OU��������Ϣ
        /// </summary>
        /// <param name="parentUnid">�ϼ�OU��Unid</param>
        /// <param name="ouType">OU�����ͣ�Ϊnull��ʾ��ѯ��������</param>
        /// <param name="includeChildOU">�Ƿ�����¼�OU�������¼����¼��ȣ�����Ϣ</param>
        /// <returns>ָ��OU��ָ�����͵�������OU��������Ϣ</returns>
        IList FindChildByParentUnid(string parentUnid, string ouType,bool includeChildOU);

        /// <summary>
        /// ��ȡָ��OU��ָ�����͵�������OU��������Ϣ
        /// </summary>
        /// <param name="parentCode">�ϼ�OU�ı���(����ȫ��)</param>
        /// <param name="ouType">OU������</param>
        /// <returns>ָ��OU��ָ�����͵�������OU��������Ϣ</returns>
        IList FindChildByParentCode(string parentCode, string ouType);

        /// <summary>
        /// ��ȡָ���������֯�ܹ�������Ϣ
        /// </summary>
        /// <param name="level">��֯�ܹ��������</param>
        /// <param name="hasChild">�Ƿ�����Ӽ�</param>
        /// <returns>������������֯�ܹ������б�</returns>
        IList FindByLevel(int level, bool hasChild);

        /// <summary>
        /// ȡ��ָ����λ�����в��ŵ���Ϣ
        /// </summary>
        /// <param name="unitUnid">��λ��UNID</param>
        /// <returns>���ط��������ļ����б�</returns>
        IList FindAllDepartment(string unitUnid);

        /// <summary>
        /// ������֯�ܹ��Ĵ��������Ӧ����֯�ܹ���Ϣ
        /// </summary>
        /// <param name="fullCode">��֯�ܹ��Ĵ���</param>
        /// <returns>��Ӧ��֯�ܹ���������Ϣ�����û���򷵻�NULL</returns>
        OUInfo LoadByFullCode(string fullCode);

        /// <summary>
        /// �ж϶��������ݿ����Ƿ�Ψһ
        /// </summary>
        /// <param name="ouInfo">��Ҫ�жϵĶ���</param>
        /// <returns>�����ݿ�����Ψһ�ľͷ���true,���򷵻�false</returns>
        bool IsUnique(OUInfo ouInfo);

        /// <summary>
        /// ��ȡָ��������֯�ܹ���ҳ����Ϣ
        /// </summary>
        /// <param name="type">��֯�ܹ�������</param>
        /// <param name="pageNo">ҳ��,��ʼҳ���1��ʼ</param>
        /// <param name="pageSize">ÿҳ�ļ�¼��</param>
        /// <param name="sortField">Ҫ�����������</param>
        /// <param name="sortDir">������</param>
        /// <param name="ouStatuses">״̬�б�Ϊ�մ�������״̬</param>
        /// <returns>����������ҳ����Ϣ</returns>
        PageInfo GetPageByType(string type, int pageNo, int pageSize, string sortField, string sortDir, object[] ouStatuses);

        /// <summary>
        /// ��ȡָ��������֯�ܹ���ҳ����Ϣ
        /// </summary>
        /// <param name="unitUnid">����������λ��Unid</param>
        /// <param name="pageNo">ҳ��,��ʼҳ���1��ʼ</param>
        /// <param name="pageSize">ÿҳ�ļ�¼��</param>
        /// <param name="sortField">Ҫ�����������</param>
        /// <param name="sortDir">������</param>
        /// <param name="ouStatuses">״̬�б�Ϊ�մ�������״̬</param>
        /// <returns>����������ҳ����Ϣ</returns>
        PageInfo GetDepartmentPage(string unitUnid, int pageNo, int pageSize, string sortField, string sortDir, object[] ouStatuses);

        /// <summary>
        /// �������ݿ���ԭ�����������
        /// </summary>
        /// <param name="oldImportDataID"></param>
        /// <returns></returns>
        OUInfo FindOUInfoByOldImportDataID(string oldImportDataID);

        /// <summary>
        /// ������е�ԭ��DeparmentId,�ö��Ÿ���
        /// </summary>
        /// <returns></returns>
        string GetAllOldImportDataID();

        /// <summary>
        /// ��ȡָ����λ�������������в�����Ϣ
        /// </summary>
        /// <param name="parentUnid">������λ��Unid</param>
        /// <param name="firstLevel">�Ƿ��ǵ�һ��Ĳ�����Ϣ</param>
        /// <returns>�����������ô���أ����򷵻ؿ�ֵ</returns>
        IList FindDepartmentByUnit(string parentUnid, bool firstLevel);

        /// <summary>
        /// ��������ͬ����������������֯�ܹ�
        /// </summary>
        /// <returns></returns>
        IList FindAll_OrgSync();
    }
}
