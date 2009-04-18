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
using TSCommon_Core.Organize.Service;
using System.Collections.Generic;
using TSLib.Utils;
using TSCommon_Core.Organize.Domain;

namespace TSCommon_Web.importData.cs
{
    public class importOULevel:ImportDataBase
    {
        private ILog logger = LogManager.GetLogger(typeof(importOULevel));

        #region 相关Service
        private IOULevelService ouLevelService;
        public importOULevel()
        {
            ouLevelService = (IOULevelService)GetObject("OULevelService");
        }
        #endregion

        public override void Execute(string pDataFilePath)
        {
            logger.Debug("开始导入单位级别");
            IList<string> list = TextHelper.ReadTextByReadLine(pDataFilePath);
            foreach (string str in list)
            {
                string[] strArray=str.Split(';');
                OULevel ouLevel = new OULevel();
                ouLevel.Name = strArray[0];
                ouLevel.Code = strArray[1];
                ouLevelService.Save(ouLevel);
            }
        }
    }
}
