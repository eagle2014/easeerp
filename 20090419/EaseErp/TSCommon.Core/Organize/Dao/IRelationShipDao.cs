/*
 * Copyright (c) 2005, 2007 ��������Ƽ����ϵͳ���޹�˾
 * ��Ȩ����
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSCommon_Core.Organize.Domain;
using TSLib.Dao;

namespace TSCommon_Core.Organize
{
    /// <summary>
    /// ������ϵDAO�ӿڵĶ���
    /// <author>CD826</author>
    /// </summary>
    public interface IRelationShipDao : IBaseDao<RelationShip>
    {        
        /// <summary>
        /// ��ȡһ��������ϵ
        /// </summary>
        /// <param name="parentUnid">������ϵ���ĵ���UNID</param>
        /// <param name="childUnid">������ϵ���ĵ���UNID</param>
        /// <returns>��������򷵻���Ӧ�Ĺ�����ϵ�����򷵻�һ��NULL</returns>
        RelationShip Get(string parentUnid, string childUnid);

        /// <summary>
        /// ��ȡ������ϵ
        /// </summary>
        /// <param name="childUnid">���ĵ���UNID</param>
        /// <param name="type">������ϵ������</param>
        /// <returns>��������򷵻���Ӧ�Ĺ�����ϵ�����򷵻�һ��NULL</returns>
        IList GetByChild(string childUnid, string type);

        /// <summary>
        /// ��ȡ������ϵ
        /// </summary>
        /// <param name="parentUnid">���ĵ���UNID</param>
        /// <param name="type">������ϵ������</param>
        /// <returns>��������򷵻���Ӧ�Ĺ�����ϵ�����򷵻�һ��NULL</returns>
        IList GetByParent(string parentUnid, string type);

        /// <summary>
        /// ���ݸ����ĸ��ĵ������ĵ���UNID��ɾ������ֱ�ӵĹ�ϵ
        /// </summary>
        /// <param name="parentUnid">���ĵ���UNID</param>
        /// <param name="childUnid">���ĵ���UNID</param>
        void Delete(string parentUnid, string childUnid);

        /// <summary>
        /// ���ݸ����ĸ��ĵ������ĵ���UNID��ɾ������ֱ�ӵĹ�ϵ
        /// </summary>
        /// <param name="parentUnid">���ĵ���UNID</param>
        /// <param name="childUnids">���ĵ���UNID��</param>
        int Delete(string parentUnid, string[] childUnids);
        
        /// <summary>
        /// ɾ��ָ�����������ĵ������й�����ϵ
        /// </summary>
        /// <param name="parentUnid">���ĵ���UNID</param>
        void DeleteAllByParent(string parentUnid);

        /// <summary>
        /// ɾ��ָ�����������ĵ������͵����й�����ϵ
        /// </summary>
        /// <param name="parentUnid">���ĵ���UNID</param>
        /// <param name="type">������ϵ������</param>
        void DeleteAllByParent(string parentUnid, string type);

        /// <summary>
        /// ɾ����ָ�����ĵ������й�����ϵ
        /// </summary>
        /// <param name="childUnid">���ĵ���UNID</param>
        void DeleteAllByChild(string childUnid);

        /// <summary>
        /// ɾ����ָ�����ĵ���������ϵ�����й�����ϵ
        /// </summary>
        /// <param name="childUnid">���ĵ���UNID</param>
        /// <param name="type">������ϵ������</param>
        void DeleteAllByChild(string childUnid, string type);

        /// <summary>
        /// ɾ����ָ���ĵ������й�����ϵ
        /// </summary>
        /// <param name="unid">�ĵ���UNID</param>
        void DeleteAll(string unid);

        /// <summary>
        /// ɾ����ָ���ĵ���������ϵ�����й�����ϵ
        /// </summary>
        /// <param name="unid">�ĵ���UNID</param>
        /// <param name="type">������ϵ������</param>
        void DeleteAll(string unid, string type);
    }
}
