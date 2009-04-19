using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Lib;
using TSLib.Utils;

namespace TSCommon_Core.Organize.Domain
{
    /// <summary>
    /// 岗位状态定义
    /// </summary>
    public enum GroupStatuses { Undefined = -1, Enable = 0, Disable = 1, Add = 2, Modified = 4, Delete = 8 };

    /// <summary>
    /// 用于岗位状态的Struts转换器
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
    /// 岗位配置的Domain
    /// </summary>
    public class Group : Entry
    {
        public static string RELATIONSHIP_CODE = "group";    // 关联关系的定义

        #region 字段定义
        
        private int groupStatus;                // 群组状态，   0－启用     1－禁用     2－添加
                                                //              4－修改     8－删除
        private string name;                    // 群组名称
        private string code;                    // 群组代码
        private string rankUnid;                // 群组级别的代码
        private string rankName;                // 群组级别的名称
        private string isCanDispatch;           // 是否是可以进行派单， Y－是   N－否
        private string isInner;                 // 是否是内置岗位， Y－是   N－否
        private string ouUnid;                  // 岗位隶属的组织的唯一标识
        private string ouName;                  // 岗位隶属的组织的名称
        private string ouCode;                  // 岗位隶属的组织的编码
        private string ouFullName;              // 岗位隶属的组织的全称
        private string ouFullCode;              // 岗位隶属的组织的全编码
        private string memo;                    // 备注信息
        private string field_1;                 // 群组的扩展信息1
        private string field_2;                 // 群组的扩展信息2
        private string field_3;                 // 群组的扩展信息3
        private string field_4;                 // 群组的扩展信息4
        private IList roleLists;                // 群组所拥有的角色列表
        private IList roleUnidLists;            // 群组所拥有的角色列表
        private IList userInfoLists;            // 群组所拥有的人员列表
        private IList userInfoUnidLists;        // 群组所拥有的人员列表

        #endregion

        #region 属性定义

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
