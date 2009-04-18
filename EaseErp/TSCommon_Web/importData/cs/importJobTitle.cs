using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using log4net;
using TSLib.ImportData;
using System.Collections.Generic;
using TSLib.Utils;
using TSCommon_Core.Organize.Service;
using TSCommon_Core.Organize.Domain;

namespace TSCommon_Web.importData.cs
{
    public class importJobTitle:ImportDataBase
    {
        private ILog logger = LogManager.GetLogger(typeof(importJobTitle));

        #region 相关Service
        private IJobTitleService jobTitleService;
        public importJobTitle()
        {
            jobTitleService = (IJobTitleService)GetObject("JobTitleService");
        }
        #endregion

        public override void Execute(string pDataFilePath)
        {
            logger.Debug("开始导入职务");
            IList<string> list = TextHelper.ReadTextByReadLine(pDataFilePath);
            foreach (string str in list)
            {
                string[] strArray = str.Split(';');
                JobTitle jobTitle = new JobTitle();
                jobTitle.ID = -1;
                jobTitle.Name = strArray[0];
                jobTitle.Code = strArray[1];
                jobTitle.Level = strArray[2];
                jobTitleService.Save(jobTitle);
            }
        }
    }
}
