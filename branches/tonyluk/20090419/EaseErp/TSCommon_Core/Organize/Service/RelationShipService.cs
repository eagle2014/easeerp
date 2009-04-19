/*
 * Copyright (c) 2005, 2007 ��������Ƽ����ϵͳ���޹�˾
 * ��Ȩ����
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
    /// ������ϵService��ʵ��
    /// <author>CD826</author>
    /// <author>Tony</author>
    /// </summary>
    public class RelationShipService : BaseService<RelationShip>, IRelationShipService
    {
        private static ILog logger = LogManager.GetLogger(typeof(RelationShipService));
        private IRelationShipDao relationShipDao;           // ������ϵ��DAO
        private IList relationShipItems;              //������Ŀ

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

        #region IRelationShipService ��Ա

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

            // ������parentUnid, parentType, childType��һ����Ϊ��
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
                    // ������ϵ�Ѵ��ڣ���������
                    relationShip.Update(parentUnid, parentType, childUnid, childType);
                }
                else
                {
                    // �����µĹ�����ϵ
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
