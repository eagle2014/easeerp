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

namespace TSCommon.Web.importData.cs
{
    public class importGroupRank:ImportDataBase
    {
        private ILog logger = LogManager.GetLogger(typeof(importGroupRank));
        public override void Execute(string pDataFilePath)
        {
            logger.Debug("开始导入岗位级别");
        }
    }
}
