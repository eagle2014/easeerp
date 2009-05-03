/*
 * Copyright (c) 2005, 2007 ��������Ƽ����ϵͳ���޹�˾
 * ��Ȩ����
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using Lib;
using TSLibStruts.Utils;

namespace TSCommon.Core.Organize.Domain
{
    /// <summary>
    /// ��֯״̬
    /// </summary>
    public enum OUStatuses
    {
        Undefined = -1, Enable = 0, Disable = 1, Add = 2, Modified = 4, Delete = 8
    }

    /// <summary>
    /// ������֯״̬��Strutsת����
    /// </summary>
    public class OUStatusConverter : IConverter
    {
        public object Convert(Type type, object value)
        {
            if (value != null && !value.Equals(""))
                return Enum.Parse(typeof(OUStatuses), value.ToString());
            else
                return OUStatuses.Undefined;
        }
    }

    /// <summary>
    /// ��λ�����ŵ���Ϣ��Domain
    /// <author>CD826</author>
    /// </summary>
    public class OUInfo : Entry
    {
        /// <summary>OU���ͣ���λ</summary>
        public const string OT_UNIT         = "DW";
        /// <summary>OU���ͣ�����</summary>
        public const string OT_DEPARTMENT = "BM";

        #region �ֶζ���

        private int ouStatus;               // ��֯��״̬������������£�0������    1������     8��ɾ��
                                            //                           2�����    4���޸�     
        private string parentOUUnid;        // ��һ�����Ż�λ��UNID
        private string name;                // ��֯������
        private string code;                // ��֯�ı�ʾ
        private string description;         // ������֯�ļ�����
        private string type;                // ��֯�ṹ�����ͣ�DW-��λ  BM-����
        private string level;               // ��֯�ļ���
        private string levelName;           // ��֯���������
        private string fullName;            // ��֯��ȫ�ƣ���ʽΪ����������Ƽ����ϵͳ���޹�˾.���Ӧ�ò�
        private string fullCode;            // ��֯��ȫ��ʾ����ʽΪ��Egrand.SGA
        private string isTmpOU;             // �Ƿ�����ʱ��֯��Y����ʱ  N������ʱ
        private string orderNo;                // ��֯���������
        private string oldImportDataID;     //��¼��ǰ����ID
        private string unitUnid;            // ������λ��UNID
        private string unitName;            // ������λ������
        private string unitFullName;        // ������λ��ȫ��
        private string unitFullCode;        // ������λ��ȫ����
        private string field_1;             // ��֯����չ��Ϣ1
        private string field_2;             // ��֯����չ��Ϣ2
        private string field_3;             // ��֯����չ��Ϣ3
        private string field_4;             // ��֯����չ��Ϣ4
        private string field_5;             // ��֯����չ��Ϣ5
        #endregion

        #region ���Զ���

        public OUStatuses OUStatus
        {
            get { return (OUStatuses)this.ouStatus; }
            set { this.ouStatus = (int)value; }
        }

        public string ParentOUUnid
        {
            get { return this.parentOUUnid; }
            set { this.parentOUUnid = value; }
        }

        public string ParentOUName
        {
            get
            {
                if (null == this.FullName)
                    return "";
                int pos = this.FullName.LastIndexOf(".");
                if (pos >= 0)
                    return this.FullName.Substring(0, pos);
                else
                    return "";
            }
            set { }
        }

        public string UnitUnid
        {
            get { return this.unitUnid; }
            set { this.unitUnid = value; }
        }

        public string UnitName
        {
            get { return this.unitName; }
            set { this.unitName = value; }
        }

        public string UnitFullName
        {
            get { return this.unitFullName; }
            set { this.unitFullName = value; }
        }

        public string UnitFullCode
        {
            get { return this.unitFullCode; }
            set { this.unitFullCode = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        public string Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public string TypeName
        {
            get 
            { 
                if(OT_UNIT.Equals(this.type, StringComparison.OrdinalIgnoreCase))
                    return "��λ��Ϣ";
                else
                    return "������Ϣ";
            }
        }

        public string Level
        {
            get { return this.level; }
            set { this.level = value; }
        }

        public string LevelName
        {
            get { return this.levelName; }
            set { this.levelName = value; }
        }

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public string FullCode
        {
            get { return fullCode; }
            set { fullCode = value; }
        }

        public string IsTmpOU
        {
            get { return isTmpOU; }
            set { isTmpOU = value; }
        }

        public string OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }

        public string OldImportDataID
        {
            get { return oldImportDataID; }
            set { oldImportDataID = value; }
        }

        public string Field_1
        {
            get { return field_1; }
            set { field_1 = value; }
        }

        public string Field_2
        {
            get { return field_2; }
            set { field_2 = value; }
        }

        public string Field_3
        {
            get { return field_3; }
            set { field_3 = value; }
        }

        public string Field_4
        {
            get { return field_4; }
            set { field_4 = value; }
        }
        public string Field_5
        {
            get { return field_5; }
            set { field_5 = value; }
        }
        #endregion

        public override string ToString()
        {
            return this.Name;
        }
    }  
       
}
