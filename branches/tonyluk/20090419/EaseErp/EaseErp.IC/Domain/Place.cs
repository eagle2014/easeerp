using System;
using System.Collections.Generic;
using System.Text;
using Lib;

namespace EaseErp.IC.Domain
{
    /// <summary>
    /// �ط�ʵ��
    /// </summary>
    public class Place : Entry
    {
        private string name;
        private string code;
        private string memo;
        private Place parent;
        /// <summary>
        /// ����
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string Code
        {
            get { return this.code; }
            set { this.code = value; }
        }
        /// <summary>
        /// ��ע
        /// </summary>
        public string Memo
        {
            get { return this.memo; }
            set { this.memo = value; }
        }
        /// <summary>
        /// ������
        /// </summary>
        public Place Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }
    }
}
