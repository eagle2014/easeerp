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
using TSCommon.Core.Security.Domain;
using TSCommon.Core.Security.Service;

namespace TSCommon.Web.importData.cs
{
    public class importPrivilege:ImportDataBase
    {
        private ILog logger = LogManager.GetLogger(typeof(importPrivilege));

        #region　相关Service
        private IModelService modelService;
        private IPrivilegeService privilegeService;

        public importPrivilege()
        {
            modelService = (IModelService)GetObject("ModelService");
            privilegeService = (IPrivilegeService)GetObject("PrivilegeService");
        }
        #endregion

        public override void Execute(string pDataFilePath)
        {
            logger.Debug("开始导入权限数据");
            IList<string> list = TextHelper.ReadTextByReadLine(FileUtils.GetAbsolutePathName(pDataFilePath));
            foreach (string str in list)
            {
                string[] strArray=str.Split(';');
                Privilege privilege = privilegeService.LoadByCode(strArray[1]);
                if (null == privilege)
                {
                    Model model = modelService.GetByCode(strArray[3]);
                    privilege = new Privilege();
                    privilege.Name = strArray[0];
                    privilege.Code = strArray[1];
                    privilege.OrderNo = strArray[2];
                    privilege.ModelID = model.ID;
                    privilege.Type = strArray[4];
                    privilege.UrlPath = strArray[5];
                    if (!string.IsNullOrEmpty(strArray[6]) && strArray[6].Length==32)
                    {
                        privilege.Unid = strArray[6];
                    }
                    privilege.IsInner = Constants.YESNO_YES;
                    privilegeService.Save(privilege);
                }
            }
        }
    }
}
