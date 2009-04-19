using System;
using System.Collections.Generic;
using System.Text;
using Common.Logging;
using TSCommon.Core.SystemPolicy.Service;
using TSCommon.Core.SystemPolicy.Domain;

namespace TSCommon.Core.SystemPolicy
{
    /// <summary>
    /// 系统策略类的辅助类
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
        /// 得到指定策略的值
        /// </summary>
        /// <param name="policyCode">系统策略的编码</param>
        /// <returns>如果没有则返回一个空字符串</returns>
        public static string GetPolicyValue(string policyCode)
        {
            if (null == policyCode || policyCode.Length == 0)
                return "";

            Policy policy = policyService.LoadByCode(policyCode);
            return policy == null ? "" : policy.Value;
        }
    }
}
