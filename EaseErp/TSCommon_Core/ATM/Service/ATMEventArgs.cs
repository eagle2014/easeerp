using System;
using System.Collections.Generic;
using System.Text;

namespace TSCommon_Core.ATM.Service
{
    public class ATMEventArgs : EventArgs
    {
        private Dictionary<string, string> atmData;   // 要操作的附件信息（标识,fileName）
        private string oldParentUnid;                        // 原来的父unid
        private string newParentUnid;                        // 要转移到的父unid
        private string proceAction;                          // 是拷比、剪切或者删除 （Copy，Move，Delete）

        public ATMEventArgs(Dictionary<string, string> tempATMData, string tempOldParentUnid, string tempNewParentUnid, string tempProceAction)
        {
            this.ATMData = tempATMData;
            this.OldParentUnid = tempOldParentUnid;
            this.NewParentUnid = tempNewParentUnid;
            this.ProceAction = tempProceAction;
        }
        /// <summary>
        /// 原来的父unid
        /// </summary>
        public string OldParentUnid
        {
            get { return oldParentUnid; }
            set { oldParentUnid = value; }
        }

        /// <summary>
        /// 要操作的附件信息（标识,fileName）
        /// </summary>
        public Dictionary<string, string> ATMData
        {
            get { return atmData; }
            set { atmData = value; }
        }

        /// <summary>
        /// 要转移到的父unid
        /// </summary>
        public string NewParentUnid
        {
            get { return newParentUnid; }
            set { newParentUnid = value; }
        }

        /// <summary>
        /// 是拷比、剪切或者删除 （Copy，Move，Delete）
        /// </summary>
        public string ProceAction
        {
            get { return proceAction; }
            set { proceAction = value; }
        }
    }
}
