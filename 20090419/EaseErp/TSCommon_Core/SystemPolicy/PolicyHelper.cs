using System;
using System.Collections.Generic;
using System.Text;
using Common.Logging;
using TSCommon.Core.SystemPolicy.Service;
using TSCommon.Core.SystemPolicy.Domain;

namespace TSCommon.Core.SystemPolicy
{
    /// <summary>
    /// ϵͳ������ĸ�����
    /// </summary>
    public class PolicyHelper
    {
        private static ILog logger = LogManager.GetLogger(typeof(PolicyHelper));
        private static IPolicyService policyService;

        static PolicyHelper()
        {
            policyService = TSLibWeb.Struts.StrutsCoreAction.GetObject("PolicyService") as IPolicyService;
        }

        /// <summary>
        /// �õ�ָ�����Ե�ֵ
        /// </summary>
        /// <param name="policyCode">ϵͳ���Եı���</param>
        /// <returns>���û���򷵻�һ�����ַ���</returns>
        public static string GetPolicyValue(string policyCode)
        {
            if (null == policyCode || policyCode.Length == 0)
                return "";

            Policy policy = policyService.LoadByCode(policyCode);
            return policy == null ? "" : policy.Value;
        }
    }
}
