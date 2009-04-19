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
using TSCommon_Core.Organize.Service;
using TSLib.Utils;
using TSCommon_Core.Organize.Domain;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using TSCommon_Core.Organize.RelationShips;
using TSCommon_Core.Organize.Dao;

namespace TSCommon_Web.importData.cs
{
    public class importUser:ImportDataBase
    {
        private ILog logger = LogManager.GetLogger(typeof(importUser));
        private IList ouInfoList = null;
        private IList groupList = null;
        private IList jobTitleList = null;
        private static string WILDCARD = "*";
        private static string REGEX = "\\w*";

        #region ���Service
        private IOUInfoService ouInfoService;
        private IGroupService groupService;
        private IJobTitleService jobTitleService;
        private IUserDao userDao;
        private IRelationShipService relationShipService;
        public importUser()
        {
            ouInfoService = (IOUInfoService)GetObject("OUInfoService");
            groupService = (IGroupService)GetObject("GroupService");
            jobTitleService = (IJobTitleService)GetObject("JobTitleService");
            userDao = (IUserDao)GetObject("UserDao");
            relationShipService = (IRelationShipService)GetObject("RelationShipService");
            ouInfoList = ouInfoService.FindAllByType(null,OUInfo.OT_DEPARTMENT);
            groupList = groupService.FindAll();
            jobTitleList = jobTitleService.FindAll();
        }
        #endregion

        public override void Execute(string pDataFilePath)
        {
            logger.Debug("��ʼ�����û�����");
            IList<string> list = TextHelper.ReadTextByReadLine(pDataFilePath);
            foreach (string str in list)
            {
                string[] strArray = str.Split(';');
                User user = new User();
                user.ID = -1;
                user.Name = strArray[0];
                user.FullName = user.Name;
                user.LoginID = strArray[1];
                user.OrderNo = strArray[2];
                user.TelephoneNo = strArray[6];
                user.Email = strArray[7];
                user.Mobile = strArray[8];
                if (strArray.Length > 9 && !"END".Equals(strArray[9]))
                    user.UserType = strArray[9];
                else
                    user.UserType = "0";    // Ĭ��Ϊ��ͨ�û�

                // ��������
                foreach (OUInfo ouInfo in ouInfoList)
                {
                    if (strArray[3].Equals(ouInfo.Code, StringComparison.OrdinalIgnoreCase))
                    {
                        user.OUCode = ouInfo.Code;
                        user.OUFullName = ouInfo.FullName;
                        user.OUName = ouInfo.Name;
                        user.OUUnid = ouInfo.Unid;
                        if (ouInfo.Type.Equals(OUInfo.OT_DEPARTMENT, StringComparison.OrdinalIgnoreCase))
                        {
                            user.UnitName = ouInfo.UnitName;
                            user.UnitUnid = ouInfo.UnitUnid;
                            user.UnitFullCode = ouInfo.UnitFullCode;
                            user.UnitFullName = ouInfo.UnitFullName;
                        }
                        else
                        {
                            user.UnitName = ouInfo.Name;
                            user.UnitUnid = ouInfo.Unid;
                            user.UnitFullCode = ouInfo.FullCode;
                            user.UnitFullName = ouInfo.FullName;
                        }
                        break;
                    }
                }

                // ְ��
                foreach (JobTitle jobTitle in jobTitleList)
                {
                    if (strArray[5].Equals(jobTitle.Code, StringComparison.OrdinalIgnoreCase))
                    {
                        user.JobTitleName = jobTitle.Name;
                        user.JobTitleUnid = jobTitle.Unid;
                        break;
                    }
                }

                // ��λ
                if (strArray[4].Trim().Length > 0)
                {
                    string relation = TSCommon_Core.Organize.Domain.Group.RELATIONSHIP_CODE + "." + User.RELATIONSHIP_CODE;
                    string[] groups = System.Text.RegularExpressions.Regex.Split(strArray[4], ",");
                    for (int i = 0; i < groups.Length; i++)
                    {
                        groups[i] = groups[i].Replace(WILDCARD, REGEX);// ƥ��[a-z,A-Z,_,0-9]
                    }
                    //foreach (string groupCode in groups)
                    //{
                    //    foreach (TSCommon_Core.Organize.Domain.Group group in groupList)
                    //    {
                    //        if (groupCode.Equals(group.Code, StringComparison.OrdinalIgnoreCase))
                    //        {
                    //            RelationShip relationShip = new RelationShip(group.Unid, TSCommon_Core.Organize.Domain.Group.RELATIONSHIP_CODE, user.Unid, User.RELATIONSHIP_CODE,
                    //                                                         relation);
                    //            this.relationShipService.Save(relationShip);
                    //        }
                    //    }
                    //}

                    bool onError = false;
                    for (int i = 0; i < groups.Length; i++)
                    {
                        onError = true;
                        foreach (TSCommon_Core.Organize.Domain.Group group in groupList)
                        {
                            if (Regex.IsMatch(group.Code, groups[i]))
                            {
                                RelationShip relationShip = new RelationShip(group.Unid, TSCommon_Core.Organize.Domain.Group.RELATIONSHIP_CODE,
                                    user.Unid, User.RELATIONSHIP_CODE, relation);
                                this.relationShipService.Save(relationShip);
                                onError = false;
                            }
                        }
                        if (onError)
                        {
                            logger.Error("��Ա��" + user.Name + "[" + user.LoginID + "]����Ҫӵ�еĸ�λ��"
                                    + groups[i].Replace(REGEX, WILDCARD) + "����ϵͳ���Ҳ�����Ӧ��ƥ�������Ա����ӵ�иø�λ!");
                        }
                    }
                }

                //
                user.Password = "(" + MD5Utils.MD5String("666666") + ")";
                user.IsTmpUser = Constants.YESNO_NO;
                user.UserStatus = UserStatuses.Enable;
                user.ValidityEndDate = DateTime.Now;
                user.ValidityStartDate = DateTime.Now;
                userDao.Save(user);
            }
        }
    }
}
