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
    public class importModel:ImportDataBase
    {
        private ILog logger = LogManager.GetLogger(typeof(importModel));
        private IModelService modelService;
        public importModel()
        {
            modelService =(IModelService) GetObject("ModelService");
        }
        public override void  Execute(string pDataFilePath)
        {
            logger.Debug("��ʼ����ģ������");
            IList<string> list = TextHelper.ReadTextByReadLine(FileUtils.GetAbsolutePathName(pDataFilePath));
            foreach (string str in list)
            {
                Model model = new Model();
                string[] tmpArray=str.Split(';');
                if (null == modelService.GetByCode(tmpArray[1]))//����û�е�ģ��
                {
                    if (!string.IsNullOrEmpty(tmpArray[4]))//�и�ģ��
                    {
                        Model tmpModel = modelService.GetByCode(tmpArray[4]);
                        if (null != tmpModel)
                            model.ParentID = tmpModel.ID;
                    }
                    model.Name = tmpArray[0];
                    model.Code = tmpArray[1];
                    if (tmpArray[2] == "0")
                        model.Type = ModelTypes.Master;
                    else if (tmpArray[2] == "1")
                        model.Type = ModelTypes.SubModel;
                    else
                        model.Type = ModelTypes.Undefined;
                    model.OrderNo = tmpArray[3];
                    model.IsInner = Constants.YESNO_YES;
                    modelService.Save(model);
                }
            }
        }
    }
}
