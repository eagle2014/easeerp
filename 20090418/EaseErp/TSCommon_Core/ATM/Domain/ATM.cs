using System;
using System.Collections.Generic;
using System.Text;
using Lib;

namespace TSCommon_Core.ATM.Domain
{
    /// <summary>
    /// 附件实体类
    /// </summary>
    public class ATM:FileEntry
    {
        #region 字段定义

        private string subject;     // 附件中文件名称
        private string fileName;	// 附件文件保存的名字
        private string type;        // 附件类别
        private string fileFormat;  // 附件中文件的格式
        private string fileSize;    // 附件的大小
        private string parentUnid;	// 所属文件的unid;
        private string savePath;    // 附件存放路径

        #endregion

        #region 属性定义
        /// <summary>
        /// 附件中文件名称
        /// </summary>
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        /// <summary>
        /// 附件文件保存的名字
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        /// <summary>
        /// 附件类别
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        /// <summary>
        /// 附件中文件的格式
        /// </summary>
        public string FileFormat
        {
            get { return fileFormat; }
            set { fileFormat = value; }
        }
        /// <summary>
        /// 附件的大小
        /// </summary>
        public string FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }
        /// <summary>
        /// 所属文件的unid
        /// </summary>
        public string ParentUnid
        {
            get { return parentUnid; }
            set { parentUnid = value; }
        }
        /// <summary>
        /// 附件存放路径
        /// </summary>
        public string SavePath
        {
            get { return savePath; }
            set { savePath = value; }
        }
        #endregion
    }
}
