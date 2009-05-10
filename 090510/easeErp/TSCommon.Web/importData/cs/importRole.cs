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
using System.Collections;
using TSCommon.Core.Organize.Domain;
using TSCommon.Core.Organize.Service;
using TSCommon.Core.Security.Service;
using System.Text.RegularExpressions;

namespace TSCommon.Web.importData.cs
{
    public class importRole : ImportDataBase
    {       
        private IList privilegeList = null;
        private static ILog logger = LogManager.GetLogger(typeof(importRole));

        private static String WILDCARD = "*";

        private static String REGEX = "\\w*";
        private IDictionary<string, OULevel> allOULevel = new Dictionary<string, OULevel>();

         #region 相关Service
        private IOULevelService ouLevelService;
        private IPrivilegeService privilegeService;
        private IRoleService roleService;
        public importRole()
        {
            ouLevelService = (IOULevelService)GetObject("OULevelService");
            privilegeService = (IPrivilegeService)GetObject("PrivilegeService");
            roleService = (IRoleService)GetObject("RoleService");

            // 级别
            IList list = ouLevelService.FindAll();
            if (list == null || list.Count == 0)
            {
                Exception e = new Exception("导入角色信息前必须先导入级别信息！");
                logger.Error(e.Message, e);
                throw e;
            }
            foreach (OULevel sys in list)
            {
                allOULevel.Add(sys.Code, sys);
            }
            privilegeList = privilegeService.FindAll();
        }
        #endregion

        public override void Execute(string pDataFilePath)
        {
            logger.Debug("开始导入角色数据");
            IList<string> list = TextHelper.ReadTextByReadLine(pDataFilePath);
            foreach (string str in list)
            {
                string[] strArray = str.Split(';');
                //“角色”数据格式：[0]名称;[1]角色代码;[2]级别的编码;[3]是否是内建角色(Y/N);[4]权限列表(使用逗号分开，如果用户全部权限则设置为SUPPER);[5]描述;END
                Role role = new Role();
                role.ID = -1;
                role.Name = strArray[0];
                role.Code = strArray[1];
                role.Level = strArray[2];
                role.LevelName = allOULevel[strArray[2]].Name;
                role.IsInner = strArray[3];
                role.Memo = strArray[5];

                // 设置权限
                if (strArray[4].Trim().Length == 0)
                {
                    logger.Error("角色“" + role.Name + "[" + role.Code
                            + "]”没有配置任何权限！");
                }
                else
                {
                    String[] privilegeRegExCodes = strArray[4].Split(',');
                    for (int i = 0; i < privilegeRegExCodes.Length; i++)
                    {
                        privilegeRegExCodes[i] = privilegeRegExCodes[i].Replace(WILDCARD, REGEX);// 匹配[a-z,A-Z,_,0-9]
                    }
                    IList privilegeSet = new ArrayList();
                    bool onError = false;
                    for (int i = 0; i < privilegeRegExCodes.Length; i++)
                    {
                        logger.Debug("----privilegeRegExCode="
                                + privilegeRegExCodes[i]);
                        onError = true;
                        foreach (Privilege privilege in privilegeList)
                        {
                            if (Regex.IsMatch(privilege.Code, privilegeRegExCodes[i]))
                            {
                                privilegeSet.Add(privilege);
                                onError = false;
                            }
                        }
                        if (onError)
                        {
                            logger.Error("角色“" + role.Name + "[" + role.Code + "]”所要拥有的权限“"
                                    + privilegeRegExCodes[i].Replace(REGEX, WILDCARD) + "”在系统中找不到对应的匹配权限。该角色不会拥有该权限!");
                        }
                    }
                    role.Privileges = privilegeSet;
                    logger.Info("角色“" + role.Name + "[" + role.Code + "]”共拥有的权限个数为：" + privilegeSet.Count);
                }
                roleService.Save(role);
            }
        }
    }
}
