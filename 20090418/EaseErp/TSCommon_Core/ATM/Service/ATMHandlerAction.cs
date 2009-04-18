using System;
using System.Collections.Generic;
using System.Text;
using Common.Logging;
using TSCommon_Core.ATM.Service;
using TSLib.Utils;
using TSLib;

namespace TSCommon_Core.ATM.Service
{
    public class ATMHandlerAction
    {
        private static ILog logger = LogManager.GetLogger(typeof(ATMHandlerAction));

        private string parentUnid;
        private string proceAction;
        private string oldParentUnid;

        public void ActionAttachmentFile(object sender, ATMEventArgs e)
        {
            try
            {
                Dictionary<string, string> attachmentData = e.ATMData;
                parentUnid = e.NewParentUnid;
                proceAction = e.ProceAction;
                oldParentUnid = e.OldParentUnid;

                if (attachmentData == null || attachmentData.Count == 0)
                    return;
                Dictionary<string, string>.ValueCollection fileNameCollection = attachmentData.Values;
                Dictionary<string, string>.ValueCollection.Enumerator fileNameEnumerator = fileNameCollection.GetEnumerator();

                while (fileNameEnumerator.MoveNext())
                {
                    string strParentUnid = oldParentUnid;
                    string strFileName = fileNameEnumerator.Current.ToString();

                    string oldAbsoluteFile = this.GetabsolutePath(strParentUnid, strFileName);
                    string newAbsolutePath = this.GetabsolutePath(parentUnid, strFileName);
                    logger.Fatal("oldAbsoluteFile = " + oldAbsoluteFile);
                    logger.Fatal("newAbsolutePath = " + newAbsolutePath);
                    if (!System.IO.File.Exists(oldAbsoluteFile))
                        throw new Exception();
                    if (proceAction.Equals("Copy"))
                        Copy(parentUnid, oldAbsoluteFile, newAbsolutePath);
                    else if (proceAction.Equals("Move"))
                        Move(parentUnid, oldAbsoluteFile, newAbsolutePath);
                    else if (proceAction.Equals("Delete"))
                        Delete(oldAbsoluteFile);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message);
            }
        }

        public void Copy(string parentUnid, string oldFilePath, string newPath)
        {
            string strRoot = FileUtils.GetAbsolutePathName(SimpleResourceHelper.GetString("ATM.DIRECTORY"));//
            string path = strRoot + "\\" + parentUnid;
            path = this.FormatePath(this.GetCurrentRunPath()) + path;
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            System.IO.File.Copy(oldFilePath, newPath, true);
        }

        public void Move(string parentUnid, string oldFilePath, string newPath)
        {
            string strRoot = FileUtils.GetAbsolutePathName(SimpleResourceHelper.GetString("ATM.DIRECTORY"));
            string path = strRoot + "\\" + parentUnid;
            path = this.FormatePath(this.GetCurrentRunPath()) + path;
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            System.IO.File.Move(oldFilePath, newPath);
        }

        public void Delete(string filePath)
        {
            System.IO.File.Delete(filePath);
        }

        private string GetabsolutePath(string parentUnid, string fileName)
        {
            string strRoot = FileUtils.GetAbsolutePathName(SimpleResourceHelper.GetString("ATM.DIRECTORY"));
            string path = strRoot + "\\" + parentUnid + "\\" + fileName;
            return path;
        }

        /// <summary>
        /// 获得当前运行路径
        /// </summary>
        /// <returns></returns>
        private string GetCurrentRunPath()
        {
            return this.FormatePath(System.AppDomain.CurrentDomain.BaseDirectory);
        }

        /// <summary>
        /// 格式化路径，如果路径没有以"\"结束，则加上"\"
        /// </summary>
        /// <param name="p_strPath"></param>
        /// <returns></returns>
        protected string FormatePath(string p_strPath)
        {
            if (!p_strPath.EndsWith("\\"))
                p_strPath += "\\";
            return p_strPath;
        }
    }
}
