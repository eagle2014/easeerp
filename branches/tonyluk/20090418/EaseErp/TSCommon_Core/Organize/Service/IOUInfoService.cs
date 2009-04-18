/*
 * Copyright (c) 2005, 2007 ��������Ƽ����ϵͳ���޹�˾
 * ��Ȩ����
 * All rights reserved.
 * 
 * Autor: CD826         2007-02-26
 */
using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using TSLib;
using System.Collections;
using TSCommon_Core.Organize.Domain;

namespace TSCommon_Core.Organize.Service
{
    /// <summary>
    /// ��֯�ܹ�Service�Ľӿڶ���
    /// </summary>
    public interface IOUInfoService : IBaseService<OUInfo>
    {        
        /// <summary>
        /// ��ȡ��֯�ܹ���������Ϣ
        /// </summary>
        /// <param name="userInfo">��ǰ��������Ա</param>
        /// <param name="type">��֯�ܹ�������</param>
        /// <param name="pageNo">ҳ��,��ʼҳ���1��ʼ</param>
        /// <param name="pageSize">ÿҳ�ļ�¼��</param>
        /// <param name="sortField">Ҫ�����������</param>
        /// <param name="sortDir">������</param>
        /// <param name="ouStatuses">״̬�б�Ϊ�մ�������״̬</param>
        /// <returns>��ҳ����Ϣ</returns>
        PageInfo GetPageByType(User userInfo, string type, int pageNo, int pageSize, string sortField, string sortDir, object[] ouStatuses);

        /// <summary>
        /// ��ȡ��֯�ܹ���������Ϣ
        /// </summary>        
        /// <param name="unitUnid">����������λ��Unid</param>
        /// <param name="pageNo">ҳ��,��ʼҳ���1��ʼ</param>
        /// <param name="pageSize">ÿҳ�ļ�¼��</param>
        /// <param name="sortField">Ҫ�����������</param>
        /// <param name="sortDir">������</param>
        /// <param name="ouStatuses">״̬�б�Ϊ�մ�������״̬</param>
        /// <returns>��ҳ����Ϣ</returns>
        PageInfo GetDepartmentPage(string unitUnid, int pageNo, int pageSize, string sortField, string sortDir,object[] ouStatuses);

        /// <summary>
        /// ��ȡָ���ĵ�λ�ṹ��Ϣ
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="privilegeType"></param>
        /// <returns></returns>
        IList GetUnitTree(User userInfo, int privilegeType);

        /// <summary>
        /// ��ȡָ����λ�����ͽṹ�б�
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="unitUnid">��λ��Unid</param>
        /// <returns></returns>
        IList GetOUTree(User userInfo, string unitUnid);

        /// <summary>
        /// ��ȡ��֯�ܹ������ͽṹ��Ϣ
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="privilegeType"></param>
        /// <returns></returns>
        IList GetOUTree(User userInfo, int privilegeType);

        /// <summary>
        /// ��ȡָ������֯�ܹ���Ϣ�б�
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IList FindAllByType(User userInfo, string type);

        /// <summary>
        /// ��ѯָ����λ���������������ӵ�λ
        /// </summary>
        /// <param name="parentUnid">��ǰ��λ��Unid</param>
        /// <param name="includeSelf">����ֵ���Ƿ��������λ</param>
        /// <returns>���������ĵ�λ�б�</returns>
        IList FindUnitByParentUnid(string parentUnid, bool includeSelf);

        /// <summary>
        /// ȡ��ָ����λ�����в��ŵ���Ϣ
        /// </summary>
        /// <param name="unitUnid">��λ��UNID</param>
        /// <returns>���ط��������ļ����б�</returns>
        IList FindAllDepartment(string unitUnid);

        /// <summary>
        /// ȡ��ָ��OU��������������OU����Ϣ
        /// </summary>
        /// <param name="parentUnid">��OU��Unid</param>
        /// <param name="hasDepartment">�����OU��һ����λ����ָʾ�Ƿ���Ҫ��ȡ���µĲ���</param>
        /// <param name="hasChildDepartment">�����OU��һ�����ţ���ָʾ�Ƿ���Ҫ��ȡ���Ӳ���</param>
        /// <returns></returns>
        IList FindOUInfoByParentUnid(string parentUnid, bool hasDepartment, bool hasChildDepartment);

        /// <summary>
        /// ������֯�ܹ��Ĵ��������Ӧ����֯�ܹ���Ϣ
        /// </summary>
        /// <param name="fullCode">��֯�ܹ��Ĵ���</param>
        /// <returns>��Ӧ��֯�ܹ���������Ϣ�����û���򷵻�NULL</returns>
        OUInfo LoadByFullCode(string fullCode);

        /// <summary>
        /// �������ݿ���ԭ�����������
        /// </summary>
        /// <param name="oldImportDataID"></param>
        /// <returns></returns>
        OUInfo FindOUInfoByOldImportDataID(string oldImportDataID);

        void ParseSave(User userInfo, OUInfo ouInfo);

        /// <summary>
        /// ������е�ԭ��DeparmentId,�ö��Ÿ���
        /// </summary>
        /// <returns></returns>
        string GetAllOldImportDataID();

        /// <summary>
        /// ����ԭ���ı��ɾ����¼ 
        /// </summary>
        /// <param name="oldImportDataID"></param>
        void DeleteByOldImportDataID(string oldImportDataID);

        IList FindAll();

        /// <summary>
        /// ��ȡָ��OU��ָ�����͵�������OU��������Ϣ
        /// </summary>
        /// <param name="parentUnid">�ϼ�OU��Unid</param>
        /// <param name="ouType">OU�����ͣ�Ϊnull��ʾ��ѯ��������</param>
        /// <returns>ָ��OU��ָ�����͵�������OU��������Ϣ</returns>
        IList FindChilds(string parentUnid, string ouType);

        /// <summary>
        /// ��ȡָ��OU��ָ�����͵�������OU��������Ϣ
        /// </summary>
        /// <param name="parentUnid">�ϼ�OU��Unid</param>
        /// <param name="ouType">OU�����ͣ�Ϊnull��ʾ��ѯ��������</param>
        /// <param name="includeChildOU">�Ƿ�����¼�OU�������¼����¼��ȣ�����Ϣ</param>
        /// <returns>ָ��OU��ָ�����͵�������OU��������Ϣ</returns>
        IList FindChilds(string parentUnid, string ouType, bool includeChildOU);

        /// <summary>
        /// ��ȡָ��OU�µ�������OU��������Ϣ
        /// </summary>
        /// <param name="parentUnid">�ϼ�OU��Unid</param>
        /// <returns>ָ��OU�µ�������OU��������Ϣ</returns>
        IList FindChilds(string parentUnid);

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
