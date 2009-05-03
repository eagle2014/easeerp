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

         #region ���Service
        private IOULevelService ouLevelService;
        private IPrivilegeService privilegeService;
        private IRoleService roleService;
        public importRole()
        {
            ouLevelService = (IOULevelService)GetObject("OULevelService");
            privilegeService = (IPrivilegeService)GetObject("PrivilegeService");
            roleService = (IRoleService)GetObject("RoleService");

            // ����
            IList list = ouLevelService.FindAll();
            if (list == null || list.Count == 0)
            {
                Exception e = new Exception("�����ɫ��Ϣǰ�����ȵ��뼶����Ϣ��");
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
            logger.Debug("��ʼ�����ɫ����");
            IList<string> list = TextHelper.ReadTextByReadLine(pDataFilePath);
            foreach (string str in list)
            {
                string[] strArray = str.Split(';');
                //����ɫ�����ݸ�ʽ��[0]����;[1]��ɫ����;[2]����ı���;[3]�Ƿ����ڽ���ɫ(Y/N);[4]Ȩ���б�(ʹ�ö��ŷֿ�������û�ȫ��Ȩ��������ΪSUPPER);[5]����;END
                Role role = new Role();
                role.ID = -1;
                role.Name = strArray[0];
                role.Code = strArray[1];
                role.Level = strArray[2];
                role.LevelName = allOULevel[strArray[2]].Name;
                role.IsInner = strArray[3];
                role.Memo = strArray[5];

                // ����Ȩ��
                if (strArray[4].Trim().Length == 0)
                {
                    logger.Error("��ɫ��" + role.Name + "[" + role.Code
                            + "]��û�������κ�Ȩ�ޣ�");
                }
                else
                {
                    String[] privilegeRegExCodes = strArray[4].Split(',');
                    for (int i = 0; i < privilegeRegExCodes.Length; i++)
                    {
                        privilegeRegExCodes[i] = privilegeRegExCodes[i].Replace(WILDCARD, REGEX);// ƥ��[a-z,A-Z,_,0-9]
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
                            logger.Error("��ɫ��" + role.Name + "[" + role.Code + "]����Ҫӵ�е�Ȩ�ޡ�"
                                    + privilegeRegExCodes[i].Replace(REGEX, WILDCARD) + "����ϵͳ���Ҳ�����Ӧ��ƥ��Ȩ�ޡ��ý�ɫ����ӵ�и�Ȩ��!");
                        }
                    }
                    role.Privileges = privilegeSet;
                    logger.Info("��ɫ��" + role.Name + "[" + role.Code + "]����ӵ�е�Ȩ�޸���Ϊ��" + privilegeSet.Count);
                }
                roleService.Save(role);
            }
        }
    }
}
