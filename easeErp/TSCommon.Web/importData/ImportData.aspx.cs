using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using TSCommon.Web.importData.cs;
using TSLib.Utils;
using TSLib.ImportData;

namespace TSCommon.Web
{
    public partial class ImportData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnimportdata_Click(object sender, EventArgs e)
        {
            ExecuteImportData da = new ExecuteImportData(FileUtils.GetAbsolutePathName(ConfigurationManager.AppSettings["ImportFileFileListPath"]));
            da.Execute();
        }
    }
}
