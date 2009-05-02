/*
 * Copyright (c) 2005, 2007 ��������Ƽ����ϵͳ���޹�˾
 * ��Ȩ����
 * All rights reserved.
 * 
 * Autor: CD826         2007-02-17
 */
using System;
using System.Collections.Generic;
using System.Text;
using Lib;

namespace TSCommon.Core.Organize.Domain
{
    /// <summary>
    /// ������ϵ�ļ�¼��Domain
    /// </summary>
    public class RelationShip : Entry
    {
        /// <summary>
        /// �ָ���
        /// </summary>
        public const string RS_SPLITFLAG = ".";

        #region �ֶζ���
        
        private string parentUnid;              // ���ĵ���UNID
        private string parentType;              // ���ĵ�������
        private string childUnid;               // ���ĵ���UNID
        private string childType;               // ���ĵ�������
        private string relationShipType;        // ������ϵ������һ��ΪparentType.childType

        #endregion

        #region ���Զ���
        
        /// <summary>
        /// ���ĵ���UNID
        /// </summary>
        public string ParentUnid
        {
            get { return parentUnid; }
            set { parentUnid = value; }
        }
        /// <summary>
        /// ���ĵ�������
        /// </summary>
        public string ParentType
        {
            get { return parentType; }
            set { parentType = value; }
        }
        /// <summary>
        /// ���ĵ���UNID
        /// </summary>
        public string ChildUnid
        {
            get { return childUnid; }
            set { childUnid = value; }
        }
        /// <summary>
        /// ���ĵ�������
        /// </summary>
        public string ChildType
        {
            get { return childType; }
            set { childType = value; }
        }
        /// <summary>
        /// ������ϵ������һ��ΪparentType.childType
        /// </summary>
        public string RelationShipType
        {
            get { return relationShipType; }
            set { relationShipType = value; }
        }

        #endregion

        /// <summary>
        /// Ĭ�Ϲ��캯��
        /// </summary>        
        public RelationShip()
        {            
        }

        /// <summary>
        /// ����ָ����Ϣ����һ��������ϵ�ļ�¼
        /// </summary>
        /// <param name="parentUnid">���ĵ���UNID</param>
        /// <param name="parentType">���ĵ�������</param>
        /// <param name="childUnid">���ĵ���UNID</param>
        /// <param name="childType">���ĵ�������</param>
        public RelationShip(string parentUnid, string parentType, string childUnid,
                            string childType)
        {
            this.parentType = parentType;
            this.parentUnid = parentUnid;
            this.childType = childType;
            this.childUnid = childUnid;
            this.relationShipType = BuildRelationShipCode(parentType, childType);
        }

        /// <summary>
        /// ����ָ����Ϣ����һ��������ϵ�ļ�¼
        /// </summary>
        /// <param name="parentUnid">���ĵ���UNID</param>
        /// <param name="parentType">���ĵ�������</param>
        /// <param name="childUnid">���ĵ���UNID</param>
        /// <param name="childType">���ĵ�������</param>
        /// <param name="relationShipType">������ϵ������һ��ΪparentType.childType</param>
        public RelationShip(string parentUnid, string parentType, string childUnid,
                            string childType, string relationShipType)
        {
            this.parentType = parentType;
            this.parentUnid = parentUnid;
            this.childType = childType;
            this.childUnid = childUnid;
            this.relationShipType = relationShipType;
        }

        /// <summary>
        /// ���������ϵ
        /// </summary>
        /// <param name="parentType">���ĵ�������</param>
        /// <param name="childType">���ĵ�������</param>
        /// <returns>������ϵ</returns>
        public static string BuildRelationShipCode(string parentType, string childType)
        {
            return parentType + RS_SPLITFLAG + childType;
        }

        /// <summary>
        /// ���ƹ�����ϵ��Ϣ
        /// </summary>
        /// <param name="relationShip">Ҫ���ƵĹ�����ϵ</param>
        public void Copy(RelationShip relationShip)
        {
            this.parentUnid = relationShip.ParentUnid;
            this.parentType = relationShip.ParentType;
            this.childUnid = relationShip.ChildUnid;
            this.childType = relationShip.ChildType;
            this.relationShipType = relationShip.RelationShipType;
        }

        /// <summary>
        /// ���¹�����ϵ��Ϣ
        /// </summary>
        /// <param name="parentUnid"></param>
        /// <param name="parentType"></param>
        /// <param name="childUnid"></param>
        /// <param name="childType"></param>
        public void Update(string parentUnid, string parentType, string childUnid, string childType)
        {
            this.parentUnid = parentUnid;
            this.parentType = parentType;
            this.childUnid = childUnid;
            this.childType = childType;
            this.relationShipType = BuildRelationShipCode(parentType, childType);
        }
    }
}
