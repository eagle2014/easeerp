/*
 * Copyright (c) 2005, 2007 ��������Ƽ����ϵͳ���޹�˾
 * ��Ȩ����
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSCommon.Core.Organize.Domain;
using TSLib.Service;

namespace TSCommon.Core.Organize.RelationShips
{
    /// <summary>
    /// ������ϵService�ӿڵĶ���
    /// <author>CD826</author>
    /// </summary>
    public interface IRelationShipService : IBaseService<RelationShip>
    {
        IList GetRelationItems();
        /// <summary>
        /// ���ݸ��ĵ���UNID�������еĹ�����ϵ
        /// </summary>
        /// <param name="parentUnid"></param>
        /// <param name="relationShipType"></param>
        /// <param name="relationShips"></param>
        void UpdateRelationByParent(string parentUnid, string relationShipType, IList relationShips);

        /// <summary>
        /// ���ݸ��ĵ���UNID�������еĹ�����ϵ
        /// </summary>
        /// <param name="childUnid"></param>
        /// <param name="relationShipType"></param>
        /// <param name="relationShips"></param>
        void UpdateRelationByChild(string childUnid, string relationShipType, IList relationShips);

        /// <summary>
        /// ���ݸ��ĵ���UNID�������еĹ�����ϵ
        /// </summary>
        /// <param name="parentUnid"></param>
        /// <param name="relationShips"></param>
        /// <param name="type"></param>
        void UpdateRelation(string parentUnid, IList relationShips, string type);       
        
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

        /// <summary>
        /// ���ݸ��ĵ������ĵ���Ϣ����������ϵ
        /// </summary>
        /// <param name="parentUnid">���ĵ�unid</param>
        /// <param name="parentType">���ĵ���������</param>
        /// <param name="childUnids">���ĵ�unid��</param>
        /// <param name="childType">���ĵ���������</param>
        /// <returns>�ɹ������Ĺ�����ϵ������</returns>
        int Build(string parentUnid, string parentType, string[] childUnids, string childType);
    }
}
