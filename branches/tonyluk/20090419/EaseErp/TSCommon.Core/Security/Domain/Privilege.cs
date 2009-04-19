using System;
using System.Collections.Generic;
using System.Text;
using Lib;
using TSLib.Utils;

namespace TSCommon_Core.Security.Domain
{
    /// <summary>
    /// Ȩ�޶���
    /// </summary>
    public class Privilege : Entry
    {
        /// <summary>������ϵ�Ķ��壬ֵΪ��privilege��</summary>
        public static string RELATIONSHIP_CODE = "privilege";

        /// <summary>���ܰ�ťȨ�ޣ�ֵΪ��btn��</summary>
        public static string PRIVILEGETYPE_BUTTON   = "btn";

        /// <summary>����ģ��Ȩ�ޣ�ֵΪ��url��</summary>
        public static string PRIVILEGETYPE_MODEL = "url";

        #region �ֶζ���
        
        private string name;                                        // Ȩ������
        private string code;                                        // Ȩ�޴���
        private string orderNo;                                     // Ȩ�����
        private string type;                                        // Ȩ�����
        private string urlPath;                                     // url·��
        private Model model;                                        // ����ģ��
        private string isInner;                                     // �Ƿ����ڽ�Ȩ�ޣ� Y���ڽ�Ȩ��     N�����ڽ�Ȩ��

        #endregion

        #region ���Զ���
        
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        public string OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string TypeName
        {
            get 
            {
                if (PRIVILEGETYPE_MODEL.Equals(this.type, StringComparison.OrdinalIgnoreCase))
                    return "ģ��Ȩ��";
                else
                    return "��ťȨ��";
            }
        }
        public string UrlPath
        {
            get { return urlPath; }
            set { urlPath = value; }
        }
        public Model Model
        {
            get { return model; }
            set { model = value; }
        }
        public long ModelID
        {
            get
            {
                if (null == this.model)
                    return Constants.BLANK_LONG_VALUE;
                else
                    return this.model.ID;
            }
            set
            {
                if (value == Constants.BLANK_LONG_VALUE)
                    this.model = null;
                else
                {
                    this.model = new Model();
                    this.model.ID = value;
                }
            }
        }
        public string ModelName
        {
            get
            {
                if (null == this.model)
                    return Constants.BLANK_STRING_VALUE;
                else
                    return this.model.Name;
            }
        }
        public string IsInner
        {
            get { return isInner; }
            set { isInner = value; }
        }

        #endregion
    }
}
