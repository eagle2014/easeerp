/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 * 
 * Autor: CD826         2007-02-26
 */
using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using TSCommon_Core.Security.Domain;
using TSCommon_Core.Security.Dao;
using System.Collections;
using TS.Exceptions;
using TSLib.Utils;

namespace TSCommon_Core.Security.Service
{
    /// <summary>
    /// 模块Service的实现
    /// </summary>
    /// <author>Tony</author>
    /// <date>2008-06-01</date>
    public class ModelService : BaseService<Model>, IModelService
    {
        private IModelDao modelDao;         // 模块Dao
        public IModelDao ModelDao
        {
            set
            {
                modelDao = value;
                base.BaseDao = value;
            }
        }

        #region IModelService 成员

        public IList FindAllWithExclude(long id)
        {
            return this.modelDao.FindAllWithExclude(id);
        }

        public IList FindAllMaster()
        {
            return this.modelDao.FindAllMaster();
        }

        public IList FindChildren(long modelID)
        {
            return this.modelDao.FindChildren(modelID);
        }

        public IList FindChildren(string modelUnid)
        {
            return this.modelDao.FindChildren(modelUnid);
        }

        public override void Save(Model model)
        {
            if (!this.modelDao.IsUnique(model))
                throw new ResourceException("MODEL.EXCEPTION.HAD_EXIST", new string[] { model.Name, model.Code });
            this.modelDao.Save(model);
        }

        public override void Save(IList objs)
        {
            if (objs == null) return;

            foreach (Model model in objs)
            {
                this.Save(model);
            }
        }

        public Model GetByCode(string code)
        {
            return modelDao.GetByCode(code);
        }
        #endregion

        #region 删除方法复写

        public override void Delete(Model model)
        {
            if (null != model)
            {
                if (model.IsInner.Equals(Constants.YESNO_YES, StringComparison.OrdinalIgnoreCase))
                    throw new ResourceException("MODEL.EXCEPTION.IS_INNER", new string[] { model.Name, model.Code });
                this.modelDao.Delete(model);
            }
        }

        public override void Delete(long id)
        {
            Model model = this.modelDao.Load(id);
            this.Delete(model);
        }

        public override void Delete(string unid)
        {
            Model model = this.modelDao.Load(unid);
            this.Delete(model);
        }

        public override void Delete(long[] ids)
        {
            foreach (long id in ids)
            {
                this.Delete(id);
            }
        }

        public override void Delete(string[] unids)
        {
            foreach (string unid in unids)
            {
                this.Delete(unid);
            }
        }

        public override void Delete(IList objs)
        {
            foreach (Model obj in objs)
            {
                this.Delete(obj);
            }
        }

        #endregion
    }
}
