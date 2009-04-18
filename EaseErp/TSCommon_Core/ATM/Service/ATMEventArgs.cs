using System;
using System.Collections.Generic;
using System.Text;

namespace TSCommon_Core.ATM.Service
{
    public class ATMEventArgs : EventArgs
    {
        private Dictionary<string, string> atmData;   // Ҫ�����ĸ�����Ϣ����ʶ,fileName��
        private string oldParentUnid;                        // ԭ���ĸ�unid
        private string newParentUnid;                        // Ҫת�Ƶ��ĸ�unid
        private string proceAction;                          // �ǿ��ȡ����л���ɾ�� ��Copy��Move��Delete��

        public ATMEventArgs(Dictionary<string, string> tempATMData, string tempOldParentUnid, string tempNewParentUnid, string tempProceAction)
        {
            this.ATMData = tempATMData;
            this.OldParentUnid = tempOldParentUnid;
            this.NewParentUnid = tempNewParentUnid;
            this.ProceAction = tempProceAction;
        }
        /// <summary>
        /// ԭ���ĸ�unid
        /// </summary>
        public string OldParentUnid
        {
            get { return oldParentUnid; }
            set { oldParentUnid = value; }
        }

        /// <summary>
        /// Ҫ�����ĸ�����Ϣ����ʶ,fileName��
        /// </summary>
        public Dictionary<string, string> ATMData
        {
            get { return atmData; }
            set { atmData = value; }
        }

        /// <summary>
        /// Ҫת�Ƶ��ĸ�unid
        /// </summary>
        public string NewParentUnid
        {
            get { return newParentUnid; }
            set { newParentUnid = value; }
        }

        /// <summary>
        /// �ǿ��ȡ����л���ɾ�� ��Copy��Move��Delete��
        /// </summary>
        public string ProceAction
        {
            get { return proceAction; }
            set { proceAction = value; }
        }
    }
}
