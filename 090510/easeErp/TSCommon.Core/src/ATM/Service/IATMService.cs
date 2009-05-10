using System;
using System.Collections.Generic;
using System.Text;
using TSLib.Service;
using TSLib;
using System.Collections;

namespace TSCommon.Core.ATM.Service
{
    public interface IATMService:IBaseService<ATM.Domain.ATM>
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
        void DeleteAll(IList<ATM.Domain.ATM> list);

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
        /// 将指定的附件信息拷贝到另外一个指定文档中
        /// </summary>
        /// <param name="sourceParentUnid">原附件信息所属文件的Unid</param>
        /// <param name="newParentUnid">新文档的Unid</param>
        /// <param name="type">所要拷贝成的类型</param>
        void Copy(string sourceParentUnid, string newParentUnid, string type);

        /// <summary>
        /// 将指定文档的附件信息拷贝到另外一个指定文档中
        /// </summary>
        /// <param name="sourceParentUnid">原附件信息所属文件的Unid</param>
        /// <param name="sourceType">所需要拷贝文件的原类型</param>
        /// <param name="newParentUnid">新文档的Unid</param>
        /// <param name="destType">所要拷贝成的类型</param>
        void CopyAll(string sourceParentUnid, string sourceType, string newParentUnid, string destType);

        /// <summary>
        /// 将指定附件的parentUnid替换成新的Unid
        /// </summary>
        /// <param name="sourceParentUnid">旧的Unid</param>
        /// <param name="newParentUnid">新的Unid</param>
        void SwapParentUnid(string sourceParentUnid, string newParentUnid);

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
        PageInfo GetPage(int pageNo, int pageSize, string sortField, string sortDir, string parent, string type);
    }
}
