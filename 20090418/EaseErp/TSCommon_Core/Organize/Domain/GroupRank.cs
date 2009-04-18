using System;
using System.Collections.Generic;
using System.Text;
using Lib;

namespace TSCommon_Core.Organize.Domain
{
    /// <summary>
    /// 岗位级别Domain定义
    /// </summary>
    public class GroupRank : Entry
    {
        public static string RELATIONSHIP_CODE = "groupRank";    // 关联关系的定义

        #region 字段定义

        private string name;            // 岗位级别的名称
        private string parentRankUnid;  // 上级岗位的Unid
        private string memo;            // 对岗位级别的描述

        #endregion

        #region 属性定义

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string ParentRankUnid
        {
            get { return parentRankUnid; }
            set { parentRankUnid = value; }
        }
        public string Memo
        {
            get { return memo; }
            set { memo = value; }
        }

        #endregion
    }
}
