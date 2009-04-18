using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Dao;
using TSLib;
using System.Collections;

namespace TSCommon_Core.ATM.Dao
{
    public interface IATMDao : IBaseDao<ATM.Domain.ATM>
    {
        /// <summary>
        /// 通过父文档的Unid得到所有的附件信息
        /// </summary>
        /// <param name="parentUnid">父文档的Unid</param>
        /// <returns>附件信息</returns>
        IList GetATM(string parentUnid);

        /// <summary>
        /// 通过父文档的Unid和相应的附件类别得到所有的附件信息
        /// </summary>
        /// <param name="parentUnid">父文档的Unid</param>
        /// <param name="type">附件类别</param>
        /// <returns>附件信息</returns>
        IList GetATM(string parentUnid, string type);

        /// <summary>
        /// 删除附件信息集合
        /// </summary>
        /// <param name="list"></param>
        void DeleteAll(IList list);

        /// <summary>
        /// 删除父文档的Unid得到所有的附件
        /// </summary>
        /// <param name="parentUnid"></param>
        void DeleteAll(string parentUnid);

        /// <summary>
        /// 删除父文档的Unid得到所有的指定类型的附件
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="type"></param>
        void DeleteAll(string parent, string type);

        /// <summary>
        ///  通过父文档的Unid和相应的附件类别得到所有的附件信息
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDir"></param>
        /// <param name="parent"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        PageInfo GetPage(int pageNo, int pageSize, string sortField, string sortDir,string parent, string type);
    }
}
