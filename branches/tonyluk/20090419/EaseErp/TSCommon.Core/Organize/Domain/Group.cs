using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Lib;
using TSLib.Utils;

namespace TSCommon_Core.Organize.Domain
{
    /// <summary>
    /// ��λ״̬����
    /// </summary>
    public enum GroupStatuses { Undefined = -1, Enable = 0, Disable = 1, Add = 2, Modified = 4, Delete = 8 };

    /// <summary>
    /// ���ڸ�λ״̬��Strutsת����
    /// </summary>
    public class GroupStatusConverter :TSLibStruts.Utils.IConverter
    {
        object TSLibStruts.Utils.IConverter.Convert(Type type, object value)
        {
            if (value != null && !value.Equals(""))
                return Enum.Parse(typeof(GroupStatuses), value.ToString());
            else
                return GroupStatuses.Undefined;
        }
    }

    /// <summary>
    /// ��λ���õ�Domain
    /// </summary>
    public class Group : Entry
    {
        public static string RELATIONSHIP_CODE = "group";    // ������ϵ�Ķ���

        #region �ֶζ���
        
        private int groupStatus;                // Ⱥ��״̬��   0������     1������     2�����
                                                //              4���޸�     8��ɾ��
        private string name;                    // Ⱥ������
        private string code;                    // Ⱥ�����
        private string rankUnid;                // Ⱥ�鼶��Ĵ���
        private string rankName;                // Ⱥ�鼶�������
        private string isCanDispatch;           // �Ƿ��ǿ��Խ����ɵ��� Y����   N����
        private string isInner;                 // �Ƿ������ø�λ�� Y����   N����
        private string ouUnid;                  // ��λ��������֯��Ψһ��ʶ
        private string ouName;                  // ��λ��������֯������
        private string ouCode;                  // ��λ��������֯�ı���
        private string ouFullName;              // ��λ��������֯��ȫ��
        private string ouFullCode;              // ��λ��������֯��ȫ����
        private string memo;                    // ��ע��Ϣ
        private string field_1;                 // Ⱥ�����չ��Ϣ1
        private string field_2;                 // Ⱥ�����չ��Ϣ2
        private string field_3;                 // Ⱥ�����չ��Ϣ3
        private string field_4;                 // Ⱥ�����չ��Ϣ4
        private IList roleLists;                // Ⱥ����ӵ�еĽ�ɫ�б�
        private IList roleUnidLists;            // Ⱥ����ӵ�еĽ�ɫ�б�
        private IList userInfoLists;            // Ⱥ����ӵ�е���Ա�б�
        private IList userInfoUnidLists;        // Ⱥ����ӵ�е���Ա�б�

        #endregion

        #region ���Զ���

        public GroupStatuses GroupStatus
        {
            get { return (GroupStatuses)groupStatus; }
            set { groupStatus = (int)value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        public string RankUnid
        {
            get { return rankUnid; }
            set { rankUnid = value; }
        }
        public string RankName
        {
            get { return rankName; }
            set { rankName = value; }
        }
        public string IsCanDispatch
        {
            get 
            {
                if (string.IsNullOrEmpty(this.isCanDispatch))
                    return Constants.YESNO_YES;
                return isCanDispatch; 
            }
            set { isCanDispatch = value; }
        }
        public string IsInner
        {
            get { return isInner; }
            set { isInner = value; }
        }
        public string OUUnid
        {
            get { return ouUnid; }
            set { ouUnid = value; }
        }
        public string OUName
        {
            get { return ouName; }
            set { ouName = value; }
        }
        public string OUCode
        {
            get { return ouCode; }
            set { ouCode = value; }
        }
        public string OUFullName
        {
            get { return ouFullName; }
            set { ouFullName = value; }
        }
        public string OUFullCode
        {
            get { return ouFullCode; }
            set { ouFullCode = value; }
        }
        public string Memo
        {
            get { return memo; }
            set { memo = value; }
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
        public IList RoleLists
        {
            get { return roleLists; }
            set { roleLists = value; }
        }
        public IList RoleUnidLists
        {
            get { return roleUnidLists; }
            set { roleUnidLists = value; }
        }
        public IList UserInfoLists
        {
            get { return userInfoLists; }
            set { userInfoLists = value; }
        }
        public IList UserInfoUnidLists
        {
            get { return this.userInfoUnidLists; }
            set { userInfoUnidLists = value; }
        }

        #endregion
    }
}
