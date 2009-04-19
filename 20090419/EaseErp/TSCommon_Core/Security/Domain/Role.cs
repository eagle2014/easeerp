using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Lib;

namespace TSCommon.Core.Security.Domain
{
    public enum RoleStatuses { Enable = 0, Disable = 1, Add = 2, Modified = 4, Delete = 8 };

    /// <summary>
    /// 角色Domain的定义
    /// </summary>
    public class Role : Entry
    {
        public static string RELATIONSHIP_CODE = "role";    // 关联关系的定义

        #region 字段定义
        
        private int roleStatus;                 // 角色状态，   0－启用     1－禁用     2－添加
                                                //              4－修改     8－删除
        private string name;                    // 角色名称
        private string code;                    // 角色代码
        private string level;                      // 角色级次编码
        private string levelName;                      // 角色级次名称

        public string LevelName
        {
            get { return levelName; }
            set { levelName = value; }
        }
        private string isInner;                 // 是否是内置角色， Y－是   N－否
        private string memo;                    // 备注信息
        private IList privileges;               // 该角色所拥有的权限信息列表

        #endregion

        #region 属性定义
        
        public RoleStatuses RoleStatus
        {
            get { return (RoleStatuses)roleStatus; }
            set { roleStatus = (int)value; }
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
        public string Level
        {
            get { return level; }
            set { level = value; }
        }
        public string IsInner
        {
            get { return isInner; }
            set { isInner = value; }
        }
        public string Memo
        {
            get { return memo; }
            set { memo = value; }
        }
        public IList Privileges
        {
            get {
                if (null == this.privileges)
                    this.privileges = new ArrayList();
                return privileges; 
            }
            set { privileges = value; }
        }
        
        #endregion

        /// <summary>
        /// 设置角色所拥有的权限信息
        /// </summary>
        /// <param name="ids"></param>
        public void SetPrivilegeIDs(string[] ids)
        {
            if (null == ids || ids.Length == 0)
            {
                this.privileges = new ArrayList();
            }
            else
            {
                IList privilegesList = new ArrayList();
                foreach (string id in ids)
                {
                    Privilege privilege = new Privilege();
                    privilege.ID = Int64.Parse(id);
                    privilegesList.Add(privilege);
                }
                this.privileges = privilegesList;
            }
        }
    }
}
