/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Common.Logging;
using TS.Exceptions;
using TSCommon.Core.Organize.Domain;
using TSLib.Service;

namespace TSCommon.Core.Organize.RelationShips
{
    /// <summary>
    /// 关联关系Service的实现
    /// <author>CD826</author>
    /// <author>Tony</author>
    /// </summary>
    public class RelationShipService : BaseService<RelationShip>, IRelationShipService
    {
        private static ILog logger = LogManager.GetLogger(typeof(RelationShipService));
        private IRelationShipDao relationShipDao;           // 关联关系的DAO
        private IList relationShipItems;              //关联项目

        public IList RelationShipItems
        {
            get { return relationShipItems; }
            set { relationShipItems = value; }
        }

        
        public IRelationShipDao RelationShipDao
        {
            set
            { 
                this.relationShipDao = value;
                base.BaseDao = value;
            }
        }

        #region IRelationShipService 成员

        public void UpdateRelationByParent(string parentUnid, string relationShipType, IList relationShips)
        {
            this.relationShipDao.DeleteAllByParent(parentUnid, relationShipType);
            if(null != relationShips && relationShips.Count > 0)
                this.relationShipDao.Save(relationShips);
        }

        public void UpdateRelationByChild(string childUnid, string relationShipType, IList relationShips)
        {
            this.relationShipDao.DeleteAllByChild(childUnid, relationShipType);
            if (null != relationShips && relationShips.Count > 0)
                this.relationShipDao.Save(relationShips);
        }

        public void UpdateRelation(string parentUnid, IList relationShips, string type)
        {
            this.relationShipDao.DeleteAllByParent(parentUnid, type);
            if (null != relationShips && relationShips.Count > 0)
                this.relationShipDao.Save(relationShips);
        }

        public RelationShip Get(string parentUnid, string childUnid)
        {
            return this.relationShipDao.Get(parentUnid, childUnid);
        }

        public IList GetByChild(string childUnid, string type)
        {
            return this.relationShipDao.GetByChild(childUnid, type);
        }

        public IList GetByParent(string parentUnid, string type)
        {
            return this.relationShipDao.GetByParent(parentUnid, type);
        }

        public void Delete(string parentUnid, string childUnid)
        {
            this.relationShipDao.Delete(parentUnid, childUnid);
        }

        public int Delete(string parentUnid, string[] childUnids)
        {
            return this.relationShipDao.Delete(parentUnid, childUnids);
        }

        public void DeleteAllByParent(string parentUnid)
        {
            this.relationShipDao.DeleteAllByParent(parentUnid);
        }

        public void DeleteAllByParent(string parentUnid, string type)
        {
            this.relationShipDao.DeleteAllByParent(parentUnid, type);
        }

        public void DeleteAllByChild(string childUnid)
        {
            this.relationShipDao.DeleteAllByChild(childUnid);
        }

        public void DeleteAllByChild(string childUnid, string type)
        {
            this.relationShipDao.DeleteAllByChild(childUnid, type);
        }

        public void DeleteAll(string unid)
        {
            this.relationShipDao.DeleteAll(unid);
        }

        public void DeleteAll(string unid, string type)
        {
            this.relationShipDao.DeleteAll(unid, type);
        }

        public int Build(string parentUnid, string parentType, string[] childUnids, string childType)
        {
            if (childUnids == null || childUnids.Length == 0) return 0;

            // 不允许parentUnid, parentType, childType任一参数为空
            if (string.IsNullOrEmpty(parentUnid)
                || string.IsNullOrEmpty(parentType)
                || string.IsNullOrEmpty(childType))
            {
                ResourceException e = new ResourceException("ARGUMENTS.ERROR.EMPTY", "[parentUnid, parentType, childType]");
                logger.Error(e.Message, e);
                throw e;
            }

            RelationShip relationShip;
            int count = 0;
            foreach (string childUnid in childUnids)
            {
                relationShip = this.relationShipDao.Get(parentUnid, childUnid);
                if (relationShip != null)
                {
                    // 关联关系已存在，仅作更新
                    relationShip.Update(parentUnid, parentType, childUnid, childType);
                }
                else
                {
                    // 创建新的关联关系
                    relationShip = new RelationShip(parentUnid, parentType, childUnid, childType);
                }
                this.relationShipDao.Save(relationShip);
                count++;
            }
            return count;
        }


        public IList GetRelationItems()
        {
            return this.relationShipItems;
        }

        #endregion
    }
}
