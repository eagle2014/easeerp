using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Lib;

namespace TSCommon.Core.Security.Domain
{
    public enum RoleStatuses { Enable = 0, Disable = 1, Add = 2, Modified = 4, Delete = 8 };

    /// <summary>
    /// ��ɫDomain�Ķ���
    /// </summary>
    public class Role : Entry
    {
        public static string RELATIONSHIP_CODE = "role";    // ������ϵ�Ķ���

        #region �ֶζ���
        
        private int roleStatus;                 // ��ɫ״̬��   0������     1������     2�����
                                                //              4���޸�     8��ɾ��
        private string name;                    // ��ɫ����
        private string code;                    // ��ɫ����
        private string level;                      // ��ɫ���α���
        private string levelName;                      // ��ɫ��������

        public string LevelName
        {
            get { return levelName; }
            set { levelName = value; }
        }
        private string isInner;                 // �Ƿ������ý�ɫ�� Y����   N����
        private string memo;                    // ��ע��Ϣ
        private IList privileges;               // �ý�ɫ��ӵ�е�Ȩ����Ϣ�б�

        #endregion

        #region ���Զ���
        
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
        /// ���ý�ɫ��ӵ�е�Ȩ����Ϣ
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
