using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TSLib.Dao.Hibernate;
using Common.Logging;
using TSCommon_Core.Security.Domain;

namespace TSCommon_Core.Security.Dao.Hibernate
{
    /// <summary>
    /// 模块Dao接口Hibernate的实现
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-06-01</date>
    public class ModelDao : BaseDao<Model>, IModelDao
    {
        private static ILog logger = LogManager.GetLogger(typeof(ModelDao));

        protected override string DefaultQueryString
        {
            get { return "from Model _alias order by _alias.OrderNo "; }
        }

        #region IModelDao 成员
                
        public IList FindAllWithExclude(long id)
        {
            string hql = "from Model model where model.ID != ? order by model.OrderNo";
            return base.HibernateTemplate.Find(hql, id);
        }
        
        public bool IsUnique(Model model)
        {
            string hql = "from Model model where model.ID != ? and model.Code = ?";
            return this.IsUnique(hql, new object[] { model.ID, model.Code });
        }

        public IList FindAllMaster()
        {
            string hql = "from Model model where model.Type = ? order by model.OrderNo";
            return base.HibernateTemplate.Find(hql, (int)ModelTypes.Master);
        }

        public IList FindChildren(long modelID)
        {
            if (modelID <= 0)
            {
                return this.FindAllMaster();
            }
            else
            {
                string hql = "from Model model where model.Parent is not null and model.Parent.ID = ? order by model.OrderNo";
                return base.HibernateTemplate.Find(hql, modelID);
            }
        }

        public IList FindChildren(string modelUnid)
        {
            if (string.IsNullOrEmpty(modelUnid))
            {
                return this.FindAllMaster();
            }
            else
            {
                string hql = "from Model model where model.Parent is not null and model.Parent.Unid = ? order by model.OrderNo";
                return base.HibernateTemplate.Find(hql, modelUnid);
            }
        }

        public Model GetByCode(string code)
        {
            string hql = "from Model model where model.Code=?";
            return this.FindUnique(hql, new object[] { code});
        }
        #endregion
    }
}
