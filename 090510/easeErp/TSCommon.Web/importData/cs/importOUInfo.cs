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
using TSCommon.Core.Organize.Service;
using TSCommon.Core.Organize.Domain;
using System.Collections;

namespace TSCommon.Web.importData.cs
{
    public class importOUInfo:ImportDataBase
    {
        private ILog logger = LogManager.GetLogger(typeof(importOUInfo));
        private IList ouLevelList = null;

        #region 相关Service
        private IOUInfoService ouInfoService;
        private IOULevelService ouLevelService;
        public importOUInfo()
        {
            ouInfoService = (IOUInfoService)GetObject("OUInfoService");
            ouLevelService = (IOULevelService)GetObject("OULevelService");
            ouLevelList = ouLevelService.FindAll();
        }
        #endregion

        public override void Execute(string pDataFilePath)
        {
            logger.Debug("开始导入单位、部门等信息");
            IList<string> list = TextHelper.ReadTextByReadLine(pDataFilePath);
            foreach (string str in list)
            {
                string[] strArray = str.Split(';');
                OUInfo info = new OUInfo();
                info.ID = -1;
                info.IsTmpOU = Constants.YESNO_NO;
                info.OUStatus = OUStatuses.Enable;
                info.Name = strArray[0];
                info.Code = strArray[1];
                foreach (OULevel ouLevel in ouLevelList)
                {
                    if (strArray[2] == ouLevel.Code)
                    {
                        info.Level = ouLevel.Code;
                        info.LevelName = ouLevel.Name;
                        break;
                    }
                }
                info.OrderNo = strArray[3];
                info.Type = strArray[4];

                IList ouList = null;
                if (strArray.Length > 5 && strArray[5].Trim().Length > 0)
                {
                    ouList = ouInfoService.FindAll();
                    bool isHas = false;
                    foreach (OUInfo ouInfo in ouList)
                    {
                        if (ouInfo.FullName.Equals(strArray[5], StringComparison.OrdinalIgnoreCase))
                        {
                            if (info.Type.Equals(OUInfo.OT_DEPARTMENT, StringComparison.OrdinalIgnoreCase))
                            {
                                info.UnitName = ouInfo.Name;
                                info.UnitUnid = ouInfo.Unid;
                                info.UnitFullName = ouInfo.FullName;
                                info.UnitFullCode = ouInfo.FullCode;
                            }
                            else
                            {
                                info.ParentOUUnid = ouInfo.Unid;
                                info.FullCode = ouInfo.FullCode + "." + info.Code;
                                info.FullName = ouInfo.FullName + "." + info.Name;
                            }

                            isHas = true;
                            break;
                        }
                    }
                    if (!isHas)
                    {
                        foreach (OUInfo ouInfo in ouList)
                        {
                            if (ouInfo.Name.Equals(strArray[5], StringComparison.OrdinalIgnoreCase))
                            {
                                if (info.Type.Equals(OUInfo.OT_DEPARTMENT, StringComparison.OrdinalIgnoreCase))
                                {
                                    info.UnitName = ouInfo.Name;
                                    info.UnitUnid = ouInfo.Unid;
                                    info.UnitFullName = ouInfo.FullName;
                                    info.UnitFullCode = ouInfo.FullCode;
                                }
                                else
                                {
                                    info.ParentOUUnid = ouInfo.Unid;
                                    info.FullCode = ouInfo.FullCode + "." + info.Code;
                                    info.FullName = ouInfo.FullName + "." + info.Name;
                                }
                                isHas = true;
                                break;
                            }
                        }
                    }
                    if (!isHas)
                    {
                        if (info.Type.Equals(OUInfo.OT_DEPARTMENT, StringComparison.OrdinalIgnoreCase))
                        { }

                        info.FullCode = info.Code;
                        info.FullName = info.Name;
                    }
                }
                else
                {
                    if (info.Type.Equals(OUInfo.OT_DEPARTMENT, StringComparison.OrdinalIgnoreCase))
                    { 
                    }

                    info.FullCode = info.Code;
                    info.FullName = info.Name;
                }

                // 判断是否具有上级部门
                if (strArray.Length > 6 && strArray[6].Trim().Length > 0 && info.Type.Equals(OUInfo.OT_DEPARTMENT, StringComparison.OrdinalIgnoreCase))
                {
                    bool isHas = false;
                    foreach (OUInfo ouInfo in ouList)
                    {
                        if (ouInfo.FullName.Equals(strArray[6], StringComparison.OrdinalIgnoreCase) && ouInfo.UnitUnid.Equals(info.UnitUnid, StringComparison.OrdinalIgnoreCase))
                        {
                            info.ParentOUUnid = ouInfo.Unid;
                            info.FullCode = ouInfo.FullCode + "." + info.Code;
                            info.FullName = ouInfo.FullName + "." + info.Name;
                            isHas = true;
                            break;
                        }

                    }
                    if (!isHas)
                    {
                        foreach (OUInfo ouInfo in ouList)
                        {
                            if (ouInfo.Name.Equals(strArray[6], StringComparison.OrdinalIgnoreCase) && ouInfo.UnitUnid.Equals(info.UnitUnid, StringComparison.OrdinalIgnoreCase))
                            {
                                info.ParentOUUnid = ouInfo.Unid;
                                info.FullCode = ouInfo.FullCode + "." + info.Code;
                                info.FullName = ouInfo.FullName + "." + info.Name;
                                isHas = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (info.Type.Equals(OUInfo.OT_DEPARTMENT, StringComparison.OrdinalIgnoreCase))
                    {
                        info.FullCode = info.UnitFullCode + "." + info.Code;
                        info.FullName = info.UnitFullName + "." + info.Name;
                    }
                }
                ouInfoService.Save(info);
            }
        }
    }
}
