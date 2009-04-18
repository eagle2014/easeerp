using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Lib;
using TSLib.Utils;
using TSLibStruts.Utils;

namespace TSCommon_Core.Organize.Domain
{
    /// <summary>
    /// ��Ա״̬
    /// </summary>
    public enum UserStatuses { Undefined = -1, Enable = 0, Disable = 1, Add = 2, Modified = 4, Delete = 8 };

    /// <summary>
    /// ������Ա״̬��Strutsת����
    /// </summary>
    public class UserStatusConverter : IConverter
    {
        public object Convert(Type type, object value)
        {
            if (value != null && !value.Equals(""))
                return Enum.Parse(typeof(UserStatuses), value.ToString());
            else
                return UserStatuses.Undefined;
        }
    }

    /// <summary>
    /// �û�����Domain
    /// </summary>
    public class User : Entry
    {
        #region ��������

        /// <summary>������ϵ�Ķ���,ֵΪuserInfo</summary>
        public static string RELATIONSHIP_CODE = "userInfo";

        /// <summary>��ͨ�û�����,ֵΪ0</summary>
        public static string T_CLIENT = "0";    	// ��ͨ�û�����
        /// <summary>����̨�û�����,ֵΪ1</summary>
        public static string T_HELPDESK = "1";    	// ����̨�û�����
        /// <summary>����֧���û�����,ֵΪ2</summary>
        public static string T_SUPPORT = "2";    	// ����֧���û�����

        #endregion

        #region �ֶζ���

        private int userStatus;                 // �û�״̬��   0������     1������     2�����        
        //              4���޸�     8��ɾ��
        private string loginID;                 // �û���½�ʺ�
        private string password;                // �û���½����--(MD5����)
        private string name;                    // �û�������
        private string fullName;                // �û���ȫ��
        private int gender;                     // �û����Ա� 0��δ����   1������     2��Ů��
        private string birthday;                // �û������գ���ʽΪyyyy-mm-dd
        private string email;                   // �û���Email
        private string mobile;                  // �û����ֻ�����
        private string telephoneNo;             // �û��Ĺ����绰
        private string orderNo;                 // �û����������
        private string cardID;                  // �û���Ա������
        private string address;                 // �û�����ϵ��ַ
        private string office;                  // �û����ڵİ칫��
        private string unitUnid;                // ������λ��UNID
        private string unitName;                // ������λ������
        private string unitCode;                // ������λ�ı���
        private string unitFullName;            // ������λ��ȫ��
        private string unitFullCode;            // ������λ��ȫ����
        private string ouUnid;                  // �û���������֯��Ψһ��ʶ
        private string ouName;                  // �û���������֯������
        private string ouCode;                  // �û���������֯�ı���
        private string ouFullName;              // �û���������֯��ȫ��
        private string ouFullCode;              // �û���������֯��ȫ����
        private string jobTypeUnid;             // �û���ְ���͵ķ���
        private string jobTypeName;             // �û���ְ���͵�����
        private string jobTitleUnid;            // �û�ְ��UNID
        private string jobTitleName;            // �û�ְ������
        private string userLigion;              // �û�������ò
        private string isTmpUser;               // �Ƿ�����ʱ�û�
        private DateTime validityStartDate;     // �������ʱ�û�����ô��Ч�ڿ�ʼʱ��
        private DateTime validityEndDate;       // �������ʱ�û�����ô��Ч�ڽ���ʱ��
        private string field_1;                 // �û�����չ��Ϣ1
        private string field_2;                 // �û�����չ��Ϣ2
        private string field_3;                 // �û�����չ��Ϣ3
        private string field_4;                 // �û�����չ��Ϣ4
        private string field_5;                 // �û�����չ��Ϣ5
        private string employeeID;
        private string departmentID;
        private string userType = T_CLIENT;     // �û����ͣ�0--��ͨ�û�(Ĭ��)��1--����̨�û���2--����֧���û�

        private IList groupLists;               // �û���ӵ�еĸ�λ�б�
        private IList groupUnidLists;           // �û���ӵ�и�λ��Unid�б�

        #endregion

        #region ���Զ���

        /// <summary>
        /// �û����ͣ�0--��ͨ�û�(Ĭ��)��1--����̨�û���2--����֧���û�
        /// </summary>
        public string UserType
        {
            get { return userType; }
            set { userType = value; }
        }

        /// <summary>
        /// �û����͵�����
        /// </summary>
        public string UserTypeDesc
        {
            get
            {
                if (this.userType == T_CLIENT)
                    return "��ͨ�û�";
                else if (this.userType == T_HELPDESK)
                    return "����̨";
                else if (this.userType == T_SUPPORT)
                    return "����֧��";
                else
                    return "δ֪����";
            }
        }

        public UserStatuses UserStatus
        {
            get { return (UserStatuses)userStatus; }
            set { userStatus = (int)value; }
        }
        public string UserStatusInfo
        {
            get
            {
                if (this.UserStatus == UserStatuses.Enable)
                    return "�";
                else if (this.UserStatus == UserStatuses.Disable)
                    return "����";
                else if (this.UserStatus == UserStatuses.Delete)
                    return "ɾ��";
                else
                    return "δ֪״̬";
            }
        }
        public string LoginID
        {
            get { return loginID; }
            set { loginID = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }
        public int Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        public string Birthday
        {
            get { return birthday; }
            set { birthday = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }
        public string TelephoneNo
        {
            get { return telephoneNo; }
            set { telephoneNo = value; }
        }
        public string OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }
        public string CardID
        {
            get { return cardID; }
            set { cardID = value; }
        }
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        public string Office
        {
            get { return office; }
            set { office = value; }
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
        public string UnitCode
        {
            get { return this.unitCode; }
            set { this.unitCode = value; }
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
        public string JobTypeUnid
        {
            get { return jobTypeUnid; }
            set { jobTypeUnid = value; }
        }
        public string JobTypeName
        {
            get { return jobTypeName; }
            set { jobTypeName = value; }
        }
        public string JobTitleUnid
        {
            get { return jobTitleUnid; }
            set { jobTitleUnid = value; }
        }
        public string JobTitleName
        {
            get { return jobTitleName; }
            set { jobTitleName = value; }
        }
        public string UserLigion
        {
            get { return userLigion; }
            set { userLigion = value; }
        }
        public string IsTmpUser
        {
            get { return isTmpUser; }
            set { isTmpUser = value; }
        }
        public DateTime ValidityStartDate
        {
            get { return validityStartDate; }
            set { validityStartDate = value; }
        }
        public DateTime ValidityEndDate
        {
            get { return validityEndDate; }
            set { validityEndDate = value; }
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

        public string EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }

        public string DepartmentID
        {
            get { return departmentID; }
            set { departmentID = value; }
        }

        public IList GroupUnidLists
        {
            get { return groupUnidLists; }
            set { groupUnidLists = value; }
        }
        public IList GroupLists
        {
            get { return groupLists; }
            set { groupLists = value; }
        }

        #endregion

        /// <summary>
        /// ȡ����Ա������λUnid���б�
        /// </summary>
        /// <returns>�����û�������λUnid���б�</returns>
        public string[] GetUserGroupUnids()
        {
            if (null == this.GroupLists || this.GroupLists.Count == 0)
                return null;

            string[] unids = new string[this.GroupLists.Count];
            int index = 0;
            foreach (Group group in this.GroupLists)
            {
                unids[index] = group.Unid;
                index++;
            }
            return unids;
        }

        public void CopyTo(User userInfo)
        {
            if (userInfo == null) return;
            userInfo.LoginID = this.LoginID;
            userInfo.Name = this.Name;
            userInfo.FullName = this.FullName;
            userInfo.Email = this.Email;
            userInfo.Mobile = this.Mobile;
            userInfo.TelephoneNo = this.TelephoneNo;
            userInfo.OUUnid = this.OUUnid;
            userInfo.OUName = this.OUName;
            userInfo.OUCode = this.OUCode;
            userInfo.OUFullName = this.OUFullName;
            userInfo.OUFullCode = this.OUFullCode;
            userInfo.UnitUnid = this.UnitUnid;
            userInfo.UnitName = this.UnitName;
            userInfo.UnitCode = this.UnitCode;
            userInfo.UnitFullName = this.UnitFullName;
            userInfo.UnitFullCode = this.UnitFullCode;
        }

        #region ������;

        private IDictionary privileges;
        /// <summary>
        /// �û���ӵ�е�Ȩ���б�
        /// </summary>
        public IDictionary Privileges
        {
            get { return privileges; }
            set { privileges = value; }
        }

        /// <summary>
        /// �жϵ�ǰ�û��Ƿ�ӵ��ָ����Ȩ��
        /// </summary>
        /// <remarks>
        /// ע�⣬�û���Ȩ�����ڵ��û���¼ϵͳʱ�ɵ�¼��֤���������û�Ȩ�޵�privilegeMap�ģ����಻����������顣
        /// </remarks>
        /// <param name="privilegeCode">��Ҫ����Ȩ�޴���</param>
        /// <returns>���ӵ���򷵻�true�����򷵻�false</returns>
        public bool HasPrivilege(string privilegeCode)
        {
            if (null == this.privileges || this.privileges.Count == 0)
                return false;
            else
                return this.privileges.Contains(privilegeCode);
        }

        /// <summary>
        /// �Կ�����м���
        /// </summary>
        /// <param name="password">���Ŀ���</param>
        /// <param name="key">�����ֵ</param>
        /// <returns>���ܺ�Ŀ���</returns>
        public static string CreateEncryptPassword(string password, string key)
        {
            return "(" + MD5Utils.MD5String(password) + ")";
        }

        #endregion
    }
}
