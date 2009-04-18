/*
 * Copyright (c) 2005, 2007 广州市忆科计算机系统有限公司
 * 版权所有
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using TSCommon_Core.Organize.Domain;
using Common.Logging;
using TSCommon_Core.Organize.Dao;
using System.Collections;
using TSLib;
using TSLib.Utils;
using TS.Exceptions;

namespace TSCommon_Core.Organize.Service
{
    /// <summary>
    /// 组织单位信息Service的实现
    /// <author>CD826</author>
    /// </summary>
    public class OUInfoService : BaseService<OUInfo>, IOUInfoService
    {
        private static ILog logger = LogManager.GetLogger(typeof(OUInfoService));
        private IOUInfoDao ouInfoDao;
        public IOUInfoDao OUInfoDao
        {
            set
            {
                ouInfoDao = value;
                base.BaseDao = value;
            }
        }

        #region IOUInfoService 成员

        public PageInfo GetPageByType(User userInfo, string type, int pageNo, int pageSize, string sortField, string sortDir, object[] ouStatuses)
        {
            return this.ouInfoDao.GetPageByType(type, pageNo, pageSize, sortField, sortDir, ouStatuses);
        }

        public PageInfo GetDepartmentPage(string unitUnid, int pageNo, int pageSize,
            string sortField, string sortDir, object[] ouStatuses)
        {
            return this.ouInfoDao.GetDepartmentPage(unitUnid, pageNo, pageSize, sortField, sortDir, ouStatuses);
        }

        public IList GetUnitTree(User userInfo, int privilegeType)
        {
            IList unitList = new ArrayList();
            if (privilegeType == Constants.OT_ALL)
            {
                unitList = this.FindAllByType(null, OUInfo.OT_UNIT);
            }
            else if (privilegeType == Constants.OT_LOCALANDCHILD)
            {
                OUInfo ouInfo = this.ouInfoDao.Load(userInfo.UnitUnid);
                if (null != ouInfo)
                {
                    unitList = this.ouInfoDao.FindChildByParentCode(ouInfo.FullCode, OUInfo.OT_UNIT);
                    unitList.Add(ouInfo);
                }
            }
            else
            {
                OUInfo ouInfo = this.ouInfoDao.Load(userInfo.UnitUnid);
                if (null != ouInfo)
                    unitList.Add(ouInfo);
            }
            if (null == unitList || unitList.Count == 0)
                return new ArrayList();
                        
            IList unitTree = new ArrayList();
            if (null != unitList && unitList.Count > 0)
            {
                foreach (OUInfo ouInfo in unitList)
                {
                    string[] unitInfo = new string[4];
                    unitInfo[0] = ouInfo.ID.ToString();
                    if (string.IsNullOrEmpty(ouInfo.ParentOUUnid))
                        unitInfo[1] = "-1";
                    else
                    {
                        foreach (OUInfo unit in unitList)
                        {
                            if (ouInfo.ParentOUUnid.Equals(unit.Unid, StringComparison.OrdinalIgnoreCase))
                                unitInfo[1] = unit.ID.ToString();
                        }
                        if (string.IsNullOrEmpty(unitInfo[1]))
                            unitInfo[1] = "-1";
                    }
                    unitInfo[2] = ouInfo.Name;
                    unitInfo[3] = ouInfo.Unid;
                    unitTree.Add(unitInfo);
                }
            }
            return unitTree;
        }

        public IList GetOUTree(User userInfo, int privilegeType)
        {
            logger.Debug("privilegeType=" + privilegeType.ToString());
            IList ouInfoList = new ArrayList();
            if (privilegeType == Constants.OT_ALL)
            {
                ouInfoList = this.ouInfoDao.FindAll();
            }
            else if (privilegeType == Constants.OT_LOCALANDCHILD)
            {
                OUInfo ouInfo = this.ouInfoDao.Load(userInfo.UnitUnid);
                if (null != ouInfo)
                {
                    IList unitList = this.ouInfoDao.FindChildByParentCode(ouInfo.FullCode, OUInfo.OT_UNIT);
                    unitList.Add(ouInfo);
                    IList departmentList = this.ouInfoDao.FindAllByType(OUInfo.OT_DEPARTMENT);
                    ((ArrayList)ouInfoList).AddRange(unitList);
                    foreach (OUInfo department in departmentList)
                    {
                        foreach (OUInfo unit in unitList)
                        {
                            if (department.UnitUnid.Equals(unit.Unid, StringComparison.OrdinalIgnoreCase))
                            {
                                ouInfoList.Add(department);
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                OUInfo ouInfo = this.ouInfoDao.Load(userInfo.UnitUnid);
                if (null != ouInfo)
                {
                    ouInfoList.Add(ouInfo);
                    IList departmentList = this.ouInfoDao.FindAllDepartment(ouInfo.Unid);
                    ((ArrayList)ouInfoList).AddRange(departmentList);
                }
            }
            if (null == ouInfoList || ouInfoList.Count == 0)
                return new ArrayList();
                        
            IList ouTree = new ArrayList();
            if (null != ouInfoList && ouInfoList.Count > 0)
            {
                foreach (OUInfo ouInfo in ouInfoList)
                {
                    string[] ouInfos = new string[4];
                    ouInfos[0] = ouInfo.ID.ToString();
                    if (ouInfo.Type.Equals(OUInfo.OT_UNIT))
                    {
                        if (string.IsNullOrEmpty(ouInfo.ParentOUUnid))
                            ouInfos[1] = "-1";
                        else
                        {
                            foreach (OUInfo unit in ouInfoList)
                            {
                                if (ouInfo.ParentOUUnid.Equals(unit.Unid, StringComparison.OrdinalIgnoreCase))
                                    ouInfos[1] = unit.ID.ToString();
                            }
                            if (string.IsNullOrEmpty(ouInfos[1]))
                                ouInfos[1] = "-1";
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(ouInfo.ParentOUUnid))
                        {
                            foreach (OUInfo ouTmp in ouInfoList)
                            {
                                if (ouInfo.UnitUnid.Equals(ouTmp.Unid, StringComparison.OrdinalIgnoreCase))
                                    ouInfos[1] = ouTmp.ID.ToString();
                            }
                        }
                        else
                        {
                            foreach (OUInfo deparetment in ouInfoList)
                            {
                                if (ouInfo.ParentOUUnid.Equals(deparetment.Unid, StringComparison.OrdinalIgnoreCase))
                                    ouInfos[1] = deparetment.ID.ToString();
                            }
                        }
                    }
                    ouInfos[2] = ouInfo.Name;
                    ouInfos[3] = ouInfo.Unid;
                    ouTree.Add(ouInfos);
                }
            }
            return ouTree;
        }

        public IList GetOUTree(User userInfo, string unitUnid)
        {
            IList ouInfoList = new ArrayList();
            OUInfo unit = this.ouInfoDao.Load(unitUnid);
            if (null != unit)
            {
                IList unitList = this.ouInfoDao.FindChildByParentCode(unit.FullCode, OUInfo.OT_UNIT);
                unitList.Add(unit);
                IList departmentList = this.ouInfoDao.FindAllByType(OUInfo.OT_DEPARTMENT);
                ((ArrayList)ouInfoList).AddRange(unitList);
                foreach (OUInfo department in departmentList)
                {
                    foreach (OUInfo unitInfo in unitList)
                    {
                        if (department.UnitUnid.Equals(unitInfo.Unid, StringComparison.OrdinalIgnoreCase))
                        {
                            ouInfoList.Add(department);
                            break;
                        }
                    }
                }
            }
            if (null == ouInfoList || ouInfoList.Count == 0)
                return new ArrayList();

            IList ouTree = new ArrayList();
            if (null != ouInfoList && ouInfoList.Count > 0)
            {
                foreach (OUInfo ouInfo in ouInfoList)
                {
                    string[] ouInfos = new string[4];
                    ouInfos[0] = ouInfo.ID.ToString();
                    if (ouInfo.Type.Equals(OUInfo.OT_UNIT))
                    {
                        if (string.IsNullOrEmpty(ouInfo.ParentOUUnid))
                            ouInfos[1] = "-1";
                        else
                        {
                            foreach (OUInfo unitInfo in ouInfoList)
                            {
                                if (ouInfo.ParentOUUnid.Equals(unitInfo.Unid, StringComparison.OrdinalIgnoreCase))
                                    ouInfos[1] = unitInfo.ID.ToString();
                            }
                            if (string.IsNullOrEmpty(ouInfos[1]))
                                ouInfos[1] = "-1";
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(ouInfo.ParentOUUnid))
                        {
                            foreach (OUInfo ouTmp in ouInfoList)
                            {
                                if (ouInfo.UnitUnid.Equals(ouTmp.Unid, StringComparison.OrdinalIgnoreCase))
                                    ouInfos[1] = ouTmp.ID.ToString();
                            }
                        }
                        else
                        {
                            foreach (OUInfo deparetment in ouInfoList)
                            {
                                if (ouInfo.ParentOUUnid.Equals(deparetment.Unid, StringComparison.OrdinalIgnoreCase))
                                    ouInfos[1] = deparetment.ID.ToString();
                            }
                        }
                    }
                    ouInfos[2] = ouInfo.Name;
                    ouInfos[3] = ouInfo.Unid;
                    ouTree.Add(ouInfos);
                }
            }
            return ouTree;

        }

        public IList FindUnitByParentUnid(string parentUnid, bool includeSelf)
        {
            OUInfo ouInfo = this.ouInfoDao.Load(parentUnid);
            if (null == ouInfo)
                return new ArrayList();

            IList unitList = this.ouInfoDao.FindChildByParentCode(ouInfo.FullCode, OUInfo.OT_UNIT);
            if(includeSelf)
                unitList.Add(ouInfo);
            return unitList;
        }

        public IList FindAllByType(User userInfo, string type)
        {
            // TODO: 判断用户的权限
            return this.ouInfoDao.FindAllByType(type);
        }

        public IList FindAllDepartment(string unitUnid)
        {
            return this.ouInfoDao.FindAllDepartment(unitUnid);
        }

        public IList FindOUInfoByParentUnid(string parentUnid, bool hasDepartment, bool hasChildDepartment)
        {
            OUInfo ouInfo = this.ouInfoDao.Load(parentUnid);
            if (null == ouInfo)
                return new ArrayList();

            ArrayList ouList = new ArrayList();
            ouList.Add(ouInfo);
            if (ouInfo.Type.Equals(OUInfo.OT_UNIT, StringComparison.OrdinalIgnoreCase))
            {
                if (hasDepartment)
                {
                    IList list = this.ouInfoDao.FindAllDepartment(ouInfo.Unid);
                    if (null != list && list.Count > 0)
                        ouList.AddRange(list);
                }
                return ouList;
            }
            else
            {
                if (hasChildDepartment)
                {
                    IList list = this.ouInfoDao.FindChildByParentCode(ouInfo.FullCode, OUInfo.OT_DEPARTMENT);
                    if (null != list && list.Count > 0)
                        ouList.AddRange(list);
                }
                return ouList;
            }
        }

        public OUInfo LoadByFullCode(string fullCode)
        {
            return this.ouInfoDao.LoadByFullCode(fullCode);
        }

        public override void Save(OUInfo ouInfo)
        {
            // 产生相应的全名称和全编码
            if (!string.IsNullOrEmpty(ouInfo.UnitUnid))
            {
                OUInfo unitInfo = this.ouInfoDao.Load(ouInfo.UnitUnid);
                if (null != unitInfo)
                {
                    ouInfo.UnitFullCode = unitInfo.FullCode;
                    ouInfo.UnitFullName = unitInfo.FullName;
                    ouInfo.UnitName = unitInfo.Name;
                }
            }

            if (!string.IsNullOrEmpty(ouInfo.ParentOUUnid))
            {
                OUInfo parentOU = this.ouInfoDao.Load(ouInfo.ParentOUUnid);
                if (null == parentOU)
                    throw new ResourceException("UNIT.EXCEPTION.UPPER_NOT_EXIST");
                ouInfo.FullName = parentOU.FullName + "." + ouInfo.Name;
                ouInfo.FullCode = parentOU.FullCode + "." + ouInfo.Code;
            }
            else
            {
                if (string.IsNullOrEmpty(ouInfo.UnitFullName))
                {
                    ouInfo.FullName = ouInfo.Name;
                    ouInfo.FullCode = ouInfo.Code;
                }
                else
                {
                    ouInfo.FullName = ouInfo.UnitFullName + "." + ouInfo.Name;
                    ouInfo.FullCode = ouInfo.UnitFullCode + "." + ouInfo.Code;
                }
            }

            // 检查是否在数据中唯一
            if (!this.ouInfoDao.IsUnique(ouInfo))
                throw new ResourceException("OUINFO.EXCEPTION.HAD_EXIST", new string[] { ouInfo.Name, ouInfo.Code });
            this.ouInfoDao.Save(ouInfo);
        }

        public override void Save(IList ouInfos)
        {
            foreach (OUInfo ouInfo in ouInfos)
            {
                this.Save(ouInfo);
            }
        }

        public void ParseSave(User userInfo, OUInfo ouInfo)
        {
            this.ouInfoDao.Save(ouInfo);
        }

        public OUInfo FindOUInfoByOldImportDataID(string oldImportDataID)
        {
            return this.ouInfoDao.FindOUInfoByOldImportDataID(oldImportDataID);
        }

        #endregion

        #region IOUInfoService 成员


        public string GetAllOldImportDataID()
        {
            return this.ouInfoDao.GetAllOldImportDataID();
        }

        #endregion

        #region IOUInfoService 成员


        public void DeleteByOldImportDataID(string oldImportDataID)
        {
            OUInfo oUInfo = this.FindOUInfoByOldImportDataID(oldImportDataID);
            if (oUInfo == null)
                return;
            oUInfo.OUStatus = OUStatuses.Delete;
            this.ouInfoDao.Save(oUInfo);
        }

        #endregion

        #region IOUInfoService 成员


        public IList FindAll()
        {
            return this.ouInfoDao.FindAll();
        }

        public IList FindChilds(string parentUnid, string ouType)
        {
            return this.ouInfoDao.FindChildByParentUnid(parentUnid, ouType);
        }

        public IList FindChilds(string parentUnid, string ouType, bool includeChildOU)
        {
            return this.ouInfoDao.FindChildByParentUnid(parentUnid, ouType, includeChildOU);
        }

        public IList FindChilds(string parentUnid)
        {
            return this.ouInfoDao.FindChildByParentUnid(parentUnid, null);
        }

        public IList FindDepartmentByUnit(string parentUnid, bool firstLevel)
        {
            return this.ouInfoDao.FindDepartmentByUnit(parentUnid, firstLevel);
        }

        public IList FindAll_OrgSync()
        {
            return this.ouInfoDao.FindAll_OrgSync();
        }

        #endregion

        #region 删除方法复写，只改状态不实际删除

        public override void Delete(OUInfo ouInfo)
        {
            if (null == ouInfo)
                return;
            ouInfo.OUStatus = OUStatuses.Delete;
            this.ouInfoDao.Save(ouInfo);
        }

        public override void Delete(long id)
        {
            OUInfo ou = this.ouInfoDao.Load(id);
            this.Delete(ou);
        }

        public override void Delete(string unid)
        {
            OUInfo ou = this.ouInfoDao.Load(unid);
            this.Delete(ou);
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
            foreach (OUInfo obj in objs)
            {
                this.Delete(obj);
            }
        }

        #endregion
    }
}
