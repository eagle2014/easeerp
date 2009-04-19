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
    /// 人员状态
    /// </summary>
    public enum UserStatuses { Undefined = -1, Enable = 0, Disable = 1, Add = 2, Modified = 4, Delete = 8 };

    /// <summary>
    /// 用于人员状态的Struts转换器
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
    /// 用户配置Domain
    /// </summary>
    public class User : Entry
    {
        #region 常数定义

        /// <summary>关联关系的定义,值为userInfo</summary>
        public static string RELATIONSHIP_CODE = "userInfo";

        /// <summary>普通用户类型,值为0</summary>
        public static string T_CLIENT = "0";    	// 普通用户类型
        /// <summary>服务台用户类型,值为1</summary>
        public static string T_HELPDESK = "1";    	// 服务台用户类型
        /// <summary>服务支持用户类型,值为2</summary>
        public static string T_SUPPORT = "2";    	// 服务支持用户类型

        #endregion

        #region 字段定义

        private int userStatus;                 // 用户状态，   0－启用     1－禁用     2－添加        
        //              4－修改     8－删除
        private string loginID;                 // 用户登陆帐号
        private string password;                // 用户登陆口令--(MD5加密)
        private string name;                    // 用户的名称
        private string fullName;                // 用户的全称
        private int gender;                     // 用户的性别， 0－未定义   1－男性     2－女性
        private string birthday;                // 用户的生日，格式为yyyy-mm-dd
        private string email;                   // 用户的Email
        private string mobile;                  // 用户的手机号码
        private string telephoneNo;             // 用户的工作电话
        private string orderNo;                 // 用户的排序序号
        private string cardID;                  // 用户的员工卡号
        private string address;                 // 用户的联系地址
        private string office;                  // 用户所在的办公室
        private string unitUnid;                // 所属单位的UNID
        private string unitName;                // 所属单位的名称
        private string unitCode;                // 所属单位的编码
        private string unitFullName;            // 所属单位的全称
        private string unitFullCode;            // 所属单位的全编码
        private string ouUnid;                  // 用户隶属的组织的唯一标识
        private string ouName;                  // 用户隶属的组织的名称
        private string ouCode;                  // 用户隶属的组织的编码
        private string ouFullName;              // 用户隶属的组织的全称
        private string ouFullCode;              // 用户隶属的组织的全编码
        private string jobTypeUnid;             // 用户在职类型的分类
        private string jobTypeName;             // 用户在职类型的名称
        private string jobTitleUnid;            // 用户职务UNID
        private string jobTitleName;            // 用户职务名称
        private string userLigion;              // 用户政治面貌
        private string isTmpUser;               // 是否是临时用户
        private DateTime validityStartDate;     // 如果是临时用户，那么有效期开始时间
        private DateTime validityEndDate;       // 如果是临时用户，那么有效期结束时间
        private string field_1;                 // 用户的扩展信息1
        private string field_2;                 // 用户的扩展信息2
        private string field_3;                 // 用户的扩展信息3
        private string field_4;                 // 用户的扩展信息4
        private string field_5;                 // 用户的扩展信息5
        private string employeeID;
        private string departmentID;
        private string userType = T_CLIENT;     // 用户类型：0--普通用户(默认)，1--服务台用户，2--服务支持用户

        private IList groupLists;               // 用户所拥有的岗位列表
        private IList groupUnidLists;           // 用户所拥有岗位的Unid列表

        #endregion

        #region 属性定义

        /// <summary>
        /// 用户类型：0--普通用户(默认)，1--服务台用户，2--服务支持用户
        /// </summary>
        public string UserType
        {
            get { return userType; }
            set { userType = value; }
        }

        /// <summary>
        /// 用户类型的描述
        /// </summary>
        public string UserTypeDesc
        {
            get
            {
                if (this.userType == T_CLIENT)
                    return "普通用户";
                else if (this.userType == T_HELPDESK)
                    return "服务台";
                else if (this.userType == T_SUPPORT)
                    return "服务支持";
                else
                    return "未知类型";
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
                    return "活动";
                else if (this.UserStatus == UserStatuses.Disable)
                    return "禁用";
                else if (this.UserStatus == UserStatuses.Delete)
                    return "删除";
                else
                    return "未知状态";
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
        /// 取得人员所属岗位Unid的列表
        /// </summary>
        /// <returns>返回用户所属岗位Unid的列表</returns>
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

        #region 特殊用途

        private IDictionary privileges;
        /// <summary>
        /// 用户所拥有的权限列表
        /// </summary>
        public IDictionary Privileges
        {
            get { return privileges; }
            set { privileges = value; }
        }

        /// <summary>
        /// 判断当前用户是否拥有指定的权限
        /// </summary>
        /// <remarks>
        /// 注意，用户的权限是在当用户登录系统时由登录验证代码设置用户权限的privilegeMap的，本类不负责处理该事情。
        /// </remarks>
        /// <param name="privilegeCode">所要检测的权限代码</param>
        /// <returns>如果拥有则返回true，否则返回false</returns>
        public bool HasPrivilege(string privilegeCode)
        {
            if (null == this.privileges || this.privileges.Count == 0)
                return false;
            else
                return this.privileges.Contains(privilegeCode);
        }

        /// <summary>
        /// 对口令进行加密
        /// </summary>
        /// <param name="password">明文口令</param>
        /// <param name="key">特殊键值</param>
        /// <returns>加密后的口令</returns>
        public static string CreateEncryptPassword(string password, string key)
        {
            return "(" + MD5Utils.MD5String(password) + ")";
        }

        #endregion
    }
}
