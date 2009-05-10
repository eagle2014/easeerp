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
using System.Collections;
using TSCommon.Core.Organize.Service;
using TSCommon.Core.Security.Service;
using TSCommon.Core.Organize.Domain;
using TSLib.Utils;
using System.Collections.Generic;
using TSCommon.Core.Security.Domain;
using TSCommon.Core.Organize.RelationShips;
using TSCommon.Core.Organize.Dao;

namespace TSCommon.Web.importData.cs
{
    public class importGroup:ImportDataBase
    {
        private ILog logger = LogManager.GetLogger(typeof(importGroup));
        private IList ouInfoList = null;
        private IList roleList = null;
        #region 相关Service
        private IOUInfoService ouInfoService;
        private IRoleService roleService;
        private IGroupDao groupDao;
        private IRelationShipService relationShipService;
        public importGroup()
        {
            ouInfoService = (IOUInfoService)GetObject("OUInfoService");
            roleService = (IRoleService)GetObject("RoleService");
            groupDao = (IGroupDao)GetObject("GroupDao");
            relationShipService = (IRelationShipService)GetObject("RelationShipService");
            ouInfoList = ouInfoService.FindAll();
            roleList=roleService.FindAll();
        }
        #endregion
        public override void Execute(string pDataFilePath)
        {
            logger.Debug("开始导入岗位");
            IList<string> list = TextHelper.ReadTextByReadLine(pDataFilePath);
            foreach (string str in list)
            {
                string[] values = str.Split(';');
                Group info = new Group();
                info.ID = -1;
                info.IsInner = Constants.YESNO_YES;
                info.GroupStatus = GroupStatuses.Enable;
                info.Name = values[0];
                info.Code = values[1];
                info.IsCanDispatch = values[3];
                info.IsInner = values[5];
                foreach (OUInfo ouInfo in ouInfoList)
                {
                    if (ouInfo.Code.Equals(values[2], StringComparison.OrdinalIgnoreCase))
                    {
                        info.OUCode = ouInfo.Code;
                        info.OUFullName = ouInfo.FullName;
                        info.OUFullCode = ouInfo.FullCode;
                        info.OUName = ouInfo.Name;
                        info.OUUnid = ouInfo.Unid;
                        break;
                    }
                }

                if (values.Length > 4)
                {
                    string relation = Group.RELATIONSHIP_CODE + "." + Role.RELATIONSHIP_CODE;
                    string[] roles = System.Text.RegularExpressions.Regex.Split(values[4], ",");
                    foreach (string roleCode in roles)
                    {
                        foreach (Role role in roleList)
                        {
                            if (roleCode.Equals(role.Code, StringComparison.OrdinalIgnoreCase))
                            {
                                RelationShip relationShip = new RelationShip(info.Unid, Group.RELATIONSHIP_CODE, role.Unid, Role.RELATIONSHIP_CODE, relation);
                                relationShipService.Save(relationShip);
                                //TSCommon_Core.Organize.Domain.RelationShip relationship = new TSCommon_Core.Organize.Domain.RelationShip();
                                //relationship.ParentUnid = info.Unid;
                                //relationship.ParentType = Group.RELATIONSHIP_CODE;
                                //relationship.ChildUnid = role.Unid;
                                //relationship.ChildType = Role.RELATIONSHIP_CODE;
                                //relationship.RelationShipType = "group.role";
                                //this.relationShipService.Save(relationship);
                            }
                        }
                    }
                }
                groupDao.Save(info);
            }
        }
    }
}
