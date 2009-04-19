using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Utils;
using Lib;

namespace TSCommon_Core.Security.Domain
{
    /// <summary>
    /// ģ������
    /// </summary>
    public enum ModelTypes { Undefined = -1, Master = 0, SubModel = 1 };

    /// <summary>
    /// ����ģ�����͵�Strutsת����
    /// </summary>
    public class ModelTypeConverter : TSLibStruts.Utils.IConverter
    {
        public object Convert(Type type, object value)
        {
            if (value != null && !value.Equals(""))
                return Enum.Parse(typeof(ModelTypes), value.ToString());
            else
                return ModelTypes.Undefined;
        }
    }

    /// <summary>
    /// ģ��Domain����
    /// </summary>
    public class Model : Entry
    {
        #region �ֶζ���

        private string name;                // ģ������
        private string code;                // ģ�����
        private string orderNo;             // ģ���˳���               
        private int type;                   // ģ�������
        private string isInner;             // �Ƿ����ڽ�ģ�飬 Y���ڽ�ģ��     N�����ڽ�ģ�� 
        private Model parent;               // ����ģ��

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
        public ModelTypes Type
        {
            get { return (ModelTypes)type; }
            set { type = (int)((ModelTypes)value); }
        }
        public string IsInner
        {
            get { return isInner; }
            set { isInner = value; }
        }
        public Model Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        public long ParentID
        {
            get 
            {
                if (null == this.parent)
                    return Constants.BLANK_LONG_VALUE;
                else
                    return this.parent.ID;
            }
            set 
            {
                if (value == Constants.BLANK_LONG_VALUE)
                    this.parent = null;
                else
                {
                    this.parent = new Model();
                    this.parent.ID = value;
                }
            }
        }
        #endregion

        /// <summary>
        /// Ĭ�Ϲ��캯��
        /// </summary>        
        public Model()
        {            
        }
        
        /// <summary>
        /// ����ָ����������һ��ģ��
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modelName"></param>
        public Model(long id, string modelName)
        {            
            this.ID = id;
            this.name = modelName;
        }

        /// <summary>
        /// ��ȡģ�����͵�����
        /// </summary>
        /// <returns></returns>
        public string GetTypeName()
        {
            if (this.Type == ModelTypes.Master)
                return "��ģ��";
            else
                return "��ģ��";
        }

        /// <summary>
        /// ��ȡģ������ģ�������
        /// </summary>
        /// <returns></returns>
        public string GetParentName()
        {
            if (null == this.Parent)
                return "";
            else
                return this.Parent.Name;
        }
    }
}
