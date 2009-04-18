using System;
using System.Collections.Generic;
using System.Text;
using TSCommon_Core.OptionItemCfg.Domain;
using TSLib.Service;
using TSCommon_Core.OptionItemCfg.Dao;
using Common.Logging;
using System.Collections;
using TSLib;

namespace TSCommon_Core.OptionItemCfg.Service
{
    public class OptionItemService:BaseService<OptionItem>,IOptionItemService
    {
        private static ILog logger = LogManager.GetLogger(typeof(OptionItemService));
        private IOptionItemDao optionItemDao;                   // 业务对象配置的Dao

        /// <summary>
        /// 业务对象配置的Dao
        /// </summary>
        public IOptionItemDao OptionItemDao
        {
            set
            {
                optionItemDao = value;
                BaseDao = value;
            }
        }

        #region IOptionItemService 成员

        public IList FindAll(string type)
        {
            return this.optionItemDao.FindAll(type);
        }

        public PageInfo GetPage(int firstNo, int maxResult, string sortField, string sortDir)
        {
            return this.optionItemDao.GetPage(firstNo, maxResult, sortField, sortDir);
        }

        public PageInfo GetPage(int firstNo, int maxResult, string sortField, string sortDir, string type)
        {
            return this.optionItemDao.GetPage(firstNo, maxResult, sortField, sortDir, type);
        }

        public void DeleteAll(string type)
        {
            IList itemList = this.optionItemDao.FindAll(type);
            if (null == itemList || itemList.Count == 0)
                return;
            this.optionItemDao.Delete(itemList);
        }

        public HtmlOption[] GetOptions(string type)
        {
            IList itemList = this.optionItemDao.FindAll(type);
            if (null == itemList || itemList.Count == 0)
                return HtmlOption.GetBlankHtmlOption();

            HtmlOption[] options = new HtmlOption[itemList.Count];
            for (int i = 0; i < itemList.Count; i++)
            {
                OptionItem optionItem = (OptionItem)itemList[i];
                options[i] = new HtmlOption(optionItem.Name, optionItem.Code);
            }
            return options;
        }

        public HtmlOption[] GetOptions(string type, string curValue)
        {
            if (string.IsNullOrEmpty(curValue)) return GetOptions(type);

            IList itemList = this.optionItemDao.FindAll(type);
            if (null == itemList || itemList.Count == 0)
                return HtmlOption.GetBlankHtmlOption();

            IList optionsList = new ArrayList();
            bool isHave = false;
            for (int i = 0; i < itemList.Count; i++)
            {
                OptionItem optionItem = (OptionItem)itemList[i];
                if (!isHave && curValue.Equals(optionItem.Code, StringComparison.OrdinalIgnoreCase))
                    isHave = true;
                optionsList.Add(new HtmlOption(optionItem.Name, optionItem.Code));
            }
            if (!isHave)
            {
                optionsList.Add(new HtmlOption(curValue, curValue));
            }

            HtmlOption[] options = new HtmlOption[optionsList.Count];
            optionsList.CopyTo(options, 0);
            return options;
        }

        public string GetTypeName(string type)
        {
            IList typesList = this.optionItemDao.FindAllType();
            if (null == typesList || typesList.Count == 0)
                return "";
            foreach (object[] typeInfos in typesList)
            {
                if (type.Equals(typeInfos[0].ToString(), StringComparison.OrdinalIgnoreCase))
                    return typeInfos[1].ToString();
            }
            return "";
        }

        public IList FindAllType()
        {
            IList typesList = this.optionItemDao.FindAllType();
            if (null == typesList || typesList.Count == 0)
                return new ArrayList();

            IList types = new ArrayList();
            foreach (object[] typeInfos in typesList)
            {
                types.Add(typeInfos[0].ToString());
            }
            return types;
        }

        public HtmlOption[] GetTypeOptions()
        {
            IList typesList = this.optionItemDao.FindAllType();
            if (null == typesList || typesList.Count == 0)
                return HtmlOption.GetBlankHtmlOption();

            HtmlOption[] options = new HtmlOption[typesList.Count];
            int index = 0;
            foreach (object[] typeInfos in typesList)
            {
                options[index++] = new HtmlOption(typeInfos[1].ToString(), typeInfos[0].ToString());
            }
            return options;
        }

        #endregion
    }
}
