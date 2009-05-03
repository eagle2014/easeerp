using System;
using System.Collections.Generic;
using System.Text;
using Lib;
using TSCommon.Core.Organize.Domain;

namespace TSCommon.Core
{
    /// <summary>
    /// �ĵ����࣬���������ߺ��޸���
    /// </summary>
    public class FileEntry:Entry
    {
        private User author;                    // �ļ�����
        private User lastChanger;               // �ļ�����޸���
        private DateTime fileDate;                  // �ļ���������
        private DateTime lastModifiedDate;          // ����޸�ʱ��
        /// <summary>
        /// �ļ�����
        /// </summary>
        public User Author
        {
            get { return author == null ? new User() : author; }
            set { author = value; }
        }

        /// <summary>
        /// �ļ�����޸���
        /// </summary>
        public User LastChanger
        {
            get { return lastChanger == null ? new User() : lastChanger; }
            set { lastChanger = value; }
        }

        /// <summary>
        /// �ļ�����
        /// </summary>
        public DateTime FileDate
        {
            get { return fileDate; }
            set { fileDate = value; }
        }

        /// <summary>
        /// �ļ����ڣ����ظ�ʽΪyyyy-MM-dd hh:mm��ʱ���ַ���
        /// </summary>
        public string FileDateTime
        {
            get
            {
                return this.fileDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {

            }
        }

        /// <summary>
        /// �ļ�����޸�ʱ�䣬Ҫ���ø����Կɵ���SetLastModifiedInfo(changer)��
        /// </summary>
        public DateTime LastModifiedDate
        {
            get { return lastModifiedDate; }
            set { lastModifiedDate = value; }
        }

        /// <summary>
        /// �ļ�����޸�ʱ�䣬���ظ�ʽΪyyyy-MM-dd hh:mm��ʱ���ַ�����Ҫ���ø����Կɵ���SetLastModifiedInfo(changer)��
        /// </summary>
        public string LastModifiedDateTime
        {
            get
            {
                return this.lastModifiedDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {

            }
        }

        /// <summary>
        /// Ĭ�Ϲ��캯��
        /// </summary>        
        public FileEntry()
        {
        }

        /// <summary>
        /// ����ָ���û��½�һ���ĵ�
        /// </summary>
        /// <param name="userInfo">�û���Ϣ</param>
        public FileEntry(User userInfo)
        {
            this.Init(userInfo);
        }
        /// <summary>
        /// ʹ��ָ�����û���Ϣ��ʼ����ǰ�ĵ�������<c>FileDate</c>��������Ϊ��ǰʱ��
        /// </summary>
        /// <param name="userInfo">�û���Ϣ</param>
        public virtual void Init(User userInfo)
        {
            SetAuthorInfo(userInfo);

            this.lastChanger = userInfo;
            DateTime now = DateTime.Now;
            this.fileDate = now;
            this.lastModifiedDate = now;
        }

        /// <summary>
        /// ��������޸ĵ���Ϣ
        /// </summary>
        /// <param name="changer">�޸���</param>
        public void SetLastModifiedInfo(User changer)
        {
            this.lastChanger = changer;
            this.lastModifiedDate = DateTime.Now;
        }

        /// <summary>
        /// ����������Ϣ
        /// </summary>
        /// <param name="author">����</param>
        public void SetAuthorInfo(User author)
        {
            this.author = author;
        }
    }
}
