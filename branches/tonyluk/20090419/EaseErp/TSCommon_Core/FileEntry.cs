using System;
using System.Collections.Generic;
using System.Text;
using Lib;
using TSCommon.Core.Organize.Domain;

namespace TSCommon.Core
{
    /// <summary>
    /// 文档基类，包括创建者和修改者
    /// </summary>
    public class FileEntry:Entry
    {
        private User author;                    // 文件作者
        private User lastChanger;               // 文件最后修改人
        private DateTime fileDate;                  // 文件创建日期
        private DateTime lastModifiedDate;          // 最后修改时间
        /// <summary>
        /// 文件作者
        /// </summary>
        public User Author
        {
            get { return author == null ? new User() : author; }
            set { author = value; }
        }

        /// <summary>
        /// 文件最后修改人
        /// </summary>
        public User LastChanger
        {
            get { return lastChanger == null ? new User() : lastChanger; }
            set { lastChanger = value; }
        }

        /// <summary>
        /// 文件日期
        /// </summary>
        public DateTime FileDate
        {
            get { return fileDate; }
            set { fileDate = value; }
        }

        /// <summary>
        /// 文件日期，返回格式为yyyy-MM-dd hh:mm的时间字符串
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
        /// 文件最后修改时间，要设置该属性可调用SetLastModifiedInfo(changer)。
        /// </summary>
        public DateTime LastModifiedDate
        {
            get { return lastModifiedDate; }
            set { lastModifiedDate = value; }
        }

        /// <summary>
        /// 文件最后修改时间，返回格式为yyyy-MM-dd hh:mm的时间字符串，要设置该属性可调用SetLastModifiedInfo(changer)。
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
        /// 默认构造函数
        /// </summary>        
        public FileEntry()
        {
        }

        /// <summary>
        /// 根据指定用户新建一个文档
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        public FileEntry(User userInfo)
        {
            this.Init(userInfo);
        }
        /// <summary>
        /// 使用指定的用户信息初始化当前文档，并将<c>FileDate</c>属性设置为当前时间
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        public virtual void Init(User userInfo)
        {
            SetAuthorInfo(userInfo);

            this.lastChanger = userInfo;
            DateTime now = DateTime.Now;
            this.fileDate = now;
            this.lastModifiedDate = now;
        }

        /// <summary>
        /// 设置最后修改的信息
        /// </summary>
        /// <param name="changer">修改人</param>
        public void SetLastModifiedInfo(User changer)
        {
            this.lastChanger = changer;
            this.lastModifiedDate = DateTime.Now;
        }

        /// <summary>
        /// 设置作者信息
        /// </summary>
        /// <param name="author">作者</param>
        public void SetAuthorInfo(User author)
        {
            this.author = author;
        }
    }
}
