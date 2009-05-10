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
using TSLibWeb.WEB;
using log4net;
using TSCommon.Core.OptionItemCfg.Service;
using TSCommon.Core.TSWebContext;
using TSCommon.Core;
using TSLib;

namespace TSCommon.Web.OptionItemCfg
{
    public partial class OptionItemView : StrutsCorePage
    {
        private static ILog logger = LogManager.GetLogger(typeof(OptionItemView));
        private IOptionItemService optionItemService;

        /// <summary>
        /// 构造函数，初始化相关Service
        /// </summary>
        public OptionItemView()
        {
            this.optionItemService = (IOptionItemService)GetObject("OptionItemService");
        }

        /// <summary>
        /// 判断当前用户是否为管理员
        /// </summary>
        public bool IsManager
        {
            get
            {
                return TSWEBContext.Current.IsHasPrivilege(SimpleResourceHelper.GetString("PRIVILEDGE.CODE.ADMIN_ALL"));
            }
        }

        /// <summary>
        /// 所有类型的下拉框列表
        /// </summary>
        public string AllSelectTypeJsonString
        {
            get
            {
                string allSelectTypeJsonString = getAllSelectTypeJsonString();
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("allSelectTypeJsonString=" + allSelectTypeJsonString);
                }
                return allSelectTypeJsonString;
            }
        }

        private string getAllSelectTypeJsonString()
        {
            Newtonsoft.Json.JavaScriptArray jsonArray = new Newtonsoft.Json.JavaScriptArray();
            Newtonsoft.Json.JavaScriptObject json;
            TSCommon.Core.HtmlOption[] options = this.optionItemService.GetTypeOptions();
            if (options != null)
            {
                foreach (TSCommon.Core.HtmlOption option in options)
                {
                    json = new Newtonsoft.Json.JavaScriptObject();
                    json.Add("Name", option.OptionName);
                    json.Add("Value", option.OptionValue);
                    jsonArray.Add(json);
                }
            }
            return Newtonsoft.Json.JavaScriptConvert.SerializeObject(jsonArray);
        }
    }
}
